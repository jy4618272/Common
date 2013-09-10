using Micajah.Common.Application;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.WebControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;

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
        internal readonly static Guid CustomStyleSheetPageActionId = new Guid("00000000-0000-0000-0000-000000000020");
        internal readonly static Guid MyAccountPageActionId = new Guid("00000000-0000-0000-0000-000000000021");
        internal readonly static Guid UserNameAndEmailPageActionId = new Guid("00000000-0000-0000-0000-000000000024");
        internal readonly static Guid UserPasswordPageActionId = new Guid("00000000-0000-0000-0000-000000000025");
        internal readonly static Guid UserGroupsPageActionId = new Guid("00000000-0000-0000-0000-000000000026");
        internal readonly static Guid UserActivateInactivatePageActionId = new Guid("97BE82B7-48B7-4A9C-BD14-6A2494EF2AA7");
        internal readonly static Guid UserAssociateToOrganizationStructurePageActionId = new Guid("DAB9B65E-0358-408E-A2F1-2D616FCA33EC");
        internal readonly static Guid SetupPageActionId = new Guid("00000000-0000-0000-0000-000000000027");
        internal readonly static Guid LoginAsUserGlobalNavigationLinkActionId = new Guid("709A6B39-36A2-43FC-AF18-24C2E3332D7A");
        internal readonly static Guid OrganizationsPageActionId = new Guid("00000000-0000-0000-0000-000000000040");
        internal readonly static Guid TreesPageActionId = new Guid("B3CCC73F-7194-4F0A-AABB-77AE91E31CE9");
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
        internal readonly static Guid GoogleIntegrationPageActionId = new Guid("D79FFA5D-54C2-4EB0-AB47-42DDA41C1380");
        internal readonly static Guid ActivityReportActionId = new Guid("7062A21C-BA40-4A8F-9F33-DA68751E4F0D");
        internal readonly static Guid MyAccountMenuGlobalNavigationLinkActionId = new Guid("CFDBB5D3-0A4E-41BB-B7BB-4C47781806DE");
        internal readonly static Guid OAuthPageActionId = new Guid("5900F3EF-F423-4AC8-BAA5-828B746E3F43");

        // The objects which are used to synchronize access to the cached collections and lists.
        private static readonly object s_PagesAndControlsSyncRoot = new object();
        private static readonly object s_GroupsInstanceActionIdListSyncRoot = new object();
        private static readonly object s_GroupInstanceActionIdListSyncRoot = new object();

        private static ActionCollection s_GlobalNavigationLinks;
        private static ActionCollection s_PagesAndControls;

        #endregion

        #region Private Properties

        private static List<string> GroupsInstanceActionIdListKeys
        {
            get
            {
                List<string> list = CacheManager.Current.Get("mc.GroupsInstanceActionIdListKeys") as List<string>;
                if (list == null)
                {
                    list = new List<string>();
                    CacheManager.Current.AddWithDefaultExpiration("mc.GroupsInstanceActionIdListKeys", list);
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
                if (s_GlobalNavigationLinks == null)
                {
                    lock (s_PagesAndControlsSyncRoot)
                    {
                        if (s_GlobalNavigationLinks == null)
                            LoadFromConfigurationDataSet();
                    }
                }
                return s_GlobalNavigationLinks;
            }
        }

        /// <summary>
        /// Gets a value indicating that the action table contains only built-in actions.
        /// </summary>
        internal static bool OnlyBuiltInActionsAvailable
        {
            get
            {
                ConfigurationDataSet.ActionDataTable table = ConfigurationDataSet.Current.Action;
                return (table.Select(string.Concat(table.BuiltInColumn.ColumnName, " = 0")).Length == 0);
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
                if (s_PagesAndControls == null)
                {
                    lock (s_PagesAndControlsSyncRoot)
                    {
                        if (s_PagesAndControls == null)
                            LoadFromConfigurationDataSet();
                    }
                }
                return s_PagesAndControls;
            }
        }

        #endregion

        #region Private Methods

        private static void Fill(ConfigurationDataSet dataSet, ActionElementCollection actions, Guid? parentActionId)
        {
            foreach (ActionElement action in actions)
            {
                ConfigurationDataSet.ActionRow actionRow = dataSet.Action.FindByActionId(action.Id);
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

        private static void FillAlternativeParents(ConfigurationDataSet.ActionsParentActionsDataTable table, ActionElement action)
        {
            if (action.AlternativeParents == null) return;

            foreach (string value in action.AlternativeParents)
            {
                object obj = Support.ConvertStringToType(value, typeof(Guid));
                if (obj != null)
                {
                    ConfigurationDataSet.ActionsParentActionsRow actionsParentActionsRow = table.NewActionsParentActionsRow();
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

        private static void LoadActionAttributes(ConfigurationDataSet.ActionRow row, ActionElement action, Guid? parentActionId)
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

        private static void LoadControlAttributes(ConfigurationDataSet.ActionRow row, ActionElement action)
        {
            row.ActionId = action.Id;
            row.Name = action.Name;
            row.Description = action.Description;
        }

        private static void LoadGlobalNavigationLinkAttributes(ConfigurationDataSet.ActionRow row, ActionElement action)
        {
            row.NavigateUrl = action.NavigateUrl;
            row.OrderNumber = action.OrderNumber;
            row.AuthenticationRequired = action.AuthenticationRequired;
            row.OrganizationRequired = action.OrganizationRequired;
            row.InstanceRequired = action.InstanceRequired;
            row.Visible = action.Visible;

            LoadControlAttributes(row, action);
            LoadDetailMenuAttributes(row, action.DetailMenu);
        }

        private static void LoadPageAttributes(ConfigurationDataSet.ActionRow row, ActionElement action)
        {
            row.LearnMoreUrl = action.LearnMoreUrl;
            row.VideoUrl = action.VideoUrl;

            LoadGlobalNavigationLinkAttributes(row, action);
            LoadSubmenuAttributes(row, action.Submenu);
        }

        private static void LoadDetailMenuAttributes(ConfigurationDataSet.ActionRow row, ActionDetailMenuElement detailMenu)
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

        private static void LoadSubmenuAttributes(ConfigurationDataSet.ActionRow row, ActionSubmenuElement submenu)
        {
            row.SubmenuItemImageUrl = submenu.ImageUrl;
            row.SubmenuItemTypeId = (int)submenu.ItemType;
            row.SubmenuItemWidth = submenu.Width;
            row.SubmenuItemHorizontalAlignId = (int)submenu.HorizontalAlign;
            row.HighlightInSubmenu = submenu.Highlight;
        }

        private static void FillActionIdList(ConfigurationDataSet.ActionRow row, ref ArrayList list)
        {
            if ((row == null) || (list == null)) return;
            if ((row.ActionTypeId == (int)ActionType.Page) && (!list.Contains(row.ActionId))
                    && ((!row.BuiltIn) || (row.ActionId == PagesAndControlsActionId)))
            {
                list.Add(row.ActionId);
            }
            foreach (ConfigurationDataSet.ActionRow actionRow in row.GetActionRows())
            {
                FillActionIdList(actionRow, ref list);
            }
        }

        private static string GetActionIdListKey(ArrayList groupIdList, Guid instanceId)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Guid groupId in groupIdList)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, ",{0:N}", groupId);
            }
            sb.Remove(0, 1);
            sb.AppendFormat(CultureInfo.InvariantCulture, "].{0:N}", instanceId);
            sb.Insert(0, "mc.GroupsInstanceActionIdList.[");

            return sb.ToString();
        }

        private static SortedList GetGroupInstanceActionIdList(Guid groupId, Guid instanceId, OrganizationDataSet ds)
        {
            string key = string.Format(CultureInfo.InvariantCulture, "mc.GroupInstanceActionIdList.{0:N}.{1:N}", groupId, instanceId);
            OrganizationDataSet.GroupsInstancesActionsDataTable giaTable = ds.GroupsInstancesActions;

            SortedList list = CacheManager.Current.Get(key) as SortedList;
            if (list == null)
            {
                lock (s_GroupInstanceActionIdListSyncRoot)
                {
                    list = CacheManager.Current.Get(key) as SortedList;
                    if (list == null)
                    {
                        list = new SortedList();

                        // Overrides the access to the actions by the values for the group in the instance.
                        foreach (OrganizationDataSet.GroupsInstancesActionsRow actionRow in giaTable.Select(string.Format(CultureInfo.InvariantCulture, "{0} = '{1}' AND {2} = '{3}'"
                            , giaTable.GroupIdColumn.ColumnName, groupId.ToString(), giaTable.InstanceIdColumn.ColumnName, instanceId.ToString())))
                        {
                            if (list.Contains(actionRow.ActionId))
                                list[actionRow.ActionId] = actionRow.Enabled;
                            else
                                list.Add(actionRow.ActionId, actionRow.Enabled);
                        }

                        CacheManager.Current.AddWithDefaultExpiration(key, list);
                    }
                }
            }

            if (list != null)
                return (SortedList)list.Clone();

            return list;
        }

        private static SortedList GetGroupsInstanceActionIdList(ArrayList groupIdList, Guid instanceId, OrganizationDataSet ds)
        {
            string key = GetActionIdListKey(groupIdList, instanceId);

            SortedList list = CacheManager.Current.Get(key) as SortedList;
            if (list == null)
            {
                lock (s_GroupsInstanceActionIdListSyncRoot)
                {
                    list = CacheManager.Current.Get(key) as SortedList;
                    if (list == null)
                    {
                        list = new SortedList();

                        foreach (Guid groupId in groupIdList)
                        {
                            SortedList list2 = GetGroupInstanceActionIdList(groupId, instanceId, ds);
                            if (list2 != null)
                            {
                                foreach (Guid actionId in list2.Keys)
                                {
                                    if (list.Contains(actionId))
                                    {
                                        if ((bool)list[actionId])
                                            list[actionId] = (bool)list2[actionId];
                                    }
                                    else
                                        list.Add(actionId, (bool)list2[actionId]);
                                }
                            }
                        }

                        CacheManager.Current.AddWithDefaultExpiration(key, list);

                        GroupsInstanceActionIdListKeys.Add(key);
                    }
                }
            }

            if (list != null)
                return (SortedList)list.Clone();

            return list;
        }

        /// <summary>
        /// Loads the collection from configuration data set.
        /// </summary>
        private static void LoadFromConfigurationDataSet()
        {
            ActionCollection globalNavigationLinks = new ActionCollection();
            ActionCollection pagesAndControls = new ActionCollection();

            ConfigurationDataSet.ActionDataTable table = ConfigurationDataSet.Current.Action;
            ConfigurationDataSet.ActionRow[] rows = (ConfigurationDataSet.ActionRow[])table.Select(string.Concat(table.ParentActionIdColumn.ColumnName, " IS NULL"));

            ReadFromConfigurationDataSet(null, rows, new List<ActionType>() { ActionType.Page, ActionType.Control }, pagesAndControls);
            ReadFromConfigurationDataSet(null, rows, new List<ActionType>() { ActionType.GlobalNavigationLink }, globalNavigationLinks);

            s_PagesAndControls = pagesAndControls;
            s_GlobalNavigationLinks = globalNavigationLinks;
        }

        private static void ReadFromConfigurationDataSet(Action parent, DataRow[] actionRows, List<ActionType> allowedTypes, ActionCollection actions)
        {
            foreach (ConfigurationDataSet.ActionRow row in actionRows)
            {
                ActionType type = (ActionType)row.ActionTypeId;
                if (allowedTypes.Contains(type))
                {
                    Action item = CreateAction(row, parent);

                    DataRow[] childActionRows = row.GetActionRows();
                    if (childActionRows.Length > 0) ReadFromConfigurationDataSet(item, childActionRows, allowedTypes, actions);

                    if ((type == ActionType.Page) || (type == ActionType.GlobalNavigationLink))
                    {
                        if (parent != null) parent.ChildActions.Add(item);
                        actions.Add(item);
                    }
                    else if (type == ActionType.Control && parent != null)
                        parent.ChildControls.Add(item);
                }
            }

            if (parent != null)
            {
                if (parent.ChildActions.Count > 1) parent.ChildActions.Sort();
            }
            else if (actions.Count > 1)
            {
                actions.Sort();
            }
        }

        #endregion

        #region Internal Methods

        internal static bool AccessDeniedToSettingsDiagnosticPage()
        {
            return (!(SettingProvider.GroupSettingsExist && GroupProvider.GroupsExist));
        }

        internal static Action CreateAction(ConfigurationDataSet.ActionRow row)
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
                action.SubmenuItemWidth = row.SubmenuItemWidth;
                action.HighlightInSubmenu = row.HighlightInSubmenu;
                action.NavigateUrl = (row.IsNavigateUrlNull() ? null : row.NavigateUrl);
                action.LearnMoreUrl = row.LearnMoreUrl;
                action.VideoUrl = row.VideoUrl;
                action.OrderNumber = row.OrderNumber;
                action.AuthenticationRequired = row.AuthenticationRequired;
                action.OrganizationRequired = row.OrganizationRequired;
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

                if (action.ActionId == LoginGlobalNavigationLinkActionId)
                    action.NavigateUrl = WebApplication.LoginProvider.GetLoginUrl(false);
                else if (action.ActionId == MyAccountGlobalNavigationLinkActionId)
                {
                    if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme != Micajah.Common.Pages.MasterPageTheme.Modern)
                        action.OrderNumber = -action.OrderNumber;
                }

                foreach (ConfigurationDataSet.ActionsParentActionsRow alternativeParentActionRow in row.GetActionsParentActionsRowsByFK_Mc_ActionsParentActions_Mc_Action_2())
                {
                    action.AlternativeParentActions.Add(alternativeParentActionRow.ParentActionId);
                }

                return action;
            }
            return null;
        }

        internal static Action CreateAction(ConfigurationDataSet.ActionRow row, Action parentAction)
        {
            Action action = CreateAction(row);
            if (action != null)
                action.ParentAction = parentAction;
            return action;
        }

        internal static void Fill(ConfigurationDataSet dataSet)
        {
            if (dataSet == null) return;

            ConfigurationDataSet.ActionRow row = dataSet.Action.NewActionRow();
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
        /// Gets the identifiers of actions associated with specified role.
        /// </summary>
        /// <param name="roleId">Specifies the role's identifier.</param>
        /// <returns>The System.Collections.ArrayList that contains identifiers of actions associated with specified role.</returns>
        internal static ArrayList GetActionIdListByRoleId(Guid roleId)
        {
            ArrayList actionIdList = new ArrayList();

            ConfigurationDataSet.RolesActionsDataTable table = ConfigurationDataSet.Current.RolesActions;
            foreach (ConfigurationDataSet.RolesActionsRow row in table.Select(string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", table.RoleIdColumn.ColumnName, roleId.ToString())))
            {
                actionIdList.Add(row.ActionId);
            }

            return actionIdList;
        }

        internal static ArrayList GetActionIdList(ArrayList roleIdList, bool isOrgAdmin, bool removeDuplicates)
        {
            bool isInstAdmin = false;
            Guid roleId = RoleProvider.AssumeRole(isOrgAdmin, roleIdList, ref isInstAdmin);
            ArrayList list = new ArrayList();

            if (roleId != Guid.Empty)
                list.AddRange(GetActionIdListByRoleId(roleId));

            if (isOrgAdmin)
                list.AddRange(GetActionIdListByRoleId(RoleProvider.OrganizationAdministratorRoleId));

            if (isInstAdmin)
                list.AddRange(GetActionIdListByRoleId(RoleProvider.InstanceAdministratorRoleId));

            if (removeDuplicates)
                RemoveDuplicates(ref list);

            return list;
        }

        internal static ArrayList GetActionIdList(ArrayList groupIdList, ArrayList roleIdList, Guid instanceId, Guid organizationId, bool isOrgAdmin)
        {
            ArrayList list = new ArrayList();

            list.AddRange(GetActionIdList(roleIdList, false, false));

            if (groupIdList.Count > 0)
            {
                groupIdList.Sort();

                foreach (Guid roleId in roleIdList)
                {
                    list.AddRange(GetActionIdListByRoleId(roleId));
                }

                if (roleIdList.Count > 0)
                    RemoveDuplicates(ref list);

                OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
                SortedList list2 = GetGroupsInstanceActionIdList(groupIdList, instanceId, ds);

                if (list2 != null)
                {
                    foreach (Guid actionId in list2.Keys)
                    {
                        if ((bool)list2[actionId])
                        {
                            if (!list.Contains(actionId))
                                list.Add(actionId);
                        }
                        else if (list.Contains(actionId))
                            list.Remove(actionId);
                    }
                }
            }

            if (isOrgAdmin)
                list.AddRange(GetActionIdListByRoleId(RoleProvider.OrganizationAdministratorRoleId));

            RemoveDuplicates(ref list);

            return list;
        }

        /// <summary>
        /// Returns the actions table.
        /// </summary>
        /// <returns>The actions table.</returns>
        internal static DataTable GetActionsTree()
        {
            ConfigurationDataSet.ActionDataTable table = ConfigurationDataSet.Current.Action.Copy() as ConfigurationDataSet.ActionDataTable;

            foreach (ConfigurationDataSet.ActionRow row in table.Select(string.Concat(table.ParentActionIdColumn.ColumnName, " = '", ConfigurationPageActionId.ToString(), "'")))
            {
                row.ParentActionId = ConfigurationGlobalNavigationLinkActionId;
            }

            foreach (ConfigurationDataSet.ActionRow row in table.Select(string.Concat(table.ParentActionIdColumn.ColumnName, " = '", MyAccountPageActionId.ToString(), "'")))
            {
                row.ParentActionId = MyAccountGlobalNavigationLinkActionId;
            }

            ConfigurationDataSet.ActionRow actionRow = table.FindByActionId(ConfigurationPageActionId);
            if (actionRow != null) actionRow.Delete();

            actionRow = table.FindByActionId(MyAccountPageActionId);
            if (actionRow != null) actionRow.Delete();

            actionRow = table.FindByActionId(LoginGlobalNavigationLinkActionId);
            if (actionRow != null) actionRow.Delete();

            actionRow = table.FindByActionId(LoginAsUserGlobalNavigationLinkActionId);
            if (actionRow != null) actionRow.Delete();

            table.AcceptChanges();

            ArrayList setupActionIdList = new ArrayList();

            foreach (ConfigurationDataSet.ActionRow row in table)
            {
                if (IsSetupPage(row))
                    setupActionIdList.Add(row.ActionId);
            }

            foreach (Guid actionId in setupActionIdList)
            {
                actionRow = table.FindByActionId(actionId);
                if (actionRow != null) actionRow.Delete();
            }

            table.AcceptChanges();

            return table;
        }

        internal static ArrayList GetAlternativeParentActionsIdList(Guid actionId)
        {
            ArrayList list = new ArrayList();
            ConfigurationDataSet.ActionsParentActionsDataTable table = ConfigurationDataSet.Current.ActionsParentActions;
            foreach (ConfigurationDataSet.ActionsParentActionsRow row in table.Select(string.Concat(table.ActionIdColumn.ColumnName, " = '", actionId.ToString(), "'")))
            {
                if (!list.Contains(row.ParentActionId)) list.Add(row.ParentActionId);
            }
            return list;
        }

        internal static DataTable GetAlternativeParentActionsTree(Guid actionId)
        {
            ConfigurationDataSet.ActionDataTable table = GetActionsTree() as ConfigurationDataSet.ActionDataTable;

            ArrayList list = new ArrayList();
            FillActionIdList(ConfigurationDataSet.Current.Action.FindByActionId(actionId), ref list);

            foreach (ConfigurationDataSet.ActionRow row in table.Rows)
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

        ///// <summary>
        ///// Gets a value indicating that the specified action is setting page.
        ///// </summary>
        ///// <param name="actionId">The identifier of the action to check.</param>
        ///// <returns>true, if the specified action is configuration page; otherwise, false.</returns>
        //internal static bool IsSettingPage(Guid actionId)
        //{
        //    if (actionId == Guid.Empty)
        //        return false;
        //    return SettingsActionIdList.Contains(actionId);
        //}

        /// <summary>
        /// Gets a value indicating that the authentication is not required for the specified URL.
        /// </summary>
        /// <param name="navigateUrl">The string that contains the URL to check.</param>
        /// <returns>true, if the authentication is not required for the specified URL; otherwise, false.</returns>
        internal static bool IsPublicPage(string navigateUrl)
        {
            Action action = FindAction(Guid.Empty, CustomUrlProvider.CreateApplicationAbsoluteUrl(navigateUrl));
            if (action != null)
            {
                return (!action.AuthenticationRequired);
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating that the specified action is setup page.
        /// </summary>
        /// <param name="action">The action to check.</param>
        /// <returns>true, if the specified action is setup page; otherwise, false.</returns>
        internal static bool IsSetupPage(Action action)
        {
            if ((action.ActionId == SetupPageActionId) || (action.ActionId == SetupGlobalNavigationLinkActionId))
                return true;
            else
            {
                if (action.ActionType == ActionType.Page)
                    return ResourceProvider.IsSetupPageUrl(action.NavigateUrl);
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating that the specified action is setup page.
        /// </summary>
        /// <param name="action">The action to check.</param>
        /// <returns>true, if the specified action is setup page; otherwise, false.</returns>
        internal static bool IsSetupPage(ConfigurationDataSet.ActionRow action)
        {
            if ((ActionType)action.ActionTypeId == ActionType.Page)
            {
                if ((action.ActionId == SetupPageActionId) || (action.ActionId == SetupGlobalNavigationLinkActionId))
                    return true;
                else if (!action.IsNavigateUrlNull())
                    return ResourceProvider.IsSetupPageUrl(action.NavigateUrl);
            }
            return false;
        }

        internal static void Refresh()
        {
            lock (s_PagesAndControlsSyncRoot)
            {
                s_GlobalNavigationLinks = null;
                s_PagesAndControls = null;
            }
        }

        internal static void Refresh(Guid groupId, Guid instanceId)
        {
            lock (s_GroupInstanceActionIdListSyncRoot)
            {
                CacheManager.Current.Remove(string.Format(CultureInfo.InvariantCulture, "mc.GroupInstanceActionIdList.{0:N}.{1:N}", groupId, instanceId));
            }

            lock (s_GroupsInstanceActionIdListSyncRoot)
            {
                string keyToRemove = null;
                string g = groupId.ToString("N");
                foreach (string key in GroupsInstanceActionIdListKeys)
                {
                    string[] parts = key.Split('[');
                    if (parts.Length > 0)
                    {
                        parts = parts[1].Split(']')[0].Split(',');
                        foreach (string p in parts)
                        {
                            if (string.Compare(p, g, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                CacheManager.Current.Remove(key);
                                keyToRemove = key;
                                break;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(keyToRemove))
                    GroupsInstanceActionIdListKeys.Remove(keyToRemove);
            }
        }

        internal static void RemoveDuplicates(ref ArrayList actionIdList)
        {
            ArrayList list = new ArrayList(actionIdList);
            actionIdList.Clear();

            foreach (Guid actionId in list)
            {
                if (!actionIdList.Contains(actionId))
                    actionIdList.Add(actionId);
            }
        }

        internal static bool ShowAction(Action action, IList actionIdList, bool isFrameworkAdmin, bool isAuthenticated)
        {
            if (action.AuthenticationRequired)
            {
                if (isAuthenticated)
                {
                    if (IsSetupPage(action))
                    {
                        if (!isFrameworkAdmin)
                            return false;
                    }
                    else if (!(((actionIdList != null) && actionIdList.Contains(action.ActionId))
                        || action.ActionId == MyAccountMenuGlobalNavigationLinkActionId
                        || action.ActionId == LoginAsUserGlobalNavigationLinkActionId))
                    {
                        return false;
                    }
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
            return ConfigurationDataSet.Current.Action;
        }

        /// <summary>
        /// Gets an object populated with information of the specified action.
        /// </summary>
        /// <param name="actionId">Specifies the action identifier to get information.</param>
        /// <returns>The object populated with information of the specified action. If the action is not found, the method returns null reference.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static Action GetAction(Guid actionId)
        {
            return FindAction(actionId);
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
