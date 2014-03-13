using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings of the LDAP integration.
    /// </summary>
    public class LdapIntegrationElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LdapIntegrationElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public LdapIntegrationElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the value indicating whether the LDAP integration is enabled.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the article how to setup LDAP.
        /// </summary>
        [ConfigurationProperty("setupUrl")]
        public string SetupUrl
        {
            get { return (string)this["setupUrl"]; }
            set { this["setupUrl"] = value; }
        }

        #endregion
    }
}
