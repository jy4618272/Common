using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings of the footer.
    /// </summary>
    public class FooterElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FooterElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public FooterElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the URL of the application logo's image that will be displayed in the footer.
        /// </summary>
        [ConfigurationProperty("logoImageUrl")]
        public string LogoImageUrl
        {
            get { return (string)this["logoImageUrl"]; }
            set { this["logoImageUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the footer is visible and rendered.
        /// </summary>
        [ConfigurationProperty("visible", DefaultValue = true)]
        public bool Visible
        {
            get { return (bool)this["visible"]; }
            set { this["visible"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the application logo's image is visible and rendered.
        /// </summary>
        [ConfigurationProperty("visibleApplicationLogo", DefaultValue = true)]
        public bool VisibleApplicationLogo
        {
            get { return (bool)this["visibleApplicationLogo"]; }
            set { this["visibleApplicationLogo"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the engineered by is visible and rendered.
        /// </summary>
        [ConfigurationProperty("visibleEngineeredBy", DefaultValue = true)]
        public bool VisibleEngineeredBy
        {
            get { return (bool)this["visibleEngineeredBy"]; }
            set { this["visibleEngineeredBy"] = value; }
        }

        #endregion
    }
}
