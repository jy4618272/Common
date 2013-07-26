using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using DotNetOpenAuth.OAuth.Messages;
using Micajah.Common.Application;
using System;
using System.Web;

namespace Micajah.Common.Bll.Providers.OAuth
{
    public class ServiceProvider : DotNetOpenAuth.OAuth.ServiceProvider
    {
        #region Memebers

        private const string OAuthPendingUserAuthorizationRequestKey = "OAuthPendingUserAuthorizationRequest";

        private static ServiceProviderDescription s_Description;

        #endregion

        #region Private Properties

        private static ServiceProviderDescription Description
        {
            get
            {
                if (s_Description == null)
                {
                    Uri baseUri = new Uri(CustomUrlProvider.CreateApplicationUri(null));
                    s_Description = new ServiceProviderDescription
                    {
                        RequestTokenEndpoint = new MessageReceivingEndpoint(new Uri(baseUri, CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.OAuthHandlerVirtualPath)), HttpDeliveryMethods.PostRequest | HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
                        UserAuthorizationEndpoint = new MessageReceivingEndpoint(new Uri(baseUri, CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.OAuthHandlerVirtualPath)), HttpDeliveryMethods.PostRequest | HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
                        AccessTokenEndpoint = new MessageReceivingEndpoint(new Uri(baseUri, CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.OAuthHandlerVirtualPath)), HttpDeliveryMethods.PostRequest | HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
                        TamperProtectionElements = new ITamperProtectionChannelBindingElement[] { new HmacSigningBindingElement(), new RsaSigningBindingElement(TokenProvider.Current) },
                    };
                }
                return s_Description;
            }
        }

        #endregion

        #region Constructors

        public ServiceProvider()
            : base(Description, TokenProvider.Current, NonceProvider.Current, new OAuthServiceProviderMessageFactory(TokenProvider.Current))
        {
        }

        #endregion

        #region Internal Methods

        internal static void SetOAuthPendingUserAuthorizationRequest(UserAuthorizationRequest requestAuth, Guid userId)
        {
            if (requestAuth == null)
                CacheManager.Current.Remove(OAuthPendingUserAuthorizationRequestKey + userId.ToString("N"));
            else
                CacheManager.Current.Add(OAuthPendingUserAuthorizationRequestKey + userId.ToString("N"), requestAuth, null, DateTime.UtcNow.AddMinutes(2), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
        }

        internal static UserAuthorizationRequest GetOAuthPendingUserAuthorizationRequest(Guid userId)
        {
            return CacheManager.Current[OAuthPendingUserAuthorizationRequestKey + userId.ToString("N")] as UserAuthorizationRequest;
        }

        #endregion
    }
}
