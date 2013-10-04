using System;
using System.ComponentModel;
using System.Globalization;
using Micajah.Common.Bll.RecurringSchedule;
using Micajah.Common.Security;

namespace Micajah.Common.WebControls
{
    #region EventArgs Class

    public class RecurringScheduleEventArgs : EventArgs
    {
        public Guid RecurringScheduleId { get; private set; }
        public Guid OrganizationId { get; private set; }
        public Guid? InstanceId { get; private set; }
        public string Name { get; private set; }
        public string LocalEntityType { get; private set; }
        public string LocalEntityId { get; private set; }
        public string RecurrenceRule { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public RecurringScheduleEventArgs(
            Guid recurringScheduleId,
                Guid organizationId,
                Guid? instanceId,
                string localEntityType,
                string localEntityId,
                string name,
                DateTime start,
                DateTime end,
                string recurrenceRule)
        {
            this.RecurringScheduleId = recurringScheduleId;
            this.OrganizationId = organizationId;
            this.InstanceId = instanceId;
            this.LocalEntityType = localEntityType;
            this.LocalEntityId = localEntityId;
            this.Name = name;
            this.StartDate = start;
            this.EndDate = end;
            this.RecurrenceRule = recurrenceRule;
        }
    }

    #endregion

    public class RecurrenceScheduleInternalControl : System.Web.UI.UserControl
    {
        #region Members

        protected Micajah.Common.WebControls.TextBox TextBoxName;
        protected Micajah.Common.WebControls.TextBox TextBoxLocalEntityId;
        protected Micajah.Common.WebControls.ComboBox ComboBoxLocalEntityType;
        protected Micajah.Common.WebControls.DatePicker DatePickerStartDate;
        protected Micajah.Common.WebControls.DatePicker DatePickerEndDate;
        protected System.Web.UI.WebControls.CheckBox CheckBoxAllDay;
        protected System.Web.UI.WebControls.CheckBox CheckBoxRecurrence;
        protected System.Web.UI.WebControls.RadioButtonList RadioButtonListRecurringOption;
        protected Telerik.Web.UI.RadNumericTextBox NumericTextBoxHourly;
        protected Telerik.Web.UI.RadNumericTextBox NumericTextBoxDaily;
        protected System.Web.UI.WebControls.RadioButton RadioButtonListRecurringOptionDaily_Everyday;
        protected System.Web.UI.WebControls.RadioButton RadioButtonListRecurringOptionDaily_EveryWeekday;
        protected Telerik.Web.UI.RadNumericTextBox NumericTextBoxWeekly;
        protected System.Web.UI.WebControls.CheckBox CheckBoxWeeklySunday;
        protected System.Web.UI.WebControls.CheckBox CheckBoxWeeklyMonday;
        protected System.Web.UI.WebControls.CheckBox CheckBoxWeeklyTuesday;
        protected System.Web.UI.WebControls.CheckBox CheckBoxWeeklyWednesday;
        protected System.Web.UI.WebControls.CheckBox CheckBoxWeeklyThursday;
        protected System.Web.UI.WebControls.CheckBox CheckBoxWeeklyFriday;
        protected System.Web.UI.WebControls.CheckBox CheckBoxWeeklySaturday;
        protected System.Web.UI.WebControls.RadioButton RadioButtonListRecurringOptionMonthly_Day;
        protected Telerik.Web.UI.RadNumericTextBox NumericTextBoxDayInMonth;
        protected Telerik.Web.UI.RadNumericTextBox NumericTextBoxMonth;
        protected System.Web.UI.WebControls.RadioButton RadioButtonListRecurringOptionMonthly_The;
        protected Telerik.Web.UI.RadComboBox ComboBoxMonthly_NumberDayInWeek;
        protected Telerik.Web.UI.RadComboBox ComboBoxMonthly_DayInWeek;
        protected Telerik.Web.UI.RadNumericTextBox NumericTextBoxMonth_Destine;
        protected System.Web.UI.WebControls.RadioButton RadioButtonListRecurringOptionYearly_Every;
        protected Telerik.Web.UI.RadComboBox ComboBoxYearly_EveryMonth;
        protected Telerik.Web.UI.RadNumericTextBox NumericTextBoxDayOfMonth;
        protected System.Web.UI.WebControls.RadioButton RadioButtonListRecurringOptionYearly_The;
        protected Telerik.Web.UI.RadComboBox ComboBoxYearly_NumberDayInMonth;
        protected Telerik.Web.UI.RadComboBox ComboBoxYearly_DayInMonth;
        protected Telerik.Web.UI.RadComboBox ComboBoxYearly_TheMonth;
        protected System.Web.UI.WebControls.RadioButton RadioButtonEndDate_NoEnd;
        protected System.Web.UI.WebControls.RadioButton RadioButtonEndDate_EndAfter;
        protected Telerik.Web.UI.RadNumericTextBox NumericTextBoxEndDate_Number;
        protected System.Web.UI.WebControls.RadioButton RadioButtonEndDate_EndBy;
        protected Micajah.Common.WebControls.DatePicker DatePickerRecurringOptionEndDate;
        protected System.Web.UI.WebControls.Button ButtonSave;
        protected System.Web.UI.WebControls.PlaceHolder ButtonsSeparator;
        protected System.Web.UI.WebControls.LinkButton ButtonCancel;
        protected System.Web.UI.HtmlControls.HtmlGenericControl PanelRecurrence;
        protected System.Web.UI.HtmlControls.HtmlGenericControl PanelOption;
        protected System.Web.UI.HtmlControls.HtmlGenericControl PanelOption_Hourly;
        protected System.Web.UI.HtmlControls.HtmlGenericControl PanelOption_Daily;
        protected System.Web.UI.HtmlControls.HtmlGenericControl PanelOption_Weekly;
        protected System.Web.UI.HtmlControls.HtmlGenericControl PanelOption_Monthly;
        protected System.Web.UI.HtmlControls.HtmlGenericControl PanelOption_Yearly;

        #endregion

        #region Events

        public event EventHandler<EventArgs> Cancel;
        public event EventHandler<CancelEventArgs> Updating;
        public event EventHandler<RecurringScheduleEventArgs> Updated;

        #endregion

        #region Properties

        public Guid RecurringScheduleId
        {
            get
            {
                return (Guid)(ViewState["RecurringScheduleId"] ?? Guid.Empty);
            }
            set
            {
                ViewState["RecurringScheduleId"] = value;
            }
        }

        public DateTime Start
        {
            get
            {
                return DatePickerStartDate.SelectedDate;
            }
            set
            {
                DatePickerStartDate.SelectedDate = value; // DateHelper.AssumeUtc(value);
            }
        }

        public DateTime End
        {
            get
            {
                return DatePickerEndDate.SelectedDate;
            }
            set
            {
                DatePickerEndDate.SelectedDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the date and time format.
        /// </summary>
        public string DateFormat
        {
            get { return DatePickerStartDate.DateFormat; }
            set { DatePickerStartDate.DateFormat = DatePickerEndDate.DateFormat = value; }
        }

        public TimeSpan Duration
        {
            get
            {
                return this.End - this.Start;
            }
        }

        public string Name
        {
            get
            {
                return TextBoxName.Text;
            }
            set
            {
                TextBoxName.Text = value;
            }
        }

        public string RecurrenceRule
        {
            get
            {
                string ruleStr = string.Empty;
                RecurrenceRule rrule = Micajah.Common.Bll.RecurringSchedule.RecurrenceRule.FromPatternAndRange(this.Pattern, this.Range);
                if (rrule != null)
                    ruleStr = rrule.ToString();
                return ruleStr;
            }
            set
            {
                RecurrenceRule rrule;
                CheckBoxRecurrence.Checked = false;
                PanelRecurrence.Style["display"] = "none";
                if (!Micajah.Common.Bll.RecurringSchedule.RecurrenceRule.TryParse(value, out rrule))
                    return;

                CheckBoxAllDay.Checked = rrule.Range.EventDuration.Equals(TimeSpan.FromDays(1.0));

                if (rrule.Pattern.Frequency != RecurrenceFrequency.None)
                {
                    CheckBoxRecurrence.Checked = true;
                    PanelRecurrence.Style["display"] = "block";
                }
                PanelOption_Hourly.Style["display"] = "none";
                PanelOption_Daily.Style["display"] = "none";
                PanelOption_Weekly.Style["display"] = "none";
                PanelOption_Monthly.Style["display"] = "none";
                PanelOption_Yearly.Style["display"] = "none";

                string interval = rrule.Pattern.Interval.ToString(CultureInfo.CurrentCulture);
                int mask = (int)rrule.Pattern.DaysOfWeekMask;

                switch (rrule.Pattern.Frequency)
                {
                    case RecurrenceFrequency.Hourly:
                        RadioButtonListRecurringOption.SelectedValue = "Hourly";
                        PanelOption_Hourly.Style["display"] = "block";
                        NumericTextBoxHourly.Text = interval;
                        break;

                    case RecurrenceFrequency.Daily:
                        RadioButtonListRecurringOption.SelectedValue = "Daily";
                        PanelOption_Daily.Style["display"] = "block";

                        if (rrule.Pattern.DaysOfWeekMask == RecurrenceDay.WeekDays)
                        {
                            RadioButtonListRecurringOptionDaily_EveryWeekday.Checked = true;
                            RadioButtonListRecurringOptionDaily_Everyday.Checked = false;
                        }
                        else
                        {
                            RadioButtonListRecurringOptionDaily_EveryWeekday.Checked = false;
                            RadioButtonListRecurringOptionDaily_Everyday.Checked = true;
                            NumericTextBoxDaily.Text = interval;
                        }
                        break;

                    case RecurrenceFrequency.Weekly:
                        RadioButtonListRecurringOption.SelectedValue = "Weekly";
                        PanelOption_Weekly.Style["display"] = "block";

                        NumericTextBoxWeekly.Text = interval;

                        CheckBoxWeeklyMonday.Checked = (RecurrenceDay.Monday & rrule.Pattern.DaysOfWeekMask) == RecurrenceDay.Monday;
                        CheckBoxWeeklyTuesday.Checked = (RecurrenceDay.Tuesday & rrule.Pattern.DaysOfWeekMask) == RecurrenceDay.Tuesday;
                        CheckBoxWeeklyWednesday.Checked = (RecurrenceDay.Wednesday & rrule.Pattern.DaysOfWeekMask) == RecurrenceDay.Wednesday;
                        CheckBoxWeeklyThursday.Checked = (RecurrenceDay.Thursday & rrule.Pattern.DaysOfWeekMask) == RecurrenceDay.Thursday;
                        CheckBoxWeeklyFriday.Checked = (RecurrenceDay.Friday & rrule.Pattern.DaysOfWeekMask) == RecurrenceDay.Friday;
                        CheckBoxWeeklySaturday.Checked = (RecurrenceDay.Saturday & rrule.Pattern.DaysOfWeekMask) == RecurrenceDay.Saturday;
                        CheckBoxWeeklySunday.Checked = (RecurrenceDay.Sunday & rrule.Pattern.DaysOfWeekMask) == RecurrenceDay.Sunday;
                        break;

                    case RecurrenceFrequency.Monthly:
                        RadioButtonListRecurringOption.SelectedValue = "Monthly";
                        PanelOption_Monthly.Style["display"] = "block";

                        if (0 < rrule.Pattern.DayOfMonth)
                        {
                            RadioButtonListRecurringOptionMonthly_Day.Checked = true;
                            RadioButtonListRecurringOptionMonthly_The.Checked = false;
                            NumericTextBoxDayInMonth.Text = rrule.Pattern.DayOfMonth.ToString(CultureInfo.CurrentCulture);
                            NumericTextBoxMonth.Text = interval;
                        }
                        else
                        {
                            RadioButtonListRecurringOptionMonthly_Day.Checked = false;
                            RadioButtonListRecurringOptionMonthly_The.Checked = true;
                            ComboBoxMonthly_NumberDayInWeek.SelectedValue = rrule.Pattern.DayOrdinal.ToString(CultureInfo.CurrentCulture);
                            ComboBoxMonthly_DayInWeek.SelectedIndex = Array.IndexOf(DateHelper.DayMaskValues(), (mask).ToString(CultureInfo.CurrentCulture));
                            NumericTextBoxMonth_Destine.Text = interval;
                        }
                        break;

                    case RecurrenceFrequency.Yearly:
                        RadioButtonListRecurringOption.SelectedValue = "Yearly";
                        PanelOption_Yearly.Style["display"] = "block";

                        if (0 < rrule.Pattern.DayOfMonth)
                        {
                            RadioButtonListRecurringOptionYearly_Every.Checked = true;
                            RadioButtonListRecurringOptionYearly_The.Checked = false;
                            NumericTextBoxDayOfMonth.Text = rrule.Pattern.DayOfMonth.ToString(CultureInfo.CurrentCulture);
                            ComboBoxYearly_EveryMonth.SelectedIndex = ((int)rrule.Pattern.Month) - 1;
                        }
                        else
                        {
                            RadioButtonListRecurringOptionYearly_Every.Checked = false;
                            RadioButtonListRecurringOptionYearly_The.Checked = true;
                            ComboBoxYearly_NumberDayInMonth.SelectedValue = rrule.Pattern.DayOrdinal.ToString(CultureInfo.CurrentCulture);
                            ComboBoxYearly_DayInMonth.SelectedIndex = Array.IndexOf(DateHelper.DayMaskValues(), (mask).ToString(CultureInfo.CurrentCulture));
                            ComboBoxYearly_TheMonth.SelectedIndex = ((int)rrule.Pattern.Month) - 1;
                        }
                        break;
                }

                bool occurrencesLimit = (rrule.Range.MaxOccurrences != int.MaxValue);
                bool timeLimit = (rrule.Range.RecursUntil != DateTime.MaxValue);

                if (!occurrencesLimit && !timeLimit)
                {
                    RadioButtonEndDate_NoEnd.Checked = true;
                    RadioButtonEndDate_EndAfter.Checked = false;
                    RadioButtonEndDate_EndBy.Checked = false;
                }
                else
                    if (occurrencesLimit)
                    {
                        RadioButtonEndDate_NoEnd.Checked = false;
                        RadioButtonEndDate_EndAfter.Checked = true;
                        RadioButtonEndDate_EndBy.Checked = false;

                        NumericTextBoxEndDate_Number.Text = rrule.Range.MaxOccurrences.ToString(CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        RadioButtonEndDate_NoEnd.Checked = false;
                        RadioButtonEndDate_EndAfter.Checked = false;
                        RadioButtonEndDate_EndBy.Checked = true;

                        DateTime recursUntil = rrule.Range.RecursUntil;
                        if (!IsAllDayAppointment())
                        {
                            recursUntil = recursUntil.AddDays(-1);
                        }
                        DatePickerRecurringOptionEndDate.SelectedDate = recursUntil;
                    }
            }
        }

        public string LocalEntityType
        {
            get
            {
                return ComboBoxLocalEntityType.Text;
            }
            set
            {
                ComboBoxLocalEntityType.Text = value;

            }
        }

        public string LocalEntityId
        {
            get
            {
                return TextBoxLocalEntityId.Text;
            }
            set
            {
                TextBoxLocalEntityId.Text = value;
            }
        }

        public Guid OrganizationId
        {
            get
            {
                return (Guid)(ViewState["OrganizationId"] ?? UserContext.Current.OrganizationId);
            }
            set
            {
                ViewState["OrganizationId"] = value;
            }
        }

        public Guid? InstanceId
        {
            get
            {
                return (Guid?)(ViewState["InstanceId"] ?? ((UserContext.Current == null || UserContext.Current.InstanceId == Guid.Empty) ? new Guid?() : new Guid?(UserContext.Current.InstanceId)));
            }
            set
            {
                ViewState["InstanceId"] = value;
            }
        }

        public DayOfWeek FirstDayOfWeek
        {
            get
            {
                object obj = this.ViewState["FirstDayOfWeek"];
                return ((obj == null) ? DayOfWeek.Monday : (DayOfWeek)obj);
            }
            set { this.ViewState["FirstDayOfWeek"] = value; }
        }

        public RecurrenceFrequency Frequency
        {
            get
            {
                if (CheckBoxRecurrence.Checked)
                    switch (RadioButtonListRecurringOption.SelectedValue)
                    {
                        case "Hourly":
                            return RecurrenceFrequency.Hourly;
                        case "Daily":
                            return RecurrenceFrequency.Daily;
                        case "Weekly":
                            return RecurrenceFrequency.Weekly;
                        case "Monthly":
                            return RecurrenceFrequency.Monthly;
                        case "Yearly":
                            return RecurrenceFrequency.Yearly;
                    }
                return RecurrenceFrequency.None;
            }
        }

        public int Interval
        {
            get
            {
                switch (this.Frequency)
                {
                    case RecurrenceFrequency.Hourly:
                        return int.Parse(NumericTextBoxHourly.Text, CultureInfo.CurrentCulture);

                    case RecurrenceFrequency.Daily:
                        if (RadioButtonListRecurringOptionDaily_Everyday.Checked)
                        {
                            return int.Parse(NumericTextBoxDaily.Text, CultureInfo.CurrentCulture);
                        }
                        break;

                    case RecurrenceFrequency.Weekly:
                        return int.Parse(NumericTextBoxWeekly.Text, CultureInfo.CurrentCulture);

                    case RecurrenceFrequency.Monthly:
                        if (RadioButtonListRecurringOptionMonthly_Day.Checked)
                        {
                            return int.Parse(NumericTextBoxMonth.Text, CultureInfo.CurrentCulture);
                        }

                        return int.Parse(NumericTextBoxMonth_Destine.Text, CultureInfo.CurrentCulture);
                }
                return 0;
            }
        }

        public RecurrenceDay DaysOfWeekMask
        {
            get
            {
                switch (this.Frequency)
                {
                    case RecurrenceFrequency.Daily:
                        return (RadioButtonListRecurringOptionDaily_EveryWeekday.Checked) ? RecurrenceDay.WeekDays : RecurrenceDay.EveryDay;

                    case RecurrenceFrequency.Weekly:
                        RecurrenceDay finalMask = RecurrenceDay.None;
                        finalMask |= CheckBoxWeeklyMonday.Checked ? RecurrenceDay.Monday : finalMask;
                        finalMask |= CheckBoxWeeklyTuesday.Checked ? RecurrenceDay.Tuesday : finalMask;
                        finalMask |= CheckBoxWeeklyWednesday.Checked ? RecurrenceDay.Wednesday : finalMask;
                        finalMask |= CheckBoxWeeklyThursday.Checked ? RecurrenceDay.Thursday : finalMask;
                        finalMask |= CheckBoxWeeklyFriday.Checked ? RecurrenceDay.Friday : finalMask;
                        finalMask |= CheckBoxWeeklySaturday.Checked ? RecurrenceDay.Saturday : finalMask;
                        finalMask |= CheckBoxWeeklySunday.Checked ? RecurrenceDay.Sunday : finalMask;
                        return finalMask;

                    case RecurrenceFrequency.Monthly:
                        if (RadioButtonListRecurringOptionMonthly_The.Checked)
                        {
                            return (RecurrenceDay)Enum.Parse(typeof(RecurrenceDay), ComboBoxMonthly_DayInWeek.SelectedValue);
                        }
                        break;

                    case RecurrenceFrequency.Yearly:
                        if (RadioButtonListRecurringOptionYearly_The.Checked)
                        {
                            return (RecurrenceDay)Enum.Parse(typeof(RecurrenceDay), ComboBoxYearly_DayInMonth.SelectedValue);
                        }
                        break;
                }

                return RecurrenceDay.None;
            }
        }

        private int DayOfMonth
        {
            get
            {
                switch (this.Frequency)
                {
                    case RecurrenceFrequency.Monthly:
                        return (RadioButtonListRecurringOptionMonthly_Day.Checked ? int.Parse(NumericTextBoxDayInMonth.Text, CultureInfo.CurrentCulture) : 0);

                    case RecurrenceFrequency.Yearly:
                        return (RadioButtonListRecurringOptionYearly_Every.Checked ? int.Parse(NumericTextBoxDayOfMonth.Text, CultureInfo.CurrentCulture) : 0);
                }
                return 0;
            }
        }

        private int DayOrdinal
        {
            get
            {
                switch (this.Frequency)
                {
                    case RecurrenceFrequency.Monthly:
                        if (RadioButtonListRecurringOptionMonthly_The.Checked)
                        {
                            return int.Parse(ComboBoxMonthly_NumberDayInWeek.SelectedValue, CultureInfo.CurrentCulture);
                        }
                        break;

                    case RecurrenceFrequency.Yearly:
                        if (RadioButtonListRecurringOptionYearly_The.Checked)
                        {
                            return int.Parse(ComboBoxYearly_NumberDayInMonth.SelectedValue, CultureInfo.CurrentCulture);
                        }
                        break;
                }

                return 0;
            }
        }

        private RecurrenceMonth Month
        {
            get
            {
                if (Frequency == RecurrenceFrequency.Yearly)
                {
                    string selectedMonth;

                    if (RadioButtonListRecurringOptionYearly_Every.Checked)
                    {
                        selectedMonth = ComboBoxYearly_EveryMonth.SelectedValue;
                    }
                    else
                    {
                        selectedMonth = ComboBoxYearly_DayInMonth.SelectedValue;
                    }

                    return (RecurrenceMonth)Enum.Parse(typeof(RecurrenceMonth), selectedMonth);
                }

                return RecurrenceMonth.None;
            }
        }

        private RecurrencePattern Pattern
        {
            get
            {
                if (!CheckBoxRecurrence.Checked)
                {
                    return null;
                }

                RecurrencePattern submittedPattern = new RecurrencePattern();
                submittedPattern.Frequency = Frequency;
                submittedPattern.Interval = Interval;
                submittedPattern.DaysOfWeekMask = DaysOfWeekMask;
                submittedPattern.DayOfMonth = DayOfMonth;
                submittedPattern.DayOrdinal = DayOrdinal;
                submittedPattern.Month = Month;

                if (submittedPattern.Frequency == RecurrenceFrequency.Weekly)
                {
                    submittedPattern.FirstDayOfWeek = this.FirstDayOfWeek;
                }

                return submittedPattern;
            }
        }

        public RecurrenceRange Range
        {
            get
            {
                RecurrenceRange range = new RecurrenceRange();
                if (!(!DatePickerStartDate.IsEmpty && !DatePickerEndDate.IsEmpty)) return range;

                DateTime startDate = DatePickerStartDate.SelectedDate;
                DateTime endDate = DatePickerEndDate.SelectedDate;
                if (CheckBoxAllDay.Checked)
                {
                    startDate = startDate.Date;
                    endDate = endDate.AddDays(1);
                }

                range.Start = startDate;
                range.EventDuration = endDate - startDate;
                range.MaxOccurrences = 0;
                range.RecursUntil = DateTime.MaxValue;

                if (CheckBoxRecurrence.Checked)
                {
                    if (RadioButtonEndDate_EndAfter.Checked)
                    {
                        int maxOccurrences;
                        if (int.TryParse(NumericTextBoxEndDate_Number.Text, out maxOccurrences))
                            range.MaxOccurrences = maxOccurrences;
                    }
                    if (RadioButtonEndDate_EndBy.Checked && !DatePickerRecurringOptionEndDate.IsEmpty)
                    {
                        range.RecursUntil = DatePickerRecurringOptionEndDate.SelectedDate;
                        if (CheckBoxAllDay.Checked)
                            range.RecursUntil = range.RecursUntil.AddDays(1);
                    }
                }
                return range;
            }
        }

        public bool VisibleCancelButton
        {
            get { return ButtonCancel.Visible; }
            set { ButtonCancel.Visible = value; }
        }

        #endregion

        #region Private Methods

        private void LoadResources()
        {
            ButtonSave.Text = MagicForm.GetUpdateButtonText(System.Web.UI.WebControls.DetailsViewMode.Edit, string.Empty, InsertButtonCaptionType.Create, UpdateButtonCaptionType.Save, CloseButtonVisibilityMode.Edit);
            ButtonCancel.Text = Micajah.Common.Properties.Resources.AutoGeneratedButtonsField_CancelButton_Text;
        }

        private bool IsAllDayAppointment()
        {
            DateTime displayStart = this.Start;
            DateTime displayEnd = this.End;
            return displayStart.CompareTo(displayStart.Date) == 0 && displayEnd.CompareTo(displayEnd.Date) == 0;
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadResources();

            if (!IsPostBack)
            {
                DateTime currDate = DateTime.UtcNow;
                DatePickerStartDate.SelectedDate = currDate;
                DatePickerEndDate.SelectedDate = currDate.AddHours(1);
                ComboBoxYearly_EveryMonth.SelectedValue = currDate.ToString("MMMM", new CultureInfo("en-US"));
                NumericTextBoxDayOfMonth.Text = currDate.Day.ToString(CultureInfo.CurrentCulture);
                ComboBoxYearly_TheMonth.SelectedValue = currDate.ToString("MMMM", new CultureInfo("en-US"));
                ComboBoxLocalEntityType.DataSource = Micajah.Common.Bll.Providers.RecurringScheduleProvider.GetEntityTypes(this.OrganizationId, this.InstanceId);
                ComboBoxLocalEntityType.DataBind();
            }

            AutoGeneratedButtonsField.InsertButtonSeparator(ButtonsSeparator);
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            CancelEventArgs cancelArg = new CancelEventArgs(false);
            if (Updating != null)
            {
                Updating(this, cancelArg);
                if (cancelArg.Cancel) return;
            }
            //OrganizationDataSet.RecurringScheduleDataTable dtRecurringSchedule = RecurringScheduleProvider.GetRecurringSchedulesByEntityId(this.OrganizationId, this.InstanceId, this.LocalEntityType, this.LocalEntityId);
            //Guid recurringScheduleId;
            //if (dtRecurringSchedule.Count > 0)
            //{
            //    recurringScheduleId = dtRecurringSchedule[0].RecurringScheduleId;
            //}
            //else
            //{
            //    recurringScheduleId = Guid.NewGuid();
            //}
            RecurringScheduleEventArgs args = new RecurringScheduleEventArgs(
                this.RecurringScheduleId,
                this.OrganizationId,
                this.InstanceId,
                this.LocalEntityType,
                this.LocalEntityId,
                this.Name,
                this.Start,
                this.End,
                this.RecurrenceRule);
            //RecurringScheduleProvider.UpdateRecurringSchedule(
            //    args.RecurringScheduleId,
            //    args.OrganizationId,
            //    args.InstanceId,
            //    args.LocalEntityType,
            //    args.LocalEntityId,
            //    args.Name,
            //    args.StartDate,
            //    args.EndDate,
            //    args.RecurrenceRule,
            //    DateTime.UtcNow,
            //    (UserContext.Current != null ? UserContext.Current.UserId : Guid.Empty),
            //    false);
            if (Updated != null) Updated(this, args);
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            if (Cancel != null) Cancel(this, EventArgs.Empty);
        }

        #endregion

        #region Overriden Methods

        public override void DataBind()
        {
            base.DataBind();

            ComboBoxLocalEntityType.DataSource = Micajah.Common.Bll.Providers.RecurringScheduleProvider.GetEntityTypes(this.OrganizationId, this.InstanceId);
            ComboBoxLocalEntityType.DataBind();
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event and registers the style sheet of the control.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            MagicForm.RegisterStyleSheet(this.Page);
        }

        #endregion
    }
}