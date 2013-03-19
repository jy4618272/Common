using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Dal;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls.Reports
{
    /// <summary>
    /// The control to manage organizations.
    /// </summary>
    public class ActivityReportControl : UserControl
    {
        #region Members

        private UserContext m_UserContext;
        protected CommonGridView cgvList;

        #endregion

        #region Overriden Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            m_UserContext = UserContext.Current;

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("OrganizationId", Type.GetType("System.Guid")));
            dt.Columns.Add(new DataColumn("InstanceId", Type.GetType("System.Guid")));
            dt.Columns.Add(new DataColumn("InstanceName", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CreationDate", Type.GetType("System.DateTime")));
            dt.Columns.Add(new DataColumn("AdminFullName", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("AdminEmail", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("AdminPhone", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("LastActivityDate", Type.GetType("System.DateTime")));
            dt.Columns.Add(new DataColumn("MonthlyBillable", Type.GetType("System.Decimal")));
            dt.Columns.Add(new DataColumn("CreditCardStatus", Type.GetType("System.String")));

            //Init datatable columns
            CommonDataSet.SettingDataTable table = Micajah.Common.Application.WebApplication.CommonDataSet.Setting;
            string filter = string.Format(CultureInfo.InvariantCulture, "{0} = "+ ((int)SettingType.Counter).ToString(), table.SettingTypeIdColumn.ColumnName);
            SettingCollection counterSettings = new SettingCollection();
            foreach (CommonDataSet.SettingRow _srow in table.Select(filter)) counterSettings.Add(SettingProvider.CreateSetting(_srow));
            foreach (Setting setting in counterSettings)
            {
                DataColumn dtCol = new DataColumn(setting.ShortName, Type.GetType("System.Int32"));
                dtCol.DefaultValue = -1;
                dt.Columns.Add(dtCol);
                BoundField col = new BoundField();
                col.HeaderText = setting.CustomName;
                col.DataField = setting.ShortName;
                cgvList.Columns.Add(col);
            }

            filter = string.Format(CultureInfo.InvariantCulture, "{0} > 0", table.PriceColumn.ColumnName);
            SettingCollection ss = new SettingCollection();
            foreach (CommonDataSet.SettingRow _srow in table.Select(filter)) ss.Add(SettingProvider.CreateSetting(_srow));

            foreach (Setting setting in ss)
            {
                if (setting.ShortName == "PhoneSupport") continue;
                if (setting.ShortName == "Training1Hour") continue;
                if (setting.ShortName == "Training3Hours") continue;
                if (setting.ShortName == "Training8Hours") continue;

                if (setting.Paid) dt.Columns.Add(new DataColumn(setting.ShortName, Type.GetType("System.Boolean")));
                else
                {
                    DataColumn dtCol = new DataColumn(setting.ShortName, Type.GetType("System.Int32"));
                    dtCol.DefaultValue = -1;
                    dt.Columns.Add(dtCol);
                }
                BoundField col=new BoundField();
                col.HeaderText = setting.CustomName;
                col.DataField = setting.ShortName;
                cgvList.Columns.Add(col);
            }

            BoundField colGrid=new BoundField();
            colGrid.HeaderText = "MonthlyBillable";
            colGrid.DataField = "MonthlyBillable";
            colGrid.DataFormatString = "{0:C}";
            cgvList.Columns.Add(colGrid);
            colGrid = new BoundField();
            colGrid.HeaderText = "Credit Card Status";
            colGrid.DataField = "CreditCardStatus";
            cgvList.Columns.Add(colGrid);

            OrganizationCollection orgs=OrganizationProvider.GetOrganizations(false, false);

            foreach (Organization org in orgs)
            {
                Dal.OrganizationDataSet.UserDataTable orgAdmins = UserProvider.GetUsers(org.OrganizationId, Guid.Empty, RoleProvider.OrganizationAdministratorRoleId);
                string afName = string.Empty;
                string aEmail = string.Empty;
                string aPhone = string.Empty;
                OrganizationDataSet.UserRow orgAdmin = null;
                if (orgAdmins.Count == 1) orgAdmin = orgAdmins[0];
                else
                {
                    if (!string.IsNullOrEmpty(org.EmailSuffixes))
                    {
                        foreach (OrganizationDataSet.UserRow admin in orgAdmins)
                        {
                            if (string.IsNullOrEmpty(admin.Email)) continue;
                            string[] arr = admin.Email.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                            string eSuffix = arr.Length > 1 ? arr[1] : arr[0];
                            if (!string.IsNullOrEmpty(org.EmailSuffixes) && org.EmailSuffixes.Contains(eSuffix))
                            {
                                orgAdmin = admin;
                                break;
                            }
                        }
                        if (orgAdmin == null && orgAdmins.Count > 0) orgAdmin = orgAdmins[0];
                    }
                    else if (orgAdmins.Count > 0) orgAdmin = orgAdmins[0];
                }
                if (orgAdmin!=null)
                {
                    afName = (!string.IsNullOrEmpty(orgAdmin.FirstName) ?  orgAdmin.FirstName : string.Empty) + (!string.IsNullOrEmpty(orgAdmin.LastName) ? " " + orgAdmin.LastName : string.Empty);
                    aEmail = !string.IsNullOrEmpty(orgAdmin.Email) ? orgAdmin.Email : string.Empty;
                    aPhone = !string.IsNullOrEmpty(orgAdmin.Phone) ? orgAdmin.Phone : string.Empty;
                }
                InstanceCollection insts = InstanceProvider.GetInstances(org.OrganizationId, false);
                foreach (Instance inst in insts)
                {
                    decimal monthlySum = 0;
                    DataRow row = dt.NewRow();
                    row["OrganizationId"] = org.OrganizationId;
                    row["InstanceId"] = inst.InstanceId;
                    row["InstanceName"] = insts.Count > 1 ? org.Name + " " + inst.Name : org.Name;
                    if (inst.CreatedTime.HasValue) row["CreationDate"] = inst.CreatedTime;
                    else row["CreationDate"] = DBNull.Value;
                    row["AdminFullName"] = afName;
                    row["AdminEmail"] = aEmail;
                    row["AdminPhone"] = aPhone;
                    foreach (Setting setting in counterSettings)
                        row[setting.ShortName] = setting.GetCounterValue(org.OrganizationId, inst.InstanceId);

                    SettingCollection settings=SettingProvider.GetCounterSettings(org.OrganizationId, inst.InstanceId);
                    foreach (Setting setting in settings)
                    {
                        if (setting.ShortName == "PhoneSupport") continue;
                        if (setting.ShortName == "Training1Hour") continue;
                        if (setting.ShortName == "Training3Hours") continue;
                        if (setting.ShortName == "Training8Hours") continue;

                        if (setting.Paid)
                        {
                            bool enabled = false;
                            if (!Boolean.TryParse(setting.Value, out enabled))
                            {
                                if (!Boolean.TryParse(setting.DefaultValue, out enabled)) enabled = false;
                            }
                            row[setting.ShortName] = enabled;
                            if (enabled) monthlySum += setting.Price;
                            continue;
                        }
                        int usageCount = 0;
                        int.TryParse(setting.Value, out usageCount);
                        int paidQty = usageCount - setting.UsageCountLimit;
                        decimal priceMonth = paidQty > 0 ? paidQty * setting.Price : 0;
                        monthlySum += priceMonth;
                        row[setting.ShortName] = usageCount;
                    }
                    row["MonthlyBillable"] = monthlySum;
                    row["CreditCardStatus"] = inst.CreditCardStatus.ToString();
                    dt.Rows.Add(row);
                }
            }

            cgvList.DataSource = dt;
            cgvList.DataBind();

        }

        #endregion
    }
}
