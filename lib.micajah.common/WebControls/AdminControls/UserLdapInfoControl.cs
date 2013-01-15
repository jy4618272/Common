using System;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;

namespace Micajah.Common.WebControls.AdminControls
{
    public class UserLdapInfoControl : BaseEditFormControl
    {
        #region Members

        private Guid? m_UserId;
        protected LinkButton GetUserGroupsButton;
        protected LinkButton ClearUserLdapInfoButton;
        protected LinkButton ReconnectUserToLdapButton;
        protected CommonGridView UserGroupsCommonGridView;
        protected Label UserGroupsNoteLabel;

        protected UpdateProgress ClearUserLdapInfoUpdateProgress;

        //GetUserGroups controls
        protected System.Web.UI.Timer GetUserGroupsTimer;
        protected MultiView GetUserGroupsMultiView;
        protected View GetUserGroupsViewProcess;
        protected View GetUserGroupsViewError;
        protected View GetUserGroupsViewResult;
        protected Image GetUserGroupsViewProcessImage;
        protected Literal GetUserGroupsViewProcessLiteral;
        protected Literal GetUserGroupsViewErrorLiteral;

        //ReconnectUserToLdap controls
        protected System.Web.UI.Timer ReconnectUserToLdapTimer;
        protected MultiView ReconnectUserToLdapMultiView;
        protected View ReconnectUserToLdapViewProcess;
        protected View ReconnectUserToLdapViewError;
        protected View ReconnectUserToLdapViewResult;
        protected Image ReconnectUserToLdapViewProcessImage;
        protected Literal ReconnectUserToLdapViewProcessLiteral;
        protected Literal ReconnectUserToLdapViewErrorLiteral;

        #endregion

        #region Private Properties

        public Guid UserId
        {
            get
            {
                if (!m_UserId.HasValue)
                {
                    object obj = Support.ConvertStringToType(this.Request.QueryString["objectid"], typeof(Guid));
                    m_UserId = ((obj == null) ? Guid.Empty : (Guid)obj);
                }
                return m_UserId.Value;
            }
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OrganizationCollection orgs = WebApplication.LoginProvider.GetOrganizationsByLoginId(this.UserId).FindAllVisible();
                if (orgs.Count > 1)
                    EditForm.Fields[11].Visible = true;
                else
                    EditForm.Fields[11].Visible = false;

                ShowResultsGetUserGroups();
                ShowResultsReconnectUserToLdap();
            }
        }

        protected void EntityDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
            {
                e.InputParameters["organizationId"] = UserContext.Current.SelectedOrganizationId;
                e.InputParameters["loginId"] = this.UserId;
            }
        }

        protected void EntityDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e == null) return;

            e.InputParameters["organizationId"] = UserContext.Current.SelectedOrganizationId;
            e.InputParameters["loginId"] = this.UserId;
            e.InputParameters["firstName"] = (EditForm.Rows[0].Cells[1].Controls[0] as TextBox).Text;
            e.InputParameters["lastName"] = (EditForm.Rows[1].Cells[1].Controls[0] as TextBox).Text;
            e.InputParameters["ldapDomain"] = (EditForm.Rows[2].Cells[1].Controls[0] as TextBox).Text;
            e.InputParameters["ldapDomainFull"] = (EditForm.Rows[3].Cells[1].Controls[0] as TextBox).Text;
            e.InputParameters["ldapUserAlias"] = (EditForm.Rows[4].Cells[1].Controls[0] as TextBox).Text;
            e.InputParameters["ldapUPN"] = (EditForm.Rows[5].Cells[1].Controls[0] as TextBox).Text;
            e.InputParameters["ldapSecurityId"] = (EditForm.Rows[6].Cells[1].Controls[0] as TextBox).Text;
            if (String.IsNullOrEmpty((EditForm.Rows[7].Cells[1].Controls[0] as TextBox).Text) == true)
                e.InputParameters["ldapUserId"] = Guid.Empty;
            else
                e.InputParameters["ldapUserId"] = new Guid((EditForm.Rows[7].Cells[1].Controls[0] as TextBox).Text);
            e.InputParameters["ldapOUPath"] = (EditForm.Rows[8].Cells[1].Controls[0] as TextBox).Text;
            e.InputParameters["secondaryEmails"] = (EditForm.Rows[10].Cells[1].Controls[0] as TextBox).Text;
            e.InputParameters.Remove("PrimaryEmail");
        }

        protected void GetUserGroupsButton_Click(object sender, EventArgs e)
        {
            GetUserGroupsTimer.Enabled = true;
            GetUserGroupsMultiView.SetActiveView(GetUserGroupsViewProcess);

            Thread thread = new Thread(LdapProcessGetUserGroups);
            thread.CurrentCulture = CultureInfo.CurrentCulture;
            thread.CurrentUICulture = CultureInfo.CurrentUICulture;
            thread.Priority = ThreadPriority.Lowest;
            thread.IsBackground = true;
            thread.Start(UserContext.Current.SelectedOrganizationId.ToString());

            GetUserGroupsButton.Enabled = false;
        }

        protected void LdapProcessGetUserGroups(object organizationId)
        {
            string processId = string.Format("GetUserGroups_{0}_{1}", (string)organizationId, this.UserId);
            Bll.LdapProcess ldapProcess = null;
            try
            {
                ldapProcess = LdapInfoProvider.LdapProcesses.Find(x => x.ProcessId == processId);
                if (ldapProcess != null)
                    LdapInfoProvider.LdapProcesses.Remove(ldapProcess);

                ldapProcess = new Bll.LdapProcess();
                ldapProcess.ProcessId = processId;
                ldapProcess.ThreadStateType = Bll.ThreadStateType.Running;
                ldapProcess.MessageError = string.Empty;
                ldapProcess.Message = string.Empty;
                ldapProcess.Data = null;
                LdapInfoProvider.LdapProcesses.Add(ldapProcess);
                ldapProcess.Data = LdapInfoProvider.GetUserLdapGroups(new Guid((string)organizationId), this.UserId);
                ldapProcess.ThreadStateType = Bll.ThreadStateType.Finished;
            }
            catch (Exception ex)
            {
                ldapProcess = LdapInfoProvider.LdapProcesses.Find(x => x.ProcessId == processId);
                if (ldapProcess != null)
                {
                    ldapProcess.ProcessId = processId;
                    ldapProcess.ThreadStateType = Bll.ThreadStateType.Failed;
                    ldapProcess.MessageError = string.Format(CultureInfo.InvariantCulture, "<br/>{0}", ex.ToString().Replace("\r\n", "<br/>"));
                    ldapProcess.Message = string.Empty;
                    ldapProcess.Data = null;
                }
            }
            finally
            {
                processId = null;
                ldapProcess = null;
            }
        }

        protected void ShowResultsGetUserGroups()
        {
            Bll.LdapProcess ldapProcess = null;
            string processId = string.Format("GetUserGroups_{0}_{1}", UserContext.Current.SelectedOrganizationId, this.UserId);
            try
            {
                GetUserGroupsTimer.Enabled = false;
                ldapProcess = LdapInfoProvider.LdapProcesses.Find(x => x.ProcessId == processId);
                if (ldapProcess != null)
                {
                    switch (ldapProcess.ThreadStateType)
                    {
                        case Bll.ThreadStateType.Failed:
                            GetUserGroupsButton.Enabled = true;
                            GetUserGroupsMultiView.SetActiveView(GetUserGroupsViewError);
                            GetUserGroupsViewErrorLiteral.Text = ldapProcess.MessageError;
                            LdapInfoProvider.LdapProcesses.Remove(ldapProcess);
                            break;

                        case Bll.ThreadStateType.Finished:
                            GetUserGroupsButton.Enabled = true;
                            GetUserGroupsMultiView.SetActiveView(GetUserGroupsViewResult);
                            UserGroupsCommonGridView.DataSource = ldapProcess.Data as DataView;
                            UserGroupsCommonGridView.DataBind();
                            LdapInfoProvider.LdapProcesses.Remove(ldapProcess);
                            break;

                        case Bll.ThreadStateType.Running:
                            GetUserGroupsMultiView.SetActiveView(GetUserGroupsViewProcess);
                            GetUserGroupsTimer.Enabled = true;
                            GetUserGroupsButton.Enabled = false;
                            break;
                    }
                }
            }
            finally
            {
                ldapProcess = null;
                processId = null;
            }
        }

        protected void GetUserGroupsTimer_Tick(object sender, EventArgs e)
        {
            ShowResultsGetUserGroups();
        }

        protected void ClearLdapInfoButton_Click(object sender, EventArgs e)
        {
            UserContext user = UserContext.Current;
            if (user != null)
            {
                WebApplication.LoginProvider.UpdateUserLdapInfo(user.SelectedOrganization.OrganizationId, this.UserId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Guid.Empty, string.Empty);
                EmailProvider.DeleteEmails(this.UserId);
                EditForm.DataBind();
            }
        }

        protected void ReconnectUserToLdapButton_Click(object sender, EventArgs e)
        {
            UserContext user = UserContext.Current;
            if (user != null)
            {
                ReconnectUserToLdapTimer.Enabled = true;
                ReconnectUserToLdapMultiView.SetActiveView(ReconnectUserToLdapViewProcess);

                Thread thread = new Thread(LdapProcessReconnectUserToLdap);
                thread.CurrentCulture = CultureInfo.CurrentCulture;
                thread.CurrentUICulture = CultureInfo.CurrentUICulture;
                thread.Priority = ThreadPriority.Lowest;
                thread.IsBackground = true;
                thread.Start(UserContext.Current);
                ReconnectUserToLdapButton.Enabled = false;
            }
        }

        protected void LdapProcessReconnectUserToLdap(object arg)
        {
            UserContext userContext = null;
            string processId = null;

            Bll.LdapProcess ldapProcess = null;
            ApplicationLogger log = null;
            LdapIntegration ldap = null;
            string loginName = null;
            LdapAdapter.IUser ldapUser = null;
            try
            {
                userContext = arg as UserContext;
                if (userContext != null)
                {
                    processId = string.Format("ReconnectUserToLdap_{0}_{1}", userContext.SelectedOrganization.OrganizationId, this.UserId);

                    ldapProcess = LdapInfoProvider.LdapProcesses.Find(x => x.ProcessId == processId);
                    if (ldapProcess != null)
                        LdapInfoProvider.LdapProcesses.Remove(ldapProcess);

                    ldapProcess = new Bll.LdapProcess();
                    ldapProcess.ProcessId = processId;
                    ldapProcess.ThreadStateType = Bll.ThreadStateType.Running;
                    ldapProcess.MessageError = string.Empty;
                    ldapProcess.Message = string.Empty;
                    ldapProcess.Data = null;
                    LdapInfoProvider.LdapProcesses.Add(ldapProcess);

                    log = new ApplicationLogger();
                    ldap = new LdapIntegration(log);
                    loginName = WebApplication.LoginProvider.GetLoginName(this.UserId);
                    ldapUser = LdapInfoProvider.GetLdapUser(userContext.SelectedOrganization.OrganizationId, loginName);
                    if (ldapUser != null)
                    {
                        ldap.CreateUserEmails(LdapInfoProvider.GetLdapUserAltEmails(userContext.SelectedOrganization.OrganizationId, ldapUser.UserId, false), ldapUser);
                        WebApplication.LoginProvider.UpdateUserLdapInfo(userContext.SelectedOrganization.OrganizationId, this.UserId, ldapUser.FirstName, ldapUser.LastName, ldapUser.LdapDomain, ldapUser.LdapDomainFull, ldapUser.LdapUserAlias, ldapUser.LdapUserPrinciple, ldapUser.UserSid, ldapUser.UserId, ldapUser.LdapOUPath);
                    }
                    ldapProcess.ThreadStateType = Bll.ThreadStateType.Finished;
                }
            }
            catch (Exception ex)
            {
                ldapProcess = LdapInfoProvider.LdapProcesses.Find(x => x.ProcessId == processId);
                if (ldapProcess != null)
                {
                    ldapProcess.ProcessId = processId;
                    ldapProcess.ThreadStateType = Bll.ThreadStateType.Failed;
                    ldapProcess.MessageError = string.Format(CultureInfo.InvariantCulture, "<br/>{0}", ex.ToString().Replace("\r\n", "<br/>"));
                    ldapProcess.Message = string.Empty;
                    ldapProcess.Data = null;
                }
            }
            finally
            {
                userContext = null;
                processId = null;
                ldapProcess = null;
                log = null;
                if (ldap != null) ldap.Dispose();
                loginName = null;
                ldapUser = null;
            }
        }

        protected void ShowResultsReconnectUserToLdap()
        {
            Bll.LdapProcess ldapProcess = null;
            string processId = string.Format("ReconnectUserToLdap_{0}_{1}", UserContext.Current.SelectedOrganizationId, this.UserId);
            try
            {
                ReconnectUserToLdapTimer.Enabled = false;
                ldapProcess = LdapInfoProvider.LdapProcesses.Find(x => x.ProcessId == processId);
                if (ldapProcess != null)
                {
                    switch (ldapProcess.ThreadStateType)
                    {
                        case Bll.ThreadStateType.Failed:
                            ReconnectUserToLdapButton.Enabled = true;
                            ReconnectUserToLdapMultiView.SetActiveView(ReconnectUserToLdapViewError);
                            ReconnectUserToLdapViewErrorLiteral.Text = ldapProcess.MessageError;
                            LdapInfoProvider.LdapProcesses.Remove(ldapProcess);
                            break;

                        case Bll.ThreadStateType.Finished:
                            ReconnectUserToLdapButton.Enabled = true;
                            ReconnectUserToLdapMultiView.SetActiveView(ReconnectUserToLdapViewResult);
                            EditForm.DataBind();
                            LdapInfoProvider.LdapProcesses.Remove(ldapProcess);
                            break;

                        case Bll.ThreadStateType.Running:
                            ReconnectUserToLdapMultiView.SetActiveView(ReconnectUserToLdapViewProcess);
                            ReconnectUserToLdapTimer.Enabled = true;
                            ReconnectUserToLdapButton.Enabled = false;
                            break;
                    }
                }
            }
            finally
            {
                ldapProcess = null;
                processId = null;
            }
        }

        protected void ReconnectUserToLdapTimer_Tick(object sender, EventArgs e)
        {
            ShowResultsReconnectUserToLdap();
        }

        protected void UserGroupsCommonGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e == null) return;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool isDirect = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsDirect"), CultureInfo.CurrentCulture);
                if (isDirect)
                    e.Row.Cells[0].Text = "<b>" + e.Row.Cells[0].Text + "</b>";
            }
        }

        protected void EditForm_DataBound(object sender, EventArgs e)
        {
            Label lbl = EditForm.FindControl("OrgMembershipCount") as Label;
            if (lbl != null)
                lbl.Text = Resources.UserLdapInfoControl_OrgMembershipCount_Text;

            (EditForm.Rows[9].Cells[1].Controls[0] as TextBox).Enabled = false;
        }

        #endregion

        #region Overriden Methods

        protected override void LoadResources()
        {
            base.LoadResources();

            GetUserGroupsButton.Text = Resources.UserLdapInfoControl_GetUserGroupsButton_Text;
            UserGroupsCommonGridView.Columns[0].HeaderText = Resources.UserLdapInfoControl_UserGroupsCommonGridView_LdapGroupNameColumn_HeaderText;
            UserGroupsCommonGridView.Columns[1].HeaderText = Resources.UserLdapInfoControl_UserGroupsCommonGridView_GroupNameColumn_HeaderText;
            UserGroupsNoteLabel.Text = Resources.UserLdapInfoControl_UserGroupsNoteLabel_Text;
            ClearUserLdapInfoButton.Text = Resources.UserLdapInfoControl_ClearUserLdapInfoButton_Text;
            ReconnectUserToLdapButton.Text = Resources.UserLdapInfoControl_ReconnectUserToLdapButton_Text;

            ClearUserLdapInfoUpdateProgress.ProgressText = Resources.UserLdapInfoControl_UpdateProgress_Text;
            ClearUserLdapInfoUpdateProgress.Timeout = int.MaxValue;
            ClearUserLdapInfoUpdateProgress.HideAfter = -1;
            ClearUserLdapInfoUpdateProgress.ShowSuccessText = false;
            ClearUserLdapInfoUpdateProgress.PostBackControlId = this.ClearUserLdapInfoButton.ClientID;

            //GetUserGroups
            GetUserGroupsViewProcessLiteral.Text = Resources.UserLdapInfoControl_UpdateProgress_Text;
            GetUserGroupsViewProcessImage.ImageAlign = ImageAlign.AbsMiddle;
            GetUserGroupsViewProcessImage.ImageUrl = ResourceProvider.GetImageUrl(typeof(UpdateProgress), "Loader.gif", true);

            //ReconnectUserToLdap
            ReconnectUserToLdapViewProcessLiteral.Text = Resources.UserLdapInfoControl_UpdateProgress_Text;
            ReconnectUserToLdapViewProcessImage.ImageAlign = ImageAlign.AbsMiddle;
            ReconnectUserToLdapViewProcessImage.ImageUrl = ResourceProvider.GetImageUrl(typeof(UpdateProgress), "Loader.gif", true);
        }

        protected override void EditForm_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            base.EditForm_ItemUpdated(sender, e);

            if (e == null) return;

            if (e.Exception == null)
            {
                this.RedirectToConfigurationPage();
            }
        }

        protected override void EditForm_ItemCommand(object sender, CommandEventArgs e)
        {
            if (e == null) return;

            if (e.CommandName.Equals("Cancel", StringComparison.OrdinalIgnoreCase))
            {
                this.RedirectToConfigurationPage();
            }
        }

        #endregion
    }
}
