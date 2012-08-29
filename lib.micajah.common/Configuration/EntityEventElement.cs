using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The event of the entity.
    /// </summary>
    public class EntityEventElement : BaseConfigurationElement, IConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EntityEventElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public EntityEventElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        [ConfigurationProperty("iconImageUrl")]
        public string IconImageUrl
        {
            get { return (string)this["iconImageUrl"]; }
            set { this["iconImageUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the edit page.
        /// </summary>
        [ConfigurationProperty("editPageUrl")]
        public string EditPageUrl
        {
            get { return (string)this["editPageUrl"]; }
            set { this["editPageUrl"] = value; }
        }

        /// <summary>
        /// Gets the key of the object.
        /// </summary>
        public object Key
        {
            get { return this.Name; }
        }

        #endregion
    }

    /// <summary>
    /// The collection of the events of the entity.
    /// </summary>
    [ConfigurationCollection(typeof(EntityEventElement), AddItemName = "event")]
    public class EntityEventElementCollection : BaseConfigurationElementCollection<EntityEventElement>
    {
    }
}
