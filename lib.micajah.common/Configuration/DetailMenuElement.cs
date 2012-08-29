using System.Configuration;
using System.Xml.XPath;
using Micajah.Common.WebControls;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings of the detail menu.
    /// </summary>
    public class DetailMenuElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DetailMenuElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public DetailMenuElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the icon size of detail menu.
        /// </summary>
        [ConfigurationProperty("iconSize")]
        public IconSize IconSize
        {
            get { return (IconSize)this["iconSize"]; }
            set { this["iconSize"] = value; }
        }

        /// <summary>
        /// Gets or sets the theme of the detail menu.
        /// </summary>
        [ConfigurationProperty("theme")]
        public DetailMenuTheme Theme
        {
            get { return (DetailMenuTheme)this["theme"]; }
            set { this["theme"] = value; }
        }

        #endregion
    }
}
