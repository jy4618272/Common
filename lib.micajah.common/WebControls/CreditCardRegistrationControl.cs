using ChargifyNET;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls
{
    public class CreditCardRegistrationControl : Micajah.Common.WebControls.SetupControls.MasterControl
    {
        public enum CreditCardRegistrationStatus
        {
            Ok,
            Reactivated,
            Error
        }

        #region Members

        protected TextBox txtCCNumber;
        protected TextBox txtCCExpMonth;
        protected TextBox txtCCExpYear;
        protected NoticeMessageBox msgStatus;
        protected CreditCardRegistrationStatus ccrStatus=CreditCardRegistrationStatus.Ok;
        protected Panel pnlMissingCard;

        #endregion

        #region Private Methods

        public event EventHandler UpdateClick;

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
            get { return ccrStatus; }
        }

        public bool ShowMissingCardTitle
        {
            get { return pnlMissingCard.Visible; }
            set { pnlMissingCard.Visible = value; }
        }

        #endregion

        private void RegisterFancyBoxInputScript()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "FancyBoxInputScript", "$(\"input[value=" + FancyboxInputValue + "]\").fancybox({type: 'inline', href: '#credit_card_form', width: 400, height: 'auto', showNavArrows: false, titlePosition: 'inside', transitionIn: 'none', transitionOut: 'none'});", true);
        }

        private void RegisterFancyBoxHyperLinkScript()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "FancyBoxHyperLinkScript", "$(\"a[rel=" + FancyboxHyperlinkRel + "]\").fancybox({type: 'inline', href: '#credit_card_form', width: 400, height: 'auto', showNavArrows: false, titlePosition: 'inside', transitionIn: 'none', transitionOut: 'none'});", true);
        }

        #endregion

        #region Protected Methods

        protected void btnUpdateCC_Click(object sender, EventArgs e)
        {
            msgStatus.Visible = true;
            msgStatus.MessageType = NoticeMessageType.Error;
            UserContext _uctx = UserContext.Current;
            string err=string.Empty;
            ccrStatus = CreditCardRegistrationStatus.Ok;

            if (!ChargifyProvider.RegisterCreditCard(ChargifyProvider.CreateChargify(), _uctx.OrganizationId, _uctx.InstanceId, _uctx.Organization.Name, _uctx.Instance.Name, _uctx.Email, _uctx.FirstName, _uctx.LastName, txtCCNumber.Text, txtCCExpMonth.Text, txtCCExpYear.Text, 1, out err))
            {
                if (txtCCNumber.Text.Contains("XXXX"))
                {
                    txtCCNumber.Text = string.Empty;
                    txtCCExpMonth.Text = string.Empty;
                    txtCCExpYear.Text = string.Empty;
                    msgStatus.Description = "Please, input correct data.";
                }
                msgStatus.Message = err;
                ccrStatus = CreditCardRegistrationStatus.Error;
                if (UpdateClick != null) UpdateClick(this, e);
                return;
            }

            InstanceProvider.UpdateInstance(_uctx.Instance, CreditCardStatus.Registered);

            if (txtCCNumber.Text.Contains("XXXX")) ccrStatus=CreditCardRegistrationStatus.Reactivated;

            if (UpdateClick != null) UpdateClick(this, e);
            else Response.Redirect(Request.Path);
        }

        #endregion

        #region Overriden Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (this.Visible)
            {
                this.MasterPage.EnableFancyBox = true;
                if (!string.IsNullOrEmpty(FancyboxHyperlinkRel)) RegisterFancyBoxHyperLinkScript();
                if (!string.IsNullOrEmpty(FancyboxInputValue)) RegisterFancyBoxInputScript();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (this.Visible) ResourceProvider.RegisterStyleSheetResource(this, ResourceProvider.CreditCardRegistrationStyleSheet, "CreditCardRegistrationStyleSheet", false);
        }

        #endregion

        public void SetCreditCardInformation(string CreditCardNumber, string ExpirationMonth, string ExpirationYear)
        {
            txtCCNumber.Text = CreditCardNumber;
            txtCCExpMonth.Text = ExpirationMonth;
            txtCCExpYear.Text = ExpirationYear;
        }
    }
}
