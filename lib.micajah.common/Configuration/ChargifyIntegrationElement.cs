using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings of the Chargify integration.
    /// </summary>
    public class ChargifyIntegrationElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ChargifyIntegrationElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public ChargifyIntegrationElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the value indicating whether the Chargify integration is enabled.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        /// <summary>
        /// Gets or sets the Connection String Name to Save Settings Values Update History 
        /// </summary>
        [ConfigurationProperty("historyConnectionStringName")]
        public string HistoryConnectionStringName
        {
            get { return (string)this["historyConnectionStringName"]; }
            set { this["historyConnectionStringName"] = value; }
        }

        #endregion
    }
}
