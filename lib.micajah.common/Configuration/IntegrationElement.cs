using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The integration settings.
    /// </summary>
    public class IntegrationElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public IntegrationElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public IntegrationElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the settings of the LDAP integration.
        /// </summary>
        [ConfigurationProperty("ldap")]
        public LdapIntegrationElement Ldap
        {
            get { return (LdapIntegrationElement)this["ldap"]; }
            set { this["ldap"] = value; }
        }

        /// <summary>
        /// Gets or sets the settings of the Google integration.
        /// </summary>
        [ConfigurationProperty("google")]
        public GoogleIntegrationElement Google
        {
            get { return (GoogleIntegrationElement)this["google"]; }
            set { this["google"] = value; }
        }

        /// <summary>
        /// Gets or sets the settings of the Bing integration.
        /// </summary>
        [ConfigurationProperty("bing")]
        public BingIntegrationElement Bing
        {
            get { return (BingIntegrationElement)this["bing"]; }
            set { this["bing"] = value; }
        }

        /// <summary>
        /// Gets or sets the settings of the Chargify integration.
        /// </summary>
        [ConfigurationProperty("chargify")]
        public ChargifyIntegrationElement Chargify
        {
            get { return (ChargifyIntegrationElement)this["chargify"]; }
            set { this["chargify"] = value; }
        }

        /// <summary>
        /// Gets or sets the settings of the Webhook integration.
        /// </summary>
        [ConfigurationProperty("webhook")]
        public WebhookIntegrationElement Webhook
        {
            get { return (WebhookIntegrationElement)this["webhook"]; }
            set { this["webhook"] = value; }
        }
        #endregion
    }
}
