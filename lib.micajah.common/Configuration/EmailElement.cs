using System;
using System.Configuration;
using System.Net.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings for emails.
    /// </summary>
    [Serializable]
    public class EmailElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EmailElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public EmailElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Members

        private string m_SmtpServer;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the SMTP-server.
        /// </summary>
        public string SmtpServer
        {
            get
            {
                if (string.IsNullOrEmpty(m_SmtpServer))
                {
                    SmtpSection sect = (ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection);
                    if (sect != null)
                    {
                        if (sect.Network != null)
                            m_SmtpServer = sect.Network.Host;
                    }
                }
                return m_SmtpServer;
            }
            set { m_SmtpServer = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether the e-mail will be sent when login, email address, password is changed, or the account is added to an organization.
        /// </summary>
        [ConfigurationProperty("enableChangeLoginNotification", DefaultValue = true)]
        public bool EnableChangeLoginNotification
        {
            get { return (bool)this["enableChangeLoginNotification"]; }
            set { this["enableChangeLoginNotification"] = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether the email notification with credentials will be sent to newly created user.
        /// </summary>
        [ConfigurationProperty("enableCreateNewLoginNotification", DefaultValue = true)]
        public bool EnableCreateNewLoginNotification
        {
            get { return (bool)this["enableCreateNewLoginNotification"]; }
            set { this["enableCreateNewLoginNotification"] = value; }
        }

        /// <summary>
        /// Gets or sets the email address of the sales team.
        /// </summary>
        [ConfigurationProperty("salesTeam")]
        public string SalesTeam
        {
            get { return (string)this["salesTeam"]; }
            set { this["salesTeam"] = value; }
        }

        #endregion
    }
}
