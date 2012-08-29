using System.Configuration;
using System.Xml.XPath;
using Micajah.Common.WebControls;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings of the submenu.
    /// </summary>
    public class SubmenuElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SubmenuElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public SubmenuElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that indicates whether the submenu is visible and rendered.
        /// </summary>
        [ConfigurationProperty("visible")]
        public bool Visible
        {
            get { return (bool)this["visible"]; }
            set { this["visible"] = value; }
        }

        /// <summary>
        /// Gets or sets the position of the submenu.
        /// </summary>
        [ConfigurationProperty("position")]
        public SubmenuPosition Position
        {
            get { return (SubmenuPosition)this["position"]; }
            set { this["position"] = value; }
        }

        #endregion
    }
}
