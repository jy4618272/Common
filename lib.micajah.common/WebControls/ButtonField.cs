using System;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Micajah.Common.Properties;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Represents a field that is displayed as a button in a data-bound control.
    /// </summary>
    public class ButtonField : System.Web.UI.WebControls.ButtonField, ISpanned
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
        /// Gets or sets the client-side script that executes when a control's Click event is raised.
        /// </summary>
        [Description("The client-side script that executes when a control's Click event is raised.")]
        [DefaultValue("")]
        public string OnClientClick
        {
            get
            {
                object obj = base.ViewState["OnClientClick"];
                return (obj == null) ? string.Empty : (string)obj;
            }
            set { base.ViewState["OnClientClick"] = value; }
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Creates an empty Micajah.Common.WebControls.ButtonField object.
        /// </summary>
        /// <returns>An empty Micajah.Common.WebControls.ButtonField.</returns>
        protected override DataControlField CreateField()
        {
            return new ButtonField();
        }

        /// <summary>
        /// Copies the properties of the current Micajah.Common.WebControls.ButtonField object to the specified System.Web.UI.WebControls.DataControlField object.
        /// </summary>
        /// <param name="newField">The System.Web.UI.WebControls.DataControlField to copy the properties of the current Micajah.Common.WebControls.ButtonField to.</param>
        protected override void CopyProperties(DataControlField newField)
        {
            base.CopyProperties(newField);
            ButtonField field = newField as ButtonField;
            if (field != null)
            {
                field.CreateNewRow = this.CreateNewRow;
                field.ColumnSpan = this.ColumnSpan;
                field.HeaderGroup = this.HeaderGroup;
                field.OnClientClick = this.OnClientClick;
            }
        }

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);

            if (cell != null)
            {
                if (cellType == DataControlCellType.DataCell)
                {
                    if (cell.Controls.Count > 0)
                    {
                        WebControl control = cell.Controls[0] as WebControl;
                        if (control != null)
                            control.Attributes.Add("onclick", this.OnClientClick);
                    }
                    if (this.ColumnSpan > 1)
                        cell.ColumnSpan = this.ColumnSpan;
                }
            }
        }

        #endregion
    }
}
