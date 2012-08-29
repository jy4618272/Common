using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Micajah.Common.Properties;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls
{
    [ToolboxData("<{0}:DateRange runat=server></{0}:DateRange>")]
    [DefaultEvent("SelectedIndexChanged")]
    [ParseChildren(true)]
    [PersistChildren(false)]
    public class DateRange : Control, INamingContainer, IThemeable
    {
        #region Members

        private const StandardDateRangeParts DefaultDateRangeParts = (StandardDateRangeParts.Custom | StandardDateRangeParts.Past | StandardDateRangeParts.Present);

        private RadDatePicker m_DateStartPicker;
        private RadDatePicker m_DateEndPicker;
        private RadCalendar m_RadCalendar;
        private DropDownList m_DateRange;
        private Label m_StartLabel;
        private Label m_EndLabel;
        private Label m_RangeLabel;
        private CustomValidator m_Validator;
        private bool m_ChildControlsCreated;
        private string m_StartupScript;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the tab index of the control.
        /// The default is 0, which indicates that this property is not set.
        /// </summary>
        [Category("Accessibility")]
        [Description("The tab index of the control.")]
        [DefaultValue(typeof(short), "0")]
        public short TabIndex
        {
            get
            {
                EnsureChildControls();
                return m_DateStartPicker.TabIndex;
            }
            set
            {
                EnsureChildControls();
                m_DateStartPicker.TabIndex = value;
                m_DateEndPicker.TabIndex = value;
                m_DateRange.TabIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets the start date of the range.
        /// </summary>
        [Category("Appearance")]
        [Description("The start date of the range.")]
        [DefaultValue(typeof(DateTime), "1/1/1980")]
        [Editor(typeof(DateTimeEditor), typeof(UITypeEditor))]
        public DateTime DateStart
        {
            get
            {
                EnsureChildControls();
                return (m_DateStartPicker.SelectedDate.HasValue ? m_DateStartPicker.SelectedDate.Value.Date : new DateTime(1980, 1, 1));
            }
            set
            {
                if (this.DateRangeSelected == StandardDateRange.Custom)
                {
                    EnsureChildControls();
                    m_DateStartPicker.SelectedDate = value;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the start date is default.
        /// </summary>
        [Browsable(false)]
        public bool DateStartIsDefault
        {
            get { return (this.DateStart == new DateTime(1980, 1, 1, 0, 0, 0)); }
        }

        /// <summary>
        /// Gets or sets the end date of the range.
        /// </summary>
        [Category("Appearance")]
        [Description("The end date of the range.")]
        [DefaultValue(typeof(DateTime), "1/1/1980")]
        [Editor(typeof(DateTimeEditor), typeof(UITypeEditor))]
        public DateTime DateEnd
        {
            get
            {
                EnsureChildControls();
                DateTime date = (m_DateEndPicker.SelectedDate.HasValue ? m_DateEndPicker.SelectedDate.Value : new DateTime(1980, 1, 1));
                return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            }
            set
            {
                if (this.DateRangeSelected == StandardDateRange.Custom)
                {
                    EnsureChildControls();
                    m_DateEndPicker.SelectedDate = value;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the end date is default.
        /// </summary>
        [Browsable(false)]
        public bool DateEndIsDefault
        {
            get { return (this.DateEnd == new DateTime(1980, 1, 1, 23, 59, 59)); }
        }

        /// <summary>
        /// Gets or sets the selected standard range.
        /// </summary>
        [Category("Appearance")]
        [Description("The selected standard range.")]
        [DefaultValue(StandardDateRange.Custom)]
        public StandardDateRange DateRangeSelected
        {
            get
            {
                EnsureChildControls();
                int range = 0;
                if (Int32.TryParse(m_DateRange.SelectedValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out range)) return (StandardDateRange)range;
                return StandardDateRange.Custom;
            }
            set
            {
                EnsureChildControls();

                int range = (int)value;
                m_DateRange.SelectedValue = range.ToString(CultureInfo.InvariantCulture);
                if (Int32.TryParse(m_DateRange.SelectedValue, out range)) value = (StandardDateRange)range;

                if (value != StandardDateRange.Custom)
                {
                    m_DateStartPicker.SelectedDate = GetRangeStartDate(value);
                    m_DateEndPicker.SelectedDate = GetRangeEndDate(value);
                }
                else
                {
                    m_DateStartPicker.SelectedDate = null;
                    m_DateEndPicker.SelectedDate = null;
                }
            }
        }

        [Category("Appearance")]
        public DateTime FiscalYearEnd
        {
            get
            {
                object obj = ViewState["FiscalYearEnd"];
                return ((obj == null) ? new DateTime(DateTime.Today.Year, 1, 1) : (DateTime)obj);
            }
            set { ViewState["FiscalYearEnd"] = value; }
        }

        /// <summary>
        /// Gets or sets the format string for the value of the date input controls.
        /// </summary>
        [Category("Behavior")]
        [Description("The format string for the value of the date input controls.")]
        [DefaultValue("d")]
        public string DateFormat
        {
            get
            {
                EnsureChildControls();
                return m_DateStartPicker.DateInput.DateFormat;
            }
            set
            {
                EnsureChildControls();
                m_DateStartPicker.DateInput.DateFormat = m_DateEndPicker.DateInput.DateFormat = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is enabled.
        /// </summary>
        [Category("Behavior")]
        [Description("Whether the control is enabled.")]
        [DefaultValue(true)]
        public bool Enabled
        {
            get
            {
                EnsureChildControls();
                return m_DateRange.Enabled;
            }
            set
            {
                EnsureChildControls();
                m_StartLabel.Enabled = m_EndLabel.Enabled = m_RangeLabel.Enabled
                    = m_DateStartPicker.DateInput.Enabled = m_DateEndPicker.DateInput.Enabled
                    = m_DateStartPicker.DatePopupButton.Enabled = m_DateEndPicker.DatePopupButton.Enabled
                    = m_DateRange.Enabled
                    = value;
            }
        }

        [Category("Behavior")]
        [Editor(typeof(Telerik.Web.Design.Common.FlagEnumUIEditor), typeof(UITypeEditor))]
        [DefaultValue(DefaultDateRangeParts)]
        public StandardDateRangeParts DateRangeParts
        {
            get
            {
                object obj = ViewState["DateRangeParts"];
                return ((obj == null) ? DefaultDateRangeParts : (StandardDateRangeParts)obj);
            }
            set
            {
                EnsureChildControls();
                ViewState["DateRangeParts"] = value;
                FillDateRange();
                DateRangeSelected = DateRangeSelected;
            }
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
                EnsureChildControls();
                return m_Validator.ValidationGroup;
            }
            set
            {
                EnsureChildControls();
                m_Validator.ValidationGroup = value;
            }
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
                object obj = ViewState["Required"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["Required"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control state automatically posts back to the server when clicked.
        /// </summary>
        [Category("Behavior")]
        [Description("Automatically postsback to the server after selection is changed.")]
        [DefaultValue(false)]
        public bool AutoPostBack
        {
            get
            {
                EnsureChildControls();
                return m_DateRange.AutoPostBack;
            }
            set
            {
                EnsureChildControls();
                m_DateRange.AutoPostBack = value;
            }
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
                return m_DateStartPicker.SharedCalendar;
            }
            set
            {
                this.EnsureChildControls();
                m_DateStartPicker.SharedCalendar = m_DateEndPicker.SharedCalendar = value;
                if (value != null) value.PreRender += new EventHandler(RadWebControl_PreRender);
            }
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

        #region Private Properties

        private bool ShowDateRange
        {
            get { return ((DateRangeParts > 0) && (DateRangeParts != StandardDateRangeParts.Custom)); }
        }

        private string DatesRangesClientScript
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                DateTime Date = new DateTime();
                sb.Append("var DateStart = new Array(); var DateEnd = new Array(); ");
                foreach (StandardDateRange range in Enum.GetValues(typeof(StandardDateRange)))
                {
                    Date = this.GetRangeStartDate((StandardDateRange)range);
                    sb.AppendFormat(CultureInfo.InvariantCulture, "DateStart[{0}] = new Date({1}, {2}, {3}, {4}, {5}, {6}); ", (int)range, Date.Year, Date.Month - 1, Date.Day, Date.Hour, Date.Minute, Date.Second);
                    Date = this.GetRangeEndDate((StandardDateRange)range);
                    sb.AppendFormat(CultureInfo.InvariantCulture, "DateEnd[{0}] = new Date({1}, {2}, {3}, {4}, {5}, {6}); ", (int)range, Date.Year, Date.Month - 1, Date.Day, Date.Hour, Date.Minute, Date.Second);
                }
                sb.Append("\r\n");
                return sb.ToString();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the selection from the list control changes between posts to the server.
        /// </summary>
        [Category("Action")]
        [Description("Fires when the selected index has been changed.")]
        public event EventHandler SelectedIndexChanged;

        #endregion

        #region Private Methods

        private void FillDateRange()
        {
            if (!this.ShowDateRange) return;
            m_DateRange.Items.Clear();

            foreach (StandardDateRange range in Enum.GetValues(typeof(StandardDateRange)))
            {
                int value = (int)range;
                bool add = (((DateRangeParts | StandardDateRangeParts.Custom) == DateRangeParts) && (range == StandardDateRange.Custom));
                add = add || (((DateRangeParts | StandardDateRangeParts.Past) == DateRangeParts) && (value <= (int)StandardDateRange.LastWeek));
                add = add || (((DateRangeParts | StandardDateRangeParts.Present) == DateRangeParts)
                    && (value >= (int)StandardDateRange.Today) && (value <= (int)StandardDateRange.ThisFiscalYear));
                add = add || (((DateRangeParts | StandardDateRangeParts.Future) == DateRangeParts) && (value >= (int)StandardDateRange.NextWeek));

                if (add)
                {
                    string text = string.Empty;
                    if (range != StandardDateRange.Custom)
                    {
                        if (this.DesignMode)
                            text = range.ToString();
                        else
                            text = Resources.ResourceManager.GetString("StandardDateRange_" + range.ToString());
                    }
                    m_DateRange.Items.Add(new ListItem(text, value.ToString(CultureInfo.InvariantCulture)));
                }
            }

            ListItem item = m_DateRange.Items.FindByValue(((int)StandardDateRange.Custom).ToString(CultureInfo.InvariantCulture)) as ListItem;
            if (item != null) item.Selected = true;
        }

        private void DateRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedIndexChanged != null) SelectedIndexChanged(sender, e);
        }

        private void RadWebControl_PreRender(object sender, EventArgs e)
        {
            RadWebControl ctl = sender as RadWebControl;
            if (ctl != null) ctl.Skin = "Default";
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Determines whether the server control contains child controls. If it does not, it creates child controls.
        /// </summary>
        protected override void EnsureChildControls()
        {
            if (!m_ChildControlsCreated) CreateChildControls();
        }

        /// <summary>
        /// Creates child controls.
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();

            m_StartLabel = new Label();
            m_EndLabel = new Label();
            m_RangeLabel = new Label();

            m_DateStartPicker = new RadDatePicker();
            m_DateStartPicker.Skin = "Default";
            m_DateStartPicker.ID = "dsp";
            m_DateStartPicker.EnableEmbeddedBaseStylesheet = false;
            m_DateStartPicker.EnableEmbeddedSkins = false;

            m_DateEndPicker = new RadDatePicker();
            m_DateEndPicker.Skin = "Default";
            m_DateEndPicker.ID = "dep";
            m_DateEndPicker.EnableEmbeddedBaseStylesheet = false;
            m_DateEndPicker.EnableEmbeddedSkins = false;

            m_DateStartPicker.DatePopupButton.ToolTip = m_DateEndPicker.DatePopupButton.ToolTip = string.Empty;

            m_DateRange = new DropDownList();
            m_DateRange.ID = "dr";
            m_DateRange.SelectedIndexChanged += new EventHandler(DateRange_SelectedIndexChanged);

            m_Validator = new CustomValidator();
            m_Validator.ID = "cust";
            m_Validator.ClientValidationFunction = "DateRange_Validate";
            m_Validator.Display = ValidatorDisplay.Dynamic;
            m_Validator.ForeColor = System.Drawing.Color.Empty;
            m_Validator.CssClass = "Error";

            FillDateRange();

            Controls.Add(m_StartLabel);
            Controls.Add(m_EndLabel);
            Controls.Add(m_RangeLabel);
            Controls.Add(m_DateStartPicker);
            Controls.Add(m_DateEndPicker);
            Controls.Add(m_DateRange);
            Controls.Add(m_Validator);

            m_ChildControlsCreated = true;
        }

        /// <summary>
        /// Register client script.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ScriptManager.RegisterClientScriptInclude(this.Page, this.Page.GetType(), "DateRangeClientScripts", ResourceProvider.GetResourceUrl("Scripts.DateRange.js", true));

            if (this.ShowDateRange)
            {
                m_DateRange.Attributes["onchange"] = "DateRange_OnChange('" + m_DateRange.ClientID + "', '" + m_DateStartPicker.ClientID + "', '" + m_DateEndPicker.ClientID + "', '" + m_Validator.ClientID + "');";

                m_StartupScript = "Sys.Application.add_load(function() { DateRange_OnChange('" + m_DateRange.ClientID + "', '" + m_DateStartPicker.ClientID + "', '" + m_DateEndPicker.ClientID + "', ''); });\r\n";

                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "DatesRangesClientScript", DatesRangesClientScript, true);
            }

            if (this.SharedCalendar == null)
            {
                m_RadCalendar = new RadCalendar();
                m_RadCalendar.ID = "sc";
                m_DateStartPicker.SharedCalendar = m_DateEndPicker.SharedCalendar = m_RadCalendar;
                m_RadCalendar.Skin = "Default";
                this.Controls.Add(m_RadCalendar);
            }

            m_Validator.Attributes["dateStartPickerId"] = m_DateStartPicker.ClientID;
            m_Validator.Attributes["dateEndPickerId"] = m_DateEndPicker.ClientID;
            m_Validator.Attributes["required"] = (this.Required ? "true" : "false");
            m_Validator.Attributes["requiredErrorMessage"] = Resources.DateRange_RequiredValidator_ErrorMessage;
            m_Validator.Attributes["compareErrorMessage"] = Resources.DateRange_CompareValidator_ErrorMessage;
        }

        /// <summary>
        /// Renders control.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (writer == null) return;

            EnsureChildControls();

            m_DateStartPicker.DateInput.Skin = m_DateEndPicker.DateInput.Skin = "None";
            if (this.Theme == MasterPageTheme.Modern)
            {
                m_DateStartPicker.DateInput.EnabledStyle.CssClass
                  = m_DateStartPicker.DateInput.HoveredStyle.CssClass
                  = m_DateStartPicker.DateInput.FocusedStyle.CssClass
                  = m_DateEndPicker.DateInput.EnabledStyle.CssClass
                   = m_DateEndPicker.DateInput.HoveredStyle.CssClass
                   = m_DateEndPicker.DateInput.FocusedStyle.CssClass
                  = string.Empty;

                m_DateStartPicker.CssClass = m_DateEndPicker.CssClass = "DatePicker_Modern";

                m_DateStartPicker.DateInput.InvalidStyle.CssClass = m_DateEndPicker.DateInput.InvalidStyle.CssClass = "Invalid";
            }

            m_StartLabel.Text = Resources.DateRange_StartDateLabel_Text;
            m_EndLabel.Text = Resources.DateRange_EndDateLabel_Text;
            m_RangeLabel.Text = Resources.DateRange_DateRangeLabel_Text;

            if (DesignMode)
            {
                m_Validator.ErrorMessage = Resources.DateRange_CompareValidator_ErrorMessage;
                if (Required)
                    m_Validator.ErrorMessage = Resources.DateRange_RequiredValidator_ErrorMessage + Resources.DateRange_ErrorMessage_Separator + m_Validator.ErrorMessage;
            }
            else
            {
                if (m_RadCalendar != null) m_RadCalendar.RenderControl(writer);
            }

            if (this.Theme != MasterPageTheme.Modern)
                BaseValidatedControl.RenderValidatedControlBeginTag(Required, writer);

            writer.RenderBeginTag(HtmlTextWriterTag.Table); // Table
            writer.RenderBeginTag(HtmlTextWriterTag.Tr); // Tr1
            if (this.Theme == MasterPageTheme.Modern)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "172px");
            writer.RenderBeginTag(HtmlTextWriterTag.Td); //Td1
            m_StartLabel.RenderControl(writer);
            writer.RenderEndTag(); // Td1
            if (this.Theme == MasterPageTheme.Modern)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "172px");
            writer.RenderBeginTag(HtmlTextWriterTag.Td); // Td2
            m_EndLabel.RenderControl(writer);
            writer.RenderEndTag(); // Td2

            if (ShowDateRange)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td); // Td3
                m_RangeLabel.RenderControl(writer);
                writer.RenderEndTag(); // Td3
            }

            writer.RenderEndTag(); // Tr1

            writer.RenderBeginTag(HtmlTextWriterTag.Tr); // Tr2
            writer.RenderBeginTag(HtmlTextWriterTag.Td); // Td4
            m_DateStartPicker.RenderControl(writer);
            writer.RenderEndTag(); // Td4
            writer.RenderBeginTag(HtmlTextWriterTag.Td); // Td5
            m_DateEndPicker.RenderControl(writer);
            writer.RenderEndTag(); // Td5

            if (ShowDateRange)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td); //Td6
                m_DateRange.RenderControl(writer);
                writer.RenderEndTag(); // Td6
            }

            writer.RenderEndTag(); // Tr2
            writer.RenderEndTag(); // Table

            if (this.Theme != MasterPageTheme.Modern)
                BaseValidatedControl.RenderValidatedControlMiddleTag(true, writer);

            m_Validator.RenderControl(writer);

            if (this.Theme != MasterPageTheme.Modern)
                BaseValidatedControl.RenderValidatedControlEndTag(true, writer);

            if (m_StartupScript != null) ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, m_StartupScript, true);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the start date of specified standard date range.
        /// </summary>
        /// <param name="range">Specifies the standard date range. One of StandardDateRange values.</param>
        /// <returns>The DateTime value that represents the start date of specified standard date range.</returns>
        public DateTime GetRangeStartDate(StandardDateRange range)
        {
            DateTime Today = DateTime.Today;
            int DayOfWeek = (int)Today.AddDays(-1).DayOfWeek;
            DateTime ThisMonth = new DateTime(Today.Year, Today.Month, 1);
            DateTime ThisYear = new DateTime(Today.Year, 1, 1);
            DateTime ThisFiscalYear = new DateTime(((Today.Month >= this.FiscalYearEnd.Month) ? Today.Year : Today.AddYears(-1).Year), this.FiscalYearEnd.Month, this.FiscalYearEnd.Day);

            switch (range)
            {
                case StandardDateRange.Today:
                    return Today;
                case StandardDateRange.ThisWeek:
                    return Today.AddDays(-DayOfWeek);
                case StandardDateRange.ThisMonth:
                    return ThisMonth;
                case StandardDateRange.ThisYear:
                    return ThisYear;
                case StandardDateRange.ThisFiscalYear:
                    return ThisFiscalYear;
                case StandardDateRange.LastWeek:
                    return Today.AddDays(-DayOfWeek - 7);
                case StandardDateRange.LastMonth:
                    return ThisMonth.AddMonths(-1);
                case StandardDateRange.LastYear:
                    return ThisYear.AddYears(-1);
                case StandardDateRange.LastFiscalYear:
                    return ThisFiscalYear.AddYears(-1);
                case StandardDateRange.Rolling30Days:
                    return Today.AddDays(-30);
                case StandardDateRange.Rolling90Days:
                    return Today.AddDays(-90);
                case StandardDateRange.Rolling365Days:
                    return Today.AddDays(-365);
                case StandardDateRange.NextWeek:
                    return Today.AddDays(-DayOfWeek + 7);
                case StandardDateRange.NextMonth:
                    return ThisMonth.AddMonths(1);
                case StandardDateRange.NextYear:
                    return ThisYear.AddYears(1);
                case StandardDateRange.NextFiscalYear:
                    return ThisFiscalYear.AddYears(1);
                case StandardDateRange.Next30Days:
                case StandardDateRange.Next90Days:
                case StandardDateRange.Next365Days:
                default:
                    return Today;
            }
        }

        /// <summary>
        /// Gets the end date of specified standard date range.
        /// </summary>
        /// <param name="range">Specifies the standard date range. One of StandardDateRange values.</param>
        /// <returns>The DateTime value that represents the end date of specified standard date range.</returns>
        public DateTime GetRangeEndDate(StandardDateRange range)
        {
            DateTime Today = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);
            int DayOfWeek = (int)Today.AddDays(-1).DayOfWeek;
            DateTime ThisMonth = new DateTime(Today.Year, Today.Month, 1);
            DateTime ThisYear = new DateTime(Today.Year, 1, 1);
            DateTime ThisFiscalYear = new DateTime(((Today.Month >= this.FiscalYearEnd.Month) ? Today.Year : Today.AddYears(-1).Year), this.FiscalYearEnd.Month, this.FiscalYearEnd.Day);

            switch (range)
            {
                case StandardDateRange.Today:
                    return Today;
                case StandardDateRange.ThisWeek:
                    return Today.AddDays(-DayOfWeek + 6);
                case StandardDateRange.ThisMonth:
                    return ThisMonth.AddMonths(1).AddDays(-1);
                case StandardDateRange.ThisYear:
                    return ThisYear.AddYears(1).AddDays(-1);
                case StandardDateRange.ThisFiscalYear:
                    return ThisFiscalYear.AddYears(1).AddDays(-1);
                case StandardDateRange.LastWeek:
                    return Today.AddDays(-DayOfWeek - 1);
                case StandardDateRange.LastMonth:
                    return ThisMonth.AddDays(-1);
                case StandardDateRange.LastYear:
                    return ThisYear.AddDays(-1);
                case StandardDateRange.LastFiscalYear:
                    return ThisFiscalYear.AddDays(-1);
                case StandardDateRange.NextWeek:
                    return Today.AddDays(-DayOfWeek + 13);
                case StandardDateRange.NextMonth:
                    return ThisMonth.AddMonths(2).AddDays(-1);
                case StandardDateRange.NextYear:
                    return ThisYear.AddYears(2).AddDays(-1);
                case StandardDateRange.NextFiscalYear:
                    return ThisFiscalYear.AddYears(2).AddDays(-1);
                case StandardDateRange.Next30Days:
                    return Today.AddDays(30);
                case StandardDateRange.Next90Days:
                    return Today.AddDays(90);
                case StandardDateRange.Next365Days:
                    return Today.AddDays(365);
                case StandardDateRange.Rolling30Days:
                case StandardDateRange.Rolling90Days:
                case StandardDateRange.Rolling365Days:
                default:
                    return Today;
            }
        }

        #endregion
    }
}
