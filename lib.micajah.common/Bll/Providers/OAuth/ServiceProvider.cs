using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using System;

namespace Micajah.Common.Bll.Providers.OAuth
{
    public class ServiceProvider : DotNetOpenAuth.OAuth.ServiceProvider
    {
        #region Memebers

        private static ServiceProviderDescription s_Description;

        #endregion

        #region Private Properties

        private static ServiceProviderDescription Description
        {
            get
            {
                if (s_Description == null)
                {
                    Uri url = new Uri(new Uri(CustomUrlProvider.CreateApplicationUri(null)), CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.OAuthHandlerVirtualPath));

                    s_Description = new ServiceProviderDescription
                    {
                        AccessTokenEndpoint = new MessageReceivingEndpoint(url, HttpDeliveryMethods.PostRequest | HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
                        RequestTokenEndpoint = new MessageReceivingEndpoint(url, HttpDeliveryMethods.PostRequest | HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
                        UserAuthorizationEndpoint = new MessageReceivingEndpoint(url, HttpDeliveryMethods.PostRequest | HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
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
    }
}
