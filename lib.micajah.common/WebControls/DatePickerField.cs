using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Represents a field that is displayed as a control that allows the user to enter or choose date and (or) time from calendar in a data-bound control.
    /// </summary>
    public class DatePickerField : BaseValidatedField, IDatePicker
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the value indicating that the typing in the date input box is enabled or disabled.
        /// </summary>
        [Category("Behavior")]
        [Description("Enables or disables typing in the date input box.")]
        [DefaultValue(true)]
        public bool EnableTyping
        {
            get
            {
                object obj = ViewState["EnableTyping"];
                return ((obj == null) ? true : (bool)obj);
            }
            set { ViewState["EnableTyping"] = value; }
        }

        /// <summary>
        /// Gets or sets the earliest valid date for selection.
        /// </summary>
        [Category("Date Selection")]
        [Description("The earliest valid date for selection.")]
        [DefaultValue(typeof(DateTime), "1/1/1980")]
        [Editor(typeof(System.ComponentModel.Design.DateTimeEditor), typeof(UITypeEditor))]
        public DateTime MinDate
        {
            get
            {
                object obj = ViewState["MinDate"];
                return ((obj == null) ? new DateTime(1980, 1, 1) : (DateTime)obj);
            }
            set { ViewState["MinDate"] = value; }
        }

        /// <summary>
        /// Gets or sets the latest valid date for selection.
        /// </summary>
        [Category("Date Selection")]
        [Description("The latest valid date for selection.")]
        [DefaultValue(typeof(DateTime), "12/31/2099")]
        [Editor(typeof(System.ComponentModel.Design.DateTimeEditor), typeof(UITypeEditor))]
        public DateTime MaxDate
        {
            get
            {
                object obj = ViewState["MaxDate"];
                return ((obj == null) ? new DateTime(2099, 12, 31) : (DateTime)obj);
            }
            set { ViewState["MaxDate"] = value; }
        }

        /// <summary>
        /// Gets or sets the selected date.
        /// </summary>
        [Category("Date Selection")]
        [Description("The selected date.")]
        [DefaultValue(typeof(DateTime), "1/1/1980")]
        [Editor(typeof(System.ComponentModel.Design.DateTimeEditor), typeof(UITypeEditor))]
        public DateTime SelectedDate
        {
            get
            {
                object obj = ViewState["SelectedDate"];
                return ((obj == null) ? this.MinDate : (DateTime)obj);
            }
            set { if (value > DateTime.MinValue) ViewState["SelectedDate"] = value; }
        }

        /// <summary>
        /// Gets or sets the date and time format.
        /// </summary>
        [Category("Behavior")]
        [Description("The date and time format.")]
        [DefaultValue("d")]
        public string DateFormat
        {
            get
            {
                object obj = ViewState["DateFormat"];
                return ((obj == null) ? "d" : (string)obj);
            }
            set { ViewState["DateFormat"] = value; }
        }

        /// <summary>
        /// Gets or sets the display width of the the date input box in characters.
        /// </summary>
        [Category("Appearance")]
        [Description("The display width of the the date input box in characters.")]
        [DefaultValue(0)]
        public int Columns
        {
            get
            {
                object obj = ViewState["Columns"];
                return ((obj == null) ? 0 : (int)obj);
            }
            set { ViewState["Columns"] = value; }
        }

        /// <summary>
        /// Gets or sets the type that define how to display and validate value in the control.
        /// </summary>
        [Category("Behavior")]
        [Description("The type that define how to display and validate value in the control.")]
        [DefaultValue(DatePickerType.DatePicker)]
        public DatePickerType Type
        {
            get
            {
                object obj = ViewState["Type"];
                return ((obj == null) ? DatePickerType.DatePicker : (DatePickerType)obj);
            }
            set { ViewState["Type"] = value; }
        }

        /// <summary>
        /// Gets the value indicating the selected date equals to MinDate.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEmpty
        {
            get { return (this.SelectedDate == this.MinDate); }
        }

        #endregion

        #region Private Methods

        private void CopyProperties(DatePicker control)
        {
            CopyProperties(this, control);

            control.EnableTyping = this.EnableTyping;
            control.MinDate = this.MinDate;
            control.MaxDate = this.MaxDate;
            control.SelectedDate = this.SelectedDate;
            control.DateFormat = this.DateFormat;
            control.ReadOnly = this.ReadOnly;
            control.Type = this.Type;
            if (!base.Visible) control.Required = false;
        }

        private void OnBindingField(object sender, EventArgs e)
        {
            DatePicker control = sender as DatePicker;
            if (control != null)
            {
                if ((!base.DesignMode) && (!this.InsertMode))
                {
                    DateTime value = this.LookupDateTimeValue(control);
                    if (value > DateTime.MinValue) control.SelectedDate = value;
                }
            }
            else
            {
                TableCell cell = sender as TableCell;
                if (cell != null) cell.Text = this.LookupStringValue(cell, this.DateFormat);
            }
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Creates an empty Micajah.Common.WebControls.DatePickerField object.
        /// </summary>
        /// <returns>An empty Micajah.Common.WebControls.DatePickerField.</returns>
        protected override DataControlField CreateField()
        {
            return new DatePickerField();
        }

        /// <summary>
        /// Copies the properties of the current Micajah.Common.WebControls.DatePickerField object to the specified System.Web.UI.WebControls.DataControlField object.
        /// </summary>
        /// <param name="newField">The System.Web.UI.WebControls.DataControlField to copy the properties of the current Micajah.Common.WebControls.DatePickerField to.</param>
        protected override void CopyProperties(DataControlField newField)
        {
            base.CopyProperties(newField);

            DatePickerField field = newField as DatePickerField;
            if (field != null)
            {
                field.EnableTyping = this.EnableTyping;
                field.MinDate = this.MinDate;
                field.MaxDate = this.MaxDate;
                field.SelectedDate = this.SelectedDate;
                field.DateFormat = this.DateFormat;
                field.Columns = this.Columns;
                field.Type = this.Type;
            }
        }

        protected override object ExtractControlValue(Control control)
        {
            DatePicker picker = control as DatePicker;
            return ((picker == null) ? DateTime.MinValue : picker.SelectedDate);
        }

        /// <summary>
        /// Initializes the specified System.Web.UI.WebControls.TableCell object to the specified row state.
        /// </summary>
        /// <param name="cell">The System.Web.UI.WebControls.TableCell to initialize.</param>
        /// <param name="rowState">One of the System.Web.UI.WebControls.DataControlRowState values.</param>
        protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
        {
            Control ctrl = null;

            if (this.EditMode || this.InsertMode)
            {
                DatePicker control = new DatePicker();
                CopyProperties(control);
                control.Init += OnControlInit;

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
