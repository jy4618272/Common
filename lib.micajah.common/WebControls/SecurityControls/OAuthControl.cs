using System;
using System.Security.Cryptography;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetOpenAuth.OAuth.Messages;
using Micajah.Common.Bll.Providers.OAuth;
using Micajah.Common.Security;

namespace Micajah.Common.WebControls.SecurityControls
{
    public class OAuthControl : UserControl
    {
        protected Label DesiredAccessLabel;
        protected Label ConsumerLabel;
        protected HiddenField OAuthAuthorizationSecToken;
        protected Panel ConsumerWarningPanel;
        protected MultiView MainMultiView;
        protected MultiView VerifierMultiView;
        protected Label VerificationCodeLabel;

        private static readonly RandomNumberGenerator s_CryptoRandomDataGenerator = new RNGCryptoServiceProvider();

        private string AuthorizationSecret
        {
            get { return Session["OAuthAuthorizationSecret"] as string; }
            set { Session["OAuthAuthorizationSecret"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (UserContext.OAuthPendingUserAuthorizationRequest == null)
                {
                    Response.Redirect("~/Members/AuthorizedConsumers.aspx"); // TODO: Need to redirect to user's start page?
                }
                else
                {
                    ITokenContainingMessage pendingToken = UserContext.OAuthPendingUserAuthorizationRequest;
                    var token = Global.DataContext.OAuthTokens.Single(t => t.Token == pendingToken.Token);

                    DesiredAccessLabel.Text = token.Scope;
                    ConsumerLabel.Text = TokenProvider.Current.GetConsumerForToken(token.Token).ConsumerKey;

                    // Generate an unpredictable secret that goes to the user agent and must come back
                    // with authorization to guarantee the user interacted with this page rather than
                    // being scripted by an evil Consumer.
                    byte[] randomData = new byte[8];
                    s_CryptoRandomDataGenerator.GetBytes(randomData);
                    this.AuthorizationSecret = Convert.ToBase64String(randomData);

                    OAuthAuthorizationSecToken.Value = this.AuthorizationSecret;

                    ConsumerWarningPanel.Visible = UserContext.OAuthPendingUserAuthorizationRequest.IsUnsafeRequest;
                }
            }
        }

        protected void AllowAccessButton_Click(object sender, EventArgs e)
        {
            if (this.AuthorizationSecret != OAuthAuthorizationSecToken.Value)
            {
                throw new ArgumentException(); // Probably someone trying to hack in.
            }

            this.AuthorizationSecret = null; // Clear one time use secret.

            UserAuthorizationRequest pendingRequest = UserContext.OAuthPendingUserAuthorizationRequest;
            string token = ((ITokenContainingMessage)pendingRequest).Token;

            TokenProvider.Current.AuthorizeRequestToken(token, LoggedInUser);

            UserContext.OAuthPendingUserAuthorizationRequest = null;

            UserContext.AuthorizePendingRequestToken();

            MainMultiView.ActiveViewIndex = 1;

            ServiceProvider sp = new ServiceProvider();
            var response = sp.PrepareAuthorizationResponse(pendingRequest);
            if (response != null)
            {
                sp.Channel.Send(response);
            }
            else
            {
                if (pendingRequest.IsUnsafeRequest)
                {
                    VerifierMultiView.ActiveViewIndex = 1;
                }
                else
                {
                    string verifier = ServiceProvider.CreateVerificationCode(DotNetOpenAuth.OAuth.VerificationCodeFormat.AlphaNumericNoLookAlikes, 10);
                    VerificationCodeLabel.Text = verifier;

                    var requestToken = TokenProvider.Current.GetRequestToken(token);
                    requestToken.VerificationCode = verifier;
                    TokenProvider.Current.UpdateToken(requestToken);
                }
            }
        }

        protected void DenyAccessButton_Click(object sender, EventArgs e)
        {
            // Erase the request token.
            MainMultiView.ActiveViewIndex = 2;
        }
    }
}
