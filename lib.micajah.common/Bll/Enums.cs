using System;

namespace Micajah.Common.Bll
{
    /// <summary>
    /// Represents the different types of actions.
    /// </summary>
    [Serializable]
    public enum ActionType
    {
        /// <summary>
        /// The type is not set.
        /// </summary>
        NotSet = 0,

        /// <summary>
        /// The page.
        /// </summary>
        Page = 1,

        /// <summary>
        /// The control.
        /// </summary>
        Control = 2,

        /// <summary>
        /// The global navigation link.
        /// </summary>
        GlobalNavigationLink = 3
    }

    [Serializable]
    public enum BillingPlan
    {
        Free = 0,
        Paid = 1,
        Custom = 3
    }

    [Serializable]
    public enum CreditCardStatus
    {
        NotRegistered = 0,
        Registered = 1,
        Expired = 2,
        Declined=3
    }
    /// <summary>
    /// Represents the different types of settings.
    /// </summary>
    [Serializable]
    public enum SettingType
    {
        /// <summary>
        /// The type is not set.
        /// </summary>
        NotSet = 0,

        /// <summary>
        /// The check box.
        /// </summary>
        CheckBox = 1,

        /// <summary>
        /// The single value.
        /// </summary>
        Value = 2,

        /// <summary>
        /// The values list.
        /// </summary>
        List = 3,

        /// <summary>
        /// On/Off switch.
        /// </summary>
        OnOffSwitch = 4
    }

    /// <summary>
    /// Represents the different levels of setting.
    /// </summary>
    [Flags]
    [Serializable]
    public enum SettingLevels
    {
        /// <summary>
        /// A level is not set.
        /// </summary>
        None = 0,

        /// <summary>
        /// A global (application) level setting.
        /// </summary>
        Global = 1,

        /// <summary>
        /// An organization level setting.
        /// </summary>
        Organization = 2,

        /// <summary>
        /// A instance level setting.
        /// </summary>
        Instance = 4,

        /// <summary>
        /// A group level setting.
        /// </summary>
        Group = 16
    }

    /// <summary>
    /// Represents the different levels of entity.
    /// </summary>
    [Serializable]
    public enum EntityLevel
    {
        None = 0,
        Organization = 1,
        Instance = 2
    }

    /// <summary>
    /// Represents the different levels of entity.
    /// </summary>
    [Serializable]
    public enum RelationType
    {
        NotSet = 0,
        Checked = 1,
        CheckedAndAllChildren = 2,
        Blocked = 3,
        Unchecked = 4
    }

    /// <summary>
    /// Represents the different types of entity.
    /// </summary>
    [Serializable]
    public enum EntityType
    {
        Default = 0,
        Hierarchical = 1
    }

    /// <summary>
    /// Represents the different types of entity field.
    /// </summary>
    [Serializable]
    public enum EntityFieldType
    {
        NotSet = 0,
        Value = 1,
        SingleSelectList = 2,
        MultipleSelectList = 3,
        MappedHierarchyEntity = 4
    }

    /// <summary>
    /// Represents the different data types of entity field.
    /// </summary>
    [Serializable]
    public enum EntityFieldDataType
    {
        NotSet = 0,
        Text = 1,
        YesNo = 2,
        DateTime = 3,
        Numeric = 4
    }

    /// <summary>
    /// Represents the different trim sides.
    /// </summary>
    [Serializable]
    public enum TrimSide
    {
        Center,
        Left
    }

    /// <summary>
    /// Represents the different types of thread states.
    /// </summary>
    [Serializable]
    public enum ThreadStateType
    {
        /// <summary>
        /// The thread is running.
        /// </summary>
        Running = 0,

        /// <summary>
        /// The is finished correctly.
        /// </summary>
        Finished = 1,

        /// <summary>
        /// The thread is failed.
        /// </summary>
        Failed = 2
    }
}
