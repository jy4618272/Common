using System.ComponentModel;
using System.Web.UI.WebControls;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Represents a field that displays custom content in a data-bound control.
    /// </summary>
    public class TemplateField : System.Web.UI.WebControls.TemplateField, ISpanned, IThemeable
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the number of columns in the control that the cell spans.
        /// The default value is 0, which indicates that this property is not set.
        /// </summary>
        [Category("Appearance")]
        [Description("The number of columns in the control that the cell spans.")]
        [DefaultValue(0)]
        public int ColumnSpan
        {
            get
            {
                object obj = ViewState["ColumnSpan"];
                return ((obj == null) ? 0 : (int)obj);
            }
            set
            {
                if (value < 0)
                    ViewState.Remove("ColumnSpan");
                else
                    ViewState["ColumnSpan"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the value indicating that the next control is appeared on new row.
        /// </summary>
        [Category("Behavior")]
        [Description("Whether the next control is appeared on new row.")]
        [DefaultValue(false)]
        public bool CreateNewRow
        {
            get
            {
                object obj = base.ViewState["CreateNewRow"];
                return (obj == null) ? false : (bool)obj;
            }
            set { base.ViewState["CreateNewRow"] = value; }
        }

        [DefaultValue("")]
        public string HeaderGroup
        {
            get
            {
                object obj = ViewState["HeaderGroup"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["HeaderGroup"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating that the left padding is not rendered.
        /// </summary>
        [DefaultValue(true)]
        public bool PaddingLeft
        {
            get
            {
                if (this.Theme == MasterPageTheme.Modern)
                    return false;

                object obj = ViewState["PaddingLeft"];
                return ((obj == null) ? true : (bool)obj);
            }
            set { ViewState["PaddingLeft"] = value; }
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

        /// <summary>
        /// Creates an empty Micajah.Common.WebControls.TemplateField object.
        /// </summary>
        /// <returns>An empty Micajah.Common.WebControls.TemplateField.</returns>
        protected override DataControlField CreateField()
        {
            return new TemplateField();
        }

        /// <summary>
        /// Copies the properties of the current Micajah.Common.WebControls.TemplateField object to the specified System.Web.UI.WebControls.DataControlField object.
        /// </summary>
        /// <param name="newField">The System.Web.UI.WebControls.DataControlField to copy the properties of the current Micajah.Common.WebControls.TemplateField to.</param>
        protected override void CopyProperties(DataControlField newField)
        {
            base.CopyProperties(newField);
            TemplateField field = newField as TemplateField;
            if (field != null)
            {
                field.CreateNewRow = this.CreateNewRow;
                field.ColumnSpan = this.ColumnSpan;
                field.HeaderGroup = this.HeaderGroup;
                field.PaddingLeft = this.PaddingLeft;
                field.Theme = this.Theme;
            }
        }

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);
            if (cell != null)
            {
                if (cellType == DataControlCellType.DataCell)
                {
                    if (this.ColumnSpan > 1) cell.ColumnSpan = this.ColumnSpan;

                    foreach (System.Web.UI.Control ctl in cell.Controls)
                    {
                        IThemeable t = ctl as IThemeable;
                        if (t != null)
                            t.Theme = this.Theme;
                    }
                }
            }
        }

        #endregion
    }
}
