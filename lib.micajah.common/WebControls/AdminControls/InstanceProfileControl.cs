using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll;
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

            DateTime dateTime = Support.DateTimeAddHoursOffset(DateTime.UtcNow, UserContext.Current.UtcOffset);
            for (int i = -12; i <= 14; i++)
            {
                list.Items.Add(new RadComboBoxItem(string.Format(CultureInfo.InvariantCulture, Resources.InstanceProfileControl_OffsetsList_Item_Text, i, Support.GetDisplayDateTime(dateTime, i, dateFormat, true))
                    , Convert.ToDecimal(i).ToString("0.00", CultureInfo.CurrentCulture)));
            }

            if (!string.IsNullOrEmpty(selectedValue))
                list.SelectedValue = selectedValue;
        }

        public static void OffsetsListDataBind(ListControl list, int dateFormat, string selectedValue)
        {
            if (list == null) return;

            DateTime dateTime = Support.DateTimeAddHoursOffset(DateTime.UtcNow, UserContext.Current.UtcOffset);
            for (int i = -12; i <= 14; i++)
            {
                list.Items.Add(new ListItem(string.Format(CultureInfo.InvariantCulture, Resources.InstanceProfileControl_OffsetsList_Item_Text, i, Support.GetDisplayDateTime(dateTime, i, dateFormat, true))
                    , Convert.ToDecimal(i).ToString("0.00", CultureInfo.CurrentCulture)));
            }

            if (!string.IsNullOrEmpty(selectedValue))
                list.SelectedValue = selectedValue;
        }

        public static void DateFormatsListDataBind(RadComboBox list, string selectedValue)
        {
            if (list == null) return;

            for (int i = 0; i < 4; i++)
            {
                list.Items.Add(new RadComboBoxItem(Resources.ResourceManager.GetString("InstanceProfileControl_DateFormat_" + i.ToString(CultureInfo.InvariantCulture)), i.ToString(CultureInfo.InvariantCulture)));
            }

            list.SelectedValue = selectedValue;
        }

        public static void DateFormatsListDataBind(ListControl list, string selectedValue)
        {
            if (list == null) return;

            for (int i = 0; i < 4; i++)
            {
                list.Items.Add(new ListItem(Resources.ResourceManager.GetString("InstanceProfileControl_DateFormat_" + i.ToString(CultureInfo.InvariantCulture)), i.ToString(CultureInfo.InvariantCulture)));
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
