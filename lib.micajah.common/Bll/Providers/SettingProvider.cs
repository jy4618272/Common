using Micajah.Common.Application;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Dal.ClientDataSetTableAdapters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with settings.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class SettingProvider
    {
        #region Members

        private const string OrganizationSettingsValuesKeyFromat = "mc.OrganizationSettingsValues.{0:N}";
        private const string InstanceSettingsValuesKeyFromat = "mc.InstanceSettingsValues.{0:N}";
        private const string GroupSettingsValuesKeyFromat = "mc.GroupSettingsValues.{0:N}.{1:N}";
        private const string OrganizationEmailSettingsKeyFromat = "mc.OrganizationEmailSettings.{0:N}";

        /// <summary>
        /// The identifier of the master page's custom style sheet.
        /// </summary>
        internal readonly static Guid CustomStyleSheetSettingId = new Guid("00000000-0000-0000-0000-000000000066");

        // The object that is used to synchronize access to the global settings collection.
        private static readonly object s_GlobalSettingsSyncRoot = new object();
        private static readonly object s_GroupSettingsExistSyncRoot = new object();

        private static SettingCollection s_GlobalSettings;
        private static bool? s_GroupSettingsExist;

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets a collection that contains the global settings.
        /// </summary>
        public static SettingCollection GlobalSettings
        {
            get
            {
                if (s_GlobalSettings == null)
                {
                    lock (s_GlobalSettingsSyncRoot)
                    {
                        if (s_GlobalSettings == null)
                        {
                            s_GlobalSettings = GetSettings(SettingLevels.Global, null);
                        }
                    }
                }
                return s_GlobalSettings;
            }
        }

        /// <summary>
        /// Gets a value indicating that the group level settings exist.
        /// </summary>
        internal static bool GroupSettingsExist
        {
            get
            {
                if (!s_GroupSettingsExist.HasValue)
                {
                    lock (s_GroupSettingsExistSyncRoot)
                    {
                        if (!s_GroupSettingsExist.HasValue)
                            s_GroupSettingsExist = (GetRootSettingRows(SettingLevels.Group, true).Length > 0);
                    }
                }
                return s_GroupSettingsExist.Value;
            }
        }

        #endregion

        #region Private Methods

        #region Cache Methods

        private static Dictionary<Guid, string> GetInstanceSettingsValuesFromCache(Guid instanceId)
        {
            string key = string.Format(CultureInfo.InvariantCulture, InstanceSettingsValuesKeyFromat, instanceId);
            return CacheManager.Current.Get(key, true) as Dictionary<Guid, string>;
        }

        private static Dictionary<Guid, string> GetGroupSettingsValuesFromCache(Guid instanceId, Guid groupId)
        {
            string key = string.Format(CultureInfo.InvariantCulture, GroupSettingsValuesKeyFromat, instanceId, groupId);
            return CacheManager.Current.Get(key, true) as Dictionary<Guid, string>;
        }

        private static Dictionary<Guid, string> PutOrganizationSettingsValuesToCache(DataTable table, Guid organizationId)
        {
            Dictionary<Guid, string> dict = ConvertSettingsValuesTableToDictionary(table);

            string key = string.Format(CultureInfo.InvariantCulture, OrganizationSettingsValuesKeyFromat, organizationId);
            CacheManager.Current.PutWithDefaultTimeout(key, dict);

            return dict;
        }

        private static Dictionary<Guid, string> PutInstanceSettingsValuesToCache(DataTable table, Guid instanceId)
        {
            Dictionary<Guid, string> dict = ConvertSettingsValuesTableToDictionary(table);

            string key = string.Format(CultureInfo.InvariantCulture, InstanceSettingsValuesKeyFromat, instanceId);
            CacheManager.Current.PutWithDefaultTimeout(key, dict);

            return dict;
        }

        private static Dictionary<Guid, string> PutGroupSettingsValuesToCache(DataTable table, Guid instanceId, Guid groupId)
        {
            Dictionary<Guid, string> dict = ConvertSettingsValuesTableToDictionary(table);

            string key = string.Format(CultureInfo.InvariantCulture, GroupSettingsValuesKeyFromat, instanceId, groupId);
            CacheManager.Current.PutWithDefaultTimeout(key, dict);

            return dict;
        }

        private static void RemoveGroupSettingsValuesFromCache(Guid instanceId, Guid groupId)
        {
            string key = string.Format(CultureInfo.InvariantCulture, GroupSettingsValuesKeyFromat, instanceId, groupId);
            CacheManager.Current.Remove(key);
        }

        private static void RemoveSettingsValuesFromCache(Guid organizationId, Guid? instanceId, Guid? groupId)
        {
            if (instanceId.HasValue)
            {
                if (groupId.HasValue)
                    RemoveGroupSettingsValuesFromCache(instanceId.Value, groupId.Value);
                else
                    RemoveInstanceSettingsValuesFromCache(instanceId.Value);
            }
            else
            {
                RemoveOrganizationSettingsValuesFromCache(organizationId);
                RemoveOrganizationEmailSettingsFromCache(organizationId);
            }
        }

        #endregion

        internal static SettingCollection CreateSettingCollection(DataRow[] rows)
        {
            SettingCollection coll = new SettingCollection();
            if (rows != null)
            {
                foreach (ConfigurationDataSet.SettingRow row in rows)
                {
                    coll.Add(CreateSetting(row));
                }
            }
            return coll;
        }

        private static ClientDataSet.SettingsValuesDataTable GetSettingsValuesByOrganizationIdInstanceId(Guid organizationId, Guid instanceId)
        {
            using (SettingsValuesTableAdapter adapter = new SettingsValuesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetSettingsValuesByOrganizationIdInstanceId(organizationId, instanceId);
            }
        }

        private static ClientDataSet.SettingsValuesDataTable GetSettingsValuesByOrganizationIdInstanceIdGroups(Guid organizationId, Guid instanceId, string groups)
        {
            using (SettingsValuesTableAdapter adapter = new SettingsValuesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetSettingsValuesByOrganizationIdInstanceIdGroups(organizationId, instanceId, groups);
            }
        }

        private static void FillSettingsByOrganizationValues(ref SettingCollection settings, Dictionary<Guid, string> values)
        {
            foreach (Setting setting in settings)
            {
                if (values.ContainsKey(setting.SettingId))
                    setting.Value = values[setting.SettingId];
                else
                    setting.Value = setting.DefaultValue;
            }
        }

        private static void FillSettingsByInstanceValues(ref SettingCollection settings, Dictionary<Guid, string> values)
        {
            foreach (Setting setting in settings)
            {
                if (values.ContainsKey(setting.SettingId))
                {
                    if (setting.EnableOrganization)
                        setting.DefaultValue = setting.Value;
                    setting.Value = values[setting.SettingId];
                }
                else if (!setting.EnableOrganization)
                    setting.Value = setting.DefaultValue;
            }
        }

        private static void FillSettingsByGroupValues(ref SettingCollection settings, Dictionary<Guid, string> values)
        {
            foreach (Setting setting in settings)
            {
                if (values.ContainsKey(setting.SettingId))
                {
                    if (setting.EnableOrganization || setting.EnableInstance)
                        setting.DefaultValue = setting.Value;
                    setting.Value = values[setting.SettingId];
                }
                else if ((!setting.EnableOrganization) && (!setting.EnableInstance))
                    setting.Value = setting.DefaultValue;
            }
        }

        private static SettingCollection GetInstanceSettingsByFilter(Guid organizationId, Guid instanceId, string filter)
        {
            SettingCollection settings = CreateSettingCollection(ConfigurationDataSet.Current.Setting.Select(filter));

            if (settings.Count > 0)
            {
                FillSettingsByInstanceValues(ref settings, organizationId, instanceId);

                settings.Sort();
            }

            return settings;
        }

        private static SettingCollection GetPricedSettings(Guid organizationId, Guid instanceId)
        {
            return GetInstanceSettingsByFilter(organizationId, instanceId, "Price > 0");
        }

        private static void LoadSettingAttributes(ConfigurationDataSet.SettingRow row, SettingElement setting, SettingLevels? levels)
        {
            row.SettingId = setting.Id;
            row.SettingTypeId = (int)setting.SettingType;
            row.Name = setting.Name;
            row.Description = setting.Description;
            row.ShortName = setting.ShortName;
            row.OrderNumber = setting.OrderNumber;
            row.DefaultValue = setting.DefaultValue;
            row.BuiltIn = setting.BuiltIn;
            row.PaidUpgradeUrl = setting.PaidUpgradeUrl;
            row.Paid = setting.Paid;
            row.UsageCountLimit = setting.UsageCountLimit;
            row.Price = setting.Price;
            row.ExternalId = setting.ExternalId;
            row.Visible = setting.Visible;
            row.Handle = setting.Handle;
            row.IconUrl = setting.IconUrl;
            row.ValidationType = setting.Validation.Type;
            row.ValidationExpression = setting.Validation.Expression;
            row.MaximumValue = setting.Validation.MaximumValue;
            row.MinimumValue = setting.Validation.MinimumValue;
            row.MaxLength = setting.Validation.MaxLength;

            if (!levels.HasValue) levels = setting.Levels;
            if ((levels.Value & SettingLevels.Instance) == SettingLevels.Instance)
                row.EnableInstance = true;
            if ((levels.Value & SettingLevels.Organization) == SettingLevels.Organization)
                row.EnableOrganization = true;
            if ((levels.Value & SettingLevels.Group) == SettingLevels.Group)
                row.EnableGroup = true;
        }

        #endregion

        #region Internal Methods

        #region Cache Methods

        internal static Dictionary<Guid, string> GetOrganizationSettingsValuesFromCache(Guid organizationId, bool putToCacheIfNotExists)
        {
            string key = string.Format(CultureInfo.InvariantCulture, OrganizationSettingsValuesKeyFromat, organizationId);
            Dictionary<Guid, string> dict = CacheManager.Current.Get(key, true) as Dictionary<Guid, string>;

            if (dict == null)
            {
                if (putToCacheIfNotExists)
                {
                    dict = ConvertSettingsValuesTableToDictionary(GetSettingsValuesByOrganizationId(organizationId));

                    CacheManager.Current.PutWithDefaultTimeout(key, dict);
                }
            }

            return dict;
        }

        internal static EmailElement GetOrganizationEmailSettingsFromCache(Guid organizationId)
        {
            string key = string.Format(CultureInfo.InvariantCulture, OrganizationEmailSettingsKeyFromat, organizationId);
            EmailElement settings = CacheManager.Current.Get(key) as EmailElement;

            if (settings == null)
            {
                settings = new EmailElement();

                FillSettingsClass(settings, GetOrganizationSettings(organizationId));
            }

            return settings;
        }

        internal static void RemoveInstanceSettingsValuesFromCache(Guid instanceId)
        {
            string key = string.Format(CultureInfo.InvariantCulture, InstanceSettingsValuesKeyFromat, instanceId);
            CacheManager.Current.Remove(key);
        }

        internal static void RemoveOrganizationSettingsValuesFromCache(Guid organizationId)
        {
            string key = string.Format(CultureInfo.InvariantCulture, OrganizationSettingsValuesKeyFromat, organizationId);
            CacheManager.Current.Remove(key);
        }

        internal static void RemoveOrganizationEmailSettingsFromCache(Guid organizationId)
        {
            string key = string.Format(CultureInfo.InvariantCulture, OrganizationEmailSettingsKeyFromat, organizationId);
            CacheManager.Current.Remove(key);
        }

        #endregion

        internal static string GetCustomStyleSheet(Guid organizationId)
        {
            Dictionary<Guid, string> dict = GetOrganizationSettingsValuesFromCache(organizationId, true);
            if (dict.ContainsKey(CustomStyleSheetSettingId))
                return dict[CustomStyleSheetSettingId];
            return null;
        }

        internal static void UpdateCustomStyleSheet(Guid organizationId, string value)
        {
            using (SettingsValuesTableAdapter adapter = new SettingsValuesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                ClientDataSet.SettingsValuesDataTable table = adapter.GetSettingValue(CustomStyleSheetSettingId, organizationId, null, null);
                ClientDataSet.SettingsValuesRow row = null;
                if (table.Count > 0)
                    row = table[0];

                if (row != null)
                {
                    if (Support.StringIsNullOrEmpty(value))
                        row.Delete();
                    else
                        row.Value = value;
                }
                else if (!Support.StringIsNullOrEmpty(value))
                {
                    row = table.NewSettingsValuesRow();
                    row.SettingValueId = Guid.NewGuid();
                    row.SettingId = CustomStyleSheetSettingId;
                    row.Value = value;

                    if (organizationId != Guid.Empty)
                        row.OrganizationId = organizationId;

                    table.AddSettingsValuesRow(row);
                }

                adapter.Update(table);
            }

            RemoveSettingsValuesFromCache(organizationId, null, null);
        }

        internal static Dictionary<Guid, string> ConvertSettingsValuesTableToDictionary(DataTable table)
        {
            Dictionary<Guid, string> dict = new Dictionary<Guid, string>();

            foreach (DataRow row in table.Rows)
            {
                dict.Add((Guid)row["SettingId"], (string)row["Value"]);
            }

            return dict;
        }

        internal static Setting CreateSetting(ConfigurationDataSet.SettingRow row)
        {
            if (row != null)
            {
                Setting setting = new Setting();
                setting.SettingId = row.SettingId;
                setting.ParentSettingId = (row.IsParentSettingIdNull() ? null : new Guid?(row.ParentSettingId));
                setting.SettingTypeId = row.SettingTypeId;
                setting.Name = row.Name;
                setting.Description = row.Description;
                setting.ShortName = row.ShortName;
                setting.DefaultValue = row.DefaultValue;
                setting.Value = (row.IsValueNull() ? null : row.Value);
                setting.OrderNumber = row.OrderNumber;
                setting.EnableOrganization = row.EnableOrganization;
                setting.EnableInstance = row.EnableInstance;
                setting.EnableGroup = row.EnableGroup;
                setting.BuiltIn = row.BuiltIn;
                setting.ActionId = (row.IsActionIdNull() ? null : new Guid?(row.ActionId));
                setting.PaidUpgradeUrl = (row.IsPaidUpgradeUrlNull() ? null : row.PaidUpgradeUrl);
                setting.Paid = (row.IsPaidNull() ? false : row.Paid);
                setting.UsageCountLimit = (row.IsUsageCountLimitNull() ? 0 : row.UsageCountLimit);
                setting.Price = (row.IsPriceNull() ? 0 : row.Price);
                setting.ExternalId = row.IsExternalIdNull() ? string.Empty : row.ExternalId;
                setting.Visible = row.Visible;
                setting.Handle = row.Handle;
                setting.IconUrl = row.IconUrl;
                setting.ValidationType = row.ValidationType;
                setting.ValidationExpression = row.ValidationExpression;
                setting.MaximumValue = row.MaximumValue;
                setting.MinimumValue = row.MinimumValue;
                setting.MaxLength = row.MaxLength;

                return setting;
            }
            return null;
        }

        internal static Setting CreateSetting(ConfigurationDataSet.SettingRow row, Setting parentSetting)
        {
            Setting setting = CreateSetting(row);
            if (setting != null)
                setting.ParentSetting = parentSetting;
            return setting;
        }

        internal static void Fill(ConfigurationDataSet dataSet)
        {
            if (dataSet == null) return;

            Fill(dataSet.Setting, dataSet.SettingListsValues, FrameworkConfiguration.Current.Settings, null, null, null);

            dataSet.Setting.AcceptChanges();
            dataSet.SettingListsValues.AcceptChanges();
        }

        internal static void Fill(ConfigurationDataSet.SettingDataTable settingTable
            , ConfigurationDataSet.SettingListsValuesDataTable settingListsValuesTable
            , SettingElementCollection settings, Guid? parentSettingId, Guid? actionId, SettingLevels? levels)
        {
            foreach (SettingElement setting in settings)
            {
                ConfigurationDataSet.SettingRow settingRow = settingTable.NewSettingRow();
                LoadSettingAttributes(settingRow, setting, levels);
                if (parentSettingId.HasValue)
                    settingRow.ParentSettingId = parentSettingId.Value;
                if (actionId.HasValue)
                    settingRow.ActionId = actionId.Value;
                settingTable.AddSettingRow(settingRow);

                if (settingListsValuesTable != null)
                {
                    foreach (SettingValueElement value in setting.Values)
                    {
                        ConfigurationDataSet.SettingListsValuesRow settingListsValuesRow = settingListsValuesTable.NewSettingListsValuesRow();
                        settingListsValuesRow.SettingListValueId = Guid.NewGuid();
                        settingListsValuesRow.Name = value.Name;
                        settingListsValuesRow.SettingId = settingRow.SettingId;
                        settingListsValuesRow.Value = value.Value;
                        settingListsValuesTable.AddSettingListsValuesRow(settingListsValuesRow);
                    }
                }

                if ((setting.SettingType == SettingType.NotSet) || (setting.SettingType == SettingType.OnOffSwitch))
                    Fill(settingTable, settingListsValuesTable, setting.Settings, settingRow.SettingId, actionId, levels);
            }
        }

        /// <summary>
        /// Fills the specified instance of the settings class by the values from specified settings collection.
        /// </summary>
        /// <param name="obj">The instance of the settings class.</param>
        /// <param name="settings">The settings collection to get the values from.</param>
        internal static void FillSettingsClass(object obj, SettingCollection settings)
        {
            if ((obj == null) || (settings == null)) return;

            BindingFlags bindingFlags = BindingFlags.Default;
            Type type = (obj as Type);
            if (type == null)
            {
                type = obj.GetType();
                bindingFlags = BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance;
            }
            else
            {
                obj = null;
                bindingFlags = BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Static;
            }
            PropertyInfo[] properties = type.GetProperties(bindingFlags);
            if (properties.Length == 0) return;

            Setting elem = settings.FindByShortName(type.Name);
            if (elem == null) return;

            bool enabled = false;
            if (!Boolean.TryParse(elem.Value, out enabled))
            {
                if (!Boolean.TryParse(elem.DefaultValue, out enabled)) enabled = false;
            }

            SettingCollection children = elem.ChildSettings;
            foreach (PropertyInfo property in properties)
            {
                Setting child = children.FindByShortName(property.Name);
                if (child != null)
                {
                    object value = null;
                    if (enabled) value = Support.ConvertStringToType(child.Value, property.PropertyType);
                    if (value == null) value = Support.ConvertStringToType(child.DefaultValue, property.PropertyType);

                    Support.SetPropertyValueSafe(property, obj, value, null);
                }
            }
        }

        /// <summary>
        /// Fills the specified collection by the settings with specified level from the array of the settings rows.
        /// </summary>
        /// <param name="settings">The collection to fill in.</param>
        /// <param name="level">The level of the settings.</param>
        /// <param name="parentSetting">The parent setting.</param>
        /// <param name="settingRows">The settings rows.</param>
        internal static void FillSettings(ref SettingCollection settings, SettingLevels level, Setting parentSetting, DataRow[] settingRows)
        {
            FillSettings(ref  settings, level, parentSetting, settingRows, null);
        }

        /// <summary>
        /// Fills the specified collection by the settings with specified level from the array of the settings rows.
        /// </summary>
        /// <param name="settings">The collection to fill in.</param>
        /// <param name="level">The level of the settings.</param>
        /// <param name="parentSetting">The parent setting.</param>
        /// <param name="settingRows">The settings rows.</param>
        /// <param name="visible">The value of visbile to match the setting before adding to the collection.</param>
        internal static void FillSettings(ref SettingCollection settings, SettingLevels level, Setting parentSetting, DataRow[] settingRows, bool? visible)
        {
            foreach (ConfigurationDataSet.SettingRow row in settingRows)
            {
                if (visible.HasValue)
                {
                    if (row.Visible != visible.Value)
                        continue;
                }

                Setting setting = CreateSetting(row, parentSetting);
                if (((setting.Level & level) == level) || ((setting.Level & level) == setting.Level))
                {
                    settings.Add(setting);

                    FillSettings(ref settings, level, setting, row.GetSettingRows(), visible);

                    if (parentSetting != null)
                    {
                        parentSetting.EnsureChildSettings();
                        if (parentSetting.ChildSettings.FindBySettingId(setting.SettingId) == null)
                            parentSetting.ChildSettings.Add(setting);
                    }
                }
            }

            if (parentSetting != null) parentSetting.ChildSettings.Sort();
        }

        /// <summary>
        /// Fills the settings values or defaults values by the values of the specified organization.
        /// </summary>
        /// <param name="settings">The settings to fill values in.</param>
        /// <param name="organizationId">The identifier of the organization to get values of.</param>
        internal static void FillSettingsByOrganizationValues(ref SettingCollection settings, Guid organizationId)
        {
            if (settings == null)
                return;

            FillSettingsByOrganizationValues(ref settings, GetOrganizationSettingsValuesFromCache(organizationId, true));
        }

        /// <summary>
        /// Fills the settings values or defaults values by the values of the specified instance.
        /// </summary>
        /// <param name="settings">The settings to fill values in.</param>
        /// <param name="organizationId">The identifier of the organization which the instance belong to.</param>
        /// <param name="instanceId">The identifier of the instance to get values of.</param>
        internal static void FillSettingsByInstanceValues(ref SettingCollection settings, Guid organizationId, Guid instanceId)
        {
            if (settings == null)
                return;

            Dictionary<Guid, string> organizationSettingsValues = GetOrganizationSettingsValuesFromCache(organizationId, false);
            Dictionary<Guid, string> instanceSettingsValues = GetInstanceSettingsValuesFromCache(instanceId);

            if ((organizationSettingsValues == null) || (instanceSettingsValues == null))
            {
                ClientDataSet.SettingsValuesDataTable table = GetSettingsValuesByOrganizationIdInstanceId(organizationId, instanceId);

                if (organizationSettingsValues == null)
                {
                    table.DefaultView.RowFilter = string.Format(CultureInfo.InvariantCulture, "{0} IS NULL", table.InstanceIdColumn.ColumnName);
                    organizationSettingsValues = PutOrganizationSettingsValuesToCache(table.DefaultView.ToTable(), organizationId);
                }

                if (instanceSettingsValues == null)
                {
                    table.DefaultView.RowFilter = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", table.InstanceIdColumn.ColumnName, instanceId);
                    instanceSettingsValues = PutInstanceSettingsValuesToCache(table.DefaultView.ToTable(), instanceId);
                }
            }

            FillSettingsByOrganizationValues(ref settings, organizationSettingsValues);
            FillSettingsByInstanceValues(ref settings, instanceSettingsValues);
        }

        /// <summary>
        /// Fills the settings values by the values of the specified group.
        /// </summary>
        /// <param name="settings">The settings to fill values in.</param>
        /// <param name="organizationId">The identifier of the organization which the group belong to.</param>
        /// <param name="instanceId">The identifier of the instance to get values for.</param>
        /// <param name="groupId">The identifier of the group to get values of.</param>
        internal static void FillSettingsByGroupValues(ref SettingCollection settings, Guid organizationId, Guid instanceId, Guid groupId)
        {
            if (settings == null)
                return;

            Dictionary<Guid, string> organizationSettingsValues = GetOrganizationSettingsValuesFromCache(organizationId, false);
            Dictionary<Guid, string> instanceSettingsValues = GetInstanceSettingsValuesFromCache(instanceId);
            Dictionary<Guid, string> groupSettingsValues = GetGroupSettingsValuesFromCache(instanceId, groupId);

            if ((organizationSettingsValues == null) || (instanceSettingsValues == null) || (groupSettingsValues == null))
            {
                ClientDataSet.SettingsValuesDataTable table = GetSettingsValuesByOrganizationIdInstanceIdGroups(organizationId, instanceId, groupId.ToString().ToUpperInvariant());

                if (organizationSettingsValues == null)
                {
                    table.DefaultView.RowFilter = string.Format(CultureInfo.InvariantCulture, "{0} IS NULL AND {1} IS NULL", table.InstanceIdColumn.ColumnName, table.GroupIdColumn.ColumnName);
                    organizationSettingsValues = PutOrganizationSettingsValuesToCache(table.DefaultView.ToTable(), organizationId);
                }

                if (instanceSettingsValues == null)
                {
                    table.DefaultView.RowFilter = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}' AND {2} IS NULL", table.InstanceIdColumn.ColumnName, instanceId, table.GroupIdColumn.ColumnName);
                    instanceSettingsValues = PutInstanceSettingsValuesToCache(table.DefaultView.ToTable(), instanceId);
                }

                if (groupSettingsValues == null)
                {
                    table.DefaultView.RowFilter = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}' AND {2} = '{3}'", table.InstanceIdColumn.ColumnName, instanceId, table.GroupIdColumn.ColumnName, groupId);
                    groupSettingsValues = PutGroupSettingsValuesToCache(table.DefaultView.ToTable(), instanceId, groupId);
                }
            }

            FillSettingsByOrganizationValues(ref settings, organizationSettingsValues);
            FillSettingsByInstanceValues(ref settings, instanceSettingsValues);
            FillSettingsByGroupValues(ref settings, groupSettingsValues);
        }

        /// <summary>
        /// Fills the settings values by the values of the specified groups.
        /// </summary>
        /// <param name="settings">The settings to fill values in.</param>
        /// <param name="organizationId">The identifier of the organization which the group belong to.</param>
        /// <param name="instanceId">The identifier of the instance to get values for.</param>
        /// <param name="groupIdList">The collection of the groups identifiers to get values of.</param>
        internal static void FillSettingsByGroupValues(ref SettingCollection settings, Guid organizationId, Guid instanceId, IList groupIdList)
        {
            if ((settings == null) || (groupIdList == null))
                return;

            Dictionary<Guid, string> organizationSettingsValues = GetOrganizationSettingsValuesFromCache(organizationId, false);
            Dictionary<Guid, string> instanceSettingsValues = GetInstanceSettingsValuesFromCache(instanceId);

            List<Guid> listToGetFromDatabase = new List<Guid>();
            Dictionary<Guid, Dictionary<Guid, string>> dict = new Dictionary<Guid, Dictionary<Guid, string>>();
            foreach (Guid groupId in groupIdList)
            {
                Dictionary<Guid, string> groupSettingsValues = GetGroupSettingsValuesFromCache(instanceId, groupId);
                if (groupSettingsValues == null)
                    listToGetFromDatabase.Add(groupId);
                else
                    dict.Add(groupId, groupSettingsValues);
            }

            if ((organizationSettingsValues == null) || (instanceSettingsValues == null) || (listToGetFromDatabase.Count > 0))
            {
                ClientDataSet.SettingsValuesDataTable table = GetSettingsValuesByOrganizationIdInstanceIdGroups(organizationId, instanceId, Support.ConvertListToString(listToGetFromDatabase).ToUpperInvariant());

                if (organizationSettingsValues == null)
                {
                    table.DefaultView.RowFilter = string.Format(CultureInfo.InvariantCulture, "{0} IS NULL AND {1} IS NULL", table.InstanceIdColumn.ColumnName, table.GroupIdColumn.ColumnName);
                    organizationSettingsValues = PutOrganizationSettingsValuesToCache(table.DefaultView.ToTable(), organizationId);
                }

                if (instanceSettingsValues == null)
                {
                    table.DefaultView.RowFilter = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}' AND {2} IS NULL", table.InstanceIdColumn.ColumnName, instanceId, table.GroupIdColumn.ColumnName);
                    instanceSettingsValues = PutInstanceSettingsValuesToCache(table.DefaultView.ToTable(), instanceId);
                }

                foreach (Guid groupId in listToGetFromDatabase)
                {
                    table.DefaultView.RowFilter = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}' AND {2} = '{3}'", table.InstanceIdColumn.ColumnName, instanceId, table.GroupIdColumn.ColumnName, groupId);
                    Dictionary<Guid, string> groupSettingsValues = PutGroupSettingsValuesToCache(table.DefaultView.ToTable(), instanceId, groupId);
                    dict.Add(groupId, groupSettingsValues);
                }
            }

            FillSettingsByOrganizationValues(ref settings, organizationSettingsValues);
            FillSettingsByInstanceValues(ref settings, instanceSettingsValues);

            Type booleanType = typeof(bool);

            foreach (Guid groupId in dict.Keys)
            {
                Dictionary<Guid, string> values = dict[groupId];

                foreach (Setting setting in settings)
                {
                    string newValue = null;
                    if (values.ContainsKey(setting.SettingId))
                    {
                        if (setting.Values.Count == 0)
                        {
                            if (setting.EnableOrganization || setting.EnableInstance)
                                setting.DefaultValue = setting.Value;
                        }
                        newValue = values[setting.SettingId];
                    }
                    else if ((!setting.EnableOrganization) && (!setting.EnableInstance))
                        newValue = setting.DefaultValue;

                    if (newValue == null)
                        continue;

                    if (setting.Values.Count == 0)
                    {
                        setting.Value = newValue;
                        setting.Values.Add(newValue);
                    }
                    else if (!setting.Values.Contains(newValue))
                    {
                        if ((setting.SettingType == SettingType.CheckBox) || (setting.SettingType == SettingType.OnOffSwitch))
                        {
                            object value1 = null;
                            object value2 = null;

                            bool val = false;
                            if (bool.TryParse(setting.Value, out val))
                                value1 = val;

                            if (bool.TryParse(newValue, out val))
                                value2 = val;

                            if (!((value1 == null) || (value2 == null)))
                            {
                                newValue = ((((bool)value1) && ((bool)value2)) ? "true" : "false");
                                setting.Value = newValue;
                                setting.Values.Add((bool)value2 ? "true" : "false");
                            }
                        }
                        else
                            setting.Values.Add(newValue);
                    }
                }
            }
        }

        internal static ClientDataSet.SettingsValuesDataTable GetSettingsValuesByOrganizationId(Guid organizationId)
        {
            using (SettingsValuesTableAdapter adapter = new SettingsValuesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetSettingsValuesByOrganizationId(organizationId);
            }
        }

        /// <summary>
        /// Returns the rows array of the root settings by specified levels.
        /// </summary>
        /// <param name="level">The levels of the settings to get.</param>
        /// <returns>The rows array of the settings.</returns>
        internal static DataRow[] GetRootSettingRows(SettingLevels level, bool? visible)
        {
            return GetChildrenSettingRows(level, null, visible);
        }

        /// <summary>
        /// Returns the rows array of the immediate child settings with specified levels of specified setting.
        /// </summary>
        /// <param name="level">The levels of the settings to get.</param>
        /// <param name="parentSettingId">The identifier of the setting to get child.</param>
        /// <returns>The rows array of the settings.</returns>
        internal static DataRow[] GetChildrenSettingRows(SettingLevels level, Guid? parentSettingId, bool? visible)
        {
            if (level == SettingLevels.None) return null;

            ConfigurationDataSet.SettingDataTable table = ConfigurationDataSet.Current.Setting;
            StringBuilder sb = new StringBuilder();
            if (parentSettingId.HasValue)
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0} = '{1}'", table.ParentSettingIdColumn.ColumnName, parentSettingId.Value.ToString());
            else
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0} IS NULL", table.ParentSettingIdColumn.ColumnName);

            if (visible.HasValue)
                sb.AppendFormat(CultureInfo.InvariantCulture, " AND {0} = {1}", table.VisibleColumn.ColumnName, (visible.Value ? 1 : 0));

            StringBuilder sb1 = new StringBuilder();

            if ((level & SettingLevels.Global) == SettingLevels.Global)
            {
                sb1.AppendFormat(CultureInfo.InvariantCulture, "({0} = 0 AND {1} = 0 AND {2} = 0)"
                , table.EnableOrganizationColumn.ColumnName
                , table.EnableInstanceColumn.ColumnName
                , table.EnableGroupColumn.ColumnName);
            }

            if ((level & SettingLevels.Organization) == SettingLevels.Organization)
            {
                if (sb1.Length > 0) sb1.Append(" OR ");
                sb1.AppendFormat(CultureInfo.InvariantCulture, "{0} = 1", table.EnableOrganizationColumn.ColumnName);
            }

            if ((level & SettingLevels.Instance) == SettingLevels.Instance)
            {
                if (sb1.Length > 0) sb1.Append(" OR ");
                sb1.AppendFormat(CultureInfo.InvariantCulture, "{0} = 1", table.EnableInstanceColumn.ColumnName);
            }

            if ((level & SettingLevels.Group) == SettingLevels.Group)
            {
                if (sb1.Length > 0) sb1.Append(" OR ");
                sb1.AppendFormat(CultureInfo.InvariantCulture, "{0} = 1", table.EnableGroupColumn.ColumnName);
            }

            if (sb1.Length > 0) sb.AppendFormat(CultureInfo.InvariantCulture, " AND ({0})", sb1.ToString());

            return table.Select(sb.ToString());
        }

        /// <summary>
        /// Returns a collection of the settings with specified levels.
        /// </summary>
        /// <param name="level">The levels of the settings.</param>
        /// <returns>The collection of the settings.</returns>
        /// <param name="visible">The value of visbile to match the setting before adding to the result collection.</param>
        internal static SettingCollection GetSettings(SettingLevels level, bool? visible)
        {
            SettingCollection settings = new SettingCollection();
            FillSettings(ref settings, level, null, GetRootSettingRows(level, visible));
            settings.Sort();
            return settings;
        }

        internal static SettingCollection GetSettings(Guid actionId, Guid organizationId, Guid? instanceId, bool? visible)
        {
            SettingCollection settings = new SettingCollection();

            SettingLevels level = SettingLevels.None;
            if (organizationId != Guid.Empty)
            {
                level = SettingLevels.Organization;
                if (instanceId.HasValue)
                {
                    if (instanceId.Value != Guid.Empty)
                        level = SettingLevels.Instance;
                }

                ConfigurationDataSet.SettingDataTable table = ConfigurationDataSet.Current.Setting;
                string filter = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}' AND {2} IS NULL", table.ActionIdColumn.ColumnName, actionId.ToString(), table.ParentSettingIdColumn.ColumnName);
                if (visible.HasValue) filter += string.Format(CultureInfo.InvariantCulture, " AND {0} = {1}", table.VisibleColumn.ColumnName, (visible.Value ? 1 : 0));
                FillSettings(ref settings, level, null, table.Select(filter), visible);

                if (level == SettingLevels.Organization)
                    FillSettingsByOrganizationValues(ref settings, organizationId);
                else if (level == SettingLevels.Instance)
                    FillSettingsByInstanceValues(ref settings, organizationId, instanceId.Value);

                settings.Sort();
            }

            return settings;
        }

        internal static SettingCollection GetPaidSettings(Guid organizationId, Guid instanceId)
        {
            return GetInstanceSettingsByFilter(organizationId, instanceId, "Paid = 1");
        }

        internal static SettingCollection GetCounterSettings(Guid organizationId, Guid instanceId)
        {
            SettingCollection settings = GetPricedSettings(organizationId, instanceId);

            if (settings.Count > 0)
            {
                Micajah.Common.Bll.Handlers.SettingHandler handler = Micajah.Common.Bll.Handlers.SettingHandler.Current;

                for (int i = 0; i < settings.Count; i++)
                {
                    Setting setting = settings[i];
                    if (!setting.Paid)
                    {
                        int value = handler.GetUsedItemsCount(setting, organizationId, instanceId);
                        if (value < 0)
                        {
                            settings.RemoveAt(i);
                            i--;
                            continue;
                        }
                        setting.Value = value.ToString(CultureInfo.InvariantCulture);
                    }
                }
            }

            return settings;
        }

        internal static SettingCollection GetAllPricedSettings(Guid organizationId, Guid instanceId)
        {
            SettingCollection settings = GetPricedSettings(organizationId, instanceId);

            if (settings.Count > 0)
            {
                Micajah.Common.Bll.Handlers.SettingHandler handler = Micajah.Common.Bll.Handlers.SettingHandler.Current;

                foreach (Setting setting in settings)
                {
                    if (!setting.Paid)
                        setting.Value = handler.GetUsedItemsCount(setting, organizationId, instanceId).ToString(CultureInfo.InvariantCulture);
                }
            }

            return settings;
        }

        /// <summary>
        /// Returns the organization level setting.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization to get the values of the settings.</param>
        /// <returns>The organization level setting.</returns>
        internal static Setting GetOrganizationSetting(Guid organizationId, Guid settingId)
        {
            SettingCollection settings = new SettingCollection();
            settings.Add(GetSetting(settingId));
            FillSettingsByOrganizationValues(ref settings, organizationId);
            return settings[0];
        }

        /// <summary>
        /// Returns a collection of the group level settings for specified group in specified instance.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization which the group belong to.</param>
        /// <param name="instanceId">The identifier of the instance.</param>
        /// <param name="groupId">The identifier of the group to get the values of the settings.</param>
        /// <returns>The collection of the group level settings.</returns>
        internal static SettingCollection GetGroupSettings(Guid organizationId, Guid instanceId, Guid groupId, bool? visible)
        {
            SettingCollection settings = GetSettings(SettingLevels.Group, visible);
            FillSettingsByGroupValues(ref settings, organizationId, instanceId, groupId);
            return settings;
        }

        /// <summary>
        /// Returns a collection of the group level settings for specified group in specified instance.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization which the group belong to.</param>
        /// <param name="instanceId">The identifier of the instance.</param>
        /// <param name="groupIdList">The collection of the groups identifiers to get the values of the settings.</param>
        /// <returns>The collection of the group level settings.</returns>
        internal static SettingCollection GetGroupSettings(Guid organizationId, Guid instanceId, IList groupIdList)
        {
            SettingCollection settings = GetSettings(SettingLevels.Group, null);
            FillSettingsByGroupValues(ref settings, organizationId, instanceId, groupIdList);
            return settings;
        }

        /// <summary>
        /// Updates the values of the specified settings.
        /// </summary>
        /// <param name="settings">>The settings to update.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="instanceId">The identifier of the instance.</param>
        /// <param name="groupId">The identifier of the group.</param>
        internal static void UpdateSettingsValues(SettingCollection settings, Guid organizationId, Guid? instanceId, Guid? groupId)
        {
            if (settings == null)
                return;

            ClientDataSet.SettingsValuesDataTable table = null;
            StringBuilder sb = new StringBuilder();
            if (instanceId.HasValue)
            {
                if (groupId.HasValue)
                {
                    table = GetSettingsValuesByOrganizationIdInstanceIdGroups(organizationId, instanceId.Value, groupId.Value.ToString().ToUpperInvariant());

                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0} = '{{0}}' AND {1} = '{2}' AND {3} = '{4}' AND {5} = '{6}'"
                        , table.SettingIdColumn.ColumnName
                        , table.OrganizationIdColumn.ColumnName, organizationId
                        , table.InstanceIdColumn.ColumnName, instanceId.Value
                        , table.GroupIdColumn.ColumnName, groupId.Value);
                }
                else
                {
                    table = GetSettingsValuesByOrganizationIdInstanceId(organizationId, instanceId.Value);

                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0} = '{{0}}' AND {1} = '{2}' AND {3} = '{4}' AND {5} IS NULL"
                        , table.SettingIdColumn.ColumnName
                        , table.OrganizationIdColumn.ColumnName, organizationId
                        , table.InstanceIdColumn.ColumnName, instanceId.Value
                        , table.GroupIdColumn.ColumnName);
                }
            }
            else
            {
                table = GetSettingsValuesByOrganizationId(organizationId);

                sb.AppendFormat(CultureInfo.InvariantCulture, "{0} = '{{0}}' AND {1} = '{2}' AND {3} IS NULL AND {4} IS NULL"
                    , table.SettingIdColumn.ColumnName
                    , table.OrganizationIdColumn.ColumnName, organizationId
                    , table.InstanceIdColumn.ColumnName
                    , table.GroupIdColumn.ColumnName);
            }

            Guid instId = instanceId.GetValueOrDefault(Guid.Empty);
            Guid grpId = groupId.GetValueOrDefault(Guid.Empty);
            ClientDataSet.SettingsValuesRow row = null;
            string filter = sb.ToString();

            foreach (Setting setting in settings)
            {
                DataRow[] rows = table.Select(string.Format(CultureInfo.InvariantCulture, filter, setting.SettingId));
                if (rows.Length > 0)
                {
                    row = (ClientDataSet.SettingsValuesRow)rows[0];
                    if (row != null)
                    {
                        row.Value = setting.Value;
                    }
                }
                else
                {
                    row = table.NewSettingsValuesRow();
                    row.SettingValueId = Guid.NewGuid();
                    row.SettingId = setting.SettingId;
                    row.Value = setting.Value;

                    if (organizationId != Guid.Empty)
                        row.OrganizationId = organizationId;
                    else
                        row.SetOrganizationIdNull();

                    if (instId != Guid.Empty)
                        row.InstanceId = instId;
                    else
                        row.SetInstanceIdNull();

                    if (grpId != Guid.Empty)
                        row.GroupId = grpId;
                    else
                        row.SetGroupIdNull();

                    table.AddSettingsValuesRow(row);
                }
            }

            using (SettingsValuesTableAdapter adapter = new SettingsValuesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                adapter.Update(table);
            }

            RemoveSettingsValuesFromCache(organizationId, instanceId, groupId);
        }

        #endregion

        #region Public Methods

        public static void CopySettingValues(Guid fromOrganizationId, Guid fromInstanceId, Guid toOrganizationId, Guid toInstanceId)
        {
            ClientDataSet.SettingsValuesDataTable fromTable = GetSettingsValuesByOrganizationIdInstanceId(fromOrganizationId, fromInstanceId);
            ClientDataSet.SettingsValuesDataTable toTable = GetSettingsValuesByOrganizationIdInstanceId(toOrganizationId, toInstanceId);

            foreach (ClientDataSet.SettingsValuesRow fromRow in fromTable)
            {
                DataRow[] rows = fromTable.Select(string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", fromTable.SettingIdColumn.ColumnName, fromRow.SettingId));

                ClientDataSet.SettingsValuesRow toRow = null;
                if (rows.Length > 0)
                    toRow = (ClientDataSet.SettingsValuesRow)rows[0];

                if (toRow == null)
                {
                    toRow = toTable.NewSettingsValuesRow();
                    toRow.SettingValueId = Guid.NewGuid();
                    toRow.SettingId = fromRow.SettingId;
                    toRow.OrganizationId = toOrganizationId;
                    toRow.InstanceId = toInstanceId;
                    toRow.Value = fromRow.Value;
                    toTable.AddSettingsValuesRow(toRow);
                }
                else
                    toRow.Value = fromRow.Value;
            }

            using (SettingsValuesTableAdapter adapter = new SettingsValuesTableAdapter(OrganizationProvider.GetConnectionString(toOrganizationId)))
            {
                adapter.Update(toTable);
            }

            RemoveInstanceSettingsValuesFromCache(toInstanceId);
        }

        /// <summary>
        /// Gets the settings, excluding marked as deleted.
        /// </summary>
        /// <returns>The System.Data.DataTable that contains settings.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetSettings()
        {
            return ConfigurationDataSet.Current.Setting;
        }

        /// <summary>
        /// Gets the root settings, excluding the built-in settings.
        /// </summary>
        /// <returns>The System.Data.DataTable that contains settings.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetRootSettings()
        {
            ConfigurationDataSet.SettingDataTable table = ConfigurationDataSet.Current.Setting;
            DataTable newTable = ConfigurationDataSet.Current.Setting.Clone();

            foreach (DataRow dr in table.Select(string.Concat(table.ParentSettingIdColumn.ColumnName, " IS NULL AND ", table.BuiltInColumn.ColumnName, " = 0")))
            {
                newTable.ImportRow(dr);
            }

            return newTable;
        }

        /// <summary>
        /// Get the child settings of the specified setting.
        /// </summary>
        /// <param name="settingId">The identifier of the setting to get children of.</param>
        /// <returns>The System.Data.DataTable that contains settings.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetChildSettings(Guid settingId)
        {
            DataTable newTable = ConfigurationDataSet.Current.Setting.Clone();
            ConfigurationDataSet.SettingRow row = ConfigurationDataSet.Current.Setting.FindBySettingId(settingId);

            if (row != null)
            {
                foreach (DataRow dr in row.GetSettingRows())
                {
                    newTable.ImportRow(dr);
                }
            }

            return newTable;
        }

        /// <summary>
        /// Gets the paid settings.
        /// </summary>
        /// <returns>The System.Data.DataTable that contains settings.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetPaidSettings()
        {
            ConfigurationDataSet.SettingDataTable table = ConfigurationDataSet.Current.Setting;
            DataTable newTable = ConfigurationDataSet.Current.Setting.Clone();

            foreach (DataRow dr in table.Select(string.Concat(table.PaidColumn.ColumnName, " = 1")))
            {
                newTable.ImportRow(dr);
            }

            return newTable;
        }

        /// <summary>
        /// Returns a collection of the organization level settings.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization to get the values of the settings.</param>
        /// <returns>The collection of the organization level settings.</returns>
        public static SettingCollection GetOrganizationSettings(Guid organizationId)
        {
            SettingCollection settings = GetSettings(SettingLevels.Organization, null);
            FillSettingsByOrganizationValues(ref settings, organizationId);
            return settings;
        }

        /// <summary>
        /// Returns a collection of the instance level settings.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization which the instance belong to.</param>
        /// <param name="instanceId">The identifier of the instance to get the values of the settings.</param>
        /// <returns>The collection of the instance level settings.</returns>
        public static SettingCollection GetInstanceSettings(Guid organizationId, Guid instanceId)
        {
            return GetInstanceSettings(organizationId, instanceId, false);
        }

        /// <summary>
        /// Returns a collection of the instance level settings.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization which the instance belong to.</param>
        /// <param name="instanceId">The identifier of the instance to get the values of the settings.</param>
        /// <param name="refreshCache">Whether the cache of the setting values should be refreshed.</param>
        /// <returns>The collection of the instance level settings.</returns>
        public static SettingCollection GetInstanceSettings(Guid organizationId, Guid instanceId, bool refreshCache)
        {
            SettingCollection settings = GetSettings(SettingLevels.Instance, null);

            if (refreshCache)
            {
                RemoveSettingsValuesFromCache(organizationId, null, null);
                RemoveSettingsValuesFromCache(organizationId, instanceId, null);
            }

            FillSettingsByInstanceValues(ref settings, organizationId, instanceId);

            return settings;
        }

        /// <summary>
        /// Gets an object populated with information of the specified setting.
        /// </summary>
        /// <param name="settingId">Specifies the setting identifier to get information.</param>
        /// <returns>
        /// The object populated with information of the specified setting. 
        /// If the setting is not found, the method returns null reference.
        /// </returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static Setting GetSetting(Guid settingId)
        {
            return CreateSetting(ConfigurationDataSet.Current.Setting.FindBySettingId(settingId));
        }

        /// <summary>
        /// Gets an object populated with information of the specified setting.
        /// </summary>
        /// <param name="shortName">Specifies the short name of the setting.</param>
        /// <returns>
        /// The object populated with information of the specified setting. 
        /// If the setting is not found, the method returns null reference.
        /// </returns>
        public static Setting GetSettingByShortName(string shortName)
        {
            ConfigurationDataSet.SettingDataTable table = ConfigurationDataSet.Current.Setting;
            DataRow[] rows = table.Select(string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", table.ShortNameColumn.ColumnName, Support.PreserveSingleQuote(shortName)));
            return ((rows.Length > 0) ? CreateSetting((ConfigurationDataSet.SettingRow)rows[0]) : null);
        }

        /// <summary>
        /// Gets the values of the specified setting, excluding marked as deleted.
        /// </summary>
        /// <returns>An array of the System.Data.DataRow that contains setting's values.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetSettingListValues(Guid settingId)
        {
            DataTable table = ConfigurationDataSet.Current.SettingListsValues.Clone();
            ConfigurationDataSet.SettingRow row = ConfigurationDataSet.Current.Setting.FindBySettingId(settingId);
            if (row != null)
            {
                foreach (DataRow dr in row.GetSettingListsValuesRows())
                {
                    table.ImportRow(dr);
                }
            }
            return table;
        }

        /// <summary>
        /// Gets an object populated with information of the specified value setting.
        /// </summary>
        /// <param name="settingListValuesId">Specifies the identifier of the setting value.</param>
        /// <returns>
        /// The object populated with information of the specified value setting.
        /// If the setting is not found, the method returns null reference.
        /// </returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ConfigurationDataSet.SettingListsValuesRow GetSettingListsValuesRow(Guid settingListValueId)
        {
            return ConfigurationDataSet.Current.SettingListsValues.FindBySettingListValueId(settingListValueId);
        }

        /// <summary>
        /// Updates the value of the specified setting.
        /// </summary>
        /// <param name="setting">>The setting to update.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="instanceId">The identifier of the instance.</param>
        /// <param name="groupId">The identifier of the group.</param>
        public static void UpdateSettingValue(Setting setting, Guid organizationId, Guid? instanceId, Guid? groupId)
        {
            if (setting == null)
                return;

            using (SettingsValuesTableAdapter adapter = new SettingsValuesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                ClientDataSet.SettingsValuesDataTable table = adapter.GetSettingValue(setting.SettingId, organizationId, instanceId, groupId);
                ClientDataSet.SettingsValuesRow row = null;
                if (table.Count > 0)
                    row = table[0];

                Guid instId = instanceId.GetValueOrDefault(Guid.Empty);
                Guid grpId = groupId.GetValueOrDefault(Guid.Empty);

                if (row != null)
                {
                    row.Value = setting.Value;
                }
                else
                {
                    row = table.NewSettingsValuesRow();
                    row.SettingValueId = Guid.NewGuid();
                    row.SettingId = setting.SettingId;
                    row.Value = setting.Value;

                    if (organizationId != Guid.Empty)
                        row.OrganizationId = organizationId;
                    else
                        row.SetOrganizationIdNull();

                    if (instId != Guid.Empty)
                        row.InstanceId = instId;
                    else
                        row.SetInstanceIdNull();

                    if (grpId != Guid.Empty)
                        row.GroupId = grpId;
                    else
                        row.SetGroupIdNull();

                    table.AddSettingsValuesRow(row);
                }

                adapter.Update(table);
            }

            RemoveSettingsValuesFromCache(organizationId, instanceId, groupId);
        }

        #endregion
    }
}
