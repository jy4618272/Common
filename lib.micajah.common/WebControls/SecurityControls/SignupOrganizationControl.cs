using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Micajah.Common.Properties;
using Micajah.Common.WebControls.SetupControls;

namespace Micajah.Common.WebControls.SecurityControls
{
    public partial class SignupOrganizationControl : System.Web.UI.UserControl
    {
        #region Members

        protected HtmlGenericControl Step1Panel;
        protected Image LogoImage1;
        protected HtmlTable Step1Form;
        protected Label OrganizationNameLabel1;
        protected Literal OrganizationNameHelpText1;
        protected TextBox OrganizationName1;
        protected CustomValidator OrganizationNameValidator1;
        protected Image OrganizationNameTick1;
        protected Label EmailLabel1;
        protected TextBox Email1;
        protected CustomValidator EmailValidator1;
        protected Image EmailTick1;
        protected HtmlTableRow OrganizationUrlRow;
        protected Label OrganizationUrlLabel;
        protected Literal Schema;
        protected TextBox OrganizationUrl;
        protected Literal PartialCustomUrlRootAddress;
        protected Button Step1Button;
        protected CustomValidator OrganizationUrlValidator;
        protected Image OrganizationUrlTick;
        protected UpdatePanel UpdatePanelOrganizationUrl;

        protected HtmlGenericControl Step2Panel;
        protected HtmlTable Step2Form;
        protected HtmlTable Step2FormButton;
        protected Image LogoImage2;
        protected Literal OrganizationAddressLabel;
        protected Label OrganizationNameLabel2;
        protected TextBox OrganizationName2;
        protected CustomValidator OrganizationNameValidator2;
        protected Image OrganizationNameTick2;
        protected Label WebsiteLabel;
        protected TextBox Website;
        protected Label HowYouHearAboutUsLabel;
        protected TextBox HowYouHearAboutUs;

        protected Literal PersonalInformationLabel;
        protected Label FirstNameLabel;
        protected TextBox FirstName;
        protected Label LastNameLabel;
        protected TextBox LastName;

        protected Literal LocalSettingsLabel;
        protected Label TimeZoneLabel;
        protected DropDownList TimeZoneList;
        protected Label CurrencyLabel;
        protected DropDownList CurrencyList;

        protected Literal EmailAndPasswordLabel;
        protected Label EmailLabel2;
        protected TextBox Email2;
        protected CustomValidator EmailValidator2;
        protected Image EmailTick2;
        protected Label PasswordLabel;
        protected TextBox Password;
        protected CustomValidator PasswordValidator;
        protected Label ConfirmPasswordLabel;
        protected TextBox ConfirmPassword;
        protected CustomValidator PasswordCompareValidator;
        protected Label CaptchaLabel;
        protected Telerik.Web.UI.RadCaptcha Captcha1;
        protected CustomValidator CaptchaValidator;
        protected ValidationSummary Step2ValidationSummary;
        protected Button Step2Button;
        protected Literal AgreeLabel;

        protected HtmlGenericControl Step3Panel;
        protected Literal CustomizeLiteral;
        protected Repeater InstanceList;
        protected CustomValidator InstanceRequiredValidator;
        protected CustomValidator UniqueDataValidator;
        protected HtmlTable Step3Form;
        protected Button Step3Button;
        protected System.Web.UI.WebControls.TextBox SelectedInstance;

        protected HtmlGenericControl Step4Panel;
        protected Literal Step4TitleLiteral;
        protected Label ErrorLabel;
        protected Label ContinueLabel;
        protected HyperLink Step4Link;

        protected ObjectDataSource CountriesDataSource;
        protected ObjectDataSource InstanceListDataSource;

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

        private string PasswordCompareValidationClientScript
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"function PasswordCompareValidation(source, arguments) {{
    arguments.IsValid = true;
    var elem1 = document.getElementById('{0}_txt');
    var elem2 = document.getElementById('{1}_txt');
    if (elem1 && elem2) {{
        if ((elem1.value.replace(/^\s+$/gm, '').length == 0) || (elem2.value.replace(/^\s+$/gm, '').length == 0))
            arguments.IsValid = true;
        else
            arguments.IsValid = (elem2.value == elem1.value);
    }}
}}
"
                    , Password.ClientID, ConfirmPassword.ClientID);
            }
        }

        private string SelectItemClientScript
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"function SelectItem(elem, value) {{
    var items = elem.parentNode.getElementsByTagName('li');
    for (var y = 0; y < items.length; y++) {{
        if (items[y] != elem)
            items[y].className = 'Cb';
    }}
    var inp = document.getElementById('{0}');
    if (elem.className.indexOf('Cbc') > -1) {{
        elem.className = 'Cb';
        inp.value = '';
    }}
    else {{
        elem.className = 'Cbc';
        inp.value = value;
    }}
}}

function InstanceRequiredValidation(source, arguments) {{
    var elem = document.getElementById('{0}');
    arguments.IsValid = (elem.value.replace(/^\s+$/gm, '').length > 0);
}}
"
                    , SelectedInstance.ClientID);
            }
        }

        private string NewPassword
        {
            get { return (string)this.ViewState["NewPassword"]; }
            set { this.ViewState["NewPassword"] = value; }
        }

        private string WebSiteUrl
        {
            get
            {
                string websiteUrl = Website.Text;
                if (!string.IsNullOrEmpty(websiteUrl))
                {
                    if (!websiteUrl.StartsWith(Uri.UriSchemeHttp + Uri.SchemeDelimiter, StringComparison.OrdinalIgnoreCase))
                    {
                        if (!websiteUrl.StartsWith(Uri.UriSchemeHttps + Uri.SchemeDelimiter, StringComparison.OrdinalIgnoreCase))
                            websiteUrl = Uri.UriSchemeHttp + Uri.SchemeDelimiter + websiteUrl;
                    }
                }
                return websiteUrl;
            }
        }

        #endregion

        #region Private Methods

        private void LoadResources()
        {
            OrganizationName1.ErrorMessage = OrganizationName2.ErrorMessage = Resources.SignupOrganizationControl_OrganizationName1_ErrorMessage;
            OrganizationNameLabel1.Text = Resources.SignupOrganizationControl_OrganizationNameLabel1_Text;
            OrganizationNameHelpText1.Text = Resources.SignupOrganizationControl_OrganizationNameHelpText1_Text;
            OrganizationNameTick1.ImageUrl = OrganizationNameTick2.ImageUrl = EmailTick1.ImageUrl = EmailTick2.ImageUrl = OrganizationUrlTick.ImageUrl
                = ResourceProvider.GetImageUrl(typeof(SignupOrganizationControl), "Tick.png", true);
            Email1.ErrorMessage = Email2.ErrorMessage = Resources.SignupOrganizationControl_Email1_ErrorMessage;
            EmailLabel1.Text = Resources.SignupOrganizationControl_EmailLabel1_Text;
            OrganizationUrlLabel.Text = Resources.SignupOrganizationControl_OrganizationUrlLabel_Text;
            Schema.Text = Uri.UriSchemeHttps + Uri.SchemeDelimiter;
            PartialCustomUrlRootAddress.Text = "." + FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlRootAddressesFirst;
            OrganizationUrl.ErrorMessage = Resources.CustomUrlProvider_CustomUrlAlreadyExists;
            OrganizationUrlValidator.ErrorMessage = Resources.CustomUrlProvider_CustomUrlAlreadyExists;
            Step1Button.Text = Resources.SignupOrganizationControl_Step1Button_Text;

            OrganizationAddressLabel.Text = Resources.SignupOrganizationControl_OrganizationAddressLabel_Text;
            OrganizationNameLabel2.Text = Resources.SignupOrganizationControl_OrganizationNameLabel1_Text;
            Website.ValidationErrorMessage = Resources.SignupOrganizationControl_Website_ValidationErrorMessage;
            WebsiteLabel.Text = Resources.SignupOrganizationControl_WebsiteLabel_Text;
            HowYouHearAboutUsLabel.Text = Resources.SignupOrganizationControl_HowYouHearAboutUsLabel_Text;

            PersonalInformationLabel.Text = Resources.SignupOrganizationControl_PersonalInformationLabel_Text;
            FirstName.ErrorMessage = Resources.SignupOrganizationControl_FirstName_ErrorMessage;
            FirstNameLabel.Text = Resources.SignupOrganizationControl_FirstNameLabel_Text;
            LastName.ErrorMessage = Resources.SignupOrganizationControl_LastName_ErrorMessage;
            LastNameLabel.Text = Resources.SignupOrganizationControl_LastNameLabel_Text;

            LocalSettingsLabel.Text = Resources.SignupOrganizationControl_LocalSettingsLabel_Text;
            TimeZoneLabel.Text = Resources.SignupOrganizationControl_TimeZoneLabel_Text;
            CurrencyLabel.Text = Resources.SignupOrganizationControl_CurrencyLabel_Text;

            EmailAndPasswordLabel.Text = Resources.SignupOrganizationControl_EmailAndPasswordLabel_Text;
            EmailLabel2.Text = Resources.SignupOrganizationControl_EmailLabel1_Text;
            Password.ErrorMessage = Resources.SignupOrganizationControl_Password_ErrorMessage;
            PasswordLabel.Text = Resources.SignupOrganizationControl_PasswordLabel_Text;
            ConfirmPassword.ErrorMessage = Resources.SignupOrganizationControl_ConfirmPassword_ErrorMessage;
            ConfirmPasswordLabel.Text = Resources.SignupOrganizationControl_ConfirmPasswordLabel_Text;
            PasswordCompareValidator.ErrorMessage = Resources.SignupOrganizationControl_PasswordCompareValidator_ErrorMessage;
            CaptchaLabel.Text = Resources.SignupOrganizationControl_CaptchaLabel_Text;
            Step2ValidationSummary.HeaderText = Resources.SignupOrganizationControl_Step2ValidationSummary_HeaderText;
            Step2ValidationSummary.ForeColor = System.Drawing.Color.Empty;
            Step2Button.Text = string.Format(CultureInfo.InvariantCulture, Resources.SignupOrganizationControl_Step2Button_Text, FrameworkConfiguration.Current.WebApplication.Name);
            AgreeLabel.Text = string.Format(CultureInfo.InvariantCulture, Resources.SignupOrganizationControl_AgreeLabel_Text
                , BaseControl.GetHyperlink(FrameworkConfiguration.Current.WebApplication.Support.TermsUrl, Resources.SignupOrganizationControl_TermsLink_Text)
                , BaseControl.GetHyperlink(FrameworkConfiguration.Current.WebApplication.Support.PrivacyPolicyUrl, Resources.SignupOrganizationControl_PrivacyPolicyLink_Text));

            InstanceRequiredValidator.ErrorMessage = Resources.CheckBoxList_RequiredValidator_ErrorMessage;

            CustomizeLiteral.Text = Resources.SignupOrganizationControl_CustomizeLiteral_Text;
            Step3Button.Text = Resources.SignupOrganizationControl_Step3Button_Text;

            Step4TitleLiteral.Text = Resources.SignupOrganizationControl_Step4TitleLiteral_Text;
            ContinueLabel.Text = Resources.SignupOrganizationControl_ContinueLabel_Text;
            Step4Link.Text = Resources.SignupOrganizationControl_Step4Link_Text;

            if (string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.BigLogoImageUrl))
                LogoImage1.Visible = LogoImage2.Visible = false;
            else
                LogoImage1.ImageUrl = LogoImage2.ImageUrl = FrameworkConfiguration.Current.WebApplication.BigLogoImageUrl;
        }

        private void FillCurrencyList()
        {
            CurrencyList.Items.Add(string.Empty);
            foreach (CurrencyElement e in FrameworkConfiguration.Current.Currencies)
            {
                CurrencyList.Items.Add(new ListItem(e.Name, e.Code));
            }
        }

        private static bool ValidateOrganizationName(string organizationName, out string errorMessage)
        {
            errorMessage = null;
            bool isValid = (OrganizationProvider.GetOrganizationIdByName(organizationName) == Guid.Empty);
            if (!isValid)
                errorMessage = string.Format(CultureInfo.InvariantCulture, Resources.OrganizationProvider_ErrorMessage_OrganizationAlreadyExists, organizationName).TrimEnd('.');
            return isValid;
        }

        private static bool ValidateEmail(string email, out string errorMessage)
        {
            errorMessage = null;
            bool isValid = Support.ValidateEmail(email, false);
            if (isValid)
            {
                isValid = (!WebApplication.LoginProvider.ValidateLogin(email, null));
                if (!isValid)
                {
                    errorMessage = Resources.SignupOrganizationControl_EmailValidator1_ErrorMessage + "<br />"
                        + BaseControl.GetHyperlink(WebApplication.LoginProvider.GetLoginUrl(email, false), Resources.SignupOrganizationControl_LoginLink_Text, null, "_parent");
                }
            }
            else
                errorMessage = Resources.SignupOrganizationControl_Email1_ValidationErrorMessage;
            return isValid;
        }

        #endregion

        #region Overriden Methods

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Micajah.Common.Pages.MasterPage.AddGlobalStyleSheet(this.Page, MasterPageTheme.Modern);

            if (Step4Panel.Visible)
            {
                ResourceProvider.RegisterStyleSheetResource(this, ResourceProvider.GetDetailMenuThemeStyleSheet(DetailMenuTheme.Modern), DetailMenuTheme.Modern.ToString());
            }

            if (Step3Panel.Visible)
            {
                MagicForm.ApplyStyle(Step3Form, ColorScheme.White, false, false, MasterPageTheme.Modern);

                InstanceList.DataBind();
                if (InstanceList.Items.Count > 0)
                {
                    ResourceProvider.RegisterStyleSheetResource(this, ResourceProvider.GetDetailMenuThemeStyleSheet(DetailMenuTheme.Modern), DetailMenuTheme.Modern.ToString());

                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "SelectItemClientScript", this.SelectItemClientScript, true);

                    string code = FrameworkConfiguration.Current.WebApplication.Integration.Google.ConversionCode.Value;
                    if (!string.IsNullOrEmpty(code))
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "GoogleConversionClientScript", code, false);
                }
                else
                {
                    Step2Panel.Visible = true;
                    Step3Panel.Visible = false;

                    Step3Button_Click(Step3Button, EventArgs.Empty);
                }
            }

            if (Step1Panel.Visible)
            {
                MagicForm.ApplyStyle(Step1Form, ColorScheme.White, false, false, MasterPageTheme.Modern);

                this.Page.Form.Target = "_parent";

                LogoImage1.Style[HtmlTextWriterStyle.Display] = "none";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ShowLogoImage", this.ShowLogoImageClientScript, true);

                if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
                {
                    OrganizationName1.Attributes["onchange"] = "SetOrganizationUrl(this);";

                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "SetOrganizationUrlClientScript", this.SetOrganizationUrlClientScript, true);
                }
            }

            if (Step2Panel.Visible)
            {
                MagicForm.ApplyStyle(Step2Form, ColorScheme.White, false, false, MasterPageTheme.Modern);
                MagicForm.ApplyStyle(Step2FormButton, ColorScheme.White, false, false, MasterPageTheme.Modern);

                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "PasswordCompareValidationClientScript", this.PasswordCompareValidationClientScript, true);
            }

            this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl("Styles.SignupOrganization.css", true)));
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            Micajah.Common.Pages.MasterPage.SetPageTitle(this.Page, ActionProvider.PublicActions.FindByActionId(ActionProvider.SignUpOrganizationPageActionId));

            if (!this.Page.IsPostBack)
            {
                this.LoadResources();

                Step1Panel.Visible = false;
                Step2Panel.Visible = false;
                Step3Panel.Visible = false;
                Step4Panel.Visible = false;

                if (GoogleProvider.IsGoogleProviderRequest(Request))
                {
                    string returnUrl = null;

                    try
                    {
                        GoogleProvider.ProcessOAuth2AuthorizationResponse(Context, ref returnUrl);
                    }
                    catch (System.Security.Authentication.AuthenticationException ex)
                    {
                        ErrorLabel.Text = ex.Message;
                        Step4Link.NavigateUrl = returnUrl;
                        Step4Panel.Visible = true;

                        return;
                    }
                }

                // Use this hack for CustomValidator for Modern theme.
                PasswordCompareValidator.Attributes["controltovalidate2"] = ConfirmPassword.ClientID;
                InstanceRequiredValidator.Attributes["controltovalidate2"] = SelectedInstance.ClientID;

                OrganizationUrlRow.Visible = FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled;

                Step1Panel.Visible = true;

                BaseControl.TimeZoneListDataBind(TimeZoneList, null, false);
                this.FillCurrencyList();

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
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.IsValid = true;

                if (textBox.ID == OrganizationName1.ID)
                    OrganizationNameTick1.Visible = false;
                else if (textBox.ID == OrganizationName2.ID)
                    OrganizationNameTick2.Visible = false;
            }
            else
            {
                if (textBox.ID == OrganizationName1.ID)
                    OrganizationNameValidator1.Validate();
                else if (textBox.ID == OrganizationName2.ID)
                    OrganizationNameValidator2.Validate();
            }
        }

        protected void OrganizationNameValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args == null) return;

            CustomValidator val = (CustomValidator)source;
            TextBox textBox = null;
            Image tickImage = null;
            string errorMessage = null;

            if (val.ID == OrganizationNameValidator1.ID)
            {
                textBox = OrganizationName1;
                tickImage = OrganizationNameTick1;
            }
            else if (val.ID == OrganizationNameValidator2.ID)
            {
                textBox = OrganizationName2;
                tickImage = OrganizationNameTick2;
            }

            tickImage.Visible = textBox.IsValid = args.IsValid = ValidateOrganizationName(textBox.Text, out errorMessage);

            if (!args.IsValid)
            {
                val.ErrorMessage = errorMessage;

                textBox.Attributes["validatorId"] = val.ClientID;
                textBox.Focus();
            }
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
                else if (textBox.ID == Email2.ID)
                    EmailValidator2.Validate();
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
            else if (val.ID == EmailValidator2.ID)
            {
                textBox = Email2;
                tickImage = EmailTick2;
            }

            tickImage.Visible = textBox.IsValid = args.IsValid = ValidateEmail(textBox.Text, out errorMessage);

            if (!args.IsValid)
            {
                val.ErrorMessage = errorMessage;

                textBox.Attributes["validatorId"] = val.ClientID;
                textBox.Focus();
            }
        }

        protected void PasswordValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args == null) return;

            Password.IsValid = args.IsValid = false;

            try
            {
                Password.IsValid = args.IsValid = WebApplication.LoginProvider.ValidatePassword(Password.Text);
            }
            catch (ArgumentException ex)
            {
                PasswordValidator.ErrorMessage = ex.Message;

                Password.Attributes["validatorId"] = PasswordValidator.ClientID;
                Password.Focus();
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

        protected void UniqueDataValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args == null) return;

            string errorMessage = null;

            args.IsValid = ValidateOrganizationName(OrganizationName2.Text, out errorMessage);
            if (args.IsValid)
                args.IsValid = ValidateEmail(Email2.Text, out errorMessage);
            if (args.IsValid)
                try
                {
                    if (!string.IsNullOrEmpty(OrganizationUrl.Text))
                    {
                        CustomUrlProvider.ValidatePartialCustomUrl(OrganizationUrl.Text);
                        args.IsValid = CustomUrlProvider.ValidateCustomUrl(OrganizationUrl.Text);
                    }
                }
                catch (Exception ex) { errorMessage = ex.Message; }

            SelectedInstance.CssClass = SelectedInstance.CssClass.Replace(" Invalid", string.Empty);

            if (!args.IsValid)
            {
                UniqueDataValidator.ErrorMessage = errorMessage;

                SelectedInstance.CssClass += " Invalid";
                SelectedInstance.Attributes["validatorId"] = UniqueDataValidator.ClientID;
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
            if (!Page.IsValid) return;

            Step1Panel.Visible = false;
            Step2Panel.Visible = true;

            Email2.Text = Email1.Text;
            OrganizationName2.Text = OrganizationName1.Text;

            Website.Focus();
        }

        protected void Step2Button_Click(object sender, EventArgs e)
        {
            Page.Validate("Step2");
            if (!Page.IsValid) return;

            Step2Panel.Visible = false;
            Step3Panel.Visible = true;

            this.NewPassword = Password.Text;
        }

        protected void Step3Button_Click(object sender, EventArgs e)
        {
            Page.Validate("Step3");
            if (!Page.IsValid) return;

            Guid? templateInstanceId = null;
            if (!string.IsNullOrEmpty(SelectedInstance.Text))
                templateInstanceId = new Guid(SelectedInstance.Text);

            Guid orgId = OrganizationProvider.InsertOrganization(OrganizationName2.Text, null, this.WebSiteUrl
                , null, null, null, null, null, null, CurrencyList.SelectedValue
                , TimeZoneList.SelectedValue, templateInstanceId
                , Email2.Text, this.NewPassword, FirstName.Text, LastName.Text, null, null, null
                , OrganizationUrl.Text, this.Request
                , true);

            Guid instId = InstanceProvider.GetFirstInstanceId(orgId);

            WebApplication.RefreshAllData(false);

            if (GoogleProvider.IsGoogleProviderRequest(this.Request))
                GoogleProvider.ProcessOAuth2AuthorizationRequest(this.Context);

            Response.Redirect(WebApplication.LoginProvider.GetLoginUrl(Email2.Text
                , WebApplication.LoginProvider.EncryptPassword(this.NewPassword), orgId, instId, true
                , CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.StartPageVirtualPath)));
        }

        #endregion
    }
}