﻿using System;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using Micajah.Common.Configuration;

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
                    Uri url = new Uri(new Uri(FrameworkConfiguration.Current.WebApplication.Url), ResourceProvider.OAuthHandlerVirtualPath);

                    s_Description = new ServiceProviderDescription
                    {
                        AccessTokenEndpoint = new MessageReceivingEndpoint(url, HttpDeliveryMethods.PostRequest | HttpDeliveryMethods.GetRequest),
                        RequestTokenEndpoint = new MessageReceivingEndpoint(url, HttpDeliveryMethods.PostRequest | HttpDeliveryMethods.GetRequest),
                        UserAuthorizationEndpoint = new MessageReceivingEndpoint(url, HttpDeliveryMethods.PostRequest | HttpDeliveryMethods.GetRequest),
                        TamperProtectionElements = new ITamperProtectionChannelBindingElement[] { new HmacSha1SigningBindingElement(), },
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