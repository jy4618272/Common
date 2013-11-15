using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings of the Google integration.
    /// </summary>
    public class WebhookIntegrationElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WebhookIntegrationElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public WebhookIntegrationElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties


        /// <summary>
        /// Gets or sets the secret.
        /// </summary>
        [ConfigurationProperty("secret")]
        public string Secret
        {
            get { return (string)this["secret"]; }
            set { this["secret"] = value; }
        }

        /// <summary>
        /// Gets or sets the webhook path.
        /// </summary>
        [ConfigurationProperty("path")]
        public string Path
        {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }

        /// <summary>
        /// Handle and process errors on http handler pages and return error information in a response body.
        /// </summary>
        [ConfigurationProperty("handleErrors")]
        public bool HandleErrors
        {
            get { return (bool)this["handleErrors"]; }
            set { this["handleErrors"] = value; }
        }

        #endregion
    }
}
