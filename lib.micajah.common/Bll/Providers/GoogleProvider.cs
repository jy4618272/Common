using System;
using System.Globalization;
using System.Security.Authentication;
using System.Web;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;

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

        #endregion

        #region Public Methods

        public static bool IsGoogleProviderRequest(HttpRequest request)
        {
            string provider = GetProviderName(request);
            if (!string.IsNullOrEmpty(provider))
            {
                if ((string.Compare(provider, "google", StringComparison.OrdinalIgnoreCase) == 0) && FrameworkConfiguration.Current.WebApplication.Integration.Google.Enabled)
                    return true;
            }
            return false;
        }

        public static string ProcessAuthenticationRequest(HttpContext context)
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
                        exception = new AuthenticationException(Resources.GoogleProvider_AuthenticationCancelled);
                    else
                        exception = new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.GoogleProvider_AuthenticationFailedWithStatus, authResponse.Status));
                }
            }
            catch (Exception ex)
            {
                exception = new AuthenticationException(Resources.GoogleProvider_AuthenticationFailed, ex);
            }
            finally
            {
                if (relyingParty != null) relyingParty.Dispose();
            }

            if (exception != null)
                throw exception;

            return null;
        }

        public static string GetReturnUrl(HttpRequest request)
        {
            return request.QueryString["callback"];
        }

        public static string GetDomain(HttpRequest request)
        {
            return request.QueryString["domain"];
        }

        public static string GetProviderName(HttpRequest request)
        {
            return request.QueryString["provider"];
        }

        #endregion
    }
}
