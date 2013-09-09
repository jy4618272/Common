using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Dal.TableAdapters;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Principal;
using System.Threading;
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

        private static ClientDataSetTableAdapters s_OrganizationDataSetTableAdapters;
        private static SortedList s_OrganizationDataSetTableAdaptersList;
        private static LoginProvider s_LoginProvider;

        // The objects which are used to synchronize access to the cached objects.
        private static object s_CommonDataSetSyncRoot = new object();
        private static object s_EntitiesSyncRoot = new object();
        private static object s_RulesEnginesSyncRoot = new object();
        private static object s_StartThreadsSyncRoot = new object();
        private static object s_OrganizationDataSetSyncRoot = new object();
        private static object s_OrganizationDataSetTableAdaptersListSyncRoot = new object();

        #endregion

        #region Events

        /// <summary>
        /// Occurs when an email is sending.
        /// </summary>
        public static event EventHandler<EmailSendingEventArgs> EmailSending;

        #endregion

        #region Private Properties

        private static List<string> OrganizationDataSetsKeys
        {
            get
            {
                List<string> list = CacheManager.Current.Get("mc.OrganizationDataSetsKeys") as List<string>;
                if (list == null)
                {
                    list = new List<string>();
                    CacheManager.Current.AddWithDefaultExpiration("mc.OrganizationDataSetsKeys", list);
                }
                return list;
            }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets the instance of the Micajah.Common.Dal.CommonDataSet class that contains common data of application.
        /// </summary>
        internal static CommonDataSet CommonDataSet
        {
            get
            {
                CommonDataSet ds = null;
                try
                {
                    ds = CacheManager.Current.Get("mc.CommonDataSet") as CommonDataSet;
                    if (ds == null)
                    {
                        lock (s_CommonDataSetSyncRoot)
                        {
                            ds = CacheManager.Current.Get("mc.CommonDataSet") as CommonDataSet;
                            if (ds == null)
                            {
                                ds = new CommonDataSet();
                                MasterDataSetTableAdapters.Current.Fill(ds);

                                CacheManager.Current.AddWithDefaultExpiration("mc.CommonDataSet", ds);
                            }
                        }
                    }
                    return ds;
                }
                finally
                {
                    if (ds != null) ds.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets the identifier of the web site, which the current web application is beign run.
        /// Zero, If the web site isn't found or is deleted.
        /// </summary>
        internal static Guid WebsiteId
        {
            get
            {
                CommonDataSet.WebsiteRow row = WebsiteProvider.GetWebsiteRowByUrl(Hosts.ToArray());
                return ((row != null) ? row.WebsiteId : Guid.Empty);
            }
        }

        /// <summary>
        /// Gets the hosts adresses of the current web application.
        /// </summary>
        internal static List<string> Hosts
        {
            get
            {
                HttpRequest request = HttpContext.Current.Request;
                List<string> list = new List<string>();
                list.Add(string.Concat(request.Url.Scheme, Uri.SchemeDelimiter, request.UserHostAddress));
                list.Add(string.Concat(request.Url.Scheme, Uri.SchemeDelimiter, request.Url.Host));
                list.Add(string.Concat(request.Url.Scheme, Uri.SchemeDelimiter, request.UserHostAddress, ':', request.Url.Port));
                list.Add(string.Concat(request.Url.Scheme, Uri.SchemeDelimiter, request.Url.Host, ':', request.Url.Port));
                return list;
            }
        }

        #endregion

        #region Public Properties

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

        public static EntityCollection Entities
        {
            get
            {
                EntityCollection coll = CacheManager.Current.Get("mc.Entities") as EntityCollection;
                if (coll == null)
                {
                    lock (s_EntitiesSyncRoot)
                    {
                        coll = CacheManager.Current.Get("mc.Entities") as EntityCollection;
                        if (coll == null)
                        {
                            coll = EntityCollection.Load();
                            CacheManager.Current.AddWithDefaultExpiration("mc.Entities", coll);
                        }
                    }
                }
                return coll;
            }
        }

        public static RulesEngineCollection RulesEngines
        {
            get
            {
                RulesEngineCollection coll = CacheManager.Current.Get("mc.RulesEngines") as RulesEngineCollection;
                if (coll == null)
                {
                    lock (s_RulesEnginesSyncRoot)
                    {
                        coll = CacheManager.Current.Get("mc.RulesEngines") as RulesEngineCollection;
                        if (coll == null)
                        {
                            coll = RulesEngineCollection.Load();
                            CacheManager.Current.AddWithDefaultExpiration("mc.RulesEngines", coll);
                        }
                    }
                }
                return coll;
            }
        }

        public static StartThreadCollection StartThreads
        {
            get
            {
                StartThreadCollection coll = CacheManager.Current.Get("mc.StartThreads") as StartThreadCollection;
                if (coll == null)
                {
                    lock (s_StartThreadsSyncRoot)
                    {
                        coll = CacheManager.Current.Get("mc.StartThreads") as StartThreadCollection;
                        if (coll == null)
                        {
                            coll = StartThreadCollection.Load();
                            CacheManager.Current.AddWithDefaultExpiration("mc.StartThreads", coll);
                        }
                    }
                }
                return coll;
            }
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

        /// <summary>
        /// Refreshes all cached data.
        /// </summary>
        internal static void RefreshAllData()
        {
            RefreshAllData(true);
        }

        /// <summary>
        /// Refreshes all cached data.
        /// </summary>
        /// <param name="refreshUserContext">Whether the current user context should be refreshed.</param>
        internal static void RefreshAllData(bool refreshUserContext)
        {
            RefreshCommonData();
            RefreshOrganizationDataSets();
            if (refreshUserContext)
                UserContext.RefreshCurrent();
        }

        /// <summary>
        /// Refreshes the cached data set of specified organization and the cached data of the current user.
        /// </summary>
        /// <param name="organizationId">The organization's identifier to refresh.</param>
        internal static void RefreshOrganizationData(Guid organizationId)
        {
            RefreshOrganizationDataSetByOrganizationId(organizationId);
            UserContext.RefreshCurrent();
        }

        /// <summary>
        /// Refreshes the cached common data set and related data stored in collections.
        /// </summary>
        internal static void RefreshCommonData()
        {
            WebApplicationElement.CurrentDatabaseVersion = 0;
            RefreshCommonDataSet(false);
            RefreshCollections();
        }

        /// <summary>
        /// Refreshes the cached common data set.
        /// </summary>
        internal static void RefreshCommonDataSet(bool organizationOnly)
        {
            if (organizationOnly)
            {
                CommonDataSet ds = CacheManager.Current.Get("mc.CommonDataSet") as CommonDataSet;
                if (ds != null)
                {
                    lock (s_CommonDataSetSyncRoot)
                    {
                        MasterDataSetTableAdapters.Current.OrganizationTableAdapter.Fill(ds.Organization);

                        CacheManager.Current.AddWithDefaultExpiration("mc.CommonDataSet", ds);
                    }
                }
            }
            else
            {
                lock (s_CommonDataSetSyncRoot)
                {
                    CacheManager.Current.Remove("mc.CommonDataSet");
                }
            }
        }

        /// <summary>
        /// Refreshes the cached collections of the actions, global navigation links and settings.
        /// </summary>
        internal static void RefreshCollections()
        {
            ActionProvider.Refresh();
            SettingProvider.Refresh();
            RefreshEntities();
            RefreshRulesEngines();
            RefreshStartThreads();
        }

        internal static void RefreshEntities()
        {
            lock (s_EntitiesSyncRoot)
            {
                CacheManager.Current.Remove("mc.Entities");
            }
        }

        internal static void RefreshRulesEngines()
        {
            lock (s_RulesEnginesSyncRoot)
            {
                CacheManager.Current.Remove("mc.RulesEngines");
            }
        }

        internal static void RefreshStartThreads()
        {
            lock (s_StartThreadsSyncRoot)
            {
                CacheManager.Current.Remove("mc.StartThreads");
            }
        }

        /// <summary>
        /// Refreshes the cached organization data sets.
        /// </summary>
        internal static void RefreshOrganizationDataSets()
        {
            lock (s_OrganizationDataSetSyncRoot)
            {
                foreach (string key in OrganizationDataSetsKeys)
                {
                    CacheManager.Current.Remove(key);
                }

                OrganizationDataSetsKeys.Clear();
            }
        }

        internal static void RefreshOrganizationDataSetTableAdaptersList()
        {
            lock (s_OrganizationDataSetTableAdaptersListSyncRoot)
            {
                s_OrganizationDataSetTableAdaptersList = null;
            }
        }

        /// <summary>
        /// Deletes the cached data set of specified organization.
        /// </summary>
        /// <param name="organizationId">The organization's identifier.</param>
        internal static void RemoveOrganizationDataSetByOrganizationId(Guid organizationId)
        {
            lock (s_OrganizationDataSetSyncRoot)
            {
                string key = "mc.OrganizationDataSet." + organizationId.ToString("N");

                CacheManager.Current.Remove(key);

                OrganizationDataSetsKeys.Remove(key);
            }
        }

        /// <summary>
        /// Refreshes the cached data set of specified organization.
        /// </summary>
        /// <param name="organizationId">The organization's identifier to refresh.</param>
        internal static void RefreshOrganizationDataSetByOrganizationId(Guid organizationId)
        {
            RemoveOrganizationDataSetByOrganizationId(organizationId);
        }

        /// <summary>
        /// Returns the instance of the OrganizationDataSet class that contains data of specified organization.
        /// </summary>
        /// <param name="organizationId">The organization's identifier to get data set.</param>
        /// <returns>The instance of the OrganizationDataSet class that contains data of specified organization.</returns>
        internal static OrganizationDataSet GetOrganizationDataSetByOrganizationId(Guid organizationId)
        {
            OrganizationDataSet ds = null;
            try
            {
                string key = "mc.OrganizationDataSet." + organizationId.ToString("N");
                ds = CacheManager.Current.Get(key) as OrganizationDataSet;
                if (ds == null)
                {
                    lock (s_OrganizationDataSetSyncRoot)
                    {
                        ds = CacheManager.Current.Get(key) as OrganizationDataSet;
                        if (ds == null)
                        {
                            ds = new OrganizationDataSet();
                            ClientDataSetTableAdapters adapters = GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
                            adapters.Fill(ds, organizationId);

                            CacheManager.Current.AddWithDefaultExpiration(key, ds);

                            OrganizationDataSetsKeys.Add(key);
                        }
                    }
                }
                return ds;
            }
            finally
            {
                if (ds != null) ds.Dispose();
            }
        }

        /// <summary>
        /// Returns the instance of the ClientDataSetTableAdapters class that contains 
        /// tables adapters and connection to database of specified organization.
        /// </summary>
        /// <param name="organizationId">The organization's identifier.</param>
        /// <returns>
        /// The instance of the ClientDataSetTableAdapters class that contains 
        /// tables adapters and connection to database of specified organization.
        /// </returns>
        internal static ClientDataSetTableAdapters GetOrganizationDataSetTableAdaptersByOrganizationId(Guid organizationId)
        {
            return GetOrganizationDataSetTableAdaptersByConnectionString(OrganizationProvider.GetConnectionString(organizationId));
        }

        /// <summary>
        /// Returns the instance of the ClientDataSetTableAdapters class that contains 
        /// tables adapters and specified connection to database.
        /// </summary>
        /// <param name="connectionString">The connection string to organization's database.</param>
        /// <returns>
        /// The instance of the ClientDataSetTableAdapters class that contains 
        /// tables adapters and specified connection to database.
        /// </returns>
        internal static ClientDataSetTableAdapters GetOrganizationDataSetTableAdaptersByConnectionString(string connectionString)
        {
            ClientDataSetTableAdapters adapters = null;

            if (s_OrganizationDataSetTableAdaptersList == null) s_OrganizationDataSetTableAdaptersList = new SortedList();

            if (!s_OrganizationDataSetTableAdaptersList.ContainsKey(connectionString))
            {
                lock (s_OrganizationDataSetTableAdaptersListSyncRoot)
                {
                    if (!s_OrganizationDataSetTableAdaptersList.ContainsKey(connectionString))
                    {
                        adapters = ClientDataSetTableAdapters.Current.Clone();
                        adapters.ConnectionString = connectionString;
                        s_OrganizationDataSetTableAdaptersList.Add(connectionString, adapters);
                    }
                }
            }
            else adapters = s_OrganizationDataSetTableAdaptersList[connectionString] as ClientDataSetTableAdapters;

            return adapters;
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

        /// <summary>
        /// Starts all start thead and rerun it.
        /// </summary>
        private void MonitorStartThreadLoop()
        {
            StartThreadCollection startThreads = WebApplication.StartThreads;
            if (startThreads.Count > 0)
            {
                foreach (StartThread startThread in startThreads)
                {
                    DateTime now = DateTime.UtcNow;
                    int startHour = 0;
                    int startMinute = 0;
                    DateTime startDate = DateTime.UtcNow;
                    string[] startTime = startThread.StartTime.Split(':');

                    if (startTime.Length == 2)
                    {
                        startHour = startTime[0].StartsWith("0", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt32(startTime[0].Substring(1, startTime[0].Length - 1), CultureInfo.InvariantCulture) : Convert.ToInt32(startTime[0], CultureInfo.InvariantCulture);
                        startMinute = Convert.ToInt32(startTime[1], CultureInfo.InvariantCulture);

                        if ((now.Hour > startHour) || ((now.Hour == startHour) && (now.Minute >= startMinute)))
                            startDate = DateTime.UtcNow.Date.AddDays(1).AddHours(startHour);
                        else
                            startDate = DateTime.UtcNow.Date.AddHours(startHour);

                        startDate = startDate.AddMinutes(startMinute);

                        StartThreadTimer timer = new StartThreadTimer();
                        timer.StartThread = startThread;
                        timer.Elapsed += new System.Timers.ElapsedEventHandler(StartThreadTimer_Elapsed);
                        timer.Interval = startDate.Subtract(now).TotalMilliseconds;
                        timer.AutoReset = true;
                        timer.Enabled = true;
                        timer.Start();
                    }
                }
            }
        }

        private void StartThreadTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            StartThreadTimer timer = sender as StartThreadTimer;
            if (timer != null)
            {
                IThreadStateProvider threadProvider = timer.StartThread.GetStartThreadClassInstance();
                Thread workerThread = new Thread(threadProvider.Start);
                workerThread.CurrentCulture = CultureInfo.CurrentCulture;
                workerThread.CurrentUICulture = CultureInfo.CurrentUICulture;
                workerThread.Priority = ThreadPriority.Lowest;
                workerThread.Start();

                timer.Interval = 24 * 60 * 60 * 1000; //24 hours
            }
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

            if (Micajah.Common.Pages.PageStatePersister.IsInUse)
                ViewStateProvider.DeleteExpiredViewState();

            Thread MonitorThread = new Thread(MonitorStartThreadLoop);
            MonitorThread.CurrentCulture = CultureInfo.CurrentCulture;
            MonitorThread.CurrentUICulture = CultureInfo.CurrentUICulture;
            MonitorThread.Priority = ThreadPriority.Lowest;
            MonitorThread.Start();
        }

        /// <summary>
        /// Raises when an application is ended.
        /// </summary>
        /// <param name="sender">The sourceRow of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void Application_End(object sender, EventArgs e)
        {
            if (Micajah.Common.Pages.PageStatePersister.IsInUse)
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
                    else if (!LoginProvider.ValidateSession(user.UserId, http.Session.SessionID))
                    {
                        if (!customUrlSettings.Enabled)
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
                                user.SelectOrganization(organizationId, setAuthCookie, null, null);
                                user.SelectInstance(instanceId, setAuthCookie, null);
                            }
                            catch (AuthenticationException)
                            {
                                redirectUrl = LoginProvider.GetLoginUrl(null, null, Guid.Empty, Guid.Empty, false, null, CustomUrlProvider.CreateApplicationUri(host, null));
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

                    if (user.SelectedOrganizationId != Guid.Empty)
                    {
                        if (user.SelectedOrganizationId != organizationId)
                            redirectUrl = LoginProvider.GetLoginUrl(null, null, organizationId, instanceId, false, null);
                        else
                        {
                            if (instanceId == Guid.Empty)
                            {
                                try
                                {
                                    user.SelectOrganization(organizationId, true, null, null);
                                }
                                catch (AuthenticationException)
                                {
                                    redirectUrl = LoginProvider.GetLoginUrl(null, null, organizationId, Guid.Empty, false, null);
                                }
                            }
                            else if (user.SelectedInstanceId != instanceId)
                            {
                                redirectUrl = LoginProvider.GetLoginUrl(null, null, organizationId, instanceId, false, null);
                            }
                            else
                            {
                                try
                                {
                                    user.SelectInstance(instanceId, true, null);
                                }
                                catch (AuthenticationException)
                                {
                                    redirectUrl = LoginProvider.GetLoginUrl(null, null, organizationId, instanceId, false, null);
                                }
                            }
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
            if (Micajah.Common.Pages.PageStatePersister.IsInUse)
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
