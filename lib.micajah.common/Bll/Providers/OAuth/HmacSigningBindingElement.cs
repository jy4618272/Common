using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth.ChannelElements;
using System;

namespace Micajah.Common.Bll.Providers.OAuth
{
    public class HmacSigningBindingElement : HmacSha1SigningBindingElement
    {
        #region Overriden Methods

        protected override bool IsSignatureValid(ITamperResistantOAuthMessage message)
        {
            message.Recipient = new Uri(new Uri(CustomUrlProvider.CreateApplicationUri(null)), message.Recipient.PathAndQuery);

            return base.IsSignatureValid(message);
        }

        protected override ITamperProtectionChannelBindingElement Clone()
        {
            return new HmacSigningBindingElement();
        }

        #endregion
    }
}
