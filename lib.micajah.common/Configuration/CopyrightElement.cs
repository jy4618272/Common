using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The copyright information.
    /// </summary>
    public class CopyrightElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CopyrightElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public CopyrightElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the copyright holder company name displaying in the page's title.
        /// </summary>
        [ConfigurationProperty("companyName")]
        public string CompanyName
        {
            get { return (string)this["companyName"]; }
            set { this["companyName"] = value; }
        }

        /// <summary>
        /// Gets the URL of the copyright holder company logo image.
        /// </summary>
        [ConfigurationProperty("companyLogoImageUrl")]
        public string CompanyLogoImageUrl
        {
            get { return (string)this["companyLogoImageUrl"]; }
            set { this["companyLogoImageUrl"] = value; }
        }

        /// <summary>
        /// Gets the web site URL of the copyright holder company.
        /// </summary>
        [ConfigurationProperty("companyWebsiteUrl")]
        public string CompanyWebsiteUrl
        {
            get { return (string)this["companyWebsiteUrl"]; }
            set { this["companyWebsiteUrl"] = value; }
        }

        #endregion
    }
}
