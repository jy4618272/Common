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
        [ConfigurationProperty("openIdProviderEndpointAddress")]
        public string OpenIdProviderEndpointAddress
        {
            get
            {
                string value = (string)this["openIdProviderEndpointAddress"];
                return (string.IsNullOrEmpty(value) ? "https://www.google.com/accounts/o8/id" : value);
            }
            set { this["openIdProviderEndpointAddress"] = value; }
        }

        /// <summary>
        /// Gets or sets the application name.
        /// </summary>
        [ConfigurationProperty("applicationName")]
        public string ApplicationName
        {
            get { return (string)this["applicationName"]; }
            set { this["applicationName"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the application listing in Google Apps Marketplace.
        /// </summary>
        [ConfigurationProperty("applicationListingUrl")]
        public string ApplicationListingUrl
        {
            get { return (string)this["applicationListingUrl"]; }
            set { this["applicationListingUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the client ID.
        /// </summary>
        [ConfigurationProperty("clientId")]
        public string ClientId
        {
            get { return (string)this["clientId"]; }
            set { this["clientId"] = value; }
        }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        [ConfigurationProperty("clientSecret")]
        public string ClientSecret
        {
            get { return (string)this["clientSecret"]; }
            set { this["clientSecret"] = value; }
        }

        /// <summary>
        /// Gets or sets the code for Google AdWords conversion tracking.
        /// </summary>
        [ConfigurationProperty("conversionCode")]
        public TextConfigurationElement<string> ConversionCode
        {
            get { return (TextConfigurationElement<string>)this["conversionCode"]; }
        }

        /// <summary>
        /// Gets or sets the code for Google Analytics.
        /// </summary>
        [ConfigurationProperty("analyticsCode")]
        public TextConfigurationElement<string> AnalyticsCode
        {
            get { return (TextConfigurationElement<string>)this["analyticsCode"]; }
        }

        #endregion
    }
}
