using System.Configuration;
using System.Xml.XPath;
using Micajah.Common.WebControls;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings for validation.
    /// </summary>
    public class ValidationElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ValidationElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public ValidationElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the data type of values.
        /// </summary>
        [ConfigurationProperty("type")]
        public string Type
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        /// <summary>
        /// Gets or sets the regular expression that determines the pattern used to validate the value.
        /// </summary>
        [ConfigurationProperty("expression")]
        public string Expression
        {
            get { return (string)this["expression"]; }
            set { this["expression"] = value; }
        }

        /// <summary>
        /// Gets or sets the maximum value for the validation range.
        /// </summary>
        [ConfigurationProperty("maximumValue")]
        public string MaximumValue
        {
            get { return (string)this["maximumValue"]; }
            set { this["maximumValue"] = value; }
        }

        /// <summary>
        /// Gets or sets the minimum value for the validation range.
        /// </summary>
        [ConfigurationProperty("minimumValue")]
        public string MinimumValue
        {
            get { return (string)this["minimumValue"]; }
            set { this["minimumValue"] = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters allowed for the value.
        /// </summary>
        [ConfigurationProperty("maxLength")]
        public int MaxLength
        {
            get { return (int)this["maxLength"]; }
            set { this["maxLength"] = value; }
        }

        #endregion
    }
}
