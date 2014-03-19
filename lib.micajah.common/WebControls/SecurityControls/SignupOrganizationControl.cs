using Google.GData.Client;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls.SecurityControls
{
    public partial class SignupOrganizationControl : System.Web.UI.UserControl
    {
        #region Members

        protected Image LogoImage;
        protected Label OrganizationNameLabel;
        protected Literal OrganizationNameHelpText;
        protected TextBox OrganizationName;
        protected Image OrganizationNameTick;
        protected Label EmailLabel;
        protected TextBox Email;
        protected CustomValidator EmailValidator;
        protected Image EmailTick;
        protected Label EmailInUseLabel;
        protected HtmlGenericControl OrganizationUrlRow;
        protected Label OrganizationUrlLabel;
        protected Literal Schema;
        protected TextBox OrganizationUrl;
        protected Literal PartialCustomUrlRootAddress;
        protected Button CreateMyAccountButton;
        protected CustomValidator OrganizationUrlValidator;
        protected Image OrganizationUrlTick;
        protected HtmlGenericControl ModalWindowHeader;
        protected Literal ModalTitleLiteral;
        protected Literal ModalMessageLiteral;
        protected Literal ModalSelectActionLiteral;
        protected HyperLink ModalLoginLink;
        protected Literal ModalSelectActionSeparatorLiteral;
        protected Button CreateMyAccountModalButton;
        protected Literal AgreeLabel;
        protected Label CaptchaLabel;
        protected Telerik.Web.UI.RadCaptcha Captcha;
        protected CustomValidator CaptchaValidator;

        protected HtmlGenericControl ErrorPanel;
        protected Image LogoImage3;
        protected Label ErrorLabel;
        protected Label ErrorContinueLabel;
        protected HyperLink ErrorContinueLink;

        #endregion

        #region Private Properties

        private string SetOrganizationUrlClientScript
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"function SetOrganizationUrl(elem1) {{
    var elem2 = document.getElementById('{0}_txt');
    if (elem1 && elem2)
        elem2.value = elem1.value.replace(/[^a-zA-Z 0-9]+|[\s]+/g, '').toLowerCase();
}}
"
                    , OrganizationUrl.ClientID);
            }
        }

        private string ShowLogoImageClientScript
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"if (window.top == window.self) {{
    var elem = document.getElementById('{0}');
    if (elem)
        elem.style.display = 'inline';
}}
"
                    , LogoImage.ClientID);
            }
        }

        private string OAuth2Parameters
        {
            get { return (string)this.ViewState["OAuth2Parameters"]; }
            set { this.ViewState["OAuth2Parameters"] = value; }
        }

        private string UserFirstName
        {
            get { return ViewState["UserFirstName"] != null ? (string)ViewState["UserFirstName"] : "Organization"; }
            set { ViewState["UserFirstName"] = value; }
        }

        private string UserLastName
        {
            get { return ViewState["UserLastName"] != null ? (string)ViewState["UserLastName"] : "Administrator"; }
            set { ViewState["UserLastName"] = value; }
        }

        #endregion

        #region Private Methods

        private void LoadResources()
        {
            OrganizationName.ErrorMessage = Resources.SignupOrganizationControl_OrganizationName_ErrorMessage;
            OrganizationNameLabel.Text = Resources.SignupOrganizationControl_OrganizationNameLabel_Text;
            OrganizationNameHelpText.Text = Resources.SignupOrganizationControl_OrganizationNameHelpText_Text;
            OrganizationNameTick.ImageUrl = EmailTick.ImageUrl = OrganizationUrlTick.ImageUrl
                = ResourceProvider.GetImageUrl(typeof(SignupOrganizationControl), "Tick.png", true);
            Email.ErrorMessage = Resources.SignupOrganizationControl_Email_ErrorMessage;
            EmailLabel.Text = Resources.SignupOrganizationControl_EmailLabel_Text;
            OrganizationUrlLabel.Text = Resources.SignupOrganizationControl_OrganizationUrlLabel_Text;
            Schema.Text = Uri.UriSchemeHttps + Uri.SchemeDelimiter;
            PartialCustomUrlRootAddress.Text = "." + FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlRootAddressesFirst;
            OrganizationUrl.ErrorMessage = Resources.CustomUrlProvider_CustomUrlAlreadyExists;
            OrganizationUrlValidator.ErrorMessage = Resources.CustomUrlProvider_CustomUrlAlreadyExists;
            CreateMyAccountButton.Text = Resources.SignupOrganizationControl_CreateMyAccountButton_Text;

            ModalTitleLiteral.Text = Resources.SignupOrganizationControl_ModalTitleLiteral_Text;
            ModalMessageLiteral.Text = Resources.SignupOrganizationControl_ModalMessageLiteral_Text;
            ModalSelectActionLiteral.Text = Resources.SignupOrganizationControl_ModalSelectActionLiteral_Text;
            ModalLoginLink.Text = Resources.SignupOrganizationControl_ModalLoginLink_Text;
            ModalSelectActionSeparatorLiteral.Text = Resources.SignupOrganizationControl_ModalSelectActionSeparatorLiteral_Text;
            CreateMyAccountModalButton.Text = Resources.SignupOrganizationControl_CreateMyAccountModalButton_Text;

            if (!string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.SmallLogoImageUrl))
            {
                ModalWindowHeader.Attributes["class"] += " bg";
                ModalWindowHeader.Style[HtmlTextWriterStyle.BackgroundImage] = "url(" + CustomUrlProvider.CreateApplicationAbsoluteUrl(FrameworkConfiguration.Current.WebApplication.SmallLogoImageUrl) + ")";
            }

            CaptchaLabel.Text = Resources.SignupOrganizationControl_CaptchaLabel_Text;
            AgreeLabel.Text = string.Format(CultureInfo.InvariantCulture, Resources.SignupOrganizationControl_AgreeLabel_Text
                , BaseControl.GetHyperlink(FrameworkConfiguration.Current.WebApplication.Support.TermsUrl, Resources.SignupOrganizationControl_TermsLink_Text)
                , BaseControl.GetHyperlink(FrameworkConfiguration.Current.WebApplication.Support.PrivacyPolicyUrl, Resources.SignupOrganizationControl_PrivacyPolicyLink_Text));

            ErrorContinueLabel.Text = Resources.SignupOrganizationControl_ErrorContinueLabel_Text;
            ErrorContinueLink.Text = Resources.SignupOrganizationControl_ErrorContinueLink_Text;

            if (string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.BigLogoImageUrl))
                LogoImage.Visible = LogoImage3.Visible = false;
            else
                LogoImage.ImageUrl = LogoImage3.ImageUrl = FrameworkConfiguration.Current.WebApplication.BigLogoImageUrl;
        }

        private static bool ValidateEmail(string email, out string errorMessage)
        {
            errorMessage = null;
            bool isValid = Support.ValidateEmail(email, false);
            if (!isValid)
                errorMessage = Resources.SignupOrganizationControl_Email_ValidationErrorMessage;
            return isValid;
        }

        private void CreateNewOrganization()
        {
            if (string.Compare((string)Session["NewOrg"], "1", StringComparison.OrdinalIgnoreCase) == 0)
            {
                string url = null;

                UserContext user = UserContext.Current;
                if (user != null)
                {
                    if (string.Compare(user.Email, Email.Text, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        url = LoginProvider.Current.GetLoginUrl(user.Email, true, user.OrganizationId, user.InstanceId, null);

                        Response.Redirect(url);
                    }
                }

                url = LoginProvider.Current.GetLoginUrl();

                Response.Redirect(url);
            }
            else
            {
                Instance templateInstance = null;
                InstanceCollection insts = Micajah.Common.Bll.Providers.InstanceProvider.GetTemplateInstances();

                if (insts.Count == 0)
                {
                    throw new NotImplementedException(Resources.SignupOrganizationControl_NoActiveTemplateInstances);
                }
                else
                {
                    templateInstance = insts[0];
                }

                string howYouHearAboutUs = null;

                bool isGoogleProviderRequest = GoogleProvider.IsGoogleProviderRequest(this.Request);
                if (isGoogleProviderRequest)
                {
                    howYouHearAboutUs = Resources.SignupOrganizationControl_HowYouHearAboutUs_Text;
                }

                string password = LoginProvider.Current.GeneratePassword(3, 0).ToLowerInvariant();

                Guid orgId = OrganizationProvider.InsertOrganization(OrganizationName.Text, null, null
                    , null, null, null, null, null, null, string.Empty, howYouHearAboutUs
                    , templateInstance.TimeZoneId, templateInstance.InstanceId
                    , Email.Text, password, this.UserFirstName, this.UserLastName, null, null, null
                    , OrganizationUrl.Text, this.Request
                    , true);

                Session["NewOrg"] = "1";

                Instance inst = InstanceProvider.GetFirstInstance(orgId);

                if (isGoogleProviderRequest)
                {
                    string returnUrl = null;
                    OAuth2Parameters parameters = JsonConvert.DeserializeObject<OAuth2Parameters>(this.OAuth2Parameters);

                    GoogleProvider.ProcessAuthorization(this.Context, ref parameters, ref returnUrl);
                }

                string url = LoginProvider.Current.GetLoginUrl(Email.Text, true, orgId, inst.InstanceId, null);

                Response.Redirect(url);
            }
        }

        #endregion

        #region Overriden Methods

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Micajah.Common.Pages.MasterPage.RegisterGlobalStyleSheet(this.Page, MasterPageTheme.Modern);
            Micajah.Common.Pages.MasterPage.RegisterClientEncodingScript(this.Page);

            if (!this.IsPostBack)
            {
                this.Page.Form.Attributes["onsubmit"] += " return true;";

                string code = FrameworkConfiguration.Current.WebApplication.Integration.Google.AnalyticsCode.Value;
                if (!string.IsNullOrEmpty(code))
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "GoogleAnalyticsClientScript", code, false);

                code = FrameworkConfiguration.Current.WebApplication.Integration.Google.ConversionCode.Value;
                if (!string.IsNullOrEmpty(code))
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "GoogleConversionClientScript", code, false);

                code = FrameworkConfiguration.Current.WebApplication.Integration.Bing.ConversionCode.Value;
                if (!string.IsNullOrEmpty(code))
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "BingConversionClientScript", code, false);
            }

            this.Page.Form.Target = "_parent";

            LogoImage.Style[HtmlTextWriterStyle.Display] = "none";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ShowLogoImage", this.ShowLogoImageClientScript, true);

            if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
            {
                OrganizationName.Attributes["onchange"] = "SetOrganizationUrl(this);";

                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "SetOrganizationUrlClientScript", this.SetOrganizationUrlClientScript, true);
            }

            this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl("Styles.SignupOrganization.css", true)));
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            Micajah.Common.Pages.MasterPage.SetPageTitle(this.Page, ActionProvider.PagesAndControls.FindByActionId(ActionProvider.SignUpOrganizationPageActionId));

            if (!this.Page.IsPostBack)
            {
                this.LoadResources();

                ErrorPanel.Visible = false;

                if (GoogleProvider.IsGoogleProviderRequest(this.Request))
                {
                    string returnUrl = null;
                    OAuth2Parameters parameters = null;

                    try
                    {
                        GoogleProvider.ProcessAuthorization(this.Context, ref parameters, ref returnUrl);
                        this.OAuth2Parameters = JsonConvert.SerializeObject(parameters);
                    }
                    catch (System.Security.Authentication.AuthenticationException ex)
                    {
                        ErrorContinueLink.NavigateUrl = returnUrl;
                        ErrorLabel.Text = ex.Message;
                        ErrorPanel.Visible = true;

                        return;
                    }

                    string email = null;
                    string firstName = null;
                    string lastName = null;

                    GoogleProvider.GetUserProfile(parameters.AccessToken, out email, out firstName, out lastName);

                    if (!string.IsNullOrEmpty(email))
                    {
                        Email.Text = email;
                        Email.ReadOnly = true;
                    }

                    if (!string.IsNullOrEmpty(firstName))
                        this.UserFirstName = firstName;

                    if (!string.IsNullOrEmpty(lastName))
                        this.UserLastName = lastName;
                }

                OrganizationUrlRow.Visible = FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled;

                OrganizationName.Focus();

                Control captchaTextBoxLabel = Captcha.FindControl("CaptchaTextBoxLabel");
                if (captchaTextBoxLabel != null)
                    captchaTextBoxLabel.Visible = false;
            }

            ResourceProvider.RegisterValidatorScriptResource(this.Page);
        }

        protected void OrganizationName_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.IsValid = true;

            bool isEmpty = string.IsNullOrEmpty(textBox.Text);

            if (textBox.ID == OrganizationName.ID)
                OrganizationNameTick.Visible = (!isEmpty);
        }

        protected void Email_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text))
                textBox.IsValid = true;
            else
            {
                if (textBox.ID == Email.ID)
                    EmailValidator.Validate();
            }
        }

        protected void EmailValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args == null) return;

            CustomValidator val = (CustomValidator)source;
            TextBox textBox = null;
            Image tickImage = null;
            string errorMessage = null;

            if (val.ID == EmailValidator.ID)
            {
                textBox = Email;
                tickImage = EmailTick;
            }

            bool isValid = ValidateEmail(textBox.Text, out errorMessage);
            tickImage.Visible = textBox.IsValid = args.IsValid = isValid;

            if (!args.IsValid)
            {
                val.ErrorMessage = errorMessage;

                textBox.Attributes["validatorId"] = val.ClientID;
                textBox.Focus();
            }
        }

        protected void CaptchaValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args == null) return;

            args.IsValid = Captcha.IsValid;

            System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)Captcha.FindControl("CaptchaTextBox");
            Captcha.CaptchaTextBoxCssClass = Captcha.CaptchaTextBoxCssClass.Replace(" Invalid", string.Empty);

            if (args.IsValid)
                textBox.Attributes.Remove("validatorId");
            else
            {
                CaptchaValidator.ErrorMessage = Resources.SignupOrganizationControl_CaptchaValidator_ErrorMessage;
                CaptchaValidator.ForeColor = System.Drawing.Color.Empty;

                Captcha.CaptchaTextBoxCssClass += " Invalid";
                textBox.Attributes["validatorId"] = CaptchaValidator.ClientID;
                textBox.Focus();
            }
        }

        protected void OrganizationUrlValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args == null) return;

            OrganizationUrlTick.Visible = OrganizationUrl.IsValid = args.IsValid = false;
            OrganizationUrlValidator.ErrorMessage = Resources.SignupOrganizationControl_OrganizationUrlValidator_ErrorMessage;

            try
            {
                OrganizationUrlValidator.ErrorMessage = Resources.CustomUrlProvider_CustomUrlAlreadyExists;
                if (!string.IsNullOrEmpty(OrganizationUrl.Text))
                {
                    CustomUrlProvider.ValidatePartialCustomUrl(OrganizationUrl.Text);
                    OrganizationUrlTick.Visible = OrganizationUrl.IsValid = args.IsValid = CustomUrlProvider.ValidateCustomUrl(OrganizationUrl.Text);
                }
                else
                    args.IsValid = true;
            }
            catch (DataException ex)
            {
                OrganizationUrlValidator.ErrorMessage = ex.Message;

                OrganizationUrl.Attributes["validatorId"] = OrganizationUrlValidator.ClientID;
                OrganizationUrl.Focus();
            }
        }

        protected void OrganizationUrl_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.IsValid = true;
            }
            else
            {
                OrganizationUrlValidator.Validate();
            }
        }

        protected void CreateMyAccountButton_Click(object sender, EventArgs e)
        {
            Page.Validate("MainForm");
            if (!Page.IsValid)
                return;

            if (LoginProvider.Current.ValidateLogin(Email.Text, null))
            {
                ModalLoginLink.NavigateUrl = LoginProvider.Current.GetLoginUrl(Email.Text, false);

                ScriptManager.RegisterClientScriptInclude(this.Page, this.Page.GetType(), "JQueryScript", ResourceProvider.JQueryScriptUrl);
                ScriptManager.RegisterClientScriptInclude(this.Page, this.Page.GetType(), "JQueryEasyModalScript", ResourceProvider.GetResourceUrl("Scripts.jquery.easyModal.js", true));

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ModalWindowScript", @"Sys.Application.add_load(function() {
    $('#ModalWindow').easyModal({
        top: 40,
        overlay: 0.2,
        overlayClose: false,
        closeOnEscape: false
    });

    $('#ModalWindow').trigger('openModal');
});

"
                    , true);

                return;
            }

            this.CreateNewOrganization();
        }

        protected void CreateMyAccountModalButton_Click(object sender, EventArgs e)
        {
            this.CreateNewOrganization();
        }

        #endregion
    }
}