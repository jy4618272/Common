using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The output for the rules engine.
    /// </summary>
    public class RulesEngineOutputElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RulesEngineOutputElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public RulesEngineOutputElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the URL of the page to edit output parameters of the rules engine.
        /// </summary>
        [ConfigurationProperty("editPageUrl", IsRequired = true)]
        public string EditPageUrl
        {
            get { return (string)this["editPageUrl"]; }
            set { this["editPageUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the full name of the custom class to manage the outputs of the rules engine.
        /// </summary>
        [ConfigurationProperty("classFullName")]
        public string ClassFullName
        {
            get { return (string)this["classFullName"]; }
            set { this["classFullName"] = value; }
        }

        #endregion
    }
}
