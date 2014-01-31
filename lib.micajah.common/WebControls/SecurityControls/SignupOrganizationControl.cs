using Google.GData.Client;
using Micajah.Common.Application;
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

        protected HtmlGenericControl Step1Panel;
        protected Image LogoImage1;
        protected HtmlGenericControl Step1Form;
        protected Label OrganizationNameLabel1;
        protected Literal OrganizationNameHelpText1;
        protected TextBox OrganizationName1;
        protected Image OrganizationNameTick1;
        protected Label EmailLabel1;
        protected TextBox Email1;
        protected CustomValidator EmailValidator1;
        protected Image EmailTick1;
        protected Label EmailInUseLabel;
        protected HtmlGenericControl OrganizationUrlRow;
        protected Label OrganizationUrlLabel;
        protected Literal Schema;
        protected TextBox OrganizationUrl;
        protected Literal PartialCustomUrlRootAddress;
        protected Button Step1Button;
        protected CustomValidator OrganizationUrlValidator;
        protected Image OrganizationUrlTick;
        protected UpdatePanel UpdatePanelOrganizationUrl;
        protected HtmlGenericControl ModalWindowHeader;
        protected Literal ModalTitleLiteral;
        protected Literal ModalMessageLiteral;
        protected Literal ModalSelectActionLiteral;
        protected HyperLink ModalLoginLink;
        protected Literal ModalSelectActionSeparatorLiteral;
        protected Button ModalStep1Button;
        protected Literal AgreeLabel;
        protected Label CaptchaLabel;
        protected Telerik.Web.UI.RadCaptcha Captcha1;
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
                    , LogoImage1.ClientID);
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

        private string HowYouHearAboutUs
        {
            get { return ViewState["HowYouHearAboutUs"] != null ? (string)ViewState["HowYouHearAboutUs"] : string.Empty; }
            set { ViewState["HowYouHearAboutUs"] = value; }
        }

        #endregion

        #region Private Methods

        private void LoadResources()
        {
            OrganizationName1.ErrorMessage = Resources.SignupOrganizationControl_OrganizationName1_ErrorMessage;
            OrganizationNameLabel1.Text = Resources.SignupOrganizationControl_OrganizationNameLabel1_Text;
            OrganizationNameHelpText1.Text = Resources.SignupOrganizationControl_OrganizationNameHelpText1_Text;
            OrganizationNameTick1.ImageUrl = EmailTick1.ImageUrl = OrganizationUrlTick.ImageUrl
                = ResourceProvider.GetImageUrl(typeof(SignupOrganizationControl), "Tick.png", true);
            Email1.ErrorMessage = Resources.SignupOrganizationControl_Email1_ErrorMessage;
            EmailLabel1.Text = Resources.SignupOrganizationControl_EmailLabel1_Text;
            OrganizationUrlLabel.Text = Resources.SignupOrganizationControl_OrganizationUrlLabel_Text;
            Schema.Text = Uri.UriSchemeHttps + Uri.SchemeDelimiter;
            PartialCustomUrlRootAddress.Text = "." + FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlRootAddressesFirst;
            OrganizationUrl.ErrorMessage = Resources.CustomUrlProvider_CustomUrlAlreadyExists;
            OrganizationUrlValidator.ErrorMessage = Resources.CustomUrlProvider_CustomUrlAlreadyExists;
            Step1Button.Text = Resources.SignupOrganizationControl_Step1Button_Text;

            ModalTitleLiteral.Text = Resources.SignupOrganizationControl_ModalTitleLiteral_Text;
            ModalMessageLiteral.Text = Resources.SignupOrganizationControl_ModalMessageLiteral_Text;
            ModalSelectActionLiteral.Text = Resources.SignupOrganizationControl_ModalSelectActionLiteral_Text;
            ModalLoginLink.Text = Resources.SignupOrganizationControl_ModalLoginLink_Text;
            ModalSelectActionSeparatorLiteral.Text = Resources.SignupOrganizationControl_ModalSelectActionSeparatorLiteral_Text;
            ModalStep1Button.Text = Resources.SignupOrganizationControl_ModalStep1Button_Text;

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
                LogoImage1.Visible = LogoImage3.Visible = false;
            else
                LogoImage1.ImageUrl = LogoImage3.ImageUrl = FrameworkConfiguration.Current.WebApplication.BigLogoImageUrl;
        }


        private static bool ValidateEmail(string email, out string errorMessage)
        {
            errorMessage = null;
            bool isValid = Support.ValidateEmail(email, false);
            if (!isValid)
                errorMessage = Resources.SignupOrganizationControl_Email1_ValidationErrorMessage;
            return isValid;
        }

        #endregion

        #region Overriden Methods

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Micajah.Common.Pages.MasterPage.RegisterGlobalStyleSheet(this.Page, MasterPageTheme.Modern);
            Micajah.Common.Pages.MasterPage.RegisterClientEncodingScript(this.Page);

            if (!this.IsPostBack)
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

            this.Page.Form.Target = "_parent";

            LogoImage1.Style[HtmlTextWriterStyle.Display] = "none";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ShowLogoImage", this.ShowLogoImageClientScript, true);

            if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
            {
                OrganizationName1.Attributes["onchange"] = "SetOrganizationUrl(this);";

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
                        GoogleProvider.ProcessOAuth2Authorization(this.Context, ref parameters, ref returnUrl);
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
                    string timeZone = null;

                    GoogleProvider.GetUserProfile(parameters.AccessToken, out email, out firstName, out lastName, out timeZone);

                    if (!string.IsNullOrEmpty(email))
                    {
                        Email1.Text = email;
                        Email1.ReadOnly = true;
                    }

                    if (!string.IsNullOrEmpty(firstName))
                        UserFirstName = firstName;

                    if (!string.IsNullOrEmpty(lastName))
                        UserLastName = lastName;

                    HowYouHearAboutUs = Resources.SignupOrganizationControl_HowYouHearAboutUs_Text;
                }

                OrganizationUrlRow.Visible = FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled;

                Step1Panel.Visible = true;

                OrganizationName1.Focus();

                Control captchaTextBoxLabel = Captcha1.FindControl("CaptchaTextBoxLabel");
                if (captchaTextBoxLabel != null)
                    captchaTextBoxLabel.Visible = false;
            }

            ResourceProvider.RegisterValidatorScriptResource(this.Page);
        }

        protected void OrganizationName1_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.IsValid = true;

            bool isEmpty = string.IsNullOrEmpty(textBox.Text);

            if (textBox.ID == OrganizationName1.ID)
                OrganizationNameTick1.Visible = (!isEmpty);
        }

        protected void Email1_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text))
                textBox.IsValid = true;
            else
            {
                if (textBox.ID == Email1.ID)
                    EmailValidator1.Validate();
            }
        }

        protected void EmailValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args == null) return;

            CustomValidator val = (CustomValidator)source;
            TextBox textBox = null;
            Image tickImage = null;
            string errorMessage = null;

            if (val.ID == EmailValidator1.ID)
            {
                textBox = Email1;
                tickImage = EmailTick1;
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

            args.IsValid = Captcha1.IsValid;

            System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)Captcha1.FindControl("CaptchaTextBox");
            Captcha1.CaptchaTextBoxCssClass = Captcha1.CaptchaTextBoxCssClass.Replace(" Invalid", string.Empty);

            if (args.IsValid)
                textBox.Attributes.Remove("validatorId");
            else
            {
                CaptchaValidator.ErrorMessage = Resources.SignupOrganizationControl_CaptchaValidator_ErrorMessage;
                CaptchaValidator.ForeColor = System.Drawing.Color.Empty;

                Captcha1.CaptchaTextBoxCssClass += " Invalid";
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
                textBox.IsValid = true;
            else
            {
                OrganizationUrlValidator.Validate();
                UpdatePanelOrganizationUrl.Update();
            }
        }

        protected void Step1Button_Click(object sender, EventArgs e)
        {
            Page.Validate("Step1");
            if (!Page.IsValid)
                return;

            if (WebApplication.LoginProvider.ValidateLogin(Email1.Text, null))
            {
                ModalLoginLink.NavigateUrl = WebApplication.LoginProvider.GetLoginUrl(Email1.Text, false);

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

            CreateNewOrganization();
        }

        protected void ModalStep1Button_Click(object sender, EventArgs e)
        {
            CreateNewOrganization();
        }

        protected void CreateNewOrganization()
        {
            if (string.Compare((string)Session["NewOrg"], "1", StringComparison.OrdinalIgnoreCase) == 0)
            {
                UserContext user = UserContext.Current;
                if (user != null)
                {
                    if (string.Compare(user.Email, Email1.Text, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        Response.Redirect(WebApplication.LoginProvider.GetLoginUrl(user.Email, true, user.OrganizationId, user.InstanceId, null));
                    }
                }
                Response.Redirect(WebApplication.LoginProvider.GetLoginUrl());
            }
            else
            {
                InstanceCollection insts = Micajah.Common.Bll.Providers.InstanceProvider.GetTemplateInstances();
                if (insts.Count == 0) throw new NotImplementedException("No Active Template Instances found to create new organization.");

                Guid orgId = OrganizationProvider.InsertOrganization(OrganizationName1.Text, null, null
                    , null, null, null, null, null, null,string.Empty, HowYouHearAboutUs
                    , insts[0].TimeZoneId, insts[0].InstanceId
                    , Email1.Text, Micajah.Common.Application.WebApplication.LoginProvider.GeneratePassword(3, 0).ToLower(), UserFirstName, UserLastName, null, null, null
                    , OrganizationUrl.Text, this.Request
                    , true);

                Session["NewOrg"] = "1";

                Instance inst = InstanceProvider.GetFirstInstance(orgId);

                if (GoogleProvider.IsGoogleProviderRequest(this.Request))
                {
                    string returnUrl = null;
                    OAuth2Parameters parameters = JsonConvert.DeserializeObject<OAuth2Parameters>(this.OAuth2Parameters);

                    GoogleProvider.ProcessOAuth2Authorization(this.Context, ref parameters, ref returnUrl);
                }

                Response.Redirect(WebApplication.LoginProvider.GetLoginUrl(Email1.Text, true, orgId, inst.InstanceId, null));
            }
        }

        #endregion
    }
}