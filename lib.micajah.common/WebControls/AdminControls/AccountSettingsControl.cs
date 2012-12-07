using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ChargifyNET;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using ChargifyNET;
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
        protected Literal lBillingPlanName;
        protected Literal lSumPerMonth;
        protected Literal lCCStatus;
        protected Literal lNextBillDate;
        protected Literal lPhoneSupport;
        protected Label lblPurchase;
        protected Label lblPurchaseSum;
        protected Repeater Repeater1;
        protected Repeater Repeater2;

        protected Label lblTraining1HourPrice;
        protected Label lblTraining3HoursPrice;
        protected Label lblTraining8HoursPrice;

        protected Button btnPurchase1Hour;
        protected Button btnPurchase3Hours;
        protected Button btnPurchase8Hours;

        protected HyperLink CancelLink;
        protected PlaceHolder ButtonsSeparator;
        protected PlaceHolder phPhoneSupportToolTip;
        protected CheckBox chkPhoneSupport;

        private SettingCollection m_PaidSettings;
        private SettingCollection m_CounterSettings;
        private decimal m_TotalSum = 0;

        protected TextBox txtEmail;
        protected TextBox txtFullName;
        protected TextBox txtCCNumber;
        protected TextBox txtCCExpMonth;
        protected TextBox txtCCExpYear;
        protected NoticeMessageBox msgStatus;

        protected RadToolTip RadToolTip1;

        protected ChargifyConnect mChargify = null;

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
                if (m_CounterSettings == null) m_CounterSettings = SettingProvider.GetCounterSettings(OrganizationId);
                return m_CounterSettings;
            }
        }

        private BillingPlan CurrentBillingPlan
        {
            get
            {
                return UserContext.Current.SelectedOrganization.BillingPlan;
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
            Repeater2.DataSource = this.CounterSettings;
            Repeater2.DataBind();
            InitPhoneSupport();
            TotalAmount = m_TotalSum;
            if (TotalAmount > 0)
            {
                lBillingPlanName.Text = "PAID";
                lSumPerMonth.Text = "$" + TotalAmount.ToString("0.00") + " per Month";
                if (CurrentBillingPlan == BillingPlan.Free) Micajah.Common.Bll.Providers.OrganizationProvider.UpdateOrganizationBillingPlan(OrganizationId, BillingPlan.Paid);
            }
            else
            {
                lBillingPlanName.Text = "FREE";
                lSumPerMonth.Text = "per Month";
                if (CurrentBillingPlan == BillingPlan.Paid) Micajah.Common.Bll.Providers.OrganizationProvider.UpdateOrganizationBillingPlan(OrganizationId, BillingPlan.Paid);
            }
            InitBillingControls(!IsPostBack);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            EnsureActiveInstance();
            Repeater1.DataSource = this.PaidSettings;
            Repeater1.DataBind();

            if (IsPostBack) return;

            if (Request.QueryString["st"]=="ok")
            {
                Micajah.Common.Pages.MasterPage masterPage = (Micajah.Common.Pages.MasterPage)Page.Master;
                masterPage.MessageType = NoticeMessageType.Success;
                masterPage.Message = "Thank You. Your Credit Card Registered Successfully!";
            }

            this.List_DataBind();
            lPhoneSupport.Text = FrameworkConfiguration.Current.WebApplication.Support.Phone;

            SettingCollection settings = this.PaidSettings;
            if (settings["Training1Hour"] != null)
                lblTraining1HourPrice.Text = settings["Training1Hour"].Price.ToString("$0.00");
            if (settings["Training3Hours"] != null)
                lblTraining3HoursPrice.Text = settings["Training3Hours"].Price.ToString("$0.00");
            if (settings["Training8Hours"] != null)
                lblTraining8HoursPrice.Text = settings["Training8Hours"].Price.ToString("$0.00");

            ICustomer _cust = Chargify.LoadCustomer(OrganizationId.ToString());

            if (_cust != null)
            {
                txtEmail.Text = _cust.Email;
                txtFullName.Text = _cust.FirstName + " " + _cust.LastName;
                IDictionary<int, ISubscription> _subscrList = Chargify.GetSubscriptionListForCustomer(_cust.ChargifyID);
                if (_subscrList.Count > 0)
                {
                    ICreditCardView _cc = null;
                    foreach (KeyValuePair<int, ISubscription> kvp in _subscrList)
                    {
                        _cc = kvp.Value.CreditCard;
                        break;
                    }
                    txtCCNumber.Text = _cc.FullNumber;
                    txtCCExpMonth.Text = _cc.ExpirationMonth.ToString();
                    txtCCExpYear.Text = _cc.ExpirationYear.ToString().Length > 2 ? _cc.ExpirationYear.ToString().Substring(2) : _cc.ExpirationYear.ToString();
                }
                IsNewSubscription = false;
            }
            else IsNewSubscription = true;
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
            if (CurrentBillingPlan == BillingPlan.Free) return;

            DateTime? _expDate = UserContext.Current.SelectedOrganization.ExpirationTime;
            ChargifyConnect _chargify = ChargifyProvider.CreateChargify();
            ISubscription _custSubscr = ChargifyProvider.GetCustomerSubscription(_chargify, OrganizationId);
            if (_custSubscr != null)
            {
                _expDate = _custSubscr.CurrentPeriodEndsAt;
                if (updateUsage) ChargifyProvider.UpdateSubscriptionAllocations(_chargify, _custSubscr.SubscriptionID, OrganizationId);
                TotalAmount = m_TotalSum;
                SubscriptionId = _custSubscr.SubscriptionID;
                lCCStatus.Text = "Credit Card Registered.";
                TimeSpan _dateDiff = (TimeSpan)(_expDate - DateTime.UtcNow);
                if (_expDate.HasValue)
                {
                    lNextBillDate.Visible = true;
                    lNextBillDate.Text = "Next billed on " + _expDate.Value.ToString("dd-MMM-yyyy");
                }
            }
            else
            {
                lCCStatus.Text = "No Credit Card on File.";
                DisablePurchaseButtons();
            }
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e == null) return;
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            Setting setting = e.Item.DataItem as Setting;
            if (setting == null) return;
            if (setting.ShortName == "btPhoneSupport") return;
            if (setting.ShortName == "Training1Hour") return;
            if (setting.ShortName == "Training3Hours") return;
            if (setting.ShortName == "Training8Hours") return;

            
            bool isChecked = false;
            if (!Boolean.TryParse(setting.Value, out isChecked))
            {
                if (!Boolean.TryParse(setting.DefaultValue, out isChecked)) isChecked = false;
            }

            if (isChecked && setting.Paid && setting.Price > 0) m_TotalSum += setting.Price;

            HtmlGenericControl div0 = new HtmlGenericControl("div");
            div0.Attributes["class"] = "account-option";
            HtmlGenericControl h4= new HtmlGenericControl("h4");
//            h4.Attributes["style"] = "background: url('/images/account.gif') top left no-repeat;";
            h4.InnerText = setting.CustomName;
            div0.Controls.Add(h4);

            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes["class"] = "feature-toggle";
            div.Attributes["id"] = "div" + setting.ShortName+"OnOff";
            Micajah.Common.WebControls.CheckBox checkBox = new Micajah.Common.WebControls.CheckBox();
            checkBox.ID = "chkOptionOnOff_" + setting.SettingId.ToString("N");
            checkBox.RenderingMode=CheckBoxRenderingMode.OnOffSwitch;
            checkBox.Checked = isChecked;
            checkBox.AutoPostBack = true;
            checkBox.CheckedChanged += new EventHandler(checkBox_CheckedChanged);
            div.Controls.Add(checkBox);
            if (setting.Paid && setting.Price > 0) div.Controls.Add(CreateToolTip(setting));
            div0.Controls.Add(div);
            e.Item.Controls.Add(div0);
        }

        private RadToolTip CreateToolTip(Setting setting)
        {
            var radToolTip = new RadToolTip();
            radToolTip.ID = "rtt" + setting.ShortName;
            radToolTip.IsClientID = true;
            radToolTip.TargetControlID = "div" + setting.ShortName + "OnOff";
            radToolTip.RelativeTo = ToolTipRelativeDisplay.Element;
            radToolTip.ShowEvent = ToolTipShowEvent.OnMouseOver;
            radToolTip.Position = ToolTipPosition.MiddleRight;
            radToolTip.HideEvent=ToolTipHideEvent.LeaveTargetAndToolTip;
            radToolTip.BackColor = System.Drawing.ColorTranslator.FromHtml("#404040");
            radToolTip.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            radToolTip.BorderColor = System.Drawing.ColorTranslator.FromHtml("#404040");
            radToolTip.Width = 300;
            radToolTip.Text =
                "By clicking OK,<br /> you will turn on the " + setting.CustomName + "<br />and incure a monthly charge of $" + setting.Price.ToString("0.00") + ".";
            return radToolTip;
        }

        private void DisablePurchaseButtons()
        {

            string _script = "alert(\"Please, Register Your Credit Card before.\"); return false;";
                btnPurchase1Hour.OnClientClick =
                    btnPurchase3Hours.OnClientClick = btnPurchase8Hours.OnClientClick = _script;
        }

        protected void btnPurchaseHours_Click(object sender, EventArgs e)
        {
            Micajah.Common.Pages.MasterPage masterPage = (Micajah.Common.Pages.MasterPage) Page.Master;
            masterPage.MessageType=NoticeMessageType.Error;
            decimal amount = 0;

            Button btn = sender as Button;
            Setting setting = null;
            string trainingName = string.Empty;

            switch (btn.ID)
            {
                case "btnPurchase1Hour":
                    setting = PaidSettings["Training1Hour"];
                    trainingName = "Training 1 Hour";
                    break;
                case "btnPurchase3Hours":
                    setting = PaidSettings["Training3Hours"];
                    trainingName = "Training 3 Hours";
                    break;
                case "btnPurchase8Hours":
                    setting = PaidSettings["Training8Hours"];
                    trainingName = "Training 8 Hours";
                    break;
            }

            if (setting==null)
            {
                masterPage.Message = "Component definition is not found.";
                return;
            }

            if (string.IsNullOrEmpty(setting.ExternalId))
            {
                masterPage.Message = "Component External Id is not defined.";
                return;
            }

            int _cid = 0;
            int _count = 1;

            if (!int.TryParse(setting.ExternalId, out _cid))
            {
                masterPage.Message = "Component External Id is invalid.";
                return;                
            }

            try
            {
                ChargifyConnect _chargify = ChargifyProvider.CreateChargify();
                _chargify.AddUsage(SubscriptionId, _cid, _count, "Purchase Training");
//                _chargify.UpdateComponentAllocationForSubscription(SubscriptionId, _cid, _count);
            }
            catch (ChargifyException cex)
            {
                if ((int)cex.StatusCode == 422)
                {
                    masterPage.Message = "Credit Card Transaction Failed!";
                }
                else masterPage.Message = cex.Message;
                return;
            }
            catch (Exception ex)
            {
                masterPage.Message = ex.Message;
                return;                
            }
            masterPage.MessageType=NoticeMessageType.Success;
            masterPage.Message = "Your " + trainingName +
                                 " proccessed successfully! You will receive confirmation email.";
            UserContext usr = UserContext.Current;
            string _subject = (!string.IsNullOrEmpty(usr.Title) ? usr.Title + " " : string.Empty) + usr.FirstName + " " +
                              usr.LastName + " from " + usr.SelectedOrganization.Name + " purchased " + trainingName;
            string _body1 = "Hi, " + usr.FirstName + " " + usr.LastName + "\r\n\r\nYou purchased " +
                            FrameworkConfiguration.Current.WebApplication.Name + " " + trainingName +
                            ".\r\nOur support team will contact with you ASAP to schedule a time when we can do a training for you.\r\n\r\nThanks.";
            string _body2 = "Please, contact me via Email" +
                           (!string.IsNullOrEmpty(usr.Phone) ? " or by phone " + usr.Phone : string.Empty) +
                           (!string.IsNullOrEmpty(usr.MobilePhone) ? " or by mobile " + usr.MobilePhone : string.Empty) +
                           " to schedule a time when we can do a training.";
            try
            {

                Support.SendEmail(FrameworkConfiguration.Current.WebApplication.Support.Email, UserContext.Current.Email,
                                  string.Empty, "Thank You for purchase " + FrameworkConfiguration.Current.WebApplication.Name + " " + trainingName, _body1, false, false, EmailSendingReason.Undefined);
                Support.SendEmail(UserContext.Current.Email, FrameworkConfiguration.Current.WebApplication.Support.Email,
                                  string.Empty, _subject, _body2, false, false, EmailSendingReason.Undefined);
            }
            catch (Exception ex)
            {
                masterPage.MessageType = NoticeMessageType.Warning;
                masterPage.Message = "Your " + trainingName +
                                     " proccessed successfully! But Confirmation emails was not sent.";
                masterPage.MessageDescription = ex.ToString();
            }
        }

        protected void InitPhoneSupport()
        {
            SettingCollection settings = this.PaidSettings;
            Setting setting = settings["btPhoneSupport"];
            if (setting==null) return;

            bool isChecked = false;
            if (!Boolean.TryParse(setting.Value, out isChecked))
            {
                if (!Boolean.TryParse(setting.DefaultValue, out isChecked)) isChecked = false;
            }
            if (isChecked && setting.Paid && setting.Price > 0) m_TotalSum += setting.Price;
            if (setting.Paid && setting.Price>0) phPhoneSupportToolTip.Controls.Add(CreateToolTip(setting));
            if (!IsPostBack) chkPhoneSupport.Checked = isChecked;
        }

        protected void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            SettingCollection settings = this.PaidSettings;
            if (chk.ID=="chkPhoneSupport")
            {
                settings["btPhoneSupport"].Value = chk.Checked.ToString();
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

        protected void btnUpdateCC_Click(object sender, EventArgs e)
        {
            string _OrgIdStr = OrganizationId.ToString();

            ICustomer _cust = IsNewSubscription ? new Customer() : Chargify.LoadCustomer(_OrgIdStr);

            _cust.SystemID = _OrgIdStr;
            _cust.Organization = UserContext.Current.SelectedOrganization.Name;
            _cust.Email = txtEmail.Text;
            string[] _arrName = txtFullName.Text.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            _cust.FirstName = _arrName[0];
            _cust.LastName = _arrName.Length > 1 ? _arrName[1] : string.Empty;

            CreditCardAttributes _ccattr = new CreditCardAttributes(_cust.FirstName, _cust.LastName, txtCCNumber.Text, 2000+int.Parse(txtCCExpYear.Text), int.Parse(txtCCExpMonth.Text), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            msgStatus.Visible = true;
            msgStatus.MessageType = NoticeMessageType.Error;
            if (IsNewSubscription)
            {
                _cust = Chargify.CreateCustomer(_cust);
                try
                {
                    Chargify.CreateSubscription(ChargifyProvider.GetProductHandle(), _cust.ChargifyID, _ccattr);
                }
                catch (ChargifyException cex)
                {
                    if ((int)cex.StatusCode == 422) msgStatus.Message = "Invalid Credit Card Information!";
                    else msgStatus.Message = cex.Message;
                    return;
                }
                catch (Exception ex)
                {
                    msgStatus.Message = ex.Message;
                    return;
                }

            }
            else
            {
                Chargify.UpdateCustomer(_cust);
                ISubscription _custSubscr = ChargifyProvider.GetCustomerSubscription(Chargify, UserContext.Current.SelectedOrganization.OrganizationId);
                try
                {
                    if (_custSubscr != null) Chargify.UpdateSubscriptionCreditCard(_custSubscr, _ccattr);
                    else Chargify.CreateSubscription(ChargifyProvider.GetProductHandle(), _cust.ChargifyID, _ccattr);
                }
                catch (ChargifyException cex)
                {
                    if ((int)cex.StatusCode == 422) msgStatus.Message = "Invalid Credit Card Information!";
                    else msgStatus.Message = cex.Message;
                    return;
                }
                catch (Exception ex)
                {
                    msgStatus.Message = ex.Message;
                    return;
                }
            }
            Response.Redirect("accountsettings.aspx?st=ok");
        }

        protected void Repeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e == null) return;
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            
            Setting setting = e.Item.DataItem as Setting;
            if (setting == null) return;

            Control tdCol = e.Item.FindControl("AccUsageCol");

            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes["class"] = "featurelabel";
            HtmlGenericControl h5 = new HtmlGenericControl("h5");
            h5.InnerText = setting.CustomName;
            div.Controls.Add(h5);
            tdCol.Controls.Add(div);
            div = new HtmlGenericControl("div");
            div.Attributes["class"] = "account-usage-amount";
            HtmlGenericControl h4 = new HtmlGenericControl("h4");

            int usageCount = 0;
            int.TryParse(setting.Value, out usageCount);

            int _paidQty = usageCount - setting.UsageCountLimit;
            decimal _priceMonth = _paidQty > 0 ? _paidQty * setting.Price : 0;
            m_TotalSum += _priceMonth;


            if (setting.UsageCountLimit>1 && usageCount<=setting.UsageCountLimit)
            {
                HtmlGenericControl span = new HtmlGenericControl("span");
                if (usageCount < setting.UsageCountLimit) span.Attributes["class"] = "under";
                else if (usageCount > setting.UsageCountLimit) span.Attributes["class"] = "over";
                else span.Attributes["class"] = "even";
                span.InnerText = usageCount.ToString();
                h4.Controls.Add(span);
                h4.Controls.Add(new LiteralControl(" of "+setting.UsageCountLimit.ToString()));
            }
            else h4.InnerText = usageCount.ToString();
            div.Controls.Add(h4);
            tdCol.Controls.Add(div);
            div = new HtmlGenericControl("div");
            div.Attributes["class"] = "clearfix";
            tdCol.Controls.Add(div);
            if (setting.UsageCountLimit > 1 && usageCount<=setting.UsageCountLimit)
            {
                div = new HtmlGenericControl("div");
                div.Attributes["class"] = "progress";
                HtmlGenericControl divBar = new HtmlGenericControl("div");
                divBar.Attributes["class"] = "bar";
                if (usageCount<setting.UsageCountLimit)
                {
                    int width = (int)Math.Round(((100 / (decimal)setting.UsageCountLimit)) * (decimal)usageCount);
                    divBar.Style.Add(HtmlTextWriterStyle.Width, width.ToString() + "%");                        
                }
                else if (usageCount>setting.UsageCountLimit)
                {
//                        div.Attributes["class"] = "progress-red";
                    divBar.Style.Add(HtmlTextWriterStyle.Width, "100%");
                }
                else
                {
//                        div.Attributes["class"] = "progress-green";
                    divBar.Style.Add(HtmlTextWriterStyle.Width, "100%");
                }
                div.Controls.Add(divBar);
                tdCol.Controls.Add(div);
            }
            else if (setting.UsageCountLimit==0 || setting.UsageCountLimit==1 || usageCount>setting.UsageCountLimit)
            {
                div = new HtmlGenericControl("div");
                div.Attributes["class"] = "paid-account";
                HyperLink a=new HyperLink();
                a.ID = "aTooltip";
                a.NavigateUrl = "#"+setting.ShortName;
                a.CssClass = "tooltip_right tooltip";
                HtmlGenericControl span = new HtmlGenericControl("span");
                if (setting.UsageCountLimit==0)
                    span.InnerHtml = "$" + setting.Price.ToString("0.00") + " for each " + setting.CustomName;
                else if (setting.UsageCountLimit==1)
                    span.InnerHtml = "1st " + setting.CustomName + " is always FREE.<br />$" +
                                setting.Price.ToString("0.00") + " / month for additional " + setting.CustomName;
                else if (setting.UsageCountLimit>1)
                    span.InnerHtml = setting.UsageCountLimit.ToString()+" " + setting.CustomName + " is always FREE.<br />$" +
                                setting.Price.ToString("0.00") + " / month for additional " + setting.CustomName;
                HtmlGenericControl h3 = new HtmlGenericControl("h3");
                if (usageCount > setting.UsageCountLimit) h3.InnerText = "$" + Convert.ToString((usageCount - setting.UsageCountLimit) * setting.Price);
                else h3.InnerText = "$0.00";
                a.Controls.Add(span);
                a.Controls.Add(h3);
                div.Controls.Add(a);
                tdCol.Controls.Add(div);
            }
            else
            {
                div = new HtmlGenericControl("div");
                div.Attributes["class"] = "paid-account";
                tdCol.Controls.Add(div);
            }
        }
        #endregion
    }
}
