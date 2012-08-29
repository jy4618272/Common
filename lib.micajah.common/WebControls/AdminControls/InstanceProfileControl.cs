using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls.AdminControls
{
    public class InstanceProfileControl : BaseEditFormControl
    {
        #region Members

        protected Panel SearchPanel;
        protected ComboBox InstanceList;
        protected ObjectDataSource InstancesDataSource;

        private ComboBox m_UtcOffsetList;
        private ComboBox m_DateFormatList;
        private CheckBoxList m_WorkingDays;

        #endregion

        #region Private Properties

        private ComboBox UtcOffsetList
        {
            get
            {
                if (m_UtcOffsetList == null) m_UtcOffsetList = EditForm.FindControl("UtcOffsetList") as ComboBox;
                return m_UtcOffsetList;
            }
        }

        private ComboBox DateFormatList
        {
            get
            {
                if (m_DateFormatList == null) m_DateFormatList = EditForm.FindControl("DateFormatList") as ComboBox;
                return m_DateFormatList;
            }
        }

        private CheckBoxList WorkingDays
        {
            get
            {
                if (m_WorkingDays == null) m_WorkingDays = EditForm.FindControl("WorkingDaysList") as CheckBoxList;
                return m_WorkingDays;
            }
        }

        #endregion

        #region Private Methods

        private void Redirect()
        {
            RedirectToActionOrStartPage(ActionProvider.ConfigurationPageActionId);
        }

        #endregion

        #region Helpdesk Date Methods

        private static string FormatTimePart(int TimePart)
        {
            if (TimePart < 10) return "0" + TimePart.ToString(CultureInfo.InvariantCulture);
            else return TimePart.ToString(CultureInfo.InvariantCulture);
        }

        private static string DisplayTime(DateTime date, int userHourOffset, int dateFormat, bool omitUtc)
        {
            return DisplayServerBasedTime(date, userHourOffset, 5, dateFormat, omitUtc);
        }

        private static string DisplayServerBasedTime(DateTime date, int userHourOffset, int serverHourOffset, int dateFormat, bool omitUtc)
        {
            date = DateTimeAddHoursOffset(date, userHourOffset);

            string sResult = string.Empty;
            //12:00AM/PM
            if (dateFormat == 0 || dateFormat == 2)
            {
                if (date.Hour > 12) sResult = FormatTimePart(date.Hour - 12) + ":" + FormatTimePart(date.Minute) + "PM";
                else if (date.Hour == 12) sResult = FormatTimePart(date.Hour) + ":" + FormatTimePart(date.Minute) + "PM";
                else if ((date.Hour < 12) && (date.Hour != 0)) sResult = FormatTimePart(date.Hour) + ":" + FormatTimePart(date.Minute) + "AM";
                else if (date.Hour == 0) sResult = "12:" + FormatTimePart(date.Minute) + "AM";
            }
            //24:00 (UTC)
            else if (dateFormat == 1 || dateFormat == 3) sResult = FormatTimePart(date.Hour) + ":" + FormatTimePart(date.Minute);
            if (omitUtc) sResult += " (UTC" + Convert.ToString(userHourOffset - serverHourOffset, CultureInfo.InvariantCulture) + ")";
            return sResult;
        }

        public static DateTime DateTimeAddHoursOffset(DateTime date, int hourOffset)
        {
            TimeSpan _difMin = date - DateTime.MinValue;
            TimeSpan _difMax = DateTime.MaxValue - date;
            if (date == DateTime.MinValue) return DateTime.MinValue;
            else if (date == DateTime.MaxValue) return DateTime.MaxValue;
            else if (hourOffset < 0 && _difMin.TotalHours < Math.Abs(hourOffset)) return DateTime.MinValue;
            else if (hourOffset > 0 && _difMax.TotalHours < hourOffset) return DateTime.MaxValue;
            else return date.AddHours(hourOffset);
        }

        public static string DisplayServerBasedDate(DateTime date, int userHourOffset, int serverHourOffset, int dateFormat, bool omitUtc)
        {
            date = DateTimeAddHoursOffset(date, userHourOffset);

            string sResult = string.Empty;
            //MM/DD/YYYY
            if (dateFormat == 0 || dateFormat == 1) sResult = date.Month.ToString(CultureInfo.InvariantCulture) + "/" + date.Day.ToString(CultureInfo.InvariantCulture) + "/" + date.Year.ToString(CultureInfo.InvariantCulture);
            //DD/MM/YYYY
            else if (dateFormat == 2 || dateFormat == 3) sResult = date.Day.ToString(CultureInfo.InvariantCulture) + "/" + date.Month.ToString(CultureInfo.InvariantCulture) + "/" + date.Year.ToString(CultureInfo.InvariantCulture);
            if (omitUtc) sResult += " (UTC" + Convert.ToString(userHourOffset - serverHourOffset, CultureInfo.InvariantCulture) + ")";
            return sResult;
        }

        public static string DisplayDate(DateTime date, int userHourOffset, int dateFormat, bool omitUtc)
        {
            return DisplayServerBasedDate(date, userHourOffset, 5, dateFormat, omitUtc);
        }

        private static string DisplayDateTime(DateTime date, int HourOffset, int DateFormat, bool OmitUtc)
        {
            return DisplayDate(date, HourOffset, DateFormat, false) + " " + DisplayTime(date, HourOffset, DateFormat, OmitUtc);
        }

        private static string DisplayDateMask(int DateFormat)
        {
            switch (DateFormat)
            {
                case 0:
                    return "mm/dd/yyyy";
                case 1:
                    return "mm/dd/yyyy";
                case 2:
                    return "dd/mm/yyyy";
                case 3:
                    return "dd/mm/yyyy";
                default:
                    return "dd/mm/yyyy";
            }
        }

        private static string DisplayTimeMask(int DateFormat)
        {
            //12:00AM/PM
            if (DateFormat == 0 || DateFormat == 2) return "hh:mm";
            //24:00 (UTC)
            else if (DateFormat == 1 || DateFormat == 3) return "HH:mm";
            return "hh:mm";
        }

        private static string DisplayDateTimeMask(int DateFormat)
        {
            return DisplayDateMask(DateFormat) + " " + DisplayTimeMask(DateFormat);
        }

        #endregion

        #region Protected Methods

        protected void InstancesDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
                e.InputParameters["organizationId"] = UserContext.SelectedOrganizationId;
        }

        protected void InstanceList_DataBound(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                if (comboBox.Items.Count == 1)
                    SearchPanel.Visible = comboBox.Visible = false;
            }
        }

        protected void EntityDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e == null) return;

            e.InputParameters["utcOffset"] = Convert.ToDecimal(UtcOffsetList.SelectedValue, CultureInfo.CurrentCulture);
            e.InputParameters["dateFormat"] = Convert.ToInt32(DateFormatList.SelectedValue, CultureInfo.CurrentCulture);
            e.InputParameters["workingDays"] = GetWorkingDays(EditForm.FindControl("WorkingDaysList") as CheckBoxList);
        }

        protected void EditForm_DataBound(object sender, EventArgs e)
        {
            int dateFormat = 0;
            string workingDays = string.Empty;
            string utcOffset = null;

            if (EditForm.DataItem != null)
            {
                dateFormat = (int)DataBinder.Eval(EditForm.DataItem, "DateFormat");
                workingDays = DataBinder.Eval(EditForm.DataItem, "WorkingDays").ToString();
                utcOffset = DataBinder.Eval(EditForm.DataItem, "UtcOffset").ToString();
            }

            DateFormatsListDataBind(DateFormatList, dateFormat.ToString(CultureInfo.InvariantCulture));
            WorkingDaysListDataBind(WorkingDays, workingDays);
            OffsetsListDataBind(UtcOffsetList, dateFormat, utcOffset);
        }

        #endregion

        #region Overriden Methods

        protected override void LoadResources()
        {
            BaseControl.LoadResources(EditForm, this.GetType().BaseType.Name);

            EditForm.Fields[3].HeaderText = Resources.InstanceProfileControl_EditForm_WorkingDaysField_HeaderText;
            EditForm.Fields[4].HeaderText = Resources.InstanceProfileControl_EditForm_UtcOffsetField_HeaderText;
            EditForm.Fields[5].HeaderText = Resources.InstanceProfileControl_EditForm_DateFormatField_HeaderText;
        }

        protected override void EditForm_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            base.EditForm_ItemUpdated(sender, e);

            if (e == null) return;

            if (e.Exception == null)
                this.Redirect();
        }

        protected override void EditForm_ItemCommand(object sender, CommandEventArgs e)
        {
            if (e == null) return;

            if (e.CommandName.Equals("Cancel", StringComparison.OrdinalIgnoreCase))
                this.Redirect();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!this.IsPostBack)
            {
                InstancesDataSource.FilterExpression = InstanceProvider.InstancesFilterExpression;
            }
        }

        #endregion

        #region Public Methods

        public static void OffsetsListDataBind(RadComboBox list, int dateFormat, string selectedValue)
        {
            if (list == null) return;

            // TODO: +5 should be removed and date and time should be converted to UTC.

            DateTime dateTime = DateTime.Now;
            for (int i = -12; i <= 14; i++)
            {
                list.Items.Add(new RadComboBoxItem(string.Format(CultureInfo.InvariantCulture, Resources.InstanceProfileControl_OffsetsList_Item_Text, i, DisplayDateTime(dateTime, i + 5, dateFormat, false))
                    , Convert.ToDecimal(i + 5).ToString("0.00", CultureInfo.CurrentCulture)));
            }

            if (!string.IsNullOrEmpty(selectedValue))
                list.SelectedValue = selectedValue;
        }

        public static void OffsetsListDataBind(ListControl list, int dateFormat, string selectedValue)
        {
            if (list == null) return;

            // TODO: +5 should be removed and date and time should be converted to UTC.

            DateTime dateTime = DateTime.Now;
            for (int i = -12; i <= 14; i++)
            {
                list.Items.Add(new ListItem(string.Format(CultureInfo.InvariantCulture, Resources.InstanceProfileControl_OffsetsList_Item_Text, i, DisplayDateTime(dateTime, i + 5, dateFormat, false))
                    , Convert.ToDecimal(i + 5).ToString("0.00", CultureInfo.CurrentCulture)));
            }

            if (!string.IsNullOrEmpty(selectedValue))
                list.SelectedValue = selectedValue;
        }

        public static void DateFormatsListDataBind(RadComboBox list, string selectedValue)
        {
            if (list == null) return;

            for (int i = 0; i < 4; i++)
            {
                string postfix = string.Empty;

                if (i == 0 || i == 2)
                    postfix = Resources.InstanceProfileControl_DateFormatsList_Postfix_12Hours;
                else if (i == 1 || i == 3)
                    postfix = Resources.InstanceProfileControl_DateFormatsList_Postfix_24Hours;

                list.Items.Add(new RadComboBoxItem(DisplayDateTimeMask(i) + postfix, i.ToString(CultureInfo.InvariantCulture)));
            }

            list.SelectedValue = selectedValue;
        }

        public static void DateFormatsListDataBind(ListControl list, string selectedValue)
        {
            if (list == null) return;

            for (int i = 0; i < 4; i++)
            {
                string postfix = string.Empty;

                if (i == 0 || i == 2)
                    postfix = Resources.InstanceProfileControl_DateFormatsList_Postfix_12Hours;
                else if (i == 1 || i == 3)
                    postfix = Resources.InstanceProfileControl_DateFormatsList_Postfix_24Hours;

                list.Items.Add(new ListItem(DisplayDateTimeMask(i) + postfix, i.ToString(CultureInfo.InvariantCulture)));
            }

            list.SelectedValue = selectedValue;
        }

        public static void WorkingDaysListDataBind(ListControl list, string selectedValue)
        {
            if (list == null) return;

            string[] days = CultureInfo.CurrentUICulture.DateTimeFormat.DayNames;
            for (int x = 1; x < 8; x++)
            {
                int y = x % 7;
                list.Items.Add(new ListItem(days[y], y.ToString(CultureInfo.InvariantCulture)));
            }

            if (!string.IsNullOrEmpty(selectedValue))
            {
                if (selectedValue.Length == list.Items.Count)
                {
                    int i = 0;
                    foreach (ListItem day in list.Items)
                    {
                        day.Selected = selectedValue[i++] == '1';
                    }
                }
            }
        }

        public static string GetWorkingDays(ListControl list)
        {
            if (list == null) return string.Empty;

            string days = string.Empty;
            foreach (ListItem item in list.Items)
            {
                days = string.Concat(days, (item.Selected) ? "1" : "0");
            }

            return days;
        }

        #endregion
    }
}
