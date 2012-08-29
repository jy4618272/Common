using System;
using System.Data;
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
using ChargifyNET.Configuration;
using System.Configuration;

namespace Micajah.Common.WebControls.AdminControls
{
    public class ChargifySubscribeControl : UserControl
    {

        #region Overriden Methods
        protected const string ControlIdPrefix = "v";
        protected MagicForm mfChargifyCustomer;
        protected ChargifyConnect mChargify=null;

        protected ChargifyConnect Chargify
        {
            get { if (mChargify == null) mChargify = ChargifyProvider.CreateChargify(); return mChargify; }
        }

        protected bool IsNewSubscription
        {
            get {return ViewState["IsNewSubscription"]==null ? true : (bool)ViewState["IsNewSubscription"]; }
            set { ViewState["IsNewSubscription"] = value; }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsPostBack) return;

            DataTable _dt = new DataTable("Years");
            _dt.Columns.Add(new DataColumn("Year", typeof(string)));
            for (int _year = DateTime.Now.Year; _year < DateTime.Now.Year + 5; _year++)
            {
                DataRow _r=_dt.NewRow();
                _r["Year"]=_year.ToString();
                _dt.Rows.Add(_r);
            }
            ComboBoxField _cmbf = (ComboBoxField)mfChargifyCustomer.Fields[7];
            _cmbf.DataSource = _dt;

            _dt = new DataTable("ChargifyCustomer");
            _dt.Columns.Add(new DataColumn("Email", typeof(string)));
            _dt.Columns.Add(new DataColumn("FirstName", typeof(string)));
            _dt.Columns.Add(new DataColumn("LastName", typeof(string)));
            _dt.Columns.Add(new DataColumn("CCNumber", typeof(string)));
            _dt.Columns.Add(new DataColumn("CCExpirationMonth", typeof(string)));
            _dt.Columns.Add(new DataColumn("CCExpirationYear", typeof(string)));
            DataRow _row = _dt.NewRow();
            ICustomer _cust = Chargify.LoadCustomer(UserContext.Current.SelectedOrganization.OrganizationId.ToString());
            if (_cust == null)
            {
                mfChargifyCustomer.Caption = "New Chargify Subscription";
                IsNewSubscription = true;
            }
            else
            {
                mfChargifyCustomer.Caption = "Edit Chargify Subscription";
                IsNewSubscription = false;
                _row["Email"] = _cust.Email;
                _row["FirstName"] = _cust.FirstName;
                _row["LastName"] = _cust.LastName;
                IDictionary<int,ISubscription> _subscrList=Chargify.GetSubscriptionListForCustomer(_cust.ChargifyID);
                if (_subscrList.Count > 0)
                {
                    ICreditCardView _cc = null;
                    foreach (KeyValuePair<int, ISubscription> kvp in _subscrList)
                    {
                        _cc = kvp.Value.CreditCard;
                        break;
                    }
                    _row["CCNumber"] = _cc.FullNumber;
                    _row["CCExpirationMonth"] = _cc.ExpirationMonth.ToString();
                    _row["CCExpirationYear"] = _cc.ExpirationYear.ToString();
                }
            }
            _dt.Rows.Add(_row);
            mfChargifyCustomer.DataSource = _dt;
            mfChargifyCustomer.DataBind();
        }

        protected void mfChargifyCustomer_Action(object sender, Micajah.Common.WebControls.MagicFormActionEventArgs e)
        {
            if (e.Action == CommandActions.Cancel) Response.Redirect("AccountSettings.aspx");
        }

        protected void mfChargifyCustomer_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            string _OrgIdStr = UserContext.Current.SelectedOrganization.OrganizationId.ToString();

            ICustomer _cust=IsNewSubscription ? new Customer() : Chargify.LoadCustomer(_OrgIdStr);

            _cust.SystemID=_OrgIdStr;
            _cust.Organization = UserContext.Current.SelectedOrganization.Name;
            _cust.Email=e.NewValues["Email"].ToString();
            _cust.FirstName=e.NewValues["FirstName"].ToString();
            _cust.LastName=e.NewValues["LastName"].ToString();

            CreditCardAttributes _ccattr = new CreditCardAttributes(_cust.FirstName, _cust.LastName, e.NewValues["CCNumber"].ToString(), int.Parse(e.NewValues["CCExpirationYear"].ToString()), int.Parse(e.NewValues["CCExpirationMonth"].ToString()), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            if (IsNewSubscription)
            {
                _cust=Chargify.CreateCustomer(_cust);
                try
                {
                    Chargify.CreateSubscription(ChargifyProvider.GetProductHandle(), _cust.ChargifyID, _ccattr);
                }
                catch (ChargifyException cex)
                {
                    if ((int)cex.StatusCode == 422)
                    {
                        ((Micajah.Common.Pages.MasterPage)Page.Master).Message = "Invalid Credit Card Information!";
                        return;
                    }
                    throw;
                }

            }
            else
            {
                Chargify.UpdateCustomer(_cust);
                ISubscription _custSubscr = ChargifyProvider.GetCustomerSubscription(Chargify, UserContext.Current.SelectedOrganization.OrganizationId);
                try
                {
                    if (_custSubscr!=null) Chargify.UpdateSubscriptionCreditCard(_custSubscr, _ccattr);
                    else Chargify.CreateSubscription(ChargifyProvider.GetProductHandle(), _cust.ChargifyID, _ccattr);
                }
                catch (ChargifyException cex)
                {
                    if ((int)cex.StatusCode == 422)
                    {
                        ((Micajah.Common.Pages.MasterPage)Page.Master).Message = "Invalid Credit Card Information!";
                        return;
                    }
                    throw;
                }
            }
            Response.Redirect("AccountSettings.aspx");
        }

        #endregion
    }
}
