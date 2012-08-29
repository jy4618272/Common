using System.ComponentModel;
using System.Xml;
using Micajah.Common.Properties;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// The WYSIWYG rich-text editor.
    /// </summary>
    public class TextEditor : HtmlEditor
    {
        #region Members

        private bool m_ToolsFileReplaced;

        #endregion

        #region Overriden Properties

        public new EditModes EditModes
        {
            get { return base.EditModes; }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the configuration of the toolbar.
        /// </summary>
        [Category("Behavior")]
        [Description("The configuration of the toolbar.")]
        [DefaultValue(TextEditorToolBarConfiguration.Lite)]
        public TextEditorToolBarConfiguration ToolBarConfiguration
        {
            get
            {
                object obj = base.ViewState["ToolBarConfiguration"];
                return (obj == null) ? TextEditorToolBarConfiguration.Lite : (TextEditorToolBarConfiguration)obj;
            }
            set { base.ViewState["ToolBarConfiguration"] = value; }
        }

        #endregion

        #region Constructors

        public TextEditor()
        {
            base.EditModes = EditModes.Design;
        }

        #endregion

        #region Overriden Methods

        protected override void LoadToolsFile(bool loadOnlyEmptyCollections)
        {
            if (m_ToolsFileReplaced)
                base.LoadToolsFile(false);
            else
            {
                m_ToolsFileReplaced = true;

                string xml = null;
                switch (this.ToolBarConfiguration)
                {
                    case TextEditorToolBarConfiguration.Lite:
                        xml = Resources.TextEditorLiteToolBarConfiguration;
                        break;
                    case TextEditorToolBarConfiguration.Standard:
                        xml = Resources.TextEditorStandardToolBarConfiguration;
                        break;
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                base.LoadToolsFile(doc);
            }
        }

        #endregion
    }
}
