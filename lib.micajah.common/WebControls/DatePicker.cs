using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.Calendar;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Represents a control that allows the user to enter or choose date and (or) time from calendar.
    /// </summary>
    public class DatePicker : BaseValidatedControl, IDatePicker, IValidated
    {
        #region Members

        private RadDateTimePicker m_RadDateTimePicker;

        #endregion

        #region Overriden Properties

        /// <summary>
        /// Gets or sets a value indicating whether the control is enabled.
        /// </summary>
        public override bool Enabled
        {
            get
            {
                EnsureChildControls();
                return m_RadDateTimePicker.Enabled;
            }
            set
            {
                base.Enabled = value;
                m_RadDateTimePicker.Enabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the tab index of the control.
        /// The default is 0, which indicates that this property is not set.
        /// </summary>
        public override short TabIndex
        {
            get
            {
                EnsureChildControls();
                return m_RadDateTimePicker.TabIndex;
            }
            set
            {
                EnsureChildControls();
                m_RadDateTimePicker.TabIndex = value;
            }
        }

        /// <summary>
        /// Gets the collection of arbitrary attributes (for rendering only) that do not correspond to properties on the control.
        /// </summary>
        public override System.Web.UI.AttributeCollection Attributes
        {
            get
            {
                EnsureChildControls();
                return m_RadDateTimePicker.Attributes;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the contents of the date input box can be changed.
        /// </summary>
        [Category("Behavior")]
        [Description("Whether the contents of the date input box can be changed.")]
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return (m_RadDateTimePicker.DateInput.ReadOnly); }
            set
            {
                EnsureChildControls();
                m_RadDateTimePicker.DateInput.ReadOnly = value;
            }
        }

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
                EnsureChildControls();
                return m_RadDateTimePicker.EnableTyping;
            }
            set
            {
                EnsureChildControls();
                m_RadDateTimePicker.EnableTyping = value;
            }
        }

        /// <summary>
        /// Gets or sets the earliest valid date for selection.
        /// </summary>
        [Category("Date Selection")]
        [Description("The earliest valid date for selection.")]
        [DefaultValue(typeof(DateTime), "1/1/1980")]
        [Editor(typeof(DateTimeEditor), typeof(UITypeEditor))]
        public DateTime MinDate
        {
            get
            {
                EnsureChildControls();
                return m_RadDateTimePicker.MinDate;
            }
            set
            {
                EnsureChildControls();
                m_RadDateTimePicker.MinDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the latest valid date for selection.
        /// </summary>
        [Category("Date Selection")]
        [Description("The latest valid date for selection.")]
        [DefaultValue(typeof(DateTime), "12/31/2099")]
        [Editor(typeof(DateTimeEditor), typeof(UITypeEditor))]
        public DateTime MaxDate
        {
            get
            {
                EnsureChildControls();
                return m_RadDateTimePicker.MaxDate;
            }
            set
            {
                EnsureChildControls();
                m_RadDateTimePicker.MaxDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected date.
        /// </summary>
        [Category("Date Selection")]
        [Description("The selected date.")]
        [DefaultValue(typeof(DateTime), "1/1/1980")]
        [Editor(typeof(DateTimeEditor), typeof(UITypeEditor))]
        public DateTime SelectedDate
        {
            get
            {
                EnsureChildControls();
                return (m_RadDateTimePicker.SelectedDate.HasValue ? m_RadDateTimePicker.SelectedDate.Value : m_RadDateTimePicker.MinDate);
            }
            set
            {
                if (value > this.MinDate)
                {
                    EnsureChildControls();
                    m_RadDateTimePicker.SelectedDate = value;
                }
            }
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
                EnsureChildControls();
                return m_RadDateTimePicker.DateInput.DateFormat;
            }
            set
            {
                EnsureChildControls();
                m_RadDateTimePicker.DateInput.DateFormat = value;
                m_RadDateTimePicker.DateInput.DisplayDateFormat = value;
                int idx = value.IndexOf("h", StringComparison.OrdinalIgnoreCase);
                if (idx > -1)
                    m_RadDateTimePicker.TimeView.TimeFormat = value.Substring(idx);
            }
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
            set
            {
                EnsureChildControls();
                ResetToDatePickerType();
                switch (value)
                {
                    case DatePickerType.Date:
                        m_RadDateTimePicker.DatePopupButton.Visible = false;
                        break;
                    case DatePickerType.DatePicker:
                        break;
                    case DatePickerType.DateTime:
                        m_RadDateTimePicker.DateInput.DateFormat = m_RadDateTimePicker.DateInput.DisplayDateFormat = "g";
                        m_RadDateTimePicker.DatePopupButton.Visible = false;
                        break;
                    case DatePickerType.DateTimePicker:
                        m_RadDateTimePicker.DateInput.DateFormat = m_RadDateTimePicker.DateInput.DisplayDateFormat = "g";
                        m_RadDateTimePicker.TimePopupButton.Visible = true;
                        break;
                    case DatePickerType.Time:
                        m_RadDateTimePicker.DateInput.DateFormat = m_RadDateTimePicker.DateInput.DisplayDateFormat = "t";
                        m_RadDateTimePicker.DatePopupButton.Visible = false;
                        break;
                    case DatePickerType.TimePicker:
                        m_RadDateTimePicker.DateInput.DateFormat = m_RadDateTimePicker.DateInput.DisplayDateFormat = "t";
                        m_RadDateTimePicker.DatePopupButton.Visible = false;
                        m_RadDateTimePicker.TimePopupButton.Visible = true;
                        break;
                }
                ViewState["Type"] = value;
            }
        }

        /// <summary>
        /// Gets the value indicating the selected date equals to MinDate.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEmpty
        {
            get { return (SelectedDate == MinDate); }
        }

        /// <summary>
        /// Gets or sets the calendar that will be shared among several date pickers.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public RadCalendar SharedCalendar
        {
            get
            {
                this.EnsureChildControls();
                return m_RadDateTimePicker.SharedCalendar;
            }
            set
            {
                this.EnsureChildControls();
                m_RadDateTimePicker.SharedCalendar = value;
                if (value != null) value.PreRender += new EventHandler(RadWebControl_PreRender);
            }
        }

        /// <summary>
        /// Gets or sets the time view that will be shared among several date pickers.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RadTimeView SharedTimeView
        {
            get
            {
                this.EnsureChildControls();
                return m_RadDateTimePicker.SharedTimeView;
            }
            set
            {
                this.EnsureChildControls();
                m_RadDateTimePicker.SharedTimeView = value;
                if (value != null) value.PreRender += new EventHandler(RadWebControl_PreRender);
            }
        }

        /// <summary>
        /// Gets or sets the control width.
        /// </summary>
        public Unit Width
        {
            get
            {
                object obj = ViewState["Width"];
                return ((obj == null) ? Unit.Empty : (Unit)obj);
            }
            set { ViewState["Width"] = value; }
        }

        #endregion

        #region Events

        /// <summary>
        /// The event is raised when the date changes between posts to the server.
        /// </summary>
        [Category("Misc")]
        public event SelectedDateChangedEventHandler SelectedDateChanged;

        #endregion

        #region Private Methods

        private void OnSelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
        {
            if (SelectedDateChanged != null) SelectedDateChanged(sender, e);
        }

        private void RadWebControl_PreRender(object sender, EventArgs e)
        {
            RadWebControl ctl = sender as RadWebControl;
            if (ctl != null) ctl.Skin = "Default";
        }

        private void ResetToDatePickerType()
        {
            m_RadDateTimePicker.DateInput.DateFormat = "d";
            m_RadDateTimePicker.DatePopupButton.Visible = true;
            m_RadDateTimePicker.TimePopupButton.Visible = false;
            m_RadDateTimePicker.Calendar.Visible = true;
        }

        #endregion

        #region Internal Methods

        protected internal void SetDateInputValue(string value)
        {
            this.EnsureChildControls();
            m_RadDateTimePicker.DateInput.Attributes["value"] = value;
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Sets focus to a control.
        /// </summary>
        public override void Focus()
        {
            EnsureChildControls();
            m_RadDateTimePicker.Focus();
        }

        /// <summary>
        /// Creates child controls.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            m_RadDateTimePicker = new RadDateTimePicker();
            m_RadDateTimePicker.Skin = m_RadDateTimePicker.Calendar.Skin = "Default";
            base.RequiredFieldValidator.ControlToValidate = m_RadDateTimePicker.ID = "rdp";
            m_RadDateTimePicker.DateInput.EnableEmbeddedBaseStylesheet = false;
            m_RadDateTimePicker.DateInput.EnableEmbeddedSkins = false;
            m_RadDateTimePicker.DatePopupButton.ToolTip = string.Empty;
            m_RadDateTimePicker.TimePopupButton.ToolTip = string.Empty;
            m_RadDateTimePicker.SelectedDateChanged += new SelectedDateChangedEventHandler(OnSelectedDateChanged);
            ResetToDatePickerType();

            Controls.Add(m_RadDateTimePicker);
        }

        /// <summary>
        /// Raises the PreRender event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (ReadOnly)
                m_RadDateTimePicker.DatePopupButton.Attributes["onclick"] = m_RadDateTimePicker.TimePopupButton.Attributes["onclick"] = "return false;";

            RadCalendar calendar = ((this.SharedCalendar == null) ? m_RadDateTimePicker.Calendar : this.SharedCalendar);
            calendar.ShowRowHeaders = false;

            if (this.Width.IsEmpty)
            {
                if (this.Theme == Pages.MasterPageTheme.Modern)
                    m_RadDateTimePicker.Width = Unit.Pixel(180);
            }
            else
                m_RadDateTimePicker.Width = this.Width;
        }

        /// <summary>
        /// Renders control.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            EnsureChildControls();

            if (this.DesignMode)
                m_RadDateTimePicker.TimeView.Visible = false;
            m_RadDateTimePicker.DateInput.Skin = "None";

            if (this.Theme == Pages.MasterPageTheme.Modern)
            {
                m_RadDateTimePicker.DateInput.EnabledStyle.CssClass
                    = m_RadDateTimePicker.DateInput.HoveredStyle.CssClass
                    = m_RadDateTimePicker.DateInput.FocusedStyle.CssClass
                    = string.Empty;

                m_RadDateTimePicker.CssClass = "DatePicker_Modern";
                m_RadDateTimePicker.DateInput.InvalidStyle.CssClass = "Invalid";
                m_RadDateTimePicker.DateInput.Width = Unit.Percentage(100);
            }

            base.Render(writer);
        }

        #endregion

        #region Public Methods

        public void Clear()
        {
            this.EnsureChildControls();
            m_RadDateTimePicker.Clear();
        }

        #endregion
    }
}
