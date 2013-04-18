using System;
using DotNetOpenAuth.OAuth.ChannelElements;

namespace Micajah.Common.Bll.Providers.OAuth
{
    public class TokenProvider : IServiceProviderTokenManager
    {
        #region Members

        private static TokenProvider s_Instance;
        private static TokenProvider s_Current;

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the instance of this class.
        /// </summary>
        internal static TokenProvider Instance
        {
            get
            {
                if (s_Instance == null) s_Instance = new TokenProvider();
                return s_Instance;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the instance of the class.
        /// </summary>
        public static TokenProvider Current
        {
            get { return ((s_Current == null) ? Instance : s_Current); }
            set { s_Current = value; }
        }

        #endregion

        #region Public Methods

        public IServiceProviderAccessToken GetAccessToken(string token)
        {
            throw new NotImplementedException();
        }

        public IConsumerDescription GetConsumer(string consumerKey)
        {
            throw new NotImplementedException();
        }

        public IServiceProviderRequestToken GetRequestToken(string token)
        {
            throw new NotImplementedException();
        }

        public bool IsRequestTokenAuthorized(string requestToken)
        {
            throw new NotImplementedException();
        }

        public void UpdateToken(IServiceProviderRequestToken token)
        {
            throw new NotImplementedException();
        }

        public void ExpireRequestTokenAndStoreNewAccessToken(string consumerKey, string requestToken, string accessToken, string accessTokenSecret)
        {
            throw new NotImplementedException();
        }

        public string GetTokenSecret(string token)
        {
            throw new NotImplementedException();
        }

        public TokenType GetTokenType(string token)
        {
            throw new NotImplementedException();
        }

        public void StoreNewRequestToken(DotNetOpenAuth.OAuth.Messages.UnauthorizedTokenRequest request, DotNetOpenAuth.OAuth.Messages.ITokenSecretContainingMessage response)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
