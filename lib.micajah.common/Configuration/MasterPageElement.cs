using System.Configuration;
using System.Xml.XPath;
using Micajah.Common.Pages;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings of the master page.
    /// </summary>
    public class MasterPageElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MasterPageElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public MasterPageElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the theme of the master page.
        /// </summary>
        [ConfigurationProperty("theme")]
        public MasterPageTheme Theme
        {
            get { return (MasterPageTheme)this["theme"]; }
            set { this["theme"] = value; }
        }

        /// <summary>
        /// Gets or sets the color for the master page theme.
        /// </summary>
        [ConfigurationProperty("themeColor")]
        public MasterPageThemeColor ThemeColor
        {
            get { return (MasterPageThemeColor)this["themeColor"]; }
            set { this["themeColor"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the name of the application is added as prefix for the page title.
        /// </summary>
        [ConfigurationProperty("titlePrefix", DefaultValue = true)]
        public bool TitlePrefix
        {
            get { return (bool)this["titlePrefix"]; }
            set { this["titlePrefix"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the CustomStyleSheet is enabled.
        /// </summary>
        [ConfigurationProperty("enableCustomStyleSheet", DefaultValue = true)]
        public bool EnableCustomStyleSheet
        {
            get { return (bool)this["enableCustomStyleSheet"]; }
            set { this["enableCustomStyleSheet"] = value; }
        }

        /// <summary>
        /// Gets or sets the settings of the header.
        /// </summary>
        [ConfigurationProperty("header")]
        public HeaderElement Header
        {
            get { return (HeaderElement)this["header"]; }
            set { this["header"] = value; }
        }

        /// <summary>
        /// Gets or sets the settings of the main menu.
        /// </summary>
        [ConfigurationProperty("mainMenu")]
        public MainMenuElement MainMenu
        {
            get { return (MainMenuElement)this["mainMenu"]; }
            set { this["mainMenu"] = value; }
        }

        /// <summary>
        /// Gets or sets the settings of the breadcrumbs.
        /// </summary>
        [ConfigurationProperty("breadcrumbs")]
        public BreadcrumbsElement Breadcrumbs
        {
            get { return (BreadcrumbsElement)this["breadcrumbs"]; }
            set { this["breadcrumbs"] = value; }
        }

        /// <summary>
        /// Gets or sets the settings of the help link.
        /// </summary>
        [ConfigurationProperty("helpLink")]
        public HelpLinkElement HelpLink
        {
            get { return (HelpLinkElement)this["helpLink"]; }
            set { this["helpLink"] = value; }
        }

        /// <summary>
        /// Gets or sets the settings of the submenu.
        /// </summary>
        [ConfigurationProperty("submenu")]
        public SubmenuElement Submenu
        {
            get { return (SubmenuElement)this["submenu"]; }
            set { this["submenu"] = value; }
        }

        /// <summary>
        /// Gets or sets the settings of the left area.
        /// </summary>
        [ConfigurationProperty("leftArea")]
        public LeftAreaElement LeftArea
        {
            get { return (LeftAreaElement)this["leftArea"]; }
            set { this["leftArea"] = value; }
        }

        /// <summary>
        /// Gets or sets the settings of the detail menu.
        /// </summary>
        [ConfigurationProperty("detailMenu")]
        public DetailMenuElement DetailMenu
        {
            get { return (DetailMenuElement)this["detailMenu"]; }
            set { this["detailMenu"] = value; }
        }

        /// <summary>
        /// Gets or sets the settings of the footer.
        /// </summary>
        [ConfigurationProperty("footer")]
        public FooterElement Footer
        {
            get { return (FooterElement)this["footer"]; }
            set { this["footer"] = value; }
        }

        #endregion
    }
}
