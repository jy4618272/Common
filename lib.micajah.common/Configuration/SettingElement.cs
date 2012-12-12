using System;
using System.Configuration;
using System.Xml.XPath;
using Micajah.Common.Bll;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The setting.
    /// </summary>
    public class SettingElement : BaseConfigurationElement, IConfigurationElement
    {
        #region Members

        private bool m_Visible = true;
        private bool m_VisibleIsLoaded;
        private bool m_BuiltIn;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SettingElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public SettingElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets or sets a value that indicates whether the setting is visible.
        /// </summary>
        [ConfigurationProperty("visible")]
        [RegexStringValidator("^(\\s*|true|false)$")]
        private string OriginalVisible
        {
            get { return (string)this["visible"]; }
            set { this["visible"] = value; }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the value indicating whether the element is built-in.
        /// </summary>
        internal bool BuiltIn
        {
            get { return m_BuiltIn; }
            set { m_BuiltIn = value; }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the unique identifier of the setting.
        /// </summary>
        [ConfigurationProperty("id", IsRequired = true)]
        public Guid Id
        {
            get { return (Guid)this["id"]; }
            set { this["id"] = value; }
        }

        /// <summary>
        /// Gets or sets the type of the setting.
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true)]
        public SettingType SettingType
        {
            get { return (SettingType)this["type"]; }
            set { this["type"] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the setting.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the description of the setting.
        /// </summary>
        [ConfigurationProperty("description")]
        public string Description
        {
            get { return (string)this["description"]; }
            set { this["description"] = value; }
        }

        /// <summary>
        /// Gets or sets the short name of the setting.
        /// </summary>
        [ConfigurationProperty("shortName", IsRequired = true)]
        public string ShortName
        {
            get { return (string)this["shortName"]; }
            set { this["shortName"] = value; }
        }

        /// <summary>
        /// Gets or sets the order number of the setting.
        /// </summary>
        [ConfigurationProperty("orderNumber")]
        [IntegerValidator()]
        public int OrderNumber
        {
            get { return (int)this["orderNumber"]; }
            set { this["orderNumber"] = value; }
        }

        /// <summary>
        /// Gets or sets the levels of the setting.
        /// </summary>
        [ConfigurationProperty("levels")]
        public SettingLevels Levels
        {
            get { return (SettingLevels)this["levels"]; }
            set { this["levels"] = value; }
        }

        /// <summary>
        /// Gets or sets the default value of the setting.
        /// </summary>
        [ConfigurationProperty("defaultValue")]
        public string DefaultValue
        {
            get { return (string)this["defaultValue"]; }
            set { this["defaultValue"] = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating the setting should be handled.
        /// </summary>
        [ConfigurationProperty("handle", DefaultValue = false)]
        public bool Handle
        {
            get { return (bool)this["handle"]; }
            set { this["handle"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL to icon of the setting.
        /// </summary>
        [ConfigurationProperty("iconUrl")]
        public string IconUrl
        {
            get { return (string)this["iconUrl"]; }
            set { this["iconUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL to the paid upgrade page.
        /// </summary>
        [ConfigurationProperty("paidUpgradeUrl")]
        public string PaidUpgradeUrl
        {
            get { return (string)this["paidUpgradeUrl"]; }
            set { this["paidUpgradeUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the value the setting is paid.
        /// </summary>
        [ConfigurationProperty("paid", DefaultValue = false)]
        public bool Paid
        {
            get { return (bool)this["paid"]; }
            set { this["paid"] = value; }
        }

        /// <summary>
        /// Gets or sets the value the setting is Calculate Usage
        /// </summary>
        [ConfigurationProperty("usageCountLimit", DefaultValue = 0)]
        [IntegerValidator()]
        public int UsageCountLimit
        {
            get { return (int)this["usageCountLimit"]; }
            set { this["usageCountLimit"] = value; }
        }

        /// <summary>
        /// Gets or sets the price for paid setting
        /// </summary>
        [ConfigurationProperty("price", DefaultValue = "0.0")]
        public decimal Price
        {
            get { return (decimal)this["price"]; }
            set { this["price"] = value; }
        }

        /// <summary>
        /// Gets or sets External Id value to bind setting with item in an external system
        /// </summary>
        [ConfigurationProperty("externalId", DefaultValue = "")]
        public string ExternalId
        {
            get { return this["externalId"].ToString(); }
            set { this["externalId"] = value; }
        }

        /// <summary>
        /// Gets or sets the settings for validation of value.
        /// </summary>
        [ConfigurationProperty("validation")]
        public ValidationElement Validation
        {
            get { return (ValidationElement)this["validation"]; }
            set { this["validation"] = value; }
        }

        /// <summary>
        /// Gets the values of the settings.
        /// </summary>
        [ConfigurationProperty("values", IsDefaultCollection = true)]
        public SettingValueElementCollection Values
        {
            get { return (SettingValueElementCollection)this["values"]; }
        }

        /// <summary>
        /// Gets the child settings.
        /// </summary>
        [ConfigurationProperty("settings", IsDefaultCollection = true)]
        public SettingElementCollection Settings
        {
            get { return (SettingElementCollection)this["settings"]; }
        }

        /// <summary>
        /// Gets the key of the object.
        /// </summary>
        public object Key
        {
            get { return this.Id; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the setting is visible.
        /// </summary>
        public bool Visible
        {
            get
            {
                this.EnsureVisibleIsLoaded();
                return m_Visible;
            }
            set
            {
                m_Visible = value;
                this.OriginalVisible = (value ? "true" : "false");
            }
        }

        #endregion

        #region Private Methods

        private void EnsureVisibleIsLoaded()
        {
            if (!m_VisibleIsLoaded)
            {
                bool val = false;
                if (bool.TryParse(this.OriginalVisible, out val))
                    m_Visible = val;

                m_VisibleIsLoaded = true;
            }
        }

        #endregion
    }

    /// <summary>
    /// The collection of the settings.
    /// </summary>
    [ConfigurationCollection(typeof(SettingElement), AddItemName = "setting")]
    public class SettingElementCollection : BaseConfigurationElementCollection<SettingElement>
    {
        #region Internal Methods

        internal void SetBuiltIn(bool builtIn)
        {
            foreach (SettingElement element in this)
            {
                element.BuiltIn = builtIn;
                element.Settings.SetBuiltIn(builtIn);
            }
        }

        #endregion
    }
}
