using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.DirectoryServices.ActiveDirectory;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Net;
using System.Threading;
using System.Data;

namespace Micajah.Common.LdapAdapter
{
    public class LdapProvider : IDisposable
    {
        Guid ServerID;
        string ServerAddress;
        Int32 Port;
        string UserName;
        string Password;
        string SiteDomain;
        string RootDN;
        string AuthenticationTypeString;
        string OutputError;

        ILdapIntegration ApplicationProvider;
        ILogger ApplicationLogger;

        bool isConnectionOK = false;
        int LdapErrorCode = 0;

        //        private static SortedDictionary<DateTime, string> results = new SortedDictionary<DateTime, string>();
        private LdapConnection ldapConn;

        #region Ldap Init Functions

        public LdapProvider(Guid serverId, ILdapIntegration applicationProvider, ILogger applicationLogger)
        {
            ApplicationProvider = applicationProvider;
            ApplicationLogger = applicationLogger;

            if (serverId != new Guid())
            {
                ILdapServer serverObject = ApplicationProvider.GetLdapServer(serverId);
                if (serverObject != null)
                {
                    ServerID = serverId;
                    ServerAddress = serverObject.ServerAddress;
                    Port = serverObject.Port;
                    UserName = serverObject.UserName;
                    Password = serverObject.Password;
                    SiteDomain = serverObject.SiteDomain;
                    AuthenticationTypeString = serverObject.AuthenticationType;
                }
            }
            else
            {
                ServerID = serverId;
                ServerAddress = ConfigurationManager.AppSettings["ServerAddress"];
                Port = Convert.ToInt16(ConfigurationManager.AppSettings["Port"], CultureInfo.CurrentCulture);
                UserName = ConfigurationManager.AppSettings["UserName"];
                Password = ConfigurationManager.AppSettings["Password"];
                SiteDomain = ConfigurationManager.AppSettings["SiteDomain"];
                AuthenticationTypeString = ConfigurationManager.AppSettings["AuthenticationType"];
            }
        }

        public LdapProvider(Guid serverId, string serverAddress, Int32 port, string userName, string password, string siteDomain, string authenticationType, ILdapIntegration applicationProvider, ILogger applicationLogger)
        {
            ApplicationProvider = applicationProvider;
            ApplicationLogger = applicationLogger;

            ServerID = serverId;
            ServerAddress = serverAddress;
            Port = port;
            UserName = userName;
            Password = password;
            SiteDomain = siteDomain;
            AuthenticationTypeString = authenticationType;
        }

        public LdapProvider(ILdapIntegration applicationProvider, ILogger applicationLogger)
        {
            ServerID = Guid.NewGuid();
            ApplicationProvider = applicationProvider;
            ApplicationLogger = applicationLogger;
        }

        public bool SetNewConnection(Guid serverId,
        string serverAddress,
        Int32 port,
        string userName,
        string password,
        string siteDomain,
        string authenticationType)
        {
            ServerID = serverId;
            ServerAddress = serverAddress;
            Port = port;
            UserName = userName;
            Password = password;
            SiteDomain = siteDomain;
            AuthenticationTypeString = authenticationType;

            return Initialize();
        }

        public static bool ServerCallback(LdapConnection connection, X509Certificate certificate)
        {
            return true;
        }

        private static X509Certificate ClientCallback(LdapConnection conn, byte[][] trustedCAs)
        {
            return null;
        }

        public bool Initialize()
        {
            // Get Connection Elements
            LdapDirectoryIdentifier ldapDir = new LdapDirectoryIdentifier(ServerAddress, Port);
            ldapConn = new LdapConnection(ldapDir);
            ldapConn.SessionOptions.ProtocolVersion = 3;
            ldapConn.SessionOptions.VerifyServerCertificate = new VerifyServerCertificateCallback(ServerCallback);
            ldapConn.SessionOptions.QueryClientCertificate = new QueryClientCertificateCallback(ClientCallback);
            ldapConn.Timeout = new TimeSpan(1, 0, 0);

            if (AuthenticationTypeString == "Default")
            {
                ldapConn.SessionOptions.SecureSocketLayer = true;
                NetworkCredential myCredentials = new NetworkCredential(UserName, Password, SiteDomain);
                ldapConn.Credential = myCredentials;
                ldapConn.AuthType = AuthType.Ntlm;
            }
            else if (AuthenticationTypeString == "NonSSL")
            {
                ldapConn.SessionOptions.SecureSocketLayer = false;
                ldapConn.AuthType = AuthType.Negotiate;
            }
            else
            {
                ApplicationLogger.LogError("LDAPServer", "LDAP connection error.", new Exception("Authentication type is incorrect!"));
                isConnectionOK = false;
                throw new System.DirectoryServices.Protocols.LdapException("Authentication type is incorrect!");
            }

            // tring to connect
            try
            {
                ldapConn.Bind();
            }
            catch (LdapException)
            {
                isConnectionOK = false;
                return false;
            }
            catch (DirectoryOperationException)
            {
                isConnectionOK = false;
                return false;
            }

            // Get RootDN through the RootDSE onject of DirectoryEntry
            var request = new SearchRequest(null, "(&(objectClass=*))", System.DirectoryServices.Protocols.SearchScope.Base);
            var response = (SearchResponse)ldapConn.SendRequest(request, new TimeSpan(1, 0, 0));
            RootDN = (string)(response.Entries[0].Attributes["defaultNamingContext"][0]);

            isConnectionOK = true;

            return true;
        }

        public bool InitializeNewConnection()
        {
            // Get Connection Elements
            LdapDirectoryIdentifier ldapDir = new LdapDirectoryIdentifier(ServerAddress, Port);
            ldapConn = new LdapConnection(ldapDir);
            ldapConn.SessionOptions.ProtocolVersion = 3;
            ldapConn.SessionOptions.VerifyServerCertificate = new VerifyServerCertificateCallback(ServerCallback);
            ldapConn.SessionOptions.QueryClientCertificate = new QueryClientCertificateCallback(ClientCallback);
            ldapConn.Timeout = new TimeSpan(1, 0, 0);

            if (AuthenticationTypeString == "Default")
            {
                ldapConn.SessionOptions.SecureSocketLayer = true;
                NetworkCredential myCredentials = new NetworkCredential(UserName, Password, SiteDomain);
                ldapConn.Credential = myCredentials;
                ldapConn.AuthType = AuthType.Ntlm;
            }
            else if (AuthenticationTypeString == "NonSSL")
            {
                ldapConn.SessionOptions.SecureSocketLayer = false;
                ldapConn.AuthType = AuthType.Negotiate;
            }
            else
            {
                ApplicationLogger.LogError("LDAPServer", "LDAP connection error.", new Exception("Authentication type is incorrect!"));
                isConnectionOK = false;
                throw new System.DirectoryServices.Protocols.LdapException("Authentication type is incorrect!");
            }

            // tring to connect
            try
            {
                // Get RootDN through the RootDSE onject of DirectoryEntry
                var request = new SearchRequest(null, "(&(objectClass=*))", System.DirectoryServices.Protocols.SearchScope.Base);
                var response = (SearchResponse)ldapConn.SendRequest(request, new TimeSpan(1, 0, 0));
                RootDN = (string)(response.Entries[0].Attributes["defaultNamingContext"][0]);

                isConnectionOK = true;
            }
            catch (LdapException)
            {
                isConnectionOK = false;
                return false;
            }
            catch (DirectoryOperationException)
            {
                isConnectionOK = false;
                return false;
            }

            return true;
        }

        public void Ping()
        {
            // Get Connection Elements
            LdapDirectoryIdentifier ldapDir = new LdapDirectoryIdentifier(ServerAddress, Port);
            ldapConn = new LdapConnection(ldapDir);
            ldapConn.SessionOptions.ProtocolVersion = 3;
            ldapConn.SessionOptions.VerifyServerCertificate = new VerifyServerCertificateCallback(ServerCallback);
            ldapConn.SessionOptions.QueryClientCertificate = new QueryClientCertificateCallback(ClientCallback);
            ldapConn.Timeout = new TimeSpan(1, 0, 0);

            if (AuthenticationTypeString == "Default")
            {
                ldapConn.SessionOptions.SecureSocketLayer = true;
                NetworkCredential myCredentials = new NetworkCredential(UserName, Password, SiteDomain);
                ldapConn.Credential = myCredentials;
                ldapConn.AuthType = AuthType.Ntlm;
            }
            else if (AuthenticationTypeString == "NonSSL")
            {
                ldapConn.SessionOptions.SecureSocketLayer = false;
                ldapConn.AuthType = AuthType.Negotiate;
            }
            else
            {
                ApplicationLogger.LogError("LDAPServer", "LDAP connection error.", new Exception("Authentication type is incorrect!"));
                isConnectionOK = false;
                throw new System.DirectoryServices.Protocols.LdapException("Authentication type is incorrect!");
            }

            ldapConn.Bind();
        }

        #endregion

        #region Main Functions - User Authentication

        public Response<AuthenticationStatus> AuthenticateUser(string inputValue, string password, bool usePasswordEncryption, Guid organizationId)
        {
            string responseMessage = null;
            // Searching for the user in the Local DB
            User user = null;
            List<IUser> users = FindLocalUsers(inputValue.Replace("'", "''"));

            if (users.Count == 1)
                user = (User)users[0];
            else if (users.Count > 1)
                user = (User)GetUserFromMultiInstanceList(users, password, usePasswordEncryption, organizationId);

            // Check if user has found in the Local DB
            if (user != null)
            {
                // Check if founded user is Authenticated and Active
                ApplicationLogger.LogInfo("LdapProvider", string.Format(CultureInfo.CurrentCulture, "OrganizationGuid={0},InstanceGuid={1},LdapServerAddress={2},LdapServerPort={3},LdapDomain={4}.", user.OrganizationId.ToString(), user.InstanceId.ToString(), user.LdapServerAddress, user.LdapServerPort.ToString(CultureInfo.CurrentCulture), user.LdapDomain));
                return IsLocalUserAuthenticatedAndActive(user, password, usePasswordEncryption);
            }
            else
            {
                // Check if Ldap details exists
                if (isConnectionOK == true)
                {
                    // Searching for the user in Ldap
                    DomainUser ldapUser = FindLdapUser(inputValue);

                    // Check if the user was found in LDAP and IsActive
                    if (ldapUser != null)
                    {
                        // Check if the user IsActive in LDAP
                        if (ldapUser.IsActive == true)
                        {
                            // We have Ldap infomation, we need try to logon
                            if (IsAuthenticatedUser(ServerAddress, this.Port, ldapUser.AccountName, password, ldapUser.DomainName))
                            {
                                // We should create local app user
                                // Fill data in User structure
                                user = convertDomainUserToUser(ldapUser, password);

                                // Get list of groups
                                Dictionary<Guid, GroupListItemValue> h = new Dictionary<Guid, GroupListItemValue>();
                                h = GetUserGroupList(ldapUser);

                                // Create user in the local DB
                                if (user.OrganizationId != new Guid())
                                {
                                    string groupId = string.Empty;
                                    foreach (Guid key in h.Keys)
                                    {
                                        groupId = string.Concat(groupId, key.ToString("D", CultureInfo.CurrentCulture), ",");
                                    }
                                    user.LoginName = inputValue;
                                    user.LocalLoginId = ApplicationProvider.CreateLocalUser(user, (string.IsNullOrEmpty(groupId)) ? groupId : groupId.Substring(0, groupId.Length - 1));

                                    //Crerate user secondary emails
                                    ApplicationProvider.CreateUserEmails(GetUserAltEmails(user.UserId, false), user);
                                }

                                // Return that user is Authenticated
                                return new Response<AuthenticationStatus>() { ResponseValue = AuthenticationStatus.Authenticated, GroupList = h, LoginId = user.LocalLoginId, ChangeGroups = true, OrganizationId = user.OrganizationId };
                            }
                            else
                                responseMessage = "User not found in local database. User found in ldap database but not authenticated.";
                        }
                        else
                            responseMessage = "User not found in local database. User is inactive in ldap database.";
                    }
                    else
                        responseMessage = "User not found in local database. User not found in ldap database or inactive.";
                }
                else
                    responseMessage = "User not found in local or ldap database.";
            }

            if (string.IsNullOrEmpty(OutputError) == true)
                return new Response<AuthenticationStatus>() { ResponseValue = AuthenticationStatus.NotAuthenticated, GroupList = null, LoginId = Guid.Empty, ResponseMessage = responseMessage, ShowResponse = true };
            else
                return new Response<AuthenticationStatus>() { ResponseValue = AuthenticationStatus.NotAuthenticated, GroupList = null, LoginId = Guid.Empty, ResponseMessage = OutputError, ShowResponse = true };
        }

        // Check if founded local user is active and authenticated
        public Response<AuthenticationStatus> CheckUserLDAPPasswordAndActive(User user, string password)
        {
            string responseMessage = null;
            
            if (string.IsNullOrEmpty(this.AuthenticationTypeString))
                this.AuthenticationTypeString = "Default";

            if (IsAuthenticatedUser(user.LdapServerAddress, user.LdapServerPort, user.LdapUserAlias, password, user.LdapDomain) || IsAuthenticatedUser(user.LdapServerAddress, user.LdapServerPort, user.LdapUserPrinciple, password, user.LdapDomain))
            {
                try
                {
                    isConnectionOK = true;
                    DomainUser domainUser = GetUserActivity(user.LdapUserId);
                    if (domainUser != null)
                    {
                        if (domainUser.IsActive == true)
                            return new Response<AuthenticationStatus>() { ResponseValue = AuthenticationStatus.Authenticated, LoginId = user.LocalLoginId, ChangeGroups = false, OrganizationId = user.OrganizationId };
                        else
                            responseMessage = "User found but local password is incorrect. User is inactive in ldap database.";
                    }
                    else
                        responseMessage = "User found but local password is incorrect. User not found in ldap database.";

                }
                catch { responseMessage = "LdapConnectionProblems"; }
            }
            else
            {
                if (LdapErrorCode == 81)
                    responseMessage = "LdapConnectionProblems";
                else
                    responseMessage = string.Format("User found but local password is incorrect. User is not authenticated in ldap. Ldap Error Code: {0}", this.LdapErrorCode);
            }


            return new Response<AuthenticationStatus>() { ResponseValue = AuthenticationStatus.NotAuthenticated, GroupList = null, LoginId = Guid.Empty, ResponseMessage = responseMessage, ShowResponse = true };
        }

        // Check if founded local user is active and authenticated
        private Response<AuthenticationStatus> IsLocalUserAuthenticatedAndActive(User user, string password, bool usePasswordEncryption)
        {
            string responseMessage = null;
            string hashedPassword = password;
            if (usePasswordEncryption) hashedPassword = ApplicationProvider.GetHashedPassword(password);
            // Compare local DB simple user password and inserted password
            if (user.Password == hashedPassword)
            {
                //return new Response<AuthenticationStatus>() { ResponseValue = AuthenticationStatus.Authenticated, LoginId = user.LocalLoginId, ChangeGroups = false, OrganizationId = user.OrganizationId };

                // Local founded user password and input password are the same!
                // we need just check if user is active
                ApplicationLogger.LogInfo("LdapProvider", "Password from local db and inputed by user are THE SAME!");
                if ((String.IsNullOrEmpty(user.LdapServerAddress) == false) && (String.IsNullOrEmpty(user.LdapServerPort.ToString(CultureInfo.CurrentCulture)) == false) && (String.IsNullOrEmpty(user.LdapDomain) == false) && (String.IsNullOrEmpty(user.LdapServerUserName) == false))
                {
                    // Set Ldap Details to Adapter
                    SetNewConnection(user.OrganizationId, user.LdapServerAddress, Convert.ToInt32(user.LdapServerPort, CultureInfo.CurrentCulture), user.LdapServerUserName, user.LdapServerPassword, user.LdapDomain, "Default");
                }
                if (isConnectionOK == true)
                {
                    // we have Ldap infomation, we need get IsActive property from Ldap
                    ApplicationLogger.LogInfo("LdapProvider", "We have Ldap Information from local DB. Checking if user isActive in Ldap...");
                    DomainUser domainUser = FindLdapUser(user.EmailAddress);
                    if (domainUser != null && domainUser.IsActive == true)
                    {
                        //// Get list of groups
                        //Dictionary<Guid, GroupListItemValue> h = new Dictionary<Guid, GroupListItemValue>();
                        //h = GetUserGroupList(domainUser);
                        // Set Ldap Details
                        User tmpUser = convertDomainUserToUser(domainUser, user.LdapServerPassword);
                        tmpUser.LocalLoginId = user.LocalLoginId;
                        ApplicationProvider.SetLdapInfoDetails(tmpUser);

                        //Crerate user secondary emails
                        //ApplicationProvider.CreateUserEmails(GetUserAltEmails(tmpUser.UserId, false), tmpUser);

                        // Return that user is Authenticated
                        //return new Response<AuthenticationStatus>() { ResponseValue = AuthenticationStatus.Authenticated, GroupList = h, LoginId = user.LocalLoginId, ChangeGroups = true, OrganizationId = tmpUser.OrganizationId };
                        return new Response<AuthenticationStatus>() { ResponseValue = AuthenticationStatus.Authenticated, LoginId = user.LocalLoginId, ChangeGroups = false, OrganizationId = tmpUser.OrganizationId };
                    }
                    else if (domainUser == null)
                    {
                        // we don't find user in Ldap, we need check IsActive property from local DB
                        ApplicationLogger.LogInfo("LdapProvider", "We don't find user in Ldap. Checking local isActive property...");
                        if (user.IsActive == true)
                        {
                            // Return that user is Authenticated
                            return new Response<AuthenticationStatus>() { ResponseValue = AuthenticationStatus.Authenticated, GroupList = null, LoginId = user.LocalLoginId };
                        }
                        else
                            responseMessage = "User found and local password is correct. But user is inactive in ldap.";
                    }
                    else
                        responseMessage = "User found and local password is correct. But user is inactive in ldap.";
                }
                else
                {
                    // we don't have Ldap information, we need check IsActive property from local DB
                    ApplicationLogger.LogInfo("LdapProvider", "We don't have Ldap Information from local DB. Checking local isActive property...");
                    if (user.IsActive == true)
                    {
                        // Return that user is Authenticated
                        return new Response<AuthenticationStatus>() { ResponseValue = AuthenticationStatus.Authenticated, GroupList = null, LoginId = user.LocalLoginId };
                    }
                    else
                        responseMessage = "User found and local password is correct. Can not connect to ldap. User is inactive in local database.";
                }
            }
            else
            {
                // Local founded user password and input password are NOT the same!
                // we need get data from Ldap server
                ApplicationLogger.LogInfo("LdapProvider", "Password from local db and inputed by user are DIFFERENT!");
                if ((String.IsNullOrEmpty(user.LdapServerAddress) == false) && (String.IsNullOrEmpty(user.LdapServerPort.ToString(CultureInfo.CurrentCulture)) == false) && (String.IsNullOrEmpty(user.LdapDomain) == false) && (String.IsNullOrEmpty(user.LdapServerUserName) == false))
                {
                    // Set Ldap Details to Adapter
                    SetNewConnection(user.OrganizationId, user.LdapServerAddress, Convert.ToInt32(user.LdapServerPort, CultureInfo.CurrentCulture), user.LdapServerUserName, user.LdapServerPassword, user.LdapDomain, "Default");
                }
                if (isConnectionOK == true)
                {
                    // we have Ldap infomation, we need try to logon and get IsActive property
                    ApplicationLogger.LogInfo("LdapProvider", "We have Ldap Information from local db. Trying to authenticate the user...");
                    DomainUser domainUser = FindLdapUser(user.EmailAddress);
                    if (domainUser != null)
                    {
                        if (domainUser.IsActive == true)
                        {
                            if (IsAuthenticatedUser(user.LdapServerAddress, user.LdapServerPort, domainUser.AccountName, password, user.LdapDomain))
                            {
                                // user is Authenticated, we need to check IsActive property from Ldap
                                ApplicationLogger.LogInfo("LdapProvider", "User has been authenticated in Ldap.");
                                // Get list of groups
                                //Dictionary<Guid, GroupListItemValue> h = new Dictionary<Guid, GroupListItemValue>();
                                //h = GetUserGroupList(domainUser);
                                // Set Ldap Details
                                User tmpUser = convertDomainUserToUser(domainUser, password);
                                tmpUser.LocalLoginId = user.LocalLoginId;
                                ApplicationProvider.SetLdapInfoDetails(tmpUser);

                                //Crerate user secondary emails
                                //ApplicationProvider.CreateUserEmails(GetUserAltEmails(tmpUser.UserId, false), tmpUser);

                                // Return that user is Authenticated
                                //return new Response<AuthenticationStatus>() { ResponseValue = AuthenticationStatus.Authenticated, GroupList = h, LoginId = user.LocalLoginId, ChangeGroups = true, OrganizationId = tmpUser.OrganizationId };
                                return new Response<AuthenticationStatus>() { ResponseValue = AuthenticationStatus.Authenticated, LoginId = user.LocalLoginId, ChangeGroups = false, OrganizationId = tmpUser.OrganizationId };

                            }
                            else
                                responseMessage = "User found but local password is incorrect. User is not authenticated in ldap.";
                        }
                        else
                            responseMessage = "User found but local password is incorrect. User is inactive in ldap database.";
                    }
                    else
                        responseMessage = "User found but local password is incorrect. User not found in ldap database.";
                }
                else
                    responseMessage = "User found but local password is incorrect. Can not connect to ldap.";
            }

            // We don't have Ldap information or user is NOT active - Logon is FAILED!
            return new Response<AuthenticationStatus>() { ResponseValue = AuthenticationStatus.NotAuthenticated, GroupList = null, LoginId = Guid.Empty, ResponseMessage = responseMessage, ShowResponse = true };
        }

        //Tries authenticated user in AD
        public bool IsAuthenticatedUser(string serverAddress, int serverPort, string login, string password, string serverDomain)
        {
            // Get Connection Elements
            LdapDirectoryIdentifier ldapDir = new LdapDirectoryIdentifier(serverAddress, serverPort);
            ldapConn = new LdapConnection(ldapDir);
            ldapConn.SessionOptions.ProtocolVersion = 3;
            ldapConn.SessionOptions.VerifyServerCertificate = new VerifyServerCertificateCallback(ServerCallback);
            ldapConn.SessionOptions.QueryClientCertificate = new QueryClientCertificateCallback(ClientCallback);
            ldapConn.Timeout = new TimeSpan(1, 0, 0);

            if (AuthenticationTypeString == "Default")
            {
                ldapConn.SessionOptions.SecureSocketLayer = true;
                NetworkCredential myCredentials = new NetworkCredential(login, password, serverDomain);
                ldapConn.Credential = myCredentials;
                ldapConn.AuthType = AuthType.Ntlm;
            }
            else if (AuthenticationTypeString == "NonSSL")
            {
                ldapConn.SessionOptions.SecureSocketLayer = false;
                ldapConn.AuthType = AuthType.Negotiate;
            }
            else
            {
                ApplicationLogger.LogError("LDAPServer", "LDAP connection error.", new Exception("Authentication type is incorrect!"));
                isConnectionOK = false;
                throw new System.DirectoryServices.Protocols.LdapException("Authentication type is incorrect!");
            }

            // tring to connect
            try
            {
                ldapConn.Bind();
            }
            catch (LdapException ex)
            {
                this.LdapErrorCode = ex.ErrorCode;
                isConnectionOK = false;
                return false;
            }
            catch (DirectoryOperationException)
            {
                isConnectionOK = false;
                return false;
            }

            // Get RootDN through the RootDSE onject of DirectoryEntry
            var request = new SearchRequest(null, "(&(objectClass=*))", System.DirectoryServices.Protocols.SearchScope.Base);
            var response = (SearchResponse)ldapConn.SendRequest(request, new TimeSpan(1, 0, 0));
            RootDN = (string)(response.Entries[0].Attributes["defaultNamingContext"][0]);

            isConnectionOK = true;

            return true;
        }

        #endregion

        #region Local User Searching

        // searching user in the local DB by username, email, e.t.c.
        private List<IUser> FindLocalUsers(string userName)
        {
            List<IUser> users = new List<IUser>();
            bool isDomainSearhed = false;
            string domainName = null;
            string userAlias = null;
            string[] parts = null;

            if (userName.Contains('@'))
            {
                // check email and domain login
                users = (List<IUser>)ApplicationProvider.GetUsersByEmail(userName);
                if (users.Count == 0)
                {
                    domainName = userName.Split('@')[1];
                    userAlias = userName.Split('@')[0];
                    if (string.IsNullOrEmpty(domainName) == false && string.IsNullOrEmpty(userAlias) == false)
                        users = (List<IUser>)ApplicationProvider.GetUsersByDomainLogin(domainName, userAlias, (userAlias.Contains('.')) ? userAlias.Split('.')[0] : null, (userAlias.Contains('.')) ? userAlias.Split('.')[1] : null);
                    isDomainSearhed = true;
                }
            }
            if (users.Count == 0 && userName.Contains('\\'))
            {
                // check domain login and email
                if (isDomainSearhed == false)
                {
                    domainName = userName.Split('\\')[0];
                    userAlias = userName.Split('\\')[1];
                    if (string.IsNullOrEmpty(domainName) == false && string.IsNullOrEmpty(userAlias) == false)
                        users = (List<IUser>)ApplicationProvider.GetUsersByDomainLogin(domainName, userAlias, (userAlias.Contains('.')) ? userAlias.Split('.')[0] : null, (userAlias.Contains('.')) ? userAlias.Split('.')[1] : null);
                    isDomainSearhed = true;
                }
                /*if (users.Count == 0)
                {
                    parts = userName.Split('\\');
                    if (string.IsNullOrEmpty(parts[0]) == false && string.IsNullOrEmpty(parts[1]) == false)
                        users = ApplicationProvider.GetUsersByEmail(parts[1] + '@' + parts[0]);
                }*/
            }
            if (users.Count == 0 && isDomainSearhed == false && userName.Contains('.'))
            {
                // check user alias and principal without domain
                parts = userName.Split('.');
                if (string.IsNullOrEmpty(parts[0]) == false && string.IsNullOrEmpty(parts[1]) == false)
                    users = (List<IUser>)ApplicationProvider.GetUsersByDomainLogin(null, userName, parts[0], parts[1]);
            }
            if (users.Count == 0 && userName.Contains(','))
            {
                // check user alias and principal without domain
                parts = userName.Split(',');
                if (string.IsNullOrEmpty(parts[0].Trim()) == false && string.IsNullOrEmpty(parts[1].Trim()) == false)
                    users = (List<IUser>)ApplicationProvider.GetUsersByDomainLogin(null, userName, parts[1].Trim(), parts[0].Trim());
            }
            if (users.Count == 0 && userName.Contains(' '))
            {
                // check user alias and principal without domain
                parts = userName.Split(' ');
                if (string.IsNullOrEmpty(parts[0].Trim()) == false && string.IsNullOrEmpty(parts[1].Trim()) == false)
                    users = (List<IUser>)ApplicationProvider.GetUsersByDomainLogin(null, userName, parts[0].Trim(), parts[1].Trim());
            }
            if (users.Count == 0)
            {
                // check user alias without domain
                users = (List<IUser>)ApplicationProvider.GetUsersByDomainLogin(null, userName, null, null);
            }

            // return result
            ApplicationLogger.LogInfo("LdapProvider", string.Format(CultureInfo.CurrentCulture, "User has {0} instance(s) in the local DB.", users.Count.ToString(CultureInfo.CurrentCulture)));

            return users;
        }

        private IUser GetUserFromMultiInstanceList(List<IUser> users, string password, bool usePasswordEncryption, Guid organizationId)
        {
            IUser user = null;
            string hashedPassword = password;
            if (usePasswordEncryption) hashedPassword = ApplicationProvider.GetHashedPassword(password);

            Guid orgID = Guid.Empty;
            string ldapServer = "";
            int ldapPort = 0;
            int orgLdapDetailsCount = 0;

            // Check simple password
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Password == hashedPassword && (organizationId == Guid.Empty || users[i].OrganizationId == organizationId))
                    return (IUser)users[i];
            }

            // Simple password compare is fail
            // Check if organizations have different ldap details
            foreach (IUser u in users)
            {
                if ((u.OrganizationId != Guid.Empty) && (u.OrganizationId != orgID) && (String.IsNullOrEmpty(u.LdapServerAddress) == false) && (String.IsNullOrEmpty(u.LdapServerPort.ToString(CultureInfo.CurrentCulture)) == false) && (String.IsNullOrEmpty(u.LdapDomain) == false) && (String.IsNullOrEmpty(u.LdapServerUserName) == false) && (ldapServer != u.LdapServerAddress) && (ldapPort != u.LdapServerPort))
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
            {
                ApplicationLogger.LogInfo("LdapProvider", "Local DB Search Error! Login not specific to (only )one organization with ldap details.");
                OutputError = "You appear to be associated to more than one organization. Use a more specific such as your email address or domain\\username and reattempt to log in.";
                return null;
            }

            return user;
        }

        #endregion

        #region Ldap User Searching

        private DomainUser FindLdapUser(string userName)
        {
            userName = userName.ToLower();
            DomainUser ldapUser = null;
            string[] parts = null;
            if (isConnectionOK == false) return null;
            if (String.IsNullOrEmpty(userName) == true) return null;

            StringBuilder ldapQuery = new StringBuilder();
            ldapQuery.Append("(&(objectClass=User)(|");

            ldapQuery.AppendFormat("(sAMAccountName={0})", userName); //Searching AD by sAMAccountName
            if (userName.Contains('@'))
                ldapQuery.AppendFormat("(mail={0})", userName); //Searching AD by primary email
            if (userName.Contains('@'))
                ldapQuery.AppendFormat("(userPrincipalName={0})", userName); //Searching AD by user principal name
            if (userName.Contains(' '))
                ldapQuery.AppendFormat("(cn={0})", userName); //Searching AD by CN            
            if (userName.Contains(','))
            {
                parts = userName.Split(',');
                if (parts.Length >= 2)
                    ldapQuery.AppendFormat("(givenName={0})(sn={0})", parts[1].Trim(), parts[0].Trim()); //Searching AD by Lastname,Firstname
            }
            if (userName.Contains('@'))
            {
                parts = userName.Split('@');
                ldapQuery.AppendFormat("(sAMAccountName={0})", parts[0]); //Searching AD by sAMAccountName
            }
            if (userName.Contains('\\'))
            {
                parts = userName.Split('\\');
                ldapQuery.AppendFormat("(sAMAccountName={0})", parts[1]); //Searching AD by sAMAccountName
            }
            if (userName.Contains('@'))
                ldapQuery.AppendFormat("(proxyAddresses=smtp:{0})", userName); //Searching AD by altemail

            ldapQuery.Append("))");

            SearchRequest sR = new SearchRequest(RootDN, ldapQuery.ToString(), System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "cn", "DistinguishedName", "givenName", "mail", "mobile", "objectCategory", "objectGUID", "objectSid", "sAMAccountName", "sn", "userAccountControl", "userPrincipalName", "memberOf", "primaryGroupID", "idautoPersonPreferredName" });
            SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

            if (sResp.Entries.Count > 0)
                ldapUser = createDomainUser(sResp.Entries[0]);
            else
                ldapUser = null;

            ldapQuery.Remove(0, ldapQuery.Length);
            ldapQuery = null;
            parts = null;

            return ldapUser;
        }

        #endregion

        #region User Role

        //public Guid GetUserRole(IUser user)
        //{
        //    SearchRequest sR = new SearchRequest(RootDN, "(&(objectClass=User)(sAMAccountName=" + user.LdapServerUserName + "))", System.DirectoryServices.Protocols.SearchScope.Subtree);
        //    SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR);

        //    SearchResultAttributeCollection attrs = sResp.Entries[0].Attributes;
        //    //showAllAttributes(attrs);

        //    // get list of group names for current organization
        //    DataSet data = ApplicationProvider.GetSortedListOfRolesGroupsById(((User)user).OrganizationId);

        //    if (data.Tables[0].Rows.Count > 0)
        //    {
        //        if (attrs["memberOf"] != null)
        //        {
        //            // get mapping group sub groups and check users memberOf attributes
        //            for (int i = 0; i < data.Tables[0].Rows.Count; i++)
        //            {
        //                DomainUserGroupCollection list = GetGroupSubgroups(data.Tables[0].Rows[i].Field<string>("GroupName"));
        //                List<string> names = new List<string>();
        //                foreach (DomainUserGroup group in list)
        //                    names.Add(group.DistinguishedName);

        //                // looking through all users memberOf attributes
        //                for (int j = 0; j < attrs["memberOf"].Count; j++)
        //                    if (names.Contains((string)attrs["memberOf"][j]))
        //                    {
        //                        ApplicationLogger.LogInfo("LdapProvider", string.Format(CultureInfo.CurrentCulture, "User '{0}' has '{1}' role. This role had been found by '{2}' group.", user.LdapServerUserName, data.Tables[0].Rows[i].Field<string>("RoleName"), (string)attrs["memberOf"][j]));
        //                        return data.Tables[0].Rows[i].Field<Guid>("RoleId");
        //                    }
        //            }
        //        }
        //        else
        //        {
        //            // check if user is in one of the selected groups
        //            for (int i = 0; i < data.Tables[0].Rows.Count; i++)
        //            {
        //                if (IsUserInGroup(user.LdapServerUserName, data.Tables[0].Rows[i].Field<string>("GroupName")))
        //                {
        //                    ApplicationLogger.LogInfo("LdapProvider", string.Format(CultureInfo.CurrentCulture, "User '{0}' has '{1}' role. This role had been found by '{2}' group.", user.LdapServerUserName, data.Tables[0].Rows[i].Field<string>("RoleName"), data.Tables[0].Rows[i].Field<string>("GroupName")));
        //                    return data.Tables[0].Rows[i].Field<Guid>("RoleId");
        //                }
        //            }
        //        }
        //    }
        //    else
        //        ApplicationLogger.LogInfo("LdapProvider", string.Format(CultureInfo.CurrentCulture, "OrganizationGuid = '{0}' doesn't have any mapped groups and roles.", user.OrganizationId));

        //    ApplicationLogger.LogInfo("LdapProvider", string.Format(CultureInfo.CurrentCulture, "'{0}' user role hadn't been found.", user.LdapServerUserName));
        //    return new Guid();
        //}

        /* old version
        public Guid getUserRole(IUser user)
        {
            DataSet data = ApplicationProvider.GetSortedListOfRolesGroupsById(((User)user).ServerID);
            if (data.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < data.Tables[0].Rows.Count; i++)
                {
                    if (isUserInGroup(user.LDAPAccountName, data.Tables[0].Rows[i].Field<string>("GroupName")))
                    {
                        ApplicationLogger.LogInfo("LdapProvider", string.Format("User '{0}' has '{1}' role.", ((User)user).LDAPAccountName, data.Tables[0].Rows[i].Field<Guid>("RoleName")));
                        return data.Tables[0].Rows[i].Field<Guid>("RoleId");
                    }
                }
            }
            else
                ApplicationLogger.LogInfo("LdapProvider", string.Format("Organization '{0}' doesn't have any mapped groups and roles.", ((User)user).organizationName));

            return new Guid();
        }*/

        #endregion

        #region Ldap Functions (Like GetDomains)

        public DomainCollection GetDomains()
        {
            if (isConnectionOK == false) return null;

            var sR = new SearchRequest(RootDN, "(&(objectClass=Domain))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "name", "DistinguishedName", "objectGuid" });
            var sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

            if (sResp.Entries.Count > 0)
            {
                //SearchResultAttributeCollection attrs = sResp.Entries[0].Attributes;
                //showAllAttributes(attrs);
                DomainCollection list = new DomainCollection();
                foreach (SearchResultEntry rez in sResp.Entries)
                {
                    list.Add(new Domain()
                    {
                        Server = this,
                        Name = getAttributeStringValue(rez.Attributes, "name"),
                        DistinguishedName = getAttributeStringValue(rez.Attributes, "DistinguishedName"),
                        Guid = new Guid(getAttributeByteArrayValue(rez.Attributes, "objectGuid"))
                        //Sid = ((rez.Attributes["objectSid"][0]).GetType() == typeof(byte[])) ? getAttributeByteArrayValue(rez.Attributes, "objectSid") : null
                    });
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        public Domain GetDomainByName(string name)
        {
            if (isConnectionOK == false) return null;

            var sR = new SearchRequest(RootDN, "(&(objectClass=Domain)(name=" + name + "))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "name", "DistinguishedName", "objectGuid", "objectSid" });
            var sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

            if (sResp.Entries.Count > 0)
            {
                SearchResultEntry rez = sResp.Entries[0];
                Domain domain = new Domain()
                {
                    Server = this,
                    Name = getAttributeStringValue(rez.Attributes, "name"),
                    DistinguishedName = getAttributeStringValue(rez.Attributes, "DistinguishedName"),
                    Guid = new Guid(getAttributeByteArrayValue(rez.Attributes, "objectGuid")),
                    Sid = ((rez.Attributes["objectSid"][0]).GetType() == typeof(byte[])) ? getAttributeByteArrayValue(rez.Attributes, "objectSid") : null
                };
                return domain;
            }
            else
            {
                return null;
            }
        }

        public Domain GetDomainByDistinguishedName(string distinguishedName)
        {
            if (isConnectionOK == false) return null;

            var sR = new SearchRequest(RootDN, "(&(objectClass=domain)(distinguishedName=" + distinguishedName + "))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "name", "distinguishedName", "objectGuid", "objectSid" });
            var sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

            if (sResp.Entries.Count > 0)
            {
                SearchResultEntry rez = sResp.Entries[0];
                Domain domain = new Domain()
                {
                    Server = this,
                    Name = getAttributeStringValue(rez.Attributes, "name"),
                    DistinguishedName = getAttributeStringValue(rez.Attributes, "distinguishedName"),
                    Guid = new Guid(getAttributeByteArrayValue(rez.Attributes, "objectGuid")),
                    Sid = ((rez.Attributes["objectSid"][0]).GetType() == typeof(byte[])) ? getAttributeByteArrayValue(rez.Attributes, "objectSid") : null
                };
                return domain;
            }
            else
            {
                return null;
            }
        }

        private Domain GetGroupDomain(string GroupName)
        {
            if (isConnectionOK == false) return null;

            DomainCollection domains = GetDomains();

            for (int j = 0; j < domains.Count; j++)
            {
                if (GroupName.Contains(domains[j].DistinguishedName))
                {
                    return domains[j];
                }
            }

            return null;
        }

        public DomainUserGroupCollection GetGroups(Domain domain)
        {
            if (isConnectionOK == false) return null;

            var request = new SearchRequest(domain.DistinguishedName, "(&(objectClass=Group))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "objectGuid", "name" });
            var response = (SearchResponse)ldapConn.SendRequest(request, new TimeSpan(1, 0, 0));

            if (response.Entries.Count > 0)
            {
                var list = new DomainUserGroupCollection();
                foreach (SearchResultEntry resultEntry in response.Entries)
                {
                    list.Add(new DomainUserGroup()
                    {
                        Domain = domain,
                        GroupGuid = new Guid(getAttributeByteArrayValue(resultEntry.Attributes, "objectGuid")),
                        Name = getAttributeStringValue(resultEntry.Attributes, "name")//,
                        //DistinguishedName = getAttributeStringValue(resultEntry.Attributes, "DistinguishedName"),
                        //SamAccountName = getAttributeStringValue(resultEntry.Attributes, "sAMAccountName"),
                        //PrimaryGroupToken = ((resultEntry.Attributes["objectSid"][0]).GetType() == typeof(byte[])) ? BitConverter.ToInt32(getAttributeByteArrayValue(resultEntry.Attributes, "objectSid"), 24) : -1
                    });
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        public DomainUserGroupCollection GetGroups(string distinguishedName)
        {
            if (isConnectionOK == false) return null;

            var request = new SearchRequest(distinguishedName, "(&(objectClass=Group))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "objectGuid", "name", "DistinguishedName" });
            var response = (SearchResponse)ldapConn.SendRequest(request, new TimeSpan(1, 0, 0));

            if (response.Entries.Count > 0)
            {
                var list = new DomainUserGroupCollection();
                foreach (SearchResultEntry resultEntry in response.Entries)
                {
                    list.Add(new DomainUserGroup()
                    {
                        Domain = null,
                        GroupGuid = new Guid(getAttributeByteArrayValue(resultEntry.Attributes, "objectGuid")),
                        Name = getAttributeStringValue(resultEntry.Attributes, "name"),
                        DistinguishedName = getAttributeStringValue(resultEntry.Attributes, "DistinguishedName")
                        //SamAccountName = getAttributeStringValue(resultEntry.Attributes, "sAMAccountName"),
                        //PrimaryGroupToken = ((resultEntry.Attributes["objectSid"][0]).GetType() == typeof(byte[])) ? BitConverter.ToInt32(getAttributeByteArrayValue(resultEntry.Attributes, "objectSid"), 24) : -1
                    });
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        public DomainUserGroupCollection GetGroups(Domain domain, int pageSize)
        {
            if (isConnectionOK == false) return null;

            var request = new SearchRequest(domain.DistinguishedName, "(&(objectClass=Group))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "objectGuid", "name" });
            PageResultRequestControl pageRequest = new PageResultRequestControl(pageSize);
            request.Controls.Add(pageRequest);
            SearchOptionsControl searchOptions = new SearchOptionsControl(SearchOption.DomainScope);
            request.Controls.Add(searchOptions);

            var response = (SearchResponse)ldapConn.SendRequest(request, new TimeSpan(1, 0, 0));

            if (response.Entries.Count > 0)
            {
                var list = new DomainUserGroupCollection();
                foreach (SearchResultEntry resultEntry in response.Entries)
                {
                    list.Add(new DomainUserGroup()
                    {
                        Domain = domain,
                        GroupGuid = new Guid(getAttributeByteArrayValue(resultEntry.Attributes, "objectGuid")),
                        Name = getAttributeStringValue(resultEntry.Attributes, "name"),
                    });
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        public DomainUserGroup GetGroupByCN(string groupName)
        {
            if (isConnectionOK == false) return null;
            DomainUserGroup group = null;

            var request = new SearchRequest(RootDN, "(&(objectClass=Group)(CN=" + groupName + "))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "DistinguishedName", "objectGuid", "name", "sAMAccountName", "sAMAccountName", "objectSid" });
            var response = (SearchResponse)ldapConn.SendRequest(request, new TimeSpan(1, 0, 0));

            if (response.Entries.Count > 0)
            {
                SearchResultEntry resultEntry = response.Entries[0];
                group = new DomainUserGroup()
                {
                    Domain = GetGroupDomain(getAttributeStringValue(resultEntry.Attributes, "DistinguishedName")),
                    GroupGuid = new Guid(getAttributeByteArrayValue(resultEntry.Attributes, "objectGuid")),
                    Name = getAttributeStringValue(resultEntry.Attributes, "name"),
                    DistinguishedName = getAttributeStringValue(resultEntry.Attributes, "DistinguishedName"),
                    SamAccountName = getAttributeStringValue(resultEntry.Attributes, "sAMAccountName"),
                    PrimaryGroupToken = ((resultEntry.Attributes["objectSid"][0]).GetType() == typeof(byte[])) ? BitConverter.ToInt32(getAttributeByteArrayValue(resultEntry.Attributes, "objectSid"), 24) : -1
                };
            }

            return group;
        }

        public DomainUserGroup GetGroupByName(string groupName)
        {
            if (isConnectionOK == false) return null;
            DomainUserGroup group = null;

            var request = new SearchRequest(RootDN, "(&(objectClass=Group)(Name=" + groupName + "))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "DistinguishedName", "objectGuid", "name", "sAMAccountName", "objectSid", "member" });
            var response = (SearchResponse)ldapConn.SendRequest(request, new TimeSpan(1, 0, 0));

            if (response.Entries.Count > 0)
            {
                /*                SearchResultAttributeCollection attrs = response.Entries[0].Attributes;
                                showAllAttributes(attrs);
                */
                SearchResultEntry resultEntry = response.Entries[0];
                group = new DomainUserGroup()
                {
                    Domain = GetGroupDomain(getAttributeStringValue(resultEntry.Attributes, "DistinguishedName")),
                    GroupGuid = new Guid(getAttributeByteArrayValue(resultEntry.Attributes, "objectGuid")),
                    Name = getAttributeStringValue(resultEntry.Attributes, "name"),
                    DistinguishedName = getAttributeStringValue(resultEntry.Attributes, "DistinguishedName"),
                    SamAccountName = getAttributeStringValue(resultEntry.Attributes, "sAMAccountName"),
                    PrimaryGroupToken = ((resultEntry.Attributes["objectSid"][0]).GetType() == typeof(byte[])) ? BitConverter.ToInt32(getAttributeByteArrayValue(resultEntry.Attributes, "objectSid"), 24) : -1,
                    GroupMembers = getAttributeStringArrayValue(resultEntry.Attributes, "member")
                };
            }

            return group;
        }

        public DomainUserCollection GetUsers(string[] groupNames)
        {
            if (isConnectionOK == false) return null;

            DomainUserCollection memberList = new DomainUserCollection();
            List<string> selectedGroups = new List<string>();
            List<string> selectedUsers = new List<string>();

            foreach (string groupName in groupNames)
            {
                DomainUserGroup group = GetGroupByName(groupName);
                if (group != null)
                {
                    //RecursiveGetGroupMembers(group, ref memberList, ref selectedGroups, ref selectedUsers);

                    if (!selectedGroups.Contains(group.SamAccountName))
                    {
                        selectedGroups.Add(group.SamAccountName);

                        ////Get all users from group and its child group                        
                        //SearchRequest sR = new SearchRequest(RootDN, string.Format("(&(objectClass=user)(memberof:1.2.840.113556.1.4.1941:={0}))", group.DistinguishedName), System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "cn", "DistinguishedName", "givenName", "mail", "mobile", "objectCategory", "objectGUID", "objectSid", "sAMAccountName", "sn", "userAccountControl", "userPrincipalName", "memberOf", "primaryGroupID", "idautoPersonPreferredName", "proxyAddresses" });
                        ////SearchRequest sR = new SearchRequest(RootDN, string.Format("(&(objectClass=user)(memberof={0}))", group.DistinguishedName), System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "cn", "DistinguishedName", "givenName", "mail", "mobile", "objectCategory", "objectGUID", "objectSid", "sAMAccountName", "sn", "userAccountControl", "userPrincipalName", "memberOf", "primaryGroupID", "idautoPersonPreferredName", "proxyAddresses" });
                        //SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

                        //AddLdapUsersToList(sResp, ref selectedUsers, ref memberList, group);

                        List<SearchResultEntry> list = PerformPagedSearch(ldapConn, RootDN, string.Format("(&(objectClass=user)(memberof:1.2.840.113556.1.4.1941:={0}))", group.DistinguishedName), new string[] { "cn", "DistinguishedName", "givenName", "mail", "mobile", "objectCategory", "objectGUID", "objectSid", "sAMAccountName", "sn", "userAccountControl", "userPrincipalName", "memberOf", "primaryGroupID", "idautoPersonPreferredName", "proxyAddresses" });
                        AddLdapUsersToList(list, ref selectedUsers, ref memberList, group);

                        //Get all users from primary group
                        if (group.PrimaryGroupToken > 0)
                        {
                            //sR = new SearchRequest(RootDN, "(&(objectClass=user)(primaryGroupID=" + group.PrimaryGroupToken.ToString(CultureInfo.CurrentCulture) + "))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "cn", "DistinguishedName", "givenName", "mail", "mobile", "objectCategory", "objectGUID", "objectSid", "sAMAccountName", "sn", "userAccountControl", "userPrincipalName", "memberOf", "primaryGroupID", "idautoPersonPreferredName", "proxyAddresses" });
                            //sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

                            //AddLdapUsersToList(sResp, ref selectedUsers, ref memberList, group);

                            list = PerformPagedSearch(ldapConn, RootDN, "(&(objectClass=user)(primaryGroupID=" + group.PrimaryGroupToken.ToString(CultureInfo.CurrentCulture) + "))", new string[] { "cn", "DistinguishedName", "givenName", "mail", "mobile", "objectCategory", "objectGUID", "objectSid", "sAMAccountName", "sn", "userAccountControl", "userPrincipalName", "memberOf", "primaryGroupID", "idautoPersonPreferredName", "proxyAddresses" });
                            AddLdapUsersToList(list, ref selectedUsers, ref memberList, group);
                        }

                        //Check if group is Domain User group or group is member of Domain User group
                        if (group.SamAccountName == "Domain User" || ((group.GroupMembers != null) && (group.GroupMembers.Count(x => x.Contains("Domain User"))) > 0))
                        {
                            //sR = new SearchRequest(RootDN, "(&(objectClass=user)(primaryGroupID=513))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "cn", "DistinguishedName", "givenName", "mail", "mobile", "objectCategory", "objectGUID", "objectSid", "sAMAccountName", "sn", "userAccountControl", "userPrincipalName", "primaryGroupID", "proxyAddresses", "memeberof" });
                            //PageResultRequestControl pageRequest = new PageResultRequestControl(int.MaxValue);
                            //sR.Controls.Add(pageRequest);
                            //sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

                            list = PerformPagedSearch(ldapConn, RootDN, "(&(objectClass=user)(primaryGroupID=513))", new string[] { "cn", "DistinguishedName", "givenName", "mail", "mobile", "objectCategory", "objectGUID", "objectSid", "sAMAccountName", "sn", "userAccountControl", "userPrincipalName", "memberOf", "primaryGroupID", "idautoPersonPreferredName", "proxyAddresses" });
                            AddLdapUsersToList(list, ref selectedUsers, ref memberList, group);
                        }
                    }

                    //GetGroupMembersByPrimaryGroupToken(group, ref memberList, ref selectedUsers);


                    //SearchRequest sR = new SearchRequest(group.DistinguishedName, "(&(objectClass=Group))", System.DirectoryServices.Protocols.SearchScope.Subtree);
                    //SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR);

                    //if (sResp.Entries.Count > 0)
                    //{
                    //    RecursiveGetGroupMembers(group, ref memberList, ref selectedGroups, ref selectedUsers);
                    //    GetGroupMembersByPrimaryGroupToken(group, ref memberList, ref selectedUsers);
                    //}
                }
            }

            return memberList;
        }

        private List<SearchResultEntry> PerformPagedSearch(LdapConnection connection, string baseDN, string filter, string[] attribs)
        {
            List<SearchResultEntry> results = new List<SearchResultEntry>();

            SearchRequest request = new SearchRequest(baseDN, filter, System.DirectoryServices.Protocols.SearchScope.Subtree, attribs);
            PageResultRequestControl prc = new PageResultRequestControl(500);

            //add the paging control
            request.Controls.Add(prc);

            while (true)
            {
                SearchResponse response = connection.SendRequest(request, new TimeSpan(1, 0, 0)) as SearchResponse;
                //find the returned page response control
                foreach (DirectoryControl control in response.Controls)
                {
                    if (control is PageResultResponseControl)
                    {
                        //update the cookie for next set
                        prc.Cookie = ((PageResultResponseControl)control).Cookie;
                        break;
                    }
                }

                //add them to our collection
                foreach (SearchResultEntry sre in response.Entries)
                {
                    results.Add(sre);
                }

                //our exit condition is when our cookie is empty
                if (prc.Cookie.Length == 0)
                    break;
            }
            return results;

        }

        private void AddLdapUsersToList(List<SearchResultEntry> list, ref List<string> selectedUsers, ref DomainUserCollection memberList, DomainUserGroup group)
        {
            if (list.Count > 0)
            {
                foreach (SearchResultEntry resultEntry in list)
                {
                    if (!selectedUsers.Contains(getAttributeStringValue(resultEntry.Attributes, "sAMAccountName")))
                    {
                        DomainUser user = createDomainUser(resultEntry);
                        if (user.MemberOfGroups != null)
                        {
                            if (!user.MemberOfGroups.Contains(group.DistinguishedName))
                            {
                                string[] userGroups = new string[user.MemberOfGroups.Length];
                                user.MemberOfGroups.CopyTo(userGroups, 0);
                                Array.Resize(ref userGroups, user.MemberOfGroups.Length + 1);
                                userGroups[user.MemberOfGroups.Length] = group.DistinguishedName;
                                user.MemberOfGroups = userGroups;
                            }
                        }
                        else
                        {
                            string[] userGroups = new string[1];
                            userGroups[0] = group.DistinguishedName;
                            user.MemberOfGroups = userGroups;
                        }
                        memberList.Add(user);
                        selectedUsers.Add(getAttributeStringValue(resultEntry.Attributes, "sAMAccountName"));
                    }
                    else
                    {
                        DomainUser user = memberList.FirstOrDefault(x => x.AccountName == getAttributeStringValue(resultEntry.Attributes, "sAMAccountName"));
                        if (user != null)
                        {
                            if (user.MemberOfGroups != null)
                            {
                                if (!user.MemberOfGroups.Contains(group.DistinguishedName))
                                {
                                    string[] userGroups = new string[user.MemberOfGroups.Length];
                                    user.MemberOfGroups.CopyTo(userGroups, 0);
                                    Array.Resize(ref userGroups, user.MemberOfGroups.Length + 1);
                                    userGroups[user.MemberOfGroups.Length] = group.DistinguishedName;
                                    user.MemberOfGroups = userGroups;
                                }
                            }
                            else
                            {
                                string[] userGroups = new string[1];
                                userGroups[0] = group.DistinguishedName;
                                user.MemberOfGroups = userGroups;
                            }
                        }
                    }
                }
            }
        }

        private void AddLdapUsersToList(SearchResponse sResp, ref List<string> selectedUsers, ref DomainUserCollection memberList, DomainUserGroup group)
        {
            if (sResp.Entries.Count > 0)
            {
                foreach (SearchResultEntry resultEntry in sResp.Entries)
                {
                    if (!selectedUsers.Contains(getAttributeStringValue(resultEntry.Attributes, "sAMAccountName")))
                    {
                        DomainUser user = createDomainUser(resultEntry);
                        if (!user.MemberOfGroups.Contains(group.DistinguishedName))
                        {
                            string[] userGroups = new string[user.MemberOfGroups.Length];
                            user.MemberOfGroups.CopyTo(userGroups, 0);
                            Array.Resize(ref userGroups, user.MemberOfGroups.Length + 1);
                            userGroups[user.MemberOfGroups.Length] = group.DistinguishedName;
                            user.MemberOfGroups = userGroups;
                        }
                        memberList.Add(user);
                        selectedUsers.Add(getAttributeStringValue(resultEntry.Attributes, "sAMAccountName"));
                    }
                }
            }
        }

        //public DomainUserCollection GetUsers(DomainUserGroup group, List<string> selectedGroups, List<string> selectedUsers)
        //{
        //    if (isConnectionOK == false) return null;

        //    SearchRequest sR = new SearchRequest(group.DistinguishedName, "(&(objectClass=Group))", System.DirectoryServices.Protocols.SearchScope.Subtree);
        //    SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR);

        //    //SearchResultAttributeCollection attrs = sResp.Entries[0].Attributes;
        //    //showAllAttributes(attrs);

        //    if (sResp.Entries.Count > 0)
        //    {
        //        DomainUserCollection memberList = new DomainUserCollection();
        //        RecursiveGetGroupMembers(group, ref memberList, ref selectedGroups, ref selectedUsers);
        //        GetGroupMembersByPrimaryGroupToken(group, ref memberList, ref selectedUsers);

        //        return memberList;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //protected void RecursiveGetGroupMembers(DomainUserGroup group, ref DomainUserCollection memberList, ref List<string> selectedGroups, ref List<string> selectedUsers)
        //{
        //    if (selectedGroups.Contains(group.SamAccountName))
        //        return;

        //    selectedGroups.Add(group.SamAccountName);

        //    SearchRequest sR = new SearchRequest(group.DistinguishedName, "(&(objectClass=Group))", System.DirectoryServices.Protocols.SearchScope.Subtree);
        //    SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR);

        //    SearchResultAttributeCollection attrs = sResp.Entries[0].Attributes;
        //    //showAllAttributes(attrs);

        //    if (attrs["member"] != null)
        //    {
        //        for (int i = 0; i < attrs["member"].Count; i++)
        //        {
        //            SearchRequest sRG = new SearchRequest(RootDN, "(&(DistinguishedName=" + (string)attrs["member"][i] + "))", System.DirectoryServices.Protocols.SearchScope.Subtree);
        //            SearchResponse sRespG = (SearchResponse)ldapConn.SendRequest(sRG);

        //            //showAllAttributes(sRespG.Entries[0].Attributes);

        //            if (isGroupEntity(sRespG.Entries[0].Attributes))
        //            {
        //                RecursiveGetGroupMembers(new DomainUserGroup() { Domain = group.Domain, GroupGuid = new Guid(getAttributeByteArrayValue(sRespG.Entries[0].Attributes, "objectGuid")), Name = getAttributeStringValue(sRespG.Entries[0].Attributes, "name"), DistinguishedName = getAttributeStringValue(sRespG.Entries[0].Attributes, "DistinguishedName"), SamAccountName = getAttributeStringValue(sRespG.Entries[0].Attributes, "sAMAccountName"), PrimaryGroupToken = ((sRespG.Entries[0].Attributes["objectSid"][0]).GetType() == typeof(byte[])) ? BitConverter.ToInt32(getAttributeByteArrayValue(sRespG.Entries[0].Attributes, "objectSid"), 24) : -1 }, ref memberList, ref selectedGroups, ref selectedUsers);
        //            }
        //            else
        //            {
        //                // add new user
        //                if (!selectedUsers.Contains(getAttributeStringValue(sRespG.Entries[0].Attributes, "sAMAccountName")))
        //                {
        //                    memberList.Add(createDomainUser(sRespG.Entries[0]));

        //                    selectedUsers.Add(getAttributeStringValue(sRespG.Entries[0].Attributes, "sAMAccountName"));
        //                }
        //            }
        //        }
        //    }

        //}

        protected void GetGroupMembersByPrimaryGroupToken(DomainUserGroup group, ref DomainUserCollection memberList, ref List<string> selectedUsers)
        {
            SearchRequest sR = new SearchRequest(RootDN, "(&(objectClass=User)(primaryGroupID=" + group.PrimaryGroupToken.ToString(CultureInfo.CurrentCulture) + "))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "cn", "DistinguishedName", "givenName", "mail", "mobile", "objectCategory", "objectGUID", "objectSid", "sAMAccountName", "sn", "userAccountControl", "userPrincipalName", "memberOf", "primaryGroupID", "idautoPersonPreferredName", "proxyAddresses" });
            SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

            if (sResp.Entries.Count > 0)
            {
                foreach (SearchResultEntry resultEntry in sResp.Entries)
                {
                    if (!selectedUsers.Contains(getAttributeStringValue(resultEntry.Attributes, "sAMAccountName")))
                    {
                        // add new user
                        memberList.Add(createDomainUser(resultEntry));

                        selectedUsers.Add(getAttributeStringValue(resultEntry.Attributes, "sAMAccountName"));
                    }
                }
            }

        }

        //public DomainUserGroupCollection GetGroupSubgroups(string groupName)
        //{
        //    if (isConnectionOK == false) return null;
        //    if (String.IsNullOrEmpty(groupName) == true)
        //        return null;

        //    Domain domain = GetGroupDomain(groupName);
        //    DomainUserGroupCollection list = new DomainUserGroupCollection();

        //    SearchRequest sR = new SearchRequest(groupName, "(&(objectClass=Group))", System.DirectoryServices.Protocols.SearchScope.Subtree);
        //    SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR);

        //    SearchResultAttributeCollection attrs = sResp.Entries[0].Attributes;
        //    //showAllAttributes(attrs);

        //    list.Add(new DomainUserGroup()
        //    {
        //        Domain = domain,
        //        GroupGuid = new Guid(getAttributeByteArrayValue(sResp.Entries[0].Attributes, "objectGuid")),
        //        Name = getAttributeStringValue(sResp.Entries[0].Attributes, "name"),
        //        DistinguishedName = getAttributeStringValue(sResp.Entries[0].Attributes, "DistinguishedName"),
        //        SamAccountName = getAttributeStringValue(sResp.Entries[0].Attributes, "sAMAccountName"),
        //        PrimaryGroupToken = ((attrs["objectSid"][0]).GetType() == typeof(byte[])) ? BitConverter.ToInt32(getAttributeByteArrayValue(sResp.Entries[0].Attributes, "objectSid"), 24) : -1
        //    });

        //    if (attrs["member"] != null)
        //    {
        //        for (int i = 0; i < attrs["member"].Count; i++)
        //        {
        //            SearchRequest sRG = new SearchRequest(RootDN, "(&(objectClass=Group)(DistinguishedName=" + (string)attrs["member"][i] + "))", System.DirectoryServices.Protocols.SearchScope.Subtree);
        //            SearchResponse sRespG = (SearchResponse)ldapConn.SendRequest(sRG);

        //            if (sRespG.Entries.Count > 0)
        //            {
        //                if (isGroupEntity(sRespG.Entries[0].Attributes))
        //                {
        //                    list.Add(new DomainUserGroup()
        //                    {
        //                        Domain = domain,
        //                        GroupGuid = new Guid(getAttributeByteArrayValue(sRespG.Entries[0].Attributes, "objectGuid")),
        //                        Name = getAttributeStringValue(sRespG.Entries[0].Attributes, "name"),
        //                        DistinguishedName = getAttributeStringValue(sRespG.Entries[0].Attributes, "DistinguishedName"),
        //                        SamAccountName = getAttributeStringValue(sRespG.Entries[0].Attributes, "sAMAccountName"),
        //                        PrimaryGroupToken = ((sRespG.Entries[0].Attributes["objectSid"][0]).GetType() == typeof(byte[])) ? BitConverter.ToInt32(getAttributeByteArrayValue(sRespG.Entries[0].Attributes, "objectSid"), 24) : -1
        //                    });
        //                }
        //            }
        //        }
        //    }

        //    return list;
        //}

        private static bool isGroupEntity(SearchResultAttributeCollection curAttr)
        {
            foreach (DictionaryEntry att in curAttr)
            {
                if (((DirectoryAttribute)att.Value).Name == "objectClass")
                {
                    for (int i = 0; i < ((DirectoryAttribute)att.Value).Count; i++)
                    {
                        if (((DirectoryAttribute)att.Value)[i].ToString() == "group")
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public IUser GetIUser(Guid userId)
        {
            return convertDomainUserToUser(GetUser(userId));
        }

        public IUser GetIUser(string userName)
        {
            return convertDomainUserToUser(GetUser(userName));
        }

        public DomainUser GetUserActivity(Guid objectId)
        {
            if (isConnectionOK == false) return null;

            string gid = Helper.ConvertObjectGuidToString(objectId);

            SearchRequest sR = new SearchRequest(RootDN, "(&(objectClass=User)(objectGUID=" + gid + "))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "userAccountControl"});
            SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

            if (sResp.Entries.Count > 0)
            {
                return new DomainUser()
                {
                    IsActive = ((Convert.ToInt32(getAttributeStringValue(sResp.Entries[0].Attributes, "userAccountControl"), CultureInfo.CurrentCulture) & 0x0002) == 0),
                };
            }
            else
            {
                return null;
            }
        }


        public DomainUser GetUser(Guid objectId)
        {
            if (isConnectionOK == false) return null;

            string gid = Helper.ConvertObjectGuidToString(objectId);

            SearchRequest sR = new SearchRequest(RootDN, "(&(objectClass=User)(objectGUID=" + gid + "))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "cn", "DistinguishedName", "givenName", "mail", "mobile", "objectCategory", "objectGUID", "objectSid", "sAMAccountName", "sn", "userAccountControl", "userPrincipalName", "memberOf", "primaryGroupID", "idautoPersonPreferredName" });
            SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

            if (sResp.Entries.Count > 0)
            {
                return createDomainUser(sResp.Entries[0]);
            }
            else
            {
                return null;
            }
        }

        public DomainUser GetUser(string userName)
        {
            if (isConnectionOK == false) return null;
            return FindLdapUser(userName);
        }

        public DomainUser GetUser(string ldapPath, string commonName)
        {
            if (isConnectionOK == false) return null;

            SearchRequest sR = new SearchRequest(ldapPath, "(&(objectClass=User)(CN=" + commonName + "))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "cn", "DistinguishedName", "givenName", "mail", "mobile", "objectCategory", "objectGUID", "objectSid", "sAMAccountName", "sn", "userAccountControl", "userPrincipalName", "memberOf", "primaryGroupID", "idautoPersonPreferredName" });
            SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

            if (sResp.Entries.Count > 0)
            {
                return createDomainUser(sResp.Entries[0]);
            }
            else
            {
                return null;
            }
        }

        public DomainUser GetUserByAccountName(string accountName)
        {
            if (isConnectionOK == false) return null;
            if (String.IsNullOrEmpty(accountName) == true) return null;

            SearchRequest sR = new SearchRequest(RootDN, "(&(objectClass=User)(sAMAccountName=" + accountName + "))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "cn", "DistinguishedName", "givenName", "mail", "mobile", "objectCategory", "objectGUID", "objectSid", "sAMAccountName", "sn", "userAccountControl", "userPrincipalName", "memberOf", "primaryGroupID", "idautoPersonPreferredName" });
            SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

            if (sResp.Entries.Count > 0)
            {
                //SearchResultAttributeCollection attrs = sResp.Entries[0].Attributes;
                //showAllAttributes(attrs);
                return createDomainUser(sResp.Entries[0]);
            }
            else
            {
                return null;
            }
        }

        public DomainUser GetUserByPrimaryEmail(string emailAddress)
        {
            if (isConnectionOK == false) return null;
            if (String.IsNullOrEmpty(emailAddress) == true) return null;

            SearchRequest sR = new SearchRequest(RootDN, "(&(objectClass=User)(mail=" + emailAddress + "))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "cn", "DistinguishedName", "givenName", "mail", "mobile", "objectCategory", "objectGUID", "objectSid", "sAMAccountName", "sn", "userAccountControl", "userPrincipalName", "memberOf", "primaryGroupID", "idautoPersonPreferredName" });
            SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

            if (sResp.Entries.Count > 0)
                return createDomainUser(sResp.Entries[0]);
            else
                return null;
        }

        public DomainUser GetUserByEmail(string emailAddress)
        {
            if (isConnectionOK == false) return null;
            if (String.IsNullOrEmpty(emailAddress) == true) return null;

            SearchRequest sR = new SearchRequest(RootDN, "(&(objectClass=User)(mail=" + emailAddress + "))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "cn", "DistinguishedName", "givenName", "mail", "mobile", "objectCategory", "objectGUID", "objectSid", "sAMAccountName", "sn", "userAccountControl", "userPrincipalName", "memberOf", "primaryGroupID", "idautoPersonPreferredName" });
            SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

            if (sResp.Entries.Count > 0)
            {
                //SearchResultAttributeCollection attrs = sResp.Entries[0].Attributes;
                //showAllAttributes(attrs);
                return createDomainUser(sResp.Entries[0]);
            }
            else
            {
                sR = new SearchRequest(RootDN, "(&(objectClass=User)(proxyAddresses=smtp:" + emailAddress + "))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "cn", "DistinguishedName", "givenName", "mail", "mobile", "objectCategory", "objectGUID", "objectSid", "sAMAccountName", "sn", "userAccountControl", "userPrincipalName", "memberOf", "primaryGroupID", "idautoPersonPreferredName" });
                sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

                if (sResp.Entries.Count > 0)
                    return createDomainUser(sResp.Entries[0]);
                else
                    return null;
            }
        }

        public DomainUser GetUserByCommonName(string commonName)
        {
            if (isConnectionOK == false) return null;
            if (String.IsNullOrEmpty(commonName) == true) return null;

            SearchRequest sR = new SearchRequest(RootDN, "(&(objectClass=User)(cn=" + commonName + "))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "cn", "DistinguishedName", "givenName", "mail", "mobile", "objectCategory", "objectGUID", "objectSid", "sAMAccountName", "sn", "userAccountControl", "userPrincipalName", "memberOf", "primaryGroupID", "idautoPersonPreferredName" });
            SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

            if (sResp.Entries.Count > 0)
            {
                return createDomainUser(sResp.Entries[0]);
            }
            else
            {
                return null;
            }
        }

        public DomainUser GetUserByPrincipalName(string userPrincipalName)
        {
            if (isConnectionOK == false) return null;
            if (String.IsNullOrEmpty(userPrincipalName) == true) return null;

            SearchRequest sR = new SearchRequest(RootDN, "(&(objectClass=User)(userPrincipalName=" + userPrincipalName + "))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "cn", "DistinguishedName", "givenName", "mail", "mobile", "objectCategory", "objectGUID", "objectSid", "sAMAccountName", "sn", "userAccountControl", "userPrincipalName", "memberOf", "primaryGroupID", "idautoPersonPreferredName" });
            SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

            if (sResp.Entries.Count > 0)
            {
                return createDomainUser(sResp.Entries[0]);
            }
            else
            {
                return null;
            }
        }

        public ReadOnlyCollection<string> GetUserAltEmails(Guid objectId, bool includeMainEmail)
        {
            List<string> list = new List<string>();
            if (isConnectionOK == false) return new ReadOnlyCollection<string>(list);

            string gid = Helper.ConvertObjectGuidToString(objectId);

            SearchRequest sR = new SearchRequest(RootDN, "(&(objectClass=User)(objectGUID=" + gid + "))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "cn", "DistinguishedName", "givenName", "mail", "mobile", "objectCategory", "objectGUID", "objectSid", "sAMAccountName", "sn", "userAccountControl", "userPrincipalName", "memberOf", "primaryGroupID", "idautoPersonPreferredName", "proxyAddresses" });
            SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

            if (sResp.Entries.Count > 0)
            {
                if (sResp.Entries[0].Attributes["proxyAddresses"] != null)
                {
                    for (int i = 0; i < sResp.Entries[0].Attributes["proxyAddresses"].Count; i++)
                    {
                        StringBuilder sb = new StringBuilder((string)sResp.Entries[0].Attributes["proxyAddresses"][i]);
                        if (sb.Length > 0 && sb.ToString().Contains('@'))
                        {
                            if (!includeMainEmail)
                            {
                                if (!sb.ToString().Contains("SMTP"))
                                    list.Add(sb.Remove(0, 5).ToString());
                            }
                            else
                                list.Add(sb.Remove(0, 5).ToString());
                        }
                    }
                }
            }

            return new ReadOnlyCollection<string>(list);
        }

        private DomainUser createDomainUser(SearchResultEntry rez)
        {
            string domainFull = GetDomainFullByUserDN(getAttributeStringValue(rez.Attributes, "DistinguishedName"));
            //Domain domain = GetDomainByDistinguishedName(ConvertDomainNameToLdapForm(domainFull));

            return new DomainUser()
            {
                Server = this,
                ObjectGuid = new Guid(getAttributeByteArrayValue(rez.Attributes, "objectGUID")),
                ObjectSid = ((rez.Attributes["objectSid"][0]).GetType() == typeof(byte[])) ? (new SecurityIdentifier(getAttributeByteArrayValue(rez.Attributes, "objectSid"), 0)).ToString() : string.Empty,
                AccountName = getAttributeStringValue(rez.Attributes, "sAMAccountName"),
                PrincipalName = getAttributeStringValue(rez.Attributes, "userPrincipalName"),
                //DomainName = domain.Name,
                DomainName = domainFull.Contains(".") ? domainFull.Split('.')[0] : domainFull,
                DomainFullName = domainFull,
                FirstName = getAttributeStringValue(rez.Attributes, "idautoPersonPreferredName") != null ? getAttributeStringValue(rez.Attributes, "idautoPersonPreferredName") : getAttributeStringValue(rez.Attributes, "givenName"),
                LastName = getAttributeStringValue(rez.Attributes, "sn"),
                EmailAddress = getAttributeStringValue(rez.Attributes, "mail"),
                CommonName = getAttributeStringValue(rez.Attributes, "cn"),
                Mobile = getAttributeStringValue(rez.Attributes, "mobile"),
                OULdapPath = getAttributeStringValue(rez.Attributes, "objectCategory"),
                DistinguishedName = getAttributeStringValue(rez.Attributes, "DistinguishedName"),
                OUPath = getOUPath(getAttributeStringValue(rez.Attributes, "DistinguishedName")),
                IsActive = ((Convert.ToInt32(getAttributeStringValue(rez.Attributes, "userAccountControl"), CultureInfo.CurrentCulture) & 0x0002) == 0),
                MemberOfGroups = getAttributeStringArrayValue(rez.Attributes, "memberof"),
                PrimaryGroupId = getAttributeStringValue(rez.Attributes, "primaryGroupID"),
                AltEmails = getAttributeStringArrayValue(rez.Attributes, "proxyAddresses")
            };
        }

        //public DomainUserGroupCollection GetMemberOfGroups(DomainUser user)
        //{
        //    if (isConnectionOK == false) return null;
        //    string gid = Helper.ConvertObjectGuidToString(user.ObjectGuid);

        //    SearchRequest sR = new SearchRequest(RootDN, "(&(objectClass=User)(objectGUID=" + gid + "))", System.DirectoryServices.Protocols.SearchScope.Subtree, );
        //    SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR);

        //    if (sResp.Entries[0].Attributes["memberOf"].Count > 0)
        //    {
        //        DomainUserGroupCollection list = new DomainUserGroupCollection();

        //        for (int i = 0; i < sResp.Entries[0].Attributes["memberOf"].Count; i++)
        //        {
        //            string memberOfName = (string)sResp.Entries[0].Attributes["memberOf"][i];
        //            SearchRequest sRloop = new SearchRequest(RootDN, "(&(objectClass=Group)(distinguishedName=" + memberOfName + "))", System.DirectoryServices.Protocols.SearchScope.Subtree);
        //            SearchResponse sRespLoop = (SearchResponse)ldapConn.SendRequest(sRloop);

        //            if (sRespLoop.Entries.Count > 0)
        //            {
        //                list.Add(new DomainUserGroup()
        //                {
        //                    Domain = GetGroupDomain(memberOfName),
        //                    GroupGuid = new Guid(getAttributeByteArrayValue(sRespLoop.Entries[0].Attributes, "objectGuid")),
        //                    Name = getAttributeStringValue(sRespLoop.Entries[0].Attributes, "name"),
        //                    DistinguishedName = getAttributeStringValue(sRespLoop.Entries[0].Attributes, "DistinguishedName"),
        //                    SamAccountName = getAttributeStringValue(sRespLoop.Entries[0].Attributes, "sAMAccountName"),
        //                    PrimaryGroupToken = ((sRespLoop.Entries[0].Attributes["objectSid"][0]).GetType() == typeof(byte[])) ? BitConverter.ToInt32(getAttributeByteArrayValue(sRespLoop.Entries[0].Attributes, "objectSid"), 24) : -1
        //                });
        //            }
        //        }

        //        return list;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public Dictionary<Guid, GroupListItemValue> GetUserGroupsById(Guid userId)
        {
            return GetUserGroupList(GetUser(userId));
        }

        public Dictionary<Guid, GroupListItemValue> GetUserGroupsByEmail(string emailAddress)
        {
            return GetUserGroupList(GetUserByEmail(emailAddress));
        }

        public Dictionary<Guid, GroupListItemValue> GetUserGroupsByPrincipalName(string userPrincipalName)
        {
            return GetUserGroupList(GetUserByPrincipalName(userPrincipalName));
        }

        protected Dictionary<Guid, GroupListItemValue> GetUserGroupList(DomainUser user)
        {
            Dictionary<Guid, GroupListItemValue> list = new Dictionary<Guid, GroupListItemValue>();

            if (user == null) return list;
            if (isConnectionOK == false) return list;

            List<string> selectedGroups = new List<string>();
            SearchRequest sR = null;
            SearchResponse sResp = null;
            SearchResultEntry entry = null;

            string groupDN = null;

            if ((user.MemberOfGroups != null) && (user.MemberOfGroups.Length > 0))
            {
                sR = new SearchRequest(RootDN, string.Format("(&(objectClass=group)(member:1.2.840.113556.1.4.1941:={0}))", user.DistinguishedName), System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "objectGuid", "name", "DistinguishedName", "sAMAccountName", "objectSid", "member", "memberOf" });
                sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

                if (sResp.Entries.Count > 0)
                {
                    for (int i = 0; i < sResp.Entries.Count; i++)
                    {
                        entry = sResp.Entries[i];
                        groupDN = getAttributeStringValue(entry.Attributes, "DistinguishedName");
                        AddLDAPGroupToLists(entry, user.MemberOfGroups.Contains(groupDN), ref selectedGroups, ref list);
                    }
                }
            }

            //string primayGroupOnjecySid = user.ObjectSid.Substring(0, user.ObjectSid.LastIndexOf('-') + 1) + user.PrimaryGroupId;

            //sR = new SearchRequest(RootDN, string.Format("(&(objectClass=group)(objectSid={0}))", primayGroupOnjecySid), System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "objectGuid", "name", "DistinguishedName", "sAMAccountName", "objectSid", "member", "memberOf" });
            //sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

            //if (sResp.Entries.Count > 0)
            //{
            //    entry = sResp.Entries[0];
            //    groupDN = getAttributeStringValue(entry.Attributes, "DistinguishedName");
            //    AddLDAPGroupToLists(entry, true, ref selectedGroups, ref list);

            //    sR = new SearchRequest(RootDN, string.Format("(&(objectClass=group)(member:1.2.840.113556.1.4.1941:={0}))", groupDN), System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "objectGuid", "name", "DistinguishedName", "sAMAccountName", "objectSid", "member", "memberOf" });
            //    sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

            //    if (sResp.Entries.Count > 0)
            //    {
            //        for (int i = 0; i < sResp.Entries.Count; i++)
            //        {
            //            entry = sResp.Entries[i];
            //            AddLDAPGroupToLists(entry, false, ref selectedGroups, ref list);
            //        }
            //    }
            //}

            return list;
        }

        private void AddLDAPGroupToLists(SearchResultEntry entry, bool isDirect, ref List<string> selectedGroups, ref Dictionary<Guid, GroupListItemValue> list)
        {
            string groupDN = null;
            string name = null;
            Guid objectGuid = Guid.Empty;

            groupDN = getAttributeStringValue(entry.Attributes, "DistinguishedName");
            name = getAttributeStringValue(entry.Attributes, "name");
            objectGuid = new Guid(getAttributeByteArrayValue(entry.Attributes, "objectGuid"));

            if (!selectedGroups.Contains(groupDN))
            {
                selectedGroups.Add(groupDN);
                list.Add(objectGuid, new GroupListItemValue(name, isDirect));
            }
        }

        //protected void GetRecursiveGroupList(string groupDN, ref Dictionary<Guid, GroupListItemValue> list, ref List<string> selectedGroups, bool isDirect)
        //{
        //    if (selectedGroups.Contains(groupDN))
        //        return;

        //    selectedGroups.Add(groupDN);

        //    SearchRequest sR = new SearchRequest(RootDN, "(&(objectClass=Group)(distinguishedName=" + groupDN + "))", System.DirectoryServices.Protocols.SearchScope.Subtree);
        //    SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR);

        //    if (sResp.Entries.Count > 0)
        //    {
        //        list.Add(new Guid(getAttributeByteArrayValue(sResp.Entries[0].Attributes, "objectGuid")), new GroupListItemValue(getAttributeStringValue(sResp.Entries[0].Attributes, "name"), isDirect));

        //        if (sResp.Entries[0].Attributes["memberOf"] != null)
        //        {
        //            for (int i = 0; i < sResp.Entries[0].Attributes["memberOf"].Count; i++)
        //            {
        //                GetRecursiveGroupList((string)sResp.Entries[0].Attributes["memberOf"][i], ref list, ref selectedGroups, false);
        //            }
        //        }
        //    }
        //}

        //protected void GetListOfGroupsByPrimaryGroupId(string groupId, ref Dictionary<Guid, GroupListItemValue> list, ref List<string> selectedGroups)
        //{
        //    SearchRequest sR = new SearchRequest(RootDN, "(&(objectClass=Group))", System.DirectoryServices.Protocols.SearchScope.Subtree);
        //    SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR);

        //    if (sResp.Entries.Count > 0)
        //    {
        //        foreach (SearchResultEntry resultEntry in sResp.Entries)
        //        {
        //            if ((resultEntry.Attributes["objectSid"][0]).GetType() == typeof(byte[]))
        //            {
        //                if (!selectedGroups.Contains(getAttributeStringValue(resultEntry.Attributes, "DistinguishedName")))
        //                {
        //                    if (groupId == BitConverter.ToInt32(getAttributeByteArrayValue(resultEntry.Attributes, "objectSid"), 24).ToString(CultureInfo.CurrentCulture))
        //                    {
        //                        GetRecursiveGroupList(getAttributeStringValue(resultEntry.Attributes, "DistinguishedName"), ref list, ref selectedGroups, true);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        public bool IsUserInGroup(string accountName, string groupName)
        {
            if (isConnectionOK == false) return false;
            if ((String.IsNullOrEmpty(accountName) == true) || (String.IsNullOrEmpty(groupName) == true)) return false;

            SearchRequest sR = new SearchRequest(RootDN, "(&(objectClass=User)(sAMAccountName=" + accountName + ")(memberOf=" + groupName + "))", System.DirectoryServices.Protocols.SearchScope.Subtree, new string[] { "sAMAccountName", "memberOf" });
            SearchResponse sResp = (SearchResponse)ldapConn.SendRequest(sR, new TimeSpan(1, 0, 0));

            if (sResp.Entries.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetDomainFullByUserDN(string userDN)
        {
            if (!string.IsNullOrEmpty(userDN))
            {
                StringBuilder sb = new StringBuilder();
                foreach (string dn in userDN.Split(','))
                {
                    string[] parts = dn.Split('=');
                    if (parts[0] == "DC") sb.AppendFormat(CultureInfo.CurrentCulture, ".{0}", parts[1]);
                }
                if (sb.Length > 0) sb.Remove(0, 1);

                return sb.ToString();
            }
            else
                return string.Empty;
        }

        #endregion

        #region Help functions

        /*        private void showAllAttributes(SearchResultAttributeCollection curAttr)
        {
            foreach (DictionaryEntry att in curAttr)
            {
                for (int i = 0; i < ((DirectoryAttribute)att.Value).Count; i++)
                {
                    if (((DirectoryAttribute)att.Value).Name == "objectSid")
                    {
                        if ((((DirectoryAttribute)att.Value)[i]).GetType() == typeof(byte[]))
                        {
                            SecurityIdentifier si = new SecurityIdentifier((byte[])(((DirectoryAttribute)att.Value)[i]), 0);
                            ApplicationLogger.LogInfo("LdapProvider", string.Format(CultureInfo.CurrentCulture, "{0} = {1}", ((DirectoryAttribute)att.Value).Name, si.ToString()));
                            ApplicationLogger.LogInfo("LdapProvider", string.Format(CultureInfo.CurrentCulture, "{0} = {1}", ((DirectoryAttribute)att.Value).Name, BitConverter.ToInt32((byte[])(((DirectoryAttribute)att.Value)[i]), 24)));
                        }
                        else
                            ApplicationLogger.LogInfo("LdapProvider", string.Format(CultureInfo.CurrentCulture, "{0} = {1}", ((DirectoryAttribute)att.Value).Name, ((DirectoryAttribute)att.Value)[i]));
                    }
                    else
                        ApplicationLogger.LogInfo("LdapProvider", string.Format(CultureInfo.CurrentCulture, "{0} = {1}", ((DirectoryAttribute)att.Value).Name, ((DirectoryAttribute)att.Value)[i]));
                }
            }
            ApplicationLogger.LogInfo("LdapProvider", "");
        } */

        private static string getAttributeStringValue(SearchResultAttributeCollection curAttr, string attrName)
        {
            if (curAttr[attrName] != null)
                return (string)curAttr[attrName][0];
            else
                return null;
        }

        private static string[] getAttributeStringArrayValue(SearchResultAttributeCollection curAttr, string attrName)
        {
            if (curAttr[attrName] != null)
                return (string[])curAttr[attrName].GetValues(typeof(string));
            else
                return null;
        }

        private static byte[] getAttributeByteArrayValue(SearchResultAttributeCollection curAttr, string attrName)
        {
            if (curAttr[attrName] != null)
            {
                return (byte[])curAttr[attrName][0];
            }
            else
            {
                return null;
            }
        }

        private static string getOUPath(string dnstring)
        {
            dnstring = Helper.ConvertDistinguishedNameToLdapPath(dnstring);
            string[] arr = dnstring.Split(',');
            string OUPath = "";
            for (int i = arr.Length - 1; i >= 0; i--)
            {
                string value = arr[i];
                if (arr[i].Split('=').Length > 1)
                    value = arr[i].Split('=')[1];

                if (i > 0)
                    OUPath += value + ".";
                else
                    OUPath += value;
            }

            return OUPath;
        }

        public static string ConvertDomainNameToLdapForm(string domainName)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string dc in domainName.Split('.'))
                sb.AppendFormat(CultureInfo.CurrentCulture, ",DC={0}", dc);

            if (sb.Length > 0) sb.Remove(0, 1);

            return sb.ToString();
        }

        private User convertDomainUserToUser(DomainUser ldapUser, string password)
        {
            return new User()
            {
                UserId = ldapUser.ObjectGuid,
                UserSid = ldapUser.ObjectSid,
                OrganizationId = ServerID,
                FirstName = ldapUser.FirstName,
                LastName = ldapUser.LastName,
                EmailAddress = ldapUser.EmailAddress,
                Password = password,
                LdapUserAlias = ldapUser.AccountName,
                LdapUserPrinciple = ldapUser.PrincipalName,
                LdapDomain = ldapUser.DomainName,
                LdapDomainFull = ldapUser.DomainFullName,
                LdapOUPath = getOUPath(ldapUser.DistinguishedName),
                LdapServerAddress = ServerAddress,
                LdapServerPort = Port,
                LdapServerUserName = ldapUser.AccountName,
                LdapServerPassword = password,
                IsActive = true
            };
        }

        public User convertDomainUserToUser(DomainUser ldapUser)
        {
            if (ldapUser != null)
            {
                return new User()
                {
                    UserId = ldapUser.ObjectGuid,
                    UserSid = ldapUser.ObjectSid,
                    OrganizationId = ServerID,
                    FirstName = ldapUser.FirstName,
                    LastName = ldapUser.LastName,
                    EmailAddress = ldapUser.EmailAddress,
                    LdapUserAlias = ldapUser.AccountName,
                    LdapUserPrinciple = ldapUser.PrincipalName,
                    LdapDomain = ldapUser.DomainName,
                    LdapDomainFull = ldapUser.DomainFullName,
                    LdapOUPath = getOUPath(ldapUser.DistinguishedName),
                    LdapServerAddress = ServerAddress,
                    LdapServerPort = Port,
                    LdapServerUserName = ldapUser.AccountName,
                    IsActive = true
                };
            }
            else
                return null;
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (ldapConn != null) ldapConn.Dispose();
                ldapConn = null;
            }
        }

        #endregion
    }
}
