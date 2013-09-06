using System;
using System.Globalization;
using System.Security.Authentication;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;
using Micajah.Common.Security;

namespace Micajah.Common.WebControls.SecurityControls
{
    /// <summary>
    /// Provides user interface elements to select active organization.
    /// </summary>
    public class ActiveOrganizationControl : UserControl
    {
        #region Members

        /// <summary>
        /// The div to display an error message, if an error occured.
        /// </summary>
        protected HtmlGenericControl ErrorDiv;

        /// <summary>
        /// The container control for controls related to organization selection process.
        /// </summary>
        protected PlaceHolder OrganizationArea;

        /// <summary>
        /// The label that displays help about selection process.
        /// </summary>
        protected Label DescriptionLabel;

        /// <summary>
        /// The organizations list. Allows select one organization.
        /// </summary>
        protected DataList OrganizationList;

        protected HtmlGenericControl OrganizationListContainer;
        protected Label OrLabel1;
        protected Label OrLabel2;
        protected Label OrLabel3;

        /// <summary>
        /// The button to log out.
        /// </summary>
        protected LinkButton LogOffLink;

        /// <summary>
        /// The control that contains the hyperlink to the setup page.
        /// </summary>
        protected Control SetupLinkContainer;

        /// <summary>
        /// The hyperlink to the setup page that is displayed only for the framework administrator.
        /// </summary>
        protected HyperLink SetupLink;

        /// <summary>
        /// The control that contains the hyperlink to Log In As Another User page.
        /// </summary>
        protected Control LogOnAsAnotherUserLinkContainer;

        /// <summary>
        /// The hyperlink to Log In As Another User page.
        /// </summary>
        protected HyperLink LogOnAsAnotherUserLink;

        #endregion

        #region Internal Methods

        /// <summary>
        /// Stores the specified organization identifier and redirects to originally requested URL.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        internal static void SelectOrganization(Guid organizationId, string redirectUrl, bool validateRedirectUrl, HtmlGenericControl errorDiv)
        {
            try
            {
                if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
                {
                    if (validateRedirectUrl)
                        ActiveInstanceControl.ValidateRedirectUrl(ref redirectUrl, true);

                    errorDiv.Page.Session.Clear();

                    errorDiv.Page.Response.Redirect(CustomUrlProvider.GetVanityUri(organizationId, Guid.Empty, redirectUrl));
                }
                else
                {
                    UserContext.Current.SelectOrganization(organizationId);

                    //if (validateRedirectUrl)
                    ActiveInstanceControl.ValidateRedirectUrl(ref redirectUrl, true);

                    if (!string.IsNullOrEmpty(redirectUrl))
                        errorDiv.Page.Response.Redirect(redirectUrl);
                }
            }
            catch (AuthenticationException ex)
            {
                ActiveInstanceControl.ShowError(ex.Message, errorDiv);
            }
        }

        internal static void OrganizationListItemDataBound(DataListItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    Organization org = e.Item.DataItem as Organization;
                    if (org != null)
                    {
                        if (org.Expired)
                        {
                            int days = org.GraceDaysRemaining;
                            if (days > 0)
                            {
                                HyperLink expirationLink = e.Item.FindControl("ExpirationLink") as HyperLink;
                                if (expirationLink != null)
                                {
                                    expirationLink.Text = string.Format(CultureInfo.InvariantCulture, Resources.ActiveOrganizationControl_OrganizationList_ExpirationLink_Text, days);
                                    expirationLink.NavigateUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.SupportPageVirtualPath) + "?o=" + org.OrganizationId.ToString("N");
                                    expirationLink.Visible = true;
                                }
                            }
                            else
                            {
                                Label expiredLabel = e.Item.FindControl("ExpiredLabel") as Label;
                                if (expiredLabel != null)
                                {
                                    expiredLabel.Text = Resources.ActiveOrganizationControl_OrganizationList_ExpiredLabel_Text;
                                    expiredLabel.Visible = true;
                                }

                                LinkButton orgButton = e.Item.FindControl("OrgButton") as LinkButton;
                                if (orgButton != null) orgButton.Visible = false;

                                HyperLink orgLink = e.Item.FindControl("OrgLink") as HyperLink;
                                if (orgLink != null)
                                {
                                    orgLink.NavigateUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.SupportPageVirtualPath) + "?o=" + org.OrganizationId.ToString("N");
                                    orgLink.Visible = true;
                                }
                            }
                        }
                    }
                    break;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Occurs when the page is being loaded.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Micajah.Common.Pages.MasterPage.CreatePageHeader(this.Page, false);

            if (!IsPostBack)
            {
                Micajah.Common.Bll.Action action = ActionProvider.FindAction(CustomUrlProvider.CreateApplicationAbsoluteUrl(Request.Url.PathAndQuery));
                Micajah.Common.Pages.MasterPage.SetPageTitle(this.Page, action);

                if (string.Compare(Request.QueryString["ao"], "1", StringComparison.OrdinalIgnoreCase) == 0)
                    ActiveInstanceControl.ShowError(Resources.ActiveOrganizationControl_YouAreLoggedIntoAnotherOrganization, ErrorDiv);

                LogOffLink.Text = Resources.ActiveOrganizationControl_LogoffLink_Text;

                UserContext user = UserContext.Current;

                if (user.IsFrameworkAdministrator)
                {
                    OrLabel1.Text = Resources.ActiveOrganizationControl_OrText;
                    SetupLinkContainer.Visible = true;
                    SetupLink.Text = Resources.ActiveOrganizationControl_SetupLink_Text;
                    action = ActionProvider.PagesAndControls.FindByActionId(ActionProvider.SetupPageActionId);
                    if (action != null) SetupLink.NavigateUrl = action.AbsoluteNavigateUrl;
                }

                if (user.CanLogOnAsUser)
                {
                    OrLabel2.Text = Resources.ActiveOrganizationControl_OrText;
                    LogOnAsAnotherUserLinkContainer.Visible = true;
                    LogOnAsAnotherUserLink.Text = Resources.ActiveOrganizationControl_LogOnAsAnotherUserLink_Text;
                    action = ActionProvider.GlobalNavigationLinks.FindByActionId(ActionProvider.LoginAsUserGlobalNavigationLinkActionId);
                    if (action != null) LogOnAsAnotherUserLink.NavigateUrl = action.AbsoluteNavigateUrl;
                }

                OrganizationCollection coll = WebApplication.LoginProvider.GetOrganizationsByLoginId(user.UserId);
                int count = 0;
                if (coll != null)
                {
                    count = coll.Count;
                    if (count > 1)
                    {
                        coll = coll.FindAllVisible();
                        count = coll.Count;
                        if (count == 1)
                        {
                            if (user.SelectedOrganization != null)
                            {
                                if (coll[0].OrganizationId != user.SelectedOrganization.OrganizationId)
                                    count = 2;
                            }
                        }
                    }
                }

                if (count == 0)
                {
                    if (user.IsFrameworkAdministrator)
                        OrLabel1.Visible = false;
                    else if (user.CanLogOnAsUser)
                        OrLabel2.Visible = false;
                }

                if (count == 0)
                {
                    ActiveInstanceControl.ShowError(Resources.UserContext_ErrorMessage_YouAreNotAssociatedWithOrganizations, ErrorDiv);
                }
                else if ((count == 1) && (!user.CanLogOnAsUser))
                {
                    OrganizationArea.Visible = false;
                    OrLabel3.Visible = false;
                    ErrorDiv.Style.Add(HtmlTextWriterStyle.PaddingBottom, "7px");
                    SelectOrganization(coll[0].OrganizationId, Request.QueryString["returnurl"], true, ErrorDiv);
                }
                else
                {
                    DescriptionLabel.Text = Resources.ActiveOrganizationControl_DescriptionLabel_Text;
                    OrLabel3.Text = Resources.ActiveOrganizationControl_OrText;

                    coll.SortByExpiration();

                    OrganizationList.DataSource = coll;
                    OrganizationList.DataBind();
                }
            }

            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern)
                this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.LogOnModernStyleSheet, true)));
            else
                this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.LogOnStyleSheet, true)));
        }

        protected void OrganizationList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            OrganizationListItemDataBound(e);
        }

        protected void OrganizationList_ItemCommand(object source, CommandEventArgs e)
        {
            if (e == null) return;
            if (e.CommandName.Equals("Select"))
                SelectOrganization((Guid)Support.ConvertStringToType(e.CommandArgument.ToString(), typeof(Guid)), Request.QueryString["returnurl"], true, ErrorDiv);
        }

        protected void LogOffLink_Click(object sender, EventArgs e)
        {
            WebApplication.LoginProvider.SignOut();
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Renders the page.
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
