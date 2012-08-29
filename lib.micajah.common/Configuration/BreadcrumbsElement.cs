using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings of the breadcrumbs.
    /// </summary>
    public class BreadcrumbsElement : BaseConfigurationElement
    {
        #region Members

        private string m_CenterHtml;
        private string m_RightHtml;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BreadcrumbsElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public BreadcrumbsElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that indicates whether the breadcrumbs is visible and rendered.
        /// </summary>
        [ConfigurationProperty("visible")]
        public bool Visible
        {
            get { return (bool)this["visible"]; }
            set { this["visible"] = value; }
        }

        /// <summary>
        /// Gets or sets the HTML to be shown at the center side of the breadcrumbs area.
        /// </summary>
        public string CenterHtml
        {
            get { return m_CenterHtml; }
            set { m_CenterHtml = value; }
        }

        /// <summary>
        /// Gets or sets the HTML to be shown at the right side of the breadcrumbs area.
        /// </summary>
        public string RightHtml
        {
            get { return m_RightHtml; }
            set { m_RightHtml = value; }
        }

        #endregion
    }
}
