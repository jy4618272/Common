using System;
using System.Globalization;
using System.Web.UI.WebControls;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;

namespace Micajah.Common.WebControls.AdminControls
{
    public class InstanceProfileControl : BaseEditFormControl
    {
        #region Members

        protected Panel SearchPanel;
        protected ComboBox InstanceList;
        protected ObjectDataSource InstancesDataSource;

        private DropDownList m_TimeZoneList;
        private DropDownList m_TimeFormatList;
        private DropDownList m_DateFormatList;
        private CheckBoxList m_WorkingDays;

        #endregion

        #region Private Properties

        private DropDownList TimeZoneList
        {
            get
            {
                if (m_TimeZoneList == null) m_TimeZoneList = EditForm.FindControl("TimeZoneList") as DropDownList;
                return m_TimeZoneList;
            }
        }

        private DropDownList TimeFormatList
        {
            get
            {
                if (m_TimeFormatList == null) m_TimeFormatList = EditForm.FindControl("TimeFormatList") as DropDownList;
                return m_TimeFormatList;
            }
        }

        private DropDownList DateFormatList
        {
            get
            {
                if (m_DateFormatList == null) m_DateFormatList = EditForm.FindControl("DateFormatList") as DropDownList;
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

            e.InputParameters["timeZoneId"] = (string.IsNullOrEmpty(TimeZoneList.SelectedValue) ? null : TimeZoneList.SelectedValue);

            int value = 0;
            if (int.TryParse(TimeFormatList.SelectedValue, out value))
                e.InputParameters["timeFormat"] = value;
            else
                e.InputParameters["timeFormat"] = null;

            if (int.TryParse(DateFormatList.SelectedValue, out value))
                e.InputParameters["dateFormat"] = value;
            else
                e.InputParameters["dateFormat"] = null;

            e.InputParameters["workingDays"] = BaseControl.GetWorkingDays(EditForm.FindControl("WorkingDaysList") as CheckBoxList);
        }

        protected void EditForm_DataBound(object sender, EventArgs e)
        {
            string workingDays = string.Empty;
            string timeZoneId = null;
            string timeFormat = null;
            string dateFormat = null;

            if (EditForm.DataItem != null)
            {
                Instance inst = (Instance)EditForm.DataItem;

                workingDays = inst.WorkingDays;
                timeZoneId = inst.TimeZoneId;

                if (inst.TimeFormat.HasValue)
                    timeFormat = inst.TimeFormat.Value.ToString(CultureInfo.InvariantCulture);

                if (inst.DateFormat.HasValue)
                    dateFormat = inst.DateFormat.Value.ToString(CultureInfo.InvariantCulture);
            }

            BaseControl.WorkingDaysListDataBind(WorkingDays, workingDays);
            BaseControl.TimeZoneListDataBind(TimeZoneList, timeZoneId, false);
            BaseControl.TimeFormatListDataBind(TimeFormatList, timeFormat, false);
            BaseControl.DateFormatListDataBind(DateFormatList, dateFormat, false);
        }

        #endregion

        #region Overriden Methods

        protected override void LoadResources()
        {
            BaseControl.LoadResources(EditForm, this.GetType().BaseType.Name);

            EditForm.Fields[3].HeaderText = Resources.InstanceProfileControl_EditForm_WorkingDaysField_HeaderText;
            EditForm.Fields[4].HeaderText = Resources.InstanceProfileControl_EditForm_TimeZoneField_HeaderText;
            EditForm.Fields[5].HeaderText = Resources.InstanceProfileControl_EditForm_TimeFormatField_HeaderText;
            EditForm.Fields[6].HeaderText = Resources.InstanceProfileControl_EditForm_DateFormatField_HeaderText;
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
    }
}
