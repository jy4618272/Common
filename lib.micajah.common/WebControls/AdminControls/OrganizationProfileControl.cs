﻿using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;
using System;
using System.Globalization;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls.AdminControls
{
    public class OrganizationProfileControl : BaseEditFormControl
    {
        #region Members

        private ComboBox m_MonthList;
        private ComboBox m_DayList;
        private ComboBox m_WeekStartsDayList;
        private TextBox m_EmailSuffixes;

        #endregion

        #region Private Properties

        private ComboBox MonthList
        {
            get
            {
                if (m_MonthList == null) m_MonthList = EditForm.FindControl("MonthList") as ComboBox;
                return m_MonthList;
            }
        }

        private ComboBox DayList
        {
            get
            {
                if (m_DayList == null) m_DayList = EditForm.FindControl("DayList") as ComboBox;
                return m_DayList;
            }
        }

        private ComboBox WeekStartsDayList
        {
            get
            {
                if (m_WeekStartsDayList == null) m_WeekStartsDayList = EditForm.FindControl("WeekStartsDayList") as ComboBox;
                return m_WeekStartsDayList;
            }
        }

        private TextBox EmailSuffixes
        {
            get
            {
                if (m_EmailSuffixes == null) m_EmailSuffixes = EditForm.FindControl("EmailSuffixes") as TextBox;
                return m_EmailSuffixes;
            }
        }

        #endregion

        #region Protected Methods

        protected void EditForm_DataBound(object sender, EventArgs e)
        {
            MonthList.Items.Add(new RadComboBoxItem(String.Empty, "0"));
            DateTime month = DateTime.MinValue;
            for (int i = 0; i < 12; i++)
            {
                DateTime date = month.AddMonths(i);
                MonthList.Items.Add(new RadComboBoxItem(date.ToString("MMMM", CultureInfo.CurrentCulture), date.Month.ToString(CultureInfo.InvariantCulture)));
            }

            DayList.Items.Add(new RadComboBoxItem(String.Empty, "0"));
            for (int i = 1; i < 32; i++)
                DayList.Items.Add(new RadComboBoxItem(i.ToString(CultureInfo.InvariantCulture), i.ToString(CultureInfo.InvariantCulture)));

            WeekStartsDayList.Items.Add(new RadComboBoxItem());
            WeekStartsDayList.Items.Add(new RadComboBoxItem(Resources.OrganizationProfileControl_WeekStartsDayList_Sunday, ((int)FirstDayOfWeek.Sunday).ToString(CultureInfo.InvariantCulture)));
            WeekStartsDayList.Items.Add(new RadComboBoxItem(Resources.OrganizationProfileControl_WeekStartsDayList_Monday, ((int)FirstDayOfWeek.Monday).ToString(CultureInfo.InvariantCulture)));

            UserContext user = UserContext.Current;

            Organization org = user.Organization;
            if (org.FiscalYearStartMonth.HasValue)
                MonthList.SelectedValue = org.FiscalYearStartMonth.Value.ToString(CultureInfo.InvariantCulture);
            if (org.FiscalYearStartDay.HasValue)
                DayList.SelectedValue = org.FiscalYearStartDay.Value.ToString(CultureInfo.InvariantCulture);
            if (org.WeekStartsDay.HasValue)
                WeekStartsDayList.SelectedValue = (org.WeekStartsDay.Value).ToString(CultureInfo.InvariantCulture);

            EmailSuffixes.Text = EmailSuffixProvider.GetEmailSuffixName(user.OrganizationId);
        }

        protected void EntityDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e == null) return;

            string ldapDomains = (EditForm.Rows[7].Cells[1].Controls[0] as TextBox).Text;
            e.InputParameters["ldapDomains"] = ldapDomains.Replace(" ", string.Empty).Trim();

            int value = 0;
            if (int.TryParse(this.MonthList.SelectedValue, out value))
                e.InputParameters["fiscalYearStartMonth"] = value;
            else
                e.InputParameters["fiscalYearStartMonth"] = null;

            if (int.TryParse(this.DayList.SelectedValue, out value))
                e.InputParameters["fiscalYearStartDay"] = value;
            else
                e.InputParameters["fiscalYearStartDay"] = null;

            if (int.TryParse(this.WeekStartsDayList.SelectedValue, out value))
                e.InputParameters["weekStartsDay"] = value;
            else
                e.InputParameters["weekStartsDay"] = null;

            e.InputParameters["emailSuffixes"] = EmailSuffixes.Text;
        }

        protected void EntityDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
                e.InputParameters["organizationId"] = UserContext.Current.OrganizationId;
        }

        #endregion

        #region Overriden Methods

        protected override void LoadResources()
        {
            BaseControl.LoadResources(EditForm, typeof(OrganizationsControl).Name);
            EditForm.ObjectName = Resources.OrganizationProfileControl_EditForm_ObjectName;
            EditForm.Fields[3].HeaderText = Resources.OrganizationsControl_EditForm_FiscalYearStartMonth_HeaderText;
            EditForm.Fields[4].HeaderText = Resources.OrganizationsControl_EditForm_FiscalYearStartDay_HeaderText;
            EditForm.Fields[5].HeaderText = Resources.OrganizationsControl_EditForm_WeekStartsDay_HeaderText;
            EditForm.Fields[6].HeaderText = Resources.OrganizationsControl_EditForm_EmailSuffixesField_HeaderText;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!FrameworkConfiguration.Current.WebApplication.Integration.Ldap.Enabled)
                {
                    EditForm.Fields[6].Visible = false;
                    EditForm.Fields[7].Visible = false;
                }
            }
            base.OnLoad(e);
        }

        protected override void EditForm_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            base.EditForm_ItemUpdated(sender, e);

            if (e == null) return;

            if (e.Exception == null)
                this.RedirectToConfigurationPage();
        }

        protected override void EditForm_ItemCommand(object sender, CommandEventArgs e)
        {
            if (e == null) return;

            if (e.CommandName.Equals("Cancel", StringComparison.OrdinalIgnoreCase))
                this.RedirectToConfigurationPage();
        }

        #endregion
    }
}
