using Google.GData.Apps;
using Google.GData.Client;
using Micajah.Common.Application;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Micajah.Common.Bll.Providers
{
    public static class GoogleProvider
    {
        #region Constants

        private const string EmailScope = "email";
        private const string ProfileScope = "profile";
        private const string UsersScope = "https://apps-apis.google.com/a/feeds/user/";

        #endregion

        #region Private Methods

        private static string CreateGoogleProvderRequestUrl(string url)
        {
            return url.Split('?')[0] + "?provider=google";
        }

        private static OAuth2Parameters CreateAuthorizationParameters(string scope)
        {
            GoogleIntegrationElement settings = FrameworkConfiguration.Current.WebApplication.Integration.Google;
            return new OAuth2Parameters()
            {
                ClientId = settings.ClientId,
                ClientSecret = settings.ClientSecret,
                Scope = "openid " + scope
            };
        }

        private static string CreateAuthorizationUrl(string returnUrl, string scope, string state, string accessType)
        {
            OAuth2Parameters parameters = CreateAuthorizationParameters(scope);
            parameters.RedirectUri = returnUrl;

            if (!string.IsNullOrEmpty(accessType))
            {
                parameters.AccessType = accessType;
            }

            if (!string.IsNullOrEmpty(state))
            {
                parameters.State = System.Web.HttpUtility.UrlEncodeUnicode(state);
            }

            string url = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);

            return url;
        }

        private static OAuth2Parameters GetAccessToken(HttpContext context, string scope)
        {
            string url = context.Request.Url.ToString();
            string urlWithProvider = CreateGoogleProvderRequestUrl(url);
            string accessCode = GetAccessCode(context.Request);

            OAuth2Parameters parameters = CreateAuthorizationParameters(scope);
            parameters.RedirectUri = urlWithProvider;
            parameters.AccessCode = accessCode;

            OAuthUtil.GetAccessToken(parameters);

            return parameters;
        }

        private static string GetRequestValue(HttpRequest request, string name)
        {
            string value = null;
            if (request != null)
            {
                value = request.QueryString[name];
                if (value == null)
                    value = request.Form[name];
            }
            return value;
        }

        private static string[] ParseAuthorizationRequestState(HttpRequest request)
        {
            string[] parts = null;

            string state = GetRequestValue(request, "state");
            if (!string.IsNullOrEmpty(state))
            {
                parts = System.Web.HttpUtility.UrlDecode(state).Split('|');
            }

            return parts;
        }

        private static void RedirectToProvider(HttpContext context, string scope, string state, string accessType)
        {
            string url = context.Request.Url.ToString();
            string urlWithProvider = CreateGoogleProvderRequestUrl(url);
            string authUrl = CreateAuthorizationUrl(urlWithProvider, scope, state, accessType);

            context.Response.Redirect(authUrl);
        }

        #endregion

        #region Internal Methods

        internal static string GetLoginUrl(Guid organizationId, Guid instanceId)
        {
            string url = WebApplication.LoginProvider.GetLoginUrl(null, null, organizationId, instanceId, null, CustomUrlProvider.GetVanityUri(Guid.Empty, Guid.Empty));
            if (url.IndexOf("?", StringComparison.OrdinalIgnoreCase) > -1)
            {
                url += "&provider=google";
            }
            else
            {
                url += "?provider=google";
            }
            return url;
        }

        internal static string GetLoginUrl(string domain)
        {
            return WebApplication.LoginProvider.GetLoginUrl(false) + "?provider=google" + (string.IsNullOrEmpty(domain) ? string.Empty : "&domain=" + domain);
        }

        internal static void ParseAuthorizationRequestState(HttpRequest request, ref string domain, ref string returnUrl)
        {
            string[] parts = ParseAuthorizationRequestState(request);
            int length = parts.Length;

            if (length > 0)
            {
                domain = parts[0];

                if (length > 1)
                {
                    returnUrl = parts[1];
                }
            }
        }

        #endregion

        #region Public Methods

        public static bool IsGoogleProviderRequest(HttpRequest request)
        {
            if (request != null)
            {
                string provider = GetProviderName(request);
                if (!string.IsNullOrEmpty(provider))
                {
                    if ((string.Compare(provider, "google", StringComparison.OrdinalIgnoreCase) == 0) && FrameworkConfiguration.Current.WebApplication.Integration.Google.Enabled)
                        return true;
                }
            }
            return false;
        }

        public static bool IsGoogleProviderRequest(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string[] parts = url.Split('?');
                if (parts.Length > 1)
                {
                    NameValueCollection query = System.Web.HttpUtility.ParseQueryString(parts[1]);

                    string provider = query["provider"];
                    if (!string.IsNullOrEmpty(provider))
                    {
                        if ((string.Compare(provider, "google", StringComparison.OrdinalIgnoreCase) == 0) && FrameworkConfiguration.Current.WebApplication.Integration.Google.Enabled)
                            return true;
                    }
                }
            }
            return false;
        }

        public static string ProcessAuthorization(HttpContext context, ref Guid organizationId, ref Guid instanceId)
        {
            string accessCode = GetAccessCode(context.Request);

            if (string.IsNullOrEmpty(accessCode))
            {
                string error = GetError(context.Request);
                if (string.IsNullOrEmpty(error))
                {
                    string state = null;
                    if (organizationId != Guid.Empty)
                    {
                        if (instanceId == Guid.Empty)
                        {
                            state = string.Format(CultureInfo.InvariantCulture, "{0:N}", organizationId);
                        }
                        else
                        {
                            state = string.Format(CultureInfo.InvariantCulture, "{0:N}|{1:N}", organizationId, instanceId);
                        }
                    }

                    RedirectToProvider(context, EmailScope, state, "online");
                }
                else
                {
                    string errorMessage = string.Format(CultureInfo.InvariantCulture, Resources.GoogleProvider_AuthenticationFailedWithError, error);

                    throw new System.Security.Authentication.AuthenticationException(errorMessage);
                }
            }
            else
            {
                try
                {
                    OAuth2Parameters parameters = GetAccessToken(context, EmailScope);

                    string[] parts = ParseAuthorizationRequestState(context.Request);
                    if (parts != null)
                    {
                        int length = parts.Length;
                        if (length > 0)
                        {
                            object obj = Support.ConvertStringToType(parts[0], typeof(Guid));
                            if (obj != null)
                            {
                                organizationId = (Guid)obj;
                            }

                            if (length > 1)
                            {
                                obj = Support.ConvertStringToType(parts[1], typeof(Guid));
                                if (obj != null)
                                {
                                    instanceId = (Guid)obj;
                                }
                            }
                        }
                    }

                    return parameters.AccessToken;
                }
                catch (WebException ex) // (400) Bad Request.
                {
                    throw new System.Security.Authentication.AuthenticationException(Resources.GoogleProvider_AuthenticationFailed, ex);
                }
            }

            return null;
        }

        public static void ProcessAuthorization(HttpContext context, ref OAuth2Parameters parameters, ref string returnUrl)
        {
            string domain = GetDomain(context.Request);
            string accessCode = GetAccessCode(context.Request);
            returnUrl = GetReturnUrl(context.Request);

            if (string.IsNullOrEmpty(domain))
            {
                ParseAuthorizationRequestState(context.Request, ref domain, ref returnUrl);
            }

            Guid organizationId = EmailSuffixProvider.GetOrganizationId(domain);

            if (string.IsNullOrEmpty(accessCode))
            {
                string error = GetError(context.Request);
                if (string.IsNullOrEmpty(error))
                {
                    if (organizationId == Guid.Empty)
                    {
                        string state = string.Join("|", domain, returnUrl);

                        RedirectToProvider(context, string.Join(" ", EmailScope, ProfileScope, UsersScope), state, "offline");
                    }
                    else // It means the organization is already registered.
                    {
                        if (string.IsNullOrEmpty(returnUrl))
                        {
                            returnUrl = GetLoginUrl(domain);
                        }

                        context.Response.Redirect(returnUrl);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        returnUrl = "javascript:window.history.back();";
                    }

                    string errorMessage = string.Format(CultureInfo.InvariantCulture, Resources.GoogleProvider_ImportFailed, error);

                    throw new System.Security.Authentication.AuthenticationException(errorMessage);
                }
            }
            else
            {
                if (parameters == null)
                {
                    parameters = GetAccessToken(context, string.Join(" ", EmailScope, ProfileScope, UsersScope));
                }

                if (organizationId != Guid.Empty)
                {
                    UpdateRefreshToken(organizationId, parameters.RefreshToken);

                    int totalcount = 0;
                    int failedCount = 0;

                    ImportUsers(organizationId, domain, parameters, out totalcount, out failedCount);

                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        returnUrl = GetLoginUrl(domain);
                    }

                    context.Response.Redirect(returnUrl);
                }
            }
        }

        public static string GetReturnUrl(HttpRequest request)
        {
            return GetRequestValue(request, "callback");
        }

        public static string GetDomain(HttpRequest request)
        {
            return GetRequestValue(request, "domain");
        }

        public static string GetError(HttpRequest request)
        {
            return GetRequestValue(request, "error");
        }

        public static string GetProviderName(HttpRequest request)
        {
            return GetRequestValue(request, "provider");
        }

        public static string GetAccessCode(HttpRequest request)
        {
            return GetRequestValue(request, "code");
        }

        public static void GetUserProfile(string accessToken, out string email, out string firstName, out string lastName)
        {
            email = null;
            firstName = null;
            lastName = null;

            HttpClient client = new HttpClient();
            try
            {
                string json = client.GetStringAsync("https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + accessToken).Result;

                JObject obj = JObject.Parse(json);

                object value = obj["email"];
                if (value != null)
                {
                    email = value.ToString();
                }

                value = obj["given_name"];
                if (value != null)
                {
                    firstName = value.ToString();
                }

                value = obj["family_name"];
                if (value != null)
                {
                    lastName = value.ToString();
                }
            }
            catch (WebException) { } // 401 (Unauthorized).
            catch (HttpRequestException) { } // 401 (Unauthorized).
            finally
            {
                if (client != null)
                {
                    client.Dispose();
                }
            }
        }

        public static AppsService CreateAppsService(string domain, OAuth2Parameters parameters)
        {
            GOAuth2RequestFactory factory = new GOAuth2RequestFactory("apps", FrameworkConfiguration.Current.WebApplication.Integration.Google.ApplicationName, parameters);
            AppsService service = new AppsService(domain, parameters.AccessToken);
            service.SetRequestFactory(factory);
            return service;
        }

        public static void ImportUsers(Guid organizationId, string domain, OAuth2Parameters parameters, out int totalCount, out int failedCount)
        {
            totalCount = 0;
            failedCount = 0;

            AppsService service = CreateAppsService(domain, parameters);

            ImportUsers(organizationId, service, out totalCount, out failedCount);
        }

        public static void ImportUsers(Guid organizationId, AppsService service, out int totalCount, out int failedCount)
        {
            totalCount = 0;
            failedCount = 0;

            if (service == null) return;

            UserFeed userFeed = service.RetrieveAllUsers();
            if (userFeed == null) return;

            totalCount = userFeed.Entries.Count;
            for (int i = 0; i < totalCount; i++)
            {
                try
                {
                    UserEntry userEntry = userFeed.Entries[i] as UserEntry;
                    if (userEntry != null)
                    {
                        Guid instanceId = InstanceProvider.GetFirstInstanceId(organizationId);
                        SortedList list = GroupProvider.GetGroupIdRoleIdList(organizationId, instanceId);
                        Guid groupId = GroupProvider.GetGroupIdOfLowestRoleInInstance(list);

                        string groups = groupId.ToString();

                        if (userEntry.Login.Admin)
                        {
                            groups = Guid.Empty.ToString();

                            System.Collections.IList roleIdList = list.GetValueList();
                            if (roleIdList != null)
                            {
                                Guid roleId = RoleProvider.GetHighestNonBuiltInRoleId(roleIdList);
                                if (roleId != Guid.Empty)
                                {
                                    int idx = list.IndexOfValue(roleId);
                                    if (idx > -1)
                                    {
                                        groupId = (Guid)list.GetKey(idx);
                                        groups = string.Format("{0}, {1}", groups, groupId.ToString());
                                    }
                                }

                                roleId = RoleProvider.GetHighestBuiltInRoleId(roleIdList);
                                if (roleId != Guid.Empty)
                                {
                                    int idx = list.IndexOfValue(roleId);
                                    if (idx > -1)
                                    {
                                        groupId = (Guid)list.GetKey(idx);
                                        groups = string.Format("{0}, {1}", groups, groupId.ToString());
                                    }
                                }
                            }
                        }

                        Guid loginId = UserProvider.AddUserToOrganization(string.Format("{0}@{1}", userEntry.Login.UserName, service.Domain), userEntry.Name.GivenName, userEntry.Name.FamilyName, null
                            , null, null, null, null, null
                            , null, null, null, null, null, null
                            , groups, organizationId
                            , WebApplication.LoginProvider.GeneratePassword(), false, true);

                        UserProvider.RaiseUserInserted(loginId, organizationId, null, Support.ConvertStringToGuidList(groups));
                    }
                }
                catch
                {
                    failedCount++;
                }
            }
        }

        public static void UpdateRefreshToken(Guid organizationId, string refreshToken)
        {
            if (!string.IsNullOrEmpty(refreshToken))
            {
                Setting setting = SettingProvider.GetSettingByShortName("RefreshToken");
                if (setting != null)
                {
                    setting.Value = refreshToken;
                    SettingProvider.UpdateSettingValue(setting, organizationId, null, null);
                }
            }
        }

        #endregion
    }
}
