using Micajah.Common.Application;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using System;
using System.Collections;
using System.Data;
using System.Text;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with roles.
    /// </summary>
    public static class RoleProvider
    {
        #region Members

        /// <summary>
        /// The identifier of the organization administrator role.
        /// </summary>
        public readonly static Guid OrganizationAdministratorRoleId = new Guid("00000000-0000-0000-0000-000000000001");

        /// <summary>
        /// The identifier of the instance administrator role.
        /// </summary>
        public readonly static Guid InstanceAdministratorRoleId = new Guid("00000000-0000-0000-0000-000000000002");

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the highest rank of the roles.
        /// </summary>
        public static int HighestRank
        {
            get
            {
                int rank = 0;
                foreach (CommonDataSet.RoleRow row in WebApplication.CommonDataSet.Role.Rows)
                {
                    if (row.Rank > rank) rank = row.Rank;
                }
                return rank;
            }
        }

        #endregion

        #region Private Methods

        private static void Fill(CommonDataSet.RoleDataTable roleTable, RoleElementCollection roles)
        {
            foreach (RoleElement role in roles)
            {
                CommonDataSet.RoleRow roleRow = roleTable.NewRoleRow();
                Fill(roleRow, role);
                roleTable.AddRoleRow(roleRow);
            }
        }

        private static void Fill(CommonDataSet.RoleRow row, RoleElement role)
        {
            row.RoleId = role.Id;
            row.Name = role.Name;
            row.Description = (string.IsNullOrEmpty(role.Description) ? string.Empty : role.Description);
            row.ShortName = role.ShortName;
            row.Rank = role.Rank;
            row.StartActionId = role.StartPageId;
            row.BuiltIn = role.BuiltIn;
        }

        #endregion

        #region Internal Methods

        // The administrators only assume the highest non built-in role; the other users assume the lowest non built-in role from the list.
        internal static Guid AssumeRole(bool isOrgAdmin, ArrayList roleIdList, ref bool isInstanceAdmin)
        {
            bool isAdminOnly = false;
            int rolesCount = ((roleIdList == null) ? 0 : roleIdList.Count);

            if (rolesCount > 0)
            {
                isInstanceAdmin = roleIdList.Contains(InstanceAdministratorRoleId);
                if (isInstanceAdmin)
                    isAdminOnly = true;
            }
            else
                isAdminOnly = isOrgAdmin;

            //if (!isInstanceAdmin)
            //    isInstanceAdmin = isOrgAdmin;

            return (isAdminOnly ? GetHighestNonBuiltInRoleId() : GetHighestNonBuiltInRoleId(roleIdList));
        }

        internal static Guid AssumeRole(bool isOrgAdmin, ref ArrayList roleIdList, ref string startUrl)
        {
            bool instanceRequired = false;
            bool isInstanceAdmin = false;

            Guid roleId = AssumeRole(isOrgAdmin, roleIdList, ref isInstanceAdmin);

            roleIdList.Clear();
            if (roleId != Guid.Empty)
            {
                roleIdList.Add(roleId);
                startUrl = GetStartActionNavigateUrl(roleId, out instanceRequired);
            }

            if (isInstanceAdmin)
            {
                if (!roleIdList.Contains(InstanceAdministratorRoleId))
                    roleIdList.Add(InstanceAdministratorRoleId);
            }

            if (isOrgAdmin)
            {
                if (!roleIdList.Contains(OrganizationAdministratorRoleId))
                    roleIdList.Add(OrganizationAdministratorRoleId);

                if (startUrl == null)
                    startUrl = GetStartActionNavigateUrl(OrganizationAdministratorRoleId, out instanceRequired);
            }
            else if (startUrl == null)
                startUrl = GetStartActionNavigateUrl(InstanceAdministratorRoleId, out instanceRequired);

            return roleId;
        }

        internal static void Fill(CommonDataSet dataSet)
        {
            if (dataSet == null) return;

            Fill(dataSet.Role, FrameworkConfiguration.Current.Roles);

            dataSet.Role.AcceptChanges();
        }

        internal static void FillRolesActions(CommonDataSet dataSet, ActionElement action)
        {
            ArrayList roleList = GetRoleIdListByShortNames(dataSet.Role, action.Roles);
            foreach (Guid roleId in roleList)
            {
                if ((!FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances) && (roleId == InstanceAdministratorRoleId))
                    continue;

                if (dataSet.RolesActions.FindByRoleIdActionId(roleId, action.Id) == null)
                {
                    CommonDataSet.RolesActionsRow rolesActionsRow = dataSet.RolesActions.NewRolesActionsRow();
                    rolesActionsRow.ActionId = action.Id;
                    rolesActionsRow.RoleId = roleId;
                    dataSet.RolesActions.AddRolesActionsRow(rolesActionsRow);
                }
            }
        }

        /// <summary>
        /// Returns the action's identifier of the start page for specified role.
        /// </summary>
        /// <param name="roleId">The identifier of the role.</param>
        /// <returns>The unique identifier of the action of the start page for specified role.</returns>
        internal static Guid GetStartActionId(Guid roleId)
        {
            CommonDataSet.RoleRow row = WebApplication.CommonDataSet.Role.FindByRoleId(roleId);
            return (row == null ? Guid.Empty : row.StartActionId);
        }

        /// <summary>
        /// Returns the highest by rank non built-in role.
        /// </summary>
        /// <returns>The role identifier.</returns>
        internal static Guid GetHighestNonBuiltInRoleId()
        {
            CommonDataSet.RoleDataTable table = WebApplication.CommonDataSet.Role;
            CommonDataSet.RoleRow searchedRoleRow = null;
            Guid roleId = Guid.Empty;
            foreach (CommonDataSet.RoleRow row in table.Rows)
            {
                roleId = row.RoleId;
                if (!IsBuiltIn(roleId))
                {
                    if (searchedRoleRow == null)
                        searchedRoleRow = row;
                    else if (row.Rank < searchedRoleRow.Rank)
                        searchedRoleRow = row;
                }
            }
            return ((searchedRoleRow != null) ? searchedRoleRow.RoleId : Guid.Empty);
        }

        /// <summary>
        /// Returns the lowest by rank non built-in role from specified roles list.
        /// </summary>
        /// <param name="roleIdList">The array of roles identifiers.</param>
        /// <returns>The role identifier.</returns>
        internal static Guid GetLowestNonBuiltInRoleId(IList roleIdList)
        {
            CommonDataSet.RoleRow searchedRoleRow = null;

            if (roleIdList != null)
            {
                CommonDataSet.RoleDataTable table = WebApplication.CommonDataSet.Role;
                CommonDataSet.RoleRow row = null;

                foreach (Guid roleId in roleIdList)
                {
                    if (!IsBuiltIn(roleId))
                    {
                        row = table.FindByRoleId(roleId);
                        if (row != null)
                        {
                            if (searchedRoleRow == null)
                                searchedRoleRow = row;
                            else if (row.Rank > searchedRoleRow.Rank)
                                searchedRoleRow = row;
                        }
                    }
                }
            }

            return ((searchedRoleRow == null) ? Guid.Empty : searchedRoleRow.RoleId);
        }

        /// <summary>
        /// Returns the highest by rank non built-in role from specified roles list.
        /// </summary>
        /// <param name="roleIdList">The array of roles identifiers.</param>
        /// <returns>The role identifier.</returns>
        internal static Guid GetHighestNonBuiltInRoleId(IList roleIdList)
        {
            CommonDataSet.RoleRow searchedRoleRow = null;

            if (roleIdList != null)
            {
                CommonDataSet.RoleDataTable table = WebApplication.CommonDataSet.Role;
                CommonDataSet.RoleRow row = null;

                foreach (Guid roleId in roleIdList)
                {
                    if (!IsBuiltIn(roleId))
                    {
                        row = table.FindByRoleId(roleId);
                        if (row != null)
                        {
                            if (searchedRoleRow == null)
                                searchedRoleRow = row;
                            else if (row.Rank < searchedRoleRow.Rank)
                                searchedRoleRow = row;
                        }
                    }
                }
            }

            return ((searchedRoleRow == null) ? Guid.Empty : searchedRoleRow.RoleId);
        }

        /// <summary>
        /// Returns the highest by rank built-in role from specified roles list.
        /// </summary>
        /// <param name="roleIdList">The array of roles identifiers.</param>
        /// <returns>The role identifier.</returns>
        internal static Guid GetHighestBuiltInRoleId(IList roleIdList)
        {
            CommonDataSet.RoleRow searchedRoleRow = null;

            if (roleIdList != null)
            {
                CommonDataSet.RoleDataTable table = WebApplication.CommonDataSet.Role;
                CommonDataSet.RoleRow row = null;

                foreach (Guid roleId in roleIdList)
                {
                    if (IsBuiltIn(roleId))
                    {
                        row = table.FindByRoleId(roleId);
                        if (row != null)
                        {
                            if (searchedRoleRow == null)
                                searchedRoleRow = row;
                            else if (row.Rank < searchedRoleRow.Rank)
                                searchedRoleRow = row;
                        }
                    }
                }
            }

            return ((searchedRoleRow == null) ? Guid.Empty : searchedRoleRow.RoleId);
        }

        /// <summary>
        /// Returns a navigate URL of the start page's action of the specified role.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="instanceRequired">The instance required flag of the start action.</param>
        /// <returns>The System.String that represents the navigate URL of the start page's action or null reference, if the action was not found.</returns>
        internal static string GetStartActionNavigateUrl(Guid roleId, out bool instanceRequired)
        {
            instanceRequired = false;
            CommonDataSet.RoleRow row = WebApplication.CommonDataSet.Role.FindByRoleId(roleId);
            Action action = null;
            if (row != null)
            {
                action = ActionProvider.PagesAndControls.FindByActionId(row.StartActionId);
                if (row.ActionRow != null)
                    instanceRequired = row.ActionRow.InstanceRequired;
            }
            return ((action == null) ? null : action.AbsoluteNavigateUrl);
        }

        /// <summary>
        /// Returns the list of the non built-in roles lower by rank than specified.
        /// </summary>
        /// <param name="roleId">The unique identifier of the role.</param>
        /// <returns>The list of the non built-in roles lower by rank than specified.</returns>
        internal static ArrayList GetLowerNonBuiltInRoleIdList(Guid roleId)
        {
            ArrayList list = new ArrayList();
            CommonDataSet.RoleDataTable table = WebApplication.CommonDataSet.Role;
            CommonDataSet.RoleRow row = table.FindByRoleId(roleId);
            if (row != null)
            {
                foreach (CommonDataSet.RoleRow row1 in table)
                {
                    if (!row1.BuiltIn)
                    {
                        if (row1.Rank > row.Rank)
                            list.Add(row1.RoleId);
                    }
                }
            }
            return list;
        }

        internal static ArrayList GetRoleIdListByShortNames(IEnumerable shortName)
        {
            return GetRoleIdListByShortNames(WebApplication.CommonDataSet.Role, shortName);
        }

        internal static ArrayList GetRoleIdListByShortNames(CommonDataSet.RoleDataTable table, IEnumerable shortName)
        {
            ArrayList roleIdList = new ArrayList();

            if ((table != null) && (shortName != null))
            {
                StringBuilder sb = new StringBuilder();
                foreach (string name in shortName)
                {
                    if (string.Compare(name, "*", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        if (sb.Length > 0)
                            sb.Remove(0, sb.Length - 1);
                        break;
                    }
                    sb.AppendFormat(",'{0}'", name.Replace("'", "''"));
                }

                if (sb.Length > 0)
                {
                    sb.Remove(0, 1);
                    sb.Append(")");
                    sb.Insert(0, string.Concat(table.ShortNameColumn.ColumnName, " IN ("));
                }

                foreach (CommonDataSet.RoleRow row in table.Select(sb.ToString()))
                {
                    if (!roleIdList.Contains(row.RoleId)) roleIdList.Add(row.RoleId);
                }
            }

            return roleIdList;
        }

        /// <summary>
        /// Returns the name of the specified role.
        /// </summary>
        /// <param name="roleId">Specifies the role's identifier.</param>
        /// <returns>The System.String that represents the name of the specified role.</returns>
        internal static string GetRoleName(Guid roleId)
        {
            CommonDataSet.RoleRow row = WebApplication.CommonDataSet.Role.FindByRoleId(roleId);
            return ((row == null) ? string.Empty : row.Name);
        }

        internal static bool IsBuiltIn(Guid roleId)
        {
            CommonDataSet.RoleRow row = WebApplication.CommonDataSet.Role.FindByRoleId(roleId);
            return ((row == null) ? false : row.BuiltIn);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the roles, excluding marked as deleted.
        /// </summary>
        /// <returns>The DataTable that contains roles.</returns>s
        public static DataTable GetRoles()
        {
            return WebApplication.CommonDataSet.Role;
        }

        /// <summary>
        /// Gets the roles, excluding the built-in roles.
        /// </summary>
        /// <returns>The System.Data.DataTable that contains roles.</returns>
        public static DataTable GetAvailableRoles()
        {
            return GetAvailableRoles(false);
        }

        /// <summary>
        /// Gets the non buil-in roles, but including the instance administrator role.
        /// </summary>
        /// <param name="includeInstanceAdministratorRole">The flag specifying that the instance administrator role will be included in result.</param>
        /// <returns>The System.Data.DataTable that contains roles.</returns>
        public static DataTable GetAvailableRoles(bool includeInstanceAdministratorRole)
        {
            return GetAvailableRoles(includeInstanceAdministratorRole, false);
        }

        /// <summary>
        /// Gets the non buil-in roles, but including the instance administrator role.
        /// </summary>
        /// <param name="includeInstanceAdministratorRole">The flag specifying that the instance administrator role will be included in result.</param>
        /// <param name="includeOrganizationAdministratorRole">The flag specifying that the organization administrator role will be included in result.</param>
        /// <returns>The System.Data.DataTable that contains roles.</returns>
        public static DataTable GetAvailableRoles(bool includeInstanceAdministratorRole, bool includeOrganizationAdministratorRole)
        {
            CommonDataSet.RoleDataTable table = WebApplication.CommonDataSet.Role.Copy() as CommonDataSet.RoleDataTable;
            CommonDataSet.RoleRow row = null;

            if (!includeOrganizationAdministratorRole)
            {
                row = table.FindByRoleId(OrganizationAdministratorRoleId);
                if (row != null) table.RemoveRoleRow(row);
            }

            if ((!includeInstanceAdministratorRole) || (!FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances))
            {
                row = table.FindByRoleId(InstanceAdministratorRoleId);
                if (row != null) table.RemoveRoleRow(row);
            }

            return table;
        }

        /// <summary>
        /// Gets the non buil-in roles, but including the instance administrator role.
        /// </summary>
        /// <param name="includeInstanceAdministratorRole">The flag specifying that the instance administrator role will be included in result.</param>
        /// <param name="sortExpression">The sort column or columns, and sort order for the result.</param>
        /// <returns>The System.Data.DataView that contains roles.</returns>
        public static DataView GetAvailableRoles(bool includeInstanceAdministratorRole, string sortExpression)
        {
            DataTable table = GetAvailableRoles(includeInstanceAdministratorRole);
            DataView dv = table.DefaultView;
            dv.Sort = sortExpression;
            return dv;
        }

        /// <summary>
        /// Returns an object populated with information of the specified role.
        /// </summary>
        /// <param name="roleId">Specifies the role identifier to get information.</param>
        /// <returns>
        /// The object populated with information of the specified role. 
        /// If the role is not found, the method returns null reference.
        /// </returns>
        public static CommonDataSet.RoleRow GetRoleRow(Guid roleId)
        {
            return WebApplication.CommonDataSet.Role.FindByRoleId(roleId);
        }

        /// <summary>
        /// Returns an object populated with information of the role that the group has in the instance.
        /// </summary>
        /// <param name="organizationId">The unique identifier of the organization.</param>
        /// <param name="groupId">The unique identifier of the group.</param>
        /// <param name="instanceId">The unique identifier of the instance.</param>
        /// <returns></returns>
        public static CommonDataSet.RoleRow GetRoleRow(Guid organizationId, Guid groupId, Guid instanceId)
        {
            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            if (ds != null)
            {
                OrganizationDataSet.GroupsInstancesRolesRow row = ds.GroupsInstancesRoles.FindByGroupIdInstanceId(groupId, instanceId);
                if (row != null)
                    return GetRoleRow(row.RoleId);
            }
            return null;
        }

        /// <summary>
        /// Returns the identifier of the role with specified name.
        /// </summary>
        /// <param name="name">The name of the role.</param>
        /// <returns>The identifier of the role with specified name.</returns>
        public static Guid GetRoleIdByName(string name)
        {
            Guid roleId = Guid.Empty;

            if (!string.IsNullOrEmpty(name))
            {
                WebApplication.RefreshCommonData();
                CommonDataSet.RoleDataTable table = WebApplication.CommonDataSet.Role.Copy() as CommonDataSet.RoleDataTable;
                CommonDataSet.RoleRow[] rows = table.Select(string.Concat(table.NameColumn.ColumnName, " = '", name.Replace("'", "''"), "'")) as CommonDataSet.RoleRow[];
                if (rows.Length > 0) roleId = rows[0].RoleId;
            }
            return roleId;
        }

        #endregion
    }
}