using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth.Messages;
using Micajah.Common.Application;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Bll.Providers.OAuth;
using Micajah.Common.Dal;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;

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

        #region Public Methods

        public void ProcessRequest(HttpContext context)
        {
            IProtocolMessage request = m_Provider.ReadRequest();
            UnauthorizedTokenRequest requestToken = null;
            UserAuthorizationRequest requestAuth = null;
            AuthorizedTokenRequest requestAccessToken;

            Guid userId = Guid.Empty;
            Guid organizationId = Guid.Empty;
            Guid instanceId = Guid.Empty;
            LoginProvider.ParseAuthCookie(out userId, out organizationId, out instanceId);

            if ((requestToken = request as UnauthorizedTokenRequest) != null)
            {
                UnauthorizedTokenResponse response = m_Provider.PrepareUnauthorizedTokenMessage(requestToken);
                m_Provider.Channel.Send(response);
            }
            else if ((requestAuth = request as UserAuthorizationRequest) != null)
            {
                ServiceProvider.SetOAuthPendingUserAuthorizationRequest(requestAuth, userId);

                context.Response.Redirect(ResourceProvider.OAuthPageVirtualPath);
            }
            else if ((requestAccessToken = request as AuthorizedTokenRequest) != null)
            {
                AuthorizedTokenResponse response = m_Provider.PrepareAccessTokenMessage(requestAccessToken);

                OAuthDataSet.OAuthTokenRow row = (OAuthDataSet.OAuthTokenRow)m_Provider.TokenManager.GetAccessToken(response.AccessToken);
                response.ExtraData.Add(new KeyValuePair<string, string>("api_token", WebApplication.LoginProvider.GetToken(row.LoginId)));

                if (organizationId != Guid.Empty)
                {
                    response.ExtraData.Add(new KeyValuePair<string, string>("org", OrganizationProvider.GetOrganization(organizationId).PseudoId));
                    if (instanceId != Guid.Empty)
                        response.ExtraData.Add(new KeyValuePair<string, string>("dept", InstanceProvider.GetInstance(instanceId, organizationId).PseudoId));
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
