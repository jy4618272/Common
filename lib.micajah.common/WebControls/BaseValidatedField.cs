using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Represents a base validated field.
    /// </summary>
    public abstract class BaseValidatedField : BoundField, IValidated, ISpanned
    {
        #region Members

        private bool m_EditMode;
        private bool m_InsertMode;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the server control contained by the field is initialized.
        /// </summary>
        public event EventHandler ControlInit;

        #endregion

        #region Protected Properties

        protected bool EditMode
        {
            get { return m_EditMode; }
        }

        protected bool InsertMode
        {
            get { return m_InsertMode; }
        }

        #endregion

        #region Public Properies

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

        /// <summary>
        /// Gets or sets a value indicating whether the control is enabled.
        /// </summary>
        [Category("Behavior")]
        [Description("Enabled state of the control.")]
        [DefaultValue(true)]
        public bool Enabled
        {
            get
            {
                object obj = base.ViewState["Enabled"];
                return (obj == null) ? true : (bool)obj;
            }
            set { base.ViewState["Enabled"] = value; }
        }

        /// <summary>
        /// Gets or sets the text for the required field's error message when validation fails.
        /// </summary>
        [Category("Appearance")]
        [Description("The text for the required field's error message when validation fails.")]
        [DefaultValue("")]
        public string ErrorMessage
        {
            get
            {
                object obj = base.ViewState["ErrorMessage"];
                return (obj == null) ? string.Empty : (string)obj;
            }
            set { base.ViewState["ErrorMessage"] = value; }
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
        /// Gets or sets the value indicating that the control's value is validated as required.
        /// </summary>
        [Category("Behavior")]
        [Description("Required state of the control.")]
        [DefaultValue(false)]
        public bool Required
        {
            get
            {
                object obj = base.ViewState["Required"];
                return (obj == null) ? false : (bool)obj;
            }
            set { base.ViewState["Required"] = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether the control is displayed as required.
        /// </summary>
        [Category("Behavior")]
        [Description("Whether the control is displayed as required.")]
        [DefaultValue(true)]
        public bool ShowRequired
        {
            get
            {
                object obj = ViewState["ShowRequired"];
                return ((obj == null) ? true : (bool)obj);
            }
            set { ViewState["ShowRequired"] = value; }
        }

        /// <summary>
        /// Gets or sets the tab index of the control.
        /// The default is 0, which indicates that this property is not set.
        /// </summary>
        [Category("Accessibility")]
        [Description("The tab order of the control.")]
        [DefaultValue(typeof(short), "0")]
        public virtual short TabIndex
        {
            get
            {
                object obj = ViewState["TabIndex"];
                return ((obj == null) ? (short)0 : (short)obj);
            }
            set { ViewState["TabIndex"] = value; }
        }

        /// <summary>
        /// Gets or sets the text displayed when the mouse pointer hovers over the control.
        /// </summary>
        [Category("Behavior")]
        [Description("The text displayed when the mouse pointer hovers over the control.")]
        [DefaultValue("")]
        public string ToolTip
        {
            get
            {
                object obj = ViewState["ToolTip"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["ToolTip"] = value; }
        }

        /// <summary>
        /// Gets or sets the group of controls for which this control causes validation when it posts back to the server.
        /// </summary>
        [Category("Behavior")]
        [Description("The group of controls for which this control causes validation when it posts back to the server.")]
        [DefaultValue("")]
        public string ValidationGroup
        {
            get
            {
                object obj = ViewState["ValidationGroup"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["ValidationGroup"] = value; }
        }

        /// <summary>
        /// Gets or sets the initial value of the control used by required validation.
        /// </summary>
        [Category("Behavior")]
        [Description("The initial value of the control used by required validation.")]
        [DefaultValue("")]
        public string ValidatorInitialValue
        {
            get
            {
                object obj = ViewState["ValidatorInitialValue"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["ValidatorInitialValue"] = value; }
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

        #region Internal Methods

        internal static void CopyProperties(IValidated fromControl, IValidated toControl)
        {
            toControl.Enabled = fromControl.Enabled;
            toControl.ErrorMessage = fromControl.ErrorMessage;
            toControl.Required = fromControl.Required;
            toControl.ShowRequired = fromControl.ShowRequired;
            toControl.TabIndex = fromControl.TabIndex;
            toControl.ValidationGroup = fromControl.ValidationGroup;
            toControl.ValidatorInitialValue = fromControl.ValidatorInitialValue;
            toControl.Theme = fromControl.Theme;
        }

        internal static void InitializeSpannedCell(DataControlFieldCell cell, DataControlCellType cellType)
        {
            if (cell == null) return;

            switch (cellType)
            {
                case DataControlCellType.Header:
                    cell.Attributes["SpannedCell"] = "start";
                    break;
                case DataControlCellType.Footer:
                    cell.Attributes["SpannedCell"] = "end";
                    break;
                case DataControlCellType.DataCell:
                    cell.Attributes["SpannedCell"] = "text";
                    break;
            }
        }

        #endregion

        #region Protected Methods

        protected virtual object ExtractControlValue(Control control)
        {
            return null;
        }

        protected void OnControlInit(object sender, EventArgs e)
        {
            if (this.ControlInit != null) this.ControlInit(sender, e);
        }

        protected virtual object LookupValue(Control control)
        {
            object value = null;
            if (base.DesignMode)
                value = base.GetDesignTimeValue();
            else if (control != null)
            {
                if (control.NamingContainer != null)
                {
                    object dataItem = DataBinder.GetDataItem(control.NamingContainer);
                    if (dataItem != null)
                        value = DataBinder.GetPropertyValue(dataItem, this.DataField);
                }
            }
            return value;
        }

        protected string LookupStringValue(Control control)
        {
            return LookupStringValue(control, false);
        }

        protected string LookupStringValue(Control control, bool applyDataFormatString)
        {
            return this.LookupStringValue(control, (applyDataFormatString ? this.DataFormatString : null));
        }

        protected string LookupStringValue(Control control, string dataFormat)
        {
            object obj = LookupValue(control);

            if ((!base.DesignMode) && (!string.IsNullOrEmpty(this.DataFormatString)))
                return string.Format(CultureInfo.CurrentCulture, dataFormat, new object[] { obj });

            if (!Support.IsNullOrDBNull(obj))
                return obj.ToString();

            return null;
        }

        protected DateTime LookupDateTimeValue(Control control)
        {
            DateTime value = DateTime.MinValue;

            if (DateTime.TryParse(LookupStringValue(control), out value))
                return value;

            return DateTime.MinValue;
        }

        protected bool LookupBooleanValue(Control control)
        {
            bool value = false;

            if (base.DesignMode)
                value = true;
            else if (bool.TryParse(LookupStringValue(control), out value))
                return value;

            return value;
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Copies the properties of the current BaseValidatedField object to the specified DataControlField object.
        /// </summary>
        /// <param name="newField">The System.Web.UI.WebControls.DataControlField to copy the properties of the current Micajah.Common.WebControls.BaseValidatedField to.</param>
        protected override void CopyProperties(DataControlField newField)
        {
            base.CopyProperties(newField);

            BaseValidatedField field = newField as BaseValidatedField;
            if (field != null)
            {
                CopyProperties(this, field);

                field.ColumnSpan = this.ColumnSpan;
                field.CreateNewRow = this.CreateNewRow;
                field.HeaderGroup = this.HeaderGroup;
                field.Enabled = this.Enabled;
                field.ToolTip = this.ToolTip;
                field.Theme = this.Theme;
            }
        }

        public override void ExtractValuesFromCell(IOrderedDictionary dictionary, DataControlFieldCell cell, DataControlRowState rowState, bool includeReadOnly)
        {
            object value = null;

            if (cell != null)
            {
                if (cell.Controls.Count > 0)
                    value = this.ExtractControlValue(cell.Controls[0]);
            }

            if (dictionary != null)
            {
                if (dictionary.Contains(this.DataField))
                    dictionary[this.DataField] = value;
                else
                    dictionary.Add(this.DataField, value);
            }
        }

        /// <summary>
        /// Initializes the specified TableCell object to the specified row state.
        /// </summary>
        /// <param name="cell">The TableCell to initialize.</param>
        /// <param name="cellType">One of the DataControlCellType values.</param>
        /// <param name="rowState">One of the DataControlRowState values.</param>
        /// <param name="rowIndex">The zero-based index of the row.</param>
        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            m_EditMode = ((rowState & DataControlRowState.Edit) == DataControlRowState.Edit);
            m_InsertMode = ((rowState & DataControlRowState.Insert) == DataControlRowState.Insert);

            if (cell == null) return;

            switch (cellType)
            {
                case DataControlCellType.Header:
                    cell.ApplyStyle(HeaderStyle);
                    break;
                case DataControlCellType.DataCell:
                    cell.ApplyStyle(ItemStyle);
                    break;
                case DataControlCellType.Footer:
                    cell.ApplyStyle(FooterStyle);
                    break;
            }

            base.InitializeCell(cell, cellType, rowState, rowIndex);

            if (cellType == DataControlCellType.Header) cell.ToolTip = ToolTip;

            if (base.Control is CommonGridView)
            {
                if (this.ColumnSpan > 1) cell.ColumnSpan = this.ColumnSpan;
            }
        }

        #endregion
    }
}
