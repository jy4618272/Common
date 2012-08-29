using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings of the help link and window.
    /// </summary>
    public class HelpLinkElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HelpLinkElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public HelpLinkElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that indicates whether the help link is visible and rendered.
        /// </summary>
        [ConfigurationProperty("visible")]
        public bool Visible
        {
            get { return (bool)this["visible"]; }
            set { this["visible"] = value; }
        }

        /// <summary>
        /// Gets or sets the help link's URL format.
        /// </summary>
        [ConfigurationProperty("urlFormat")]
        public string UrlFormat
        {
            get { return (string)this["urlFormat"]; }
            set { this["urlFormat"] = value; }
        }

        /// <summary>
        /// The width, in pixels, of the popup window with help.
        /// </summary>
        [ConfigurationProperty("windowWidth")]
        [IntegerValidator()]
        public int WindowWidth
        {
            get { return (int)this["windowWidth"]; }
            set { this["windowWidth"] = value; }
        }

        /// <summary>
        /// The height, in pixels, of the popup window with help.
        /// </summary>
        [ConfigurationProperty("windowHeight")]
        [IntegerValidator()]
        public int WindowHeight
        {
            get { return (int)this["windowHeight"]; }
            set { this["windowHeight"] = value; }
        }

        #endregion
    }
}
