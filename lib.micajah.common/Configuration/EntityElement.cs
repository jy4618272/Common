using System;
using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The entity.
    /// </summary>
    public class EntityElement : BaseConfigurationElement, IConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EntityElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public EntityElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the unique identifier of the entity.
        /// </summary>
        [ConfigurationProperty("id", IsRequired = true)]
        public Guid Id
        {
            get { return (Guid)this["id"]; }
            set { this["id"] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the table2 where the entity stores.
        /// </summary>
        [ConfigurationProperty("tableName")]
        public string TableName
        {
            get { return (string)this["tableName"]; }
            set { this["tableName"] = value; }
        }

        /// <summary>
        /// Gets or sets the settings for the hierarchy.
        /// </summary>
        [ConfigurationProperty("hierarchy")]
        public EntityHierarchyElement Hierarchy
        {
            get { return (EntityHierarchyElement)this["hierarchy"]; }
            set { this["hierarchy"] = value; }
        }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        [ConfigurationProperty("fields", IsDefaultCollection = true)]
        public EntityFieldElementCollection Fields
        {
            get { return (EntityFieldElementCollection)this["fields"]; }
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        [ConfigurationProperty("events", IsDefaultCollection = true)]
        public EntityEventElementCollection Events
        {
            get { return (EntityEventElementCollection)this["events"]; }
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
    /// The collection of the entities.
    /// </summary>
    [ConfigurationCollection(typeof(EntityElement), AddItemName = "entity")]
    public class EntityElementCollection : BaseConfigurationElementCollection<EntityElement>
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the vale whether the entities are enabled in the application.
        /// </summary>
        [ConfigurationProperty("enabled", DefaultValue = true)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        #endregion
    }
}
