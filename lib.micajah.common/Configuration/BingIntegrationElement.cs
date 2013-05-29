using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings of the Bing integration.
    /// </summary>
    public class BingIntegrationElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BingIntegrationElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public BingIntegrationElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the code for Bing Ads.
        /// </summary>
        [ConfigurationProperty("conversionCode")]
        public TextConfigurationElement<string> ConversionCode
        {
            get { return (TextConfigurationElement<string>)this["conversionCode"]; }
        }

        #endregion
    }
}
