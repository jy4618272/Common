using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings for custom URLs engine.
    /// </summary>
    public class CustomUrlElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CustomUrlElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public CustomUrlElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the value indicating whether the application supports the custom URLs.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        /// <summary>
        /// Gets or sets the root domains for partial custom URLs.
        /// </summary>
        [ConfigurationProperty("partialCustomUrlRootAddresses")]
        [TypeConverter(typeof(CommaDelimitedStringCollectionConverter))]
        public StringCollection PartialCustomUrlRootAddresses
        {
            get { return (StringCollection)this["partialCustomUrlRootAddresses"]; }
        }

        public string PartialCustomUrlRootAddressesFirst
        {
            get
            {
                string address = string.Empty;
                if (this.PartialCustomUrlRootAddresses != null && this.PartialCustomUrlRootAddresses.Count > 0)
                    address = this.PartialCustomUrlRootAddresses[0];

                return address;
            }
        }

        /// <summary>
        /// Gets the reserved addresses for partial custom URLs used by internal applications.
        /// </summary>
        [ConfigurationProperty("partialCustomUrlReservedAddresses")]
        [TypeConverter(typeof(CommaDelimitedStringCollectionConverter))]
        public StringCollection PartialCustomUrlReservedAddresses
        {
            get { return (StringCollection)this["partialCustomUrlReservedAddresses"]; }
        }


        /// <summary>
        /// Gets the reserved default address for partial custom URLs used by internal applications.
        /// </summary>
        [ConfigurationProperty("defaultPartialCustomUrl")]
        public string DefaultPartialCustomUrl
        {
            get { return (string)this["defaultPartialCustomUrl"]; }
        }
        
        /// <summary>
        /// Gets or the root domain for authentication ticket.
        /// </summary>
        [ConfigurationProperty("authenticationTicketDomain")]
        public string AuthenticationTicketDomain
        {
            get { return (string)this["authenticationTicketDomain"]; }
        }

        #endregion
    }
}
