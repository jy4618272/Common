using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Dal;
using Micajah.Common.Security;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Micajah.Common.WebControls.Reports
{
    /// <summary>
    /// The control to manage organizations.
    /// </summary>
    public class ActivityReportControl : UserControl
    {
        #region Members

        protected CommonGridView cgvList;
        protected HyperLink hlExcelFile;

        private SettingCollection counterSettings;
        private SettingCollection paidSettings;

        #endregion

        #region Overriden Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            hlExcelFile.NavigateUrl = Request.Url.ToString() + "?file=excel";
            CommonDataSet.SettingDataTable table = Micajah.Common.Application.WebApplication.CommonDataSet.Setting;
            string filter = string.Format(CultureInfo.InvariantCulture, "{0} = "+ ((int)SettingType.Counter).ToString(), table.SettingTypeIdColumn.ColumnName);
            counterSettings = new SettingCollection();
            foreach (CommonDataSet.SettingRow _srow in table.Select(filter)) counterSettings.Add(SettingProvider.CreateSetting(_srow));

            filter = string.Format(CultureInfo.InvariantCulture, "{0} > 0", table.PriceColumn.ColumnName);
            paidSettings = new SettingCollection();
            foreach (CommonDataSet.SettingRow _srow in table.Select(filter)) paidSettings.Add(SettingProvider.CreateSetting(_srow));

            if (!string.IsNullOrEmpty(Request.QueryString["file"]) && Request.QueryString["file"] == "excel") return;

            InitDataGridColumns();
            cgvList.DataSource = BuildReport();
            cgvList.DataBind();

        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(Request.QueryString["file"]) || Request.QueryString["file"]!="excel")
            {
                base.Render(writer);
                return;
            }

            byte[] responseData = ExportDataToExcelOpenXML();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-length", responseData.LongLength.ToString());
            Response.AddHeader("content-disposition", "attachment; filename=ActivityReport_" + string.Format("{0:yyyymmdd}", DateTime.UtcNow) + ".xlsx");
            Response.BinaryWrite(responseData);
            Response.End();
        }

        #endregion

        private byte[] ExportDataToExcelOpenXML()
        {
            using (ExcelPackage package = new ExcelPackage(new MemoryStream()))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ActivityReport");
                //Add the headers
                worksheet.Cells[1, 1].Value = "Instance Name";
                worksheet.Cells[1, 2].Value = "Created";
                worksheet.Cells[1, 3].Value = "Admin Name";
                worksheet.Cells[1, 4].Value = "Admin Email";
                worksheet.Cells[1, 5].Value = "Admin Phone";

                int i = 6;

                foreach (Setting setting in counterSettings)
                {
                    worksheet.Cells[1, i].Value = setting.CustomName;
                    i++;
                }

                foreach (Setting setting in paidSettings)
                {
                    if (setting.ShortName == "PhoneSupport") continue;
                    if (setting.ShortName == "Training1Hour") continue;
                    if (setting.ShortName == "Training3Hours") continue;
                    if (setting.ShortName == "Training8Hours") continue;

                    worksheet.Cells[1, i].Value = setting.CustomName;
                    i++;
                }
                worksheet.Cells[1, i].Value = "Monthly Billable";
                i++;
                worksheet.Cells[1, i].Value = "Credit Card Status";
                i++;
                worksheet.Cells[1, i].Value = "How'd You Hear About Us";

                using (var range = worksheet.Cells[1, 1, 1, i])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                i = 2;
                DataTable dt = BuildReport();

                foreach (DataRow row in dt.Rows)
                {
                    int j = 1;

                    foreach (DataColumn col in dt.Columns)
                    {
                        if (col.ColumnName == "OrganizationId") continue;
                        if (col.ColumnName == "InstanceId") continue;
                        worksheet.Cells[i, j].Value = row[col];
                        if (col.DataType == Type.GetType("System.Decimal")) worksheet.Cells[i, j].Style.Numberformat.Format = "$#,##0.00";
                        else if (col.DataType == Type.GetType("System.DateTime")) worksheet.Cells[i, j].Style.Numberformat.Format = "d-MMM-yyyy";
                        j++;
                    }
                    i++;
                }
                return package.GetAsByteArray();
            }
        }

        private void InitDataGridColumns()
        {
            //Init datagrid columns
            foreach (Setting setting in counterSettings)
            {
                BoundField col = new BoundField();
                col.HeaderText = setting.CustomName;
                col.DataField = setting.ShortName;
                cgvList.Columns.Add(col);
            }

            foreach (Setting setting in paidSettings)
            {
                if (setting.ShortName == "PhoneSupport") continue;
                if (setting.ShortName == "Training1Hour") continue;
                if (setting.ShortName == "Training3Hours") continue;
                if (setting.ShortName == "Training8Hours") continue;

                BoundField col = new BoundField();
                col.HeaderText = setting.CustomName;
                col.DataField = setting.ShortName;
                cgvList.Columns.Add(col);
            }

            BoundField colGrid = new BoundField();
            colGrid.HeaderText = "MonthlyBillable";
            colGrid.DataField = "MonthlyBillable";
            colGrid.DataFormatString = "{0:C}";
            cgvList.Columns.Add(colGrid);
            colGrid = new BoundField();
            colGrid.HeaderText = "Credit Card Status";
            colGrid.DataField = "CreditCardStatus";
            cgvList.Columns.Add(colGrid);
            colGrid = new BoundField();
            colGrid.HeaderText = "How'd You Hear About Us";
            colGrid.DataField = "HowYouHearAboutUs";
            cgvList.Columns.Add(colGrid);
        }

        private DataTable BuildReport()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("OrganizationId", Type.GetType("System.Guid")));
            dt.Columns.Add(new DataColumn("InstanceId", Type.GetType("System.Guid")));
            dt.Columns.Add(new DataColumn("InstanceName", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CreationDate", Type.GetType("System.DateTime")));
            dt.Columns.Add(new DataColumn("AdminFullName", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("AdminEmail", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("AdminPhone", Type.GetType("System.String")));

            foreach (Setting setting in counterSettings)
            {
                DataColumn dtCol = new DataColumn(setting.ShortName, Type.GetType("System.Int32"));
                dtCol.DefaultValue = -1;
                dt.Columns.Add(dtCol);
            }

            foreach (Setting setting in paidSettings)
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
            }

            dt.Columns.Add(new DataColumn("MonthlyBillable", Type.GetType("System.Decimal")));
            dt.Columns.Add(new DataColumn("CreditCardStatus", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("HowYouHearAboutUs", Type.GetType("System.String")));

            OrganizationCollection orgs = OrganizationProvider.GetOrganizations(false, false);

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
                if (orgAdmin != null)
                {
                    afName = (!string.IsNullOrEmpty(orgAdmin.FirstName) ? orgAdmin.FirstName : string.Empty) + (!string.IsNullOrEmpty(orgAdmin.LastName) ? " " + orgAdmin.LastName : string.Empty);
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

                    SettingCollection settings = SettingProvider.GetCounterSettings(org.OrganizationId, inst.InstanceId);
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
                    row["HowYouHearAboutUs"] = org.HowYouHearAboutUs;
                    dt.Rows.Add(row);
                }
            }

            return dt;
        }
    }
}
