using System;
using System.Security.Cryptography;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth.ChannelElements;
using DotNetOpenAuth.OAuth.Messages;
using Micajah.Common.Application;
using Micajah.Common.Dal;
using System.Text;

namespace Micajah.Common.Bll.Providers.OAuth
{
    public class TokenProvider : IServiceProviderTokenManager
    {
        #region Members

        private static TokenProvider s_Instance;
        private static TokenProvider s_Current;
        private static readonly RandomNumberGenerator s_CryptoRandomDataGenerator = new RNGCryptoServiceProvider();

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

        #region Private Methods

        private Guid GetConsumerId(string consumerKey)
        {
            return ((OAuthDataSet.OAuthConsumerRow)GetConsumer(consumerKey)).ConsumerId;
        }

        #endregion

        #region Public Methods

        public IConsumerDescription GetConsumer(Guid consumerId)
        {
            OAuthDataSet.OAuthConsumerDataTable table = new OAuthDataSet.OAuthConsumerDataTable();
            WebApplication.CommonDataSetTableAdapters.OAuthConsumerTableAdapter.Fill(table, 0, consumerId);
            return ((table.Count > 0) ? table[0] : null);
        }

        public IConsumerDescription GetConsumer(string consumerKey)
        {
            OAuthDataSet.OAuthConsumerDataTable table = new OAuthDataSet.OAuthConsumerDataTable();
            WebApplication.CommonDataSetTableAdapters.OAuthConsumerTableAdapter.Fill(table, 1, consumerKey);
            return ((table.Count > 0) ? table[0] : null);
        }

        public IServiceProviderAccessToken GetAccessToken(string token)
        {
            OAuthDataSet.OAuthTokenDataTable table = new OAuthDataSet.OAuthTokenDataTable();
            WebApplication.CommonDataSetTableAdapters.OAuthTokenTableAdapter.Fill(table, 0, token);
            if (table.Count > 0)
            {
                OAuthDataSet.OAuthTokenRow row = table[0];
                if (row.TokenTypeId == (int)OAuthTokenType.AccessToken)
                    return row;
            }
            return null;
        }

        public IServiceProviderRequestToken GetRequestToken(string token)
        {
            OAuthDataSet.OAuthTokenDataTable table = new OAuthDataSet.OAuthTokenDataTable();
            WebApplication.CommonDataSetTableAdapters.OAuthTokenTableAdapter.Fill(table, 0, token);
            if (table.Count > 0)
            {
                OAuthDataSet.OAuthTokenRow row = table[0];
                if (row.TokenTypeId != (int)OAuthTokenType.AccessToken)
                    return row;
            }
            return null;
        }

        public bool IsRequestTokenAuthorized(string requestToken)
        {
            OAuthDataSet.OAuthTokenDataTable table = new OAuthDataSet.OAuthTokenDataTable();
            WebApplication.CommonDataSetTableAdapters.OAuthTokenTableAdapter.Fill(table, 0, requestToken);
            if (table.Count > 0)
            {
                OAuthDataSet.OAuthTokenRow row = table[0];
                if (row.TokenTypeId == (int)OAuthTokenType.AuthorizedRequestToken)
                    return true;
            }
            return false;
        }

        public string GetTokenSecret(string token)
        {
            OAuthDataSet.OAuthTokenDataTable table = new OAuthDataSet.OAuthTokenDataTable();
            WebApplication.CommonDataSetTableAdapters.OAuthTokenTableAdapter.Fill(table, 0, token);
            return ((table.Count > 0) ? table[0].TokenSecret : null);
        }

        public TokenType GetTokenType(string token)
        {
            OAuthDataSet.OAuthTokenDataTable table = new OAuthDataSet.OAuthTokenDataTable();
            WebApplication.CommonDataSetTableAdapters.OAuthTokenTableAdapter.Fill(table, 0, token);
            if (table.Count > 0)
                return ((table[0].TokenTypeId == (int)OAuthTokenType.AccessToken) ? TokenType.AccessToken : TokenType.RequestToken);
            return TokenType.InvalidToken;
        }

        public void StoreNewRequestToken(UnauthorizedTokenRequest request, ITokenSecretContainingMessage response)
        {
            // ((RequestScopedTokenMessage)request).Scope
            string scope = null;// request.ExtraData["Scope"];
            if (scope == null) scope = string.Empty;

            string callback = string.Empty;
            if (request.Callback != null)
                callback = request.Callback.AbsoluteUri;

            WebApplication.CommonDataSetTableAdapters.OAuthTokenTableAdapter.Insert(Guid.NewGuid(), response.Token, response.TokenSecret, (int)OAuthTokenType.UnauthorizedRequestToken
                , GetConsumerId(request.ConsumerKey), ((IMessage)request).Version.ToString(), scope, null, string.Empty, callback, DateTime.UtcNow);
        }

        public void ExpireRequestTokenAndStoreNewAccessToken(string consumerKey, string requestToken, string accessToken, string accessTokenSecret)
        {
            OAuthDataSet.OAuthTokenDataTable table = new OAuthDataSet.OAuthTokenDataTable();
            WebApplication.CommonDataSetTableAdapters.OAuthTokenTableAdapter.Fill(table, 0, requestToken);
            if (table.Count > 0)
            {
                OAuthDataSet.OAuthTokenRow row = table[0];
                if (string.Compare(consumerKey, GetConsumer(row.ConsumerId).Key, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    row.CreatedTime = DateTime.UtcNow;
                    row.TokenTypeId = (int)OAuthTokenType.AccessToken;
                    row.Token = accessToken;
                    row.TokenSecret = accessTokenSecret;

                    WebApplication.CommonDataSetTableAdapters.OAuthTokenTableAdapter.Update(row);
                }
            }
        }

        public void UpdateToken(IServiceProviderRequestToken token)
        {
            OAuthDataSet.OAuthTokenDataTable table = new OAuthDataSet.OAuthTokenDataTable();
            WebApplication.CommonDataSetTableAdapters.OAuthTokenTableAdapter.Fill(table, 0, token.Token);
            if (table.Count > 0)
            {
                OAuthDataSet.OAuthTokenRow row = table[0];
                row.RequestTokenVerifier = token.VerificationCode;

                WebApplication.CommonDataSetTableAdapters.OAuthTokenTableAdapter.Update(row);
            }
        }

        public string UpdateRequestTokenVerifier(string token)
        {
            string verifier = null;

            OAuthDataSet.OAuthTokenDataTable table = new OAuthDataSet.OAuthTokenDataTable();
            WebApplication.CommonDataSetTableAdapters.OAuthTokenTableAdapter.Fill(table, 0, token);
            if (table.Count > 0)
            {
                OAuthDataSet.OAuthTokenRow row = table[0];

                OAuthDataSet.OAuthConsumerRow consumerRow = (OAuthDataSet.OAuthConsumerRow)TokenProvider.Current.GetConsumer(row.ConsumerId);
                if (consumerRow != null)
                {
                    verifier = ServiceProvider.CreateVerificationCode((DotNetOpenAuth.OAuth.VerificationCodeFormat)consumerRow.VerificationCodeFormat, consumerRow.VerificationCodeLength);

                    row.RequestTokenVerifier = verifier;

                    WebApplication.CommonDataSetTableAdapters.OAuthTokenTableAdapter.Update(row);
                }

                return verifier;
            }

            return null;
        }

        public void AuthorizeRequestToken(string requestToken, Guid loginId)
        {
            OAuthDataSet.OAuthTokenDataTable table = new OAuthDataSet.OAuthTokenDataTable();
            WebApplication.CommonDataSetTableAdapters.OAuthTokenTableAdapter.Fill(table, 0, requestToken);
            if (table.Count > 0)
            {
                OAuthDataSet.OAuthTokenRow row = table[0];
                row.TokenTypeId = (int)OAuthTokenType.AuthorizedRequestToken;
                row.LoginId = loginId;

                WebApplication.CommonDataSetTableAdapters.OAuthTokenTableAdapter.Update(row);
            }
        }

        public void DeleteToken(string token)
        {
            WebApplication.CommonDataSetTableAdapters.OAuthTokenTableAdapter.Delete(token);
        }

        public string GenerateTokenSecret()
        {
            byte[] randomData = new byte[8];
            s_CryptoRandomDataGenerator.GetBytes(randomData);
            return Convert.ToBase64String(randomData);
        }

        public string GenerateConsumerSecret()
        {
            byte[] randomData = new byte[16];
            s_CryptoRandomDataGenerator.GetBytes(randomData);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < randomData.Length; i++)
            {
                sb.Append(randomData[i].ToString("x2"));
            }

            return sb.ToString();
        }

        #endregion
    }
}
