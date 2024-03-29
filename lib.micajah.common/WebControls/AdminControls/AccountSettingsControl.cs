﻿using ChargifyNET;
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

namespace Micajah.Common.WebControls.AdminControls
{
    public class AccountSettingsControl : Micajah.Common.WebControls.SetupControls.MasterControl
    {
        #region Members

        protected HtmlControl divCCInfo;
        protected Button btnUpdateBillingPlan;
        protected Button btnCancelMyAccount;
        protected Literal lBillingPlanName;
        protected Literal lSumPerMonth;
        protected Literal lCCStatus;
        protected HtmlGenericControl smallNextBillDate;
        protected Literal lPhoneSupport;
        protected Label lblPurchase;
        protected Label lblPurchaseSum;
        protected Literal lAccountUsage;
        protected Repeater Repeater1;
        protected HtmlContainerControl divPaidUsageHeader;
        protected Repeater Repeater2;
        protected HtmlContainerControl divFreeUsageHeader;
        protected Repeater Repeater3;
        protected Repeater Repeater4;

        protected Label lblTraining1HourPrice;
        protected Label lblTraining3HoursPrice;
        protected Label lblTraining8HoursPrice;

        protected Button btnPurchase1Hour;
        protected Button btnPurchase3Hours;
        protected Button btnPurchase8Hours;
        protected HiddenField hfPurchaseTrainingHours;

        protected HyperLink CancelLink;
        protected PlaceHolder ButtonsSeparator;
        protected PlaceHolder phPhoneSupportToolTip;
        protected CheckBox chkPhoneSupport;
        protected HtmlContainerControl divPhoneSupport;
        protected HtmlContainerControl divCancelAccountHeader;
        protected HtmlContainerControl divCancelAccount;
        protected HtmlContainerControl divPaymentHistoryHeader;
        protected HtmlContainerControl divPaymentUpdate;
        protected HtmlContainerControl divAccountHead;
        protected HtmlContainerControl divAccountType;
        protected HtmlContainerControl divTrainingHeader;
        protected HtmlContainerControl divTraining;

        protected CreditCardRegistrationControl ccrControl;

        protected CommonGridView cgvTransactList;

        protected RadToolTip RadToolTip1;

        private SettingCollection m_PaidSettings;
        private SettingCollection m_CounterSettings;
        private decimal m_TotalSum = 0;
        private Guid m_OrgId = Guid.Empty;
        private Guid m_InstId = Guid.Empty;

        protected ChargifyConnect m_Chargify = null;
        protected bool ChargifyEnabled = FrameworkConfiguration.Current.WebApplication.Integration.Chargify.Enabled;

        #endregion

        #region Private Properties

        private Guid OrganizationId
        {
            get
            {
                if (m_OrgId == Guid.Empty) m_OrgId = UserContext.Current.OrganizationId;
                return m_OrgId;
            }
        }

        private Guid InstanceId
        {
            get
            {
                if (m_InstId == Guid.Empty) m_InstId = UserContext.Current.InstanceId;
                return m_InstId;
            }
        }

        private SettingCollection PaidSettings
        {
            get
            {
                if (m_PaidSettings == null) m_PaidSettings = SettingProvider.GetPaidSettings(OrganizationId, InstanceId);
                return m_PaidSettings;
            }
        }

        private SettingCollection CounterSettings
        {
            get
            {
                if (m_CounterSettings == null) m_CounterSettings = SettingProvider.GetCounterSettings(OrganizationId, InstanceId);
                return m_CounterSettings;
            }
        }

        private BillingPlan CurrentBillingPlan
        {
            get
            {
                return UserContext.Current.Instance.BillingPlan;
            }
        }

        #endregion

        #region Protected Properties

        protected decimal TotalAmount
        {
            get { return ViewState["TotalAmount"] == null ? 0 : (decimal)ViewState["TotalAmount"]; }
            set { ViewState["TotalAmount"] = value; }
        }

        protected int SubscriptionId
        {
            get { return ViewState["SubscriptionId"] == null ? 0 : (int)ViewState["SubscriptionId"]; }
            set { ViewState["SubscriptionId"] = value; }
        }

        protected ChargifyConnect Chargify
        {
            get { if (m_Chargify == null) m_Chargify = ChargifyProvider.CreateChargify(); return m_Chargify; }
        }

        protected bool IsNewSubscription
        {
            get { return ViewState["IsNewSubscription"] == null ? true : (bool)ViewState["IsNewSubscription"]; }
            set { ViewState["IsNewSubscription"] = value; }
        }

        #endregion

        #region Private Methods

        private void DisablePurchaseButtons()
        {
            ccrControl.FancyboxInputValue = "Purchase";
            btnPurchase1Hour.OnClientClick = "$(\"#" + btnPurchase1Hour.ClientID + "\").value=\"1\"; return false;";
            btnPurchase3Hours.OnClientClick = "$(\"#" + btnPurchase3Hours.ClientID + "\").value=\"3\"; return false;";
            btnPurchase8Hours.OnClientClick = "$(\"#" + btnPurchase8Hours.ClientID + "\").value=\"8\"; return false;";
        }

        private void EnsureActiveInstance()
        {
            if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
            {
                UserContext user = UserContext.Current;
                if (user != null)
                {
                    if (user.InstanceId == Guid.Empty)
                        Response.Redirect(ResourceProvider.GetActiveInstanceUrl(Request.Url.PathAndQuery, false));
                }
            }
        }

        private void InitPhoneSupport()
        {
            SettingCollection settings = this.PaidSettings;
            Setting setting = settings["PhoneSupport"];
            if (setting == null)
            {
                divPhoneSupport.Visible = false;
                return;
            }

            lPhoneSupport.Text = FrameworkConfiguration.Current.WebApplication.Support.Phone;
            bool isChecked = false;
            if (!Boolean.TryParse(setting.Value, out isChecked))
            {
                if (!Boolean.TryParse(setting.DefaultValue, out isChecked)) isChecked = false;
            }
            if (isChecked && setting.Paid && setting.Price > 0) m_TotalSum += setting.Price;
            if (!IsPostBack) chkPhoneSupport.Checked = isChecked;
        }

        private void List_DataBind()
        {
            if (!ChargifyEnabled || CurrentBillingPlan == BillingPlan.Custom)
            {
                if (CurrentBillingPlan == BillingPlan.Custom)
                {
                    lCCStatus.Text = "Custom Billing Plan";
                    divAccountType.Visible = false;
                }

                divAccountHead.Visible = ChargifyEnabled;
                divPaymentUpdate.Visible = false;
                divTrainingHeader.Visible = false;
                divTraining.Visible = false;
                divCancelAccountHeader.Visible = false;
                divCancelAccount.Visible = false;
                divPaymentHistoryHeader.Visible = false;
                cgvTransactList.Visible = false;
                divPhoneSupport.Visible = false;

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("SettingName", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("UsageCount", Type.GetType("System.Int32")));
                foreach (Setting setting in this.CounterSettings)
                {
                    if (setting.Paid)
                    {
                        bool isChecked;
                        if (!Boolean.TryParse(setting.Value, out isChecked))
                        {
                            if (!Boolean.TryParse(setting.DefaultValue, out isChecked)) isChecked = false;
                        }
                        if (!isChecked) continue;
                    }
                    DataRow row = dt.NewRow();
                    row["SettingName"] = setting.CustomName;
                    int usageCount;
                    int.TryParse(setting.Value, out usageCount);
                    row["UsageCount"] = usageCount;
                    dt.Rows.Add(row);
                }
                lAccountUsage.Text = "Account Usage";
                divFreeUsageHeader.Visible = false;
                Repeater2.DataSource = dt;
                Repeater2.DataBind();
                Repeater3.Visible = false;
                Repeater4.Visible = false;
            }
            else
            {
                Repeater2.Visible = false;
                InitPhoneSupport();
                InitBillingControls();

                DataTable dtPaid = new DataTable();
                dtPaid.Columns.Add(new DataColumn("SettingName", Type.GetType("System.String")));
                dtPaid.Columns.Add(new DataColumn("UsageCount", Type.GetType("System.Int32")));
                dtPaid.Columns.Add(new DataColumn("UsageCountLimit", Type.GetType("System.Int32")));
                dtPaid.Columns.Add(new DataColumn("Price", Type.GetType("System.Decimal")));
                dtPaid.Columns.Add(new DataColumn("SettingDescription", Type.GetType("System.String")));


                DataTable dtFree = new DataTable();
                dtFree.Columns.Add(new DataColumn("SettingName", Type.GetType("System.String")));
                dtFree.Columns.Add(new DataColumn("UsageCount", Type.GetType("System.Int32")));
                dtFree.Columns.Add(new DataColumn("UsageCountLimit", Type.GetType("System.Int32")));
                dtFree.Columns.Add(new DataColumn("UsagePersent", Type.GetType("System.Int32")));

                foreach (Setting setting in this.CounterSettings)
                {
                    if (setting.Paid)
                    {
                        bool isChecked;
                        if (!Boolean.TryParse(setting.Value, out isChecked))
                        {
                            if (!Boolean.TryParse(setting.DefaultValue, out isChecked)) isChecked = false;
                        }
                        if (!isChecked) continue;
                    }
                    if (setting.Paid)
                    {
                        m_TotalSum += setting.Price;
                        DataRow row = dtPaid.NewRow();
                        row["SettingName"] = setting.CustomName;
                        row["UsageCount"] = -1;
                        row["UsageCountLimit"] = -1;
                        row["Price"] = setting.Price;
                        dtPaid.Rows.Add(row);
                    }
                    else
                    {
                        int usageCount;
                        int.TryParse(setting.Value, out usageCount);
                        int _paidQty = usageCount - setting.UsageCountLimit;
                        decimal _priceMonth = _paidQty > 0 ? _paidQty * setting.Price : 0;
                        if (_priceMonth > 0)
                        {
                            m_TotalSum += _priceMonth;
                            DataRow row = dtPaid.NewRow();
                            row["SettingName"] = setting.CustomName;
                            row["UsageCount"] = usageCount;
                            row["UsageCountLimit"] = setting.UsageCountLimit;
                            row["Price"] = _priceMonth;
                            row["SettingDescription"] = setting.CustomDescription;
                            dtPaid.Rows.Add(row);
                        }
                        else
                        {
                            DataRow row = dtFree.NewRow();
                            row["SettingName"] = setting.CustomName;
                            row["UsageCount"] = usageCount;
                            row["UsageCountLimit"] = setting.UsageCountLimit;
                            if (setting.UsageCountLimit > 0) row["UsagePersent"] = (int)Math.Round(((100 / (decimal)setting.UsageCountLimit)) * usageCount);
                            else if (usageCount > 0) row["UsagePersent"] = 100;
                            else row["UsagePersent"] = 0;
                            dtFree.Rows.Add(row);
                        }
                    }
                }
                if (dtPaid.Rows.Count > 0)
                {
                    lAccountUsage.Text = "Paid Usage";
                    Repeater3.DataSource = dtPaid;
                    Repeater3.DataBind();
                }
                else
                {
                    divPaidUsageHeader.Visible = false;
                    Repeater3.Visible = false;
                }
                if (dtFree.Rows.Count > 0)
                {
                    divFreeUsageHeader.Visible = true;
                    Repeater4.Visible = true;
                    Repeater4.DataSource = dtFree;
                    Repeater4.DataBind();
                }
                else
                {
                    divFreeUsageHeader.Visible = false;
                    Repeater4.Visible = false;
                }
                TotalAmount = m_TotalSum;
                if (TotalAmount > 0)
                {
                    lBillingPlanName.Text = "$" + TotalAmount.ToString("0.00");
                    if (CurrentBillingPlan == BillingPlan.Free) InstanceProvider.UpdateInstance(UserContext.Current.Instance, BillingPlan.Paid);
                }
                else
                {
                    lBillingPlanName.Text = "FREE";
                    if (CurrentBillingPlan == BillingPlan.Paid) InstanceProvider.UpdateInstance(UserContext.Current.Instance, BillingPlan.Free);
                }
            }
        }

        private void InitBillingControls()
        {
            DateTime? _expDate = UserContext.Current.Organization.ExpirationTime;

            ISubscription _custSubscr = ChargifyProvider.GetCustomerSubscription(Chargify, OrganizationId, InstanceId);
            if (_custSubscr != null && _custSubscr.CreditCard != null)
            {
                _expDate = _custSubscr.CurrentPeriodEndsAt;
                if (!IsPostBack)
                {
                    SettingCollection paidSettings=SettingProvider.GetAllPricedSettings(OrganizationId, InstanceId);
                    ChargifyProvider.UpdateSubscriptionAllocations(Chargify, _custSubscr.SubscriptionID, UserContext.Current.Instance, paidSettings, paidSettings);
                }
                TotalAmount = m_TotalSum;
                SubscriptionId = _custSubscr.SubscriptionID;
                lCCStatus.Text = "Credit Card Registered and " + _custSubscr.State.ToString() + ".";
                TimeSpan _dateDiff = (TimeSpan)(_expDate - DateTime.UtcNow);
                if (_expDate.HasValue)
                {
                    smallNextBillDate.Visible = true;
                    smallNextBillDate.InnerText = "Next billed on " + _expDate.Value.ToString("dd-MMM-yyyy");
                }
            }
            else
            {
                lCCStatus.Text = "No Credit Card on File.";
                if (!IsPostBack) DisablePurchaseButtons();
            }
            if (_custSubscr != null && _custSubscr.CreditCard != null)
            {
                System.Collections.Generic.List<TransactionType> transTypes = new List<TransactionType>();
                transTypes.Add(TransactionType.Payment);
                System.Collections.Generic.IDictionary<int, ITransaction> trans = Chargify.GetTransactionsForSubscription(_custSubscr.SubscriptionID, 1, 25, transTypes);
                if (trans != null && trans.Count > 0)
                {
                    divPaymentHistoryHeader.Visible = true;
                    cgvTransactList.Visible = true;
                    cgvTransactList.DataSource = trans.Values;
                    cgvTransactList.DataBind();
                }
            }
        }

        private void PurchaseTrainingHours(string buttonID)
        {
            this.MasterPage.MessageType = NoticeMessageType.Error;

            Setting setting = null;
            string trainingName = string.Empty;

            switch (buttonID)
            {
                case "btnPurchase1Hour":
                    setting = PaidSettings["Training1Hour"];
                    trainingName = "Purchase Training 1 Hour";
                    break;
                case "btnPurchase3Hours":
                    setting = PaidSettings["Training3Hours"];
                    trainingName = "Purchase Training 3 Hours";
                    break;
                case "btnPurchase8Hours":
                    setting = PaidSettings["Training8Hours"];
                    trainingName = "Purchase Training 8 Hours";
                    break;
            }

            if (setting == null)
            {
                this.MasterPage.Message = trainingName + ": Component definition is not found.";
                this.List_DataBind();
                return;
            }

            if (string.IsNullOrEmpty(setting.ExternalId))
            {
                this.MasterPage.Message = trainingName + ": Component External Id is not defined.";
                this.List_DataBind();
                return;
            }

            int _cid = 0;
            int _count = 1;

            if (!int.TryParse(setting.ExternalId, out _cid))
            {
                this.MasterPage.Message = trainingName + ": Component External Id is invalid.";
                this.List_DataBind();
                return;
            }

            try
            {
                Chargify.AddUsage(SubscriptionId, _cid, _count, "Purchase Training");
            }
            catch (ChargifyException cex)
            {
                if ((int)cex.StatusCode == 422)
                {
                    this.MasterPage.Message = trainingName + ": Credit Card Transaction Failed!";
                }
                else this.MasterPage.Message = cex.Message;
                this.List_DataBind();
                return;
            }
            catch (Exception ex)
            {
                this.MasterPage.Message = ex.Message;
                this.List_DataBind();
                return;
            }
            this.MasterPage.MessageType = NoticeMessageType.Success;
            this.MasterPage.Message = "Your " + trainingName +
                                 " proccessed successfully! You will receive confirmation email.";
            UserContext usr = UserContext.Current;
            string _usrFullName = usr.FirstName + " " + usr.LastName;
            string _subject = (!string.IsNullOrEmpty(usr.Title) ? usr.Title + " " : string.Empty) + _usrFullName + " from " + usr.Organization.Name + " " + usr.Instance.Name + " purchased " + trainingName + ".";
            string _body1 = "Hi, " + _usrFullName + "\r\n\r\nYou purchased " +
                            FrameworkConfiguration.Current.WebApplication.Name + " " + trainingName +
                            ".\r\nOur support team will contact with you ASAP to schedule a time when we can do a training for you.\r\n\r\nThanks.";
            string _body2 = "Please, contact me via Email " + usr.Email +
                           (!string.IsNullOrEmpty(usr.Phone) ? " or by phone " + usr.Phone : string.Empty) +
                           (!string.IsNullOrEmpty(usr.MobilePhone) ? " or by mobile " + usr.MobilePhone : string.Empty) +
                           " to schedule a time when we can do a training.";
            try
            {

                Support.SendEmail(FrameworkConfiguration.Current.WebApplication.Email.SalesTeam, FrameworkConfiguration.Current.WebApplication.Email.SalesTeam, usr.Email,
                                  string.Empty, "Thank You for purchase " + FrameworkConfiguration.Current.WebApplication.Name + " " + trainingName, _body1, false, false, EmailSendingReason.Undefined);
                Support.SendEmail(FrameworkConfiguration.Current.WebApplication.Support.Email, FrameworkConfiguration.Current.WebApplication.Support.Email, FrameworkConfiguration.Current.WebApplication.Email.SalesTeam,
                                  string.Empty, _subject, _body2, false, false, EmailSendingReason.Undefined);
            }
            catch (Exception ex)
            {
                this.MasterPage.MessageType = NoticeMessageType.Warning;
                this.MasterPage.Message = "Your " + trainingName +
                                     " proccessed successfully! But Confirmation emails was not sent.";
                this.MasterPage.MessageDescription = ex.ToString();
            }
            this.List_DataBind();
        }

        #endregion

        #region Protected Methods

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e == null) return;
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            Setting setting = e.Item.DataItem as Setting;
            if (setting == null) return;
            if (setting.ShortName == "PhoneSupport") return;
            if (setting.ShortName == "Training1Hour") return;
            if (setting.ShortName == "Training3Hours") return;
            if (setting.ShortName == "Training8Hours") return;


            bool isChecked = false;
            if (!Boolean.TryParse(setting.Value, out isChecked))
            {
                if (!Boolean.TryParse(setting.DefaultValue, out isChecked)) isChecked = false;
            }

            HtmlGenericControl div0 = new HtmlGenericControl("div");
            div0.Attributes["class"] = "account-option";
            HtmlGenericControl h4 = new HtmlGenericControl("h4");
            if (!string.IsNullOrEmpty(setting.IconUrl)) h4.Attributes["style"] = "background: url('" + setting.IconUrl + "') top left no-repeat;";
            h4.InnerText = setting.CustomName;
            div0.Controls.Add(h4);

            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes["class"] = "feature-toggle";
            div.Attributes["id"] = "div" + setting.ShortName + "OnOff";
            Micajah.Common.WebControls.CheckBox checkBox = new Micajah.Common.WebControls.CheckBox();
            checkBox.ID = "chkOptionOnOff_" + setting.SettingId.ToString("N");
            checkBox.RenderingMode = CheckBoxRenderingMode.OnOffSwitch;
            checkBox.Checked = isChecked;
            checkBox.AutoPostBack = true;
            checkBox.CheckedChanged += new EventHandler(checkBox_CheckedChanged);
            div.Controls.Add(checkBox);
            //            if (setting.Paid && setting.Price > 0) div.Controls.Add(CreateToolTip(setting));
            div0.Controls.Add(div);
            e.Item.Controls.Add(div0);
        }

        protected void btnPurchaseHours_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            PurchaseTrainingHours(btn.ID);
        }

        protected void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            SettingCollection settings = this.PaidSettings;
            if (chk.ID == "chkPhoneSupport")
            {
                settings["PhoneSupport"].Value = chk.Checked.ToString();
            }
            else
            {
                Guid settingId = Guid.Parse(chk.ID.Replace("chkOptionOnOff_", string.Empty));
                settings[settingId].Value = chk.Checked.ToString();
            }
            settings.UpdateValues(OrganizationId, InstanceId);

            this.List_DataBind();
        }

        protected void btnCancelMyAccount_Click(object sender, EventArgs e)
        {
            if (ChargifyProvider.DeleteCustomerSubscription(Chargify, OrganizationId, InstanceId))
            {
                InstanceProvider.UpdateInstance(UserContext.Current.Instance, CreditCardStatus.NotRegistered);
                Response.Redirect(ResourceProvider.AccountSettingsVirtualPath + "?st=cancel");
            }
            else
            {
                this.MasterPage.MessageType = NoticeMessageType.Error;
                this.MasterPage.Message = "Your Credit Card Registration was not deleted!";
            }
        }

        void ccrControl_UpdateClick(object sender, EventArgs e)
        {
            if (ccrControl.Status == CreditCardRegistrationControl.CreditCardRegistrationStatus.Error) return;
            if (ccrControl.Status == CreditCardRegistrationControl.CreditCardRegistrationStatus.Reactivated) Response.Redirect(ResourceProvider.AccountSettingsVirtualPath + "?st=reactivated");
            if (!string.IsNullOrEmpty(hfPurchaseTrainingHours.Value) && hfPurchaseTrainingHours.Value != "0")
            {
                switch (hfPurchaseTrainingHours.Value)
                {
                    case "1":
                        PurchaseTrainingHours("btnPurchase1Hour");
                        break;
                    case "3":
                        PurchaseTrainingHours("btnPurchase3Hours");
                        break;
                    case "8":
                        PurchaseTrainingHours("btnPurchase8Hours");
                        break;
                }
            }
            Response.Redirect(ResourceProvider.AccountSettingsVirtualPath + "?st=ok" + (!string.IsNullOrEmpty(this.MasterPage.Message) ? "&msg=" + HttpUtility.UrlEncode(this.MasterPage.Message) : string.Empty));
        }


        #endregion

        #region Overriden Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ccrControl.UpdateClick += ccrControl_UpdateClick;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            EnsureActiveInstance();
            Repeater1.DataSource = this.PaidSettings;
            Repeater1.DataBind();

            this.MasterPage.VisibleBreadcrumbs = false;

            if (IsPostBack) return;

            if (!ChargifyEnabled || CurrentBillingPlan == BillingPlan.Custom) ccrControl.Visible = false;

            this.List_DataBind();

            if (CurrentBillingPlan == BillingPlan.Custom || !ChargifyEnabled) return;

            lblTraining1HourPrice.Style.Add(HtmlTextWriterStyle.TextAlign, "right");
            lblTraining3HoursPrice.Style.Add(HtmlTextWriterStyle.TextAlign, "right");
            lblTraining8HoursPrice.Style.Add(HtmlTextWriterStyle.TextAlign, "right");

            if (Request.QueryString["st"] == "ok")
            {
                this.MasterPage.MessageType = NoticeMessageType.Success;
                this.MasterPage.Message = "Thank You. Your Credit Card Registered Successfully!";
                if (!string.IsNullOrEmpty(Request.QueryString["msg"])) this.MasterPage.Message += Request.QueryString["msg"];
            }
            else if (Request.QueryString["st"] == "cancel")
            {
                this.MasterPage.MessageType = NoticeMessageType.Success;
                this.MasterPage.Message = "Your Credit Card registration was Removed Successfully!";
            }
            else if (Request.QueryString["st"] == "reactivated")
            {
                this.MasterPage.MessageType = NoticeMessageType.Success;
                this.MasterPage.Message = "Your Credit Card registration was Reactivated Successfully!";
            }

            SettingCollection settings = this.PaidSettings;
            if (settings["Training1Hour"] != null)
                lblTraining1HourPrice.Text = settings["Training1Hour"].Price.ToString("$0.00");
            if (settings["Training3Hours"] != null)
                lblTraining3HoursPrice.Text = settings["Training3Hours"].Price.ToString("$0.00");
            if (settings["Training8Hours"] != null)
                lblTraining8HoursPrice.Text = settings["Training8Hours"].Price.ToString("$0.00");

            ISubscription _subscription = ChargifyProvider.GetCustomerSubscription(Chargify, OrganizationId, InstanceId);
            if (_subscription != null)
            {
                ICreditCardView _cc = _subscription.CreditCard;
                if (_cc != null)
                {
                    ccrControl.SetCreditCardInformation(_cc.FullNumber, _cc.ExpirationMonth.ToString(), _cc.ExpirationYear.ToString().Length > 2 ? _cc.ExpirationYear.ToString().Substring(2) : _cc.ExpirationYear.ToString());
                    IsNewSubscription = false;
                }
                divCancelAccountHeader.Visible = _cc != null && _subscription.State != SubscriptionState.Canceled;
                divCancelAccount.Visible = divCancelAccountHeader.Visible;
            }
            else IsNewSubscription = true;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ResourceProvider.RegisterStyleSheetResource(this, ResourceProvider.AccountSettingsStyleSheet, "AccountSettingsStyleSheet", false);

        }

        #endregion
    }
}
