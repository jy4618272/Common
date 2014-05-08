using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls.SecurityControls
{
    /// <summary>
    /// Provides user interface (UI) elements for logging off from a Web site.
    /// </summary>
    public partial class LogOffControl : UserControl
    {
        #region Members

        /// <summary>
        /// The title label.
        /// </summary>
        protected Label TitleLabel;

        /// <summary>
        /// The button to log off.
        /// </summary>
        protected Button LogOffLink;

        /// <summary>
        /// The table row that contains the controls related to organization selection process.
        /// </summary>
        protected PlaceHolder OrganizationArea;

        /// <summary>
        /// The label that displays help about organization selection process.
        /// </summary>
        protected Label OrganizationLabel;

        /// <summary>
        /// The organizations list. Allows switch to another organization.
        /// </summary>
        protected DataList OrganizationList;

        /// <summary>
        /// The div to display an error message, if an error occured.
        /// </summary>
        protected HtmlGenericControl ErrorPanel;

        protected PlaceHolder InstanceArea;
        protected Label InstanceDescriptionLabel;
        protected HtmlGenericControl InstanceListContainer;
        protected DataList InstanceList;
        protected HtmlGenericControl OrganizationListContainer;

        #endregion

        #region Protected Methods

        /// <summary>
        /// Occurs when the page is being loaded.
        /// </summary>
        /// <param name="sender">The sourceRow of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Micajah.Common.Pages.MasterPage.CreatePageHeader(this.Page, false, false);

            if (!IsPostBack)
            {
                UserContext user = UserContext.Current;

                if (user.OrganizationId == null)
                    Response.Redirect(ResourceProvider.GetActiveOrganizationUrl(Request.Url.PathAndQuery));

                Micajah.Common.Pages.MasterPage.SetPageTitle(this.Page, ActionProvider.GlobalNavigationLinks.FindByActionId(ActionProvider.LogOffGlobalNavigationLinkActionId));

                OrganizationArea.Visible = false;
                InstanceArea.Visible = false;

                InstanceCollection instances = LoginProvider.Current.GetLoginInstances(user.UserId, user.OrganizationId);
                OrganizationCollection orgs = LoginProvider.Current.GetOrganizationsByLoginId(user.UserId);
                int instCount = instances.Count;
                int orgsCount = 0;
                if (orgs != null)
                {
                    orgsCount = orgs.Count;
                    if (orgsCount > 1)
                    {
                        orgs = orgs.FindAllVisible();
                        orgsCount = orgs.Count;
                        if (orgsCount == 1)
                        {
                            if (orgs[0].OrganizationId != user.OrganizationId)
                                orgsCount = 2;
                        }
                    }
                }

                if (user.InstanceId == Guid.Empty)
                    instCount = 0;
                else
                {
                    if (instCount > 1)
                    {
                        InstanceDescriptionLabel.Text = Resources.LogOffControl_InstanceDescriptionLabel_Text;

                        InstanceList.DataSource = instances;
                        InstanceList.DataBind();

                        InstanceArea.Visible = true;
                    }
                }

                if (orgsCount > 1)
                {
                    OrganizationLabel.Text = Resources.LogOffControl_OrganizationLabel_Text;

                    orgs.SortByExpiration();

                    OrganizationList.DataSource = orgs;
                    OrganizationList.DataBind();

                    OrganizationArea.Visible = true;
                }
                else if (!InstanceArea.Visible)
                {
                    LoginProvider.Current.SignOut();

                    return;
                }

                TitleLabel.Text = Resources.LogOffControl_TitleLabel_Text;
                LogOffLink.Text = Resources.LogoffControl_LogoffLink_Text;
            }

            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern)
                this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.LogOnModernStyleSheet, true)));
            else
                this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.LogOnStyleSheet, true)));
        }

        /// <summary>
        /// Occurs when the hyperlink to logoff is clicked.
        /// Cancels the current session, removes the forms-authentication ticket from the browser and redirects to the login page.
        /// </summary>
        /// <param name="sender">The sourceRow of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        protected void LogOffLink_Click(object sender, EventArgs e)
        {
            LoginProvider.Current.SignOut();
        }

        protected void InstanceList_ItemCommand(object source, CommandEventArgs e)
        {
            if (e == null) return;
            if (e.CommandName.Equals("Select"))
                ActiveInstanceControl.SelectInstance((Guid)Support.ConvertStringToType(e.CommandArgument.ToString(), typeof(Guid)), null, false, ErrorPanel);
        }

        protected void OrganizationList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            ActiveOrganizationControl.OrganizationListItemDataBound(e);
        }

        /// <summary>
        /// Occurs when an organization hyperlink is clicked.
        /// Switches to selected organization and redirects to the default page. 
        /// </summary>
        /// <param name="sourceRow">The sourceRow of the event.</param>
        /// <param name="e">An DataListCommandEventArgs that contains event data.</param>
        protected void OrganizationList_ItemCommand(object source, CommandEventArgs e)
        {
            if (e == null) return;
            if (e.CommandName.Equals("Select"))
                ActiveOrganizationControl.SelectOrganization((Guid)Support.ConvertStringToType(e.CommandArgument.ToString(), typeof(Guid)), null, false, ErrorPanel);
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Renders the control.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            LogOnControl.RenderHeader(writer);
            base.Render(writer);
        }

        #endregion
    }
}
