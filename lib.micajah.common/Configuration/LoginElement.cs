using System;
using System.Configuration;
using System.Globalization;
using System.Web.Configuration;
using System.Xml.XPath;
using Micajah.Common.Properties;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings for login process.
    /// </summary>
    public class LoginElement : BaseConfigurationElement
    {
        #region Members

        private string m_TitleLabelText;
        private string m_LoginLabelText = Resources.LoginElement_LoginLabelText;
        private string m_PasswordLabelText = Resources.LoginElement_PasswordLabelText;
        private string m_LoginButtonText = Resources.LoginElement_LoginButtonText;
        private string m_FailureText = Resources.LoginElement_FailureText;
        private int m_Timeout;
        private string m_LoginValidationExpression = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LoginElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public LoginElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the text of the title for the login form.
        /// </summary>
        public string TitleLabelText
        {
            get { return m_TitleLabelText; }
            set { m_TitleLabelText = value; }
        }

        /// <summary>
        /// Gets or sets the text of the label for the login text box.
        /// </summary>
        public string LoginLabelText
        {
            get { return m_LoginLabelText; }
            set { m_LoginLabelText = value; }
        }

        /// <summary>
        /// Gets or sets the text of the label for the password text box.
        /// </summary>
        public string PasswordLabelText
        {
            get { return m_PasswordLabelText; }
            set { m_PasswordLabelText = value; }
        }

        /// <summary>
        /// Gets or sets the text for the login button.
        /// </summary>
        public string LoginButtonText
        {
            get { return m_LoginButtonText; }
            set { m_LoginButtonText = value; }
        }

        /// <summary>
        /// Gets or sets the text displayed when a login attempt fails.
        /// </summary>
        public string FailureText
        {
            get { return m_FailureText; }
            set { m_FailureText = value; }
        }

        /// <summary>
        /// Gets or sets the authentication time-out in minutes.
        /// By default gets from authentication section of web.config file.
        /// </summary>
        public int Timeout
        {
            get
            {
                if (m_Timeout <= 0)
                {
                    m_Timeout = 30;
                    AuthenticationSection section = (ConfigurationManager.GetSection("system.web/authentication") as AuthenticationSection);
                    if ((section.Mode == AuthenticationMode.Forms) && (section.Forms != null))
                        m_Timeout = Convert.ToInt32(section.Forms.Timeout.TotalMinutes, CultureInfo.InvariantCulture);
                }
                return m_Timeout;
            }
            set { m_Timeout = value; }
        }

        /// <summary>
        /// Gets or sets the regular expression to validate the login name.
        /// By default it is a e-mail address validation expression.
        /// </summary>
        public string LoginValidationExpression
        {
            get { return m_LoginValidationExpression; }
            set { m_LoginValidationExpression = value; }
        }

        #endregion
    }
}
