using System.Configuration;
using System.Xml.XPath;
using Micajah.Common.Security;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings for the passwords.
    /// </summary>
    public class PasswordElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PasswordElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public PasswordElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the cryptographic algorithm for encrypting password string.
        /// </summary>
        [ConfigurationProperty("format", DefaultValue = CryptoMethod.Sha1)]
        public CryptoMethod Format
        {
            get { return (CryptoMethod)this["format"]; }
            set { this["format"] = value; }
        }

        /// <summary>
        /// Gets the minimum number of special characters that must be present in a valid password.
        /// </summary>
        [ConfigurationProperty("minRequiredNonAlphanumericCharacters")]
        [IntegerValidator()]
        public int MinRequiredNonAlphanumericCharacters
        {
            get { return (int)this["minRequiredNonAlphanumericCharacters"]; }
            set { this["minRequiredNonAlphanumericCharacters"] = value; }
        }

        /// <summary>
        /// Gets or sets the minimum length required for a password.
        /// </summary>
        [ConfigurationProperty("minRequiredPasswordLength", DefaultValue = 5)]
        [IntegerValidator()]
        public int MinRequiredPasswordLength
        {
            get { return (int)this["minRequiredPasswordLength"]; }
            set { this["minRequiredPasswordLength"] = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether the application is configured to allow users to retrieve their passwords.
        /// </summary>
        [ConfigurationProperty("enablePasswordRetrieval", DefaultValue = true)]
        public bool EnablePasswordRetrieval
        {
            get { return (bool)this["enablePasswordRetrieval"]; }
            set { this["enablePasswordRetrieval"] = value; }
        }

        #endregion
    }
}
