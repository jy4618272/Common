using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth.Messages;
using Micajah.Common.Application;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Bll.Providers.OAuth;
using Micajah.Common.Dal;
using System;
using System.Collections.Generic;
using System.Web;

namespace Micajah.Common.Bll.Handlers
{
    public class OAuthHandler : IHttpHandler
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

        #region Public Methods

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
                string token = ((ITokenContainingMessage)requestAuth).Token;

                ((TokenProvider)m_Provider.TokenManager).UpdatePendingUserAuthorizationRequest(token, requestAuth);

                TokenProvider.SetTokenCookie(token);

                context.Response.Redirect(ActionProvider.FindAction(ActionProvider.OAuthPageActionId).AbsoluteNavigateUrl);
            }
            else if ((requestAccessToken = request as AuthorizedTokenRequest) != null)
            {
                AuthorizedTokenResponse response = m_Provider.PrepareAccessTokenMessage(requestAccessToken);

                OAuthDataSet.OAuthTokenRow row = (OAuthDataSet.OAuthTokenRow)m_Provider.TokenManager.GetAccessToken(response.AccessToken);
                response.ExtraData.Add(new KeyValuePair<string, string>("api_token", LoginProvider.Current.GetToken(row.LoginId)));

                if (!row.IsOrganizationIdNull())
                {
                    response.ExtraData.Add(new KeyValuePair<string, string>("org", OrganizationProvider.GetOrganization(row.OrganizationId).PseudoId));
                    if (!row.IsInstanceIdNull())
                        response.ExtraData.Add(new KeyValuePair<string, string>("dept", InstanceProvider.GetInstance(row.InstanceId, row.OrganizationId).PseudoId));
                }

                m_Provider.Channel.Send(response);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        #endregion
    }
}
