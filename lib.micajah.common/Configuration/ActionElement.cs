using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Xml.XPath;
using Micajah.Common.Bll;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The action.
    /// </summary>
    public class ActionElement : BaseConfigurationElement, IConfigurationElement
    {
        #region Members

        private bool m_Visible = true;
        private bool m_InstanceRequired = true;
        private bool? m_OriginalVisible;
        private bool? m_OriginalInstanceRequired;
        private bool m_IsLoaded;
        private bool m_BuiltIn;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ActionElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public ActionElement(IXPathNavigable node) : base(node) { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        /// <param name="builtIn">The value indicating whether the element is built-in.</param>
        public ActionElement(IXPathNavigable node, bool builtIn)
            : base(node)
        {
            m_BuiltIn = builtIn;
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets or sets a value that indicates whether the action is visible.
        /// </summary>
        [ConfigurationProperty("visible")]
        [RegexStringValidator("^(\\s*|true|false)$")]
        private string OriginalVisible
        {
            get { return (string)this["visible"]; }
            set { this["visible"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the action requires the authentication.
        /// </summary>
        [ConfigurationProperty("instanceRequired")]
        [RegexStringValidator("^(\\s*|true|false)$")]
        private string OriginalInstanceRequired
        {
            get { return (string)this["instanceRequired"]; }
            set { this["instanceRequired"] = value; }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the value indicating whether the element is built-in.
        /// </summary>
        internal bool BuiltIn
        {
            get { return m_BuiltIn; }
            set { m_BuiltIn = value; }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the unique identifier of the action.
        /// </summary>
        [ConfigurationProperty("id", IsRequired = true)]
        public Guid Id
        {
            get { return (Guid)this["id"]; }
            set { this["id"] = value; }
        }

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        [ConfigurationProperty("type")]
        public ActionType ActionType
        {
            get { return (ActionType)this["type"]; }
            set { this["type"] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the description of the action.
        /// </summary>
        [ConfigurationProperty("description")]
        public string Description
        {
            get { return (string)this["description"]; }
            set { this["description"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the action to navigate.
        /// </summary>
        [ConfigurationProperty("navigateUrl")]
        public string NavigateUrl
        {
            get
            {
                string value = (string)this["navigateUrl"];
                if (string.Compare(value, "null", StringComparison.OrdinalIgnoreCase) == 0)
                    return null;
                else
                    return value;
            }
            set
            {
                if (string.Compare(value, "null", StringComparison.OrdinalIgnoreCase) == 0)
                    this["navigateUrl"] = null;
                else
                    this["navigateUrl"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the order number of the action.
        /// </summary>
        [ConfigurationProperty("orderNumber")]
        [IntegerValidator()]
        public int OrderNumber
        {
            get { return (int)this["orderNumber"]; }
            set { this["orderNumber"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the action requires the authentication.
        /// </summary>
        [ConfigurationProperty("authenticationRequired")]
        public bool AuthenticationRequired
        {
            get { return (bool)this["authenticationRequired"]; }
            set { this["authenticationRequired"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the action requires the authentication.
        /// </summary>
        public bool InstanceRequired
        {
            get
            {
                this.EnsureIsLoaded();
                return m_InstanceRequired;
            }
            set
            {
                m_OriginalInstanceRequired = m_InstanceRequired = value;
                this.OriginalInstanceRequired = (value ? "true" : "false");
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the action is visible.
        /// </summary>
        public bool Visible
        {
            get
            {
                this.EnsureIsLoaded();
                return m_Visible;
            }
            set
            {
                m_OriginalVisible = m_Visible = value;
                this.OriginalVisible = (value ? "true" : "false");
            }
        }

        /// <summary>
        /// Gets or sets the URL of the page to learn more.
        /// </summary>
        [ConfigurationProperty("learnMoreUrl")]
        public string LearnMoreUrl
        {
            get { return (string)this["learnMoreUrl"]; }
            set { this["learnMoreUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the video.
        /// </summary>
        [ConfigurationProperty("videoUrl")]
        public string VideoUrl
        {
            get { return (string)this["videoUrl"]; }
            set { this["videoUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating the action should be handled.
        /// </summary>
        [ConfigurationProperty("handle", DefaultValue = false)]
        public bool Handle
        {
            get { return (bool)this["handle"]; }
            set { this["handle"] = value; }
        }

        /// <summary>
        /// Gets the roles which have the access rights to this action.
        /// </summary>
        [ConfigurationProperty("roles")]
        [TypeConverter(typeof(CommaDelimitedStringCollectionConverter))]
        public StringCollection Roles
        {
            get { return (StringCollection)this["roles"]; }
        }

        /// <summary>
        /// Gets the unique identifiers of the alternative parents of the action.
        /// </summary>
        [ConfigurationProperty("alternativeParents")]
        [TypeConverter(typeof(CommaDelimitedStringCollectionConverter))]
        public StringCollection AlternativeParents
        {
            get { return (StringCollection)this["alternativeParents"]; }
        }

        /// <summary>
        /// Gets or sets the settings of the action in the detail menu.
        /// </summary>
        [ConfigurationProperty("detailMenu")]
        public ActionDetailMenuElement DetailMenu
        {
            get { return (ActionDetailMenuElement)this["detailMenu"]; }
            set { this["detailMenu"] = value; }
        }

        /// <summary>
        /// Gets or sets the settings of the action in the submenu.
        /// </summary>
        [ConfigurationProperty("submenu")]
        public ActionSubmenuElement Submenu
        {
            get { return (ActionSubmenuElement)this["submenu"]; }
            set { this["submenu"] = value; }
        }

        /// <summary>
        /// Gets the child actions.
        /// </summary>
        [ConfigurationProperty("actions", IsDefaultCollection = true)]
        public ActionElementCollection Actions
        {
            get { return (ActionElementCollection)this["actions"]; }
        }

        /// <summary>
        /// Gets the child settings.
        /// </summary>
        [ConfigurationProperty("settings", IsDefaultCollection = true)]
        public SettingElementCollection Settings
        {
            get { return (SettingElementCollection)this["settings"]; }
        }

        /// <summary>
        /// Gets the key of the object.
        /// </summary>
        public object Key
        {
            get { return this.Id; }
        }

        #endregion

        #region Private Methods

        private void EnsureIsLoaded()
        {
            if (!m_IsLoaded)
            {
                object obj = Support.ConvertStringToType(this.OriginalVisible, typeof(bool));
                if (obj != null) m_OriginalVisible = m_Visible = (bool)obj;

                obj = Support.ConvertStringToType(this.OriginalInstanceRequired, typeof(bool));
                if (obj != null) m_OriginalInstanceRequired = m_InstanceRequired = (bool)obj;

                m_IsLoaded = true;
            }
        }

        #endregion

        #region Internal Methods

        internal void Merge(ActionElement action)
        {
            action.EnsureIsLoaded();

            if (action.m_OriginalVisible.HasValue)
                this.Visible = action.Visible;

            if (action.m_OriginalInstanceRequired.HasValue)
                this.InstanceRequired = action.InstanceRequired;

            if (action.DetailMenu.Theme.HasValue)
                this.DetailMenu.Theme = action.DetailMenu.Theme;

            if (action.DetailMenu.IconSize.HasValue)
                this.DetailMenu.IconSize = action.DetailMenu.IconSize;

            foreach (ActionElement actionToAdd in action.Actions)
            {
                this.Actions.Add(actionToAdd);
            }

            foreach (SettingElement settingToAdd in action.Settings)
            {
                this.Settings.Add(settingToAdd);
            }
        }

        #endregion
    }

    /// <summary>
    /// The collection of the actions.
    /// </summary>w
    [ConfigurationCollection(typeof(ActionElement), AddItemName = "action")]
    public class ActionElementCollection : BaseConfigurationElementCollection<ActionElement>
    {
        #region Internal Methods

        internal void SetBuiltIn(bool builtIn)
        {
            foreach (ActionElement element in this)
            {
                element.BuiltIn = builtIn;
                element.Actions.SetBuiltIn(builtIn);
            }
        }

        #endregion

        #region Overriden Methods

        public override void Add(ActionElement item)
        {
            if (item == null) return;

            ActionElement existingItem = base.BaseGet(item.Key) as ActionElement;
            if (existingItem != null)
            {
                if (existingItem.BuiltIn)
                    existingItem.Merge(item);
            }
            else
                base.Add(item);
        }

        #endregion
    }
}
