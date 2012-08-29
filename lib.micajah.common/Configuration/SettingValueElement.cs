using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The value for the setting.
    /// </summary>
    public class SettingValueElement : BaseConfigurationElement, IConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SettingValueElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public SettingValueElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the setting's value.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the value of the setting.
        /// </summary>
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }

        /// <summary>
        /// Gets the key of the object.
        /// </summary>
        public object Key
        {
            get { return this.Name; }
        }

        #endregion
    }

    /// <summary>
    /// The collection of the setting's values.
    /// </summary>
    [ConfigurationCollection(typeof(SettingValueElement), AddItemName = "value")]
    public class SettingValueElementCollection : BaseConfigurationElementCollection<SettingValueElement>
    {
    }
}
