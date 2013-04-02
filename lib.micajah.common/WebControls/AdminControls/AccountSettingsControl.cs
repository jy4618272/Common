using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ChargifyNET;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Security;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls.AdminControls
{
    public class AccountSettingsControl : UserControl
    {
        #region Overriden Methods
        protected const string ControlIdPrefix = "v";
        private Guid m_OrgId = Guid.Empty;
        private Guid m_InstId = Guid.Empty;

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

        private SettingCollection m_PaidSettings;
        private SettingCollection m_CounterSettings;
        private decimal m_TotalSum = 0;

        protected TextBox txtCCNumber;
        protected TextBox txtCCExpMonth;
        protected TextBox txtCCExpYear;
        protected NoticeMessageBox msgStatus;

        protected CommonGridView cgvTransactList;

        protected RadToolTip RadToolTip1;

        protected ChargifyConnect mChargify = null;
        protected bool ChargifyEnabled = FrameworkConfiguration.Current.WebApplication.Integration.Chargify.Enabled;

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

        private Guid OrganizationId
        {
            get
            {
                if (m_OrgId == Guid.Empty) m_OrgId = UserContext.Current.SelectedOrganization.OrganizationId;
                return m_OrgId;
            }
        }

        private Guid InstanceId
        {
            get
            {
                if (m_InstId == Guid.Empty) m_InstId = UserContext.Current.SelectedInstance.InstanceId;
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
                return UserContext.Current.SelectedInstance.BillingPlan;
            }
        }

        protected ChargifyConnect Chargify
        {
            get { if (mChargify == null) mChargify = ChargifyProvider.CreateChargify(); return mChargify; }
        }

        protected bool IsNewSubscription
        {
            get { return ViewState["IsNewSubscription"] == null ? true : (bool)ViewState["IsNewSubscription"]; }
            set { ViewState["IsNewSubscription"] = value; }
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
                foreach(Setting setting in this.CounterSettings)
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
                InitBillingControls(!IsPostBack);

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
                            row["UsagePersent"] = (int)Math.Round(((100 / (decimal)setting.UsageCountLimit)) * usageCount);
                            dtFree.Rows.Add(row);                            
                        }
                    }
                }
                if (dtPaid.Rows.Count>0)
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
                if (dtFree.Rows.Count>0)
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
                    if (CurrentBillingPlan == BillingPlan.Free) InstanceProvider.UpdateInstance(UserContext.Current.SelectedInstance, BillingPlan.Paid);
                }
                else
                {
                    lBillingPlanName.Text = "FREE";
                    if (CurrentBillingPlan == BillingPlan.Paid) InstanceProvider.UpdateInstance(UserContext.Current.SelectedInstance, BillingPlan.Free);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            EnsureActiveInstance();
            Repeater1.DataSource = this.PaidSettings;
            Repeater1.DataBind();

            Micajah.Common.Pages.MasterPage masterPage = (Micajah.Common.Pages.MasterPage)Page.Master;

            masterPage.VisibleBreadcrumbs = false;
            
            if (ChargifyEnabled && CurrentBillingPlan != BillingPlan.Custom)
            {
                masterPage.EnableFancyBox = true;
                this.RegisterFancyBoxInitScript();
            }

            if (IsPostBack) return;

            this.List_DataBind();

            if (CurrentBillingPlan == BillingPlan.Custom || !ChargifyEnabled) return;

            lblTraining1HourPrice.Style.Add(HtmlTextWriterStyle.TextAlign, "right");
            lblTraining3HoursPrice.Style.Add(HtmlTextWriterStyle.TextAlign, "right");
            lblTraining8HoursPrice.Style.Add(HtmlTextWriterStyle.TextAlign, "right");

            if (Request.QueryString["st"] == "ok")
            {
                masterPage.MessageType = NoticeMessageType.Success;
                masterPage.Message = "Thank You. Your Credit Card Registered Successfully!";
                if (!string.IsNullOrEmpty(Request.QueryString["msg"])) masterPage.Message += Request.QueryString["msg"];
            }
            else if (Request.QueryString["st"] == "cancel")
            {
                masterPage.MessageType = NoticeMessageType.Success;
                masterPage.Message = "Your Credit Card registration was Removed Successfully!";
            }
            else if (Request.QueryString["st"] == "reactivated")
            {
                masterPage.MessageType = NoticeMessageType.Success;
                masterPage.Message = "Your Credit Card registration was Reactivated Successfully!";
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
                    txtCCNumber.Text = _cc.FullNumber;
                    txtCCExpMonth.Text = _cc.ExpirationMonth.ToString();
                    txtCCExpYear.Text = _cc.ExpirationYear.ToString().Length > 2
                                            ? _cc.ExpirationYear.ToString().Substring(2)
                                            : _cc.ExpirationYear.ToString();
                    IsNewSubscription = false;
                }
                divCancelAccountHeader.Visible = _cc != null && _subscription.State != SubscriptionState.Canceled;
                divCancelAccount.Visible = divCancelAccountHeader.Visible;
            }
            else IsNewSubscription = true;
        }

        private void RegisterFancyBoxInitScript()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "FancyBoxInitScript", @"$(""a[rel=facebox]"").fancybox({
                    'type': 'inline',
                    'width': '400',
                    'height': 'auto',
                    'showNavArrows': false,
                    'titlePosition': 'inside',
                    'transitionIn': 'none',
                    'transitionOut': 'none'
                });
                "
                , true);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ResourceProvider.RegisterStyleSheetResource(this, ResourceProvider.AccountSettingsStyleSheet, "AccountSettingsStyleSheet", false);

        }

        protected void EnsureActiveInstance()
        {
            if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
            {
                UserContext user = UserContext.Current;
                if (user != null)
                {
                    if (user.SelectedInstance == null)
                        Response.Redirect(ResourceProvider.GetActiveInstanceUrl(Request.Url.PathAndQuery, false));
                }
            }
        }

        private void InitBillingControls(bool updateUsage)
        {
            DateTime? _expDate = UserContext.Current.SelectedOrganization.ExpirationTime;
            
            ISubscription _custSubscr = ChargifyProvider.GetCustomerSubscription(Chargify, OrganizationId, InstanceId);
            if (_custSubscr != null && _custSubscr.CreditCard != null && _custSubscr.State!=SubscriptionState.Canceled)
            {
                _expDate = _custSubscr.CurrentPeriodEndsAt;
                if (updateUsage) ChargifyProvider.UpdateSubscriptionAllocations(Chargify, _custSubscr.SubscriptionID, UserContext.Current.SelectedInstance);
                TotalAmount = m_TotalSum;
                SubscriptionId = _custSubscr.SubscriptionID;
                lCCStatus.Text = "Credit Card Registered.";
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
            if (_custSubscr!=null && _custSubscr.CreditCard!=null)
            {
                System.Collections.Generic.List<TransactionType> transTypes=new List<TransactionType>();
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

        private void DisablePurchaseButtons()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "FancyBoxPurchaseInitScript", @"$(""input[value=Purchase]"").fancybox({
                    'type': 'inline',
                    'href': '#credit_card_form',
                    'width': '400',
                    'height': 'auto',
                    'showNavArrows': false,
                    'titlePosition': 'inside',
                    'transitionIn': 'none',
                    'transitionOut': 'none'
                });", true);

            btnPurchase1Hour.OnClientClick = "$(\"#" + btnPurchase1Hour.ClientID + "\").value=\"1\"; return false;";
            btnPurchase3Hours.OnClientClick = "$(\"#" + btnPurchase3Hours.ClientID + "\").value=\"3\"; return false;";
            btnPurchase8Hours.OnClientClick = "$(\"#" + btnPurchase8Hours.ClientID + "\").value=\"8\"; return false;";
        }

        protected void btnPurchaseHours_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            PurchaseTrainingHours(btn.ID);
        }

        protected void PurchaseTrainingHours(string buttonID)
        {
            Micajah.Common.Pages.MasterPage masterPage = (Micajah.Common.Pages.MasterPage)Page.Master;
            masterPage.MessageType = NoticeMessageType.Error;

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
                masterPage.Message = trainingName + ": Component definition is not found.";
                this.List_DataBind();
                return;
            }

            if (string.IsNullOrEmpty(setting.ExternalId))
            {
                masterPage.Message = trainingName + ": Component External Id is not defined.";
                this.List_DataBind();
                return;
            }

            int _cid = 0;
            int _count = 1;

            if (!int.TryParse(setting.ExternalId, out _cid))
            {
                masterPage.Message = trainingName + ": Component External Id is invalid.";
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
                    masterPage.Message = trainingName + ": Credit Card Transaction Failed!";
                }
                else masterPage.Message = cex.Message;
                this.List_DataBind();
                return;
            }
            catch (Exception ex)
            {
                masterPage.Message = ex.Message;
                this.List_DataBind();
                return;
            }
            masterPage.MessageType = NoticeMessageType.Success;
            masterPage.Message = "Your " + trainingName +
                                 " proccessed successfully! You will receive confirmation email.";
            UserContext usr = UserContext.Current;
            string _usrFullName = usr.FirstName + " " + usr.LastName;
            string _subject = (!string.IsNullOrEmpty(usr.Title) ? usr.Title + " " : string.Empty) +  _usrFullName + " from " + usr.SelectedOrganization.Name+ " "+usr.SelectedInstance.Name + " purchased " + trainingName+".";
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
                masterPage.MessageType = NoticeMessageType.Warning;
                masterPage.Message = "Your " + trainingName +
                                     " proccessed successfully! But Confirmation emails was not sent.";
                masterPage.MessageDescription = ex.ToString();
            }
            this.List_DataBind();
        }

        protected void InitPhoneSupport()
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
            UserContext.Current.Refresh();
            this.List_DataBind();
        }

        protected void btnCancelMyAccount_Click(object sender, EventArgs e)
        {
            if (ChargifyProvider.DeleteCustomerSubscription(Chargify, OrganizationId, InstanceId))
            {
                InstanceProvider.UpdateInstance(UserContext.Current.SelectedInstance, CreditCardStatus.NotRegistered);
                Response.Redirect(ResourceProvider.AccountSettingsVirtualPath + "?st=cancel");
            }
            else
            {
                Micajah.Common.Pages.MasterPage masterPage = (Micajah.Common.Pages.MasterPage)Page.Master;
                masterPage.MessageType = NoticeMessageType.Error;
                masterPage.Message = "Your Credit Card Registration was not deleted!";
            }
        }

        protected void btnUpdateCC_Click(object sender, EventArgs e)
        {
            string _CustSystemId = OrganizationId.ToString() + "," + InstanceId.ToString();

            ICustomer _cust = Chargify.LoadCustomer(_CustSystemId);
            ISubscription _subscr = null;
            UserContext _uctx = UserContext.Current;

            msgStatus.Visible = true;
            msgStatus.MessageType = NoticeMessageType.Error;

            try
            {
                if (_cust == null)
                {
                    msgStatus.Message = "Can't create Chargify Customer!";
                    _cust = new Customer();
                    _cust.SystemID = _CustSystemId;
                    _cust.Organization = _uctx.SelectedOrganization.Name + " " + _uctx.SelectedInstance.Name;
                    _cust.Email = _uctx.Email;
                    _cust.FirstName = _uctx.FirstName;
                    _cust.LastName = _uctx.LastName;
                    _cust = Chargify.CreateCustomer(_cust);
                }
                else if (_cust.Organization != _uctx.SelectedOrganization.Name + " " + _uctx.SelectedInstance.Name || _cust.Email != _uctx.Email || _cust.FirstName != _uctx.FirstName || _cust.LastName != _uctx.LastName)
                {
                    msgStatus.Message = "Can't update Chargify Customer!";
                    _cust.Organization = _uctx.SelectedOrganization.Name + " " + _uctx.SelectedInstance.Name;
                    _cust.Email = _uctx.Email;
                    _cust.FirstName = _uctx.FirstName;
                    _cust.LastName = _uctx.LastName;
                    _cust = Chargify.UpdateCustomer(_cust);
                    msgStatus.Message = "Can't get Chargify Customer Substriction!";
                    _subscr = ChargifyProvider.GetCustomerSubscription(Chargify, _cust.ChargifyID);
                }
                else
                {
                    msgStatus.Message = "Can't get Chargify Customer Substriction!";
                    _subscr = ChargifyProvider.GetCustomerSubscription(Chargify, _cust.ChargifyID);
                }
            }
            catch (ChargifyException cex)
            {
                if ((int)cex.StatusCode != 422) msgStatus.Message += " " + cex.Message;
                return;
            }
            catch (Exception ex)
            {
                msgStatus.Message += " " + ex.Message;
                return;
            }

            if (txtCCNumber.Text.Contains("XXXX"))
            {
                if (_subscr != null && _subscr.CreditCard!=null && _subscr.State != SubscriptionState.Active)
                {
                    Chargify.ReactivateSubscription(_subscr.SubscriptionID);
                    Response.Redirect(ResourceProvider.AccountSettingsVirtualPath + "?st=reactivated");
                }
                txtCCNumber.Text = string.Empty;
                txtCCExpMonth.Text = string.Empty;
                txtCCExpYear.Text = string.Empty;
                msgStatus.Message = "Invalid Credit Card Information!";
                msgStatus.Description = "Please, input correct data.";
                return;
            }

            CreditCardAttributes _ccattr = new CreditCardAttributes(_cust.FirstName, _cust.LastName, txtCCNumber.Text, 2000 + int.Parse(txtCCExpYear.Text), int.Parse(txtCCExpMonth.Text), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            try
            {
                if (_subscr == null)
                {
                    msgStatus.Message = "Can't create Chargify Subscription!";
                    _subscr = Chargify.CreateSubscription(ChargifyProvider.GetProductHandle(), _cust.ChargifyID, _ccattr);
                }
                else
                {
                    msgStatus.Message = "Can't update Chargify Subscription!";
                    Chargify.UpdateSubscriptionCreditCard(_subscr, _ccattr);
                    if (_subscr.State != SubscriptionState.Active) Chargify.ReactivateSubscription(_subscr.SubscriptionID);
                }
            }
            catch (ChargifyException cex)
            {
                if ((int)cex.StatusCode == 422) msgStatus.Message += " Invalid Credit Card Information!";
                else msgStatus.Message += " " + cex.Message;
                return;
            }
            catch (Exception ex)
            {
                msgStatus.Message += " " + ex.Message;
                return;
            }

            InstanceProvider.UpdateInstance(_uctx.SelectedInstance, CreditCardStatus.Registered);

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
            Micajah.Common.Pages.MasterPage masterPage = (Micajah.Common.Pages.MasterPage)Page.Master;
            Response.Redirect(ResourceProvider.AccountSettingsVirtualPath + "?st=ok" + (!string.IsNullOrEmpty(masterPage.Message) ? "&msg=" + HttpUtility.UrlEncode(masterPage.Message) : string.Empty));
        }
        #endregion
    }
}
