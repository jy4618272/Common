using System;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using Micajah.Common.Application;
using Micajah.Common.Bll.Providers.OAuth;

namespace Micajah.Common.Dal
{
    public partial class OAuthDataSet
    {
        partial class OAuthConsumerRow : IConsumerDescription
        {
            #region IConsumerDescription Members

            Uri IConsumerDescription.Callback
            {
                get { return string.IsNullOrEmpty(this.Callback) ? null : new Uri(this.Callback); }
            }

            System.Security.Cryptography.X509Certificates.X509Certificate2 IConsumerDescription.Certificate
            {
                get { return null; }
            }

            string IConsumerDescription.Key
            {
                get { return this.ConsumerKey; }
            }

            string IConsumerDescription.Secret
            {
                get { return this.ConsumerSecret; }
            }

            VerificationCodeFormat IConsumerDescription.VerificationCodeFormat
            {
                get { return (VerificationCodeFormat)this.VerificationCodeFormat; }
            }

            int IConsumerDescription.VerificationCodeLength
            {
                get { return this.VerificationCodeLength; }
            }

            #endregion
        }

        partial class OAuthTokenRow : IServiceProviderAccessToken, IServiceProviderRequestToken
        {
            #region IServiceProviderAccessToken Members

            DateTime? IServiceProviderAccessToken.ExpirationDate
            {
                get { return null; }
            }

            string[] IServiceProviderAccessToken.Roles
            {
                get { return this.Scope.Split('|'); }
            }

            string IServiceProviderAccessToken.Token
            {
                get { return this.Token; }
            }

            string IServiceProviderAccessToken.Username
            {
                get { return WebApplication.LoginProvider.GetLoginName(this.LoginId); }
            }

            #endregion

            #region IServiceProviderRequestToken Members

            Uri IServiceProviderRequestToken.Callback
            {
                get { return string.IsNullOrEmpty(this.RequestTokenCallback) ? null : new Uri(this.RequestTokenCallback); }
                set { this.RequestTokenCallback = value.AbsoluteUri; }
            }

            string IServiceProviderRequestToken.ConsumerKey
            {
                get { return TokenProvider.Current.GetConsumer(this.ConsumerId).Key; }
            }

            Version IServiceProviderRequestToken.ConsumerVersion
            {
                get { return new Version(this.ConsumerVersion); }
                set { this.ConsumerVersion = value.ToString(); }
            }

            DateTime IServiceProviderRequestToken.CreatedOn
            {
                get { return this.CreatedTime; }
            }

            string IServiceProviderRequestToken.Token
            {
                get { return this.Token; }
            }

            string IServiceProviderRequestToken.VerificationCode
            {
                get { return this.RequestTokenVerifier; }
                set { this.RequestTokenVerifier = value; }
            }

            #endregion
        }
    }
}
