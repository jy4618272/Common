using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;
using Micajah.Common.Configuration;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;
using ChargifyNET;

namespace Micajah.Common.WebControls.AdminControls
{
    public class AccountSettingsControl : UserControl
    {

        #region Overriden Methods
        protected const string ControlIdPrefix = "v";
        private CheckBoxList m_PaidSettingsList;
        private Guid m_OrgId = Guid.Empty;
        private Guid m_InstId = Guid.Empty;

        protected HtmlControl divCCInfo;
        protected Button btnUpdateBillingPlan;
        protected Literal lBillingPlanName;
        protected Literal lSumPerMonth;
        protected Literal lCCStatus;
        protected Literal lNextBillDate;
        protected Label lblPurchase;
        protected Label lblPurchaseSum;
        protected Repeater Repeater1;
        protected Repeater Repeater2;
        protected Button UpdateButton;
        protected Button ChargifyPayButton;
        protected HyperLink CancelLink;
        protected PlaceHolder ButtonsSeparator;

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
            ResourceProvider.RegisterStyleSheetResource(this, "Styles.Settings.css", "SettingsStyleSheet", false);
            if (IsPostBack) return;

            this.LoadResources();
            this.List_DataBind();
            InitBillingControls(true);
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
            lblPurchase.Visible = false;
            lblPurchaseSum.Visible = false;
            ChargifyPayButton.Visible = false;

            if (CurrentBillingPlan == BillingPlan.Free)
            {
                divCCInfo.Visible = false;
                btnUpdateBillingPlan.Text = "Upgrade to Paid";
            }
            else
            {
                divCCInfo.Visible = true;
                btnUpdateBillingPlan.Text = "Update Credit Card";
                DateTime? _expDate = UserContext.Current.SelectedOrganization.ExpirationTime;
                ChargifyConnect _chargify = ChargifyProvider.CreateChargify();
                ISubscription _custSubscr = ChargifyProvider.GetCustomerSubscription(_chargify, OrganizationId);
                if (_custSubscr != null)
                {
                    _expDate = _custSubscr.CurrentPeriodEndsAt;
                    if (updateUsage) ChargifyProvider.UpdateSubscriptionAllocations(_chargify, _custSubscr.SubscriptionID, OrganizationId);
                    TotalAmount=m_TotalSum;
                    lCCStatus.Text = "Credit Card Registered.";
                    lblPurchase.Text = "<br/>Purchase HelpDesk using on another one month to " + _expDate.Value.AddMonths(1).ToString("dd MMMM yyyy") + " for ";
                    lblPurchaseSum.Text = "$" + m_TotalSum.ToString("0.00");
                    lSumPerMonth.Text = "$" + TotalAmount.ToString("0.00") + " per Month";
                    lblPurchase.Visible = true;
                    lblPurchaseSum.Visible = true;
                    TimeSpan _dateDiff = (TimeSpan)(_expDate - DateTime.Now);
                    ChargifyPayButton.Visible = true;
                }
                else lCCStatus.Text = "No Credit Card on File.";
                lNextBillDate.Text = "Next billed on " + _expDate.Value.ToString("dd MMMM yyyy");
            }
        }

        private void LoadResources()
        {
            ChargifyPayButton.Text = "Chargify Pay";
            UpdateButton.Text = "Update Options";
            CancelLink.Text = Resources.AutoGeneratedButtonsField_CancelButton_Text;
            CancelLink.Visible = true;
            ButtonsSeparator.Visible = true;
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
                    decimal _priceMonth = _paidQty>0 ? _paidQty * setting.Price : 0;
                    _lblPrice.Text = (setting.UsageCountLimit>0 ? setting.UsageCountLimit.ToString()+" * $0.0/each" : string.Empty) + (_paidQty>0 ? (setting.UsageCountLimit>0 ? " + " : string.Empty) + _paidQty.ToString()+ " * $" + setting.Price.ToString("0.00") + "/each" : string.Empty) +" = $"+_priceMonth.ToString("0.00")+"/month";
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
            if (!((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))) return;

            Setting setting = e.Item.DataItem as Setting;
            if (setting == null) return;

            Repeater_ItemDataBound(e, "ControlHoder2", "PriceHolder");

            Label label = e.Item.FindControl("NameLabel2") as Label;
            if (label != null)
            {
                label.Text = setting.CustomName;
                label.ToolTip = setting.CustomDescription;
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
