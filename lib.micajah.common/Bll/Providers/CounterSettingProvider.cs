using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Micajah.Common.Dal;

namespace Micajah.Common.Bll.Providers
{
    public class CounterSettingProvider
    {
        public static decimal GetCalculatedMonthlyPaidSum(Guid OrgId, Guid InstId)
        {
            decimal? monthlySum = 0;

            monthlySum = (decimal)Micajah.Common.Application.CacheManager.Current.Get(string.Format("MonthlyPaidSum{0}{1}",OrgId, InstId));

            if (monthlySum.HasValue) return monthlySum.Value;

            SettingCollection settings = CounterSettingProvider.GetCalculatedPaidSettings(OrgId, InstId);
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
                    if (enabled) monthlySum += setting.Price;
                    continue;
                }
                int usageCount = 0;
                int.TryParse(setting.Value, out usageCount);
                int paidQty = usageCount - setting.UsageCountLimit;
                decimal priceMonth = paidQty > 0 ? paidQty * setting.Price : 0;
                monthlySum += priceMonth;
            }

            Micajah.Common.Application.CacheManager.Current.Put(string.Format("MonthlyPaidSum{0}{1}", OrgId, InstId), monthlySum.Value, TimeSpan.FromMinutes(1440));

            return monthlySum.Value;
        }

        public static int GetCalculatedCounterSettingValue(Guid OrgId, Guid InstId, Guid SettingId)
        {
            int? counterVal = null;

            counterVal = (int)Micajah.Common.Application.CacheManager.Current.Get(string.Format("CounterSettingValue{0}{1}{2}", OrgId, InstId, SettingId));

            if (counterVal.HasValue) return counterVal.Value;

            string cnnString = GetSettingsValuesHistoryDbConnectionString();

            if (!string.IsNullOrEmpty(cnnString))
            {
                DataTable dt = SelectByQuery(string.Format("SELECT TOP 1 SettingValue FROM SettingsValuesHistory WHERE OrganizationId='{0}' AND InstanceId='{1}' AND SettingId='{2}' ORDER BY Id DESC", OrgId, InstId, SettingId));
                if (dt.Rows.Count > 0 && !dt.Rows[0].IsNull(0))
                {
                    int cVal=0;
                    if (int.TryParse(dt.Rows[0][0].ToString(), out cVal))
                    {
                        Micajah.Common.Application.CacheManager.Current.Put(string.Format("CounterSettingValue{0}{1}{2}", OrgId, InstId, SettingId), cVal, TimeSpan.FromMinutes(1440));
                        return cVal;
                    }
                }
            }

            Micajah.Common.Bll.Handlers.SettingHandler handler = Micajah.Common.Bll.Handlers.SettingHandler.Current;

            Setting s = SettingProvider.GetSetting(SettingId);
            counterVal = handler.GetUsedItemsCount(s, OrgId, InstId);

            Micajah.Common.Application.CacheManager.Current.Put(string.Format("CounterSettingValue{0}{1}{2}", OrgId, InstId, SettingId), counterVal.Value, TimeSpan.FromMinutes(1440));

            return counterVal.Value;
        }

        public static SettingCollection GetLastModifiedPaidSettings(Guid OrgId, Guid InstId, DateTime? FromDate)
        {
            if (!FromDate.HasValue) return SettingProvider.GetAllPricedSettings(OrgId, InstId);

            string cnnString = GetSettingsValuesHistoryDbConnectionString();

            if (string.IsNullOrEmpty(cnnString)) return SettingProvider.GetAllPricedSettings(OrgId, InstId);

            DataTable dtS = SelectLastModifiedSettings(OrgId, InstId, FromDate.Value);

            SettingCollection resCol = new SettingCollection();

            if (dtS.Rows.Count == 0) return resCol;

            dtS.PrimaryKey = new DataColumn[] { dtS.Columns["SettingId"] };

            ConfigurationDataSet.SettingDataTable table = ConfigurationDataSet.Current.Setting;
            string filter = string.Format(CultureInfo.InvariantCulture, "{0} > 0", table.PriceColumn.ColumnName);

            foreach (ConfigurationDataSet.SettingRow _srow in table.Select(filter))
            {
                DataRow rowS = dtS.Rows.Find(_srow[table.SettingIdColumn.ColumnName]);
                if (rowS == null) continue;
                Setting s = SettingProvider.CreateSetting(_srow);
                s.Value = rowS["SettingValue"].ToString();
                resCol.Add(s);
            }

            return resCol;
        }

        public static SettingCollection GetCalculatedPaidSettings(Guid OrgId, Guid InstId)
        {
            string cnnString = GetSettingsValuesHistoryDbConnectionString();

            if (string.IsNullOrEmpty(cnnString)) return SettingProvider.GetAllPricedSettings(OrgId, InstId);

            DataTable dtLastValues = SelectSettingsLastValues(OrgId, InstId);

            if (dtLastValues.Rows.Count == 0) return SettingProvider.GetAllPricedSettings(OrgId, InstId);
            
            dtLastValues.PrimaryKey = new DataColumn[] { dtLastValues.Columns["SettingId"] };

            SettingCollection resCol = new SettingCollection();
            ConfigurationDataSet.SettingDataTable table = ConfigurationDataSet.Current.Setting;
            string filter = string.Format(CultureInfo.InvariantCulture, "{0} > 0", table.PriceColumn.ColumnName);
            Micajah.Common.Bll.Handlers.SettingHandler handler = Micajah.Common.Bll.Handlers.SettingHandler.Current;

            foreach (ConfigurationDataSet.SettingRow _srow in table.Select(filter))
            {
                Setting s = SettingProvider.CreateSetting(_srow);
                DataRow rowS = dtLastValues.Rows.Find(_srow[table.SettingIdColumn.ColumnName]);
                if (rowS != null) s.Value = rowS["SettingValue"].ToString();
                else
                {
                    int settingVal = handler.GetUsedItemsCount(s, OrgId, InstId);
                    s.Value = settingVal.ToString(CultureInfo.InvariantCulture);
                }
                resCol.Add(s);
            }

            return resCol;
        }

        public static SettingCollection GetCalculatedAllCounterSettings(Guid OrgId, Guid InstId)
        {
            string cnnString = GetSettingsValuesHistoryDbConnectionString();
            ConfigurationDataSet.SettingDataTable table = ConfigurationDataSet.Current.Setting;
            string filter = string.Format(CultureInfo.InvariantCulture, "{0} = " + ((int)SettingType.Counter).ToString(), table.SettingTypeIdColumn.ColumnName);
            SettingCollection counterSettings = new SettingCollection();
            Micajah.Common.Bll.Handlers.SettingHandler handler = Micajah.Common.Bll.Handlers.SettingHandler.Current;
            DataTable dtLastValues = null;

            if (!string.IsNullOrEmpty(cnnString)) dtLastValues = SelectSettingsLastValues(OrgId, InstId);
            if (dtLastValues==null || dtLastValues.Rows.Count==0)
            {
                foreach (ConfigurationDataSet.SettingRow _srow in table.Select(filter))
                {
                    Setting s = SettingProvider.CreateSetting(_srow);
                    s.Value = handler.GetUsedItemsCount(s, OrgId, InstId).ToString(CultureInfo.InvariantCulture);
                    counterSettings.Add(s);
                }

                filter = string.Format(CultureInfo.InvariantCulture, "{0} > 0", table.PriceColumn.ColumnName);

                foreach (ConfigurationDataSet.SettingRow _srow in table.Select(filter))
                {
                    Setting s = SettingProvider.CreateSetting(_srow);
                    s.Value = handler.GetUsedItemsCount(s, OrgId, InstId).ToString(CultureInfo.InvariantCulture);
                    counterSettings.Add(s);
                }

                return counterSettings;
            }

            dtLastValues.PrimaryKey = new DataColumn[] { dtLastValues.Columns["SettingId"] };

            foreach (ConfigurationDataSet.SettingRow _srow in table.Select(filter))
            {
                Setting s = SettingProvider.CreateSetting(_srow);
                DataRow rowS = dtLastValues.Rows.Find(_srow[table.SettingIdColumn.ColumnName]);
                if (rowS != null) s.Value = rowS["SettingValue"].ToString();
                else s.Value = handler.GetUsedItemsCount(s, OrgId, InstId).ToString(CultureInfo.InvariantCulture);
                counterSettings.Add(s);
            }

            filter = string.Format(CultureInfo.InvariantCulture, "{0} > 0", table.PriceColumn.ColumnName);

            foreach (ConfigurationDataSet.SettingRow _srow in table.Select(filter))
            {
                Setting s = SettingProvider.CreateSetting(_srow);
                DataRow rowS = dtLastValues.Rows.Find(_srow[table.SettingIdColumn.ColumnName]);
                if (rowS != null) s.Value = rowS["SettingValue"].ToString();
                else s.Value = handler.GetUsedItemsCount(s, OrgId, InstId).ToString(CultureInfo.InvariantCulture);
                counterSettings.Add(s);
            }

            return counterSettings;
        }

        public static void CalculateCounterSettingsValues()
        {
            string cnnString = GetSettingsValuesHistoryDbConnectionString();

            if (string.IsNullOrEmpty(cnnString)) return;

            ConfigurationDataSet.SettingDataTable table = ConfigurationDataSet.Current.Setting;
            string filter = string.Format(CultureInfo.InvariantCulture, "{0} = " + ((int)SettingType.Counter).ToString(), table.SettingTypeIdColumn.ColumnName);
            SettingCollection counterSettings = new SettingCollection();
            foreach (ConfigurationDataSet.SettingRow _srow in table.Select(filter)) counterSettings.Add(SettingProvider.CreateSetting(_srow));

            filter = string.Format(CultureInfo.InvariantCulture, "{0} > 0", table.PriceColumn.ColumnName);
            SettingCollection paidSettings = new SettingCollection();
            foreach (ConfigurationDataSet.SettingRow _srow in table.Select(filter)) paidSettings.Add(SettingProvider.CreateSetting(_srow));

            if (counterSettings.Count == 0 && paidSettings.Count == 0) return;

            OrganizationCollection orgs = OrganizationProvider.GetOrganizations(false, false);

            DataTable dtModifiedSettings = new DataTable();
            dtModifiedSettings.Columns.Add(new DataColumn("OrganizationId", typeof(Guid)));
            dtModifiedSettings.Columns.Add(new DataColumn("InstanceId", typeof(Guid)));
            dtModifiedSettings.Columns.Add(new DataColumn("SettingId", typeof(Guid)));
            dtModifiedSettings.Columns.Add(new DataColumn("SettingValue", typeof(string)));

            Micajah.Common.Bll.Handlers.SettingHandler handler = Micajah.Common.Bll.Handlers.SettingHandler.Current;

            foreach (Organization org in orgs)
            {
                InstanceCollection insts = InstanceProvider.GetInstances(org.OrganizationId, false);
                if (insts.Count == 0) continue;

                foreach (Instance inst in insts)
                {
                    DataTable dtLastValues = SelectSettingsLastValues(org.OrganizationId, inst.InstanceId);
                    dtLastValues.PrimaryKey = new DataColumn[] { dtLastValues.Columns["SettingId"] };

                    foreach (Setting setting in counterSettings)
                    {
                        int settingVal = handler.GetUsedItemsCount(setting, org.OrganizationId, inst.InstanceId);
                        DataRow rowLastVal = dtLastValues.Rows.Find(setting.SettingId);
                        if (rowLastVal == null || rowLastVal["Value"].ToString() != settingVal.ToString())
                        {
                            DataRow rowNewVal = dtModifiedSettings.NewRow();
                            rowNewVal["OrganizationId"] = org.OrganizationId;
                            rowNewVal["InstanceId"] = inst.InstanceId;
                            rowNewVal["SettingId"] = setting.SettingId;
                            rowNewVal["SettingValue"] = settingVal.ToString(CultureInfo.InvariantCulture);
                            dtModifiedSettings.Rows.Add(rowNewVal);
                        }
                    }

                    SettingCollection settings = SettingProvider.GetAllPricedSettings(org.OrganizationId, inst.InstanceId);
                    foreach (Setting setting in settings)
                    {
                        string settingVal = string.Empty;
                        if (setting.Paid) 
                        {
                            if (string.IsNullOrEmpty(setting.Value)) settingVal = setting.DefaultValue;
                            else settingVal = setting.Value;
                        }
                        else settingVal = handler.GetUsedItemsCount(setting, org.OrganizationId, inst.InstanceId).ToString();
                        
                        DataRow rowLastVal = dtLastValues.Rows.Find(setting.SettingId);

                        if (rowLastVal == null || rowLastVal["Value"].ToString() != settingVal)
                        {
                            DataRow rowNewVal = dtModifiedSettings.NewRow();
                            rowNewVal["OrganizationId"] = org.OrganizationId;
                            rowNewVal["InstanceId"] = inst.InstanceId;
                            rowNewVal["SettingId"] = setting.SettingId;
                            rowNewVal["SettingValue"] = settingVal.ToString(CultureInfo.InvariantCulture);
                            dtModifiedSettings.Rows.Add(rowNewVal);
                        }
                    }
                }
            }

            if (dtModifiedSettings.Rows.Count == 0) return;

            using (SqlConnection connection = new SqlConnection(cnnString))
            {
                using (var cmd = GetInsertHistoryRecordCommand(connection))
                {
                    SqlParameter pOrgId = cmd.Parameters["@OrganizationId"];
                    SqlParameter pInstId = cmd.Parameters["@InstanceId"];
                    SqlParameter pSettingId = cmd.Parameters["@SettingId"];
                    SqlParameter pSettingValue = cmd.Parameters["@SettingValue"];
                    cmd.Connection.Open();
                    foreach (DataRow rowS in dtModifiedSettings.Rows)
                    {
                        pOrgId.Value = rowS["OrganizationId"];
                        pInstId.Value = rowS["InstanceId"];
                        pSettingId.Value = rowS["SettingId"];
                        pSettingValue.Value = rowS["SettingValue"];
                        cmd.ExecuteNonQuery();
                    }
                    pOrgId.Value = Guid.Empty;
                    pInstId.Value = Guid.Empty;
                    pSettingId.Value = Guid.Empty;
                    pSettingValue.Value = DBNull.Value;
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
            }
        }

        private static SqlCommand GetInsertHistoryRecordCommand(SqlConnection cnn)
        {
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO SettingsValuesHistory (OrganizationId, InstanceId, SettingId, UpdatedAt, SettingValue) VALUES (@OrganizationId, @InstanceId, @SettingId, @UpdatedAt, @SettingValue)";
            cmd.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@SettingId", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@SettingValue", SqlDbType.NVarChar, -1));
            cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.UtcNow);
            return cmd;
        }

        internal static void SetDateTimeMark(byte MarkType)
        {
            string cnnString = GetSettingsValuesHistoryDbConnectionString();

            if (string.IsNullOrEmpty(cnnString)) return;

            using (SqlConnection connection = new SqlConnection(cnnString))
            {
                using (var cmd = GetInsertHistoryRecordCommand(connection))
                {
                    cmd.Parameters["@OrganizationId"].Value=Guid.Empty;
                    cmd.Parameters["@InstanceId"].Value=Guid.Empty;
                    byte[] g=Guid.Empty.ToByteArray();
                    g[g.Length-1]=MarkType;
                    cmd.Parameters["@SettingId"].Value=new Guid(g);
                    cmd.Parameters["@SettingValue"].Value=DBNull.Value;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
            }
        }

        internal static DateTime? GetDateTimeMark(byte MarkType)
        {
            string cnnString = GetSettingsValuesHistoryDbConnectionString();

            if (string.IsNullOrEmpty(cnnString)) return null;

            byte[] g = Guid.Empty.ToByteArray();
            g[g.Length - 1] = MarkType;

            DataTable dt = SelectByQuery(string.Format("SELECT MAX(UpdatedAt) AS Mark FROM SettingsValuesHistory WHERE OrganizationId='{0}' AND InstanceId='{1}' AND SettingId='{2}'", Guid.Empty, Guid.Empty, new Guid(g)));

            if (dt.Rows.Count == 0) return null;

            if (dt.Rows[0].IsNull(0)) return null;

            return (DateTime)dt.Rows[0][0]; 
        }

        internal static DateTime? GetLastCalculationDate()
        {
            string cnnString = GetSettingsValuesHistoryDbConnectionString();

            if (string.IsNullOrEmpty(cnnString)) return null;

            DataTable dt = SelectByQuery(string.Format("SELECT MAX(UpdatedAt) AS Mark FROM SettingsValuesHistory WHERE OrganizationId='{0}' AND InstanceId='{1}' AND SettingId='{2}'", Guid.Empty, Guid.Empty, Guid.Empty));

            if (dt.Rows.Count == 0) return null;

            if (dt.Rows[0].IsNull(0)) return null;

            return (DateTime)dt.Rows[0][0];
        }

        private static string GetSettingsValuesHistoryDbConnectionString()
        {
            string cnnName = Configuration.FrameworkConfiguration.Current.WebApplication.Integration.Chargify.HistoryConnectionStringName;
            if (string.IsNullOrEmpty(cnnName)) return string.Empty;
            if (System.Configuration.ConfigurationManager.ConnectionStrings[cnnName] == null) throw new ArgumentException("Can't find Settings Values History Database Connection Sting with Name=\"" + cnnName + "\"");
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.ConnectionStrings[cnnName].ConnectionString)) throw new ArgumentException("Connection String for Settings Values History Database is not defined.");
            return System.Configuration.ConfigurationManager.ConnectionStrings[cnnName].ConnectionString;
        }

        private static DataTable SelectLastModifiedSettings(Guid OrgId, Guid InstId, DateTime FromDate)
        {
            return SelectByQuery(string.Format("SELECT svh.SettingId, svh.SettingValue FROM SettingsValuesHistory svh INNER JOIN (SELECT SettingId, Max(Id) As LastId FROM SettingsValuesHistory WHERE OrganizationId='{0}' AND InstanceId='{1}' AND UpdatedAt > '{2}' GROUP BY SettingId) svhg ON svh.Id=svhg.LastId", OrgId, InstId, FromDate.ToString("yyyyMMdd HH:mm")));
        }

        private static DataTable SelectSettingsLastValues(Guid OrgId, Guid InstId)
        {
            return SelectByQuery(string.Format("SELECT svh.SettingId, svh.SettingValue FROM SettingsValuesHistory svh INNER JOIN (SELECT SettingId, Max(Id) As LastId FROM SettingsValuesHistory WHERE OrganizationId='{0}' AND InstanceId='{1}' GROUP BY SettingId) svhg ON svh.Id=svhg.LastId", OrgId, InstId));
        }

        private static DataTable SelectByQuery(string Query)
        {
            using (SqlConnection connection = new SqlConnection(GetSettingsValuesHistoryDbConnectionString()))
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = Query;
                    var tb = new DataTable();
                    using (var da = new SqlDataAdapter(cmd))
                        da.Fill(tb);
                    if (cmd.Connection.State == ConnectionState.Open) cmd.Connection.Close();
                    return tb;
                }
            }
        }
    }
}
