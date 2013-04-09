using System;
using System.Web;
using System.Web.SessionState;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.Messages;
using Micajah.Common.Security;

namespace Micajah.Common.Bll.Handlers
{
    public class OAuthHandler : IHttpHandler, IRequiresSessionState
    {
        #region Members

        ServiceProvider m_Provider;

        #endregion

        #region Public Properties

        public bool IsReusable
        {
            get { return true; }
        }

        #endregion

        #region Constructors

        public OAuthHandler()
        {
            m_Provider = new ServiceProvider(Constants.SelfDescription, Global.TokenManager);//, new CustomOAuthMessageFactory(Global.TokenManager));
        }

        #endregion

        public void ProcessRequest(HttpContext context)
        {
            IProtocolMessage request = m_Provider.ReadRequest();
            UnauthorizedTokenRequest requestToken = null;
            UserAuthorizationRequest requestAuth = null;
            AuthorizedTokenRequest requestAccessToken;

            if ((requestToken = request as UnauthorizedTokenRequest) != null)
            {
                UnauthorizedTokenResponse response = m_Provider.PrepareUnauthorizedTokenMessage(requestToken);
                m_Provider.Channel.Send(response);
            }
            else if ((requestAuth = request as UserAuthorizationRequest) != null)
            {
                UserContext.OAuthPendingUserAuthorizationRequest = requestAuth;
                HttpContext.Current.Response.Redirect("~/mc/authorize.aspx");
            }
            else if ((requestAccessToken = request as AuthorizedTokenRequest) != null)
            {
                AuthorizedTokenResponse response = m_Provider.PrepareAccessTokenMessage(requestAccessToken);
                m_Provider.Channel.Send(response);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
