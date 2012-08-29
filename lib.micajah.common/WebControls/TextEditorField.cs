using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls
{
    public class TextEditorField : HtmlEditorField
    {
        #region Overriden Methods

        protected override HtmlEditor CreateControl()
        {
            return new TextEditor();
        }

        protected override DataControlField CreateField()
        {
            return new TextEditorField();
        }

        #endregion
    }
}