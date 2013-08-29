using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The configuration section for the website specific settings.
    /// </summary>
    public class WebsiteConfiguration : BaseConfigurationSection
    {
        #region Members

        /// <summary>
        /// The name of the section in configuration file.
        /// </summary>
        private const string SectionName = "micajah.common.website";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WebsiteConfiguration() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public WebsiteConfiguration(IXPathNavigable node) : base(node) { }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets the current configuration of the website.
        /// </summary>
        internal static WebsiteConfiguration Current
        {
            get { return ConfigurationManager.GetSection(SectionName) as WebsiteConfiguration; }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the settings for custom URLs engine.
        /// </summary>
        [ConfigurationProperty("customUrl")]
        public CustomUrlElement CustomUrl
        {
            get { return (CustomUrlElement)this["customUrl"]; }
        }

        /// <summary>
        /// Gets the integration settings.
        /// </summary>
        [ConfigurationProperty("integration")]
        public IntegrationElement Integration
        {
            get { return (IntegrationElement)this["integration"]; }
        }

        #endregion
    }
}
