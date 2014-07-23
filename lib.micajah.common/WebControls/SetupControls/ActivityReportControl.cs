using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Dal;
using Micajah.Common.Properties;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls.SetupControls
{
    /// <summary>
    /// The control to display activity report.
    /// </summary>
    public class ActivityReportControl : UserControl
    {
        #region Constants

        private const string OrganizationIdColumnName = "OrganizationId";
        private const string InstanceIdColumnName = "InstanceId";
        private const string InstanceNameColumnName = "InstanceName";
        private const string CreationDateColumnName = "CreationDate";
        private const string AdminFullNameColumnName = "AdminFullName";
        private const string AdminEmailColumnName = "AdminEmail";
        private const string AdminPhoneColumnName = "AdminPhone";
        private const string MonthlyBillableColumnName = "MonthlyBillable";
        private const string CreditCardStatusColumnName = "CreditCardStatus";
        private const string HowYouHearAboutUsColumnName = "HowYouHearAboutUs";

        #endregion

        #region Members

        protected CommonGridView List;
        protected HyperLink ExportLink;

        private SettingCollection m_CounterSettings;
        private SettingCollection m_PaidSettings;

        #endregion

        #region Private Properties

        private DataTable ReportTable
        {
            get
            {
                DataTable table = null;

                try
                {
                    Type integerType = typeof(int);
                    Type booleanType = typeof(bool);
                    Type stringType = typeof(string);
                    Type guidType = typeof(Guid);

                    table = new DataTable();
                    table.Locale = CultureInfo.CurrentCulture;

                    table.Columns.Add(new DataColumn(OrganizationIdColumnName, guidType));
                    table.Columns.Add(new DataColumn(InstanceIdColumnName, guidType));
                    table.Columns.Add(new DataColumn(InstanceNameColumnName, stringType));
                    table.Columns.Add(new DataColumn(CreationDateColumnName, typeof(DateTime)));
                    table.Columns.Add(new DataColumn(AdminFullNameColumnName, stringType));
                    table.Columns.Add(new DataColumn(AdminEmailColumnName, stringType));
                    table.Columns.Add(new DataColumn(AdminPhoneColumnName, stringType));

                    foreach (Setting setting in m_CounterSettings)
                    {
                        using (DataColumn column = new DataColumn(setting.ShortName, integerType))
                        {
                            column.DefaultValue = -1;
                            table.Columns.Add(column);
                        }
                    }

                    foreach (Setting setting in m_PaidSettings)
                    {
                        using (DataColumn column = new DataColumn(setting.ShortName))
                        {
                            if (setting.Paid)
                            {
                                column.DataType = booleanType;
                            }
                            else
                            {
                                column.DataType = integerType;
                                column.DefaultValue = -1;
                            }

                            table.Columns.Add(column);
                        }
                    }

                    table.Columns.Add(new DataColumn(MonthlyBillableColumnName, typeof(decimal)));
                    table.Columns.Add(new DataColumn(CreditCardStatusColumnName, stringType));
                    table.Columns.Add(new DataColumn(HowYouHearAboutUsColumnName, stringType));

                    return table;
                }
                finally
                {
                    if (table != null)
                    {
                        table.Dispose();
                    }
                }
            }
        }

        private DataTable ReportDataSource
        {
            get
            {
                DataTable table = this.ReportTable;

                OrganizationCollection orgs = OrganizationProvider.GetOrganizations(false, false);

                foreach (Organization org in orgs)
                {
                    ClientDataSet.UserRow orgAdmin = UserProvider.GetOrganizationAdministratorUserRow(org.OrganizationId);

                    string adminFullName = string.Empty;
                    string adminEmail = string.Empty;
                    string adminPhone = string.Empty;

                    if (orgAdmin != null)
                    {
                        adminFullName = (!string.IsNullOrEmpty(orgAdmin.FirstName) ? orgAdmin.FirstName : string.Empty) + (!string.IsNullOrEmpty(orgAdmin.LastName) ? " " + orgAdmin.LastName : string.Empty);
                        adminEmail = !string.IsNullOrEmpty(orgAdmin.Email) ? orgAdmin.Email : string.Empty;
                        adminPhone = !string.IsNullOrEmpty(orgAdmin.Phone) ? orgAdmin.Phone : string.Empty;
                    }

                    InstanceCollection insts = InstanceProvider.GetInstances(org.OrganizationId, false);

                    if (insts.Count > 0)
                    {
                        foreach (Instance inst in insts)
                        {
                            decimal monthlySum = 0;

                            DataRow row = table.NewRow();
                            row[OrganizationIdColumnName] = org.OrganizationId;
                            row[InstanceIdColumnName] = inst.InstanceId;
                            row[InstanceNameColumnName] = (insts.Count > 1 ? org.Name + " " + inst.Name : org.Name);
                            if (inst.CreatedTime.HasValue)
                            {
                                row[CreationDateColumnName] = inst.CreatedTime;
                            }
                            row[AdminFullNameColumnName] = adminFullName;
                            row[AdminEmailColumnName] = adminEmail;
                            row[AdminPhoneColumnName] = adminPhone;
                            row[CreditCardStatusColumnName] = Support.SplitCamelCase(inst.CreditCardStatus.ToString());
                            row[HowYouHearAboutUsColumnName] = org.HowYouHearAboutUs;

                            SettingCollection settings = CounterSettingProvider.GetCalculatedAllCounterSettings(org.OrganizationId, inst.InstanceId);

                            foreach (Setting setting in settings)
                            {
                                if (setting.SettingType == SettingType.Counter)
                                {
                                    int sVal = 0;
                                    if (!int.TryParse(setting.Value, out sVal))
                                    {
                                        sVal = -1;
                                    }

                                    row[setting.ShortName] = sVal;

                                    continue;
                                }

                                if (setting.Paid)
                                {
                                    if ((setting.ShortName == "PhoneSupport") || (setting.ShortName == "Training1Hour") || (setting.ShortName == "Training3Hours") || (setting.ShortName == "Training8Hours"))
                                    {
                                        continue;
                                    }

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
                                if (!int.TryParse(setting.Value, out usageCount))
                                {
                                    usageCount = 0;
                                }

                                int paidQty = usageCount - setting.UsageCountLimit;
                                decimal priceMonth = paidQty > 0 ? paidQty * setting.Price : 0;

                                monthlySum += priceMonth;

                                row[setting.ShortName] = usageCount;
                            }

                            row[MonthlyBillableColumnName] = monthlySum;

                            table.Rows.Add(row);
                        }
                    }
                }

                return table;
            }
        }

        private bool IsExport
        {
            get
            {
                return (string.Compare(Request.QueryString["file"], "excel", StringComparison.OrdinalIgnoreCase) == 0);
            }
        }

        #endregion

        #region Private Methods

        private byte[] ExportToExcel()
        {
            MemoryStream stream = null;
            ExcelPackage package = null;

            try
            {
                stream = new MemoryStream();
                package = new ExcelPackage(stream);

                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ActivityReport");
                //Add the headers
                worksheet.Cells[1, 1].Value = "Instance Name";
                worksheet.Cells[1, 2].Value = "Created";
                worksheet.Cells[1, 3].Value = "Admin Name";
                worksheet.Cells[1, 4].Value = "Admin Email";
                worksheet.Cells[1, 5].Value = "Admin Phone";

                int i = 6;

                foreach (Setting setting in m_CounterSettings)
                {
                    worksheet.Cells[1, i].Value = setting.CustomName;
                    i++;
                }

                foreach (Setting setting in m_PaidSettings)
                {
                    worksheet.Cells[1, i].Value = setting.CustomName;
                    i++;
                }

                worksheet.Cells[1, i].Value = Resources.ActivityReportControl_List_MonthlyBillableColumn_HeaderText;
                i++;

                worksheet.Cells[1, i].Value = Resources.ActivityReportControl_List_CreditCardStatusColumn_HeaderText;
                i++;

                worksheet.Cells[1, i].Value = Resources.ActivityReportControl_List_HowYouHearAboutUsColumn_HeaderText;

                using (var range = worksheet.Cells[1, 1, 1, i])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                i = 2;

                DataTable table = this.ReportDataSource;

                foreach (DataRow row in table.Rows)
                {
                    int j = 1;

                    foreach (DataColumn col in table.Columns)
                    {
                        if (col.ColumnName == OrganizationIdColumnName) continue;
                        if (col.ColumnName == InstanceIdColumnName) continue;

                        worksheet.Cells[i, j].Value = row[col];

                        if (col.DataType == typeof(decimal))
                            worksheet.Cells[i, j].Style.Numberformat.Format = "$#,##0.00";
                        else if (col.DataType == typeof(DateTime))
                            worksheet.Cells[i, j].Style.Numberformat.Format = "d-MMM-yyyy";

                        j++;
                    }

                    i++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                return package.GetAsByteArray();
            }
            finally
            {
                if (package != null)
                {
                    package.Dispose();
                }
                else if (stream != null)
                {
                    stream.Dispose();
                }
            }
        }

        private void InitializeList()
        {
            List.Columns[0].HeaderText = Resources.ActivityReportControl_List_InstanceNameColumn_HeaderText;
            List.Columns[1].HeaderText = Resources.ActivityReportControl_List_CreationDateColumn_HeaderText;
            List.Columns[2].HeaderText = Resources.ActivityReportControl_List_AdminFullNameColumn_HeaderText;
            List.Columns[3].HeaderText = Resources.ActivityReportControl_List_AdminEmailColumn_HeaderText;
            List.Columns[4].HeaderText = Resources.ActivityReportControl_List_AdminPhoneColumn_HeaderText;

            foreach (Setting setting in m_CounterSettings)
            {
                BoundField col = new BoundField();
                col.HeaderText = setting.CustomName;
                col.DataField = setting.ShortName;
                List.Columns.Add(col);
            }

            foreach (Setting setting in m_PaidSettings)
            {
                BoundField col = new BoundField();
                col.HeaderText = setting.CustomName;
                col.DataField = setting.ShortName;
                List.Columns.Add(col);
            }

            BoundField colGrid = new BoundField();
            colGrid.HeaderText = Resources.ActivityReportControl_List_MonthlyBillableColumn_HeaderText;
            colGrid.DataField = MonthlyBillableColumnName;
            colGrid.DataFormatString = "{0:C}";
            List.Columns.Add(colGrid);

            colGrid = new BoundField();
            colGrid.HeaderText = Resources.ActivityReportControl_List_CreditCardStatusColumn_HeaderText;
            colGrid.DataField = CreditCardStatusColumnName;
            List.Columns.Add(colGrid);

            colGrid = new BoundField();
            colGrid.HeaderText = Resources.ActivityReportControl_List_HowYouHearAboutUsColumn_HeaderText;
            colGrid.DataField = HowYouHearAboutUsColumnName;
            List.Columns.Add(colGrid);
        }

        private void InitializeSettings()
        {
            m_CounterSettings = new SettingCollection();
            m_PaidSettings = new SettingCollection();

            ConfigurationDataSet.SettingDataTable table = ConfigurationDataSet.Current.Setting;

            string filter = string.Format(CultureInfo.InvariantCulture, "{0} = {1}", table.SettingTypeIdColumn.ColumnName, (int)SettingType.Counter);
            foreach (ConfigurationDataSet.SettingRow row in table.Select(filter))
            {
                m_CounterSettings.Add(SettingProvider.CreateSetting(row));
            }

            filter = string.Format(CultureInfo.InvariantCulture, "{0} > 0", table.PriceColumn.ColumnName);
            foreach (ConfigurationDataSet.SettingRow row in table.Select(filter))
            {
                if ((row.ShortName == "PhoneSupport") || (row.ShortName == "Training1Hour") || (row.ShortName == "Training3Hours") || (row.ShortName == "Training8Hours"))
                {
                    continue;
                }

                m_PaidSettings.Add(SettingProvider.CreateSetting(row));
            }
        }

        private void SetPageTitle()
        {
            Micajah.Common.Pages.MasterPage masterPage = (Micajah.Common.Pages.MasterPage)this.Page.Master;

            DateTime? lastCalculationDate = CounterSettingProvider.GetLastCalculationDate();
            if (lastCalculationDate.HasValue)
            {
                masterPage.CustomName = string.Format(CultureInfo.CurrentCulture, Resources.ActivityReportControl_MasterPage_CustomName_Format, lastCalculationDate.Value);
            }
            else
            {
                masterPage.CustomName = Resources.ActivityReportControl_MasterPage_CustomName;
            }
        }

        #endregion

        #region Overriden Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Micajah.Common.Pages.MasterPage.InitializeSetupPage(this.Page);

            ExportLink.Text = Resources.ActivityReportControl_ExportLink_Text;
            ExportLink.NavigateUrl = Request.Url.ToString() + "?file=excel";

            this.SetPageTitle();

            this.InitializeSettings();

            if (!this.IsExport)
            {
                this.InitializeList();

                List.DataSource = this.ReportDataSource;
                List.DataBind();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.IsExport)
            {
                byte[] responseData = this.ExportToExcel();

                Response.Clear();
                Response.ContentType = MimeType.OpenXmlSpreadsheet;
                Response.AddHeader("content-length", responseData.LongLength.ToString(CultureInfo.InvariantCulture));
                Response.AddHeader("content-disposition", string.Format(CultureInfo.CurrentCulture, "attachment;filename=ActivityReport_{0:yyyymmdd}.xlsx", DateTime.UtcNow));
                Response.BinaryWrite(responseData);
                Response.End();
            }
            else
            {
                base.Render(writer);
            }
        }

        #endregion
    }
}
