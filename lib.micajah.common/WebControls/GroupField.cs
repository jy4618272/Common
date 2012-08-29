using System.ComponentModel;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;

namespace Micajah.Common.WebControls
{
    public class GroupField : DataControlField, IThemeable
    {
        #region Public Properties

        [DefaultValue("")]
        public string Text
        {
            get
            {
                object obj = base.ViewState["Text"];
                return (obj == null) ? string.Empty : (string)obj;
            }
            set { base.ViewState["Text"] = value; }
        }

        public MasterPageTheme Theme
        {
            get
            {
                object obj = ViewState["Theme"];
                return ((obj == null) ? FrameworkConfiguration.Current.WebApplication.MasterPage.Theme : (MasterPageTheme)obj);
            }
            set { ViewState["Theme"] = value; }
        }

        #endregion

        #region Overriden Methods

        protected override void CopyProperties(DataControlField newField)
        {
            base.CopyProperties(newField);

            GroupField field = (newField as GroupField);
            if (field != null)
            {
                field.Text = this.Text;
                field.Theme = this.Theme;
            }
        }

        protected override DataControlField CreateField()
        {
            return new GroupField();
        }

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            if (cell == null) return;

            if (this.Control is MagicForm)
            {
                cell.CssClass = "Mf_H";
                if (this.Theme == MasterPageTheme.Modern)
                {
                    HtmlGenericControl div = new HtmlGenericControl("div");
                    div.InnerHtml = this.Text;

                    cell.Controls.Add(div);
                }
                else
                    if (cellType == DataControlCellType.DataCell) cell.Text = Text;
            }
            else
                if (cellType == DataControlCellType.DataCell) cell.Text = Text;

            BaseValidatedField.InitializeSpannedCell(cell, cellType);

            base.InitializeCell(cell, cellType, rowState, rowIndex);
        }

        public override void ExtractValuesFromCell(System.Collections.Specialized.IOrderedDictionary dictionary, DataControlFieldCell cell, DataControlRowState rowState, bool includeReadOnly)
        {
        }

        #endregion
    }
}
