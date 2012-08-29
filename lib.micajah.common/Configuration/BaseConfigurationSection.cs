using System.Configuration;
using System.Xml;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// Represents a section within a configuration file.
    /// </summary>
    public class BaseConfigurationSection : ConfigurationSection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BaseConfigurationSection() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public BaseConfigurationSection(IXPathNavigable node)
            : base()
        {
            BaseConfigurationElement.Initialize(node as XmlNode, this);
        }

        #endregion
    }
}
