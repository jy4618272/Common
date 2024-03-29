using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Xml.XPath;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings for ASP.NET application.
    /// </summary>
    public class WebApplicationElement : BaseConfigurationElement
    {
        #region Members

        internal const string MasterConnectionStringName = "Micajah.Common.Properties.Settings.MasterConnectionString";

        private string m_ApplicationUrl;
        private object m_AuthenticationMode;
        private string m_ConnectionString;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WebApplicationElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public WebApplicationElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets the login names of the administrators of the framework.
        /// </summary>
        [ConfigurationProperty("frameworkAdministrators")]
        [TypeConverter(typeof(CommaDelimitedStringCollectionConverter))]
        internal StringCollection FrameworkAdministrators
        {
            get { return (StringCollection)this["frameworkAdministrators"]; }
        }

        /// <summary>
        /// Gets the login names of the users who have the access to the Log On As Another User feature.
        /// </summary>
        [ConfigurationProperty("canLogOnAsAnotherUser")]
        [TypeConverter(typeof(CommaDelimitedStringCollectionConverter))]
        internal StringCollection CanLogOnAsAnotherUser
        {
            get { return (StringCollection)this["canLogOnAsAnotherUser"]; }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the value indicating whether the non-default port should be added to the URL of the application.
        /// </summary>
        [ConfigurationProperty("addPort", DefaultValue = true)]
        public bool AddPort
        {
            get { return (bool)this["addPort"]; }
        }

        /// <summary>
        /// Gets or sets the name of the application project.
        /// </summary>
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the application logo's image.
        /// </summary>
        [ConfigurationProperty("logoImageUrl")]
        public string LogoImageUrl
        {
            get { return (string)this["logoImageUrl"]; }
            set { this["logoImageUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the big application logo's image.
        /// </summary>
        [ConfigurationProperty("bigLogoImageUrl")]
        public string BigLogoImageUrl
        {
            get { return (string)this["bigLogoImageUrl"]; }
            set { this["bigLogoImageUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the mobile application logo's image.
        /// </summary>
        [ConfigurationProperty("mobileLogoImageUrl")]
        public string MobileLogoImageUrl
        {
            get { return (string)this["mobileLogoImageUrl"]; }
            set { this["mobileLogoImageUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the small application logo's image.
        /// </summary>
        [ConfigurationProperty("smallLogoImageUrl")]
        public string SmallLogoImageUrl
        {
            get { return (string)this["smallLogoImageUrl"]; }
            set { this["smallLogoImageUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the height of the big application logo's image.
        /// </summary>
        [ConfigurationProperty("bigLogoImageHeight", DefaultValue = 100)]
        public int BigLogoImageHeight
        {
            get { return (int)this["bigLogoImageHeight"]; }
            set { this["bigLogoImageHeight"] = value; }
        }

        /// <summary>
        /// Gets or sets the height of the mobile application logo's image.
        /// </summary>
        [ConfigurationProperty("mobileLogoImageHeight", DefaultValue = 100)]
        public int MobileLogoImageHeight
        {
            get { return (int)this["mobileLogoImageHeight"]; }
            set { this["mobileLogoImageHeight"] = value; }
        }

        /// <summary>
        /// Gets the URL of the application.
        /// </summary>
        public string Url
        {
            get
            {
                if (string.IsNullOrEmpty(m_ApplicationUrl))
                {
                    HttpRequest request = HttpContext.Current.Request;
                    string portString = string.Empty;
                    if (this.AddPort)
                    {
                        int port = request.Url.Port;
                        if ((!request.Url.IsDefaultPort) && (port > -1)) portString = string.Concat(":", port);
                    }
                    m_ApplicationUrl = string.Concat(request.Url.Scheme, Uri.SchemeDelimiter, request.Url.Host, portString, request.ApplicationPath);
                }
                return m_ApplicationUrl;
            }
        }

        /// <summary>
        /// Gets or sets the authentication mode that is used in the Web application.
        /// </summary>
        public AuthenticationMode AuthenticationMode
        {
            get
            {
                if (m_AuthenticationMode == null)
                {
                    AuthenticationSection sect = (ConfigurationManager.GetSection("system.web/authentication") as AuthenticationSection);
                    m_AuthenticationMode = sect.Mode;
                }
                return (AuthenticationMode)m_AuthenticationMode;
            }
            set { m_AuthenticationMode = value; }
        }

        /// <summary>
        /// Gets or sets the connection string of common database.
        /// By default gets the Micajah.Common.ConnectionString key's value of web.config file.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(m_ConnectionString))
                    m_ConnectionString = ConfigurationManager.ConnectionStrings[MasterConnectionStringName].ConnectionString;
                return m_ConnectionString;
            }
            set
            {
                m_ConnectionString = value;
                Micajah.Common.Properties.Settings.Default["MasterConnectionString"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the copyright information.
        /// </summary>
        [ConfigurationProperty("copyright")]
        public CopyrightElement Copyright
        {
            get { return (CopyrightElement)this["copyright"]; }
            set { this["copyright"] = value; }
        }

        /// <summary>
        /// Gets the settings for custom URLs engine.
        /// </summary>
        [ConfigurationProperty("customUrl")]
        public CustomUrlElement CustomUrl
        {
            get
            {
                WebsiteConfiguration website = WebsiteConfiguration.Current;
                if ((website != null) && website.ElementInformation.IsPresent)
                {
                    if (website.CustomUrl.ElementInformation.IsPresent)
                        return website.CustomUrl;
                }
                return (CustomUrlElement)this["customUrl"];
            }
        }

        /// <summary>
        /// Gets the value indicating whether the application supports the multiple instances per organization.
        /// </summary>
        [ConfigurationProperty("enableMultipleInstances", DefaultValue = true)]
        public bool EnableMultipleInstances
        {
            get { return (bool)this["enableMultipleInstances"]; }
            set { this["enableMultipleInstances"] = value; }
        }

        /// <summary>
        /// Gets or sets the settings for the emails.
        /// </summary>
        [ConfigurationProperty("email")]
        public EmailElement Email
        {
            get { return (EmailElement)this["email"]; }
            set { this["email"] = value; }
        }

        /// <summary>
        /// Gets or sets the settings for login process.
        /// </summary>
        [ConfigurationProperty("login")]
        public LoginElement Login
        {
            get { return (LoginElement)this["login"]; }
            set { this["login"] = value; }
        }

        /// <summary>
        /// Gets or sets the settings for the passwords.
        /// </summary>
        [ConfigurationProperty("password")]
        public PasswordElement Password
        {
            get { return (PasswordElement)this["password"]; }
            set { this["password"] = value; }
        }

        /// <summary>
        /// Gets or sets the support information.
        /// </summary>
        [ConfigurationProperty("support")]
        public SupportElement Support
        {
            get { return (SupportElement)this["support"]; }
            set { this["support"] = value; }
        }

        /// <summary>
        /// Gets or sets the settings of the master page.
        /// </summary>
        [ConfigurationProperty("masterPage")]
        public MasterPageElement MasterPage
        {
            get { return (MasterPageElement)this["masterPage"]; }
            set { this["masterPage"] = value; }
        }

        /// <summary>
        /// Gets the integration settings.
        /// </summary>
        [ConfigurationProperty("integration")]
        public IntegrationElement Integration
        {
            get
            {
                IntegrationElement elem = new IntegrationElement();

                WebsiteConfiguration website = WebsiteConfiguration.Current;
                if ((website != null) && website.ElementInformation.IsPresent)
                {
                    IntegrationElement value = (IntegrationElement)this["integration"];
                    elem.Ldap = value.Ldap;
                    elem.Google = value.Google;
                    elem.Bing = value.Bing;

                    if (website.Integration.ElementInformation.IsPresent)
                    {
                        if (website.Integration.Ldap.ElementInformation.IsPresent)
                            elem.Ldap = website.Integration.Ldap;

                        if (website.Integration.Google.ElementInformation.IsPresent)
                            elem.Google = website.Integration.Google;

                        if (website.Integration.Bing.ElementInformation.IsPresent)
                            elem.Bing = website.Integration.Bing;

                        if (website.Integration.Chargify.ElementInformation.IsPresent)
                            elem.Chargify = website.Integration.Chargify;

                        if (website.Integration.Webhook.ElementInformation.IsPresent)
                            elem.Webhook = website.Integration.Webhook;

                    }
                }

                return elem;
            }
        }

        /// <summary>
        /// Gets the viewstate settings.
        /// </summary>
        [ConfigurationProperty("viewState")]
        public ViewStateElement ViewState
        {
            get
            {
                WebsiteConfiguration website = WebsiteConfiguration.Current;
                if ((website != null) && website.ElementInformation.IsPresent)
                {
                    if (website.ViewState.ElementInformation.IsPresent)
                        return website.ViewState;
                }
                return (ViewStateElement)this["viewState"];
            }
        }

        #endregion
    }
}
