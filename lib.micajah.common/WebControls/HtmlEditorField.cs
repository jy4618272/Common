using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls
{
    public class HtmlEditorField : BaseValidatedField, IValidated
    {
        #region Private Methods

        private void CopyProperties(HtmlEditor control)
        {
            BaseValidatedField.CopyProperties(this, control);
            control.ToolTip = this.ToolTip;
        }

        private void OnBindingField(object sender, EventArgs e)
        {
            HtmlEditor control = sender as HtmlEditor;
            if (control != null)
            {
                if (!this.InsertMode) control.Content = this.LookupStringValue(control);
            }
            else
            {
                TableCell cell = sender as TableCell;
                if (cell != null) cell.Text = this.LookupStringValue(cell);
            }
        }

        #endregion

        #region Protected Methods

        protected virtual HtmlEditor CreateControl()
        {
            return new HtmlEditor();
        }

        #endregion

        #region Overriden Methods

        protected override DataControlField CreateField()
        {
            return new HtmlEditorField();
        }

        protected override object ExtractControlValue(Control control)
        {
            HtmlEditor editor = control as HtmlEditor;
            return ((editor == null) ? string.Empty : editor.Content);
        }

        protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
        {
            Control ctrl = null;

            if (this.EditMode || this.InsertMode)
            {
                HtmlEditor control = this.CreateControl();
                this.CopyProperties(control);
                control.Init += OnControlInit;
                if (!base.Visible) control.Required = false;
                if (cell != null)
                    cell.Controls.Add(control);
                ctrl = control;
            }
            else
            {
                if (cell != null)
                    cell.Style[HtmlTextWriterStyle.PaddingLeft] = "3px";
                ctrl = cell;
            }

            if (ctrl != null && base.Visible) ctrl.DataBinding += new EventHandler(this.OnBindingField);
        }

        #endregion
    }
}
