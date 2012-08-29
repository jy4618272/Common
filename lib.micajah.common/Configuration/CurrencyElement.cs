using System;
using System.Configuration;
using System.Xml.XPath;
using System.Collections.Generic;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The currency.
    /// </summary>
    public class CurrencyElement : BaseConfigurationElement, IConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CurrencyElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public CurrencyElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the 3-letters ISO code of the currency.
        /// </summary>
        [ConfigurationProperty("code", IsRequired = true)]
        public string Code
        {
            get { return (string)this["code"]; }
            set { this["code"] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the currency.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets the key of the object.
        /// </summary>
        public object Key
        {
            get { return this.Code; }
        }

        #endregion
    }

    /// <summary>
    /// The collection of the currencies.
    /// </summary>
    [ConfigurationCollection(typeof(CurrencyElement), AddItemName = "currency")]
    public class CurrencyElementCollection : BaseConfigurationElementCollection<CurrencyElement>
    {
    }
}
