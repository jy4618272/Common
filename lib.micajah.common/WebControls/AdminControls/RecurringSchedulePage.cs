using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Security;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls.AdminControls
{
    public class RecurringSchedulePage : System.Web.UI.Page
    {
        #region Members

        protected CommonGridView List;
        protected ObjectDataSource ScheduleListDataSource;
        protected RecurrenceSchedule RecurrenceScheduleControl;

        private Micajah.Common.Pages.MasterPage m_MasterPage;
        private UserContext m_UserContext;

        #endregion

        #region Properties

        protected Micajah.Common.Pages.MasterPage MasterPage
        {
            get
            {
                if (m_MasterPage == null)
                    m_MasterPage = Micajah.Common.Pages.MasterPage.GetMasterPage(Page);
                return m_MasterPage;
            }
        }

        #endregion

        #region Private Methods

        private void AddBreadcrumbs()
        {
            if (this.MasterPage != null)
            {
                Micajah.Common.Bll.Action item = new Micajah.Common.Bll.Action();
                item.ActionId = Guid.NewGuid();
                item.Name = RecurrenceScheduleControl.Description;
                item.ParentAction = this.MasterPage.ActiveAction;
                UserContext.Breadcrumbs.Add(item);
            }
        }

        private static void ResetBreadcrumbs()
        {
            UserContext.Breadcrumbs.RemoveLast();
            //this.MasterPage.UpdateBreadcrumbs();
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            m_UserContext = UserContext.Current;

            if (!IsPostBack)
            {
                if (m_UserContext == null || m_UserContext.SelectedOrganizationId == Guid.Empty)
                {
                    this.Visible = false;
                    return;
                }

                RecurrenceScheduleControl.OrganizationId = m_UserContext.SelectedOrganizationId;
                if (m_UserContext.SelectedInstanceId != Guid.Empty)
                    RecurrenceScheduleControl.InstanceId = m_UserContext.SelectedInstanceId;
                RecurrenceScheduleControl.DateFormat = Support.GetLongDateTimeFormat(m_UserContext.TimeFormat, m_UserContext.DateFormat);
            }

            this.MasterPage.VisibleBreadcrumbs = true;
        }

        protected void RecurrenceScheduleControl_Cancel(object sender, EventArgs e)
        {
            List.Visible = true;
            RecurrenceScheduleControl.Visible = false;
            ResetBreadcrumbs();
        }

        protected void ScheduleListDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e == null) return;

            e.InputParameters["organizationId"] = m_UserContext.SelectedOrganizationId;
            e.InputParameters["instanceId"] = m_UserContext.SelectedInstanceId;
        }

        protected void List_Action(object sender, CommonGridViewActionEventArgs e)
        {
            Guid organizationId = m_UserContext.SelectedOrganizationId;
            Guid recurringScheduleId = Guid.Empty;

            if (e == null) return;

            if (e.RowIndex > -1)
                recurringScheduleId = (Guid)List.DataKeys[e.RowIndex].Values["RecurringScheduleId"];
            switch (e.Action)
            {
                case CommandActions.Add:
                    List.Visible = false;
                    RecurrenceScheduleControl.Visible = true;
                    DateTime currDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, m_UserContext.TimeZone);
                    RecurrenceScheduleControl.RecurringScheduleId = Guid.NewGuid();
                    RecurrenceScheduleControl.DateStart = currDate;
                    RecurrenceScheduleControl.DateEnd = currDate.AddHours(1);
                    RecurrenceScheduleControl.Description = string.Empty;
                    RecurrenceScheduleControl.LocalEntityId = string.Empty;
                    RecurrenceScheduleControl.LocalEntityType = string.Empty;
                    RecurrenceScheduleControl.RecurrenceRule = string.Empty;
                    this.AddBreadcrumbs();
                    break;
                case CommandActions.Edit:
                    Micajah.Common.Dal.ClientDataSet.RecurringScheduleRow row = Micajah.Common.Bll.Providers.RecurringScheduleProvider.GetRecurringSchedulesRow(recurringScheduleId, organizationId);
                    if (row != null)
                    {
                        List.Visible = false;
                        RecurrenceScheduleControl.Visible = true;
                        RecurrenceScheduleControl.RecurringScheduleId = recurringScheduleId;
                        RecurrenceScheduleControl.DateStart = TimeZoneInfo.ConvertTimeFromUtc(row.StartDate, m_UserContext.TimeZone);
                        RecurrenceScheduleControl.DateEnd = TimeZoneInfo.ConvertTimeFromUtc(row.EndDate, m_UserContext.TimeZone);
                        RecurrenceScheduleControl.Description = row.Name;
                        RecurrenceScheduleControl.LocalEntityId = row.LocalEntityId;
                        RecurrenceScheduleControl.LocalEntityType = row.LocalEntityType;
                        RecurrenceScheduleControl.RecurrenceRule = row.RecurrenceRule;
                        this.AddBreadcrumbs();
                    }
                    break;
                default:
                    break;
            }
        }

        protected void List_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e == null) return;

            object obj = DataBinder.Eval(e.Row.DataItem, "StartDate");
            if (!Support.IsNullOrDBNull(obj))
            {
                Literal lit = (Literal)e.Row.FindControl("StartDateLiteral");
                lit.Text = Support.ToLongDateTimeString((DateTime)obj, m_UserContext.TimeZone, m_UserContext.TimeFormat, true);
            }

            obj = DataBinder.Eval(e.Row.DataItem, "EndDate");
            if (!Support.IsNullOrDBNull(obj))
            {
                Literal lit = (Literal)e.Row.FindControl("EndDateLiteral");
                lit.Text = Support.ToLongDateTimeString((DateTime)obj, m_UserContext.TimeZone, m_UserContext.TimeFormat, true);
            }
        }

        protected void RecurrenceScheduleControl_Updated(object sender, RecurringScheduleEventArgs e)
        {
            if (e == null) return;

            if (e.RecurringScheduleId != Guid.Empty)
            {
                RecurringScheduleProvider.UpdateRecurringSchedule(
                    e.RecurringScheduleId,
                    e.OrganizationId,
                    e.InstanceId,
                    e.LocalEntityType,
                    e.LocalEntityId,
                    e.Name,
                    TimeZoneInfo.ConvertTimeToUtc(e.StartDate, m_UserContext.TimeZone),
                    TimeZoneInfo.ConvertTimeToUtc(e.EndDate, m_UserContext.TimeZone),
                    e.RecurrenceRule,
                    DateTime.UtcNow,
                    (m_UserContext != null ? m_UserContext.UserId : Guid.Empty),
                    false);
                List.Visible = true;
                RecurrenceScheduleControl.Visible = false;
                List.DataBind();
                ResetBreadcrumbs();
            }
        }

        #endregion
    }
}
