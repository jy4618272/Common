using DotNetOpenAuth.OAuth.ChannelElements;
using System;

namespace Micajah.Common.Bll.Providers.OAuth
{
    public class RsaSigningBindingElement : RsaSha1ServiceProviderSigningBindingElement
    {
        #region Members

        private IServiceProviderTokenManager m_TokenManager;

        #endregion

        #region Constructors

        public RsaSigningBindingElement(IServiceProviderTokenManager tokenManager)
            : base(tokenManager)
        {
            m_TokenManager = tokenManager;
        }

        #endregion

        #region Overriden Methods

        protected override bool IsSignatureValid(ITamperResistantOAuthMessage message)
        {
            message.Recipient = new Uri(new Uri(CustomUrlProvider.CreateApplicationUri(null)), message.Recipient.PathAndQuery);

            return base.IsSignatureValid(message);
        }

        protected override DotNetOpenAuth.Messaging.ITamperProtectionChannelBindingElement Clone()
        {
            return new RsaSigningBindingElement(m_TokenManager);
        }

        #endregion
    }
}
