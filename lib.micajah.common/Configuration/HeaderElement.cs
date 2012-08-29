using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings of the header.
    /// </summary>
    public class HeaderElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HeaderElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public HeaderElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that indicates whether the header is visible and rendered.
        /// </summary>
        [ConfigurationProperty("visible")]
        public bool Visible
        {
            get { return (bool)this["visible"]; }
            set { this["visible"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the header links are visible and rendered.
        /// </summary>
        [ConfigurationProperty("visibleLinks")]
        public bool VisibleLinks
        {
            get { return (bool)this["visibleLinks"]; }
            set { this["visibleLinks"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the logo's image or text is visible and rendered in the header.
        /// </summary>
        [ConfigurationProperty("visibleLogo")]
        public bool VisibleLogo
        {
            get { return (bool)this["visibleLogo"]; }
            set { this["visibleLogo"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the search control are visible and rendered.
        /// </summary>
        [ConfigurationProperty("visibleSearch")]
        public bool VisibleSearch
        {
            get { return (bool)this["visibleSearch"]; }
            set { this["visibleSearch"] = value; }
        }

        #endregion
    }
}
