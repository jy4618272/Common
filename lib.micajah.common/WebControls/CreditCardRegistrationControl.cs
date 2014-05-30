using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls
{
    public class CreditCardRegistrationControl : Micajah.Common.WebControls.SetupControls.MasterControl
    {
        #region Members

        public enum CreditCardRegistrationStatus
        {
            Ok,
            Reactivated,
            Error
        }

        protected TextBox NumberTextBox;
        protected TextBox ExpirationMonthTextBox;
        protected TextBox ExpirationYearTextBox;
        protected NoticeMessageBox StatusMessageBox;
        protected Panel MissingCardPanel;
        protected Button UpdateButton;

        private CreditCardRegistrationStatus m_Status = CreditCardRegistrationStatus.Ok;

        #endregion

        #region Events

        public event EventHandler UpdateClick;

        #endregion

        #region Public Properties

        public string FancyboxInputValue
        {
            get { return ViewState["FancyboxInputValue"] != null ? (string)ViewState["FancyboxInputValue"] : string.Empty; }
            set { ViewState["FancyboxInputValue"] = value; }
        }

        public string FancyboxHyperlinkRel
        {
            get { return ViewState["FancyboxHyperlinkRel"] != null ? (string)ViewState["FancyboxHyperlinkRel"] : string.Empty; }
            set { ViewState["FancyboxHyperlinkRel"] = value; }
        }

        public CreditCardRegistrationStatus Status
        {
            get { return m_Status; }
        }

        public bool ShowMissingCardTitle
        {
            get { return MissingCardPanel.Visible; }
            set { MissingCardPanel.Visible = value; }
        }

        #endregion

        #region Private Methods

        private void RegisterFancyBoxInputScript()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "FancyBoxInputScript", "$(\"input[value=" + FancyboxInputValue + "]\").fancybox({type: 'inline', href: '#credit_card_form', width: 400, height: 'auto', showNavArrows: false, titlePosition: 'inside', transitionIn: 'none', transitionOut: 'none'});", true);
        }

        private void RegisterFancyBoxHyperLinkScript()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "FancyBoxHyperLinkScript", "$(\"a[rel=" + FancyboxHyperlinkRel + "]\").fancybox({type: 'inline', width: 400, height: 'auto', showNavArrows: false, titlePosition: 'inside', transitionIn: 'none', transitionOut: 'none'});", true);
        }

        #endregion

        #region Protected Methods

        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            m_Status = CreditCardRegistrationStatus.Ok;
            string error = string.Empty;
            UserContext user = UserContext.Current;

            StatusMessageBox.Visible = true;
            StatusMessageBox.MessageType = NoticeMessageType.Error;

            if (!ChargifyProvider.RegisterCreditCard(ChargifyProvider.CreateChargify()
                , user.OrganizationId, user.InstanceId, user.Organization.Name, user.Instance.Name, user.Email, user.FirstName, user.LastName
                , NumberTextBox.Text, ExpirationMonthTextBox.Text, ExpirationYearTextBox.Text, 1
                , out error))
            {
                if (NumberTextBox.Text.Contains("XXXX"))
                {
                    NumberTextBox.Text = string.Empty;
                    ExpirationMonthTextBox.Text = string.Empty;
                    ExpirationYearTextBox.Text = string.Empty;

                    StatusMessageBox.Description = Resources.CreditCardRegistrationControl_StatusMessageBox_Description;
                }

                StatusMessageBox.Message = error;

                m_Status = CreditCardRegistrationStatus.Error;

                if (UpdateClick != null)
                {
                    UpdateClick(this, e);
                }
            }
            else
            {
                InstanceProvider.UpdateInstance(user.Instance, CreditCardStatus.Registered);

                if (NumberTextBox.Text.Contains("XXXX"))
                {
                    m_Status = CreditCardRegistrationStatus.Reactivated;
                }

                if (UpdateClick != null)
                {
                    UpdateClick(this, e);
                }
                else
                {
                    Response.Redirect(Request.Path);
                }
            }
        }

        #endregion

        #region Overriden Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (this.Visible)
            {
                this.MasterPage.EnableFancyBox = true;

                if (!string.IsNullOrEmpty(FancyboxHyperlinkRel))
                {
                    RegisterFancyBoxHyperLinkScript();
                }

                if (!string.IsNullOrEmpty(FancyboxInputValue))
                {
                    RegisterFancyBoxInputScript();
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (this.Visible)
            {
                ResourceProvider.RegisterStyleSheetResource(this, ResourceProvider.CreditCardRegistrationStyleSheet, "CreditCardRegistrationStyleSheet", false);
            }
        }

        #endregion

        #region Public Methods

        public void SetCreditCardInformation(string creditCardNumber, string expirationMonth, string expirationYear)
        {
            NumberTextBox.Text = creditCardNumber;
            ExpirationMonthTextBox.Text = expirationMonth;
            ExpirationYearTextBox.Text = expirationYear;
        }

        #endregion
    }
}
