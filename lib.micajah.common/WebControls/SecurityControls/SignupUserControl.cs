using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Properties;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls.SecurityControls
{
    public class SignupUserControl : UserControl
    {
        #region Members

        protected HtmlGenericControl MainContainer;
        protected HtmlGenericControl ErrorDiv;
        protected CustomValidator PasswordCompareValidator;
        protected Label TitleLabel;
        protected Panel DescriptionPanel;
        protected Label DescriptionLabel;
        protected HtmlTable Step1Table;
        protected Label LoginLabel;
        protected TextBox LoginTextBox;
        protected Button SubmitButton1;
        protected HtmlTable Step2Table;
        protected Label LoginLabel1;
        protected Literal LoginTextLabel;
        protected Label FirstNameLabel;
        protected TextBox FirstNameTextBox;
        protected Label LastNameLabel;
        protected TextBox LastNameTextBox;
        protected Label PasswordLabel;
        protected TextBox PasswordTextBox;
        protected HtmlTableRow ConfirmPasswordRow;
        protected Label ConfirmPasswordLabel;
        protected TextBox ConfirmPasswordTextBox;
        protected Button SubmitButton2;
        protected LinkButton Step1Button;
        protected HyperLink LogOnPageLink1;
        protected HyperLink LogOnPageLink2;
        protected HyperLink LogOnPageLink3;
        protected PlaceHolder ButtonsSeparator0;
        protected PlaceHolder Step1ButtonHolder;
        protected PlaceHolder ButtonsSeparator1;
        protected PlaceHolder ButtonsSeparator2;
        protected Label CaptchaLabel;
        protected Telerik.Web.UI.RadCaptcha CaptchaControl;
        protected CustomValidator CaptchaValidator;

        private Organization m_Organization;
        private Guid? m_InvitedLoginId;

        #endregion

        #region Private Properties

        private string LoginName
        {
            get { return (string)this.ViewState["LoginName"]; }
            set { this.ViewState["LoginName"] = value; }
        }

        private Guid InvitedLoginId
        {
            get
            {
                if (!m_InvitedLoginId.HasValue)
                {
                    object obj = Support.ConvertStringToType(Request.QueryString["i"], typeof(Guid));
                    m_InvitedLoginId = ((obj == null) ? Guid.Empty : (Guid)obj);
                }
                return m_InvitedLoginId.Value;
            }
        }

        private Guid OrganizationId
        {
            get
            {
                object obj = this.ViewState["OrganizationId"];
                if (obj == null)
                {
                    if (this.InvitedLoginId == Guid.Empty)
                    {
                        string str = Request.QueryString["o"];
                        obj = Support.ConvertStringToType(str, typeof(Guid));
                    }
                    else
                    {
                        WebApplication.LoginProvider.DeleteExpiredInvitation();
                        MasterDataSet.InvitedLoginDataTable table = LoginProvider.GetInvitedLogin(this.InvitedLoginId);
                        if (table.Count == 1)
                        {
                            MasterDataSet.InvitedLoginRow row = table[0];
                            obj = row.OrganizationId;
                            this.GroupId = row.GroupId;
                            this.LoginName = row.LoginName;
                        }
                    }
                    if (obj == null) obj = Guid.Empty;
                    this.ViewState["OrganizationId"] = obj;
                }
                return (Guid)obj;
            }
        }

        public Guid InstanceId
        {
            get
            {
                object obj = Support.ConvertStringToType(Request.QueryString["d"], typeof(Guid));
                return ((obj == null) ? Guid.Empty : (Guid)obj);
            }
        }

        private string GroupId
        {
            get
            {
                object obj = this.ViewState["GroupId"];
                if (obj == null)
                {
                    Guid groupId = GroupProvider.GetGroupIdOfLowestRoleInInstance(this.OrganizationId, this.InstanceId);
                    if (groupId != Guid.Empty)
                        obj = groupId.ToString();
                }
                return (string)obj;
            }
            set { this.ViewState["GroupId"] = value; }
        }

        #endregion

        #region Private Methods

        private void LoadResources()
        {
            TitleLabel.Text = Resources.SignupUserControl_TitleLabel_Text;
            PasswordCompareValidator.ErrorMessage = Resources.SignupUserControl_PasswordCompareValidator_ErrorMessage;
            SubmitButton1.Text = SubmitButton2.Text = Resources.SignupUserControl_SubmitButton_Text;
            FirstNameLabel.Text = Resources.UsersControl_EditForm_FirstNameField_HeaderText;
            LastNameLabel.Text = Resources.UsersControl_EditForm_LastNameField_HeaderText;
            PasswordLabel.Text = FrameworkConfiguration.Current.WebApplication.Login.PasswordLabelText;
            ConfirmPasswordLabel.Text = Resources.SignupUserControl_ConfirmPasswordLabel_Text;
            Step1Button.Text = Resources.SignupUserControl_Step1Button_Text;
            LogOnPageLink2.Text = LogOnPageLink1.Text = Resources.SignupUserControl_LoginPageLink_Text_ReturnToLoginPage;
            CaptchaLabel.Text = Resources.SignupUserControl_CaptchaLabel_Text;

            if (FrameworkConfiguration.Current.WebApplication.Integration.Ldap.Enabled)
                LoginLabel.Text = LoginLabel1.Text = Resources.LoginElement_LdapLoginLabelText;
            else
                LoginLabel.Text = LoginLabel1.Text = FrameworkConfiguration.Current.WebApplication.Login.LoginLabelText;
        }

        private void EnsureOrganization()
        {
            Guid orgId = this.OrganizationId;
            if (orgId != Guid.Empty)
                m_Organization = OrganizationProvider.GetOrganization(orgId);
            if (m_Organization == null)
                this.RedirectToLoginPage();
        }

        private void ShowErrorMessage(string message)
        {
            ErrorDiv.InnerHtml = message;
            ErrorDiv.Visible = true;
        }

        private void ShowDescription(string description)
        {
            DescriptionLabel.Text = description;
            DescriptionPanel.Visible = true;
        }

        private void HideDescription()
        {
            DescriptionLabel.Text = string.Empty;
            DescriptionPanel.Visible = false;
        }

        private int ValidateLoginName(string loginName)
        {
            int returnValue = 0;
            ConfirmPasswordRow.Visible = true;
            if (WebApplication.LoginProvider.LoginNameExists(loginName))
            {
                if (WebApplication.LoginProvider.LoginInOrganization(loginName, this.OrganizationId))
                {
                    this.ShowErrorMessage(string.Format(CultureInfo.CurrentCulture, Resources.SignupUserControl_ErrorMessage_LoginInOrganizationExists, m_Organization.Name));
                    return 1;
                }

                ClientDataSet.UserRow userRow = UserProvider.GetUserRow(loginName);
                if (userRow != null)
                {
                    FirstNameTextBox.Text = userRow.FirstName;
                    LastNameTextBox.Text = userRow.LastName;
                }

                ConfirmPasswordRow.Visible = false;

                this.ShowDescription(string.Format(CultureInfo.CurrentCulture
                       , Resources.SignupUserControl_DescriptionLabel_Text_UserExists
                       , string.Concat(CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.PasswordRecoveryPageVirtualPath), "?l=", HttpUtility.UrlEncodeUnicode(loginName))));

                returnValue = 2;
            }

            LoginTextLabel.Text = loginName;
            FirstNameTextBox.Focus();
            Step1Table.Visible = false;
            Step2Table.Visible = true;

            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern)
            {
                MainContainer.Style[HtmlTextWriterStyle.Height] = "500px";
                MainContainer.Style[HtmlTextWriterStyle.MarginTop] = "-250px";
            }
            else
            {
                MainContainer.Style[HtmlTextWriterStyle.Height] = "360px";
                MainContainer.Style[HtmlTextWriterStyle.MarginTop] = "-180px";
            }

            return returnValue;
        }

        private void RedirectToLoginPage()
        {
            Response.Redirect(LogOnPageLink1.NavigateUrl);
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            Micajah.Common.Pages.MasterPage.CreatePageHeader(this.Page, false, true, false, false, true);
            AutoGeneratedButtonsField.InsertButtonSeparator(ButtonsSeparator0);
            AutoGeneratedButtonsField.InsertButtonSeparator(ButtonsSeparator1);
            AutoGeneratedButtonsField.InsertButtonSeparator(ButtonsSeparator2);

            Micajah.Common.Pages.MasterPage.SetPageTitle(this.Page, Resources.SignupUserControl_TitleLabel_Text);

            if (!IsPostBack)
            {
                string url = WebApplication.LoginProvider.GetLoginUrl(false);
                if (this.InvitedLoginId == Guid.Empty)
                {
                    if (!string.IsNullOrEmpty(Request.Url.Query))
                        url += Request.Url.Query;
                }
                LogOnPageLink2.NavigateUrl = LogOnPageLink1.NavigateUrl = url;

                this.EnsureOrganization();

                this.LoadResources();

                if (this.LoginName != null)
                {
                    int errorCode = ValidateLoginName(this.LoginName);
                    if (errorCode > 0)
                    {
                        LoginTextBox.Text = this.LoginName;
                        LoginTextBox.ReadOnly = true;
                        SubmitButton1.Enabled = false;
                    }
                    Step1ButtonHolder.Visible = false;
                }
            }
            else
                this.EnsureOrganization();


            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern)
            {
                this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.LogOnModernStyleSheet, true)));

                MagicForm.ApplyStyle(Step1Table);
                MagicForm.ApplyStyle(Step2Table);

                PasswordCompareValidator.Attributes["controltovalidate2"] = ConfirmPasswordTextBox.ClientID;
            }
            else
                this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.LogOnStyleSheet, true)));
        }

        protected void SubmitButton1_Click(object sender, EventArgs e)
        {
            FirstNameTextBox.Text = LastNameTextBox.Text = string.Empty;
            ValidateLoginName(LoginTextBox.Text);
        }

        protected void SubmitButton2_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string loginName = LoginTextLabel.Text;
                    string password = PasswordTextBox.Text;
                    Guid userId = Guid.Empty;

                    if (ConfirmPasswordRow.Visible)
                        userId = UserProvider.AddUserToOrganization(loginName, FirstNameTextBox.Text, LastNameTextBox.Text, string.Empty, this.GroupId, this.OrganizationId, password);
                    else
                    {
                        if (WebApplication.LoginProvider.ValidateLogin(loginName, password))
                            userId = UserProvider.AddUserToOrganization(loginName, FirstNameTextBox.Text, LastNameTextBox.Text, string.Empty, this.GroupId, this.OrganizationId);
                        else
                            throw new ArgumentException(Resources.SignupUserControl_ErrorMessage_PasswordSuppliedDoesNotMatchCurrentPassword);
                    }

                    UserProvider.RaiseUserInserted(userId, this.OrganizationId, this.InstanceId, Support.ConvertStringToGuidList(this.GroupId));

                    if (this.InvitedLoginId != Guid.Empty)
                        WebApplication.LoginProvider.CancelInvitation(this.InvitedLoginId);

                    LogOnPageLink3.Text = Resources.SignupUserControl_LoginPageLink_Text_ClickHereToLogin;
                    LogOnPageLink3.NavigateUrl = WebApplication.LoginProvider.GetLoginUrl(loginName, false);
                    LogOnPageLink3.Visible = true;

                    TitleLabel.Text = Resources.SignupUserControl_TitleLabel_SuccessText;
                    this.ShowDescription(string.Format(CultureInfo.CurrentCulture, Resources.SignupUserControl_DescriptionLabel_Text_AddUserToOrganization, loginName));

                    Step2Table.Visible = false;

                    MainContainer.Style[HtmlTextWriterStyle.Height] = "170px";
                    MainContainer.Style[HtmlTextWriterStyle.MarginTop] = "-85px";
                }
                catch (ArgumentException ex)
                {
                    ShowErrorMessage(ex.Message);
                }
            }
        }

        protected void Step1Button_Click(object sender, EventArgs e)
        {
            this.HideDescription();

            Step1Table.Visible = true;
            Step2Table.Visible = false;
            MainContainer.Style[HtmlTextWriterStyle.Height] = "170px";
            MainContainer.Style[HtmlTextWriterStyle.MarginTop] = "-85px";
        }

        protected void CaptchaValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args == null) return;

            args.IsValid = CaptchaControl.IsValid;

            System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)CaptchaControl.FindControl("CaptchaTextBox");
            CaptchaControl.CaptchaTextBoxCssClass = CaptchaControl.CaptchaTextBoxCssClass.Replace(" Invalid", string.Empty);

            if (args.IsValid)
                textBox.Attributes.Remove("validatorId");
            else
            {
                CaptchaValidator.ErrorMessage = Resources.SignupUserControl_CaptchaControl_ErrorMessage;

                CaptchaControl.CaptchaTextBoxCssClass += " Invalid";
                textBox.Attributes["validatorId"] = CaptchaValidator.ClientID;
                textBox.Focus();
            }
        }

        #endregion

        #region Overriden Methods

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern)
                ResourceProvider.RegisterValidatorScriptResource(this.Page);
        }

        /// <summary>
        /// Renders the control.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            LogOnControl.RenderHeader(writer, this.OrganizationId, this.InstanceId);
            base.Render(writer);
        }

        #endregion
    }
}
