using System.Configuration;
using System.Web.UI.WebControls;
using System.Xml.XPath;
using Micajah.Common.WebControls;

namespace Micajah.Common.Configuration
{
    /// <summary>
    /// The settings of the action in the submenu.
    /// </summary>
    public class ActionSubmenuElement : BaseConfigurationElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ActionSubmenuElement() : base() { }

        /// <summary>
        /// Initializes a new instance of the class from the specified XML node.
        /// </summary>
        /// <param name="node">The XML node to initialize from.</param>
        public ActionSubmenuElement(IXPathNavigable node) : base(node) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the type of the action in the submenu.
        /// </summary>
        [ConfigurationProperty("itemType")]
        public SubmenuItemType ItemType
        {
            get { return (SubmenuItemType)this["itemType"]; }
            set { this["itemType"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL to the image for the action that is rendered as image in the submenu.
        /// </summary>
        [ConfigurationProperty("imageUrl")]
        public string ImageUrl
        {
            get { return (string)this["imageUrl"]; }
            set { this["imageUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the width, in pixels, for the action that is rendered in the submenu.
        /// </summary>
        [ConfigurationProperty("width")]
        [IntegerValidator()]
        public int Width
        {
            get { return (int)this["width"]; }
            set { this["width"] = value; }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the action in the submenu.
        /// </summary>
        [ConfigurationProperty("horizontalAlign")]
        public HorizontalAlign HorizontalAlign
        {
            get { return (HorizontalAlign)this["horizontalAlign"]; }
            set { this["horizontalAlign"] = value; }
        }

        #endregion
    }
}
