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
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls.SecurityControls
{
    public class OAuthControl : Micajah.Common.WebControls.SetupControls.MasterControl
    {
        #region Members

        protected Literal TitleLiteral;
        protected Literal ConsumerLiteral;
        protected LinkButton AllowAccessButton;
        protected LinkButton DenyAccessButton;
        protected HiddenField OAuthAuthorizationSecToken;
        protected Literal RevokeLiteral;
        protected Literal AuthorizationGrantedLiteral;
        protected MultiView MainMultiView;
        protected MultiView VerifierMultiView;
        protected Literal VerificationCodeLiteral;
        protected Literal CloseLiteral;
        protected Literal AuthorizationDeniedLiteral;

        private UserAuthorizationRequest m_PendingRequest;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the embedded stylesheets should be added into page's header.
        /// </summary>
        public bool EnableEmbeddedStyleSheets
        {
            get
            {
                object obj = ViewState["EnableEmbeddedStyleSheets"];
                return ((obj == null) ? true : (bool)obj);
            }
            set { ViewState["EnableEmbeddedStyleSheets"] = value; }
        }

        #endregion

        #region Private Methods

        private void LoadResources()
        {
            TitleLiteral.Text = Resources.OAuthControl_TitleLiteral_Text;
            ConsumerLiteral.Text = Resources.OAuthControl_ConsumerLiteral_Text;
            AllowAccessButton.Text = Resources.OAuthControl_AllowAccessButton_Text;
            DenyAccessButton.Text = Resources.OAuthControl_DenyAccessButton_Text;
            RevokeLiteral.Text = string.Format(CultureInfo.InvariantCulture, Resources.OAuthControl_RevokeLiteral_Text, FrameworkConfiguration.Current.WebApplication.Name);
            AuthorizationGrantedLiteral.Text = Resources.OAuthControl_AuthorizationGrantedLiteral_Text;
            VerificationCodeLiteral.Text = Resources.OAuthControl_VerificationCodeLiteral_Text;
            CloseLiteral.Text = Resources.OAuthControl_CloseLiteral_Text;
            AuthorizationDeniedLiteral.Text = Resources.OAuthControl_AuthorizationDeniedLiteral_Text;
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

            if (this.EnableEmbeddedStyleSheets)
            {
                if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern)
                    this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.LogOnModernStyleSheet, true)));
                else
                    this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.LogOnStyleSheet, true)));
            }

            m_PendingRequest = TokenProvider.Current.GetPendingUserAuthorizationRequest();

            if (!IsPostBack)
            {
                this.LoadResources();

                MainMultiView.ActiveViewIndex = 2;

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

                    ConsumerLiteral.Text = string.Format(CultureInfo.InvariantCulture, Resources.OAuthControl_ConsumerLiteral_Text, TokenProvider.Current.GetConsumer(requestTokenRow.ConsumerId).Key, FrameworkConfiguration.Current.WebApplication.Name);

                    // Generate an unpredictable secret that goes to the user agent and must come back with authorization 
                    // to guarantee the user interacted with this page rather than being scripted by an evil Consumer.
                    OAuthAuthorizationSecToken.Value = UserContext.OAuthAuthorizationSecret = TokenProvider.Current.GenerateTokenSecret();
                }
            }
        }

        protected void AllowAccessButton_Click(object sender, EventArgs e)
        {
            if (UserContext.OAuthAuthorizationSecret != OAuthAuthorizationSecToken.Value)
                throw new ArgumentException(Resources.OAuthControl_InvalidAuthorizationSecret); // Probably someone trying to hack in.

            string token = ((ITokenContainingMessage)m_PendingRequest).Token;

            TokenProvider.Current.AuthorizeRequestToken(token);
            TokenProvider.SetTokenCookie(null);

            UserContext.OAuthAuthorizationSecret = null; // Clear one time use secret.

            MainMultiView.ActiveViewIndex = 1;

            using (ServiceProvider provider = new ServiceProvider())
            {
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
                        VerificationCodeLiteral.Text = string.Format(CultureInfo.InvariantCulture, Resources.OAuthControl_VerificationCodeLiteral_Text, TokenProvider.Current.UpdateRequestTokenVerifier(token));
                }
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
