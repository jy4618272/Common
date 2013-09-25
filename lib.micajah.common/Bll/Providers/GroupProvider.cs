using Micajah.Common.Application;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Dal.ClientDataSetTableAdapters;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Text;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with groups.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class GroupProvider
    {
        #region Internal Properties

        internal static bool GroupsExist
        {
            get
            {
                bool groupsExist = false;
                UserContext user = UserContext.Current;
                if (user != null)
                {
                    if (user.SelectedOrganizationId != Guid.Empty)
                    {
                        using (GroupTableAdapter adapter = new GroupTableAdapter(OrganizationProvider.GetConnectionString(user.SelectedOrganizationId)))
                        {
                            groupsExist = (adapter.GetGroups(user.SelectedOrganizationId).Count > 0);
                        }
                    }
                }
                return groupsExist;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns the list of the groups wich have the specified roles in the specified instance.
        /// </summary>
        /// <param name="organizationId">The organizations' identifier.</param>
        /// <param name="instanceId">The instance's identifier.</param>
        /// <param name="roleIdList">An array containing the unique identifiers of the roles.</param>
        /// <returns>The list of the groups.</returns>
        public static ArrayList GetGroupIdList(Guid organizationId, Guid instanceId, ArrayList roleIdList)
        {
            ArrayList groupIdList = new ArrayList();
            if (roleIdList != null)
            {
                if (roleIdList.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (Guid roleId in roleIdList)
                    {
                        sb.AppendFormat(CultureInfo.InvariantCulture, ",'{0}'", roleId);
                    }

                    if (sb.Length > 0)
                    {
                        sb.Remove(0, 1);

                        using (GroupTableAdapter adapter = new GroupTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                        {
                            foreach (ClientDataSet.GroupRow row in adapter.GetGroupsByRoles(organizationId, ((instanceId == Guid.Empty) ? null : new Guid?(instanceId)), sb.ToString()))
                            {
                                if (!groupIdList.Contains(row.GroupId))
                                    groupIdList.Add(row.GroupId);
                            }
                        }
                    }
                }
            }
            return groupIdList;
        }

        private static ClientDataSet.GroupsInstancesRolesDataTable GetGroupsInstancesRolesByGroups(Guid organizationId, List<Guid> groupIds)
        {
            using (GroupsInstancesRolesTableAdapter adapter = new GroupsInstancesRolesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetGroupsInstancesRolesByGroups(organizationId, Support.ConvertListToString(groupIds));
            }
        }

        private static ClientDataSet.GroupsInstancesRolesDataTable GetGroupsInstancesRolesByGroupsInstanceId(Guid organizationId, IEnumerable groupIds, Guid instanceId)
        {
            using (GroupsInstancesRolesTableAdapter adapter = new GroupsInstancesRolesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetGroupsInstancesRolesByGroupsInstanceId(organizationId, Support.ConvertListToString(groupIds), instanceId);
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Gets the identifiers of actions associated with specified group role in specified instance.
        /// </summary>
        /// <param name="groupId">The group's identifier.</param>
        /// <param name="instanceId">The instance's identifier.</param>
        /// <param name="roleId">The group's role identifier in specified instance.</param>
        /// <returns>The System.Collections.ArrayList that contains identifiers of actions associated with specified group role in specified instance.</returns>
        internal static ArrayList GetActionIdList(Guid groupId, Guid instanceId, Guid roleId)
        {
            ArrayList list = ActionProvider.GetActionIdListByRoleId(roleId);

            foreach (ClientDataSet.GroupsInstancesActionsRow actionRow in GetGroupsInstancesActionsByGroupIdInstanceId(UserContext.Current.SelectedOrganizationId, groupId, instanceId))
            {
                Guid actionId = actionRow.ActionId;
                if (!actionRow.Enabled)
                    list.Remove(actionId);
                else
                    list.Add(actionId);
            }

            return list;
        }

        internal static Guid GetGroupIdOfLowestRoleInInstance(Guid organizationId, Guid instanceId)
        {
            Instance inst = new Instance();
            if (inst.Load(organizationId, instanceId))
                return GetGroupIdOfLowestRoleInInstance(inst);
            return Guid.Empty;
        }

        internal static Guid GetGroupIdOfLowestRoleInInstance(Instance instance)
        {
            Guid groupId = Guid.Empty;
            if (instance != null)
            {
                Guid lowestRoleId = RoleProvider.GetLowestNonBuiltInRoleId(instance.GroupIdRoleIdList.GetValueList());
                if (lowestRoleId != Guid.Empty)
                {
                    int idx = instance.GroupIdRoleIdList.IndexOfValue(lowestRoleId);
                    if (idx > -1)
                        groupId = (Guid)instance.GroupIdRoleIdList.GetKey(idx);
                }
            }
            return groupId;
        }

        /// <summary>
        /// Gets the collection of group/role identifiers pairs for the specified instance.
        /// </summary>
        internal static SortedList GetGroupIdRoleIdList(Guid organizationId, Guid instanceId)
        {
            SortedList list = new SortedList();

            using (GroupsInstancesRolesTableAdapter adapter = new GroupsInstancesRolesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                foreach (ClientDataSet.GroupsInstancesRolesRow row in adapter.GetGroupsInstancesRolesByInstanceId(organizationId, instanceId))
                {
                    if ((RoleProvider.GetRoleRow(row.RoleId) != null) && (!list.Contains(row.GroupId)))
                        list.Add(row.GroupId, row.RoleId);
                }
            }

            return list;
        }

        internal static ClientDataSet.GroupsInstancesRolesDataTable GetGroupsInstancesRoles(Guid organizationId)
        {
            using (GroupsInstancesRolesTableAdapter adapter = new GroupsInstancesRolesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetGroupsInstancesRoles(organizationId);
            }
        }

        internal static ClientDataSet.GroupsInstancesRolesDataTable GetGroupsInstancesRolesByRoleId(Guid organizationId, Guid roleId)
        {
            using (GroupsInstancesRolesTableAdapter adapter = new GroupsInstancesRolesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetGroupsInstancesRolesByRoleId(organizationId, roleId);
            }
        }

        internal static ClientDataSet.GroupsInstancesActionsDataTable GetGroupsInstancesActionsByGroupIdInstanceId(Guid organizationId, Guid groupId, Guid instanceId)
        {
            using (GroupsInstancesActionsTableAdapter adapter = new GroupsInstancesActionsTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetGroupsInstancesActionsByGroupIdInstanceId(organizationId, groupId, instanceId);
            }
        }

        internal static Guid GetGroupInstanceRole(Guid organizationId, Guid groupId, Guid instanceId)
        {
            Guid roleId = Guid.Empty;
            using (GroupsInstancesRolesTableAdapter adapter = new GroupsInstancesRolesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                ClientDataSet.GroupsInstancesRolesDataTable table = adapter.GetGroupsInstancesRolesByGroupIdInstanceId(organizationId, groupId, instanceId);
                if (table.Count > 0)
                    roleId = table[0].RoleId;
            }
            return roleId;
        }

        /// <summary>
        /// Modifies the actions list associated with specified group role in specified instance.
        /// </summary>
        /// <param name="groupId">The group's identifier.</param>
        /// <param name="instanceId">The instance's identifier.</param>
        /// <param name="actionIdList">The ArrayList that contains identifiers of actions.</param>
        internal static void GroupsInstancesActionsAcceptChanges(Guid groupId, Guid instanceId, ArrayList actionIdList)
        {
            UserContext user = UserContext.Current;

            Guid roleId = GetGroupInstanceRole(user.SelectedOrganizationId, groupId, instanceId);

            if (roleId == Guid.Empty)
                return;

            ArrayList list = ActionProvider.GetActionIdListByRoleId(roleId);

            ClientDataSet.GroupsInstancesActionsDataTable giaTable = null;
            using (GroupsInstancesActionsTableAdapter adapter = new GroupsInstancesActionsTableAdapter(OrganizationProvider.GetConnectionString(user.SelectedOrganizationId)))
            {
                giaTable = adapter.GetGroupsInstancesActionsByGroupIdInstanceId(user.SelectedOrganizationId, groupId, instanceId);

                foreach (ClientDataSet.GroupsInstancesActionsRow actionRow in giaTable)
                {
                    Guid actionId = actionRow.ActionId;
                    if (actionIdList.Contains(actionId))
                    {
                        if (!actionRow.Enabled)
                            actionRow.Delete();
                        else
                            actionIdList.Remove(actionId);
                    }
                    else if (actionRow.Enabled)
                    {
                        actionRow.Delete();
                    }
                }

                ClientDataSet.GroupsInstancesActionsRow newActionRow = null;

                foreach (Guid actionId in list)
                {
                    if (!actionIdList.Contains(actionId))
                    {
                        newActionRow = giaTable.FindByGroupIdInstanceIdActionId(groupId, instanceId, actionId);
                        if (newActionRow == null)
                        {
                            newActionRow = giaTable.NewGroupsInstancesActionsRow();

                            newActionRow.GroupId = groupId;
                            newActionRow.InstanceId = instanceId;
                            newActionRow.ActionId = actionId;
                            newActionRow.Enabled = false;

                            giaTable.AddGroupsInstancesActionsRow(newActionRow);
                        }
                        else if (newActionRow.Enabled)
                            newActionRow.Enabled = false;
                    }
                    else actionIdList.Remove(actionId);
                }

                foreach (Guid actionId in actionIdList)
                {
                    if (!list.Contains(actionId))
                    {
                        newActionRow = giaTable.FindByGroupIdInstanceIdActionId(groupId, instanceId, actionId);
                        if (newActionRow == null)
                        {
                            newActionRow = giaTable.NewGroupsInstancesActionsRow();

                            newActionRow.GroupId = groupId;
                            newActionRow.InstanceId = instanceId;
                            newActionRow.ActionId = actionId;
                            newActionRow.Enabled = true;

                            giaTable.AddGroupsInstancesActionsRow(newActionRow);
                        }
                        else if (!newActionRow.Enabled)
                            newActionRow.Enabled = true;
                    }
                }

                adapter.Update(giaTable);
            }

            ActionProvider.Refresh(groupId, instanceId);
        }

        /// <summary>
        /// Creates new group with specified details in specified organization.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="description">The group description.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="builtIn">Whether the group is built-in.</param>
        /// <returns>The System.Guid that represents the identifier of the newly created group.</returns>
        internal static Guid InsertGroup(string name, string description, Guid organizationId, bool builtIn)
        {
            ClientDataSet.GroupDataTable table = new ClientDataSet.GroupDataTable();
            ClientDataSet.GroupRow row = table.NewGroupRow();

            row.GroupId = Guid.NewGuid();
            row.Name = name;
            row.Description = description;
            row.OrganizationId = organizationId;
            row.BuiltIn = builtIn;

            try
            {
                table.AddGroupRow(row);
            }
            catch (ConstraintException ex)
            {
                throw new ConstraintException(string.Format(CultureInfo.CurrentCulture, Resources.GroupProvider_ErrorMessage_GroupAlreadyExists, name), ex);
            }

            using (GroupTableAdapter adapter = new GroupTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                adapter.Update(row);
            }

            Guid groupId = row.GroupId;

            return groupId;
        }

        /// <summary>
        /// Assignes the specified role for specified group in specified instance in specified organization.
        /// </summary>
        /// <param name="groupId">Specifies the group's identifier.</param>
        /// <param name="instanceId">Specifies the instance's identifier.</param>
        /// <param name="roleId">Specifies the role's identifier.</param>
        /// <param name="organizationId">Specifies the organization's identifier.</param>
        internal static void InsertGroupInstanceRole(Guid groupId, Guid instanceId, Guid roleId, Guid organizationId)
        {
            using (GroupsInstancesRolesTableAdapter adapter = new GroupsInstancesRolesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                adapter.Insert(groupId, instanceId, roleId);
            }
        }

        internal static bool GroupsInstanceHasRole(Guid organizationId, IList groupIdList, Guid instanceId, IList roleIdList)
        {
            foreach (ClientDataSet.GroupsInstancesRolesRow gdrRow in GetGroupsInstancesRolesByGroupsInstanceId(organizationId, groupIdList, instanceId))
            {
                if (roleIdList.Contains(gdrRow.RoleId))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Updates the name of the built-in group that is associated to the specified instance.
        /// </summary>
        /// <param name="ds">The data sourceRow.</param>
        /// <param name="instanceId">The unique identifier of the instance.</param>
        /// <param name="name">The name of the instance.</param>
        internal static void UpdateInstanceAdministratorGroup(Guid organizationId, Guid instanceId, string name)
        {
            using (GroupTableAdapter adapter = new GroupTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                ClientDataSet.GroupDataTable table = adapter.GetGroupsByRoles(organizationId, ((instanceId == Guid.Empty) ? null : new Guid?(instanceId)), RoleProvider.InstanceAdministratorRoleId.ToString());
                if (table.Count > 0)
                {
                    ClientDataSet.GroupRow row = table[0];

                    try
                    {
                        row.Name = string.Format(CultureInfo.InvariantCulture, Resources.GroupProvider_InstanceAdministratorGroup_Name, name);
                    }
                    catch (ConstraintException ex)
                    {
                        throw new ConstraintException(string.Format(CultureInfo.CurrentCulture, Resources.GroupProvider_ErrorMessage_GroupAlreadyExists, name), ex);
                    }

                    row.Description = Resources.GroupProvider_InstanceAdministratorGroup_Description;

                    adapter.Update(row);
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the groups, excluding marked as deleted.
        /// </summary>
        /// <returns>The System.Data.DataTable object that contains groups.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetGroups()
        {
            Guid organizationId = UserContext.Current.SelectedOrganizationId;

            if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
                return GetGroups(organizationId);
            else
            {
                ClientDataSet.GroupDataTable table = GetGroups(organizationId);

                Instance firstInstance = InstanceProvider.GetFirstInstance();

                if (firstInstance != null)
                {
                    ArrayList list = new ArrayList();
                    ClientDataSet.GroupsInstancesRolesDataTable girTable = null;

                    using (GroupsInstancesRolesTableAdapter adapter = new GroupsInstancesRolesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                    {
                        girTable = adapter.GetGroupsInstancesRolesByInstanceId(organizationId, firstInstance.InstanceId);
                    }

                    foreach (ClientDataSet.GroupsInstancesRolesRow girRow in girTable)
                    {
                        if (girRow.RoleId == RoleProvider.InstanceAdministratorRoleId)
                        {
                            if (!list.Contains(girRow.GroupId))
                                list.Add(girRow.GroupId);
                        }
                    }

                    foreach (Guid groupId in list)
                    {
                        ClientDataSet.GroupRow row = table.FindByGroupId(groupId);
                        if (row != null)
                            table.RemoveGroupRow(row);
                    }

                    table.AcceptChanges();
                }

                return table;
            }
        }

        /// <summary>
        /// Gets the groups, excluding marked as deleted.
        /// </summary>
        /// <param name="includeOrganizationAdministratorGroup">true to add the built-in group for organization administrator; otherwise, false.</param>
        /// <returns>The System.Data.DataView object that contains groups.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetGroups(bool includeOrganizationAdministratorGroup)
        {
            ClientDataSet.GroupDataTable table = GetGroups() as ClientDataSet.GroupDataTable;
            if (!table.Columns.Contains("SortNumber"))
                table.Columns.Add("SortNumber", typeof(int), "IIF((GroupId = '00000000-0000-0000-0000-000000000000'), 0, IIF((BuiltIn = 'True'), 1, 2))");

            if (includeOrganizationAdministratorGroup)
            {
                if (table.Columns.Contains("SortNumber"))
                {
                    if (table.FindByGroupId(Guid.Empty) == null)
                    {
                        ClientDataSet.GroupRow row = table.NewGroupRow();

                        row.GroupId = Guid.Empty;
                        row.OrganizationId = UserContext.Current.SelectedOrganizationId;
                        row.Name = Resources.GroupProvider_OrganizationAdministratorGroupName;
                        row.Description = Resources.GroupProvider_OrganizationAdministratorGroupDescription;
                        row.BuiltIn = true;

                        table.AddGroupRow(row);
                        table.AcceptChanges();
                    }
                }
            }
            table.DefaultView.Sort = string.Format(CultureInfo.InvariantCulture, "SortNumber, {0}", table.NameColumn.ColumnName);
            return table.DefaultView;
        }

        /// <summary>
        /// Gets the groups for the specified organization, excluding marked as deleted.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <returns>The table object that contains groups.</returns>
        public static ClientDataSet.GroupDataTable GetGroups(Guid organizationId)
        {
            using (GroupTableAdapter adapter = new GroupTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetGroups(organizationId);
            }
        }

        /// <summary>
        /// Returns the list of the groups wich have the specified roles in the specified instance.
        /// </summary>
        /// <param name="organizationId">The organizations' identifier.</param>
        /// <param name="instanceId">The instance's identifier.</param>
        /// <param name="roleShortName">An array containing the short names of the roles.</param>
        /// <returns>The list of the groups.</returns>
        public static ArrayList GetGroupIdList(Guid organizationId, Guid instanceId, params string[] roleShortName)
        {
            ArrayList groupIdList = new ArrayList();
            if (roleShortName != null)
                groupIdList = GetGroupIdList(organizationId, instanceId, RoleProvider.GetRoleIdListByShortNames(roleShortName));
            return groupIdList;
        }

        /// <summary>
        /// Returns the list of the groups wich have the specified roles in the specified instance.
        /// </summary>
        /// <param name="organizationId">The organizations' identifier.</param>
        /// <param name="instanceId">The instance's identifier.</param>
        /// <param name="roleIdList">An array containing the unique identifiers of the roles.</param>
        /// <returns>The list of the groups.</returns>
        public static ArrayList GetGroupIdList(Guid organizationId, Guid instanceId, params Guid[] roleIdList)
        {
            ArrayList groupIdList = new ArrayList();
            if (roleIdList != null)
                groupIdList = GetGroupIdList(organizationId, instanceId, new ArrayList(roleIdList));
            return groupIdList;
        }

        /// <summary>
        /// Gets an object populated with information of the specified group.
        /// </summary>
        /// <param name="groupId">Specifies the group identifier to get information.</param>
        /// <returns>The object populated with information of the specified group. If the group is not found, the method returns null reference.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.GroupRow GetGroupRow(Guid groupId)
        {
            using (GroupTableAdapter adapter = new GroupTableAdapter(OrganizationProvider.GetConnectionString(UserContext.Current.SelectedOrganizationId)))
            {
                ClientDataSet.GroupDataTable table = adapter.GetGroup(groupId);
                return ((table.Count > 0) ? table[0] : null);
            }
        }

        /// <summary>
        /// Creates new group with specified details.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="description">The group description.</param>
        /// <returns>The System.Guid that represents the identifier of the newly created group.</returns>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertGroup(string name, string description)
        {
            return InsertGroup(name, description, UserContext.Current.SelectedOrganizationId, false);
        }

        /// <summary>
        /// Creates new group with specified details in specified organization
        /// and refreshes all cached data by application after the action has been performed.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="description">The group description.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <returns>The System.Guid that represents the identifier of the newly created group.</returns>
        public static Guid InsertGroup(string name, string description, Guid organizationId)
        {
            return InsertGroup(name, description, organizationId, false);
        }

        /// <summary>
        /// Updates the details of specified group.
        /// </summary>
        /// <param name="groupId">The identifier of the group.</param>
        /// <param name="name">The name of the group.</param>
        /// <param name="description">The group description.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateGroup(Guid groupId, string name, string description)
        {
            UpdateGroup(groupId, name, description, UserContext.Current.SelectedOrganizationId);
        }

        /// <summary>
        /// Updates the details of specified group in the specified organization.
        /// </summary>
        /// <param name="groupId">The identifier of the group.</param>
        /// <param name="name">The name of the group.</param>
        /// <param name="description">The group description.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        public static void UpdateGroup(Guid groupId, string name, string description, Guid organizationId)
        {
            ClientDataSet.GroupRow row = GetGroupRow(groupId);
            if (row != null)
            {
                try
                {
                    row.Name = name;
                }
                catch (ConstraintException ex)
                {
                    throw new ConstraintException(string.Format(CultureInfo.CurrentCulture, Resources.GroupProvider_ErrorMessage_GroupAlreadyExists, name), ex);
                }

                row.Description = ((description == null) ? string.Empty : description);

                using (GroupTableAdapter adapter = new GroupTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                {
                    adapter.Update(row);
                }
            }
        }

        /// <summary>
        /// Marks as deleted the specified group.
        /// </summary>
        /// <param name="groupId">Specifies the group's identifier.</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteGroup(Guid groupId)
        {
            UserContext ctx = UserContext.Current;

            ClientDataSet.GroupRow row = GetGroupRow(groupId);
            if (row != null)
            {
                row.Deleted = true;

                using (GroupTableAdapter adapter = new GroupTableAdapter(OrganizationProvider.GetConnectionString(ctx.SelectedOrganizationId)))
                {
                    adapter.Update(row);
                }
            }

            ctx.Refresh();
        }

        /// <summary>
        /// Returns the DataTable that contains the roles list of the specified group inside instances.
        /// </summary>
        /// <param name="groupId">Specifies the group's identifier.</param>
        /// <returns>The DataTable that contains the roles list of the specified group inside instances.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetGroupInstancesRoles(Guid groupId)
        {
            Guid organizationId = UserContext.Current.SelectedOrganizationId;

            ClientDataSet.GroupsInstancesRolesDataTable table = null;
            using (GroupsInstancesRolesTableAdapter adapter = new GroupsInstancesRolesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                table = adapter.GetGroupsInstancesRolesByGroupId(organizationId, groupId);
            }
            table.Columns.Add("RoleName", typeof(string));

            Instance firstInstance = InstanceProvider.GetFirstInstance();

            foreach (ClientDataSet.GroupsInstancesRolesRow row in table)
            {
                Guid roleId = row.RoleId;

                if (!FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
                {
                    if ((roleId == RoleProvider.InstanceAdministratorRoleId) || ((firstInstance != null) && (row.InstanceId != firstInstance.InstanceId)))
                        continue;
                }

                ConfigurationDataSet.RoleRow roleRow = RoleProvider.GetRoleRow(roleId);
                if (roleRow != null)
                    row["RoleName"] = roleRow.Name;
            }

            return table;
        }

        /// <summary>
        /// Returns the DataTable that contains the roles list of the specified groups list inside instances.
        /// </summary>
        /// <param name="groupId">Specifies the group's identifier.</param>
        /// <returns>The DataTable that contains the roles list of the specified groups list inside instances.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetGroupsInstancesRoles(List<Guid> groupIds)
        {
            if (groupIds == null)
                return null;

            DataTable table = null;

            try
            {
                Guid organizationId = UserContext.Current.SelectedOrganizationId;
                Instance firstInstance = InstanceProvider.GetFirstInstance();

                ClientDataSet.GroupsInstancesRolesDataTable girTable = GetGroupsInstancesRolesByGroups(organizationId, groupIds);

                table = girTable.DefaultView.ToTable(true, "GroupId", "Name");
                table.Columns.Add("ParentGroupId", typeof(Guid));
                table.Columns.Add("InstancesRoles", typeof(string));

                foreach (DataRow row in table.Rows)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (ClientDataSet.GroupsInstancesRolesRow gdrRow in girTable.Select(string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", girTable.GroupIdColumn.ColumnName, row["GroupId"])))
                    {
                        Guid roleId = gdrRow.RoleId;

                        if (!FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
                        {
                            if ((roleId == RoleProvider.InstanceAdministratorRoleId) || ((firstInstance != null) && (gdrRow.InstanceId != firstInstance.InstanceId)))
                                continue;
                        }

                        ConfigurationDataSet.RoleRow roleRow = RoleProvider.GetRoleRow(roleId);
                        if (roleRow != null)
                            sb.AppendFormat(CultureInfo.InvariantCulture, ",{0} | {1}", gdrRow["InstanceName"], roleRow.Name);
                    }
                    if (sb.Length > 0)
                    {
                        sb.Remove(0, 1);
                        row["InstancesRoles"] = sb.ToString();
                    }
                }

                return table;
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        /// <summary>
        /// Assignes the specified role for specified group in specified instance.
        /// </summary>
        /// <param name="groupId">Specifies the group's identifier.</param>
        /// <param name="instanceId">Specifies the instance's identifier.</param>
        /// <param name="roleId">Specifies the role's identifier.</param>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void InsertGroupInstanceRole(Guid groupId, Guid instanceId, Guid roleId)
        {
            UserContext user = UserContext.Current;
            InsertGroupInstanceRole(groupId, instanceId, roleId, user.SelectedOrganizationId);
            user.Refresh();
        }

        /// <summary>
        /// Deletes the role for specified group in specified instance.
        /// </summary>
        /// <param name="groupId">Specifies the group's identifier.</param>
        /// <param name="instanceId">Specifies the instance's identifier.</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteGroupInstanceRoles(Guid groupId, Guid instanceId)
        {
            UserContext ctx = UserContext.Current;

            using (GroupsInstancesRolesTableAdapter adapter = new GroupsInstancesRolesTableAdapter(OrganizationProvider.GetConnectionString(ctx.SelectedOrganizationId)))
            {
                adapter.Delete(groupId, instanceId);
            }

            ctx.Refresh();
        }

        /// <summary>
        /// Returns the identifier of the group with specified name in the specified organization.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="organizationId">The identifier of the organization which the group belong to.</param>
        /// <returns>The identifier of the group with specified name.</returns>
        public static Guid GetGroupIdByName(string name, Guid organizationId)
        {
            Guid groupId = Guid.Empty;
            if (!string.IsNullOrEmpty(name))
            {
                using (GroupTableAdapter adapter = new GroupTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                {
                    ClientDataSet.GroupDataTable table = adapter.GetGroupByName(name);
                    if (table.Count > 0) groupId = table[0].GroupId;
                }
            }
            return groupId;
        }

        #endregion
    }
}
