using System;
using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityNodeTypeElement : BaseConfigurationElement, IConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EntityNodeTypeElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public EntityNodeTypeElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the unique identifier of the node type.
        /// </summary>
        [ConfigurationProperty("id", IsRequired = true)]
        public Guid Id
        {
            get { return (Guid)this["id"]; }
            set { this["id"] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the node type.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the order number of the node type.
        /// </summary>
        [ConfigurationProperty("orderNumber")]
        [IntegerValidator()]
        public int OrderNumber
        {
            get { return (int)this["orderNumber"]; }
            set { this["orderNumber"] = value; }
        }

        /// <summary>
        /// Gets or sets the maximal restrict of the node type.
        /// </summary>
        [ConfigurationProperty("maxRestrict")]
        [IntegerValidator()]
        public int MaxRestrict
        {
            get { return (int)this["maxRestrict"]; }
            set { this["maxRestrict"] = value; }
        }

        /// <summary>
        /// Gets the key of the object.
        /// </summary>
        public object Key
        {
            get { return this.Id; }
        }

        #endregion
    }

    /// <summary>
    /// The collection of the node types.
    /// </summary>
    [ConfigurationCollection(typeof(EntityNodeTypeElement), AddItemName = "nodeType")]
    public class EntityNodeTypeElementCollection : BaseConfigurationElementCollection<EntityNodeTypeElement>
    {
    }
}
