using System.Configuration;
using System.Web.Caching;
using System.Xml;
using System.Xml.XPath;
using Micajah.Common.Application;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The configuration section for the framework.
    /// </summary>
    public class FrameworkConfiguration : BaseConfigurationSection
    {
        #region Members

        /// <summary>
        /// The name of the section in configuration file.
        /// </summary>
        private const string SectionName = "micajah.common";

        // The objects which are used to synchronize access to the cached objects.
        private static object s_CurrentSyncRoot = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrameworkConfiguration() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public FrameworkConfiguration(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the current configuration of the framework.
        /// </summary>
        public static FrameworkConfiguration Current
        {
            get
            {
                FrameworkConfiguration section = CacheManager.Current.Get("mc.FrameworkConfiguration") as FrameworkConfiguration;
                if (section == null)
                {
                    lock (s_CurrentSyncRoot)
                    {
                        section = CacheManager.Current.Get("mc.FrameworkConfiguration") as FrameworkConfiguration;
                        if (section == null)
                        {
                            XmlDocument doc = new XmlDocument();

                            doc.LoadXml(Micajah.Common.Properties.Resources.MicajahCommonConfig);

                            section = new FrameworkConfiguration(doc.SelectSingleNode(SectionName));
                            section.SetBuiltIn(true);
                            section.Merge(ConfigurationManager.GetSection(SectionName) as FrameworkConfiguration);

                            CacheManager.Current.Add("mc.FrameworkConfiguration", section, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                        }
                    }
                }
                return section;
            }
        }

        /// <summary>
        /// Gets the currencies.
        /// </summary>
        [ConfigurationProperty("currencies", IsDefaultCollection = true)]
        public CurrencyElementCollection Currencies
        {
            get { return (CurrencyElementCollection)this["currencies"]; }
        }

        /// <summary>
        /// Gets or sets the settings for ASP.NET application.
        /// </summary>
        [ConfigurationProperty("webApplication")]
        public WebApplicationElement WebApplication
        {
            get { return (WebApplicationElement)this["webApplication"]; }
            set { this["webApplication"] = value; }
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        [ConfigurationProperty("roles", IsDefaultCollection = true)]
        public RoleElementCollection Roles
        {
            get { return (RoleElementCollection)this["roles"]; }
        }

        /// <summary>
        /// Gets the actions.
        /// </summary>
        [ConfigurationProperty("actions", IsDefaultCollection = true)]
        public ActionElementCollection Actions
        {
            get { return (ActionElementCollection)this["actions"]; }
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        [ConfigurationProperty("settings", IsDefaultCollection = true)]
        public SettingElementCollection Settings
        {
            get { return (SettingElementCollection)this["settings"]; }
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        [ConfigurationProperty("entities", IsDefaultCollection = true)]
        public EntityElementCollection Entities
        {
            get { return (EntityElementCollection)this["entities"]; }
        }

        /// <summary>
        /// Gets the rules engines.
        /// </summary>
        [ConfigurationProperty("rulesEngines", IsDefaultCollection = true)]
        public RulesEngineElementCollection RulesEngines
        {
            get { return (RulesEngineElementCollection)this["rulesEngines"]; }
        }

        /// <summary>
        /// Gets the threads, which are started when the web application starts.
        /// </summary>
        [ConfigurationProperty("startThreads", IsDefaultCollection = true)]
        public StartThreadElementCollection StartThreads
        {
            get { return (StartThreadElementCollection)this["startThreads"]; }
        }

        /// <summary>
        /// The security settings.
        /// </summary>
        [ConfigurationProperty("security")]
        public SecurityElement Security
        {
            get { return (SecurityElement)this["security"]; }
            set { this["security"] = value; }
        }

        #endregion

        #region Internal Methods

        internal void Merge(FrameworkConfiguration section)
        {
            if (section == null) return;

            this.Currencies.AddRange(section.Currencies);
            this.WebApplication = section.WebApplication;
            this.Roles.AddRange(section.Roles);
            this.Actions.AddRange(section.Actions);
            this.Settings.AddRange(section.Settings);
            this.Entities.AddRange(section.Entities);
            this.RulesEngines.AddRange(section.RulesEngines);
            this.StartThreads.AddRange(section.StartThreads);
            if (!string.IsNullOrEmpty(section.Security.PrivateKey))
                this.Security.PrivateKey = section.Security.PrivateKey;
        }

        internal void SetBuiltIn(bool builtIn)
        {
            foreach (RoleElement role in this.Roles)
            {
                role.BuiltIn = builtIn;
            }

            this.Actions.SetBuiltIn(builtIn);
            this.Settings.SetBuiltIn(builtIn);
        }

        #endregion
    }
}
