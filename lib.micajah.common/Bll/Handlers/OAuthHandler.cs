using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth.Messages;
using Micajah.Common.Application;
using Micajah.Common.Bll.Providers.OAuth;
using Micajah.Common.Dal;
using Micajah.Common.Security;

namespace Micajah.Common.Bll.Handlers
{
    public class OAuthHandler : IHttpHandler, IRequiresSessionState
    {
        #region Members

        private ServiceProvider m_Provider;

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
            m_Provider = new ServiceProvider();
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
                context.Response.Redirect("~/mc/oauth.aspx");
            }
            else if ((requestAccessToken = request as AuthorizedTokenRequest) != null)
            {
                AuthorizedTokenResponse response = m_Provider.PrepareAccessTokenMessage(requestAccessToken);

                OAuthDataSet.OAuthTokenRow row = (OAuthDataSet.OAuthTokenRow)m_Provider.TokenManager.GetAccessToken(response.AccessToken);
                response.ExtraData.Add(new KeyValuePair<string, string>("api_token", WebApplication.LoginProvider.GetToken(row.LoginId)));

                UserContext user = UserContext.Current;
                if (user != null)
                {
                    if (user.SelectedOrganization != null)
                    {
                        response.ExtraData.Add(new KeyValuePair<string, string>("org", user.SelectedOrganization.PseudoId));
                        if (user.SelectedInstance != null)
                            response.ExtraData.Add(new KeyValuePair<string, string>("dept", user.SelectedInstance.PseudoId));
                    }
                }

                m_Provider.Channel.Send(response);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
