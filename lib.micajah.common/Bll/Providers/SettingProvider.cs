using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using Micajah.Common.Application;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Dal.TableAdapters;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with settings.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class SettingProvider
    {
        #region Members

        /// <summary>
        /// The identifier of the master page's custom style sheet.
        /// </summary>
        internal readonly static Guid MasterPageCustomStyleSheetSettingId = new Guid("00000000-0000-0000-0000-000000000066");

        // The object that is used to synchronize access to the global settings collection.
        private static readonly object s_GlobalSettingsSyncRoot = new object();
        private static readonly object s_GroupSettingsExistSyncRoot = new object();

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
                SettingCollection coll = CacheManager.Current.Get("mc.GlobalSettings") as SettingCollection;
                if (coll == null)
                {
                    lock (s_GlobalSettingsSyncRoot)
                    {
                        coll = CacheManager.Current.Get("mc.GlobalSettings") as SettingCollection;
                        if (coll == null)
                        {
                            coll = SettingProvider.GetSettings(SettingLevels.Global, null);

                            CacheManager.Current.Add("mc.GlobalSettings", coll);
                        }
                    }
                }
                return coll;
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

        private static string GetSettingsValuesDataTableFilter(OrganizationDataSet.SettingsValuesDataTable svTable
            , Guid organizationId, Guid instanceId)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(CultureInfo.InvariantCulture, "{0} = '{{0}}' AND {1} = '{2}' AND {3}"
                , svTable.SettingIdColumn.ColumnName
                , svTable.OrganizationIdColumn.ColumnName, organizationId.ToString()
                , svTable.InstanceIdColumn.ColumnName);

            if (instanceId != Guid.Empty)
                sb.AppendFormat(CultureInfo.InvariantCulture, " = '{0}' AND {1} = '{{1}}'", instanceId.ToString(), svTable.GroupIdColumn.ColumnName);
            else
                sb.AppendFormat(CultureInfo.InvariantCulture, " IS NULL AND {0} IS NULL", svTable.GroupIdColumn.ColumnName);

            return sb.ToString();
        }

        private static string GetSettingsValuesDataTableFilter(OrganizationDataSet.SettingsValuesDataTable svTable
            , Guid organizationId, Guid instanceId, Guid groupId)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(CultureInfo.InvariantCulture, "{0} = '{{0}}' AND {1} = '{2}' AND {3}"
                , svTable.SettingIdColumn.ColumnName
                , svTable.OrganizationIdColumn.ColumnName, organizationId.ToString()
                , svTable.InstanceIdColumn.ColumnName);

            if (instanceId != Guid.Empty)
            {
                if (groupId != Guid.Empty)
                    sb.AppendFormat(CultureInfo.InvariantCulture, " = '{0}' AND {1} = '{2}'", instanceId.ToString(), svTable.GroupIdColumn.ColumnName, groupId.ToString());
                else
                    sb.AppendFormat(CultureInfo.InvariantCulture, " = '{0}' AND {1} IS NULL", instanceId.ToString(), svTable.GroupIdColumn.ColumnName);
            }
            else
                sb.AppendFormat(CultureInfo.InvariantCulture, " IS NULL AND {0} IS NULL", svTable.GroupIdColumn.ColumnName);

            return sb.ToString();
        }

        private static void LoadSettingAttributes(CommonDataSet.SettingRow row, SettingElement setting, SettingLevels? levels)
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

            if (!levels.HasValue) levels = setting.Levels;
            if ((levels.Value & SettingLevels.Instance) == SettingLevels.Instance)
                row.EnableInstance = true;
            if ((levels.Value & SettingLevels.Organization) == SettingLevels.Organization)
                row.EnableOrganization = true;
            if ((levels.Value & SettingLevels.Group) == SettingLevels.Group)
                row.EnableGroup = true;
        }

        /// <summary>
        /// Updates the value of the specified setting in the data row.
        /// </summary>
        /// <param name="setting">The setting to update.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="instanceId">The identifier of the instance.</param>
        /// <param name="groupId">The identifier of the group.</param>
        /// <param name="svTable"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private static OrganizationDataSet.SettingsValuesRow UpdateSettingValueRow(Setting setting, Guid organizationId, Guid? instanceId, Guid? groupId
            , OrganizationDataSet.SettingsValuesDataTable svTable, string filter)
        {
            OrganizationDataSet.SettingsValuesRow svRow = null;

            Guid instId = instanceId.GetValueOrDefault(Guid.Empty);
            Guid grpId = groupId.GetValueOrDefault(Guid.Empty);

            if (string.IsNullOrEmpty(filter))
                filter = GetSettingsValuesDataTableFilter(svTable, organizationId, instId, grpId);

            DataRow[] svRows = svTable.Select(string.Format(CultureInfo.CurrentCulture, filter, setting.SettingId));

            if (svRows.Length > 0)
            {
                svRow = (svRows[0] as OrganizationDataSet.SettingsValuesRow);
                if (svRow != null)
                {
                    if (setting.ValueIsDefault)
                        svRow.Delete();
                    else
                        svRow.Value = setting.Value;
                }
            }
            else if (!setting.ValueIsDefault)
            {
                svRow = svTable.NewSettingsValuesRow();
                svRow.SettingValueId = Guid.NewGuid();
                svRow.SettingId = setting.SettingId;
                svRow.Value = setting.Value;

                if (organizationId != Guid.Empty)
                    svRow.OrganizationId = organizationId;
                else
                    svRow.SetOrganizationIdNull();

                if (instId != Guid.Empty)
                    svRow.InstanceId = instId;
                else
                    svRow.SetInstanceIdNull();

                if (grpId != Guid.Empty)
                    svRow.GroupId = grpId;
                else
                    svRow.SetGroupIdNull();
            }

            return svRow;
        }

        /// <summary>
        /// Updates the values of the specified settings.
        /// </summary>
        /// <param name="settings">>The settings to update.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="instanceId">The identifier of the instance.</param>
        /// <param name="groupId">The identifier of the group.</param>
        private static void UpdateSettingsValues(SettingCollection settings, Guid organizationId, Guid? instanceId, Guid? groupId)
        {
            if (settings == null) return;

            OrganizationDataSet orgDataSet = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            if (orgDataSet == null) return;

            OrganizationDataSet.SettingsValuesDataTable svTable = orgDataSet.SettingsValues;
            Guid instId = instanceId.GetValueOrDefault(Guid.Empty);
            Guid grpId = groupId.GetValueOrDefault(Guid.Empty);
            string filter = GetSettingsValuesDataTableFilter(svTable, organizationId, instId, grpId);

            foreach (Setting setting in settings)
            {
                OrganizationDataSet.SettingsValuesRow svRow = UpdateSettingValueRow(setting, organizationId, instanceId, groupId, svTable, filter);
                if (svRow != null)
                {
                    if (svRow.RowState == DataRowState.Detached)
                        svTable.AddSettingsValuesRow(svRow);
                }
            }

            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            adapters.SettingsValuesTableAdapter.Update(svTable);

            Refresh();
        }

        #endregion

        #region Internal Methods

        internal static Setting CreateSetting(CommonDataSet.SettingRow row)
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
                return setting;
            }
            return null;
        }

        internal static Setting CreateSetting(CommonDataSet.SettingRow row, Setting parentSetting)
        {
            Setting setting = CreateSetting(row);
            if (setting != null)
                setting.ParentSetting = parentSetting;
            return setting;
        }

        internal static void Fill(CommonDataSet dataSet)
        {
            if (dataSet == null) return;

            Fill(dataSet.Setting, dataSet.SettingListsValues, FrameworkConfiguration.Current.Settings, null, null, null);

            dataSet.Setting.AcceptChanges();
            dataSet.SettingListsValues.AcceptChanges();
        }

        internal static void Fill(CommonDataSet.SettingDataTable settingTable
            , CommonDataSet.SettingListsValuesDataTable settingListsValuesTable
            , SettingElementCollection settings, Guid? parentSettingId, Guid? actionId, SettingLevels? levels)
        {
            foreach (SettingElement setting in settings)
            {
                CommonDataSet.SettingRow settingRow = settingTable.NewSettingRow();
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
                        CommonDataSet.SettingListsValuesRow settingListsValuesRow = settingListsValuesTable.NewSettingListsValuesRow();
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
            foreach (CommonDataSet.SettingRow row in settingRows)
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
            if (settings == null) return;

            OrganizationDataSet orgDataSet = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            if (orgDataSet == null) return;

            OrganizationDataSet.SettingsValuesDataTable svTable = orgDataSet.SettingsValues;
            DataRow[] svRow = null;
            string columnName = svTable.ValueColumn.ColumnName;
            string filter = GetSettingsValuesDataTableFilter(svTable, organizationId, Guid.Empty, Guid.Empty);

            foreach (Setting setting in settings)
            {
                svRow = svTable.Select(string.Format(CultureInfo.CurrentCulture, filter, setting.SettingId));
                if (svRow.Length > 0)
                    setting.Value = svRow[0][columnName].ToString();
                else
                    setting.Value = setting.DefaultValue;
            }
        }

        /// <summary>
        /// Fills the settings values or defaults values by the values of the specified instance.
        /// </summary>
        /// <param name="settings">The settings to fill values in.</param>
        /// <param name="organizationId">The identifier of the organization which the instance belong to.</param>
        /// <param name="instanceId">The identifier of the instance to get values of.</param>
        internal static void FillSettingsByInstanceValues(ref SettingCollection settings, Guid organizationId, Guid instanceId)
        {
            if (settings == null) return;

            OrganizationDataSet orgDataSet = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            if (orgDataSet == null) return;

            FillSettingsByOrganizationValues(ref settings, organizationId);

            OrganizationDataSet.SettingsValuesDataTable svTable = orgDataSet.SettingsValues;
            string columnName = svTable.ValueColumn.ColumnName;
            string filter = GetSettingsValuesDataTableFilter(svTable, organizationId, instanceId, Guid.Empty);

            DataRow[] svRow = null;
            foreach (Setting setting in settings)
            {
                svRow = svTable.Select(string.Format(CultureInfo.CurrentCulture, filter, setting.SettingId));
                if (svRow.Length > 0)
                {
                    if (setting.EnableOrganization)
                        setting.DefaultValue = setting.Value;
                    setting.Value = svRow[0][columnName].ToString();
                }
                else if (!setting.EnableOrganization)
                    setting.Value = setting.DefaultValue;
            }
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
            if (settings == null) return;

            OrganizationDataSet orgDataSet = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            if (orgDataSet == null) return;

            FillSettingsByInstanceValues(ref settings, organizationId, instanceId);

            OrganizationDataSet.SettingsValuesDataTable svTable = orgDataSet.SettingsValues;
            string columnName = svTable.ValueColumn.ColumnName;
            string filter = GetSettingsValuesDataTableFilter(svTable, organizationId, instanceId, groupId);

            DataRow[] svRow = null;
            foreach (Setting setting in settings)
            {
                svRow = svTable.Select(string.Format(CultureInfo.CurrentCulture, filter, setting.SettingId));
                if (svRow.Length > 0)
                {
                    if (setting.EnableOrganization || setting.EnableInstance)
                        setting.DefaultValue = setting.Value;
                    setting.Value = svRow[0][columnName].ToString();
                }
                else if ((!setting.EnableOrganization) && (!setting.EnableInstance))
                    setting.Value = setting.DefaultValue;
            }
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
            if ((settings == null) || (groupIdList == null)) return;

            OrganizationDataSet orgDataSet = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            if (orgDataSet == null) return;

            FillSettingsByInstanceValues(ref settings, organizationId, instanceId);

            OrganizationDataSet.SettingsValuesDataTable svTable = orgDataSet.SettingsValues;
            DataRow[] svRow = null;
            Type booleanType = typeof(bool);
            string columnName = svTable.ValueColumn.ColumnName;
            string filter = GetSettingsValuesDataTableFilter(svTable, organizationId, instanceId);
            object value1 = null;
            object value2 = null;
            string newValue = null;

            foreach (Guid groupId in groupIdList)
            {
                foreach (Setting setting in settings)
                {
                    svRow = svTable.Select(string.Format(CultureInfo.CurrentCulture, filter, setting.SettingId, groupId));
                    newValue = null;

                    if (svRow.Length > 0)
                    {
                        if (setting.Values.Count == 0)
                        {
                            if (setting.EnableOrganization || setting.EnableInstance)
                                setting.DefaultValue = setting.Value;
                        }
                        newValue = svRow[0][columnName].ToString();
                    }
                    else if ((!setting.EnableOrganization) && (!setting.EnableInstance))
                        newValue = setting.DefaultValue;

                    if (newValue == null) continue;

                    if (setting.Values.Count == 0)
                    {
                        setting.Value = newValue;
                        setting.Values.Add(newValue);
                    }
                    else if (!setting.Values.Contains(newValue))
                    {
                        if ((setting.SettingType == SettingType.CheckBox) || (setting.SettingType == SettingType.OnOffSwitch))
                        {
                            value1 = Support.ConvertStringToType(setting.Value, booleanType);
                            value2 = Support.ConvertStringToType(newValue, booleanType);
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

            CommonDataSet.SettingDataTable table = WebApplication.CommonDataSet.Setting;
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

                CommonDataSet.SettingDataTable table = WebApplication.CommonDataSet.Setting;
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
            CommonDataSet.SettingDataTable table = WebApplication.CommonDataSet.Setting;
            string filter = string.Format(CultureInfo.InvariantCulture, "{0} = 1", table.PaidColumn.ColumnName);
            SettingCollection settings = new SettingCollection();
            foreach (CommonDataSet.SettingRow _srow in table.Select(filter)) settings.Add(CreateSetting(_srow));
            if (instanceId != Guid.Empty) FillSettingsByInstanceValues(ref settings, organizationId, instanceId);
            else
            {
                OrganizationDataSet orgDataSet = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
                OrganizationDataSet.SettingsValuesDataTable svTable = orgDataSet.SettingsValues;
                DataRow[] svRow = null;
                string columnName = svTable.ValueColumn.ColumnName;

                foreach (Setting setting in settings)
                {
                    setting.Value = setting.DefaultValue;
                    svRow = svTable.Select(string.Format(CultureInfo.CurrentCulture, "{0}='{1}' AND {2}='{3}'", svTable.SettingIdColumn.ColumnName, setting.SettingId, svTable.OrganizationIdColumn.ColumnName, organizationId));
                    foreach (DataRow _row in svRow)
                    {
                        bool _checked=false;
                        if (bool.TryParse(_row[columnName].ToString(), out _checked) && _checked)
                        {
                            setting.Value = _row[columnName].ToString();
                            break;
                        }
                    }
                }
            }
            settings.Sort();
            return settings;
        }

        internal static SettingCollection GetPaidSettings(Guid organizationId)
        {
            return GetPaidSettings(organizationId, Guid.Empty);
        }

        internal static SettingCollection GetCounterSettings(Guid organizationId)
        {
            CommonDataSet.SettingDataTable table = WebApplication.CommonDataSet.Setting;
            string filter = string.Format(CultureInfo.InvariantCulture, "{0} > 0 AND {1} = 0", table.PriceColumn.ColumnName, table.PaidColumn.ColumnName);
            SettingCollection settings = new SettingCollection();
            foreach (CommonDataSet.SettingRow _srow in table.Select(filter))
            {
                Setting _setting = CreateSetting(_srow);
                int _cval = _setting.GetCounterValue(organizationId);
                if (_cval < 0) continue;
                _setting.Value = _cval.ToString(CultureInfo.InvariantCulture);
                settings.Add(_setting);
            }
            settings.Sort();
            return settings;
        }

        /// <summary>
        /// Returns a collection of the organization level settings.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization to get the values of the settings.</param>
        /// <returns>The collection of the organization level settings.</returns>
        internal static SettingCollection GetOrganizationSettings(Guid organizationId)
        {
            SettingCollection settings = GetSettings(SettingLevels.Organization, null);
            FillSettingsByOrganizationValues(ref settings, organizationId);
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
        /// Returns a collection of the instance level settings.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization which the instance belong to.</param>
        /// <param name="instanceId">The identifier of the instance to get the values of the settings.</param>
        /// <returns>The collection of the instance level settings.</returns>
        internal static SettingCollection GetInstanceSettings(Guid organizationId, Guid instanceId)
        {
            SettingCollection settings = GetSettings(SettingLevels.Instance, null);
            FillSettingsByInstanceValues(ref settings, organizationId, instanceId);
            return settings;
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

        internal static void Refresh()
        {
            lock (s_GlobalSettingsSyncRoot)
            {
                CacheManager.Current.Remove("mc.GlobalSettings");
            }

            s_GroupSettingsExist = null;
        }

        /// <summary>
        /// Updates the instance level settings for the specified organization's instance.
        /// </summary>
        /// <param name="settings">The settings to update.</param>
        /// <param name="organizationId">The identifier of the organization which the instance belong to.</param>
        /// <param name="instanceId">The identifier of the instance to update the settings for.</param>
        internal static void UpdateInstanceSettingsValues(SettingCollection settings, Guid organizationId, Guid instanceId)
        {
            UpdateSettingsValues(settings, organizationId, instanceId, null);
        }

        /// <summary>
        /// Updates the group level settings for the specified organization's group in specified instance.
        /// </summary>
        /// <param name="settings">The settings to update.</param>
        /// <param name="organizationId">The identifier of the organization which the group belong to.</param>
        /// <param name="instanceId">The identifier of the instance.</param>
        /// <param name="groupId">The identifier of the group to update the settings for.</param>
        internal static void UpdateGroupSettingsValues(SettingCollection settings, Guid organizationId, Guid instanceId, Guid groupId)
        {
            UpdateSettingsValues(settings, organizationId, instanceId, groupId);
        }

        /// <summary>
        /// Updates the organization level settings for the specified organization.
        /// </summary>
        /// <param name="settings">The settings to update.</param>
        /// <param name="organizationId">The identifier of the organization to update the settings for.</param>
        internal static void UpdateOrganizationSettingsValues(SettingCollection settings, Guid organizationId)
        {
            UpdateSettingsValues(settings, organizationId, null, null);
        }

        internal static void InitializeStartMenuCheckedItemsSetting(Guid organizationId, Guid? instanceId)
        {
            Setting setting = GetSettingByShortName("StartMenuCheckedItems");
            if (setting != null)
            {
                setting.Value = bool.TrueString;
                SettingProvider.UpdateSettingValue(setting, organizationId, instanceId, null);
            }
        }

        #endregion

        #region Public Methods

        public static void CopySettingValues(Guid fromOrganizationId, Guid fromInstanceId, Guid toOrganizationId, Guid toInstanceId)
        {
            OrganizationDataSet fromOrgDataSet = WebApplication.GetOrganizationDataSetByOrganizationId(fromOrganizationId);
            if (fromOrgDataSet == null) return;

            OrganizationDataSet toOrgDataSet = WebApplication.GetOrganizationDataSetByOrganizationId(toOrganizationId);
            if (toOrgDataSet == null) return;

            OrganizationDataSet.SettingsValuesDataTable fromTable = fromOrgDataSet.SettingsValues;
            OrganizationDataSet.SettingsValuesDataTable toTable = toOrgDataSet.SettingsValues;

            foreach (OrganizationDataSet.SettingsValuesRow fromRow in fromTable.Select(string.Format(CultureInfo.InvariantCulture, "{0} = '{1}' AND {2} = '{3}' AND {4} IS NULL"
                , fromTable.OrganizationIdColumn.ColumnName, fromOrganizationId.ToString()
                , fromTable.InstanceIdColumn.ColumnName, fromInstanceId.ToString()
                , fromTable.GroupIdColumn.ColumnName)))
            {
                OrganizationDataSet.SettingsValuesRow toRow = null;
                DataRow[] rows = toTable.Select(string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", fromTable.SettingIdColumn.ColumnName, fromRow.SettingId.ToString()));
                if (rows.Length > 0)
                    toRow = (OrganizationDataSet.SettingsValuesRow)rows[0];

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

            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(toOrganizationId);
            adapters.SettingsValuesTableAdapter.Update(toTable);

            WebApplication.RefreshOrganizationDataSetByOrganizationId(toOrganizationId);
        }

        /// <summary>
        /// Gets the settings, excluding marked as deleted.
        /// </summary>
        /// <returns>The System.Data.DataTable that contains settings.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetSettings()
        {
            return WebApplication.CommonDataSet.Setting;
        }

        /// <summary>
        /// Gets the root settings, excluding the built-in settings.
        /// </summary>
        /// <returns>The System.Data.DataTable that contains settings.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetRootSettings()
        {
            CommonDataSet.SettingDataTable table = WebApplication.CommonDataSet.Setting;
            DataTable newTable = WebApplication.CommonDataSet.Setting.Clone();

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
            DataTable newTable = WebApplication.CommonDataSet.Setting.Clone();
            CommonDataSet.SettingRow row = WebApplication.CommonDataSet.Setting.FindBySettingId(settingId);

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
            CommonDataSet.SettingDataTable table = WebApplication.CommonDataSet.Setting;
            DataTable newTable = WebApplication.CommonDataSet.Setting.Clone();

            foreach (DataRow dr in table.Select(string.Concat(table.PaidColumn.ColumnName, " = 1")))
            {
                newTable.ImportRow(dr);
            }

            return newTable;
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
            return CreateSetting(WebApplication.CommonDataSet.Setting.FindBySettingId(settingId));
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
            CommonDataSet.SettingDataTable table = WebApplication.CommonDataSet.Setting;
            DataRow[] rows = table.Select(string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", table.ShortNameColumn.ColumnName, Support.PreserveSingleQuote(shortName)));
            return ((rows.Length > 0) ? CreateSetting((CommonDataSet.SettingRow)rows[0]) : null);
        }

        /// <summary>
        /// Gets the values of the specified setting, excluding marked as deleted.
        /// </summary>
        /// <returns>An array of the System.Data.DataRow that contains setting's values.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetSettingListValues(Guid settingId)
        {
            DataTable table = WebApplication.CommonDataSet.SettingListsValues.Clone();
            CommonDataSet.SettingRow row = WebApplication.CommonDataSet.Setting.FindBySettingId(settingId);
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
        public static CommonDataSet.SettingListsValuesRow GetSettingListsValuesRow(Guid settingListValueId)
        {
            return WebApplication.CommonDataSet.SettingListsValues.FindBySettingListValueId(settingListValueId);
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
            if (setting == null) return;

            OrganizationDataSet orgDataSet = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            if (orgDataSet == null) return;

            OrganizationDataSet.SettingsValuesDataTable svTable = orgDataSet.SettingsValues;

            OrganizationDataSet.SettingsValuesRow svRow = UpdateSettingValueRow(setting, organizationId, instanceId, groupId, svTable, null);
            if (svRow.RowState == DataRowState.Detached)
                svTable.AddSettingsValuesRow(svRow);

            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            adapters.SettingsValuesTableAdapter.Update(svRow);

            Refresh();
        }

        #endregion
    }
}
