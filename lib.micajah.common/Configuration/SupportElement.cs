using System.Configuration;
using System.Net.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The support information.
    /// </summary>
    public class SupportElement : BaseConfigurationElement
    {
        #region Members

        private string m_Email;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SupportElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public SupportElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the support email address of the application.
        /// </summary>
        public string Email
        {
            get
            {
                if (string.IsNullOrEmpty(m_Email))
                {
                    SmtpSection sect = (ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection);
                    if (sect != null)
                        m_Email = sect.From;
                }
                return m_Email;
            }
            set { m_Email = value; }
        }

        /// <summary>
        /// Gets or sets the phone of the support.
        /// </summary>
        [ConfigurationProperty("phone")]
        public string Phone
        {
            get { return (string)this["phone"]; }
            set { this["phone"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the Terms of Use page.
        /// </summary>
        [ConfigurationProperty("termsUrl")]
        public string TermsUrl
        {
            get { return (string)this["termsUrl"]; }
            set { this["termsUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the Privacy Policy page.
        /// </summary>
        [ConfigurationProperty("privacyPolicyUrl")]
        public string PrivacyPolicyUrl
        {
            get { return (string)this["privacyPolicyUrl"]; }
            set { this["privacyPolicyUrl"] = value; }
        }

        #endregion
    }
}
