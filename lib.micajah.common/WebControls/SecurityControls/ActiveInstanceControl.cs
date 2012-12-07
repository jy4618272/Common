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
    /// Provides user interface elements to select active instance.
    /// </summary>
    public class ActiveInstanceControl : UserControl
    {
        #region Members

        /// <summary>
        /// The div to display an error message, if an error occured.
        /// </summary>
        protected HtmlGenericControl ErrorDiv;

        /// <summary>
        /// The container control for controls related to instance selection process.
        /// </summary>
        protected PlaceHolder InstanceArea;

        /// <summary>
        /// The label that displays help about instance selection process.
        /// </summary>
        protected Label DescriptionLabel;

        /// <summary>
        /// The instances list. Allows select one instance.
        /// </summary>
        protected DataList InstanceList;

        protected HtmlGenericControl InstanceListContainer;
        protected Label LogOffDescriptionLabel;
        protected HyperLink LogOffLink;

        #endregion

        #region Internal Methods

        /// <summary>
        /// Selectes the specified instance and redirects to specified URL.
        /// </summary>
        /// <param name="instanceId">The instance identifier to select.</param>
        /// <param name="redirectUrl">The URL to redirect to.</param>
        internal static void SelectInstance(Guid instanceId, string redirectUrl, HtmlGenericControl errorDiv)
        {
            try
            {
                UserContext user = UserContext.Current;

                user.SelectInstance(instanceId);

                ValidateRedirectUrl(ref redirectUrl, ((ActionProvider.StartPageSettingsLevels & SettingLevels.Instance) == SettingLevels.Instance));

                if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
                    errorDiv.Page.Response.Redirect(CustomUrlProvider.GetVanityUri(user.SelectedOrganizationId, instanceId, redirectUrl));
                else if (!string.IsNullOrEmpty(redirectUrl))
                    errorDiv.Page.Response.Redirect(redirectUrl);
            }
            catch (AuthenticationException ex)
            {
                ShowError(ex.Message, errorDiv);
            }
        }

        internal static bool ShowError(string errorMessage, HtmlGenericControl errorDiv)
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                errorDiv.Visible = true;
                if (string.IsNullOrEmpty(errorDiv.InnerHtml))
                    errorDiv.InnerHtml = errorMessage;
                else
                    errorDiv.InnerHtml += "<div style='padding-top: 17px;'>" + errorMessage + "</div>";

                return true;
            }
            return false;
        }

        internal static void ValidateRedirectUrl(ref string redirectUrl, bool enableStartMenu)
        {
            UserContext user = UserContext.Current;

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                string relativeUrl = CustomUrlProvider.CreateApplicationRelativeUrl(redirectUrl);
                if (string.Compare(relativeUrl, "/", StringComparison.OrdinalIgnoreCase) == 0)
                    redirectUrl = null;
                else
                {
                    Guid actionId = Guid.Empty;
                    object obj = Support.ConvertStringToType(Support.ExtractQueryStringParameterValue(redirectUrl, "pageid"), typeof(Guid));
                    if (obj != null) actionId = (Guid)obj;

                    if (user != null && user.SelectedOrganization != null)
                    {
                        Micajah.Common.Bll.Action action = ActionProvider.FindAction(actionId, redirectUrl);
                        if (action != null)
                        {
                            if (!user.ActionIdList.Contains(action.ActionId))
                                redirectUrl = null;
                        }
                        else
                            redirectUrl = null;
                    }
                }
            }

            if (enableStartMenu)
            {
                if (user != null && user.IsOrganizationAdministrator)
                {
                    bool redirect = false;
                    Micajah.Common.WebControls.AdminControls.StartControl.GetStartMenuCheckedItems(user, out redirect);
                    if (!redirect)
                        redirectUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.StartPageVirtualPath);
                }
            }

            if (string.IsNullOrEmpty(redirectUrl))
                redirectUrl = user.StartPageUrl;
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
                UserContext user = UserContext.Current;

                if (user.SelectedOrganization == null)
                    Response.Redirect(ResourceProvider.GetActiveOrganizationUrl(Request.Url.PathAndQuery));

                Micajah.Common.Bll.Action action = ActionProvider.FindAction(CustomUrlProvider.CreateApplicationAbsoluteUrl(Request.Url.PathAndQuery));
                Micajah.Common.Pages.MasterPage.SetPageTitle(this.Page, action);

                if (string.Compare(Request.QueryString["ai"], "1", StringComparison.OrdinalIgnoreCase) == 0)
                    ShowError(Resources.ActiveInstanceControl_YouAreLoggedIntoAnotherInstance, ErrorDiv);

                LogOffLink.Text = Resources.ActiveInstanceControl_LogoffLink_Text;

                action = ActionProvider.GlobalNavigationLinks.FindByActionId(ActionProvider.LogOffGlobalNavigationLinkActionId);
                LogOffLink.NavigateUrl = ((action == null) ? ResourceProvider.LogOffPageVirtualPath : action.AbsoluteNavigateUrl);

                InstanceCollection coll = WebApplication.LoginProvider.GetLoginInstances(user.UserId, user.SelectedOrganization.OrganizationId);
                int count = 0;

                if (coll != null)
                    count = coll.Count;

                if (count == 0)
                {
                    ShowError(
                        (user.IsOrganizationAdministrator
                            ? Resources.UserContext_ErrorMessage_YouAreNotAssociatedWithInstances + "<br />"
                                + string.Format(CultureInfo.InvariantCulture, Resources.ActiveInstanceControl_ConfigureOrganization, ResourceProvider.GetDetailMenuPageUrl(ActionProvider.ConfigurationPageActionId))
                            : Resources.UserContext_ErrorMessage_YouAreNotAssociatedWithInstances)
                        , ErrorDiv);
                }
                else if (count == 1)
                {
                    InstanceArea.Visible = false;
                    LogOffDescriptionLabel.Visible = false;
                    SelectInstance(coll[0].InstanceId, Request.QueryString["returnurl"], ErrorDiv);
                }
                else
                {
                    DescriptionLabel.Text = Resources.ActiveInstanceControl_DescriptionLabel_Text;
                    LogOffDescriptionLabel.Text = Resources.ActiveInstanceControl_LogoffDescriptionLabel_Text;

                    InstanceList.DataSource = coll;
                    InstanceList.DataBind();
                }
            }

            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern)
                this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.LogOnModernStyleSheet, true)));
            else
                this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.LogOnStyleSheet, true)));
        }

        protected void InstanceList_ItemCommand(object source, CommandEventArgs e)
        {
            if (e == null) return;
            if (e.CommandName.Equals("Select"))
                SelectInstance((Guid)Support.ConvertStringToType(e.CommandArgument.ToString(), typeof(Guid)), Request.QueryString["returnurl"], ErrorDiv);
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
