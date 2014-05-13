using Micajah.Common.Bll.Providers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Micajah.Common.Bll
{
    /// <summary>
    /// The setting.
    /// </summary>
    [Serializable]
    public sealed class Setting : IComparable<Setting>
    {
        #region Members

        private Guid m_SettingId;
        private Guid? m_ParentSettingId;
        private int m_SettingTypeId;
        private string m_Name;
        private string m_Description;
        private string m_ShortName;
        private string m_DefaultValue;
        private string m_Value;
        private int m_OrderNumber;
        private bool m_EnableOrganization;
        private bool m_EnableInstance;
        private bool m_EnableGroup;
        private bool m_BuiltIn;
        private Guid? m_ActionId;
        private string m_PaidUpgradeUrl;
        private bool m_Paid;
        private int m_UsageCountLimit;
        private decimal m_Price;
        private string m_ExternalId;
        private bool m_Visible;
        private Micajah.Common.Bll.Action m_Action;
        private SettingLevels m_Level;
        private Setting m_ParentSetting;
        private SettingCollection m_ChildSettings;

        private ArrayList m_Values;

        #endregion

        #region Internal Properties

        internal bool BuiltIn
        {
            get { return m_BuiltIn; }
            set { m_BuiltIn = value; }
        }

        /// <summary>
        /// Gets a collection of the conflicting values of the setting.
        /// </summary>
        internal ArrayList Values
        {
            get
            {
                if (m_Values == null) m_Values = new ArrayList();
                return m_Values;
            }
        }

        /// <summary>
        /// Gets a value indicating that the settings have the conflicting values.
        /// </summary>
        internal bool IsConflicting
        {
            get { return ((SettingType != SettingType.CheckBox) && (SettingType != SettingType.OnOffSwitch) && (Values.Count > 1)); }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the identifier of the setting.
        /// </summary>
        public Guid SettingId
        {
            get { return m_SettingId; }
            set { m_SettingId = value; }
        }

        /// <summary>
        /// Gets the identifier of the parent setting.
        /// </summary>
        public Guid? ParentSettingId
        {
            get { return m_ParentSettingId; }
            set
            {
                m_ParentSettingId = value;
                m_ParentSetting = null;
            }
        }

        /// <summary>
        /// Gets or sets the identifier of the setting type.
        /// </summary>
        public int SettingTypeId
        {
            get { return m_SettingTypeId; }
            set { m_SettingTypeId = value; }
        }

        /// <summary>
        /// Gets or sets the name of the setting.
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>
        /// Gets or sets the description of the setting.
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        /// <summary>
        /// Gets or sets the short name of the setting.
        /// </summary>
        public string ShortName
        {
            get { return m_ShortName; }
            set { m_ShortName = value; }
        }

        /// <summary>
        /// Gets or sets the default value of the setting.
        /// </summary>
        public string DefaultValue
        {
            get { return m_DefaultValue; }
            set { m_DefaultValue = value; }
        }

        /// <summary>
        /// Gets or sets the value of the setting.
        /// </summary>
        public string Value
        {
            get
            {
                return ((m_Value == null) ? DefaultValue : m_Value);
            }
            set { m_Value = value; }
        }

        /// <summary>
        /// Gets the counter value of the setting.
        /// </summary>

        /// <summary>
        /// Gets or sets the order number of the setting.
        /// </summary>
        public int OrderNumber
        {
            get { return m_OrderNumber; }
            set { m_OrderNumber = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating that managing is allowed on the organization level.
        /// </summary>
        public bool EnableOrganization
        {
            get { return m_EnableOrganization; }
            set { m_EnableOrganization = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating that managing is allowed on the instance level.
        /// </summary>
        public bool EnableInstance
        {
            get { return m_EnableInstance; }
            set { m_EnableInstance = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating that managing is allowed on the group level.
        /// </summary>
        public bool EnableGroup
        {
            get { return m_EnableGroup; }
            set { m_EnableGroup = value; }
        }

        /// <summary>
        /// Gets or sets the identifier of the action this settings is associated to.
        /// </summary>
        public Guid? ActionId
        {
            get { return m_ActionId; }
            set { m_ActionId = value; }
        }

        /// <summary>
        /// Gets or sets the URL to the paid upgrade page.
        /// </summary>
        public string PaidUpgradeUrl
        {
            get { return m_PaidUpgradeUrl; }
            set { m_PaidUpgradeUrl = value; }
        }

        /// <summary>
        /// Gets or sets the value the setting is paid.
        /// </summary>
        public bool Paid
        {
            get { return m_Paid; }
            set { m_Paid = value; }
        }

        /// <summary>
        /// Gets or sets the value the setting is Calculate Usage
        /// </summary>
        public int UsageCountLimit
        {
            get { return m_UsageCountLimit; }
            set { m_UsageCountLimit = value; }
        }

        /// <summary>
        /// Gets or sets the value the setting Price for paid setting
        /// </summary>
        public decimal Price
        {
            get { return m_Price; }
            set { m_Price = value; }
        }

        /// <summary>
        /// Gets or sets External Id value to bind setting with item in an external system
        /// </summary>
        public string ExternalId
        {
            get { return m_ExternalId; }
            set { m_ExternalId = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the action is visible.
        /// </summary>
        public bool Visible
        {
            get { return m_Visible; }
            set { m_Visible = value; }
        }

        /// <summary>
        /// Gets or sets the data type of values.
        /// </summary>
        public string ValidationType { get; set; }

        /// <summary>
        /// Gets or sets the regular expression that determines the pattern used to validate the value.
        /// </summary>
        public string ValidationExpression { get; set; }

        /// <summary>
        /// Gets or sets the maximum value for the validation range.
        /// </summary>
        public string MaximumValue { get; set; }

        /// <summary>
        /// Gets or sets the minimum value for the validation range.
        /// </summary>
        public string MinimumValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of characters allowed for the value.
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the action this settings is associated to.
        /// </summary>
        public Micajah.Common.Bll.Action Action
        {
            get
            {
                if (m_ActionId.HasValue)
                    m_Action = ActionProvider.FindAction(m_ActionId.Value);
                return m_Action;
            }
            set
            {
                if (value != null)
                    m_ActionId = value.ActionId;
                m_Action = value;
            }
        }

        /// <summary>
        /// Gets the levels of the settings.
        /// </summary>
        public SettingLevels Level
        {
            get
            {
                if (m_Level == SettingLevels.None)
                {
                    if (m_EnableOrganization || m_EnableInstance || m_EnableGroup)
                    {
                        if (m_EnableOrganization) m_Level = m_Level | SettingLevels.Organization;
                        if (m_EnableInstance) m_Level = m_Level | SettingLevels.Instance;
                        if (m_EnableGroup) m_Level = m_Level | SettingLevels.Group;
                    }
                    else
                        m_Level = SettingLevels.Global;
                }
                return m_Level;
            }
        }

        /// <summary>
        /// Gets the parent setting.
        /// </summary>
        public Setting ParentSetting
        {
            get
            {
                if (m_ParentSetting == null && m_ParentSettingId.HasValue)
                    m_ParentSetting = SettingProvider.GetSetting(m_ParentSettingId.Value);
                return m_ParentSetting;
            }
            set
            {
                m_ParentSetting = value;
                m_ParentSettingId = ((value == null) ? null : new Guid?(value.SettingId));
            }
        }

        /// <summary>
        /// Gets or sets the type of the setting.
        /// </summary>
        public SettingType SettingType
        {
            get { return (SettingType)m_SettingTypeId; }
            set { m_SettingTypeId = (int)value; }
        }

        /// <summary>
        /// Gets the child settings of this setting.
        /// </summary>
        public SettingCollection ChildSettings
        {
            get
            {
                if (m_ChildSettings == null)
                {
                    m_ChildSettings = new SettingCollection();
                    SettingProvider.FillSettings(ref m_ChildSettings, Level, this
                        , SettingProvider.GetChildrenSettingRows(Level, SettingId, null));
                    m_ChildSettings.Sort();
                }
                return m_ChildSettings;
            }
        }

        /// <summary>
        /// Gets a value indicating that the value is default.
        /// </summary>
        public bool ValueIsDefault
        {
            get { return (string.Compare(Value, DefaultValue, StringComparison.CurrentCulture) == 0); }
        }

        /// <summary>
        /// Gets or sets the value indicating the setting should be handled.
        /// </summary>
        public bool Handle { get; set; }

        /// <summary>
        /// Gets or sets the URL to icon of the setting.
        /// </summary>
        public string IconUrl { get; set; }

        public string CustomDescription
        {
            get
            {
                return (this.Handle
                    ? (this.BuiltIn ? Handlers.SettingHandler.Instance.GetDescription(this)
                    : Handlers.SettingHandler.Current.GetDescription(this)) : this.Description);
            }
        }

        public string CustomName
        {
            get
            {
                return (this.Handle
                    ? (this.BuiltIn ? Handlers.SettingHandler.Instance.GetName(this) : Handlers.SettingHandler.Current.GetName(this))
                    : this.Name);
            }
        }

        #endregion

        #region Operators

        public static bool operator ==(Setting setting1, Setting setting2)
        {
            if (object.ReferenceEquals(setting1, setting2))
                return true;

            if (((object)setting1 == null) || ((object)setting2 == null))
                return false;

            return setting1.Equals(setting2);
        }

        public static bool operator !=(Setting setting1, Setting setting2)
        {
            return (!(setting1 == setting2));
        }

        public static bool operator <(Setting setting1, Setting setting2)
        {
            if (setting1 == null)
            {
                return (setting2 != null);
            }
            return (setting1.CompareTo(setting2) < 0);
        }

        public static bool operator >(Setting setting1, Setting setting2)
        {
            if (setting1 == null)
            {
                return false;
            }
            return (setting1.CompareTo(setting2) > 0);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Determines whether the collection for the child settings is initialized. If not, initializes it.
        /// </summary>
        internal void EnsureChildSettings()
        {
            if (m_ChildSettings == null) m_ChildSettings = new SettingCollection();
        }

        #endregion

        #region Overriden Methods

        public override bool Equals(object obj)
        {
            Setting setting = obj as Setting;
            if ((object)setting == null)
                return false;
            return (this.CompareTo(setting) == 0);
        }

        public override int GetHashCode()
        {
            return this.SettingId.GetHashCode();
        }

        /// <summary>
        /// Returns a System.String that represents this object.
        /// </summary>
        /// <returns>A System.String that represents this object.</returns>
        public override string ToString()
        {
            return this.ToString(null);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Compares the current setting with another.
        /// </summary>
        /// <param name="other">A setting to compare with this setting.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the settings being compared.</returns>
        public int CompareTo(Setting other)
        {
            int result = 0;
            if ((object)other != null)
            {
                if (result == 0) result += (this.OrderNumber - other.OrderNumber);
                if (result == 0) result += string.Compare(this.Name, other.Name, StringComparison.CurrentCultureIgnoreCase);
            }
            return result;
        }

        /// <summary>
        /// Returns a System.String that represents this object.
        /// </summary>
        /// <param name="delimiter">The string that delimit the properties/values in the string.</param>
        /// <returns>A System.String that represents this object.</returns>
        public string ToString(string delimiter)
        {
            if (string.IsNullOrEmpty(delimiter)) delimiter = Environment.NewLine;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SettingId={0}{1}", SettingId, delimiter);
            sb.AppendFormat("SettingType={0}{1}", SettingType, delimiter);
            sb.AppendFormat("Name={0}{1}", Name, delimiter);
            sb.AppendFormat("ShortName={0}{1}", ShortName, delimiter);
            sb.AppendFormat("DefaultValue={0}{1}", DefaultValue, delimiter);
            sb.AppendFormat("Value={0}", Value);
            return sb.ToString();
        }

        #endregion
    }

    /// <summary>
    /// The collection of the settings.
    /// </summary>
    [Serializable]
    public sealed class SettingCollection : List<Setting>
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the setting with the specified unique identifier.
        /// </summary>
        /// <param name="settingId">The unique identifier of the setting.</param>
        /// <returns>The setting with the specified unique identifier, or null reference if not found.</returns>
        public Setting this[Guid settingId]
        {
            get { return FindBySettingId(settingId); }
            set
            {
                int index = this.FindIndex(
                    delegate(Setting setting)
                    {
                        return setting.SettingId == settingId;
                    });

                if (index > -1)
                    base[index] = value;
                else
                    base.Add(value);
            }
        }

        /// <summary>
        /// Gets or sets the setting with the specified short name.
        /// </summary>
        /// <param name="shortName">The short namme of the setting.</param>
        /// <returns>The setting with the specified short name, or null reference if not found.</returns>
        public Setting this[string shortName]
        {
            get { return FindByShortName(shortName); }
            set
            {
                int index = this.FindIndex(
                    delegate(Setting setting)
                    {
                        return (string.Compare(setting.ShortName, shortName, StringComparison.Ordinal) == 0);
                    });

                if (index > -1)
                    base[index] = value;
                else
                    base.Add(value);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs before an update values operation.
        /// </summary>
        public static event EventHandler<SettingCollectionUpdateValuesEventArgs> ValuesUpdating;

        /// <summary>
        /// Occurs when an update values operation has completed.
        /// </summary>
        public static event EventHandler<SettingCollectionUpdateValuesEventArgs> ValuesUpdated;

        #endregion

        #region Private Methods

        private void UpdateValues(Guid organizationId, Guid? instanceId, Guid? groupId)
        {
            if (ValuesUpdating != null)
                ValuesUpdating(this, new SettingCollectionUpdateValuesEventArgs(organizationId, instanceId, groupId));

            SettingProvider.UpdateSettingsValues(this, organizationId, instanceId, groupId);

            if (ValuesUpdated != null)
                ValuesUpdated(this, new SettingCollectionUpdateValuesEventArgs(organizationId, instanceId, groupId));
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Changes the parent of the settings in the collection.
        /// </summary>
        /// <param name="setting">The setting that will be set as parent for the settings in the collection.</param>
        internal void ChangeParent(Setting setting)
        {
            foreach (Setting s in this)
            {
                s.ParentSetting = setting;
            }
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Returns a System.String that represents this object.
        /// </summary>
        /// <returns>A System.String that represents this object.</returns>
        public override string ToString()
        {
            return this.ToString(null);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Searches for a first setting that matches the specified identifier.
        /// </summary>
        /// <param name="settingId">The identifier of the setting to search for.</param>
        /// <returns>The first setting that matches the specified identifier, if found; otherwise, the null reference.</returns>
        public Setting FindBySettingId(Guid settingId)
        {
            return this.Find(
                delegate(Setting setting)
                {
                    return setting.SettingId == settingId;
                });
        }

        /// <summary>
        /// Searches for a first setting that matches the specified short name.
        /// </summary>
        /// <param name="shortName">The short name of the element to search for.</param>
        /// <returns>The first setting that matches the specified short name, if found; otherwise, the null reference.</returns>
        public Setting FindByShortName(string shortName)
        {
            return this.Find(
               delegate(Setting setting)
               {
                   return (string.Compare(setting.ShortName, shortName, StringComparison.Ordinal) == 0);
               });
        }

        /// <summary>
        /// Searches for a setting the value of which matches the specified value.
        /// </summary>
        /// <param name="value">The value of the setting to search for.</param>
        /// <returns>The first setting the value of which matches the specified value, if found; otherwise, the null reference.</returns>
        public Setting FindByValue(string value)
        {
            return this.Find(
               delegate(Setting setting)
               {
                   return (string.Compare(setting.Value, value, StringComparison.Ordinal) == 0);
               });
        }

        /// <summary>
        /// Retrieves the child settings of the specified setting.
        /// </summary>
        /// <param name="settingId">The identifier of the setting to get child of.</param>
        /// <returns>The collection of child settings of the specified setting.</returns>
        public SettingCollection FindChildSettings(Guid? settingId)
        {
            SettingCollection coll = new SettingCollection();

            coll.AddRange(this.FindAll(
               delegate(Setting setting)
               {
                   return (setting.ParentSettingId == settingId);
               }));

            return coll;
        }

        /// <summary>
        /// Retrieves the all settings with the specified levels.
        /// </summary>
        /// <param name="level">The levels of the settings.</param>
        /// <returns>The collection of the settings.</returns>
        public SettingCollection FindAllByLevel(SettingLevels level)
        {
            SettingCollection coll = new SettingCollection();

            coll.AddRange(this.FindAll(
               delegate(Setting setting)
               {
                   return (((setting.Level & level) == level) || ((setting.Level & level) == setting.Level));
               }));

            return coll;
        }

        /// <summary>
        /// Retrieves the all settings with the specified levels.
        /// </summary>
        /// <param name="visible">The visible value.</param>
        /// <returns>The collection of the settings.</returns>
        public SettingCollection FindAllByVisible(bool visible)
        {
            SettingCollection coll = new SettingCollection();

            coll.AddRange(this.FindAll(
               delegate(Setting setting)
               {
                   return (setting.Visible == visible);
               }));

            return coll;
        }

        /// <summary>
        /// Returns a System.String that represents this object.
        /// </summary>
        /// <param name="delimiter">The string that delimit the properties/values in the string.</param>
        /// <returns>A System.String that represents this object.</returns>
        public string ToString(string delimiter)
        {
            if (string.IsNullOrEmpty(delimiter)) delimiter = Environment.NewLine;

            StringBuilder sb = new StringBuilder();
            foreach (Setting s in this)
            {
                sb.AppendFormat("{0}[{1}]", delimiter, s.ToString(delimiter));
            }
            if (sb.Length > 0) sb.Remove(0, delimiter.Length);
            return sb.ToString();
        }

        /// <summary>
        /// Updates the organization level settings for the specified organization.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization to update the settings for.</param>
        public void UpdateValues(Guid organizationId)
        {
            UpdateValues(organizationId, null, null);
        }

        /// <summary>
        /// Updates the instance level settings for the specified organization's instance.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization which the instance belong to.</param>
        /// <param name="instanceId">The identifier of the instance to update the settings for.</param>
        public void UpdateValues(Guid organizationId, Guid instanceId)
        {
            UpdateValues(organizationId, instanceId, null);
        }

        /// <summary>
        /// Updates the group level settings for the specified organization's group in specified instance.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization which the group belong to.</param>
        /// <param name="instanceId">The identifier of the instance.</param>
        /// <param name="groupId">The identifier of the group to update the settings for.</param>
        public void UpdateValues(Guid organizationId, Guid instanceId, Guid groupId)
        {
            UpdateValues(organizationId, new Guid?(instanceId), new Guid?(groupId));
        }

        #endregion
    }

    public class SettingCollectionUpdateValuesEventArgs : EventArgs
    {
        #region Members

        private Guid m_OrganizationId;
        private Guid? m_InstanceId;
        private Guid? m_GroupId;

        #endregion

        #region Constructors

        public SettingCollectionUpdateValuesEventArgs() { }

        public SettingCollectionUpdateValuesEventArgs(Guid organizationId)
        {
            m_OrganizationId = organizationId;
        }

        public SettingCollectionUpdateValuesEventArgs(Guid organizationId, Guid instanceId)
        {
            m_OrganizationId = organizationId;
            m_InstanceId = instanceId;
        }

        public SettingCollectionUpdateValuesEventArgs(Guid organizationId, Guid instanceId, Guid groupId)
        {
            m_OrganizationId = organizationId;
            m_InstanceId = instanceId;
            m_GroupId = groupId;
        }

        public SettingCollectionUpdateValuesEventArgs(Guid organizationId, Guid? instanceId, Guid? groupId)
        {
            m_OrganizationId = organizationId;
            m_InstanceId = instanceId;
            m_GroupId = groupId;
        }

        #endregion

        #region Public Properties

        public Guid OrganizationId
        {
            get { return m_OrganizationId; }
            set { m_OrganizationId = value; }
        }

        public Guid? InstanceId
        {
            get { return m_InstanceId; }
            set { m_InstanceId = value; }
        }

        public Guid? GroupId
        {
            get { return m_GroupId; }
            set { m_GroupId = value; }
        }

        #endregion
    }
}
