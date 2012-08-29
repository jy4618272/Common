using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The security settings.
    /// </summary>
    public class SecurityElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SecurityElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public SecurityElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the private key.
        /// </summary>
        [ConfigurationProperty("privateKey")]
        public string PrivateKey
        {
            get { return (string)this["privateKey"]; }
            set { this["privateKey"] = value; }
        }

        #endregion
    }
}
