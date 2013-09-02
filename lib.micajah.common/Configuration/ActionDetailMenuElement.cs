using System.Configuration;
using System.Xml.XPath;
using Micajah.Common.WebControls;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings of the action in the detail menu.
    /// </summary>
    public class ActionDetailMenuElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ActionDetailMenuElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public ActionDetailMenuElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that indicates whether the action is rendered in the detail menu.
        /// </summary>
        [ConfigurationProperty("show", DefaultValue = true)]
        public bool Show
        {
            get { return (bool)this["show"]; }
            set { this["show"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the child actions is rendered in the detail menu.
        /// </summary>
        [ConfigurationProperty("showChildren")]
        public bool ShowChildren
        {
            get { return (bool)this["showChildren"]; }
            set { this["showChildren"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the description is rendered in the detail menu.
        /// </summary>
        [ConfigurationProperty("showDescription", DefaultValue = true)]
        public bool ShowDescription
        {
            get { return (bool)this["showDescription"]; }
            set { this["showDescription"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the action is rendered as group in the detail menu.
        /// </summary>
        [ConfigurationProperty("group")]
        public bool Group
        {
            get { return (bool)this["group"]; }
            set { this["group"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the action is highlighted in the detail menu.
        /// </summary>
        [ConfigurationProperty("highlight")]
        public bool Highlight
        {
            get { return (bool)this["highlight"]; }
            set { this["highlight"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL to icon of the action.
        /// </summary>
        [ConfigurationProperty("iconUrl")]
        public string IconUrl
        {
            get { return (string)this["iconUrl"]; }
            set { this["iconUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the size of icon of the action.
        /// </summary>
        [ConfigurationProperty("iconSize")]
        public IconSize? IconSize
        {
            get { return (IconSize?)this["iconSize"]; }
            set { this["iconSize"] = value; }
        }

        /// <summary>
        /// Gets or sets the theme of the detail menu action.
        /// </summary>
        [ConfigurationProperty("theme")]
        public DetailMenuTheme? Theme
        {
            get { return (DetailMenuTheme?)this["theme"]; }
            set { this["theme"] = value; }
        }

        #endregion

        #region Internal Methods

        internal void Merge(ActionDetailMenuElement detailMenu)
        {
            this.Show = detailMenu.Show;
            this.ShowChildren = detailMenu.ShowChildren;
            this.ShowDescription = detailMenu.ShowDescription;
            this.Group = detailMenu.Group;
            this.Highlight = detailMenu.Highlight;
            this.IconUrl = detailMenu.IconUrl;
            if (!this.IconSize.HasValue)
                this.IconSize = detailMenu.IconSize;
            if (!this.Theme.HasValue)
                this.Theme = detailMenu.Theme;
        }

        #endregion
    }
}
