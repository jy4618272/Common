using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;

namespace Micajah.Common.WebControls.SecurityControls
{
    /// <summary>
    /// Provides user interface (UI) elements for password recovery.
    /// </summary>
    public class PasswordRecoveryControl : System.Web.UI.UserControl
    {
        #region Members

        protected HtmlGenericControl MainContainer;

        /// <summary>
        /// The table row that contains password recovery form.
        /// </summary>
        protected HtmlTable FormTable;

        /// <summary>
        /// The title label.
        /// </summary>
        protected Literal TitleLabel;

        /// <summary>
        /// The label associated with the text box to input login name.
        /// </summary>
        protected Label LoginLabel;

        /// <summary>
        /// The text box to input login name.
        /// </summary>
        protected TextBox LoginTextBox;

        /// <summary>
        /// The button to submit form.
        /// </summary>
        protected Button SubmitButton;

        /// <summary>
        /// The hyperlink to login page.
        /// </summary>
        protected LinkButton LogOnPageButton;

        /// <summary>
        /// The div to display an error message, if an error occured.
        /// </summary>
        protected HtmlGenericControl ErrorDiv;

        protected HtmlTable ResultTable;
        protected HtmlTableRow SuccessTableRow;
        protected Literal TitleLabel2;
        protected LinkButton LogOnPageButton2;
        protected PlaceHolder ButtonsSeparator;

        #endregion

        #region Private Methods

        private void LoadResources()
        {
            TitleLabel.Text = Resources.PasswordRecoveryControl_TitleLabel_Text;
            TitleLabel2.Text = Resources.PasswordRecoveryControl_TitleLabel_SuccessText;
            SubmitButton.Text = Resources.PasswordRecoveryControl_SubmitButton_Text;
            LogOnPageButton2.Text = LogOnPageButton.Text = Resources.PasswordRecoveryControl_LogOnPageLink_Text;
            LoginLabel.Text = FrameworkConfiguration.Current.WebApplication.Login.LoginLabelText;

            if (FrameworkConfiguration.Current.WebApplication.Integration.Ldap.Enabled)
            {

                LoginTextBox.ValidationExpression = FrameworkConfiguration.Current.WebApplication.Login.LoginValidationExpression;
                LoginTextBox.ValidationType = Micajah.Common.WebControls.CustomValidationDataType.RegularExpression;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Occurs when the page is being loaded.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Micajah.Common.Pages.MasterPage.CreatePageHeader(this.Page, false, true, false, false, true);
            AutoGeneratedButtonsField.InsertButtonSeparator(ButtonsSeparator);

            if (!IsPostBack)
            {
                Micajah.Common.Pages.MasterPage.SetPageTitle(this.Page, ActionProvider.FindAction(CustomUrlProvider.CreateApplicationAbsoluteUrl(Request.Url.PathAndQuery)));

                LoadResources();

                string loginName = Request.QueryString["l"];
                if (!string.IsNullOrEmpty(loginName))
                    LoginTextBox.Text = loginName;

                LoginTextBox.Focus();

                ResultTable.Visible = false;
            }

            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern)
            {
                this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.LogOnModernStyleSheet, true)));

                MagicForm.ApplyStyle(FormTable);
                MagicForm.ApplyStyle(ResultTable);
            }
            else
                this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.LogOnStyleSheet, true)));
        }

        /// <summary>
        /// Occurs when the submit button is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string loginName = LoginTextBox.Text;
            DataRowView drv = WebApplication.LoginProvider.GetLogin(loginName);
            if (drv != null)
            {
                WebApplication.LoginProvider.ResetPassword((Guid)drv["LoginId"]);

                SuccessTableRow.Visible = true;
            }
            else
            {
                SuccessTableRow.Visible = false;

                ErrorDiv.Visible = true;
                ErrorDiv.InnerHtml = Resources.PasswordRecoveryControl_TitleLabel_FailureText;
            }

            FormTable.Visible = false;
            ResultTable.Visible = true;

            MainContainer.Style[HtmlTextWriterStyle.Height] = "120px";
            MainContainer.Style[HtmlTextWriterStyle.MarginTop] = "-60px";
        }

        protected void LogOnPageButton_Click(object sender, EventArgs e)
        {
            string url = null;
            if (!string.IsNullOrEmpty(LoginTextBox.Text))
                url = WebApplication.LoginProvider.GetLoginUrl(LoginTextBox.Text, false);
            else
                url = WebApplication.LoginProvider.GetLoginUrl(false);
            Response.Redirect(url);
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
            LogOnControl.RenderHeader(writer);
            base.Render(writer);
        }

        #endregion
    }
}
