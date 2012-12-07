using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using Micajah.Common.Application;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.LdapAdapter;
using Micajah.Common.Properties;
using Micajah.Common.Security;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with logins.
    /// </summary>
    [DataObjectAttribute(true)]
    public class LoginProvider
    {
        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether the cookie that contains the forms-authentication ticket information is persistent.
        /// </summary>
        public static bool FormsAuthenticationTicketIsPersistent
        {
            get
            {
                bool isPersistent = true;
                HttpContext context = HttpContext.Current;
                if ((context != null) && (context.Request != null))
                {
                    HttpCookie cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
                    if (cookie != null)
                    {
                        if (!string.IsNullOrEmpty(cookie.Value))
                        {
                            try
                            {
                                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                                isPersistent = ticket.IsPersistent;
                            }
                            catch (HttpException) { }
                        }
                    }
                }
                return isPersistent;
            }
        }

        #endregion

        #region Private Methods

        private static void ClearAuthCookie()
        {
            HttpContext http = HttpContext.Current;
            if (http != null)
            {
                FormsAuthentication.SignOut();

                if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
                {
                    // Expire all the cookies so browser visits us as a brand new user.
                    List<string> cookiesToClear = new List<string>();
                    foreach (string cookieName in http.Request.Cookies)
                    {
                        HttpCookie cookie = http.Request.Cookies[cookieName];
                        cookiesToClear.Add(cookie.Name);
                    }

                    foreach (string name in cookiesToClear)
                    {
                        HttpCookie cookie = new HttpCookie(name, string.Empty);
                        cookie.Domain = FrameworkConfiguration.Current.WebApplication.CustomUrl.AuthenticationTicketDomain;
                        cookie.Expires = DateTime.Today.AddYears(-1);

                        http.Response.Cookies.Set(cookie);
                    }
                }
            }
        }

        private static bool LoginNameIsInList(string loginName, StringCollection logins)
        {
            bool result = false;
            if (logins != null)
            {
                foreach (string value in logins)
                {
                    if (string.Compare(value, loginName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        private static DataTable GetOrganizationsLogins(Guid loginId, Guid organizationId)
        {
            if ((loginId != Guid.Empty) && (organizationId != Guid.Empty))
            {
                SqlConnection connection = null;
                SqlCommand command = null;

                try
                {
                    connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString);

                    command = new SqlCommand("dbo.Mc_GetOrganizationLogin", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@OrganizationId", SqlDbType.UniqueIdentifier).Value = organizationId;
                    command.Parameters.Add("@LoginId", SqlDbType.UniqueIdentifier).Value = loginId;

                    return Support.GetDataTable(command);
                }
                finally
                {
                    if (connection != null) connection.Dispose();
                    if (command != null) command.Dispose();
                }
            }
            return null;
        }

        private static DataRow GetOrganizationsLoginsDataRow(Guid loginId, Guid organizationId)
        {
            DataTable table = GetOrganizationsLogins(loginId, organizationId);
            if (table != null)
            {
                if (table.Rows.Count > 0) return table.Rows[0];
            }
            return null;
        }

        /// <summary>
        /// Authenticates the user in ldap with specified login name and password.
        /// </summary>
        /// <param name="loginName">The name for the login.</param>
        /// <param name="password">The password for the new login.</param>
        /// <param name="details">An System.Object array containing zero or more objects that represents the optional details for authentication.</param>
        /// <param name="usePasswordEncryption">true to use encryption for password before compare login details; otherwise, false.</param>
        /// <returns>DataRowView with the user information</returns>
        private DataRowView LdapAuthenticate(string loginName, string password, bool usePasswordEncryption, Guid organiationId)
        {
            DataRowView drv = null;
            LdapProvider server = null;
            Guid orgId = Guid.Empty;
            Organization org = null;
            Response<AuthenticationStatus> isAuth = null;
            User user = null;

            ApplicationLogger log = new ApplicationLogger();
            LdapIntegration ldi = new LdapIntegration(log);
            List<IUser> users = FindLocalUsers(loginName);

            if (users.Count == 1)
                user = (User)users[0];
            else if (users.Count > 1)
                user = (User)GetUserFromUsersList(users, password, usePasswordEncryption);

            if (user != null)
            {
                string hashedPassword = password;
                if (usePasswordEncryption) hashedPassword = WebApplication.LoginProvider.EncryptPassword(password);

                if (user.Password == hashedPassword)
                    return GetLogin(user.LocalLoginId);
                else
                {
                    orgId = user.OrganizationId;
                    org = OrganizationProvider.GetOrganization(orgId);
                    if (org != null)
                    {
                        if (String.IsNullOrEmpty(org.LdapServerAddress) == true || String.IsNullOrEmpty(org.LdapServerPort) == true || String.IsNullOrEmpty(org.LdapUserName) == true || String.IsNullOrEmpty(org.LdapPassword) == true || String.IsNullOrEmpty(org.LdapDomain) == true || !org.Beta)
                            return drv;
                        else
                        {
                            server = new LdapProvider(orgId, ldi, log);
                            isAuth = server.CheckUserLDAPPasswordAndActive(user, password);
                            if (isAuth.ResponseValue == AuthenticationStatus.NotAuthenticated)
                            {
                                if (isAuth.ShowResponse && (!string.IsNullOrEmpty(isAuth.ResponseMessage)))
                                {
                                    if (isAuth.ResponseMessage == "LdapConnectionProblems")
                                        throw new AuthenticationException(Resources.LoginProvider_ErrorMessage_LDAPConnectionError);
                                    else
                                        throw new AuthenticationException(isAuth.ResponseMessage);
                                }
                                throw new AuthenticationException(FrameworkConfiguration.Current.WebApplication.Login.FailureText);
                            }
                        }

                        return GetLogin(user.LocalLoginId);
                    }
                }
            }

            //Get Organization Id from authentication details
            if (organiationId != Guid.Empty)
                orgId = organiationId;

            if (orgId == Guid.Empty)
            {
                // Get Organization Id by Email Suffix
                if (!string.IsNullOrEmpty(loginName) && loginName.Contains("@") && EmailSuffixProvider.IsEmailSuffixExist())
                {
                    string[] parts = loginName.Split('@');
                    DataTable table = EmailSuffixProvider.GetEmailSuffixes(null, null, parts[1]);

                    if (table.Rows.Count > 0)
                        orgId = (Guid)table.Rows[0]["OrganizationId"];

                    // Get Organization Id by Ldap Domain
                    if (orgId == Guid.Empty)
                    {
                        table = GetOrganizationsByLdapDomain(parts[1]);

                        if (table.Rows.Count > 0)
                            orgId = (Guid)table.Rows[0]["OrganizationId"];
                    }
                }
            }

            if (orgId == Guid.Empty)
                return drv;

            org = OrganizationProvider.GetOrganization(orgId);
            if (org != null)
            {
                if (String.IsNullOrEmpty(org.LdapServerAddress) == true || String.IsNullOrEmpty(org.LdapServerPort) == true || String.IsNullOrEmpty(org.LdapUserName) == true || String.IsNullOrEmpty(org.LdapPassword) == true || String.IsNullOrEmpty(org.LdapDomain) == true || !org.Beta)
                    return drv;
                else
                {
                    server = new LdapProvider(orgId, ldi, log);
                    if (!server.Initialize())
                        throw new AuthenticationException(Resources.LoginProvider_ErrorMessage_LDAPConnectionError);
                }
            }
            else
                return drv;

            isAuth = server.AuthenticateUser(loginName, password, usePasswordEncryption, orgId);
            if (isAuth.ResponseValue == AuthenticationStatus.NotAuthenticated)
            {
                if (isAuth.ShowResponse && (!string.IsNullOrEmpty(isAuth.ResponseMessage)))
                    throw new AuthenticationException(isAuth.ResponseMessage);

                throw new AuthenticationException(FrameworkConfiguration.Current.WebApplication.Login.FailureText);
            }

            if (isAuth.LoginId != Guid.Empty)
                drv = GetLogin(isAuth.LoginId);
            else
                drv = GetLogin(loginName);

            return drv;
        }

        private List<IUser> FindLocalUsers(string userName)
        {
            List<IUser> users = new List<IUser>();
            bool isDomainSearhed = false;
            string domainName = null;
            string userAlias = null;
            string[] parts = null;
            ApplicationLogger log = new ApplicationLogger();
            LdapIntegration ldi = new LdapIntegration(log);

            try
            {
                if (userName.Contains("@"))
                {
                    // check email and domain login
                    users = (List<IUser>)ldi.GetUsersByEmail(userName);
                    if (users.Count == 0)
                    {
                        domainName = userName.Split('@')[1];
                        userAlias = userName.Split('@')[0];
                        if (string.IsNullOrEmpty(domainName) == false && string.IsNullOrEmpty(userAlias) == false)
                            users = (List<IUser>)ldi.GetUsersByDomainLogin(domainName, userAlias, (userAlias.Contains(".")) ? userAlias.Split('.')[0] : null, (userAlias.Contains(".")) ? userAlias.Split('.')[1] : null);
                        isDomainSearhed = true;
                    }
                }
                if (users.Count == 0 && userName.Contains("\\"))
                {
                    // check domain login and email
                    if (isDomainSearhed == false)
                    {
                        domainName = userName.Split('\\')[0];
                        userAlias = userName.Split('\\')[1];
                        if (string.IsNullOrEmpty(domainName) == false && string.IsNullOrEmpty(userAlias) == false)
                            users = (List<IUser>)ldi.GetUsersByDomainLogin(domainName, userAlias, (userAlias.Contains(".")) ? userAlias.Split('.')[0] : null, (userAlias.Contains(".")) ? userAlias.Split('.')[1] : null);
                        isDomainSearhed = true;
                    }
                    /*if (users.Count == 0)
                    {
                        parts = userName.Split('\\');
                        if (string.IsNullOrEmpty(parts[0]) == false && string.IsNullOrEmpty(parts[1]) == false)
                            users = ApplicationProvider.GetUsersByEmail(parts[1] + '@' + parts[0]);
                    }*/
                }
                if (users.Count == 0 && isDomainSearhed == false && userName.Contains("."))
                {
                    // check user alias and principal without domain
                    parts = userName.Split('.');
                    if (string.IsNullOrEmpty(parts[0]) == false && string.IsNullOrEmpty(parts[1]) == false)
                        users = (List<IUser>)ldi.GetUsersByDomainLogin(null, userName, parts[0], parts[1]);
                }
                if (users.Count == 0 && userName.Contains(","))
                {
                    // check user alias and principal without domain
                    parts = userName.Split(',');
                    if (string.IsNullOrEmpty(parts[0].Trim()) == false && string.IsNullOrEmpty(parts[1].Trim()) == false)
                        users = (List<IUser>)ldi.GetUsersByDomainLogin(null, userName, parts[1].Trim(), parts[0].Trim());
                }
                if (users.Count == 0 && userName.Contains(" "))
                {
                    // check user alias and principal without domain
                    parts = userName.Split(' ');
                    if (string.IsNullOrEmpty(parts[0].Trim()) == false && string.IsNullOrEmpty(parts[1].Trim()) == false)
                        users = (List<IUser>)ldi.GetUsersByDomainLogin(null, userName, parts[0].Trim(), parts[1].Trim());
                }
                if (users.Count == 0)
                {
                    // check user alias without domain
                    users = (List<IUser>)ldi.GetUsersByDomainLogin(null, userName, null, null);
                }

                return users;
            }
            finally
            {
                if (ldi != null) ldi.Dispose();
            }
        }

        private IUser GetUserFromUsersList(List<IUser> users, string password, bool usePasswordEncryption)
        {
            IUser user = null;
            string hashedPassword = password;
            if (usePasswordEncryption) hashedPassword = WebApplication.LoginProvider.EncryptPassword(password);

            Guid orgID = Guid.Empty;
            string ldapServer = "";
            int ldapPort = 0;
            int orgLdapDetailsCount = 0;

            // Check simple password
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Password == hashedPassword)
                    return (IUser)users[i];
            }

            // Simple password compare is fail
            // Check if organizations have different ldap details
            foreach (IUser u in users)
            {
                if ((u.OrganizationId != Guid.Empty) && (u.OrganizationId != orgID) && (String.IsNullOrEmpty(u.LdapServerAddress) == false) && (String.IsNullOrEmpty(u.LdapServerPort.ToString(CultureInfo.CurrentCulture)) == false) && (String.IsNullOrEmpty(u.LdapDomain) == false) && (String.IsNullOrEmpty(u.LdapServerUserName) == false) && (ldapServer != u.LdapServerAddress))
                {
                    orgID = u.OrganizationId;
                    ldapServer = u.LdapServerAddress;
                    ldapPort = u.LdapServerPort;
                    user = u;
                    orgLdapDetailsCount++;
                }
                else if (ldapPort != u.LdapServerPort && !string.IsNullOrEmpty(u.LdapServerAddress))
                {
                    orgID = u.OrganizationId;
                    ldapServer = u.LdapServerAddress;
                    ldapPort = u.LdapServerPort;
                    user = u;
                    orgLdapDetailsCount++;
                }
            }

            // Several organizations have ldap details - fail
            if (orgLdapDetailsCount != 1)
                return null;

            return user;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Returns the specified invitation.
        /// </summary>
        /// <param name="invitedLoginId">The unique identifier of the invitation.</param>
        /// <returns>The specified invitation.</returns>
        internal static CommonDataSet.InvitedLoginDataTable GetInvitedLogin(Guid invitedLoginId)
        {
            CommonDataSet.InvitedLoginDataTable table = null;
            try
            {
                table = new CommonDataSet.InvitedLoginDataTable();
                WebApplication.CommonDataSetTableAdapters.InvitedLoginTableAdapter.Fill(table, 0, invitedLoginId);
                return table;
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        internal string GetLoginUrl(string loginName, string password, Guid organizationId, Guid instanceId, bool newOrg, string returnUrl, string applicationUrl)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(loginName))
                sb.AppendFormat(CultureInfo.InvariantCulture, "&l={0}", HttpUtility.UrlEncodeUnicode(loginName));

            if (!string.IsNullOrEmpty(password))
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "&p={0}", HttpUtility.UrlEncodeUnicode(Support.Encrypt(password)));

                bool isPersistent = true;
                if (FrameworkConfiguration.Current.WebApplication.AuthenticationMode == AuthenticationMode.Forms)
                    isPersistent = FormsAuthenticationTicketIsPersistent;
                if (isPersistent)
                    sb.Append("&cp=true");
            }

            if (organizationId != Guid.Empty)
                sb.AppendFormat(CultureInfo.InvariantCulture, "&o={0:N}", organizationId);

            if (instanceId != Guid.Empty)
                sb.AppendFormat(CultureInfo.InvariantCulture, "&d={0:N}", instanceId);

            if (organizationId != Guid.Empty)
            {
                if (newOrg)
                    sb.Append("&on=true");
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                sb.Append("&returnurl=");
                sb.Append(HttpUtility.UrlEncodeUnicode(returnUrl));
            }

            if (applicationUrl == null)
                applicationUrl = string.Empty;

            return applicationUrl.TrimEnd('/') + CustomUrlProvider.CreateApplicationRelativeUrl(GetLoginUrl(false)) + ((sb.Length > 0) ? "?" + sb.ToString().TrimStart('&') : string.Empty);
        }

        internal string GetLoginUrl(string loginName, string password, Guid organizationId, Guid instanceId, bool newOrg, string returnUrl)
        {
            return GetLoginUrl(loginName, password, Guid.Empty, Guid.Empty, newOrg, returnUrl, CustomUrlProvider.GetVanityUri(organizationId, instanceId));
        }

        /// <summary>
        /// Returns the specified reset password request.
        /// </summary>
        /// <param name="resetPasswordRequestId">The unique identifier of the reset password request.</param>
        /// <returns>The specified reset password request.</returns>
        internal static CommonDataSet.ResetPasswordRequestDataTable GetResetPasswordRequest(Guid resetPasswordRequestId)
        {
            CommonDataSet.ResetPasswordRequestDataTable table = null;
            try
            {
                table = new CommonDataSet.ResetPasswordRequestDataTable();
                WebApplication.CommonDataSetTableAdapters.ResetPasswordRequestTableAdapter.Fill(table, 0, resetPasswordRequestId);
                return table;
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        /// <summary>
        /// Deletes the expired invitations.
        /// </summary>
        internal void DeleteExpiredInvitation()
        {
            this.CancelInvitation(Guid.Empty);
        }

        /// <summary>
        /// Deletes the expired reset password requests.
        /// </summary>
        internal void DeleteExpiredResetPasswordRequests()
        {
            this.CancelResetPasswordRequest(Guid.Empty);
        }

        /// <summary>
        /// Returns a value indicating whether the user is administrator of the framework.
        /// </summary>
        /// <param name="loginName">The login name of the user to check.</param>
        /// <returns>true, if the user is administrator of the framework; otherwise, false.</returns>
        internal static bool IsFrameworkAdministrator(string loginName)
        {
            return LoginNameIsInList(loginName, FrameworkConfiguration.Current.WebApplication.FrameworkAdministrators);
        }

        /// <summary>
        /// Returns a value indicating whether the user has the access to the Log On As Another User feature.
        /// </summary>
        /// <param name="loginName">The login name of the user to check.</param>
        /// <returns>true, if the user has the access to the Log On As Another User feature; otherwise, false.</returns>
        internal static bool CanLogOnAsUser(string loginName)
        {
            bool result = IsFrameworkAdministrator(loginName);
            if (!result)
            {
                result = LoginNameIsInList(loginName, FrameworkConfiguration.Current.WebApplication.CanLogOnAsAnotherUser);
            }
            return result;
        }

        internal static void ParseAuthCookie(out Guid userId, out Guid organizationId, out Guid instanceId)
        {
            userId = Guid.Empty;
            organizationId = Guid.Empty;
            instanceId = Guid.Empty;

            HttpContext ctx = HttpContext.Current;
            if (ctx != null)
            {
                string[] parts = ctx.User.Identity.Name.Split(',');
                object obj = Support.ConvertStringToType(parts[0], typeof(Guid));

                if (obj != null)
                    userId = (Guid)obj;

                if (parts.Length > 1)
                {
                    obj = Support.ConvertStringToType(parts[1], typeof(Guid));
                    if (obj != null)
                        organizationId = (Guid)obj;

                    if (parts.Length > 2)
                    {
                        obj = Support.ConvertStringToType(parts[2], typeof(Guid));
                        if (obj != null)
                            instanceId = (Guid)obj;
                    }
                }
            }
        }

        internal static void SetAuthCookie(Guid userId, Guid organizationId, Guid instanceId, bool? isPersistent)
        {
            string userName = string.Format(CultureInfo.InvariantCulture, "{0:N},{1:N},{2:N}", userId, organizationId, instanceId);

            if (!isPersistent.HasValue)
                isPersistent = FormsAuthenticationTicketIsPersistent;

            if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
            {
                HttpCookie authcookie = FormsAuthentication.GetAuthCookie(userName, isPersistent.Value);
                authcookie.Domain = FrameworkConfiguration.Current.WebApplication.CustomUrl.AuthenticationTicketDomain;
                HttpContext.Current.Response.AppendCookie(authcookie);
            }
            else
                FormsAuthentication.SetAuthCookie(userName, isPersistent.Value);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the specified login to specified organization.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to add.</param>
        /// <param name="organizationId">The organization identifier to add to.</param>
        /// <param name="organizationAdministrator">true, if the user is organization administrator; otherwise, false.</param>
        public virtual void AddLoginToOrganization(Guid loginId, Guid organizationId, bool organizationAdministrator)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString);

                command = new SqlCommand("dbo.Mc_InsertOrganizationLogin", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@OrganizationId", SqlDbType.UniqueIdentifier).Value = organizationId;
                command.Parameters.Add("@LoginId", SqlDbType.UniqueIdentifier).Value = loginId;
                command.Parameters.Add("@OrganizationAdministrator", SqlDbType.Bit).Value = organizationAdministrator;
                command.Parameters.Add("@Active", SqlDbType.Bit).Value = true;

                Support.ExecuteNonQuery(command);
            }
            finally
            {
                if (connection != null) connection.Dispose();
                if (command != null) command.Dispose();
            }
        }

        /// <summary>
        /// Authenticates the user with specified login name and password.
        /// </summary>
        /// <param name="loginName">The name for the login.</param>
        /// <param name="password">The password for the new login.</param>
        /// <param name="details">An System.Object array containing zero or more objects that represents the optional details for authentication.</param>
        /// <param name="usePasswordEncryption">true to use encryption for password before compare login details; otherwise, false.</param>
        /// <returns>true, if the user is successfully authenticated; otherwise, false.</returns>
        public virtual bool Authenticate(string loginName, string password, bool usePasswordEncryption, params object[] details)
        {
            DataRowView drv = GetLogin(loginName, password, usePasswordEncryption);

            Guid organizationId = Guid.Empty;
            Guid instanceId = Guid.Empty;
            bool isPersistent = true;

            if (details != null)
            {
                if (details.Length > 0)
                {
                    if (details[0] == null)
                        isPersistent = FormsAuthenticationTicketIsPersistent;
                    else
                        isPersistent = (bool)details[0];

                    if (details.Length > 1)
                    {
                        if (details[1] != null)
                            organizationId = (Guid)details[1];

                        if (details.Length > 2)
                        {
                            if (details[2] != null)
                                instanceId = (Guid)details[2];
                        }
                    }
                }
            }

            if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
            {
                if (organizationId == Guid.Empty)
                {
                    HttpContext http = HttpContext.Current;
                    if (http != null)
                    {
                        string host = http.Request.Url.Host;
                        if (!CustomUrlProvider.IsDefaultVanityUrl(host))
                            CustomUrlProvider.ParseHost(host, ref organizationId, ref instanceId);
                    }
                }
            }

            if ((drv == null) && FrameworkConfiguration.Current.WebApplication.EnableLdap)
                drv = LdapAuthenticate(loginName, password, usePasswordEncryption, organizationId);

            if (drv == null)
                throw new AuthenticationException(FrameworkConfiguration.Current.WebApplication.Login.FailureText);
            else if (Convert.ToBoolean(drv.Row["Deleted"], CultureInfo.CurrentCulture))
                throw new AuthenticationException(Resources.UserContext_ErrorMessage_YourAccountIsDeleted);

            Guid loginId = (Guid)drv["LoginId"];

            UserContext user = new UserContext();
            user.Initialize(loginId, organizationId, instanceId, true, isPersistent);

            UserContext.Current = user;

            if (HttpContext.Current.Session != null)
                UpdateSession(loginId, HttpContext.Current.Session.SessionID);

            return true;
        }

        /// <summary>
        /// Signs out the current user.
        /// </summary>
        public void SignOut()
        {
            this.SignOut(true);
        }

        /// <summary>
        /// Signs out the current user.
        /// </summary>
        /// <param name="removeAuthInfo">true to remove the authentication information from the browser.</param>
        public void SignOut(bool removeAuthInfo)
        {
            this.SignOut(removeAuthInfo, true);
        }

        /// <summary>
        /// Signs out the current user.
        /// </summary>
        /// <param name="removeAuthInfo">true to remove the authentication information from the browser.</param>
        /// <param name="redirectToLogOnPage">true to redirect the browser to the login URL.</param>
        public void SignOut(bool removeAuthInfo, bool redirectToLogOnPage)
        {
            SignOut(removeAuthInfo, redirectToLogOnPage, false);
        }

        /// <summary>
        /// Signs out the current user.
        /// </summary>
        /// <param name="removeAuthInfo">true to remove the authentication information from the browser.</param>
        /// <param name="redirectToLogOnPage">true to redirect the browser to the login URL.</param>
        /// <param name="sessionIsNotValid">true, if the session is not valid (when an user is logged from another computer).</param>
        public void SignOut(bool removeAuthInfo, bool redirectToLogOnPage, bool sessionIsNotValid)
        {
            if (redirectToLogOnPage)
                SignOut(removeAuthInfo, GetLoginUrl(false) + (sessionIsNotValid ? "?ac=1" : string.Empty));
            else
                SignOut(removeAuthInfo, null);
        }

        /// <summary>
        /// Signs out the current user.
        /// </summary>
        /// <param name="redirectUrl">The URL to navigate.</param>
        public void SignOut(string redirectUrl)
        {
            SignOut(true, redirectUrl);
        }

        /// <summary>
        /// Signs out the current user.
        /// </summary>
        /// <param name="removeAuthInfo">true to remove the authentication information from the browser.</param>
        /// <param name="redirectUrl">The URL to navigate.</param>
        public virtual void SignOut(bool removeAuthInfo, string redirectUrl)
        {
            HttpContext http = HttpContext.Current;
            if (http == null) return;

            if (http.Session != null)
                http.Session.Clear();

            if (FrameworkConfiguration.Current.WebApplication.AuthenticationMode == AuthenticationMode.Forms)
            {
                if (removeAuthInfo)
                    ClearAuthCookie();
            }

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                http.Response.Clear();

                Uri uri = null;
                if (!Uri.TryCreate(redirectUrl, UriKind.Absolute, out uri))
                {
                    redirectUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(redirectUrl);

                    if ((redirectUrl.IndexOf(http.Request.Url.ToString(), StringComparison.OrdinalIgnoreCase) > -1)
                        || (http.Request.Url.ToString().IndexOf(redirectUrl, StringComparison.OrdinalIgnoreCase) > -1)) // Checks if the current and redirect URLs are not the same.
                    {
                        string loginUrl = GetLoginUrl(false);
                        if (string.Compare(CustomUrlProvider.CreateApplicationAbsoluteUrl(loginUrl), redirectUrl, StringComparison.OrdinalIgnoreCase) == 0) // Checks if the current page is not login page.
                            redirectUrl = loginUrl;
                        else
                            redirectUrl = loginUrl + "?returnurl=" + HttpUtility.UrlEncodeUnicode(redirectUrl);
                    }
                }

                http.Response.Redirect(redirectUrl);
            }
        }

        /// <summary>
        /// Cancels the specified invitation.
        /// </summary>
        /// <param name="invitedLoginId">The unique identifier of the invitation.</param>
        public virtual void CancelInvitation(Guid invitedLoginId)
        {
            WebApplication.CommonDataSetTableAdapters.InvitedLoginTableAdapter.Delete((invitedLoginId == Guid.Empty) ? (object)DBNull.Value : (object)invitedLoginId);
        }

        /// <summary>
        /// Cancels the specified reset password request.
        /// </summary>
        /// <param name="resetPasswordRequestId">The unique identifier of the reset password request.</param>
        public virtual void CancelResetPasswordRequest(Guid resetPasswordRequestId)
        {
            WebApplication.CommonDataSetTableAdapters.ResetPasswordRequestTableAdapter.Delete((resetPasswordRequestId == Guid.Empty) ? (object)DBNull.Value : (object)resetPasswordRequestId);
        }

        /// <summary>
        /// Processes a request to update the login name and send an e-mail notification, if the login name was updated successfully.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to update.</param>
        /// <param name="loginName">The new login name.</param>
        /// <returns>true, if the login name was updated successfully; otherwise, false.</returns>
        public bool ChangeLoginName(Guid loginId, string loginName)
        {
            return ChangeLoginName(loginId, loginName, true);
        }

        /// <summary>
        /// Processes a request to update the login name.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to update.</param>
        /// <param name="loginName">The new login name.</param>
        /// <param name="sendEmailNotification">The flag specifying that the e-mail notification will be sent, if the login name was updated successfully.</param>
        /// <returns>true if the login name was updated successfully; otherwise, false.</returns>
        public virtual bool ChangeLoginName(Guid loginId, string loginName, bool sendEmailNotification)
        {
            bool success = false;
            if (string.IsNullOrEmpty(loginName)) return false;

            string originalLoginName = GetLoginName(loginId);
            if (string.Compare(loginName, originalLoginName, StringComparison.OrdinalIgnoreCase) != 0)
            {
                if (LoginNameExists(loginName))
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.LoginProvider_ErrorMessage_LoginAlreadyExists, loginName, originalLoginName));
                else
                {
                    success = UpdateLogin(loginId, loginName);
                    if (sendEmailNotification)
                    {
                        UserContext user = UserContext.Current;
                        UserProvider.SendChangeLoginEmail(GetEmail(loginId), ((user == null) ? Guid.Empty : user.SelectedOrganizationId));
                    }
                }
            }
            return success;
        }

        /// <summary>
        /// Processes a request to update the password for specified login and send an e-mail notification, if the password was updated successfully.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to update.</param>
        /// <param name="password">The new password for the specified login.</param>
        /// <returns>true, if the password was updated successfully; otherwise, false.</returns>
        public bool ChangePassword(Guid loginId, string password)
        {
            return ChangePassword(loginId, password, true);
        }

        /// <summary>
        /// Processes a request to update the password for specified login.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to update.</param>
        /// <param name="password">The new password for the specified login.</param>
        /// <param name="sendEmailNotification">The flag specifying that the e-mail notification will be sent, if the password was updated successfully.</param>
        /// <returns>true if the password was updated successfully; otherwise, false.</returns>
        public bool ChangePassword(Guid loginId, string password, bool sendEmailNotification)
        {
            return ChangePassword(loginId, password, sendEmailNotification, true);
        }

        /// <summary>
        /// Processes a request to update the password for specified login.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to update.</param>
        /// <param name="password">The new password for the specified login.</param>
        /// <param name="sendEmailNotification">The flag specifying that the e-mail notification will be sent, if the password was updated successfully.</param>
        /// <param name="validatePassword">true to validate the password; otherwise, false.</param>
        /// <returns>true if the password was updated successfully; otherwise, false.</returns>
        public virtual bool ChangePassword(Guid loginId, string password, bool sendEmailNotification, bool validatePassword)
        {
            if (validatePassword)
                ValidatePassword(password);
            else if (password == null)
                return false;

            if (UpdateLogin(loginId, null, EncryptPassword(password)))
            {
                if (sendEmailNotification && GetEmail(loginId) != "")
                {
                    UserContext user = UserContext.Current;
                    UserProvider.SendChangePasswordEmail(GetEmail(loginId), password, ((user == null) ? Guid.Empty : user.SelectedOrganizationId));
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes a request to update the password for specified login and send an e-mail notification, if the password was updated successfully.
        /// </summary>
        /// <param name="loginName">The name for the login to update.</param>
        /// <param name="password">The new password for the specified login.</param>
        /// <returns>true, if the password was updated successfully; otherwise, false.</returns>
        public bool ChangePassword(string loginName, string password)
        {
            return ChangePassword(loginName, password, true);
        }

        /// <summary>
        /// Processes a request to update the password for specified login.
        /// </summary>
        /// <param name="loginName">The name for the login to update.</param>
        /// <param name="password">The new password for the specified login.</param>
        /// <param name="sendEmailNotification">The flag specifying that the e-mail notification will be sent, if the password was updated successfully.</param>
        /// <returns>true if the password was updated successfully; otherwise, false.</returns>
        public bool ChangePassword(string loginName, string password, bool sendEmailNotification)
        {
            return ChangePassword(loginName, password, sendEmailNotification, true);
        }

        /// <summary>
        /// Processes a request to update the password for specified login.
        /// </summary>
        /// <param name="loginName">The name for the login to update.</param>
        /// <param name="password">The new password for the specified login.</param>
        /// <param name="sendEmailNotification">The flag specifying that the e-mail notification will be sent, if the password was updated successfully.</param>
        /// <param name="validatePassword">true to validate the password; otherwise, false.</param>
        /// <returns>true if the password was updated successfully; otherwise, false.</returns>
        public bool ChangePassword(string loginName, string password, bool sendEmailNotification, bool validatePassword)
        {
            return ChangePassword(GetLoginId(loginName), password, sendEmailNotification, validatePassword);
        }

        /// <summary>
        /// Adds a new login to the data source. 
        /// </summary>
        /// <param name="loginName">The name for the new login.</param>
        /// <param name="password">The password for the new login.</param>
        /// <param name="details">An System.Object array containing zero or more objects that represents the details for the new login.</param>
        /// <returns>An object populated with the information for the newly created login.</returns>
        public virtual DataRow CreateLogin(string loginName, string password, params object[] details)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString);

                command = new SqlCommand("[dbo].[Mc_InsertLogin]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@LoginId", SqlDbType.UniqueIdentifier, 16).Value = Guid.NewGuid();
                command.Parameters.Add("@LoginName", SqlDbType.NVarChar, 255).Value = loginName;
                command.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = EncryptPassword(password);
                command.Parameters.Add("@Token", SqlDbType.VarChar, 50).Value = Support.GeneratePseudoUnique(32);

                return Support.GetDataRow(command);
            }
            finally
            {
                if (connection != null) connection.Dispose();
                if (command != null) command.Dispose();
            }
        }

        /// <summary>
        /// Removes a login from the data source. 
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to delete.</param>
        /// <returns>true if the user was successfully deleted; otherwise, false.</returns>
        public virtual bool DeleteLogin(Guid loginId)
        {
            return UpdateLogin(loginId, null, null, null, null, true);
        }

        /// <summary>
        /// Returns the encrypted password string.
        /// </summary>
        /// <param name="password">Specifies the password string.</param>
        /// <returns>The encrypted password string.</returns>
        public string EncryptPassword(string password)
        {
            return EncryptPassword(password, FrameworkConfiguration.Current.WebApplication.Password.Format);
        }

        /// <summary>
        /// Returns the encrypted password string.
        /// </summary>
        /// <param name="password">Specifies the password string.</param>
        /// <param name="passwordFormat">Specifies the cryptographic algorithm for encrypting password string.</param>
        /// <returns>The encrypted password string.</returns>
        public virtual string EncryptPassword(string password, CryptoMethod passwordFormat)
        {
            string pwd = password;
            if (password != null)
            {
                switch (passwordFormat)
                {
                    case CryptoMethod.Sha1:
                        UTF8Encoding encoder = new UTF8Encoding();
                        using (SHA1 sha = new SHA1Managed())
                        {
                            byte[] bytes = sha.ComputeHash(encoder.GetBytes(password));
                            pwd = Convert.ToBase64String(bytes);
                        }
                        break;
                }
            }
            return pwd;
        }

        /// <summary>
        /// Returns the invitations to the specified organization.
        /// </summary>
        /// <param name="organizationId">The unique identifier for the organization to get invitations for.</param>
        /// <returns>The invitations to the specified organization.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public CommonDataSet.InvitedLoginDataTable GetInvitedLoginsByOrganizationId(Guid organizationId)
        {
            CommonDataSet.InvitedLoginDataTable table = null;
            try
            {
                table = new CommonDataSet.InvitedLoginDataTable();
                WebApplication.CommonDataSetTableAdapters.InvitedLoginTableAdapter.Fill(table, 1, organizationId);
                return table;
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        /// <summary>
        /// Gets a list of all the logins in the data source.
        /// </summary>
        /// <returns>A list that contains all the logins.</returns>
        public virtual DataTable GetLogins()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the organization's instances which the specified user have access to.
        /// </summary>
        /// <param name="loginId">The unique identifier of the login.</param>
        /// <param name="organizationId">The unique identifier for the organization.</param>
        /// <returns>A Micajah.Common.Bll.InstanceCollection object that contains the instances.</returns>
        public virtual InstanceCollection GetLoginInstances(Guid loginId, Guid organizationId)
        {
            return InstanceProvider.GetUserInstances(loginId, organizationId, LoginIsOrganizationAdministrator(loginId, organizationId));
        }

        /// <summary>
        /// Gets a collection of the login's organizations.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to get organizations for.</param>
        /// <returns>A Micajah.Common.Bll.OrganizationCollection object that contains the login's organizations.</returns>
        public virtual OrganizationCollection GetOrganizationsByLoginId(Guid loginId)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString);

                command = new SqlCommand("dbo.Mc_GetOrganizationsByLoginId", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@LoginId", SqlDbType.UniqueIdentifier).Value = loginId;

                return OrganizationProvider.CreateOrganizationCollection(Support.GetDataTable(command));
            }
            finally
            {
                if (connection != null) connection.Dispose();
                if (command != null) command.Dispose();
            }
        }

        /// <summary>
        /// Gets a collection of the login's organizations.
        /// </summary>
        /// <param name="loginName">The name for the login to get organizations for.</param>
        /// <returns>A Micajah.Common.Bll.OrganizationCollection object that contains the login's organizations.</returns>
        public virtual OrganizationCollection GetOrganizationsByLoginName(string loginName)
        {
            Guid loginId = Guid.Empty;
            DataRowView drv = GetLogin(loginName);
            if (drv != null) loginId = (Guid)drv["LoginId"];
            if (loginId != Guid.Empty)
                return GetOrganizationsByLoginId(loginId);
            else
                return new OrganizationCollection();
        }

        /// <summary>
        /// Gets a list of the organizations.
        /// </summary>
        /// <param name="ldapDomain">The ldap domain to get organizations for.</param>
        /// <returns>A list that contains the ldap domain's organizations.</returns>
        public virtual DataTable GetOrganizationsByLdapDomain(string ldapDomain)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString);

                command = new SqlCommand("dbo.Mc_GetOrganizationsByLdapDomain", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@LdapDomain", SqlDbType.NVarChar).Value = ldapDomain;

                return Support.GetDataTable(command);
            }
            finally
            {
                if (connection != null) connection.Dispose();
                if (command != null) command.Dispose();
            }
        }

        /// <summary>
        /// Gets a list of the logins that belong to specified organization.
        /// </summary>
        /// <param name="organizationId">The unique identifier for the organization to get logins for.</param>
        /// <returns>A list of the logins that belong to specified organization.</returns>
        public virtual DataTable GetLoginsByOrganizationId(Guid organizationId)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString);

                command = new SqlCommand("dbo.Mc_GetLoginsByOrganizationId", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@OrganizationId", SqlDbType.UniqueIdentifier).Value = organizationId;

                return Support.GetDataTable(command);
            }
            finally
            {
                if (connection != null) connection.Dispose();
                if (command != null) command.Dispose();
            }
        }

        /// <summary>
        /// Gets the e-mail address for the specified login from the data source.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to retrieve the e-mail for.</param>
        /// <returns>The e-mail address for the specified login.</returns>
        public virtual string GetEmail(Guid loginId)
        {
            return GetLoginName(loginId);
        }

        /// <summary>
        /// Gets information from the data source for a login based on the unique identifier for the login.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to get information for.</param>
        /// <returns>An object populated with the specified login's information from the data source.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public virtual DataRowView GetLogin(Guid loginId)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString);

                command = new SqlCommand("dbo.Mc_GetLogin", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@LoginId", SqlDbType.UniqueIdentifier).Value = loginId;

                return Support.GetDataRowView(command);
            }
            finally
            {
                if (connection != null) connection.Dispose();
                if (command != null) command.Dispose();
            }
        }

        /// <summary>
        /// Gets information from the data source based on the name for the login.
        /// </summary>
        /// <param name="loginName">The name for the login to get information for.</param>
        /// <returns>An object populated with the specified login's information from the data source.</returns>
        public virtual DataRowView GetLogin(string loginName)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString);

                command = new SqlCommand("dbo.Mc_GetLoginByLoginName", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@LoginName", SqlDbType.NVarChar, 255).Value = loginName;

                return Support.GetDataRowView(command);
            }
            finally
            {
                if (connection != null) connection.Dispose();
                if (command != null) command.Dispose();
            }
        }

        /// <summary>
        /// Gets information from the data source for a login based on the unique identifier and password for the login.
        /// </summary>
        /// <param name="loginName">The name for the login to get information for.</param>
        /// <param name="password">The password.</param>
        /// <returns>An object populated with the specified login's information from the data source.</returns>
        public virtual DataRowView GetLogin(string loginName, string password)
        {
            return GetLogin(loginName, password, true);
        }

        /// <summary>
        /// Gets information from the data source for a login based on the unique identifier and password for the login.
        /// </summary>
        /// <param name="loginName">The name for the login to get information for.</param>
        /// <param name="password">The password.</param>
        /// <param name="usePasswordEncryption">true to use encryption for password before compare login details; otherwise, false.</param>
        /// <returns>An object populated with the specified login's information from the data source.</returns>
        public virtual DataRowView GetLogin(string loginName, string password, bool usePasswordEncryption)
        {
            DataRowView drv = GetLogin(loginName);
            if (drv != null)
            {
                if (password != null)
                {
                    if (string.Compare((string)drv["Password"], ((usePasswordEncryption && (password != null)) ? EncryptPassword(password) : password), StringComparison.Ordinal) != 0)
                        drv = null;
                }
            }
            return drv;
        }

        /// <summary>
        /// Gets information from the data source based on the token for the login.
        /// </summary>
        /// <param name="token">The token for the login to get information for.</param>
        /// <returns>An object populated with the specified login's information from the data source.</returns>
        public virtual DataRowView GetLoginByToken(string token)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString);

                command = new SqlCommand("dbo.Mc_GetLoginByToken", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Token", SqlDbType.VarChar, 50).Value = token;

                return Support.GetDataRowView(command);
            }
            finally
            {
                if (connection != null) connection.Dispose();
                if (command != null) command.Dispose();
            }
        }

        /// <summary>
        /// Gets the login identifier for the specified login name from the data source.
        /// </summary>
        /// <param name="loginName">The name for the login to get information for.</param>
        /// <returns>The login identifier for the specified login name.</returns>
        public virtual Guid GetLoginId(string loginName)
        {
            DataRowView drv = GetLogin(loginName);
            if (drv != null) return (Guid)drv["LoginId"];
            return Guid.Empty;
        }

        /// <summary>
        /// Gets the login name for the specified login from the data source.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to get information for.</param>
        /// <returns>The login name for the specified login.</returns>
        public virtual string GetLoginName(Guid loginId)
        {
            DataRowView drv = GetLogin(loginId);
            if (drv != null) return drv["LoginName"].ToString();
            return string.Empty;
        }

        /// <summary>
        /// Gets the identifier of the last session for the specified login from the data source.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to get information for.</param>
        /// <returns>The identifier of the last session for the specified login.</returns>
        public virtual string GetSession(Guid loginId)
        {
            DataRowView drv = GetLogin(loginId);
            if (drv != null)
            {
                if (drv.Row.Table.Columns.Contains("SessionId") && (!Support.IsNullOrDBNull(drv["SessionId"])))
                    return (string)drv["SessionId"];
            }
            return null;
        }

        /// <summary>
        /// Gets the token of the specified login from the data source.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to get information for.</param>
        /// <returns>The token of the specified login.</returns>
        public virtual string GetToken(Guid loginId)
        {
            DataRowView drv = GetLogin(loginId);
            if (drv != null)
            {
                if (!Support.IsNullOrDBNull(drv["Token"]))
                    return (string)drv["Token"];
            }
            return null;
        }

        /// <summary>
        /// Gets the token of the specified login from the data source.
        /// </summary>
        /// <param name="loginName">The name for the login to get information for.</param>
        /// <returns>The token of the specified login.</returns>
        public virtual string GetToken(string loginName)
        {
            DataRowView drv = GetLogin(loginName);
            if (drv != null)
            {
                if (!Support.IsNullOrDBNull(drv["Token"]))
                    return (string)drv["Token"];
            }
            return null;
        }

        /// <summary>
        /// Returns the URL for the login page.
        /// </summary>
        /// <returns>The URL for the login page.</returns>
        public string GetLoginUrl()
        {
            return GetLoginUrl(null, null, Guid.Empty, Guid.Empty, false, null);
        }

        /// <summary>
        /// Returns the URL for the login page.
        /// </summary>
        /// <param name="absoluteUri">Whether the specified URL should be converted to the URI, if it is possible.</param>
        /// <returns>he URL for the login page.</returns>
        public virtual string GetLoginUrl(bool absoluteUri)
        {
            if (absoluteUri)
                return GetLoginUrl(null, null, Guid.Empty, Guid.Empty, false, null);

            return ((FrameworkConfiguration.Current.WebApplication.AuthenticationMode == AuthenticationMode.Forms)
                ? FormsAuthentication.LoginUrl.ToLowerInvariant()
                : ResourceProvider.LogOnPageVirtualPath);
        }

        /// <summary>
        /// Returns the URL for the automatical authentication the specified login to the web-site of the specified organization.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to log in.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>The URL for the automatical logging the specified login to the web-site of the specified organization.</returns>
        public string GetLoginUrl(Guid loginId, Guid organizationId)
        {
            return GetLoginUrl(loginId, organizationId, null);
        }

        /// <summary>
        /// Returns the URL for the automatical authentication the specified login to the web-site of the specified organization.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to log in.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="returnUrl">The URL that the autenticated user will redirect to.</param>
        /// <returns>The URL for the automatical logging the specified login to the web-site of the specified organization.</returns>
        public string GetLoginUrl(Guid loginId, Guid organizationId, string returnUrl)
        {
            return GetLoginUrl(loginId, organizationId, Guid.Empty, returnUrl);
        }

        /// <summary>
        /// Returns the URL for the automatical authentication the specified login to the web-site of the specified organization and instance.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to log in.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="instanceId">The instance identifier.</param>
        /// <param name="returnUrl">The URL that the autenticated user will redirect to.</param>
        /// <returns>The URL for the automatical logging the specified login to the web-site of the specified organization.</returns>
        public string GetLoginUrl(Guid loginId, Guid organizationId, Guid instanceId, string returnUrl)
        {
            return GetLoginUrl(loginId, true, organizationId, instanceId, returnUrl);
        }

        /// <summary>
        /// Returns the URL for the automatical authentication the specified login to the web-site of the specified organization and instance.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to log in.</param>
        /// <param name="addPassword">The value indicating that the encrypted hash of the password should be added to the result string. It's should be true for auto login.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="instanceId">The instance identifier.</param>
        /// <param name="returnUrl">The URL that the autenticated user will redirect to.</param>
        /// <returns>The URL for the automatical logging the specified login to the web-site of the specified organization.</returns>
        public virtual string GetLoginUrl(Guid loginId, bool addPassword, Guid organizationId, Guid instanceId, string returnUrl)
        {
            DataRowView drv = GetLogin(loginId);
            if (drv != null)
                return GetLoginUrl(drv["LoginName"].ToString(), (addPassword ? drv["Password"].ToString() : null), organizationId, instanceId, false, returnUrl);

            return GetLoginUrl(null, null, organizationId, instanceId, false, returnUrl);
        }

        /// <summary>
        /// Returns the URL for the login page, the text box of which to input login name will be filled by specified value.
        /// </summary>
        /// <param name="loginName">The name for the login to fill the text box.</param>
        /// <returns>The URL for the login page, the text box of which to input login name will be filled by specified value.</returns>
        public string GetLoginUrl(string loginName)
        {
            return GetLoginUrl(loginName, null, Guid.Empty, Guid.Empty, false, null);
        }

        /// <summary>
        /// Returns the URL for the login page, the text box of which to input login name will be filled by specified value.
        /// </summary>
        /// <param name="loginName">The name for the login to fill the text box.</param>
        /// <param name="absoluteUri">Whether the specified URL should be converted to the URI, if it is possible.</param>
        /// <returns>The URL for the login page, the text box of which to input login name will be filled by specified value.</returns>
        public virtual string GetLoginUrl(string loginName, bool absoluteUri)
        {
            if (absoluteUri)
                return GetLoginUrl(loginName, null, Guid.Empty, Guid.Empty, false, null);

            return GetLoginUrl(false) + "?l=" + HttpUtility.UrlEncodeUnicode(loginName);
        }

        /// <summary>
        /// Returns the URL for the automatical authentication the specified login to the web-site of the specified organization.
        /// </summary>
        /// <param name="loginName">The login name to log in.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>The URL for the automatical logging the specified login to the web-site of the specified organization.</returns>
        public string GetLoginUrl(string loginName, Guid organizationId)
        {
            return GetLoginUrl(loginName, organizationId, null);
        }

        /// <summary>
        /// Returns the URL for the automatical authentication the specified login to the web-site of the specified organization.
        /// </summary>
        /// <param name="loginName">The login name to log in.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="returnUrl">The URL that the autenticated user will redirect to.</param>
        /// <returns>The URL for the automatical logging the specified login to the web-site of the specified organization.</returns>
        public string GetLoginUrl(string loginName, Guid organizationId, string returnUrl)
        {
            return GetLoginUrl(loginName, organizationId, Guid.Empty, returnUrl);
        }

        /// <summary>
        /// Returns the URL for the automatical authentication the specified login to the web-site of the specified organization and instance.
        /// </summary>
        /// <param name="loginName">The login name to log in.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="instanceId">The instance identifier.</param>
        /// <param name="returnUrl">The URL that the autenticated user will redirect to.</param>
        /// <returns>The URL for the automatical logging the specified login to the web-site of the specified organization and instance.</returns>
        public string GetLoginUrl(string loginName, Guid organizationId, Guid instanceId, string returnUrl)
        {
            return GetLoginUrl(loginName, false, organizationId, instanceId, returnUrl);
        }

        /// <summary>
        /// Returns the URL for the automatical authentication the specified login to the web-site of the specified organization and instance.
        /// </summary>
        /// <param name="loginName">The login name to log in.</param>
        /// <param name="addPassword">The value indicating that the encrypted hash of the password should be added to the result string. It's should be true for auto login.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="instanceId">The instance identifier.</param>
        /// <param name="returnUrl">The URL that the autenticated user will redirect to.</param>
        /// <returns>The URL for the automatical logging the specified login to the web-site of the specified organization and instance.</returns>
        public virtual string GetLoginUrl(string loginName, bool addPassword, Guid organizationId, Guid instanceId, string returnUrl)
        {
            DataRowView drv = GetLogin(loginName);
            if (drv != null)
                return GetLoginUrl(drv["LoginName"].ToString(), (addPassword ? drv["Password"].ToString() : null), organizationId, instanceId, false, returnUrl);

            return GetLoginUrl(null, null, organizationId, instanceId, false, returnUrl);
        }

        /// <summary>
        /// Returns the page's URL for the create new login in the specified instance of the specified organization.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="instanceId">The instance identifier.</param>
        /// <returns></returns>
        public virtual string GetCreateLoginUrl(Guid organizationId, Guid instanceId)
        {
            return GetLoginUrl(null, null, organizationId, instanceId, false, null);
        }

        public string GetPasswordRecoveryUrl(string loginName)
        {
            return GetPasswordRecoveryUrl(loginName, true);
        }

        public virtual string GetPasswordRecoveryUrl(string loginName, bool absoluteUri)
        {
            string url = string.Empty;
            if (absoluteUri)
                url = CustomUrlProvider.CreateApplicationUri(ResourceProvider.PasswordRecoveryPageVirtualPath);
            else
                url = ResourceProvider.PasswordRecoveryPageVirtualPath;
            if (!string.IsNullOrEmpty(loginName))
                url += "?l=" + HttpUtility.UrlEncodeUnicode(loginName);
            return url;
        }

        /// <summary>
        /// Gets the password for the specified login from the data source.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to retrieve the password for.</param>
        /// <returns>The password for the specified login.</returns>
        public virtual string GetPassword(Guid loginId)
        {
            string password = string.Empty;
            switch (FrameworkConfiguration.Current.WebApplication.Password.Format)
            {
                case CryptoMethod.None:
                    DataRowView drv = GetLogin(loginId);
                    if (drv != null) password = drv["Password"].ToString();
                    break;
                case CryptoMethod.Sha1:
                    password = GeneratePassword();
                    break;
            }
            return password;
        }

        /// <summary>
        /// Gets the specified login of specified organization.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="loginId">The unique identifier for the login.</param>
        public virtual DataView GetUserLdapInfo(Guid organizationId, Guid loginId)
        {
            return GetUserLdapInfo(organizationId, loginId, false);
        }

        /// <summary>
        /// Gets the specified login of specified organization.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="loginId">The unique identifier for the login.</param>
        public virtual DataView GetUserLdapInfo(Guid organizationId, Guid loginId, bool includeEmails)
        {
            DataRow dr = GetOrganizationsLoginsDataRow(loginId, organizationId);

            if (includeEmails)
            {
                string secondaryEmail = string.Empty;
                DataTable dt = EmailProvider.GetEmails(loginId);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (secondaryEmail.Length > 0)
                            secondaryEmail += ",";
                        secondaryEmail += dt.Rows[i]["Email"];
                    }
                }

                dr.Table.Columns.Add("PrimaryEmail");
                dr.Table.Columns.Add("SecondaryEmails");

                dr["PrimaryEmail"] = GetEmail(loginId);
                dr["SecondaryEmails"] = secondaryEmail;
            }

            return dr.Table.DefaultView;
        }

        /// <summary>
        /// Update the specified login of specified organization.
        /// </summary>
        /// <param name="organizationId">The organization identifier to update to.</param>
        /// <param name="loginId">The unique identifier for the login to update.</param>
        /// <param name="ldapDomain">User ldap domain.</param>
        /// <param name="ldapUserAlias">User ldap alias.</param>
        /// <param name="ldapSecurityId">User ldap SID.</param>
        /// <param name="ldapUserId">User ldap GUID.</param>
        public virtual void UpdateUserLdapInfo(Guid organizationId, Guid loginId, string firstName, string lastName, string ldapDomain, string ldapDomainFull, string ldapUserAlias, string ldapUpn, string ldapSecurityId, Guid ldapUserId, string ldapOUPath)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlCommand command2 = null;

            try
            {
                connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString);

                command = new SqlCommand("dbo.Mc_UpdateLoginLdapInfo", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@OrganizationId", SqlDbType.UniqueIdentifier).Value = organizationId;
                command.Parameters.Add("@LoginId", SqlDbType.UniqueIdentifier).Value = loginId;
                command.Parameters.Add("@LdapDomain", SqlDbType.NVarChar).Value = ldapDomain;
                command.Parameters.Add("@LdapDomainFull", SqlDbType.NVarChar).Value = ldapDomainFull;
                command.Parameters.Add("@LdapUserAlias", SqlDbType.NVarChar).Value = ldapUserAlias;
                command.Parameters.Add("@LdapUPN", SqlDbType.NVarChar).Value = ldapUpn ?? string.Empty;
                command.Parameters.Add("@LdapSecurityId", SqlDbType.NVarChar).Value = ldapSecurityId;
                command.Parameters.Add("@LdapUserId", SqlDbType.UniqueIdentifier).Value = ldapUserId;
                command.Parameters.Add("@LdapOUPath", SqlDbType.NVarChar).Value = ldapOUPath;

                Support.ExecuteNonQuery(command);

                if ((string.IsNullOrEmpty(firstName) == false) && (string.IsNullOrEmpty(lastName) == false))
                {
                    command2 = new SqlCommand("dbo.Mc_GetOrganizationsByLoginId", connection);
                    command2.CommandType = CommandType.StoredProcedure;
                    command2.Parameters.Add("@LoginId", SqlDbType.UniqueIdentifier).Value = loginId;

                    DataTable organizationsTable = Support.GetDataTable(command2);

                    if (organizationsTable.Rows.Count == 1)
                    {
                        DataRowView drv = GetLogin(loginId);
                        UpdateLogin(loginId, (string)drv["LoginName"], (string)drv["Password"], firstName, lastName);
                    }

                    Dal.OrganizationDataSet.UserRow userRow = UserProvider.GetUserRow(loginId, organizationId);
                    if (userRow != null)
                    {
                        UserProvider.UpdateUser(loginId, userRow.Email, firstName, lastName, userRow.MiddleName, userRow.Phone, userRow.MobilePhone, userRow.Fax, userRow.Title, userRow.Department, userRow.Street, userRow.Street2, userRow.City, userRow.State, userRow.PostalCode, userRow.Country
                            , (userRow.IsTimeZoneIdNull() ? null : userRow.TimeZoneId), (userRow.IsTimeFormatNull() ? null : new int?(userRow.TimeFormat)), (userRow.IsDateFormatNull() ? null : new int?(userRow.DateFormat))
                            , userRow.GroupId, organizationId, false);
                        UserProvider.RaiseUserUpdated(loginId, organizationId, new List<Guid>());
                    }
                }
            }
            finally
            {
                if (connection != null) connection.Dispose();
                if (command != null) command.Dispose();
                if (command2 != null) command2.Dispose();
            }
        }

        /// <summary>
        /// Update the specified login of specified organization.
        /// </summary>
        /// <param name="organizationId">The organization identifier to update to.</param>
        /// <param name="loginId">The unique identifier for the login to update.</param>
        /// <param name="ldapDomain">User ldap domain.</param>
        /// <param name="ldapUserAlias">User ldap alias.</param>
        /// <param name="ldapSecurityId">User ldap SID.</param>
        /// <param name="ldapUserId">User ldap GUID.</param>
        /// <param name="secondaryEmails">User secondary emails.</param>
        public virtual void UpdateUserLdapInfo(Guid organizationId, Guid loginId, string firstName, string lastName, string ldapDomain, string ldapDomainFull, string ldapUserAlias, string ldapUpn, string ldapSecurityId, Guid ldapUserId, string ldapOUPath, string secondaryEmails)
        {
            UpdateUserLdapInfo(organizationId, loginId, firstName, lastName, ldapDomain, ldapDomainFull, ldapUserAlias, ldapUpn, ldapSecurityId, ldapUserId, ldapOUPath);
            UserProvider.UpdateUserSecondaryEmails(loginId, secondaryEmails);
        }

        /// <summary>
        /// Invites the people to the organization's groups.
        /// </summary> 
        /// <param name="invitedByLoginId">The identifier of the login which create the invitation.</param>
        /// <param name="invitedByFullName">The full name of the person, which create the invitation.</param>
        /// <param name="invitedByEmail">The email of the person, which create the invitation.</param>
        /// <param name="emails">The emails of the people, who are invited.</param>
        /// <param name="organizationId">The organization's identifier the people are invited to.</param>
        /// <param name="groupId">The list of the group's identifiers the people are invited to.</param>
        /// <param name="additionalMessage">The additional message for the people.</param>
        public virtual void Invite(Guid invitedByLoginId, string invitedByFullName, string invitedByEmail, string emails, Guid organizationId, string groupId, string additionalMessage)
        {
            CommonDataSet.InvitedLoginDataTable table = GetInvitedLoginsByOrganizationId(organizationId);
            if (table == null) return;

            if (string.IsNullOrEmpty(emails)) return;

            List<string> emailsList = new List<string>(emails.ToLowerInvariant().Split(",;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            foreach (CommonDataSet.InvitedLoginRow row in table)
            {
                string str = row.LoginName.ToLowerInvariant();
                if (emailsList.Contains(str)) emails = emails.Replace(str, string.Empty);
            }

            emailsList = new List<string>(emails.Split(",;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            if (emailsList.Count == 0) return;

            string url = CustomUrlProvider.CreateApplicationUri(ResourceProvider.SignupUserPageVirtualPath) + "?i=";
            string subject = string.Format(CultureInfo.CurrentCulture, Resources.EmailNotification_InviteUser_Subject, invitedByFullName, FrameworkConfiguration.Current.WebApplication.Name);

            StringBuilder sb = new StringBuilder(Resources.EmailNotification_InviteUser_Body);
            sb.Replace("{ApplicationName}", FrameworkConfiguration.Current.WebApplication.Name);
            sb.Replace("{ApplicationUrl}", CustomUrlProvider.ApplicationUri);
            sb.Replace("{InvitedByFullName}", invitedByFullName);
            sb.Replace("{InvitedByEmail}", invitedByEmail);
            if (!string.IsNullOrEmpty(additionalMessage)) additionalMessage += Environment.NewLine;
            sb.Replace("{AdditionalMessage}", additionalMessage);

            table.Clear();
            table.AcceptChanges();

            foreach (string email in emailsList)
            {
                if (!this.LoginInOrganization(email, organizationId))
                {
                    CommonDataSet.InvitedLoginRow row = table.NewInvitedLoginRow();
                    row.InvitedLoginId = Guid.NewGuid();
                    row.LoginName = email;
                    row.OrganizationId = organizationId;
                    row.GroupId = groupId;
                    row.InvitedBy = invitedByLoginId;
                    row.CreatedTime = DateTime.UtcNow;
                    table.AddInvitedLoginRow(row);

                    Support.SendEmail(invitedByEmail, email, null, subject, sb.ToString().Replace("{SignUpUserPageUrl}", url + row.InvitedLoginId.ToString("N")), false, true, EmailSendingReason.InviteUser);
                }
            }

            WebApplication.CommonDataSetTableAdapters.InvitedLoginTableAdapter.Update(table);
        }

        /// <summary>
        /// Verifies that the specified login exists in the specified organization. 
        /// </summary>
        /// <param name="loginId">The unique identifier for the login.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>true, if the login exists in the organization; otherwise, false.</returns>
        public virtual bool LoginInOrganization(Guid loginId, Guid organizationId)
        {
            return (GetOrganizationsLoginsDataRow(loginId, organizationId) != null);
        }

        /// <summary>
        /// Verifies that the specified login exists in the specified organization. 
        /// </summary>
        /// <param name="loginName">The login name.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>true, if the login exists in the organization; otherwise, false.</returns>
        public virtual bool LoginInOrganization(string loginName, Guid organizationId)
        {
            Guid loginId = Guid.Empty;
            DataRowView drv = GetLogin(loginName);
            if (drv != null) loginId = (Guid)drv["LoginId"];
            return LoginInOrganization(loginId, organizationId);
        }

        /// <summary>
        /// Verifies that the specified login exists and active in the specified organization. 
        /// </summary>
        /// <param name="loginId">The unique identifier for the login.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>true, if the login exists and active in the organization; otherwise, false.</returns>
        public virtual bool LoginIsActiveInOrganization(Guid loginId, Guid organizationId)
        {
            DataRow row = GetOrganizationsLoginsDataRow(loginId, organizationId);
            if (row != null)
                return Convert.ToBoolean(row["Active"], CultureInfo.InvariantCulture);
            return false;
        }

        /// <summary>
        /// Verifies that the specified login is administrator of the specified organization. 
        /// </summary>
        /// <param name="loginId">The unique identifier for the login.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>true, if the login is administrator of the specified organization; otherwise, false.</returns>
        public virtual bool LoginIsOrganizationAdministrator(Guid loginId, Guid organizationId)
        {
            DataRow row = GetOrganizationsLoginsDataRow(loginId, organizationId);
            if (row != null)
                return Convert.ToBoolean(row["OrganizationAdministrator"], CultureInfo.CurrentCulture);
            return false;
        }

        /// <summary>
        /// Verifies that the specified login is administrator of the specified organization. 
        /// </summary>
        /// <param name="loginId">The login name.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>true, if the login is administrator of the specified organization; otherwise, false.</returns>
        public virtual bool LoginIsOrganizationAdministrator(string loginName, Guid organizationId)
        {
            Guid loginId = Guid.Empty;
            DataRowView drv = GetLogin(loginName);
            if (drv != null) loginId = (Guid)drv["LoginId"];
            return LoginIsOrganizationAdministrator(loginId, organizationId);
        }

        /// <summary>
        /// Returns true if the login name is found in data source; otherwise, false;
        /// </summary>
        /// <param name="loginName">The name for the login to find.</param>
        /// <returns>true if the login name is found in data source; otherwise, false;</returns>
        public virtual bool LoginNameExists(string loginName)
        {
            return (GetLogin(loginName) != null);
        }

        /// <summary>
        /// Generates a password.
        /// </summary>
        /// <returns>A newly generated password.</returns>
        public string GeneratePassword()
        {
            return GeneratePassword(FrameworkConfiguration.Current.WebApplication.Password.MinRequiredPasswordLength, FrameworkConfiguration.Current.WebApplication.Password.MinRequiredNonAlphanumericCharacters);
        }

        /// <summary>
        /// Generates a password with specified parameters.
        /// </summary>
        /// <param name="minRequiredPasswordLength">The minimum length required for a password.</param>
        /// <param name="minRequiredNonAlphanumericCharacters">The minimum number of special characters that must be present in a password.</param>
        /// <returns>A newly generated password.</returns>
        public virtual string GeneratePassword(int minRequiredPasswordLength, int minRequiredNonAlphanumericCharacters)
        {
            return Support.GeneratePassword(((minRequiredPasswordLength < 5) ? 5 : minRequiredPasswordLength), minRequiredNonAlphanumericCharacters);
        }

        /// <summary>
        /// Removes the specified login from specified organization.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to remove.</param>
        /// <param name="organizationId">The organization identifier to remove from.</param>
        public virtual void RemoveLoginFromOrganization(Guid loginId, Guid organizationId)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString);

                command = new SqlCommand("[dbo].[Mc_DeleteOrganizationLogin]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@OrganizationId", SqlDbType.UniqueIdentifier).Value = organizationId;
                command.Parameters.Add("@LoginId", SqlDbType.UniqueIdentifier).Value = loginId;

                Support.ExecuteNonQuery(command);
            }
            finally
            {
                if (connection != null) connection.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public virtual void ResetPassword(Guid loginId)
        {
            this.DeleteExpiredResetPasswordRequests();

            CommonDataSet.ResetPasswordRequestRow row = null;
            CommonDataSet.ResetPasswordRequestDataTable table = null;
            try
            {
                table = new CommonDataSet.ResetPasswordRequestDataTable();
                WebApplication.CommonDataSetTableAdapters.ResetPasswordRequestTableAdapter.Fill(table, 1, loginId);

                if (table.Count > 0)
                    row = table[0];
                else
                {
                    row = table.NewResetPasswordRequestRow();
                    row.ResetPasswordRequestId = Guid.NewGuid();
                    row.LoginId = loginId;
                    row.CreatedTime = DateTime.UtcNow;
                    table.AddResetPasswordRequestRow(row);

                    WebApplication.CommonDataSetTableAdapters.ResetPasswordRequestTableAdapter.Update(table);
                }
            }
            finally
            {
                if (table != null) table.Dispose();
            }

            string subject = string.Format(CultureInfo.InvariantCulture, Resources.EmailNotification_ResetPassword_Subject, FrameworkConfiguration.Current.WebApplication.Name);

            string body = Resources.EmailNotification_ResetPassword_Body;
            body = body.Replace("{PasswordResetPageUrl}", CustomUrlProvider.CreateApplicationUri(ResourceProvider.ResetPasswordPageVirtualPath) + "?r=" + row.ResetPasswordRequestId.ToString("N"));
            body = body.Replace("{ApplicationUrl}", CustomUrlProvider.ApplicationUri);

            Support.SendEmail(FrameworkConfiguration.Current.WebApplication.Support.Email, GetEmail(loginId), null, subject, body, false, false, EmailSendingReason.ResetPassword);
        }

        /// <summary>
        /// Generates a new token for the specified login.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to reset token of.</param>
        /// <returns>A newly generated token.</returns>
        public virtual string ResetToken(Guid loginId)
        {
            string token = Support.GeneratePseudoUnique(32);
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString);

                command = new SqlCommand("[dbo].[Mc_UpdateLoginToken]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@LoginId", SqlDbType.UniqueIdentifier).Value = loginId;
                command.Parameters.Add("@Token", SqlDbType.VarChar, 50).Value = token;

                Support.ExecuteNonQuery(command);
            }
            finally
            {
                if (connection != null) connection.Dispose();
                if (command != null) command.Dispose();
            }

            return token;
        }

        /// <summary>
        /// Generates a new token for the specified login.
        /// </summary>
        /// <param name="loginName">The name for the login to reset token of.</param>
        /// <returns>A newly generated token.</returns>
        public virtual string ResetToken(string loginName)
        {
            Guid loginId = Guid.Empty;
            DataRowView drv = GetLogin(loginName);
            if (drv != null)
                loginId = (Guid)drv["LoginId"];

            return ResetToken(loginId);
        }

        /// <summary>
        /// Updates information about a login in the data source.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to update information for.</param>
        /// <param name="details">An System.Object array containing zero or more objects that represents the details for the login.</param>
        /// <returns>true, if the user was successfully updated; otherwise, false.</returns>
        public virtual bool UpdateLogin(Guid loginId, params object[] details)
        {
            int rowAffected = -1;
            string firstName = null;
            string lastName = null;
            string loginName = null;
            string password = null;
            bool? deleted = null;
            bool? isOrganizationAdministrator = null;

            if (details != null)
            {
                if (details.Length > 0)
                {
                    loginName = (details[0] as string);
                    if (details.Length > 1) password = (details[1] as string);
                    if (details.Length > 2) firstName = (details[2] as string);
                    if (details.Length > 3) lastName = (details[3] as string);
                    if (details.Length > 4) deleted = (details[4] as bool?);
                    if (details.Length > 5) isOrganizationAdministrator = (details[5] as bool?);

                    SqlConnection connection = null;
                    SqlCommand command = null;

                    try
                    {
                        connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString);

                        command = new SqlCommand("[dbo].[Mc_UpdateLogin]", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@LoginId", SqlDbType.UniqueIdentifier).Value = loginId;
                        command.Parameters.Add("@LoginName", SqlDbType.NVarChar, 255).Value = (string.IsNullOrEmpty(loginName) ? (object)DBNull.Value : loginName);
                        command.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = (string.IsNullOrEmpty(password) ? (object)DBNull.Value : password);
                        command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 255).Value = (string.IsNullOrEmpty(firstName) ? (object)DBNull.Value : firstName);
                        command.Parameters.Add("@LastName", SqlDbType.NVarChar, 255).Value = (string.IsNullOrEmpty(lastName) ? (object)DBNull.Value : lastName);
                        command.Parameters.Add("@Deleted", SqlDbType.Bit).Value = (deleted.HasValue ? deleted.Value : (object)DBNull.Value);

                        rowAffected = Support.ExecuteNonQuery(command);
                    }
                    finally
                    {
                        if (connection != null) connection.Dispose();
                        if (command != null) command.Dispose();
                    }
                }
            }

            return (rowAffected > 0);
        }

        /// <summary>
        /// Updates the identifier of the session of the specified login in the data source.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to update information for.</param>
        /// <param name="sessionId">The identifier of the session.</param>
        /// <returns>true, if the user was successfully updated; otherwise, false.</returns>
        public virtual bool UpdateSession(Guid loginId, string sessionId)
        {
            int rowAffected = -1;

            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString);

                command = new SqlCommand("[dbo].[Mc_UpdateLoginSession]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@LoginId", SqlDbType.UniqueIdentifier).Value = loginId;
                command.Parameters.Add("@SessionId", SqlDbType.VarChar, 50).Value = (string.IsNullOrEmpty(sessionId) ? (object)DBNull.Value : sessionId);

                rowAffected = Support.ExecuteNonQuery(command);
            }
            finally
            {
                if (connection != null) connection.Dispose();
                if (command != null) command.Dispose();
            }

            return (rowAffected > 0);
        }

        /// <summary>
        /// Updates the organization's specific information of the specified login in the data source.
        /// </summary>
        /// <param name="loginId">The unique identifier for the login to update information for.</param>
        /// <param name="organizationId">The organization identifier to update information for.</param>
        /// <param name="organizationAdministrator">true, if the user is organization administrator; otherwise, false; if it's null reference, the flag will be ignored during update.</param>
        /// <param name="active">true, if the user is active in the specified organization; otherwise, false; if it's null reference, the flag will be ignored during update.</param>
        /// <returns>true, if the user was successfully updated; otherwise, false.</returns>
        public virtual bool UpdateLoginInOrganization(Guid loginId, Guid organizationId, bool? organizationAdministrator, bool? active)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString);

                command = new SqlCommand("[dbo].[Mc_UpdateOrganizationLogin]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@OrganizationId", SqlDbType.UniqueIdentifier).Value = organizationId;
                command.Parameters.Add("@LoginId", SqlDbType.UniqueIdentifier).Value = loginId;
                command.Parameters.Add("@OrganizationAdministrator", SqlDbType.Bit).Value = (organizationAdministrator.HasValue ? organizationAdministrator.Value : (object)DBNull.Value);
                command.Parameters.Add("@Active", SqlDbType.Bit).Value = (active.HasValue ? active.Value : (object)DBNull.Value);

                return (Support.ExecuteNonQuery(command) > 0);

            }
            finally
            {
                if (connection != null) connection.Dispose();
                if (command != null) command.Dispose();
            }
        }

        /// <summary>
        /// Verifies that the specified login and password exist in the data source. 
        /// </summary>
        /// <param name="loginName">The name for the login to validate.</param>
        /// <param name="password">The password for the specified login.</param>
        /// <returns>true, if the specified login and password are valid; otherwise, false.</returns>
        public virtual bool ValidateLogin(string loginName, string password)
        {
            return (GetLogin(loginName, password) != null);
        }

        /// <summary>
        /// Verifies that the specified login and password exist in the data source. 
        /// </summary>
        /// <param name="loginId">The unique identifier of the login to validate.</param>
        /// <param name="password">The password for the specified login.</param>
        /// <returns>true, if the unique identifier and password of the specified login are valid; otherwise, false.</returns>
        public virtual bool ValidateLogin(Guid loginId, string password)
        {
            DataRowView drv = GetLogin(loginId);
            if (drv != null)
                return (string.Compare((string)drv["Password"], ((password != null) ? EncryptPassword(password) : password), StringComparison.Ordinal) == 0);
            return false;
        }

        /// <summary>
        /// Verifies that the specified password matches the requirements and generates an exception, if it is invalid.
        /// </summary>
        /// <param name="password">Specifies the password string.</param>
        /// <returns>true, if the specified password are valid; otherwise, false.</returns>
        public bool ValidatePassword(string password)
        {
            return ValidatePassword(password, true);
        }

        /// <summary>
        /// Verifies that the specified password matches the requirements.
        /// </summary>
        /// <param name="password">Specifies the password string.</param>
        /// <param name="throwOnError">true to throw an exception if an error occured; false to return false.</param>
        /// <returns>true, if the specified password are valid; otherwise, false.</returns>
        public virtual bool ValidatePassword(string password, bool throwOnError)
        {
            bool isValid = true;
            if (password == null) password = string.Empty;

            if (password.Length < FrameworkConfiguration.Current.WebApplication.Password.MinRequiredPasswordLength)
                isValid = false;

            if ((!isValid) && throwOnError)
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture
                    , Resources.LoginProvider_ErrorMessage_PasswordTooShort
                    , FrameworkConfiguration.Current.WebApplication.Password.MinRequiredPasswordLength));

            Regex rx = new Regex("\\d|\\w");
            if (rx.Replace(password, string.Empty).Length < FrameworkConfiguration.Current.WebApplication.Password.MinRequiredNonAlphanumericCharacters)
                isValid = false;

            if ((!isValid) && throwOnError)
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture
                    , Resources.LoginProvider_ErrorMessage_PasswordTooSimple
                    , FrameworkConfiguration.Current.WebApplication.Password.MinRequiredPasswordLength
                    , FrameworkConfiguration.Current.WebApplication.Password.MinRequiredNonAlphanumericCharacters));

            return isValid;
        }

        /// <summary>
        /// Verifies that the specified and the last sessions of the specified login are the same, or the last session is null.
        /// </summary>
        /// <param name="loginId">The unique identifier of the login to validate.</param>
        /// <param name="sessionId">The identifier of the current session.</param>
        /// <returns>true, if the validation is passed; otherwise, false.</returns>
        public virtual bool ValidateSession(Guid loginId, string sessionId)
        {
            string lastSessionId = GetSession(loginId);
            return (string.IsNullOrEmpty(lastSessionId) || (string.Compare(lastSessionId, sessionId, StringComparison.OrdinalIgnoreCase) == 0));
        }

        #endregion
    }
}
