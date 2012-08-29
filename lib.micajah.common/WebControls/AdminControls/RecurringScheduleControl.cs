using System;
using System.Web.UI.WebControls;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Security;

namespace Micajah.Common.WebControls.AdminControls
{
    public class RecurringSchedulePage : System.Web.UI.Page
    {
        #region Members
        protected CommonGridView List;
        protected ObjectDataSource ScheduleListDataSource;
        protected RecurrenceSchedule RecurrenceScheduleControl;
        private Micajah.Common.Pages.MasterPage m_MasterPage;
        #endregion

        #region Properties
        protected Micajah.Common.Pages.MasterPage MasterPage
        {
            get
            {
                if (m_MasterPage == null) m_MasterPage = Page.Master as Micajah.Common.Pages.MasterPage;
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
            if (!IsPostBack)
            {
                if (UserContext.Current == null || UserContext.Current.SelectedOrganization == null)
                {
                    this.Visible = false;
                    return;
                }
                RecurrenceScheduleControl.OrganizationId = UserContext.Current.SelectedOrganization.OrganizationId;
                if (UserContext.Current.SelectedInstance != null)
                    RecurrenceScheduleControl.InstanceId = UserContext.Current.SelectedInstance.InstanceId;
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

            e.InputParameters["organizationId"] = UserContext.Current.SelectedOrganization.OrganizationId;
            e.InputParameters["instanceId"] = UserContext.Current.SelectedInstance == null ? Guid.Empty : UserContext.Current.SelectedInstance.InstanceId;
        }

        protected void List_Action(object sender, CommonGridViewActionEventArgs e)
        {
            Guid organizationId = UserContext.Current.SelectedOrganization.OrganizationId;
            Guid recurringScheduleId = Guid.Empty;

            if (e == null) return;

            if (e.RowIndex > -1)
                recurringScheduleId = (Guid)List.DataKeys[e.RowIndex].Values["RecurringScheduleId"];
            switch (e.Action)
            {
                case CommandActions.Add:
                    List.Visible = false;
                    RecurrenceScheduleControl.Visible = true;
                    DateTime currDate = DateTime.Now;
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
                    Micajah.Common.Dal.OrganizationDataSet.RecurringScheduleRow row = Micajah.Common.Bll.Providers.RecurringScheduleProvider.GetRecurringSchedulesRow(recurringScheduleId, organizationId);
                    if (row != null)
                    {
                        List.Visible = false;
                        RecurrenceScheduleControl.Visible = true;
                        RecurrenceScheduleControl.RecurringScheduleId = recurringScheduleId;
                        RecurrenceScheduleControl.DateStart = row.StartDate;
                        RecurrenceScheduleControl.DateEnd = row.EndDate;
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
                    e.StartDate,
                    e.EndDate,
                    e.RecurrenceRule,
                    DateTime.Now,
                    (UserContext.Current != null ? UserContext.Current.UserId : Guid.Empty),
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
