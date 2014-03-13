using Google.GData.Apps;
using Google.GData.Apps.Groups;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;
using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls.AdminControls
{
    public class GoogleIntegrationControl : BaseEditFormControl
    {
        #region Members

        protected Label lblDescription;
        protected Literal lblGoogleSetup;

        protected Literal lblGoogleDomain;
        protected Literal lblGoogleEmail;
        protected Literal lblGooglePassword;
        protected Literal lblTitle;

        protected System.Web.UI.WebControls.TextBox txtGoogleDomain;
        protected System.Web.UI.WebControls.TextBox txtGoogleEmail;
        protected System.Web.UI.WebControls.TextBox txtGooglePassword;

        protected Label lblStep1;
        protected LinkButton lbStep1;
        protected LinkButton lbImportUsers;
        protected UpdateProgress upStep1Progress;
        protected MultiView mvStep1;
        protected View vwStep1Result;
        protected View vwStep1Error;
        protected Label lblStep1Error;
        protected CommonGridView gvStep1Results;

        protected Label lblStep2;
        protected LinkButton lbStep2;
        protected UpdateProgress upStep2Progress;
        protected MultiView mvStep2;
        protected View vwStep2Result;
        protected View vwStep2Error;
        protected Label lblStep2Error;
        protected CommonGridView gvStep2Results;

        protected Image imgGoogleAppsLogo;
        protected HyperLink hlGoogleAppsForBusiness;
        protected HyperLink hlAddApplication;
        protected Literal litCaption;

        #endregion

        #region Protected Methods

        protected void lbStep1_Click(object sender, EventArgs e)
        {
            try
            {
                AppsService service = new AppsService(txtGoogleDomain.Text, txtGoogleEmail.Text, txtGooglePassword.Text);
                UserFeed userFeed = service.RetrieveAllUsers();

                DataTable dt = new DataTable();
                dt.Columns.Add(Resources.GoogleIntegrationControl_UsernameColumn_HeaderText, typeof(string));
                dt.Columns.Add(Resources.GoogleIntegrationControl_FirstNameColumn_HeaderText, typeof(string));
                dt.Columns.Add(Resources.GoogleIntegrationControl_LastNameColumn_HeaderText, typeof(string));
                dt.Columns.Add(Resources.GoogleIntegrationControl_AdminColumn_HeaderText, typeof(string));

                for (int i = 0; i < userFeed.Entries.Count; i++)
                {
                    UserEntry userEntry = userFeed.Entries[i] as UserEntry;
                    dt.Rows.Add(userEntry.Login.UserName, userEntry.Name.GivenName, userEntry.Name.FamilyName, userEntry.Login.Admin ? Resources.GoogleIntegrationControl_AdminColumn_Value : string.Empty);
                }

                gvStep1Results.DataSource = dt;
                gvStep1Results.DataBind();

                mvStep1.SetActiveView(vwStep1Result);

                lbImportUsers.Text = Resources.GoogleIntegrationControl_ImportUsers_LinkButton_Text;
                lbImportUsers.Visible = lbImportUsers.Enabled = dt.Rows.Count > 0;
            }
            catch (AppsException a)
            {
                lblStep1Error.Text = string.Format(CultureInfo.CurrentCulture, Resources.GoogleIntegrationControl_GoogleAppsError_Text, a.ErrorCode, a.InvalidInput, a.Reason);
                mvStep1.SetActiveView(vwStep1Error);
            }
            catch (Exception ex)
            {
                lblStep1Error.Text = ex.Message;
                mvStep1.SetActiveView(vwStep1Error);
            }
        }

        protected void lbStep2_Click(object sender, EventArgs e)
        {
            try
            {
                AppsService service = new AppsService(txtGoogleDomain.Text, txtGoogleEmail.Text, txtGooglePassword.Text);
                AppsExtendedFeed groupsFeed = service.Groups.RetrieveAllGroups();

                DataTable dt = new DataTable();
                dt.Columns.Add(Resources.GoogleIntegrationControl_NameColumn_HeaderText, typeof(string));
                dt.Columns.Add(Resources.GoogleIntegrationControl_IdColumn_HeaderText, typeof(string));
                dt.Columns.Add(Resources.GoogleIntegrationControl_DescriptionColumn_HeaderText, typeof(string));
                dt.Columns.Add(Resources.GoogleIntegrationControl_MembersColumn_HeaderText, typeof(string));

                for (int i = 0; i < groupsFeed.Entries.Count; i++)
                {
                    GroupEntry groupEntry = groupsFeed.Entries[i] as GroupEntry;
                    MemberFeed memberFeed = service.Groups.RetrieveAllMembers(groupEntry.GroupId);
                    StringBuilder sb = new StringBuilder();

                    for (int j = 0; j < memberFeed.Entries.Count; j++)
                    {
                        MemberEntry memberEntry = memberFeed.Entries[j] as MemberEntry;
                        if (string.Compare(memberEntry.MemberId, "*", true) == 0)
                            sb.AppendFormat(Resources.GoogleIntegrationControl_MembersColumn_AllUsersValue);
                        else
                            sb.AppendFormat("{0}<br>", memberEntry.MemberId);
                    }

                    dt.Rows.Add(groupEntry.GroupName, groupEntry.GroupId, groupEntry.Description, sb.ToString());
                }

                gvStep2Results.DataSource = dt;
                gvStep2Results.DataBind();

                mvStep2.SetActiveView(vwStep2Result);
            }
            catch (AppsException a)
            {
                lblStep2Error.Text = string.Format(CultureInfo.CurrentCulture, Resources.GoogleIntegrationControl_GoogleAppsError_Text, a.ErrorCode, a.InvalidInput, a.Reason);
                mvStep2.SetActiveView(vwStep2Error);
            }
            catch (Exception ex)
            {
                lblStep2Error.Text = ex.Message;
                mvStep2.SetActiveView(vwStep2Error);
            }
        }

        protected void lbImportUsers_Click(object sender, EventArgs e)
        {
            try
            {
                AppsService service = new AppsService(txtGoogleDomain.Text, txtGoogleEmail.Text, txtGooglePassword.Text);

                int count = 0;
                int failed = 0;

                GoogleProvider.ImportUsers(UserContext.Current.OrganizationId, service, out count, out failed);

                lbImportUsers.Enabled = false;
                lbImportUsers.Enabled = false;

                lbImportUsers.Text = string.Format(CultureInfo.CurrentCulture, Resources.GoogleIntegrationControl_ImportUsers_Result_Text, count - failed, failed);
            }
            catch (AppsException a)
            {
                lblStep1Error.Text = string.Format(CultureInfo.CurrentCulture, Resources.GoogleIntegrationControl_GoogleAppsError_Text, a.ErrorCode, a.InvalidInput, a.Reason);
                mvStep1.SetActiveView(vwStep1Error);
            }
            catch (Exception ex)
            {
                lblStep1Error.Text = ex.Message;
                mvStep1.SetActiveView(vwStep1Error);
            }
        }

        protected void gvStep2Results_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int j = 0; j < e.Row.Cells.Count; j++)
                {
                    string encoded = e.Row.Cells[j].Text;
                    e.Row.Cells[j].Text = Context.Server.HtmlDecode(encoded);
                }
            }
        }

        #endregion

        #region Overriden Methods

        protected override void LoadResources()
        {
            imgGoogleAppsLogo.ImageUrl = ResourceProvider.GetImageUrl(typeof(GoogleIntegrationControl), "GoogleApps.jpg", true);

            hlGoogleAppsForBusiness.Text = Resources.GoogleIntegrationControl_GoogleAppsForBusiness_Text;

            GoogleIntegrationElement settings = FrameworkConfiguration.Current.WebApplication.Integration.Google;

            hlAddApplication.Text = string.Format(CultureInfo.InvariantCulture, Resources.GoogleIntegrationControl_AddApplication_Text, settings.ApplicationName);
            hlAddApplication.NavigateUrl = settings.ApplicationListingUrl;

            litCaption.Text = Resources.GoogleIntegrationControl_Caption_Text;

            BaseControl.LoadResources(EditForm, typeof(OrganizationLdapSettingsControl).Name);
            lblGoogleDomain.Text = Resources.GoogleIntegrationControl_EditForm_GoogleDomain_HeaderText;
            lblGoogleEmail.Text = Resources.GoogleIntegrationControl_EditForm_GoogleEmail_HeaderText;
            lblGooglePassword.Text = Resources.GoogleIntegrationControl_EditForm_GooglePassword_HeaderText;
            lblTitle.Text = Resources.GoogleIntegrationControl_EditForm_ObjectName;

            lblDescription.Text = Resources.GoogleIntegrationControl_Description_Text;
            lblGoogleSetup.Text = Resources.GoogleIntegrationControl_GoogleSetup_Text;

            lblStep1.Text = Resources.GoogleIntegrationControl_Step1_Text;
            lbStep1.Text = Resources.GoogleIntegrationControl_Step1_LinkButton_Text;

            lbImportUsers.Text = Resources.GoogleIntegrationControl_ImportUsers_LinkButton_Text;

            upStep1Progress.ProgressText = Resources.GoogleIntegrationControl_UpdateProgress_Text;
            upStep1Progress.Timeout = int.MaxValue;
            upStep1Progress.HideAfter = int.MaxValue;
            upStep1Progress.ShowSuccessText = false;
            upStep1Progress.PostBackControlId = this.lbStep1.ClientID;

            lblStep2.Text = Resources.GoogleIntegrationControl_Step2_Text;
            lbStep2.Text = Resources.GoogleIntegrationControl_Step2_LinkButton_Text;

            upStep2Progress.ProgressText = Resources.GoogleIntegrationControl_UpdateProgress_Text;
            upStep2Progress.Timeout = int.MaxValue;
            upStep2Progress.HideAfter = int.MaxValue;
            upStep2Progress.ShowSuccessText = false;
            upStep2Progress.PostBackControlId = this.lbStep2.ClientID;
        }

        #endregion
    }
}
