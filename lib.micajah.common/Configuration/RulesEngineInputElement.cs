using System;
using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The input for the rules engine.
    /// </summary>
    public class RulesEngineInputElement : BaseConfigurationElement, IConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RulesEngineInputElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public RulesEngineInputElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the rules engine.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the unique identifier of the entity.
        /// </summary>
        [ConfigurationProperty("entityId", IsRequired = true)]
        public Guid EntityId
        {
            get { return (Guid)this["entityId"]; }
            set { this["entityId"] = value; }
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
    /// The collection of the inputs of the rules engine.
    /// </summary>
    [ConfigurationCollection(typeof(RulesEngineInputElement), AddItemName = "input")]
    public class RulesEngineInputElementCollection : BaseConfigurationElementCollection<RulesEngineInputElement>
    {
    }
}
