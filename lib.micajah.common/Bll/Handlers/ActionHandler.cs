using Micajah.Common.Application;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Micajah.Common.Security;

namespace Micajah.Common.Bll.Handlers
{
    /// <summary>
    /// The class that provides a mechanism to get the name and description of the action.
    /// </summary>
    public class ActionHandler
    {
        #region Members

        private static ActionHandler s_Instance;
        private static ActionHandler s_Current;

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the instance of this class.
        /// </summary>
        internal static ActionHandler Instance
        {
            get
            {
                if (s_Instance == null) s_Instance = new ActionHandler();
                return s_Instance;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the instance of the class.
        /// </summary>
        public static ActionHandler Current
        {
            get { return ((s_Current == null) ? Instance : s_Current); }
            set { s_Current = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the value indicating that the access to the specified action is denied.
        /// </summary>
        /// <param name="action">The action to check access to.</param>
        /// <returns>true, if the access to the action is denied; otherwise, false.</returns>
        public virtual bool AccessDenied(Action action)
        {
            bool accessDenied = false;

            if (action != null)
            {
                if (action.ActionId == ActionProvider.SettingsDiagnosticPageActionId)
                    accessDenied = ActionProvider.AccessDeniedToSettingsDiagnosticPage();
                else if ((action.ActionId == ActionProvider.SetupEntitiesPageActionId)
                    || (action.ActionId == ActionProvider.EntitiesFieldsPageActionId)
                    || (action.ActionId == ActionProvider.EntityFieldsPageActionId))
                    accessDenied = (WebApplication.Entities.FindAllByEnableHierarchy(false).Count == 0);
                else if (action.ActionId == ActionProvider.UserAssociateToOrganizationStructurePageActionId)
                    accessDenied = (WebApplication.Entities["4cda22f3-4f01-4768-8608-938dc6a06825"] == null);
                else if ((action.ActionId == ActionProvider.RulesEnginePageActionId)
                    || (action.ActionId == ActionProvider.RulesPageActionId)
                    || (action.ActionId == ActionProvider.RuleParametersPageActionId))
                    accessDenied = (WebApplication.RulesEngines.Count == 0);
                else if (action.ActionId == ActionProvider.TreesPageActionId)
                    accessDenied = (WebApplication.Entities.FindAllByEnableHierarchy(true).Count == 0);
                else if (action.ActionId == ActionProvider.InstancesPageActionId)
                    accessDenied = (!FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances);
                else if (action.ActionId == ActionProvider.CustomUrlsPageActionId)
                    accessDenied = (!FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled);
                else if (action.ActionId == ActionProvider.StartPageActionId)
                    Micajah.Common.WebControls.AdminControls.StartControl.GetStartMenuCheckedItems(UserContext.Current, out accessDenied);
                else if ((action.ActionId == ActionProvider.LdapIntegrationPageActionId)
                    || (action.ActionId == ActionProvider.LdapGroupMappingsPageActionId)
                    || (action.ActionId == ActionProvider.LdapMappingsPageActionId)
                    || (action.ActionId == ActionProvider.LdapServerSettingsPageActionId)
                    || (action.ActionId == ActionProvider.LdapUserInfoPageActionId))
                    accessDenied = !(FrameworkConfiguration.Current.WebApplication.Integration.Ldap.Enabled && UserContext.Current.SelectedOrganization.Beta);
                else if (action.ActionId == ActionProvider.MyAccountGlobalNavigationLinkActionId || action.ActionId == ActionProvider.MyAccountPageActionId)
                    accessDenied = ((UserContext.Current != null) && (UserContext.Current.SelectedOrganization == null));
                else if (action.ActionId == ActionProvider.GoogleIntegrationPageActionId)
                    accessDenied = !FrameworkConfiguration.Current.WebApplication.Integration.Google.Enabled;
                else if (action.ActionId == ActionProvider.ActivityReportActionId)
                    accessDenied = !FrameworkConfiguration.Current.WebApplication.Integration.Chargify.Enabled;
            }

            return accessDenied;
        }

        /// <summary>
        /// Returns the name of the action.
        /// </summary>
        /// <param name="action">The action to get name of.</param>
        /// <returns>The System.String that represents the name of the action.</returns>
        public virtual string GetName(Action action)
        {
            if (action == null)
                return null;

            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme != MasterPageTheme.Modern)
            {
                if ((action.ActionId == ActionProvider.MyAccountGlobalNavigationLinkActionId) || (action.ActionId == ActionProvider.MyAccountPageActionId))
                {
                    UserContext user = UserContext.Current;
                    if (user != null)
                    {
                        string name = user.FirstName + System.Web.UI.Html32TextWriter.SpaceChar + user.LastName;
                        return ((name.Trim().Length > 0) ? name : user.LoginName);
                    }
                }
            }

            return action.Name;
        }

        /// <summary>
        /// Returns the description of the action.
        /// </summary>
        /// <returns>The System.String that represents the description of the action.</returns>
        public virtual string GetDescription(Action action)
        {
            if (action == null)
                return null;
            return action.Description;
        }

        /// <summary>
        /// Returns the URL of the action to navigate.
        /// </summary>
        /// <param name="action">The action to get URL of.</param>
        /// <returns>The System.String that represents the URL of the action to navigate.</returns>
        public virtual string GetNavigateUrl(Action action)
        {
            if (action == null)
                return null;

            if ((action.ActionId == ActionProvider.ConfigurationPageActionId) || (action.ActionId == ActionProvider.ConfigurationGlobalNavigationLinkActionId))
            {
                if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern)
                    return CustomUrlProvider.CreateApplicationRelativeUrl(ResourceProvider.AccountSettingsVirtualPath);
            }

            return action.NavigateUrl;
        }

        #endregion
    }
}
