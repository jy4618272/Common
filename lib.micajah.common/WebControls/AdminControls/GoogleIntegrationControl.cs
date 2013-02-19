using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll.Handlers;
using Micajah.Common.Bll.Providers;
using Micajah.Common.LdapAdapter;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;

using Google.GData.Apps;
using Google.GData.Apps.Groups;
using Google.GData.Extensions;


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

        #endregion

        #region Protected Methods

        protected void lbStep1_Click(object sender, EventArgs e)
        {
            try
            {
                AppsService service = new AppsService(txtGoogleDomain.Text, txtGoogleEmail.Text, txtGooglePassword.Text);
                UserFeed userFeed = service.RetrieveAllUsers();

                DataTable dt = new DataTable();
                dt.Columns.Add("User Name", typeof(string));
                dt.Columns.Add("Given Name", typeof(string));
                dt.Columns.Add("Family Name", typeof(string));
                dt.Columns.Add("Is Admin", typeof(string));

                for (int i = 0; i < userFeed.Entries.Count; i++)
                {
                    UserEntry userEntry = userFeed.Entries[i] as UserEntry;
                    dt.Rows.Add(userEntry.Login.UserName, userEntry.Name.GivenName, userEntry.Name.FamilyName, userEntry.Login.Admin ? "yes" : "no");
                }

                gvStep1Results.DataSource = dt;
                gvStep1Results.DataBind();

                mvStep1.SetActiveView(vwStep1Result);
            }
            catch (AppsException a)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("A Google Apps error occurred.<br>");
                sb.AppendFormat("Error code: {0}<br>", a.ErrorCode);
                sb.AppendFormat("Invalid input: {0}<br>", a.InvalidInput);
                sb.AppendFormat("Reason: {0}<br>", a.Reason);

                lblStep1Error.Text = sb.ToString();
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
                dt.Columns.Add("Group Id", typeof(string));
                dt.Columns.Add("Group Name", typeof(string));
                dt.Columns.Add("Group Description", typeof(string));
                dt.Columns.Add("Group Members", typeof(string));

                for (int i = 0; i < groupsFeed.Entries.Count; i++)
                {
                    GroupEntry groupEntry = groupsFeed.Entries[i] as GroupEntry;
                    MemberFeed memberFeed = service.Groups.RetrieveAllMembers(groupEntry.GroupId);
                    StringBuilder sb = new StringBuilder();

                    for (int j = 0; j < memberFeed.Entries.Count; j++)
                    {
                        MemberEntry memberEntry = memberFeed.Entries[j] as MemberEntry;
                        if (string.Compare(memberEntry.MemberId, "*", true) == 0)
                            sb.AppendFormat("Group contains all users");
                        else
                            sb.AppendFormat("{0}<br>", memberEntry.MemberId);
                    }

                    dt.Rows.Add(groupEntry.GroupId, groupEntry.GroupName, groupEntry.Description, sb.ToString());
                }
                
                gvStep2Results.DataSource = dt;
                gvStep2Results.DataBind();

                mvStep2.SetActiveView(vwStep2Result);
            }
            catch (AppsException a)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("A Google Apps error occurred.<br>");
                sb.AppendFormat("Error code: {0}<br>", a.ErrorCode);
                sb.AppendFormat("Invalid input: {0}<br>", a.InvalidInput);
                sb.AppendFormat("Reason: {0}<br>", a.Reason);

                lblStep2Error.Text = sb.ToString();
                mvStep2.SetActiveView(vwStep2Error);
            }
            catch (Exception ex)
            {
                lblStep2Error.Text = ex.Message;
                mvStep2.SetActiveView(vwStep2Error);
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
            BaseControl.LoadResources(EditForm, typeof(OrganizationLdapSettingsControl).Name);
            lblGoogleDomain.Text = Resources.GoogleIntegrationControl_EditForm_GoogleDomain_HeaderText;
            lblGoogleEmail.Text = Resources.GoogleIntegrationControl_EditForm_GoogleEmail_HeaderText;
            lblGooglePassword.Text = Resources.GoogleIntegrationControl_EditForm_GooglePassword_HeaderText;
            lblTitle.Text = Resources.GoogleIntegrationControl_EditForm_ObjectName;

            lblDescription.Text = Resources.GoogleIntegrationControl_Description_Text;
            lblGoogleSetup.Text = Resources.GoogleIntegrationControl_GoogleSetup_Text;

            lblStep1.Text = Resources.GoogleIntegrationControl_Step1_Text;
            lbStep1.Text = Resources.GoogleIntegrationControl_Step1_LinkButton_Text;

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

        /// <summary>
        /// Raises the PreRender event and registers client scripts.
        /// </summary>
        /// <param name="e">An EventArgs that contains no event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        #endregion
    }
}
