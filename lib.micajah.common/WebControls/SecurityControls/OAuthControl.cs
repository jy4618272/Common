using DotNetOpenAuth.OAuth.ChannelElements;
using DotNetOpenAuth.OAuth.Messages;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Bll.Providers.OAuth;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls.SecurityControls
{
    public class OAuthControl : UserControl
    {
        #region Members

        protected HtmlGenericControl ErrorDiv;
        protected Label ConsumerLabel;
        protected Label AllowLabel;
        protected LinkButton AllowAccessButton;
        protected LinkButton DenyAccessButton;
        protected HiddenField OAuthAuthorizationSecToken;
        protected Panel ConsumerWarningPanel;
        protected Label ConsumerWarningLabel;
        protected Label JavascriptDisabledLabel;
        protected Label RevokeLabel;
        protected Label AuthorizationGrantedLabel;
        protected MultiView MainMultiView;
        protected MultiView VerifierMultiView;
        protected Label VerificationCodeLabel;
        protected Label CloseLabel;
        protected Label AuthorizationDeniedLabel;

        private Micajah.Common.Pages.MasterPage m_MasterPage;
        private UserAuthorizationRequest m_PendingRequest;
        private Guid m_UserId;

        #endregion

        #region Private Properties

        protected Micajah.Common.Pages.MasterPage MasterPage
        {
            get
            {
                if (m_MasterPage == null) m_MasterPage = Page.Master as Micajah.Common.Pages.MasterPage;
                return m_MasterPage;
            }
        }

        #endregion

        #region Private Methods

        private void LoadResources()
        {
            ConsumerLabel.Text = Resources.OAuthControl_ConsumerLabel_Text;
            AllowLabel.Text = Resources.OAuthControl_AllowLabel_Text;
            AllowAccessButton.Text = Resources.OAuthControl_AllowAccessButton_Text;
            DenyAccessButton.Text = Resources.OAuthControl_DenyAccessButton_Text;
            JavascriptDisabledLabel.Text = Resources.OAuthControl_JavascriptDisabledLabel_Text;
            RevokeLabel.Text = Resources.OAuthControl_RevokeLabel_Text;
            ConsumerWarningLabel.Text = Resources.OAuthControl_ConsumerWarningLabel_Text;
            AuthorizationGrantedLabel.Text = Resources.OAuthControl_AuthorizationGrantedLabel_Text;
            VerificationCodeLabel.Text = Resources.OAuthControl_VerificationCodeLabel_Text;
            CloseLabel.Text = Resources.OAuthControl_CloseLabel_Text;
            AuthorizationDeniedLabel.Text = Resources.OAuthControl_AuthorizationDeniedLabel_Text;
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            this.MasterPage.VisibleHeader
                = this.MasterPage.VisibleMainMenu
                = this.MasterPage.VisibleLeftArea
                = this.MasterPage.VisibleSubmenu
                = this.MasterPage.VisibleBreadcrumbs
                = this.MasterPage.VisibleFooter
                = this.MasterPage.VisibleHeaderMessage
                = this.MasterPage.EnableOverlay
                = false;

            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern)
                this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.LogOnModernStyleSheet, true)));
            else
                this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.LogOnStyleSheet, true)));

            m_UserId = UserContext.Current.UserId;
            m_PendingRequest = TokenProvider.Current.GetPendingUserAuthorizationRequest();

            if (!IsPostBack)
            {
                this.LoadResources();

                MainMultiView.ActiveViewIndex = 2;
                ConsumerWarningPanel.Visible = false;


                if (m_PendingRequest == null)
                {
                    //Response.Redirect("~/Members/AuthorizedConsumers.aspx"); // TODO: Need to redirect to user's start page?
                }
                else
                {
                    MainMultiView.ActiveViewIndex = 0;

                    string token = ((ITokenContainingMessage)m_PendingRequest).Token;
                    IServiceProviderRequestToken requestToken = TokenProvider.Current.GetRequestToken(token);
                    OAuthDataSet.OAuthTokenRow requestTokenRow = (OAuthDataSet.OAuthTokenRow)requestToken;

                    ConsumerLabel.Text = string.Format(CultureInfo.InvariantCulture, Resources.OAuthControl_ConsumerLabel_Text, TokenProvider.Current.GetConsumer(requestTokenRow.ConsumerId).Key, requestTokenRow.Scope);

                    ConsumerWarningPanel.Visible = m_PendingRequest.IsUnsafeRequest;
                    if (m_PendingRequest.IsUnsafeRequest)
                    {
                        Uri rootUrl = new Uri(FrameworkConfiguration.Current.WebApplication.Url);

                        string consumerDomain = ((Request.UrlReferrer != null) ? Request.UrlReferrer.Host : Resources.OAuthControl_UnrecognizedConsumerDomain);

                        ConsumerWarningLabel.Text = string.Format(CultureInfo.InvariantCulture, Resources.OAuthControl_ConsumerWarningLabel_Text, rootUrl.Host, consumerDomain);
                    }

                    // Generate an unpredictable secret that goes to the user agent and must come back with authorization 
                    // to guarantee the user interacted with this page rather than being scripted by an evil Consumer.
                    OAuthAuthorizationSecToken.Value = UserContext.OAuthAuthorizationSecret = TokenProvider.Current.GenerateTokenSecret();
                }
            }
        }

        protected void AllowAccessButton_Click(object sender, EventArgs e)
        {
            if (UserContext.OAuthAuthorizationSecret != OAuthAuthorizationSecToken.Value)
                throw new ArgumentException(); // Probably someone trying to hack in.

            string token = ((ITokenContainingMessage)m_PendingRequest).Token;

            TokenProvider.Current.AuthorizeRequestToken(token, m_UserId);
            TokenProvider.SetTokenCookie(null);

            UserContext.OAuthAuthorizationSecret = null; // Clear one time use secret.

            MainMultiView.ActiveViewIndex = 1;

            ServiceProvider provider = new ServiceProvider();
            UserAuthorizationResponse response = provider.PrepareAuthorizationResponse(m_PendingRequest);
            if (response != null)
            {
                provider.Channel.Send(response);
            }
            else
            {
                if (m_PendingRequest.IsUnsafeRequest)
                    VerifierMultiView.ActiveViewIndex = 1;
                else
                    VerificationCodeLabel.Text = string.Format(CultureInfo.InvariantCulture, Resources.OAuthControl_VerificationCodeLabel_Text, TokenProvider.Current.UpdateRequestTokenVerifier(token));
            }
        }

        protected void DenyAccessButton_Click(object sender, EventArgs e)
        {
            MainMultiView.ActiveViewIndex = 2;

            string token = ((ITokenContainingMessage)m_PendingRequest).Token;

            TokenProvider.Current.DeleteToken(token);
            TokenProvider.SetTokenCookie(null);
        }

        #endregion
    }
}
