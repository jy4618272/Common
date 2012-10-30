using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Pages;
using Micajah.Common.Security;
using Micajah.Common.WebControls;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with actions.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class ActionProvider
    {
        #region Members

        // The built-in actions.
        internal readonly static Guid GlobalNavigationLinksActionId = new Guid("00000000-0000-0000-0000-000000000001");
        internal readonly static Guid ConfigurationGlobalNavigationLinkActionId = new Guid("00000000-0000-0000-0000-000000000002");
        internal readonly static Guid MyAccountGlobalNavigationLinkActionId = new Guid("00000000-0000-0000-0000-000000000005");
        internal readonly static Guid LogOffGlobalNavigationLinkActionId = new Guid("00000000-0000-0000-0000-000000000006");
        internal readonly static Guid PagesAndControlsActionId = new Guid("00000000-0000-0000-0000-000000000007");
        internal readonly static Guid SetupEntitiesPageActionId = new Guid("00000000-0000-0000-0000-000000000008");
        internal readonly static Guid ConfigurationPageActionId = new Guid("00000000-0000-0000-0000-000000000009");
        internal readonly static Guid InstancesPageActionId = new Guid("00000000-0000-0000-0000-000000000010");
        internal readonly static Guid UsersPageActionId = new Guid("00000000-0000-0000-0000-000000000012");
        internal readonly static Guid SettingsDiagnosticPageActionId = new Guid("00000000-0000-0000-0000-000000000018");
        internal readonly static Guid MyAccountPageActionId = new Guid("00000000-0000-0000-0000-000000000021");
        internal readonly static Guid MyNameAndEmailPageActionId = new Guid("00000000-0000-0000-0000-000000000022");
        internal readonly static Guid MyPasswordPageActionId = new Guid("00000000-0000-0000-0000-000000000023");
        internal readonly static Guid UserNameAndEmailPageActionId = new Guid("00000000-0000-0000-0000-000000000024");
        internal readonly static Guid UserPasswordPageActionId = new Guid("00000000-0000-0000-0000-000000000025");
        internal readonly static Guid UserGroupsPageActionId = new Guid("00000000-0000-0000-0000-000000000026");
        internal readonly static Guid UserActivateInactivatePageActionId = new Guid("97BE82B7-48B7-4A9C-BD14-6A2494EF2AA7");
        internal readonly static Guid UserAssociateToOrganizationStructurePageActionId = new Guid("DAB9B65E-0358-408E-A2F1-2D616FCA33EC");
        internal readonly static Guid SetupPageActionId = new Guid("00000000-0000-0000-0000-000000000027");
        internal readonly static Guid LoginAsUserGlobalNavigationLinkActionId = new Guid("709A6B39-36A2-43FC-AF18-24C2E3332D7A");
        internal readonly static Guid OrganizationsPageActionId = new Guid("00000000-0000-0000-0000-000000000040");
        internal readonly static Guid TreesPageActionId = new Guid("B3CCC73F-7194-4F0A-AABB-77AE91E31CE9");
        internal readonly static Guid LoginAsUserPageActionId = new Guid("00000000-0000-0000-0000-000000000041");
        internal readonly static Guid SetupGlobalNavigationLinkActionId = new Guid("00000000-0000-0000-0000-000000000042");
        internal readonly static Guid LoginGlobalNavigationLinkActionId = new Guid("00000000-0000-0000-0000-000000000043");
        internal readonly static Guid TreePageActionId = new Guid("7D20C2C0-09DC-4399-89D0-FE16757FF169");
        internal readonly static Guid EntitiesFieldsPageActionId = new Guid("5A25507B-F8B2-4D5C-AC70-71A0DD5C8729");
        internal readonly static Guid EntityFieldsPageActionId = new Guid("6E03B1C3-7AD0-460B-BF73-E3F9055A71DE");
        internal readonly static Guid NodeTypePageActionId = new Guid("8375FC0A-4A82-46F1-B9D8-E53918E4FB42");
        internal readonly static Guid EntityFieldListsValuesPageActionId = new Guid("A058B233-203B-472C-8D15-86A353ADD11A");
        internal readonly static Guid RulesEnginePageActionId = new Guid("D63CAC58-9B6E-46A4-B845-68332F759DFB");
        internal readonly static Guid RulesPageActionId = new Guid("BB0239EC-6259-4D7A-B392-FA22ED8FEF65");
        internal readonly static Guid RuleParametersPageActionId = new Guid("CD1EBE2B-C229-421C-847A-6FF560665D1A");
        internal readonly static Guid CustomUrlsPageActionId = new Guid("291F64C6-EF01-4D22-864D-AE3B4FF92D38");
        internal readonly static Guid LdapIntegrationPageActionId = new Guid("A3223AB8-D9E7-437A-AE0E-14164C0F90B7");
        internal readonly static Guid LdapMappingsPageActionId = new Guid("14C1A008-D65D-40BB-BFF8-D077BA1DE995");
        internal readonly static Guid LdapServerSettingsPageActionId = new Guid("F53C5189-BD4D-47E5-88EF-58B81178F7EF");
        internal readonly static Guid LdapGroupMappingsPageActionId = new Guid("72B411D0-FB81-4444-B217-AFC4416A5319");
        internal readonly static Guid LdapUserInfoPageActionId = new Guid("5749FF70-4592-4B91-8579-5E7E203C0410");
        internal readonly static Guid SignUpOrganizationPageActionId = new Guid("3E9E3609-7F2A-4DF2-92DF-DFFBD6978E84");
        internal readonly static Guid StartPageActionId = new Guid("4CB3BB95-A829-4DF7-BAD8-05FB38FF019A");
        internal readonly static Guid AccountSettingsPageActionId = new Guid("BA74DDA9-A12F-4987-BE66-45137F2F21B2");

        // The objects which are used to synchronize access to the cached collections and lists.
        private static readonly object s_GlobalNavigationLinksSyncRoot = new object();
        private static readonly object s_MyAccountActionIdListSyncRoot = new object();
        private static readonly object s_PagesAndControlsSyncRoot = new object();
        private static readonly object s_PublicActionsSyncRoot = new object();
        private static readonly object s_SettingsActionIdListSyncRoot = new object();
        private static readonly object s_SetupActionIdListSyncRoot = new object();
        private static readonly object s_RoleActionIdListSyncRoot = new object();
        private static readonly object s_GroupInstanceActionIdListSyncRoot = new object();

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets a collection of the pages that are available only in development mode.
        /// </summary>
        private static ArrayList SetupActionIdList
        {
            get
            {
                ArrayList list = CacheManager.Current.Get("mc.SetupActionIdList") as ArrayList;
                if (list == null)
                {
                    lock (s_SetupActionIdListSyncRoot)
                    {
                        list = CacheManager.Current.Get("mc.SetupActionIdList") as ArrayList;
                        if (list == null)
                        {
                            list = new ArrayList();
                            GetActionIdListByParentActionId(WebApplication.CommonDataSet.Action.FindByActionId(SetupPageActionId), list);
                            list.Add(SetupGlobalNavigationLinkActionId);

                            CacheManager.Current.Add("mc.SetupActionIdList", list);
                        }
                    }
                }
                return list;
            }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets the collection of the global navigation links's actions.
        /// </summary>
        internal static ActionCollection GlobalNavigationLinks
        {
            get
            {
                ActionCollection coll = CacheManager.Current.Get("mc.GlobalNavigationLinks") as ActionCollection;
                if (coll == null)
                {
                    lock (s_GlobalNavigationLinksSyncRoot)
                    {
                        coll = CacheManager.Current.Get("mc.GlobalNavigationLinks") as ActionCollection;
                        if (coll == null)
                        {
                            coll = new ActionCollection();
                            CommonDataSet.ActionDataTable table = WebApplication.CommonDataSet.Action;

                            foreach (CommonDataSet.ActionRow row in table.Select(string.Format(CultureInfo.InvariantCulture
                                , "({0} = 1) AND ({1} IS NOT NULL) AND ({2} = {3})"
                                , table.VisibleColumn.ColumnName, table.ParentActionIdColumn.ColumnName, table.ActionTypeIdColumn.ColumnName
                                , (int)ActionType.GlobalNavigationLink)))
                            {
                                if (row.ActionId == LoginGlobalNavigationLinkActionId)
                                    row.NavigateUrl = WebApplication.LoginProvider.GetLoginUrl();
                                else if (row.ActionId == MyAccountGlobalNavigationLinkActionId)
                                {
                                    if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme != MasterPageTheme.Modern)
                                        row.OrderNumber = -row.OrderNumber;
                                }
                                coll.Add(CreateAction(row));
                            }

                            coll.Sort();

                            CacheManager.Current.Add("mc.GlobalNavigationLinks", coll);
                        }
                    }
                }
                return coll;
            }
        }

        /// <summary>
        /// Gets the collection of the actions identifiers of the profile management pages.
        /// </summary>
        internal static ArrayList MyAccountActionIdList
        {
            get
            {
                ArrayList list = CacheManager.Current.Get("mc.MyAccountActionIdList") as ArrayList;
                if (list == null)
                {
                    lock (s_MyAccountActionIdListSyncRoot)
                    {
                        list = CacheManager.Current.Get("mc.MyAccountActionIdList") as ArrayList;
                        if (list == null)
                        {
                            list = new ArrayList();
                            GetActionIdListByParentActionId(WebApplication.CommonDataSet.Action.FindByActionId(MyAccountPageActionId), list);
                            list.Add(MyAccountGlobalNavigationLinkActionId);

                            CacheManager.Current.Add("mc.MyAccountActionIdList", list);
                        }
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// Gets a value indicating that the action table contains only built-in actions.
        /// </summary>
        internal static bool OnlyBuiltInActionsAvailable
        {
            get
            {
                CommonDataSet.ActionDataTable table = WebApplication.CommonDataSet.Action;
                return (table.Select(string.Concat(table.BuiltInColumn.ColumnName, " = 1")).Length == 0);
            }
        }

        /// <summary>
        /// Gets the collection of the actions identifiers which available for the administrator of the organizations and instances.
        /// </summary>
        internal static ArrayList SettingsActionIdList
        {
            get
            {
                ArrayList list = CacheManager.Current.Get("mc.SettingsActionIdList") as ArrayList;
                if (list == null)
                {
                    lock (s_SettingsActionIdListSyncRoot)
                    {
                        list = CacheManager.Current.Get("mc.SettingsActionIdList") as ArrayList;
                        if (list == null)
                        {
                            list = new ArrayList();
                            GetActionIdListByParentActionId(WebApplication.CommonDataSet.Action.FindByActionId(ConfigurationPageActionId), list);
                            list.Add(ConfigurationGlobalNavigationLinkActionId);

                            CacheManager.Current.Add("mc.SettingsActionIdList", list);
                        }
                    }
                }
                return list;
            }
        }

        internal static SettingLevels StartPageSettingsLevels
        {
            get
            {
                Micajah.Common.Bll.Action action = FindAction(StartPageActionId);
                if (action != null)
                    return GetActionSettingLevels(action.AuthenticationRequired, action.InstanceRequired);
                return SettingLevels.Organization;
            }
        }

        /// <summary>
        /// Gets the actions's collection of the pages and controls.
        /// </summary>
        internal static ActionCollection PagesAndControls
        {
            get
            {
                ActionCollection coll = CacheManager.Current.Get("mc.PagesAndControls") as ActionCollection;
                if (coll == null)
                {
                    lock (s_PagesAndControlsSyncRoot)
                    {
                        coll = CacheManager.Current.Get("mc.PagesAndControls") as ActionCollection;
                        if (coll == null)
                        {
                            coll = new ActionCollection();
                            coll.LoadFromCommonDataSet();

                            CacheManager.Current.Add("mc.PagesAndControls", coll);
                        }
                    }
                }
                return coll;
            }
        }

        /// <summary>
        /// Gets the collection of the actions for which the authentication is not required.
        /// </summary>
        internal static ActionCollection PublicActions
        {
            get
            {
                ActionCollection coll = CacheManager.Current.Get("mc.PublicActions") as ActionCollection;
                if (coll == null)
                {
                    lock (s_PublicActionsSyncRoot)
                    {
                        coll = CacheManager.Current.Get("mc.PublicActions") as ActionCollection;
                        if (coll == null)
                        {
                            coll = new ActionCollection();
                            coll.AddRange(PagesAndControls.FindAllPublic());
                            coll.AddRange(GlobalNavigationLinks.FindAllPublic());

                            Action action = new Action();
                            action.ActionId = Guid.NewGuid();
                            action.ActionType = ActionType.Page;
                            action.NavigateUrl = ResourceProvider.ResourceHandlerVirtualPath;
                            coll.Add(action);

                            CacheManager.Current.Add("mc.PublicActions", coll);
                        }
                    }
                }
                return coll;
            }
        }

        #endregion

        #region Private Methods

        private static void Fill(CommonDataSet dataSet, ActionElementCollection actions, Guid? parentActionId)
        {
            foreach (ActionElement action in actions)
            {
                CommonDataSet.ActionRow actionRow = dataSet.Action.FindByActionId(action.Id);
                if (actionRow != null)
                {
                    if (!actionRow.BuiltIn) actionRow = null;
                }

                if (actionRow == null)
                {
                    actionRow = dataSet.Action.NewActionRow();
                    LoadActionAttributes(actionRow, action, parentActionId);
                    dataSet.Action.AddActionRow(actionRow);
                }

                FillAlternativeParents(dataSet.ActionsParentActions, action);
                RoleProvider.FillRolesActions(dataSet, action);

                int visibleChildSettingsCount = 0;
                foreach (SettingElement s in action.Settings)
                {
                    if (s.Visible) visibleChildSettingsCount++;
                }

                if (visibleChildSettingsCount > 0)
                {
                    if (string.IsNullOrEmpty(action.NavigateUrl))
                        actionRow.NavigateUrl = ResourceProvider.SettingsPageVirtualPath + "?actionid=" + actionRow.ActionId.ToString("N");

                    SettingProvider.Fill(dataSet.Setting, dataSet.SettingListsValues, action.Settings, null, actionRow.ActionId, GetActionSettingLevels(actionRow.AuthenticationRequired, actionRow.InstanceRequired));

                    dataSet.Setting.AcceptChanges();
                    dataSet.SettingListsValues.AcceptChanges();
                }

                Fill(dataSet, action.Actions, actionRow.ActionId);
            }
        }

        private static void FillAlternativeParents(CommonDataSet.ActionsParentActionsDataTable table, ActionElement action)
        {
            if (action.AlternativeParents == null) return;

            foreach (string value in action.AlternativeParents)
            {
                object obj = Support.ConvertStringToType(value, typeof(Guid));
                if (obj != null)
                {
                    CommonDataSet.ActionsParentActionsRow actionsParentActionsRow = table.NewActionsParentActionsRow();
                    actionsParentActionsRow.ActionId = action.Id;
                    actionsParentActionsRow.ParentActionId = (Guid)obj;
                    table.AddActionsParentActionsRow(actionsParentActionsRow);
                }
            }
        }

        private static Action FindAction(Guid actionId, string absoluteNavigateUrl, ActionCollection actions)
        {
            Action originalItem = null;

            if (actionId != Guid.Empty)
                originalItem = actions.FindByActionId(actionId);
            else if (!string.IsNullOrEmpty(absoluteNavigateUrl))
            {
                if (absoluteNavigateUrl.Contains("?"))
                {
                    originalItem = actions.FindByNavigateUrlPathAndQuery(absoluteNavigateUrl, true, true);
                    if (originalItem == null)
                        originalItem = actions.FindByNavigateUrlPathAndQuery(absoluteNavigateUrl, true, false);
                }
                if (originalItem == null)
                    originalItem = actions.FindByNavigateUrl(absoluteNavigateUrl.Split('?')[0], true);
            }

            return originalItem;
        }

        private static void LoadActionAttributes(CommonDataSet.ActionRow row, ActionElement action, Guid? parentActionId)
        {
            row.BuiltIn = action.BuiltIn;
            if (action.ActionType != ActionType.NotSet)
                row.ActionTypeId = (int)action.ActionType;
            row.ParentActionId = (parentActionId.HasValue
                ? parentActionId.Value
                : ((action.ActionType == ActionType.Page) ? PagesAndControlsActionId : GlobalNavigationLinksActionId));
            row.Handle = action.Handle;

            switch ((ActionType)row.ActionTypeId)
            {
                case ActionType.Page:
                    LoadPageAttributes(row, action);
                    break;
                case ActionType.Control:
                    LoadControlAttributes(row, action);
                    break;
                case ActionType.GlobalNavigationLink:
                    LoadGlobalNavigationLinkAttributes(row, action);
                    break;
            }
        }

        private static void LoadControlAttributes(CommonDataSet.ActionRow row, ActionElement action)
        {
            row.ActionId = action.Id;
            row.Name = action.Name;
            row.Description = action.Description;
        }

        private static void LoadGlobalNavigationLinkAttributes(CommonDataSet.ActionRow row, ActionElement action)
        {
            LoadControlAttributes(row, action);

            row.NavigateUrl = action.NavigateUrl;
            row.OrderNumber = action.OrderNumber;
            row.AuthenticationRequired = action.AuthenticationRequired;
            row.InstanceRequired = action.InstanceRequired;
            row.Visible = action.Visible;
        }

        private static void LoadPageAttributes(CommonDataSet.ActionRow row, ActionElement action)
        {
            row.LearnMoreUrl = action.LearnMoreUrl;
            row.VideoUrl = action.VideoUrl;

            LoadGlobalNavigationLinkAttributes(row, action);
            LoadDetailMenuAttributes(row, action.DetailMenu);
            LoadSubmenuAttributes(row, action.Submenu);
        }

        private static void LoadDetailMenuAttributes(CommonDataSet.ActionRow row, ActionDetailMenuElement detailMenu)
        {
            row.ShowInDetailMenu = detailMenu.Show;
            row.ShowChildrenInDetailMenu = detailMenu.ShowChildren;
            row.ShowDescriptionInDetailMenu = detailMenu.ShowDescription;
            row.GroupInDetailMenu = detailMenu.Group;
            row.HighlightInDetailMenu = detailMenu.Highlight;
            row.IconUrl = detailMenu.IconUrl;
            if (detailMenu.Theme.HasValue)
                row.DetailMenuTheme = (int)detailMenu.Theme.Value;
            if (detailMenu.IconSize.HasValue)
                row.DetailMenuIconSize = (int)detailMenu.IconSize.Value;
        }

        private static void LoadSubmenuAttributes(CommonDataSet.ActionRow row, ActionSubmenuElement submenu)
        {
            row.SubmenuItemImageUrl = submenu.ImageUrl;
            row.SubmenuItemTypeId = (int)submenu.ItemType;
            row.SubmenuItemWidth = submenu.Width;
            row.SubmenuItemHorizontalAlignId = (int)submenu.HorizontalAlign;
        }

        private static void FillActionIdList(CommonDataSet.ActionRow row, ref ArrayList list)
        {
            if ((row == null) || (list == null)) return;
            if ((row.ActionTypeId == (int)ActionType.Page) && (!list.Contains(row.ActionId))
                    && ((!row.BuiltIn) || (row.ActionId == PagesAndControlsActionId)))
            {
                list.Add(row.ActionId);
            }
            foreach (CommonDataSet.ActionRow actionRow in row.GetActionRows())
            {
                FillActionIdList(actionRow, ref list);
            }
        }

        private static void GetActionIdListByParentActionId(CommonDataSet.ActionRow row, ArrayList list)
        {
            if (row == null) return;

            list.Add(row.ActionId);
            foreach (CommonDataSet.ActionRow dr in row.GetActionRows())
            {
                GetActionIdListByParentActionId(dr, list);
            }
        }

        #endregion

        #region Internal Methods

        internal static bool AccessDeniedToSettingsDiagnosticPage()
        {
            bool groupsExist = false;
            UserContext user = UserContext.Current;
            if (user != null)
            {
                Organization org = user.SelectedOrganization;
                if (org != null)
                    groupsExist = (org.DataSet.Group.Count > 0);
            }
            return (!(SettingProvider.GroupSettingsExist && groupsExist));
        }

        internal static bool AuthenticationRequired(Guid actionId)
        {
            return (ActionProvider.SettingsActionIdList.Contains(actionId) || ActionProvider.MyAccountActionIdList.Contains(actionId));
        }

        internal static Action CreateAction(CommonDataSet.ActionRow row)
        {
            if (row != null)
            {
                Action action = new Action();
                action.ActionId = row.ActionId;
                action.ParentActionId = (row.IsParentActionIdNull() ? null : new Guid?(row.ParentActionId));
                action.ActionType = (ActionType)row.ActionTypeId;
                action.Name = row.Name;
                action.Description = row.Description;
                action.IconUrl = row.IconUrl;
                action.SubmenuItemImageUrl = row.SubmenuItemImageUrl;
                action.SubmenuItemType = (SubmenuItemType)row.SubmenuItemTypeId;
                action.SubmenuItemHorizontalAlign = (HorizontalAlign)row.SubmenuItemHorizontalAlignId;
                if (row.SubmenuItemWidth > 0)
                    action.SubmenuItemWidth = Unit.Pixel(row.SubmenuItemWidth);
                action.NavigateUrl = (row.IsNavigateUrlNull() ? null : row.NavigateUrl);
                action.LearnMoreUrl = row.LearnMoreUrl;
                action.VideoUrl = row.VideoUrl;
                action.OrderNumber = row.OrderNumber;
                action.AuthenticationRequired = row.AuthenticationRequired;
                action.InstanceRequired = row.InstanceRequired;
                action.Visible = row.Visible;
                action.ShowInDetailMenu = row.ShowInDetailMenu;
                action.ShowChildrenInDetailMenu = row.ShowChildrenInDetailMenu;
                action.ShowDescriptionInDetailMenu = row.ShowDescriptionInDetailMenu;
                action.GroupInDetailMenu = row.GroupInDetailMenu;
                action.HighlightInDetailMenu = row.HighlightInDetailMenu;
                if (!row.IsDetailMenuThemeNull())
                    action.DetailMenuTheme = (DetailMenuTheme)row.DetailMenuTheme;
                if (!row.IsDetailMenuIconSizeNull())
                    action.IconSize = (IconSize)row.DetailMenuIconSize;
                action.BuiltIn = row.BuiltIn;
                action.Handle = row.Handle;
                return action;
            }
            return null;
        }

        internal static Action CreateAction(CommonDataSet.ActionRow row, Action parentAction)
        {
            Action action = CreateAction(row);
            if (action != null)
                action.ParentAction = parentAction;
            return action;
        }

        internal static void Fill(CommonDataSet dataSet)
        {
            if (dataSet == null) return;

            CommonDataSet.ActionRow row = dataSet.Action.NewActionRow();
            row.ActionTypeId = (int)ActionType.GlobalNavigationLink;
            row.Name = "Global Navigation Links";
            row.ActionId = GlobalNavigationLinksActionId;
            row.BuiltIn = true;
            dataSet.Action.AddActionRow(row);

            row = dataSet.Action.NewActionRow();
            row.ActionTypeId = (int)ActionType.Page;
            row.Name = "Pages & Controls";
            row.ActionId = PagesAndControlsActionId;
            row.BuiltIn = true;
            dataSet.Action.AddActionRow(row);

            Fill(dataSet, FrameworkConfiguration.Current.Actions, null);

            dataSet.Action.AcceptChanges();
            dataSet.ActionsParentActions.AcceptChanges();
            dataSet.RolesActions.AcceptChanges();
        }

        /// <summary>
        /// Gets the root actions.
        /// </summary>
        /// <returns>The array of the DataRow that contains the root actions.</returns>
        internal static DataRow[] GetRootActionRows()
        {
            CommonDataSet.ActionDataTable table = WebApplication.CommonDataSet.Action;
            return table.Select(string.Concat(table.ParentActionIdColumn.ColumnName, " IS NULL"));
        }

        /// <summary>
        /// Gets the identifiers of actions associated with specified role.
        /// </summary>
        /// <param name="roleId">Specifies the role's identifier.</param>
        /// <returns>The System.Collections.ArrayList that contains identifiers of actions associated with specified role.</returns>
        internal static ArrayList GetActionIdListByRoleId(Guid roleId)
        {
            string key = string.Format(CultureInfo.InvariantCulture, "mc.RoleActionIdList.{0:N}", roleId);

            ArrayList actionIdList = CacheManager.Current.Get(key) as ArrayList;
            if (actionIdList == null)
            {
                lock (s_RoleActionIdListSyncRoot)
                {
                    actionIdList = CacheManager.Current.Get(key) as ArrayList;
                    if (actionIdList == null)
                    {
                        actionIdList = new ArrayList();

                        CommonDataSet.RolesActionsDataTable table = WebApplication.CommonDataSet.RolesActions;
                        foreach (CommonDataSet.RolesActionsRow row in table.Select(string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", table.RoleIdColumn.ColumnName, roleId.ToString())))
                        {
                            actionIdList.Add(row.ActionId);
                        }

                        CacheManager.Current.Add(key, actionIdList);
                    }
                }
            }

            return (ArrayList)actionIdList.Clone();
        }

        internal static void AddAdminActionIdList(bool isOrgAdmin, bool isInstAdmin, ref ArrayList actionIdList)
        {
            if (isInstAdmin)
                actionIdList.AddRange(GetActionIdListByRoleId(RoleProvider.InstanceAdministratorRoleId));

            if (isOrgAdmin)
                actionIdList.AddRange(GetActionIdListByRoleId(RoleProvider.OrganizationAdministratorRoleId));

            // Removes duplicate values.
            HashSet<Guid> set = new HashSet<Guid>((Guid[])actionIdList.ToArray(typeof(Guid)));
            Guid[] result = new Guid[set.Count];
            set.CopyTo(result);
            actionIdList = new ArrayList(result);
        }

        internal static ArrayList GetRoleActionIdList(ArrayList roleIdList, bool isOrgAdmin)
        {
            bool isInstAdmin = false;
            Guid roleId = RoleProvider.AssumeRole(isOrgAdmin, roleIdList, ref isInstAdmin);
            ArrayList actionIdList = new ArrayList();

            if (roleId != Guid.Empty)
                actionIdList.AddRange(GetActionIdListByRoleId(roleId));

            AddAdminActionIdList(isOrgAdmin, isInstAdmin, ref actionIdList);

            return actionIdList;
        }

        internal static ArrayList GetGroupActionIdList(ArrayList groupIdList, Guid instanceId, Guid organizationId, bool isOrgAdmin, bool isInstAdmin)
        {
            ArrayList actionIdList = new ArrayList();

            if (groupIdList.Count > 0)
            {
                OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
                OrganizationDataSet.GroupsInstancesRolesDataTable gdrTable = ds.GroupsInstancesRoles;
                OrganizationDataSet.GroupsInstancesActionsDataTable giaTable = ds.GroupsInstancesActions;

                foreach (Guid groupId in groupIdList)
                {
                    string key = string.Format(CultureInfo.InvariantCulture, "mc.GroupInstanceActionIdList.{0:N}.{1:N}", groupId, instanceId);

                    ArrayList list2 = CacheManager.Current.Get(key) as ArrayList;
                    if (list2 == null)
                    {
                        lock (s_GroupInstanceActionIdListSyncRoot)
                        {
                            list2 = CacheManager.Current.Get(key) as ArrayList;
                            if (list2 == null)
                            {
                                OrganizationDataSet.GroupsInstancesRolesRow gdrRow = gdrTable.FindByGroupIdInstanceId(groupId, instanceId);
                                if (gdrRow != null)
                                {
                                    list2 = GetActionIdListByRoleId(gdrRow.RoleId);

                                    foreach (OrganizationDataSet.GroupsInstancesActionsRow actionRow in giaTable.Select(string.Format(CultureInfo.InvariantCulture, "{0} = '{1}' AND {2} = '{3}'"
                                        , giaTable.GroupIdColumn.ColumnName, groupId.ToString(), giaTable.InstanceIdColumn.ColumnName, instanceId.ToString())))
                                    {
                                        if (list2.Contains(actionRow.ActionId))
                                        {
                                            if (!actionRow.Enabled)
                                                list2.Remove(actionRow.ActionId);
                                        }
                                        else if (actionRow.Enabled)
                                            list2.Add(actionRow.ActionId);
                                    }

                                    CacheManager.Current.Add(key, list2);
                                }
                            }
                        }
                    }

                    actionIdList.AddRange(list2);
                }

                AddAdminActionIdList(isOrgAdmin, isInstAdmin, ref actionIdList);

                actionIdList = (ArrayList)actionIdList.Clone();
            }

            return actionIdList;
        }

        internal static void Refresh(Guid groupId, Guid instanceId)
        {
            lock (s_GroupInstanceActionIdListSyncRoot)
            {
                string key = string.Format(CultureInfo.InvariantCulture, "mc.GroupInstanceActionIdList.{0:N}.{1:N}", groupId, instanceId);
                CacheManager.Current.Remove(key);
            }
        }

        /// <summary>
        /// Returns the actions table.
        /// </summary>
        /// <returns>The actions table.</returns>
        internal static DataTable GetActionsTree()
        {
            CommonDataSet.ActionDataTable table = WebApplication.CommonDataSet.Action.Copy() as CommonDataSet.ActionDataTable;

            foreach (CommonDataSet.ActionRow row in table.Select(string.Concat(table.ParentActionIdColumn.ColumnName, " = '", ConfigurationPageActionId.ToString(), "'")))
            {
                row.ParentActionId = ConfigurationGlobalNavigationLinkActionId;
            }

            foreach (CommonDataSet.ActionRow row in table.Select(string.Concat(table.ParentActionIdColumn.ColumnName, " = '", MyAccountPageActionId.ToString(), "'")))
            {
                row.ParentActionId = MyAccountGlobalNavigationLinkActionId;
            }

            CommonDataSet.ActionRow actionRow = table.FindByActionId(ConfigurationPageActionId);
            if (actionRow != null) actionRow.Delete();

            actionRow = table.FindByActionId(MyAccountPageActionId);
            if (actionRow != null) actionRow.Delete();

            actionRow = table.FindByActionId(LoginGlobalNavigationLinkActionId);
            if (actionRow != null) actionRow.Delete();

            actionRow = table.FindByActionId(LoginAsUserGlobalNavigationLinkActionId);
            if (actionRow != null) actionRow.Delete();

            actionRow = table.FindByActionId(LoginAsUserPageActionId);
            if (actionRow != null) actionRow.Delete();

            foreach (Guid actionId in SetupActionIdList)
            {
                actionRow = table.FindByActionId(actionId);
                if (actionRow != null) actionRow.Delete();
            }

            table.AcceptChanges();

            return table;
        }

        /// <summary>
        /// Gets the collection of the alternative parent actions for the specified action.
        /// </summary>
        /// <param name="actionId">The identifier of the action.</param>
        /// <returns>The collection of the alternative parent actions.</returns>
        internal static ActionCollection GetAlternativeParentActions(Guid actionId)
        {
            ActionCollection coll = new ActionCollection();
            CommonDataSet.ActionsParentActionsDataTable table = WebApplication.CommonDataSet.ActionsParentActions;
            foreach (CommonDataSet.ActionsParentActionsRow row in table.Select(string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", table.ActionIdColumn.ColumnName, actionId.ToString())))
            {
                Action action = ActionProvider.PagesAndControls.FindByActionId(row.ParentActionId);
                if (action != null) coll.Add(action);
            }
            return coll;
        }

        internal static ArrayList GetAlternativeParentActionsIdList(Guid actionId)
        {
            ArrayList list = new ArrayList();
            CommonDataSet.ActionsParentActionsDataTable table = WebApplication.CommonDataSet.ActionsParentActions;
            foreach (CommonDataSet.ActionsParentActionsRow row in table.Select(string.Concat(table.ActionIdColumn.ColumnName, " = '", actionId.ToString(), "'")))
            {
                if (!list.Contains(row.ParentActionId)) list.Add(row.ParentActionId);
            }
            return list;
        }

        internal static DataTable GetAlternativeParentActionsTree(Guid actionId)
        {
            CommonDataSet.ActionDataTable table = GetActionsTree() as CommonDataSet.ActionDataTable;

            ArrayList list = new ArrayList();
            FillActionIdList(WebApplication.CommonDataSet.Action.FindByActionId(actionId), ref list);

            foreach (CommonDataSet.ActionRow row in table.Rows)
            {
                if (!((row.ActionId == GlobalNavigationLinksActionId)
                    || (row.ActionId == ConfigurationGlobalNavigationLinkActionId)
                    || (row.ActionId == PagesAndControlsActionId)
                    || (row.ActionId == MyAccountGlobalNavigationLinkActionId)
                    || (row.ActionId == UsersPageActionId)))
                {
                    if (list.Contains(row.ActionId)
                        || row.BuiltIn
                        || (((ActionType)row.ActionTypeId != ActionType.Page) && ((ActionType)row.ActionTypeId != ActionType.GlobalNavigationLink)))
                    {
                        row.Delete();
                    }
                }
            }

            table.AcceptChanges();

            return table;
        }

        internal static SettingLevels GetActionSettingLevels(bool authenticationRequired, bool instanceRequired)
        {
            SettingLevels levels = SettingLevels.None;
            if (authenticationRequired)
                levels = SettingLevels.Organization;
            if (instanceRequired)
                levels = SettingLevels.Instance;
            return levels;
        }

        /// <summary>
        /// Gets a value indicating that the specified action is setting page.
        /// </summary>
        /// <param name="actionId">The identifier of the action to check.</param>
        /// <returns>true, if the specified action is configuration page; otherwise, false.</returns>
        internal static bool IsSettingPage(Guid actionId)
        {
            if (actionId == Guid.Empty)
                return false;
            return SettingsActionIdList.Contains(actionId);
        }

        internal static bool IsMyAccountBuiltInAction(Guid actionId)
        {
            if ((actionId == MyAccountPageActionId)
                || (actionId == MyNameAndEmailPageActionId)
                || (actionId == MyPasswordPageActionId))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating that the authentication is not required for the specified URL.
        /// </summary>
        /// <param name="navigateUrl">The string that contains the URL to check.</param>
        /// <returns>true, if the authentication is not required for the specified URL; otherwise, false.</returns>
        internal static bool IsPublicPage(string navigateUrl)
        {
            Action action = FindAction(Guid.Empty, WebApplication.CreateApplicationAbsoluteUrl(navigateUrl), PublicActions);
            if (action != null)
                return ((!SetupActionIdList.Contains(action.ActionId)) || (string.Compare(navigateUrl, ResourceProvider.FrameworkPageVirtualPath, StringComparison.OrdinalIgnoreCase) == 0));
            return false;
        }

        /// <summary>
        /// Gets a value indicating that the specified action is setup page.
        /// </summary>
        /// <param name="actionId">The identifier of the action to check.</param>
        /// <returns>true, if the specified action is setup page; otherwise, false.</returns>
        internal static bool IsSetupPage(Guid actionId)
        {
            Action action = FindAction(actionId);
            if (action != null)
                return SetupActionIdList.Contains(action.ActionId);
            return false;
        }

        /// <summary>
        /// Gets a value indicating that the specified URL is setup page's URL.
        /// </summary>
        /// <param name="navigateUrl">The string that contains the URL to check.</param>
        /// <returns>true, if the specified URL is setup page's URL; otherwise, false.</returns>
        internal static bool IsSetupPage(string navigateUrl)
        {
            if (!string.IsNullOrEmpty(navigateUrl))
                navigateUrl = navigateUrl.Split('?')[0];
            bool result = (string.Compare(WebApplication.CreateApplicationAbsoluteUrl(navigateUrl), WebApplication.CreateApplicationAbsoluteUrl(ResourceProvider.FrameworkPageVirtualPath), StringComparison.OrdinalIgnoreCase) == 0);
            if (!result)
            {
                Action action = FindAction(navigateUrl);
                if (action != null) result = SetupActionIdList.Contains(action.ActionId);
            }
            return result;
        }

        internal static void Refresh()
        {
            lock (s_SetupActionIdListSyncRoot)
            {
                CacheManager.Current.Remove("mc.SetupActionIdList");
            }

            lock (s_GlobalNavigationLinksSyncRoot)
            {
                CacheManager.Current.Remove("mc.GlobalNavigationLinks");
            }

            lock (s_MyAccountActionIdListSyncRoot)
            {
                CacheManager.Current.Remove("mc.MyAccountActionIdList");
            }

            lock (s_SettingsActionIdListSyncRoot)
            {
                CacheManager.Current.Remove("mc.SettingsActionIdList");
            }

            lock (s_PagesAndControlsSyncRoot)
            {
                CacheManager.Current.Remove("mc.PagesAndControls");
            }

            lock (s_PublicActionsSyncRoot)
            {
                CacheManager.Current.Remove("mc.PublicActions");
            }
        }

        internal static bool ShowAction(Action action, bool isFrameworkAdmin, bool isAuthenticated, IList actionIdList)
        {
            if (action.AuthenticationRequired)
            {
                if (isAuthenticated)
                {
                    if (SetupActionIdList.Contains(action.ActionId))
                    {
                        if (!isFrameworkAdmin)
                            return false;
                    }
                    else if (!(((actionIdList != null) && actionIdList.Contains(action.ActionId)) || IsMyAccountBuiltInAction(action.ActionId)))
                        return false;
                }
                else return false;
            }
            return true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the actions, excluding marked as deleted.
        /// </summary>
        /// <returns>The System.Data.DataTable object that contains actions.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetActions()
        {
            return WebApplication.CommonDataSet.Action;
        }

        /// <summary>
        /// Gets an object populated with information of the specified action.
        /// </summary>
        /// <param name="actionId">Specifies the action identifier to get information.</param>
        /// <returns>The object populated with information of the specified action. If the action is not found, the method returns null reference.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static Action GetAction(Guid actionId)
        {
            return CreateAction(WebApplication.CommonDataSet.Action.FindByActionId(actionId));
        }

        /// <summary>
        /// Searches for an action that matches the specified navigate URL in all actions collections.
        /// </summary>
        /// <param name="absoluteNavigateUrl">The aplication absolute navigate URL of the action to search for.</param>
        /// <returns>The first action that matches the specified navigate URL, if found; otherwise, the null reference.</returns>
        public static Action FindAction(string absoluteNavigateUrl)
        {
            return FindAction(Guid.Empty, absoluteNavigateUrl);
        }

        /// <summary>
        /// Searches for an action that matches the specified identifier in all actions collections.
        /// </summary>
        /// <param name="actionId">The identifier of the action to search for.</param>
        /// <returns>The first action that matches the specified identifier, if found; otherwise, the null reference.</returns>
        public static Action FindAction(Guid actionId)
        {
            return FindAction(actionId, null);
        }

        /// <summary>
        /// Searches for an action that matches the specified identifier or navigate URL in all actions collections.
        /// </summary>
        /// <param name="actionId">The identifier of the action to search for.</param>
        /// <param name="absoluteNavigateUrl">The aplication absolute navigate URL of the action to search for.</param>
        /// <returns>The first action that matches the specified identifier or navigate URL, if found; otherwise, the null reference.</returns>
        public static Action FindAction(Guid actionId, string absoluteNavigateUrl)
        {
            Action originalItem = FindAction(actionId, absoluteNavigateUrl, PagesAndControls);

            if (originalItem == null)
                originalItem = FindAction(actionId, absoluteNavigateUrl, GlobalNavigationLinks);

            return originalItem;
        }

        #endregion
    }
}
