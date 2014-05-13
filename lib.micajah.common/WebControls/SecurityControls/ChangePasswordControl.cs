using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;
using Micajah.Common.Security;

namespace Micajah.Common.WebControls.SecurityControls
{
    /// <summary>
    /// Provides a user interface that enable users to change their Web site password.
    /// </summary>
    public class ChangePasswordControl : UserControl
    {
        #region Members

        internal protected MagicForm EditForm;
        protected Label LoginIdLabel;
        protected ObjectDataSource PasswordDataSource;

        private TextBox m_CurrentPassword;
        private Label m_CurrentPasswordErrorLabel;
        private TextBox m_NewPassword;
        private TextBox m_ConfirmNewPassword;
        private Label m_ChangePasswordErrorLabel;
        private CustomValidator m_PasswordCompareValidator;

        #endregion

        #region Private Properties

        private string ClientScripts
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (NewPassword != null && ConfirmNewPassword != null && ChangePasswordErrorLabel != null)
                {
                    sb.Append("function PasswordCompareValidation(source, arguments) { arguments.IsValid = true; ");
                    sb.AppendFormat("var Elem1 = document.getElementById('{0}_txt'); ", NewPassword.ClientID);
                    sb.AppendFormat("var Elem2 = document.getElementById('{0}_txt'); ", ConfirmNewPassword.ClientID);
                    sb.AppendFormat("var Elem3 = document.getElementById('{0}'); ", ChangePasswordErrorLabel.ClientID);
                    sb.Append("if (Elem1 && Elem2) { arguments.IsValid = (Elem2.value == Elem1.value); if (Elem3) Elem3.style.display = 'none'; } }\r\n");
                }

                return sb.ToString();
            }
        }

        private TextBox CurrentPassword
        {
            get
            {
                if (m_CurrentPassword == null)
                    m_CurrentPassword = (EditForm.FindControl("CurrentPassword") as TextBox);
                return m_CurrentPassword;
            }
        }

        private Label CurrentPasswordErrorLabel
        {
            get
            {
                if (m_CurrentPasswordErrorLabel == null)
                    m_CurrentPasswordErrorLabel = EditForm.FindControl("CurrentPasswordErrorLabel") as Label;
                return m_CurrentPasswordErrorLabel;
            }
        }

        private TextBox NewPassword
        {
            get
            {
                if (m_NewPassword == null)
                    m_NewPassword = (EditForm.FindControl("NewPassword") as TextBox);
                return m_NewPassword;
            }
        }

        private TextBox ConfirmNewPassword
        {
            get
            {
                if (m_ConfirmNewPassword == null)
                    m_ConfirmNewPassword = (EditForm.FindControl("ConfirmNewPassword") as TextBox);
                return m_ConfirmNewPassword;
            }
        }

        private Label ChangePasswordErrorLabel
        {
            get
            {
                if (m_ChangePasswordErrorLabel == null)
                    m_ChangePasswordErrorLabel = EditForm.FindControl("ChangePasswordErrorLabel") as Label;
                return m_ChangePasswordErrorLabel;
            }
        }

        private CustomValidator PasswordCompareValidator
        {
            get
            {
                if (m_PasswordCompareValidator == null)
                    m_PasswordCompareValidator = (EditForm.FindControl("PasswordCompareValidator") as CustomValidator);
                return m_PasswordCompareValidator;
            }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets the password comparison error message from resources.
        /// </summary>
        protected static string PasswordCompareErrorMessage
        {
            get { return Resources.ChangePasswordControl_EditForm_PasswordCompareValidator_ErrorMessage; }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the value indicating whether the current user's password should be validated before changing the password.
        /// </summary>
        [DefaultValue(false)]
        public bool ValidateCurrentPassword
        {
            get { return EditForm.Fields[0].Visible; }
            set { EditForm.Fields[0].Visible = value; }
        }

        public Guid LoginId
        {
            get
            {
                object obj = Support.ConvertStringToType(LoginIdLabel.Text, typeof(Guid));
                return ((obj == null) ? Guid.Empty : (Guid)obj);
            }
            set { LoginIdLabel.Text = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the value indicating the mode in which a close button is shown.
        /// </summary>
        [DefaultValue(CloseButtonVisibilityMode.Always)]
        public CloseButtonVisibilityMode ShowCloseButton
        {
            get { return EditForm.ShowCloseButton; }
            set { EditForm.ShowCloseButton = value; }
        }

        /// <summary>
        /// Gets or sets the name of the object.
        /// </summary>
        [DefaultValue("")]
        public string ObjectName
        {
            get { return EditForm.ObjectName; }
            set { EditForm.ObjectName = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether the e-mail field are rendered.
        /// </summary>
        [DefaultValue(false)]
        public bool ShowLogOnNameInCaption
        {
            get
            {
                object obj = this.ViewState["ShowLogOnNameInCaption"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { this.ViewState["ShowLogOnNameInCaption"] = value; }
        }

        #endregion

        #region Events

        public event EventHandler PasswordUpdated;
        public event EventHandler PasswordUpdateCanceled;

        #endregion

        #region Private Methods

        private void PasswordUpdateOnCancel()
        {
            if (PasswordUpdateCanceled != null)
                PasswordUpdateCanceled(this, EventArgs.Empty);
            else
                this.Redirect();
        }

        private void Redirect()
        {
            Micajah.Common.Bll.Action action = ActionProvider.PagesAndControls.FindByActionId(ActionProvider.MyAccountPageActionId);
            Response.Redirect((action != null) ? action.AbsoluteNavigateUrl : UserContext.Current.StartPageUrl);
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EditForm.Fields[0].HeaderText = Resources.ChangePasswordControl_EditForm_CurrentPasswordField_HeaderText;
                EditForm.Fields[1].HeaderText = Resources.ChangePasswordControl_EditForm_NewPasswordField_HeaderText;
                EditForm.Fields[2].HeaderText = Resources.ChangePasswordControl_EditForm_ConfirmNewPasswordField_HeaderText;

                LoginIdLabel.Text = UserContext.Current.UserId.ToString();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(EditForm.ObjectName))
                EditForm.ObjectName = Resources.ChangePasswordControl_EditForm_ObjectName;
        }

        protected void EditForm_DataBound(object sender, EventArgs e)
        {
            if (this.ShowLogOnNameInCaption)
                EditForm.Caption = MagicForm.GetCaption(EditForm.CurrentMode, string.Format(CultureInfo.InvariantCulture, Resources.ChangePasswordControl_EditForm_CaptionFormat, DataBinder.Eval(EditForm.DataItem, "LoginName")));

            if (EditForm.CurrentMode != DetailsViewMode.ReadOnly)
            {
                if ((PasswordCompareValidator != null) && (ConfirmNewPassword != null))
                    m_PasswordCompareValidator.Attributes["controltovalidate2"] = m_ConfirmNewPassword.ClientID;
            }
        }

        protected void EditForm_ItemUpdating(object sender, CancelEventArgs e)
        {
            if (e == null) return;

            if (string.IsNullOrEmpty(NewPassword.Text))
            {
                e.Cancel = true;
                this.PasswordUpdateOnCancel();
            }
            else if (this.ValidateCurrentPassword)
            {
                string msg = null;
                if (string.IsNullOrEmpty(CurrentPassword.Text))
                    msg = Resources.ChangePasswordControl_CurrentPasswordRequired;
                else if (!LoginProvider.Current.ValidateLogin(this.LoginId, CurrentPassword.Text))
                    msg = Resources.ChangePasswordControl_CurrentPasswordIsNotValid;

                if (msg != null)
                {
                    CurrentPasswordErrorLabel.Text = msg;
                    CurrentPasswordErrorLabel.Visible = true;
                    e.Cancel = true;
                    return;
                }
            }
        }

        protected void EditForm_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            if (e == null) return;

            e.KeepInEditMode = true;

            if (e.Exception != null)
            {
                e.ExceptionHandled = true;

                ChangePasswordErrorLabel.Text = e.Exception.GetBaseException().Message;
                ChangePasswordErrorLabel.Visible = true;
            }
            else
            {
                if (PasswordUpdated != null)
                    PasswordUpdated(this, EventArgs.Empty);
                else
                    this.Redirect();
            }
        }

        protected void EditForm_ItemCommand(object sender, CommandEventArgs e)
        {
            if (e != null && e.CommandName.Equals("Cancel", StringComparison.OrdinalIgnoreCase))
            {
                this.PasswordUpdateOnCancel();
            }
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Raises the PreRender event and registers client scripts.
        /// </summary>
        /// <param name="e">An EventArgs that contains no event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string scripts = ClientScripts;
            if (!string.IsNullOrEmpty(scripts))
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ClientScripts", scripts, true);
        }

        #endregion
    }
}
