using Google.GData.Apps;
using Google.GData.Apps.Groups;
using Google.GData.Client;
using Google.GData.Extensions.Apps;
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
using System.Web.UI.HtmlControls;
using System.Net;

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

        protected MultiView mvConnectionSettings;
        protected View vwForm;
        protected View vwToken;
        protected Button btnSaveToken;
        protected Button btnChangeToken;
        protected Label lblTokenDescription;
        protected HtmlTableRow trFormError;
        protected Label lblFromError;
        protected Button btnCancel;
        protected Label lblYourGoogleDomain;
        protected DropDownList ddlDomains;
        protected HtmlTableRow trGoogleDomain;

        #endregion

        #region Protected Methods

        protected void lbStep1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddlDomains.SelectedValue))
            {
                try
                {
                    AppsService service = new AppsService(ddlDomains.SelectedValue, UserContext.Current.Organization.GoogleAdminAuthToken);
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
                    ShowError(ex, lblStep1Error, mvStep1, vwStep1Error);
                }
            }
            else
            {
                lblStep1Error.Text = Resources.GoogleIntegrationControl_DomainMisingError_Text;
                mvStep1.SetActiveView(vwStep1Error);
            }
        }

        protected void lbStep2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddlDomains.SelectedValue))
            {
                try
                {
                    AppsService service = new AppsService(ddlDomains.SelectedValue, UserContext.Current.Organization.GoogleAdminAuthToken);
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
                    ShowError(ex, lblStep2Error, mvStep2, vwStep2Error);
                }
            }
            else
            {
                lblStep2Error.Text = Resources.GoogleIntegrationControl_DomainMisingError_Text;
                mvStep2.SetActiveView(vwStep2Error);
            }

        }

        protected void lbImportUsers_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddlDomains.SelectedValue))
            {
                try
                {
                    AppsService service = new AppsService(ddlDomains.SelectedValue, UserContext.Current.Organization.GoogleAdminAuthToken);

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
                    ShowError(ex, lblStep1Error, mvStep1, vwStep1Error);                    
                }
            }
            else
            {
                lblStep1Error.Text = Resources.GoogleIntegrationControl_DomainMisingError_Text;
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

        protected void btnChangeToken_Click(object sender, EventArgs e)
        {
            ShowTokenForm();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mvStep1.ActiveViewIndex = -1;
            mvStep2.ActiveViewIndex = -1;
            mvConnectionSettings.SetActiveView(vwToken);
        }

        public void ShowTokenForm()
        {
            FillDomains();
            trFormError.Visible = false;
            trGoogleDomain.Visible = ddlDomains.Items.Count == 0;
            mvConnectionSettings.SetActiveView(vwForm);
        }

        protected void btnSaveToken_Click(object sender, EventArgs e)
        {
            trFormError.Visible = false;
            mvStep1.ActiveViewIndex = -1;
            mvStep2.ActiveViewIndex = -1;
            try
            {
                Service service = new Service(AppsNameTable.GAppsService, "sherpadesk");
                string token = string.Empty;

                if (trGoogleDomain.Visible)
                {
                    string googleDomain = txtGoogleDomain.Text;
                    AppsService appService = new AppsService(googleDomain, txtGoogleEmail.Text, txtGooglePassword.Text);                    
                    GroupFeed groups = appService.Groups.RetrieveAllGroups();

                    EmailSuffixProvider.InsertEmailSuffixName(UserContext.Current.OrganizationId, null, ref googleDomain);
                    FillDomains();
                }                

                service.setUserCredentials(txtGoogleEmail.Text, txtGooglePassword.Text);
                token = service.QueryClientLoginToken();
                OrganizationProvider.UpdateOrganizationGoogleAdminAuthToken(UserContext.Current.OrganizationId, token);                    
                mvConnectionSettings.SetActiveView(vwToken);
            }
            catch (AppsException a)
            {
                lblFromError.Text = string.Format(CultureInfo.CurrentCulture, Resources.GoogleIntegrationControl_GoogleAppsError_Text, a.ErrorCode, a.InvalidInput, a.Reason);
                trFormError.Visible = true;
            }
            catch (Exception ex)
            {
                lblFromError.Text = ex.Message;
                trFormError.Visible = true;
            }
        }

        private void FillDomains()
        {
            ddlDomains.Items.Clear();

            if (UserContext.Current != null)
            {
                var list = EmailSuffixProvider.GetEmailSuffixesList(UserContext.Current.OrganizationId);

                if (list != null)
                {
                    foreach (string domain in list)
                    {
                        ddlDomains.Items.Add(new ListItem() { Text = domain, Value = domain });
                    }
                }
            }
        }

        private void ShowError(Exception ex, Label lbl, MultiView mv, View vw)
        {
            lbl.Text = ex.Message;
            mv.SetActiveView(vw);

            if (ex is Google.GData.Client.GDataRequestException)
            {
                GDataRequestException googleException = ex as Google.GData.Client.GDataRequestException;
                if (googleException != null && googleException.InnerException != null)
                {
                    lbl.Text = googleException.InnerException.Message;
                }
            }

            if (lbl.Text.Contains("401")) 
            {
                lbl.Text += Resources.GoogleIntegrationControl_TokenError_Text;
            }

            if (lbl.Text.Contains("403"))
            {
                lbl.Text += string.Format(Resources.GoogleIntegrationControl_DomainError_Text, ResolveUrl("~/mc/admin/organizationprofile.aspx"));
            }
        }

        #endregion

        #region Overriden Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            btnCancel.Visible = UserContext.Current != null && UserContext.Current.Organization != null && !string.IsNullOrWhiteSpace(UserContext.Current.Organization.GoogleAdminAuthToken);

            if (!IsPostBack)
            {
                FillDomains();
                if (UserContext.Current != null && UserContext.Current.Organization != null && !string.IsNullOrWhiteSpace(UserContext.Current.Organization.GoogleAdminAuthToken))
                {
                    mvConnectionSettings.SetActiveView(vwToken);
                }
                else
                {
                    ShowTokenForm();
                }
            }
        }

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

            btnSaveToken.Text = Resources.GoogleIntegrationControl_SaveTokenButton_Text;
            btnChangeToken.Text = Resources.GoogleIntegrationControl_ChangTokenButton_Text;
            lblTokenDescription.Text = Resources.GoogleIntegrationControl_TokenDescription_Text;
            btnCancel.Text = Resources.AutoGeneratedButtonsField_CancelButton_Text;

            lblYourGoogleDomain.Text = Resources.GoogleIntegrationControl_YourGoogleDomain_Text;
        }

        #endregion
    }
}
