using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Web;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;
using Google.GData.Apps;
using Google.GData.Client;
using Micajah.Common.Application;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;
using Newtonsoft.Json.Linq;

namespace Micajah.Common.Bll.Providers
{
    public static class GoogleProvider
    {
        #region Private Methods

        private static OpenIdRelyingParty CreateRelyingParty()
        {
            HostMetaDiscoveryService googleAppsDiscovery = new HostMetaDiscoveryService
            {
                UseGoogleHostedHostMeta = true
            };

            OpenIdRelyingParty relyingParty = new OpenIdRelyingParty();
            relyingParty.DiscoveryServices.Insert(0, googleAppsDiscovery);

            return relyingParty;
        }

        private static string GetRequestValue(HttpRequest request, string name)
        {
            string value = request.QueryString[name];
            if (value == null)
                value = request.Form[name];
            return value;
        }

        #endregion

        #region Internal Methods

        internal static OAuth2Parameters CreateOAuth2Parameters(string returnUrl, string accessCode)
        {
            OAuth2Parameters parameters = CreateOAuth2Parameters();

            parameters.RedirectUri = returnUrl;
            parameters.AccessCode = accessCode;

            OAuthUtil.GetAccessToken(parameters);

            return parameters;
        }

        internal static string GetLoginUrl()
        {
            return GetLoginUrl(null);
        }

        internal static string GetLoginUrl(string domain)
        {
            return WebApplication.LoginProvider.GetLoginUrl(false) + "?provider=google" + (string.IsNullOrEmpty(domain) ? string.Empty : "&domain=" + domain);
        }

        internal static string CreateGoogleProvderRequestUrl(string url)
        {
            return url.Split('?')[0] + "?provider=google";
        }

        internal static string CreateGoogleProvderRequestUrl(string url, string domain, string returnUrl)
        {
            string url2 = CreateGoogleProvderRequestUrl(url);
            if (!string.IsNullOrEmpty(domain))
                url2 += "&domain=" + domain;
            if (!string.IsNullOrEmpty(returnUrl))
                url2 += "&callback=" + returnUrl;
            return url2;
        }

        internal static string CreateOAuth2AuthorizationRequestState(string domain, string returnUrl)
        {
            return string.Join("|", domain, returnUrl);
        }

        internal static void ParseOAuth2AuthorizationRequestState(HttpRequest request, ref string domain, ref string returnUrl)
        {
            string state = GetRequestValue(request, "state");
            if (!string.IsNullOrEmpty(state))
            {
                string[] parts = System.Web.HttpUtility.UrlDecode(state).Split('|');
                domain = parts[0];
                if (parts.Length > 1)
                    returnUrl = parts[1];
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

        public static string ProcessOpenIdAuthenticationRequest(HttpContext context)
        {
            OpenIdRelyingParty relyingParty = null;
            Exception exception = null;

            try
            {
                relyingParty = CreateRelyingParty();

                IAuthenticationResponse authResponse = relyingParty.GetResponse(new HttpRequestWrapper(context.Request));
                if (authResponse == null)
                {
                    string domain = GetDomain(context.Request);
                    Identifier id = null;
                    if (string.IsNullOrEmpty(domain) || (!Identifier.TryParse(domain, out id)))
                        id = Identifier.Parse(FrameworkConfiguration.Current.WebApplication.Integration.Google.OpenIdProviderEndpointAddress);

                    IAuthenticationRequest request = relyingParty.CreateRequest(id);

                    // Request access to e-mail address via OpenID Attribute Exchange (AX).
                    FetchRequest fetch = new FetchRequest();
                    fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Contact.Email, true));
                    request.AddExtension(fetch);

                    // Send a visitor to their Provider for the authentication.
                    request.RedirectToProvider();
                }
                else
                {
                    if (authResponse.Status == AuthenticationStatus.Authenticated)
                    {
                        FetchResponse fetch = authResponse.GetExtension<FetchResponse>();
                        if (fetch != null)
                            return fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);
                    }
                    else if (authResponse.Status == AuthenticationStatus.Canceled)
                        exception = new System.Security.Authentication.AuthenticationException(Resources.GoogleProvider_AuthenticationCancelled);
                    else
                        exception = new System.Security.Authentication.AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.GoogleProvider_AuthenticationFailedWithStatus, authResponse.Status));
                }
            }
            catch (Exception ex)
            {
                exception = new System.Security.Authentication.AuthenticationException(Resources.GoogleProvider_AuthenticationFailed, ex);
            }
            finally
            {
                if (relyingParty != null) relyingParty.Dispose();
            }

            if (exception != null)
                throw exception;

            return null;
        }

        public static void ProcessOAuth2Authorization(HttpContext context, ref OAuth2Parameters parameters, ref string returnUrl)
        {
            string domain = GetDomain(context.Request);
            string accessCode = GetOAuth2AccessCode(context.Request);
            returnUrl = GetReturnUrl(context.Request);

            if (string.IsNullOrEmpty(domain))
                GoogleProvider.ParseOAuth2AuthorizationRequestState(context.Request, ref domain, ref returnUrl);

            Guid organizationId = EmailSuffixProvider.GetOrganizationId(domain);

            if (string.IsNullOrEmpty(accessCode))
            {
                string error = GetError(context.Request);
                if (string.IsNullOrEmpty(error))
                {
                    if (organizationId == Guid.Empty)
                    {
                        // OAuth2 authorization request.
                        context.Response.Redirect(CreateOAuth2AuthorizationUrl(CreateGoogleProvderRequestUrl(context.Request.Url.ToString())
                            , CreateOAuth2AuthorizationRequestState(GetDomain(context.Request), GetReturnUrl(context.Request))));
                    }
                    else // It means the organization is already registered.
                    {
                        if (string.IsNullOrEmpty(returnUrl))
                            returnUrl = GetLoginUrl(domain);

                        context.Response.Redirect(returnUrl);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(returnUrl))
                        returnUrl = "javascript:window.history.back();";

                    throw new System.Security.Authentication.AuthenticationException(GetErrorMessage(error));
                }
            }
            else
            {
                if (parameters == null)
                    parameters = CreateOAuth2Parameters(CreateGoogleProvderRequestUrl(context.Request.Url.ToString()), accessCode);

                if (organizationId != Guid.Empty)
                {
                    UpdateOAuth2RefreshToken(organizationId, parameters.RefreshToken);

                    ImportUsers(organizationId, domain, parameters);

                    if (string.IsNullOrEmpty(returnUrl))
                        returnUrl = GetLoginUrl(domain);

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

        public static string GetErrorMessage(string error)
        {
            return string.Format(CultureInfo.InvariantCulture, Resources.GoogleProvider_ImportFailed, error);
        }

        public static string GetProviderName(HttpRequest request)
        {
            return GetRequestValue(request, "provider");
        }

        public static string GetOAuth2AccessCode(HttpRequest request)
        {
            return GetRequestValue(request, "code");
        }

        public static void GetUserProfile(string accessToken, out string email, out string firstName, out string lastName, out string timeZone)
        {
            email = null;
            firstName = null;
            lastName = null;
            timeZone = null;

            HttpClient client = new HttpClient();
            try
            {
                string json = client.GetStringAsync("https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + accessToken).Result;

                JObject obj = JObject.Parse(json);
                email = obj["email"].ToString();
                firstName = obj["given_name"].ToString();
                lastName = obj["family_name"].ToString();
            }
            catch (WebException) { } // 401 (Unauthorized).
            catch (HttpRequestException) { } // 401 (Unauthorized).
            finally
            {
                if (client != null) client.Dispose();
            }
        }

        public static OAuth2Parameters CreateOAuth2Parameters()
        {
            GoogleIntegrationElement settings = FrameworkConfiguration.Current.WebApplication.Integration.Google;
            return new OAuth2Parameters() { ClientId = settings.ClientId, ClientSecret = settings.ClientSecret, Scope = settings.Scope };
        }

        public static string CreateOAuth2AuthorizationUrl(string returnUrl, string state)
        {
            OAuth2Parameters parameters = CreateOAuth2Parameters();

            parameters.RedirectUri = returnUrl;
            parameters.State = System.Web.HttpUtility.UrlEncodeUnicode(state);

            return OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);
        }

        public static AppsService CreateAppsService(string domain, OAuth2Parameters parameters)
        {
            GOAuth2RequestFactory factory = new GOAuth2RequestFactory("apps", FrameworkConfiguration.Current.WebApplication.Integration.Google.ApplicationName, parameters);
            AppsService service = new AppsService(domain, parameters.AccessToken);
            service.SetRequestFactory(factory);
            return service;
        }

        public static void ImportUsers(Guid organizationId, string domain, OAuth2Parameters parameters)
        {
            AppsService service = CreateAppsService(domain, parameters);
            if (service != null)
            {
                UserFeed userFeed = service.RetrieveAllUsers();
                if (userFeed != null)
                {
                    if (userFeed.Entries.Count > 0)
                    {
                        for (int i = 0; i < userFeed.Entries.Count; i++)
                        {
                            UserEntry userEntry = userFeed.Entries[i] as UserEntry;
                            if (userEntry != null)
                            {
                                Guid instanceId = InstanceProvider.GetFirstInstanceId(organizationId);
                                Guid groupId = GroupProvider.GetGroupIdOfLowestRoleInInstance(organizationId, instanceId);

                                string groups = groupId.ToString();

                                if (userEntry.Login.Admin)
                                {
                                    groups = Guid.Empty.ToString();

                                    Bll.Instance inst = new Bll.Instance();
                                    if (inst.Load(organizationId, instanceId))
                                    {
                                        System.Collections.IList roleIdList = inst.GroupIdRoleIdList.GetValueList();
                                        if (roleIdList != null)
                                        {
                                            Guid roleId = RoleProvider.GetHighestNonBuiltInRoleId(roleIdList);
                                            if (roleId != Guid.Empty)
                                            {
                                                int idx = inst.GroupIdRoleIdList.IndexOfValue(roleId);
                                                if (idx > -1)
                                                {
                                                    groupId = (Guid)inst.GroupIdRoleIdList.GetKey(idx);
                                                    groups = string.Format("{0}, {1}", groups, groupId.ToString());
                                                }
                                            }

                                            roleId = RoleProvider.GetHighestBuiltInRoleId(roleIdList);
                                            if (roleId != Guid.Empty)
                                            {
                                                int idx = inst.GroupIdRoleIdList.IndexOfValue(roleId);
                                                if (idx > -1)
                                                {
                                                    groupId = (Guid)inst.GroupIdRoleIdList.GetKey(idx);
                                                    groups = string.Format("{0}, {1}", groups, groupId.ToString());
                                                }
                                            }
                                        }
                                    }
                                }

                                Guid loginId = UserProvider.AddUserToOrganization(string.Format("{0}@{1}", userEntry.Login.UserName, domain), userEntry.Name.GivenName, userEntry.Name.FamilyName, null
                                , null, null, null, null, null
                                , null, null, null, null, null, null
                                , groups, organizationId
                                , Micajah.Common.Application.WebApplication.LoginProvider.GeneratePassword(), false, true);

                                UserProvider.RaiseUserInserted(loginId, organizationId, null, Bll.Support.ConvertStringToGuidList(groups));
                            }
                        }
                    }
                }
            }
        }

        public static void UpdateOAuth2RefreshToken(Guid organizationId, string refreshToken)
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
