using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings of the Google integration.
    /// </summary>
    public class GoogleIntegrationElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public GoogleIntegrationElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public GoogleIntegrationElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the value indicating whether the Google integration is enabled.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        /// <summary>
        /// Gets or sets the value endpoint address of Google OpenID Provider.
        /// </summary>
        [ConfigurationProperty("openIdProviderEndpointAddress", IsRequired = true)]
        public string OpenIdProviderEndpointAddress
        {
            get { return (string)this["openIdProviderEndpointAddress"]; }
            set { this["openIdProviderEndpointAddress"] = value; }
        }

        #endregion
    }
}
