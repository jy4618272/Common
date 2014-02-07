using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Security;

namespace Micajah.Common.Application
{
    /// <summary>
    /// The base class for applications.
    /// Defines the methods, properties, and events that are common to all application objects within an ASP.NET application.
    /// </summary>
    public class WebApplication : HttpApplication
    {
        #region Members

        private static LoginProvider s_LoginProvider;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when an email is sending.
        /// </summary>
        public static event EventHandler<EmailSendingEventArgs> EmailSending;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the hosts adresses of the current web application.
        /// </summary>
        public static ReadOnlyCollection<string> Hosts
        {
            get
            {
                HttpRequest request = HttpContext.Current.Request;
                List<string> list = new List<string>();
                list.Add(string.Concat(request.Url.Scheme, Uri.SchemeDelimiter, request.UserHostAddress));
                list.Add(string.Concat(request.Url.Scheme, Uri.SchemeDelimiter, request.Url.Host));
                list.Add(string.Concat(request.Url.Scheme, Uri.SchemeDelimiter, request.UserHostAddress, ':', request.Url.Port));
                list.Add(string.Concat(request.Url.Scheme, Uri.SchemeDelimiter, request.Url.Host, ':', request.Url.Port));
                return list.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the application's virtual application root path on the server.
        /// </summary>
        public static string RootPath
        {
            get
            {
                return ((HttpContext.Current.Request.ApplicationPath == "/")
                    ? string.Empty : HttpContext.Current.Request.ApplicationPath);
            }
        }

        /// <summary>
        /// Gets or sets the login provider.
        /// </summary>
        public static LoginProvider LoginProvider
        {
            get
            {
                if (s_LoginProvider == null) s_LoginProvider = new LoginProvider();
                return s_LoginProvider;
            }
            set { s_LoginProvider = value; }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Raises the EmailSending event.
        /// </summary>
        /// <param name="e">An EmailSendingEventArgs that contains the event data.</param>
        internal static void RaiseEmailSending(EmailSendingEventArgs e)
        {
            if (EmailSending != null)
                EmailSending(null, e);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Registers a new ResourceVirtualPathProvider instance with the ASP.NET compilation system.
        /// </summary>
        private static void RegisterResourceVirtualPathProvider()
        {
            MethodInfo mi = typeof(HostingEnvironment).GetMethod("RegisterVirtualPathProviderInternal", (BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod));
            if (mi != null)
                mi.Invoke(null, new object[] { new ResourceVirtualPathProvider() });
            else
                HostingEnvironment.RegisterVirtualPathProvider(new ResourceVirtualPathProvider());
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises when a security module has established the identity of the user.
        /// </summary>
        /// <param name="sender">The sourceRow of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            string pageUrl = Request.AppRelativeCurrentExecutionFilePath;

            if (ResourceProvider.IsResourceUrl(pageUrl))
            {
                Context.SkipAuthorization = true;
            }
            else if (ActionProvider.IsPublicPage(pageUrl))
            {
                if ((!FrameworkConfiguration.Current.WebApplication.Password.EnablePasswordRetrieval) &&
                    (string.Compare(pageUrl, ResourceProvider.PasswordRecoveryPageVirtualPath, StringComparison.OrdinalIgnoreCase) == 0))
                    throw new HttpException(404, Resources.Error_404);
                else
                {
                    Micajah.Common.Bll.Action action = ActionProvider.FindAction(CustomUrlProvider.CreateApplicationAbsoluteUrl(Request.Url.PathAndQuery));
                    if (action != null)
                        Context.SkipAuthorization = (!action.AuthenticationRequired);
                    else
                        Context.SkipAuthorization = true;

                    switch (FrameworkConfiguration.Current.WebApplication.AuthenticationMode)
                    {
                        case AuthenticationMode.Forms:
                            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
                            if (authCookie == null)
                            {
                                FormsIdentity id = new FormsIdentity(new FormsAuthenticationTicket(string.Empty, false, FrameworkConfiguration.Current.WebApplication.Login.Timeout));
                                GenericPrincipal principal = new GenericPrincipal(id, null);
                                Context.User = principal;
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Raises when an application is started.
        /// </summary>
        /// <param name="sender">The sourceRow of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void Application_Start(object sender, EventArgs e)
        {
            RegisterResourceVirtualPathProvider();
            Resources.ResourceManager.IgnoreCase = true;

            if (Micajah.Common.Pages.PageStatePersister.IsInUse && FrameworkConfiguration.Current.WebApplication.ViewState.DeleteExpiredViewState)
                ViewStateProvider.DeleteExpiredViewState();
        }

        /// <summary>
        /// Raises when an application is ended.
        /// </summary>
        /// <param name="sender">The sourceRow of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void Application_End(object sender, EventArgs e)
        {
            if (Micajah.Common.Pages.PageStatePersister.IsInUse && FrameworkConfiguration.Current.WebApplication.ViewState.DeleteExpiredViewState)
                ViewStateProvider.DeleteExpiredViewState();
        }

        /// <summary>
        /// Raises when an error occured in an application.
        /// </summary>
        /// <param name="sender">The sourceRow of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void Application_Error(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Occurs as the first event in the HTTP pipeline chain of execution when ASP.NET responds to a request.
        /// </summary>
        /// <param name="sender">The sourceRow of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Occurs when a security module has established the identity of the user.
        /// </summary>
        /// <param name="sender">The sourceRow of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Occurs when the request state (for example, session state) that is associated with the current request has been obtained.
        /// </summary>
        /// <param name="sender">The sourceRow of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void Application_PostAcquireRequestState(object sender, EventArgs e)
        {
            HttpContext http = HttpContext.Current;

            if (http == null) return;
            if (http.Session == null) return;

            UserContext user = null;
            Micajah.Common.Bll.Action action = null;

            CustomUrlElement customUrlSettings = FrameworkConfiguration.Current.WebApplication.CustomUrl;
            string host = http.Request.Url.Host;
            bool isDefaultPartialCustomUrl = false;

            if (customUrlSettings.Enabled)
                isDefaultPartialCustomUrl = CustomUrlProvider.IsDefaultVanityUrl(host);

            if (!isDefaultPartialCustomUrl)
            {
                if (http.Session.IsNewSession)
                {
                    user = UserContext.Current;
                    if (user != null)
                        LoginProvider.UpdateSession(user.UserId, http.Session.SessionID);
                }

                if (!http.SkipAuthorization)
                {
                    if (user == null)
                        user = UserContext.Current;

                    if (user == null)
                    {
                        action = ActionProvider.FindAction(CustomUrlProvider.CreateApplicationAbsoluteUrl(Request.Url.PathAndQuery));
                        if (action != null)
                        {
                            if (action.AuthenticationRequired)
                                LoginProvider.SignOut(true, Request.Url.PathAndQuery);
                        }
                    }
                    else if (!customUrlSettings.Enabled)
                    {
                        if (!LoginProvider.ValidateSession(user.UserId, http.Session.SessionID))
                            LoginProvider.SignOut(true, true, true);
                    }
                }
            }

            if (!customUrlSettings.Enabled)
                return;

            if (!((http.User != null) && (http.User.Identity != null) && http.User.Identity.IsAuthenticated))
                return;

            string redirectUrl = string.Empty;
            Guid organizationId = Guid.Empty;
            Guid instanceId = Guid.Empty;

            if (http.Session.IsNewSession || (string.Compare(host, UserContext.VanityUrl, StringComparison.OrdinalIgnoreCase) != 0))
            {
                if (isDefaultPartialCustomUrl)
                    return;

                string vanityUrl = null;
                bool setAuthCookie = true;

                CustomUrlProvider.ParseHost(host, ref organizationId, ref instanceId);

                if (organizationId == Guid.Empty)
                {
                    Guid userId = Guid.Empty;
                    LoginProvider.ParseUserIdentityName(out userId, out organizationId, out instanceId);

                    if (userId != Guid.Empty)
                    {
                        setAuthCookie = false;
                        vanityUrl = CustomUrlProvider.GetVanityUrl(organizationId, instanceId);
                    }
                }
                else
                {
                    vanityUrl = host;
                    UserContext.VanityUrl = host;
                }

                if (string.IsNullOrEmpty(vanityUrl))
                {
                    if (!isDefaultPartialCustomUrl)
                    {
                        http.Session.Abandon(); // Important fix of the issue with the same SessionID for all the child domains.

                        redirectUrl = CustomUrlProvider.CreateApplicationUri(http.Request.Url.PathAndQuery);
                    }
                }
                else
                {
                    if (string.Compare(host, vanityUrl, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        if (user == null)
                            user = UserContext.Current;

                        if (user != null)
                        {
                            try
                            {
                                if (user.OrganizationId != organizationId)
                                {
                                    user.SelectOrganization(organizationId, setAuthCookie, null, null);
                                    user.SelectInstance(instanceId, setAuthCookie, null);
                                }
                                else if (user.InstanceId != instanceId)
                                    user.SelectInstance(instanceId, setAuthCookie, null);
                            }
                            catch (AuthenticationException)
                            {
                                redirectUrl = LoginProvider.GetLoginUrl(null, null, Guid.Empty, Guid.Empty, null, CustomUrlProvider.CreateApplicationUri(host, null));
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(redirectUrl))
                    {
                        if (string.Compare(host, customUrlSettings.PartialCustomUrlRootAddressesFirst, StringComparison.OrdinalIgnoreCase) == 0)
                            http.Session.Abandon(); // Important fix of the issue with the same SessionID for all the child domains.

                        redirectUrl = CustomUrlProvider.CreateApplicationUri(vanityUrl, http.Request.Url.PathAndQuery);
                    }
                }

            }
            else
            {
                if (user == null)
                    user = UserContext.Current;

                if (user != null)
                {
                    CustomUrlProvider.ParseHost(host, ref organizationId, ref instanceId);

                    if (user.OrganizationId != Guid.Empty)
                    {
                        if (user.OrganizationId != organizationId)
                            redirectUrl = LoginProvider.GetLoginUrl(null, null, organizationId, instanceId, null);
                        else
                        {
                            if (instanceId == Guid.Empty)
                            {
                                if (user.InstanceId != Guid.Empty)
                                {
                                    try
                                    {
                                        user.SelectOrganization(organizationId, true, null, null);
                                    }
                                    catch (AuthenticationException)
                                    {
                                        redirectUrl = LoginProvider.GetLoginUrl(null, null, organizationId, Guid.Empty, null);
                                    }
                                }
                            }
                            else if (user.InstanceId != instanceId)
                                redirectUrl = LoginProvider.GetLoginUrl(null, null, organizationId, instanceId, null);
                        }
                    }
                    else if (organizationId != Guid.Empty)
                        redirectUrl = LoginProvider.GetLoginUrl(Guid.Empty, organizationId);
                }
            }

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                if ((redirectUrl.IndexOf(http.Request.Url.ToString(), StringComparison.OrdinalIgnoreCase) == -1)
                    && (http.Request.Url.ToString().IndexOf(redirectUrl, StringComparison.OrdinalIgnoreCase) == -1))
                {
                    http.Response.Redirect(redirectUrl);
                }
            }
        }

        /// <summary>
        /// Raises when a session is started.
        /// </summary>
        /// <param name="sender">The sourceRow of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void Session_Start(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Raises when a session is abandoned or expires.
        /// </summary>
        /// <param name="sender">The sourceRow of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void Session_End(object sender, EventArgs e)
        {
            if (Micajah.Common.Pages.PageStatePersister.IsInUse && FrameworkConfiguration.Current.WebApplication.ViewState.DeleteExpiredViewState)
                ViewStateProvider.DeleteExpiredViewState();
        }

        #endregion
    }

    /// <summary>
    /// The class containing the data for the Micajah.Common.Application.WebApplication.EmailSending event.
    /// </summary>
    public class EmailSendingEventArgs : EventArgs
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the e-mail message.
        /// </summary>
        public MailMessage MailMessage { get; set; }

        /// <summary>
        /// Gets or sets whether the email should be sent and the calling thread will not blocked.
        /// </summary>
        public bool Async { get; set; }

        /// <summary>
        /// Gets or sets whether the sending should be cancelled.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets or sets by what reason an email is sending.
        /// </summary>
        public EmailSendingReason Reason { get; set; }

        /// <summary>
        /// Gets or sets the organization.
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user password.
        /// </summary>
        public string Password { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents the different reasons for email sending.
    /// </summary>
    public enum EmailSendingReason
    {
        Undefined,
        SignupOrganization,
        ResetPassword,
        CreateNewLogin,
        ChangeLogin,
        ChangePassword,
        InviteUser
    }
}
