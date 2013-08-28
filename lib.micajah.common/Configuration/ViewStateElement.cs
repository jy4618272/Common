using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The viewstate settings.
    /// </summary>
    public class ViewStateElement : BaseConfigurationElement
    {
        #region Members

        private string m_ConnectionString;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ViewStateElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public ViewStateElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the expiration time-out for viewstate in minutes. It's 8 days by default.
        /// </summary>
        [ConfigurationProperty("expirationTimeout", DefaultValue = 11520)]
        [IntegerValidator()]
        public int ExpirationTimeout
        {
            get { return (int)this["expirationTimeout"]; }
            set { this["expirationTimeout"] = value; }
        }

        /// <summary>
        /// Gets or sets the connection string name to viewstate database.
        /// </summary>
        [ConfigurationProperty("connectionStringName", DefaultValue = WebApplicationElement.MasterConnectionStringName)]
        public string ConnectionStringName
        {
            get { return (string)this["connectionStringName"]; }
            set { this["connectionStringName"] = value; }
        }

        /// <summary>
        /// Gets or sets the connection string to viewstate database.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(m_ConnectionString))
                    m_ConnectionString = ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString;
                return m_ConnectionString;
            }
            set { m_ConnectionString = value; }
        }

        #endregion
    }
}
