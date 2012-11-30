using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ChargifyNET;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;
using Micajah.Common.Security;

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
        protected Button UpdateButton;
        protected Button ChargifyPayButton;
        protected HyperLink CancelLink;
        protected PlaceHolder ButtonsSeparator;
        protected CheckBox chkPhoneSupport;

        private SettingCollection m_PaidSettings;
        private SettingCollection m_CounterSettings;
        private decimal m_TotalSum = 0;

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
                return BillingPlan.Free;
                return UserContext.Current.SelectedOrganization.BillingPlan;
            }
        }

        private void List_DataBind()
        {
            Repeater1.DataSource = this.PaidSettings;
            Repeater1.DataBind();
            Repeater2.DataSource = this.CounterSettings;
            Repeater2.DataBind();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            EnsureActiveInstance();
            if (IsPostBack) return;

            this.List_DataBind();
            InitBillingControls(true);
            lPhoneSupport.Text = FrameworkConfiguration.Current.WebApplication.Support.Phone;
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
                        Response.Redirect(ResourceProvider.GetActiveInstancePageUrl(Request.Url.PathAndQuery, false));
                }
            }
        }

        private void InitBillingControls(bool updateUsage)
        {
            lBillingPlanName.Text = CurrentBillingPlan.ToString();

            if (CurrentBillingPlan != BillingPlan.Free)
            {
                DateTime? _expDate = UserContext.Current.SelectedOrganization.ExpirationTime;
                ChargifyConnect _chargify = ChargifyProvider.CreateChargify();
                ISubscription _custSubscr = ChargifyProvider.GetCustomerSubscription(_chargify, OrganizationId);
                if (_custSubscr != null)
                {
                    _expDate = _custSubscr.CurrentPeriodEndsAt;
                    if (updateUsage) ChargifyProvider.UpdateSubscriptionAllocations(_chargify, _custSubscr.SubscriptionID, OrganizationId);
                    TotalAmount = m_TotalSum;
                    lCCStatus.Text = "Credit Card Registered.";
                    lSumPerMonth.Text = "$" + TotalAmount.ToString("0.00") + " per Month";
                    TimeSpan _dateDiff = (TimeSpan)(_expDate - DateTime.UtcNow);
                }
                else lCCStatus.Text = "No Credit Card on File.";
                if (_expDate.HasValue)
                {
                    lNextBillDate.Visible = true;
                    lNextBillDate.Text = "Next billed on " + _expDate.Value.ToString("dd-MMM-yyyy");
                }
            }
        }

        private void UpdateSettings()
        {
            NameValueCollection form = Request.Form;
            SettingCollection settings = this.PaidSettings;
            string value = null;

            foreach (Setting setting in settings)
            {
                value = form[string.Concat(ControlIdPrefix, setting.SettingId.ToString("N"))];
                setting.Value = ((value == null) ? "false" : "true");
            }

            settings.UpdateValues(OrganizationId, InstanceId);

            UserContext.Current.Refresh();
        }

        private static Control CreateCheckBox(Setting setting, bool allowDisableState, bool diagnoseConflictingSettings, int visibleChildSettingsCount)
        {
            Control container = null;
            HtmlGenericControl ctl = null;

            try
            {
                string str = setting.SettingId.ToString("N");
                string controlId = string.Concat(ControlIdPrefix, str);

                bool isChecked = false;
                if (!Boolean.TryParse(setting.Value, out isChecked))
                {
                    if (!Boolean.TryParse(setting.DefaultValue, out isChecked)) isChecked = false;
                }

                container = new Control();
                ctl = new HtmlGenericControl("input");
                ctl.Attributes["type"] = "checkbox";
                ctl.Attributes["class"] = "Nm";
                ctl.Attributes["id"] = ctl.Attributes["name"] = controlId;
                if (allowDisableState && (visibleChildSettingsCount > 0))
                    ctl.Attributes["onclick"] = string.Concat("ChangeDisabledState('", str, "', (!this.checked));");
                if (isChecked) ctl.Attributes["checked"] = "checked";
                if (diagnoseConflictingSettings)
                {
                    ctl.Disabled = true;
                    ctl.Style.Add(HtmlTextWriterStyle.Color, "Gray");
                }
                container.Controls.Add(ctl);

                return container;
            }
            finally
            {
                if (container != null) container.Dispose();
                if (ctl != null) ctl.Dispose();
            }
        }

        private void CreateSettingControl(Setting setting, Control container, Control pricecontainer, int visibleChildSettingsCount)
        {
            if (setting == null) return;
            if (setting.SettingId == SettingProvider.MasterPageCustomStyleSheetSettingId) return;

            string _val = setting.Value;

            if (CurrentBillingPlan == BillingPlan.Free)
            {
                if (setting.Paid)
                {
                    Label _lbl = new Label();
                    _lbl.CssClass = "Nm";
                    bool _defValue = false;
                    bool.TryParse(setting.DefaultValue, out _defValue);
                    if (_defValue)
                    {
                        _lbl.Text = "On";
                        _lbl.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        _lbl.Text = "Off";
                        _lbl.ForeColor = System.Drawing.Color.Red;
                    }
                    container.Controls.Add(_lbl);
                }
                else
                {
                    Label _lbl = new Label();
                    _lbl.Font.Bold = true;
                    if (int.Parse(_val) > setting.UsageCountLimit) _lbl.ForeColor = System.Drawing.Color.Red;
                    _lbl.Text = _val + " of " + setting.UsageCountLimit.ToString();
                    container.Controls.Add(_lbl);
                }
            }
            else
            {
                Label _lblPrice = new Label();
                _lblPrice.CssClass = "Nm";
                _lblPrice.Font.Bold = false;

                if (setting.Paid)
                {
                    container.Controls.Add(CreateCheckBox(setting, false, false, visibleChildSettingsCount));
                    if (setting.Price > 0)
                    {
                        _lblPrice.Text = "$" + setting.Price.ToString("0.00") + "/month";
                        bool _isChecked = false;
                        bool.TryParse(setting.Value, out _isChecked);
                        if (_isChecked) m_TotalSum += setting.Price;
                        else _lblPrice.Font.Strikeout = true;
                        if (pricecontainer != null) pricecontainer.Controls.Add(_lblPrice);
                    }
                }
                else
                {
                    int _paidQty = int.Parse(_val) - setting.UsageCountLimit;
                    Label _lbl = new Label();
                    _lbl.Font.Bold = true;
                    _lbl.Text = _val;
                    container.Controls.Add(_lbl);
                    decimal _priceMonth = _paidQty > 0 ? _paidQty * setting.Price : 0;
                    _lblPrice.Text = (setting.UsageCountLimit > 0 ? setting.UsageCountLimit.ToString() + " * $0.0/each" : string.Empty) + (_paidQty > 0 ? (setting.UsageCountLimit > 0 ? " + " : string.Empty) + _paidQty.ToString() + " * $" + setting.Price.ToString("0.00") + "/each" : string.Empty) + " = $" + _priceMonth.ToString("0.00") + "/month";
                    m_TotalSum += _priceMonth;
                    if (pricecontainer != null) pricecontainer.Controls.Add(_lblPrice);
                }
            }
        }

        private void Repeater_ItemDataBound(RepeaterItemEventArgs e, string controlHolderId, string priceHolderId)
        {
            Setting setting = e.Item.DataItem as Setting;
            if (setting == null) return;

            Control controlHoder1 = e.Item.FindControl(controlHolderId);
            if (controlHoder1 == null) return;
            Control priceHolder = e.Item.FindControl(priceHolderId);

            CreateSettingControl(setting, controlHoder1, priceHolder, 0);

        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e == null) return;
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            Setting setting = e.Item.DataItem as Setting;
            if (setting == null) return;
            
            bool isChecked = false;
            if (!Boolean.TryParse(setting.Value, out isChecked))
            {
                if (!Boolean.TryParse(setting.DefaultValue, out isChecked)) isChecked = false;
            }

            if (setting.ShortName=="btPhoneSupport")
            {
                chkPhoneSupport.Checked = isChecked;
                return;
            }

            HtmlGenericControl div0 = new HtmlGenericControl("div");
            div0.Attributes["class"] = "account-option";
            HtmlGenericControl h4= new HtmlGenericControl("h4");
//            h4.Attributes["style"] = "background: url('/images/account.gif') top left no-repeat;";
            h4.InnerText = setting.CustomName;
            div0.Controls.Add(h4);

            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes["class"] = "feature-toggle";
            Micajah.Common.WebControls.CheckBox checkBox = new Micajah.Common.WebControls.CheckBox();
            checkBox.ID = "chkOptionOnOff_" + setting.SettingId.ToString("N");
            checkBox.RenderingMode=CheckBoxRenderingMode.OnOffSwitch;
            checkBox.Checked = isChecked;
            div.Controls.Add(checkBox);
            div0.Controls.Add(div);
            e.Item.Controls.Add(div0);
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

            if (CurrentBillingPlan == BillingPlan.Free)
            {
                if (setting.UsageCountLimit>1)
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
                if (setting.UsageCountLimit > 1)
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
                else if (setting.UsageCountLimit==1)
                {
                    div = new HtmlGenericControl("div");
                    div.Attributes["class"] = "paid-account";
                    HyperLink a=new HyperLink();
                    a.ID = "aTooltip";
                    a.NavigateUrl = "#";
                    a.CssClass = "tooltip_right tooltip";
                    HtmlGenericControl span = new HtmlGenericControl("span");
                    span.InnerHtml = "1st " + setting.CustomName + " is always FREE.<br />$" +
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
            else
            {
                h4.InnerText = usageCount.ToString();
                div.Controls.Add(h4);
                tdCol.Controls.Add(div);
                div = new HtmlGenericControl("div");
                div.Attributes["class"] = "clearfix";
                tdCol.Controls.Add(div);
                div = new HtmlGenericControl("div");
                div.Attributes["class"] = "paid-account";
                HtmlAnchor a = new HtmlAnchor();
                a.HRef = "#";
                a.Attributes["class"] = "tooltip_right tooltip";
                HtmlGenericControl span = new HtmlGenericControl("span");
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
        }

        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            this.UpdateSettings();

            this.List_DataBind();
            InitBillingControls(true);
        }

        protected void UpdateBillingPlanButton_Click(object sender, EventArgs e)
        {
            if (CurrentBillingPlan == BillingPlan.Free)
            {
                Micajah.Common.Bll.Providers.OrganizationProvider.UpdateOrganizationBillingPlan(OrganizationId, BillingPlan.Paid);
                Response.Redirect(Request.Url.ToString());
            }
            else Response.Redirect("ChargifySubscribe.aspx");
        }

        protected void ChargifyPayButton_Click(object sender, EventArgs e)
        {
            try
            {
                ICharge _charge = ChargifyProvider.CreateChargify().CreateCharge(SubscriptionId, TotalAmount, "Manual Payment");

                if (!_charge.Success) ((Micajah.Common.Pages.MasterPage)Page.Master).Message = "Can't charge $" + TotalAmount.ToString("0.00") + " amount!";
            }
            catch (ChargifyException cex)
            {
                if ((int)cex.StatusCode == 422)
                {
                    ((Micajah.Common.Pages.MasterPage)Page.Master).Message = "Credit Card Transaction Failed!";
                }
            }
            this.List_DataBind();
            InitBillingControls(false);
        }

        #endregion
    }
}
