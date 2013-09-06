using Micajah.Common.Application;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Dal.TableAdapters;
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
                        OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(user.SelectedOrganizationId);
                        groupsExist = (ds.Group.Count > 0);
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
                    OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
                    if (ds != null)
                    {
                        OrganizationDataSet.GroupsInstancesRolesDataTable gdrTable = ds.GroupsInstancesRoles;

                        StringBuilder sb = new StringBuilder();
                        foreach (Guid roleId in roleIdList)
                        {
                            sb.AppendFormat(CultureInfo.InvariantCulture, ",'{0}'", roleId);
                        }

                        if (sb.Length > 0)
                        {
                            sb.Remove(0, 1);

                            string filter = string.Empty;
                            if (instanceId != Guid.Empty)
                                filter = string.Concat(gdrTable.InstanceIdColumn.ColumnName, " = '", instanceId.ToString(), "'");
                            if (!string.IsNullOrEmpty(filter))
                                filter += " AND ";
                            filter += string.Concat("CONVERT(", gdrTable.RoleIdColumn.ColumnName, ", 'System.String') IN (", sb.ToString(), ")");

                            foreach (OrganizationDataSet.GroupsInstancesRolesRow gdrRow in gdrTable.Select(filter))
                            {
                                if (!groupIdList.Contains(gdrRow.GroupId))
                                    groupIdList.Add(gdrRow.GroupId);
                            }
                        }
                    }
                }
            }
            return groupIdList;
        }

        /// <summary>
        /// Returns the object populated with information of the built-in group that is associated to the specified instance.
        /// </summary>
        /// <param name="ds">The data sourceRow to get data from.</param>
        /// <param name="instanceId">The unique identifier of the instance.</param>
        /// <returns>The object populated with information of the group. If the group is not found, the method returns null reference.</returns>
        private static OrganizationDataSet.GroupRow GetInstanceAdministratorGroupRow(OrganizationDataSet ds, Guid instanceId)
        {
            OrganizationDataSet.GroupRow row = null;
            OrganizationDataSet.GroupsInstancesRolesDataTable table = ds.GroupsInstancesRoles;
            DataRow[] rows = table.Select(string.Format(CultureInfo.InvariantCulture
                , "{0} = '{1}' AND {2} = '{3}'"
                , table.InstanceIdColumn.ColumnName, instanceId.ToString()
                , table.RoleIdColumn.ColumnName, RoleProvider.InstanceAdministratorRoleId.ToString()));
            if (rows.Length > 0)
            {
                OrganizationDataSet.GroupsInstancesRolesRow row1 = (rows[0] as OrganizationDataSet.GroupsInstancesRolesRow);
                if (row1 != null)
                    row = row1.GroupRow;
            }
            return row;
        }

        /// <summary>
        /// Updates the details of specified group in the specified organization.
        /// </summary>
        /// <param name="row">The object populated with information of the group to update.</param>
        /// <param name="name">The name of the group.</param>
        /// <param name="description">The description of the group.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        private static void UpdateGroupRow(OrganizationDataSet.GroupRow row, string name, string description, Guid organizationId)
        {
            if (row == null) return;

            try
            {
                row.Name = name;
            }
            catch (ConstraintException ex)
            {
                throw new ConstraintException(string.Format(CultureInfo.CurrentCulture, Resources.GroupProvider_ErrorMessage_GroupAlreadyExists, name), ex);
            }

            if (description != null)
                row.Description = description;

            WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId).GroupTableAdapter.Update(row);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Gets the identifiers of actions associated with specified group role in specified instance.
        /// </summary>
        /// <param name="groupId">The group's identifier.</param>
        /// <param name="instanceId">The instance's identifier.</param>
        /// <param name="roleId">The group's role identifier in specified instance.</param>
        /// <param name="table2">The data sourceRow to get data from.</param>
        /// <returns>The System.Collections.ArrayList that contains identifiers of actions associated with specified group role in specified instance.</returns>
        internal static ArrayList GetActionIdList(Guid groupId, Guid instanceId, Guid roleId, OrganizationDataSet.GroupsInstancesActionsDataTable table)
        {
            ArrayList list = ActionProvider.GetActionIdListByRoleId(roleId);

            foreach (OrganizationDataSet.GroupsInstancesActionsRow actionRow in table.Select(string.Format(CultureInfo.InvariantCulture, "{0} = '{1}' AND {2} = '{3}'", table.GroupIdColumn.ColumnName, groupId.ToString(), table.InstanceIdColumn.ColumnName, instanceId.ToString())))
            {
                Guid actionId = actionRow.ActionId;
                if (!actionRow.Enabled)
                    list.Remove(actionId);
                else
                    list.Add(actionId);
            }

            return list;
        }

        /// <summary>
        /// Gets the identifiers of actions associated with specified group role in specified instance.
        /// </summary>
        /// <param name="groupId">The group's identifier.</param>
        /// <param name="instanceId">The instance's identifier.</param>
        /// <param name="roleId">The group's role identifier in specified instance.</param>
        /// <returns>The System.Collections.ArrayList that contains identifiers of actions associated with specified group role in specified instance.</returns>
        internal static ArrayList GetActionIdList(Guid groupId, Guid instanceId, Guid roleId)
        {
            return GetActionIdList(groupId, instanceId, roleId, WebApplication.GetOrganizationDataSetByOrganizationId(UserContext.Current.SelectedOrganizationId).GroupsInstancesActions);
        }

        internal static List<Guid> GetGroupsInstances(string groupId, Guid organizationId)
        {
            List<Guid> instanceIdList = new List<Guid>();

            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            OrganizationDataSet.GroupsInstancesRolesDataTable table = ds.GroupsInstancesRoles;

            StringBuilder sb = new StringBuilder();
            foreach (string id in groupId.Split(','))
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, ",'{0}'", id);
            }
            if (sb.Length > 0)
            {
                sb.Remove(0, 1);
                sb.Append(")");
                sb.Insert(0, ", 'System.String') IN (");
                sb.Insert(0, table.GroupIdColumn.ColumnName);
                sb.Insert(0, "CONVERT(");
            }

            foreach (OrganizationDataSet.GroupsInstancesRolesRow row in table.Select(sb.ToString()))
            {
                if (!instanceIdList.Contains(row.InstanceId))
                    instanceIdList.Add(row.InstanceId);
            }

            return instanceIdList;
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
        /// Modifies the actions list associated with specified group role in specified instance.
        /// </summary>
        /// <param name="groupId">The group's identifier.</param>
        /// <param name="instanceId">The instance's identifier.</param>
        /// <param name="actionIdList">The ArrayList that contains identifiers of actions.</param>
        internal static void GroupsInstancesActionsAcceptChanges(Guid groupId, Guid instanceId, ArrayList actionIdList)
        {
            UserContext ctx = UserContext.Current;
            Guid roleId = Guid.Empty;
            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(ctx.SelectedOrganizationId);
            OrganizationDataSet.GroupsInstancesActionsDataTable table = ds.GroupsInstancesActions;
            OrganizationDataSet.GroupsInstancesRolesRow roleRow = ds.GroupsInstancesRoles.FindByGroupIdInstanceId(groupId, instanceId);

            if (roleRow != null) roleId = roleRow.RoleId;

            if (roleId != Guid.Empty)
            {
                ArrayList list = ActionProvider.GetActionIdListByRoleId(roleId);

                foreach (OrganizationDataSet.GroupsInstancesActionsRow actionRow in table.Select(string.Format(CultureInfo.InvariantCulture, "{0} = '{1}' AND {2} = '{3}'"
                    , table.GroupIdColumn.ColumnName, groupId.ToString(), table.InstanceIdColumn.ColumnName, instanceId.ToString())))
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

                OrganizationDataSet.GroupsInstancesActionsRow newActionRow = null;

                foreach (Guid actionId in list)
                {
                    if (!actionIdList.Contains(actionId))
                    {
                        newActionRow = table.FindByGroupIdInstanceIdActionId(groupId, instanceId, actionId);
                        if (newActionRow == null)
                        {
                            newActionRow = table.NewGroupsInstancesActionsRow();

                            newActionRow.GroupId = groupId;
                            newActionRow.InstanceId = instanceId;
                            newActionRow.ActionId = actionId;
                            newActionRow.Enabled = false;

                            table.AddGroupsInstancesActionsRow(newActionRow);
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
                        newActionRow = table.FindByGroupIdInstanceIdActionId(groupId, instanceId, actionId);
                        if (newActionRow == null)
                        {
                            newActionRow = table.NewGroupsInstancesActionsRow();

                            newActionRow.GroupId = groupId;
                            newActionRow.InstanceId = instanceId;
                            newActionRow.ActionId = actionId;
                            newActionRow.Enabled = true;

                            table.AddGroupsInstancesActionsRow(newActionRow);
                        }
                        else if (!newActionRow.Enabled)
                            newActionRow.Enabled = true;
                    }
                }

                WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(ctx.SelectedOrganizationId).GroupsInstancesActionsTableAdapter.Update(table);

                ActionProvider.Refresh(groupId, instanceId);
            }
        }

        /// <summary>
        /// Creates new group with specified details in specified organization.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="description">The group description.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="builtIn">Whether the group is built-in.</param>
        /// <param name="refreshOrganizationData">true to refresh cached organization's data.</param>
        /// <returns>The System.Guid that represents the identifier of the newly created group.</returns>
        internal static Guid InsertGroup(string name, string description, Guid organizationId, bool builtIn, bool refreshOrganizationData)
        {
            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            if (ds == null) return Guid.Empty;

            OrganizationDataSet.GroupDataTable table = ds.Group;
            OrganizationDataSet.GroupRow row = table.NewGroupRow();

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

            ClientDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            if (adapters != null) adapters.GroupTableAdapter.Update(row);

            Guid groupId = row.GroupId;

            if (refreshOrganizationData) WebApplication.RefreshOrganizationData(organizationId);

            return groupId;
        }

        /// <summary>
        /// Assignes the specified role for specified group in specified instance in specified organization.
        /// </summary>
        /// <param name="groupId">Specifies the group's identifier.</param>
        /// <param name="instanceId">Specifies the instance's identifier.</param>
        /// <param name="roleId">Specifies the role's identifier.</param>
        /// <param name="organizationId">Specifies the organization's identifier.</param>
        /// <param name="refreshOrganizationData">true to refresh cached organization's data.</param>
        internal static void InsertGroupInstanceRole(Guid groupId, Guid instanceId, Guid roleId, Guid organizationId, bool refreshOrganizationData)
        {
            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            if (ds == null) return;

            OrganizationDataSet.GroupsInstancesRolesDataTable table = ds.GroupsInstancesRoles;
            OrganizationDataSet.GroupsInstancesRolesRow row = table.NewGroupsInstancesRolesRow();

            row.GroupId = groupId;
            row.InstanceId = instanceId;
            row.RoleId = roleId;

            table.AddGroupsInstancesRolesRow(row);

            ClientDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            if (adapters != null) adapters.GroupsInstancesRolesTableAdapter.Update(row);

            if (refreshOrganizationData) WebApplication.RefreshOrganizationData(organizationId);
        }

        /// <summary>
        /// Removes the specified rows collection from GroupsInstancesRoles data table2.
        /// </summary>
        /// <param name="rows">The rows collection to remove.</param>
        internal static void RemoveGroupsInstancesRolesRows(OrganizationDataSet.GroupsInstancesRolesRow[] rows)
        {
            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(UserContext.Current.SelectedOrganizationId);
            OrganizationDataSet.GroupsInstancesRolesDataTable table = ds.GroupsInstancesRoles;
            foreach (OrganizationDataSet.GroupsInstancesRolesRow row in rows)
            {
                table.RemoveGroupsInstancesRolesRow(row);
            }
        }

        /// <summary>
        /// Updates the name of the built-in group that is associated to the specified instance.
        /// </summary>
        /// <param name="ds">The data sourceRow.</param>
        /// <param name="instanceId">The unique identifier of the instance.</param>
        /// <param name="name">The name of the instance.</param>
        internal static void UpdateInstanceAdministratorGroup(OrganizationDataSet ds, Guid organizationid, Guid instanceId, string name)
        {
            OrganizationDataSet.GroupRow groupRow = GroupProvider.GetInstanceAdministratorGroupRow(ds, instanceId);
            if (groupRow != null)
                UpdateGroupRow(groupRow, string.Format(CultureInfo.InvariantCulture, Resources.GroupProvider_InstanceAdministratorGroup_Name, name), Resources.GroupProvider_InstanceAdministratorGroup_Description, organizationid);
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
            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(UserContext.Current.SelectedOrganizationId);
            if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
                return ds.Group.Copy();
            else
            {
                OrganizationDataSet.GroupDataTable table = ds.Group;
                OrganizationDataSet.GroupDataTable newTable = table.Copy() as OrganizationDataSet.GroupDataTable;
                Instance firstInstance = InstanceProvider.GetFirstInstance();

                foreach (OrganizationDataSet.GroupRow gRow in table)
                {
                    foreach (OrganizationDataSet.GroupsInstancesRolesRow girRow in gRow.GetGroupsInstancesRolesRows())
                    {
                        Guid instanceId = girRow.InstanceId;
                        if ((firstInstance != null) && (instanceId == firstInstance.InstanceId))
                        {
                            Guid roleId = girRow.RoleId;
                            if (roleId == RoleProvider.InstanceAdministratorRoleId)
                            {
                                OrganizationDataSet.GroupRow row = newTable.FindByGroupId(gRow.GroupId);
                                if (row != null)
                                    newTable.RemoveGroupRow(row);
                            }
                        }
                    }
                }

                newTable.AcceptChanges();

                return newTable;
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
            OrganizationDataSet.GroupDataTable table = GetGroups() as OrganizationDataSet.GroupDataTable;
            if (!table.Columns.Contains("SortNumber"))
                table.Columns.Add("SortNumber", typeof(int), "IIF((GroupId = '00000000-0000-0000-0000-000000000000'), 0, IIF((BuiltIn = 'True'), 1, 2))");

            if (includeOrganizationAdministratorGroup)
            {
                if (table.Columns.Contains("SortNumber"))
                {
                    if (table.FindByGroupId(Guid.Empty) == null)
                    {
                        OrganizationDataSet.GroupRow row = table.NewGroupRow();

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
        /// <returns>The System.Data.DataTable object that contains groups.</returns>
        public static DataTable GetGroups(Guid organizationId)
        {
            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            return ((ds == null) ? null : ds.Group);
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
        public static OrganizationDataSet.GroupRow GetGroupRow(Guid groupId)
        {
            return WebApplication.GetOrganizationDataSetByOrganizationId(UserContext.Current.SelectedOrganizationId).Group.FindByGroupId(groupId);
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
            return InsertGroup(name, description, UserContext.Current.SelectedOrganizationId, false, false);
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
            return InsertGroup(name, description, organizationId, false, true);
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
            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            if (ds != null)
                UpdateGroupRow(ds.Group.FindByGroupId(groupId), name, ((description == null) ? string.Empty : description), organizationId);
        }

        /// <summary>
        /// Marks as deleted the specified group.
        /// </summary>
        /// <param name="groupId">Specifies the group's identifier.</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteGroup(Guid groupId)
        {
            UserContext ctx = UserContext.Current;
            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(ctx.SelectedOrganizationId);
            OrganizationDataSet.GroupDataTable groupTable = ds.Group;
            OrganizationDataSet.GroupRow row = groupTable.FindByGroupId(groupId);
            if (row == null) return;

            row.Deleted = true;

            WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(ctx.SelectedOrganizationId).GroupTableAdapter.Update(row);

            OrganizationDataSet.GroupsInstancesActionsDataTable actionTable = ds.GroupsInstancesActions;
            foreach (OrganizationDataSet.GroupsInstancesActionsRow actionRow in row.GetGroupsInstancesActionsRows())
            {
                actionTable.RemoveGroupsInstancesActionsRow(actionRow);
            }

            RemoveGroupsInstancesRolesRows(row.GetGroupsInstancesRolesRows());
            groupTable.RemoveGroupRow(row);

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
            DataTable table = null;
            try
            {
                table = new DataTable();
                table.Locale = CultureInfo.CurrentCulture;
                table.Columns.Add("GroupId", typeof(Guid));
                table.Columns.Add("InstanceId", typeof(Guid));
                table.Columns.Add("InstanceName", typeof(string));
                table.Columns.Add("RoleId", typeof(Guid));
                table.Columns.Add("RoleName", typeof(string));

                OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(UserContext.Current.SelectedOrganizationId);
                OrganizationDataSet.GroupRow row = ds.Group.FindByGroupId(groupId);

                if (row != null)
                {
                    OrganizationDataSet.InstanceDataTable instanceTable = ds.Instance;
                    CommonDataSet.RoleDataTable roleTable = WebApplication.CommonDataSet.Role;
                    Instance firstInstance = InstanceProvider.GetFirstInstance();

                    foreach (OrganizationDataSet.GroupsInstancesRolesRow gdrRow in row.GetGroupsInstancesRolesRows())
                    {
                        Guid instanceId = gdrRow.InstanceId;
                        Guid roleId = gdrRow.RoleId;

                        if (!FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
                        {
                            if ((roleId == RoleProvider.InstanceAdministratorRoleId) || ((firstInstance != null) && (instanceId != firstInstance.InstanceId)))
                                continue;
                        }

                        OrganizationDataSet.InstanceRow instanceRow = instanceTable.FindByInstanceId(instanceId);
                        CommonDataSet.RoleRow roleRow = roleTable.FindByRoleId(roleId);

                        if ((instanceRow != null) && (roleRow != null))
                        {
                            DataRow newRow = table.NewRow();
                            newRow["GroupId"] = groupId;
                            newRow["InstanceId"] = instanceId;
                            newRow["InstanceName"] = instanceRow.Name;
                            newRow["RoleId"] = roleId;
                            newRow["RoleName"] = roleRow.Name;

                            table.Rows.Add(newRow);
                        }
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
        /// Returns the DataTable that contains the roles list of the specified groups list inside instances.
        /// </summary>
        /// <param name="groupId">Specifies the group's identifier.</param>
        /// <returns>The DataTable that contains the roles list of the specified groups list inside instances.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetGroupsInstancesRoles(List<Guid> groupIds)
        {
            DataTable table = null;

            try
            {
                if (groupIds != null)
                {
                    table = new DataTable();
                    table.Locale = CultureInfo.CurrentCulture;
                    table.Columns.Add("GroupId", typeof(Guid));
                    table.Columns.Add("ParentGroupId", typeof(Guid));
                    table.Columns.Add("Name", typeof(string));
                    table.Columns.Add("InstancesRoles", typeof(string));

                    OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(UserContext.Current.SelectedOrganizationId);
                    OrganizationDataSet.InstanceDataTable instanceTable = ds.Instance;
                    CommonDataSet.RoleDataTable roleTable = WebApplication.CommonDataSet.Role;
                    Instance firstInstance = InstanceProvider.GetFirstInstance();

                    foreach (Guid groupId in groupIds)
                    {
                        OrganizationDataSet.GroupRow row = ds.Group.FindByGroupId(groupId);

                        if (row != null)
                        {
                            DataRow newRow = table.NewRow();
                            newRow["GroupId"] = groupId;
                            newRow["Name"] = row.Name;
                            newRow["ParentGroupId"] = DBNull.Value;
                            string instancesRoles = string.Empty;
                            bool isInstanceActive = true;

                            foreach (OrganizationDataSet.GroupsInstancesRolesRow gdrRow in row.GetGroupsInstancesRolesRows())
                            {
                                Guid instanceId = gdrRow.InstanceId;
                                Guid roleId = gdrRow.RoleId;

                                if (!FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
                                {
                                    if ((roleId == RoleProvider.InstanceAdministratorRoleId) || ((firstInstance != null) && (instanceId != firstInstance.InstanceId)))
                                        continue;
                                }

                                OrganizationDataSet.InstanceRow instanceRow = instanceTable.FindByInstanceId(instanceId);
                                CommonDataSet.RoleRow roleRow = roleTable.FindByRoleId(roleId);

                                if ((instanceRow != null) && (roleRow != null))
                                {
                                    if (instanceRow.Active)
                                    {
                                        if (instancesRoles.Trim().Length > 0)
                                            instancesRoles += ", ";
                                        instancesRoles += string.Format(CultureInfo.InvariantCulture, "{0} | {1}", instanceRow.Name, roleRow.Name);
                                    }
                                    else
                                        isInstanceActive = false;
                                }
                            }
                            newRow["InstancesRoles"] = instancesRoles;
                            if (isInstanceActive)
                                table.Rows.Add(newRow);
                        }
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
            InsertGroupInstanceRole(groupId, instanceId, roleId, user.SelectedOrganizationId, false);
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
            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(ctx.SelectedOrganizationId);
            OrganizationDataSet.GroupsInstancesRolesRow row = ds.GroupsInstancesRoles.FindByGroupIdInstanceId(groupId, instanceId);

            if (row != null)
            {
                row.Delete();

                OrganizationDataSet.GroupsInstancesActionsDataTable table = ds.GroupsInstancesActions;
                foreach (OrganizationDataSet.GroupsInstancesActionsRow actionRow in table.Select(string.Format(CultureInfo.InvariantCulture, "{0} = '{1}' AND {2} = '{3}'", table.GroupIdColumn.ColumnName, groupId.ToString(), table.InstanceIdColumn.ColumnName, instanceId.ToString())))
                {
                    actionRow.Delete();
                }

                ClientDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(ctx.SelectedOrganizationId);
                adapters.GroupsInstancesActionsTableAdapter.Update(table);
                adapters.GroupsInstancesRolesTableAdapter.Update(row);

                ctx.Refresh();
            }
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
                OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
                if (ds != null)
                {
                    OrganizationDataSet.GroupDataTable table = ds.Group;
                    OrganizationDataSet.GroupRow[] rows = table.Select(string.Format(CultureInfo.InvariantCulture, "{0} = '{1}' AND {2} = 0", table.NameColumn.ColumnName, Support.PreserveSingleQuote(name), table.DeletedColumn.ColumnName)) as OrganizationDataSet.GroupRow[];
                    if (rows.Length > 0) groupId = rows[0].GroupId;
                }
            }
            return groupId;
        }

        #endregion
    }
}
