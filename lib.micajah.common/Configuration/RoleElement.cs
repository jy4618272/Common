using System;
using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The role.
    /// </summary>
    public class RoleElement : BaseConfigurationElement, IConfigurationElement
    {
        #region Members

        private bool m_BuiltIn;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RoleElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public RoleElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the value indicating whether the element is built-in.
        /// </summary>
        internal bool BuiltIn
        {
            get { return m_BuiltIn; }
            set { m_BuiltIn = value; }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the unique identifier of the role.
        /// </summary>
        [ConfigurationProperty("id", IsRequired = true)]
        public Guid Id
        {
            get { return (Guid)this["id"]; }
            set { this["id"] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the description of the role.
        /// </summary>
        [ConfigurationProperty("description")]
        public string Description
        {
            get { return (string)this["description"]; }
            set { this["description"] = value; }
        }

        /// <summary>
        /// Gets or sets the short name of the role.
        /// </summary>
        [ConfigurationProperty("shortName", IsRequired = true)]
        public string ShortName
        {
            get { return (string)this["shortName"]; }
            set { this["shortName"] = value; }
        }

        /// <summary>
        /// Gets or sets the rank of the role.
        /// </summary>
        [ConfigurationProperty("rank", IsRequired = true)]
        [IntegerValidator()]
        public int Rank
        {
            get { return (int)this["rank"]; }
            set { this["rank"] = value; }
        }

        /// <summary>
        /// Gets or sets the unique identifier of the start page for the role.
        /// </summary>
        [ConfigurationProperty("startPageId", IsRequired = true)]
        public Guid StartPageId
        {
            get { return (Guid)this["startPageId"]; }
            set { this["startPageId"] = value; }
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
    /// The collection of the roles.
    /// </summary>
    [ConfigurationCollection(typeof(RoleElement), AddItemName = "role")]
    public class RoleElementCollection : BaseConfigurationElementCollection<RoleElement>
    {
    }
}
