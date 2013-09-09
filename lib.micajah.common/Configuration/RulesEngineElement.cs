using System;
using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The rules engine.
    /// </summary>
    public class RulesEngineElement : BaseConfigurationElement, IConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RulesEngineElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public RulesEngineElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the unique identifier of the rules engine.
        /// </summary>
        [ConfigurationProperty("id", IsRequired = true)]
        public Guid Id
        {
            get { return (Guid)this["id"]; }
            set { this["id"] = value; }
        }

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
        /// Gets or sets the display name of the rules engine.
        /// </summary>
        [ConfigurationProperty("displayName")]
        public string DisplayName
        {
            get { return (string)this["displayName"]; }
            set { this["displayName"] = value; }
        }

        /// <summary>
        /// Gets the inputs for the rules engine.
        /// </summary>
        [ConfigurationProperty("inputs", IsDefaultCollection = true)]
        public RulesEngineInputElementCollection Inputs
        {
            get { return (RulesEngineInputElementCollection)this["inputs"]; }
        }

        /// <summary>
        /// Gets or sets the output of the rules engine.
        /// </summary>
        [ConfigurationProperty("output", IsRequired = true)]
        public RulesEngineOutputElement Output
        {
            get { return (RulesEngineOutputElement)this["output"]; }
            set { this["output"] = value; }
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
    /// The collection of the rules engine.
    /// </summary>
    [ConfigurationCollection(typeof(RulesEngineElement), AddItemName = "rulesEngine")]
    public class RulesEngineElementCollection : BaseConfigurationElementCollection<RulesEngineElement>
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the vale whether the rules engine is enabled in the application.
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
