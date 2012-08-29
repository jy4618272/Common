using System.Configuration;
using System.Xml.XPath;
using Micajah.Common.Bll;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings for the hierarchical entity.
    /// </summary>
    public class EntityHierarchyElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EntityHierarchyElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public EntityHierarchyElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the navigate URL.
        /// </summary>
        [ConfigurationProperty("customNavigateUrl")]
        public string CustomNavigateUrl
        {
            get { return (string)this["customNavigateUrl"]; }
            set { this["customNavigateUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the custom root node's text.
        /// </summary>
        [ConfigurationProperty("customRootNodeText")]
        public string CustomRootNodeText
        {
            get { return (string)this["customRootNodeText"]; }
            set { this["customRootNodeText"] = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether the entity is hierarchical.
        /// </summary>
        [ConfigurationProperty("enabled")]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether the node types customization is enabled.
        /// </summary>
        [ConfigurationProperty("enableNodeTypesCustomization")]
        public bool EnableNodeTypesCustomization
        {
            get { return (bool)this["enableNodeTypesCustomization"]; }
            set { this["enableNodeTypesCustomization"] = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether the root node selection is enabled.
        /// </summary>
        [ConfigurationProperty("enableRootNodeSelection")]
        public bool EnableRootNodeSelection
        {
            get { return (bool)this["enableRootNodeSelection"]; }
            set { this["enableRootNodeSelection"] = value; }
        }

        /// <summary>
        /// Gets or sets the maximal hierarchy depth.
        /// </summary>
        [ConfigurationProperty("maxDepth")]
        [IntegerValidator()]
        public int MaxDepth
        {
            get { return (int)this["maxDepth"]; }
            set { this["maxDepth"] = value; }
        }

        /// <summary>
        /// Gets or sets the hierarchy start level.
        /// </summary>
        [ConfigurationProperty("startLevel")]
        public EntityLevel StartLevel
        {
            get { return (EntityLevel)this["startLevel"]; }
            set { this["startLevel"] = value; }
        }

        /// <summary>
        /// Gets the node types.
        /// </summary>
        [ConfigurationProperty("nodeTypes", IsDefaultCollection = true)]
        public EntityNodeTypeElementCollection NodeTypes
        {
            get { return (EntityNodeTypeElementCollection)this["nodeTypes"]; }
        }

        #endregion
    }
}
