using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings of the left area.
    /// </summary>
    public class LeftAreaElement : BaseConfigurationElement
    {
        #region Members

        private string m_Html;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LeftAreaElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public LeftAreaElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that indicates whether the left area is visible and rendered.
        /// </summary>
        [ConfigurationProperty("visible")]
        public bool Visible
        {
            get { return (bool)this["visible"]; }
            set { this["visible"] = value; }
        }

        /// <summary>
        /// Gets or sets the width, in pixels, of the left area.
        /// </summary>
        [ConfigurationProperty("width", DefaultValue = 175)]
        [IntegerValidator()]
        public int Width
        {
            get { return (int)this["width"]; }
            set { this["width"] = value; }
        }

        /// <summary>
        /// Gets or sets the HTML to be shown at the left area below of the submenu.
        /// </summary>
        public string Html
        {
            get { return m_Html; }
            set { m_Html = value; }
        }

        #endregion
    }
}
