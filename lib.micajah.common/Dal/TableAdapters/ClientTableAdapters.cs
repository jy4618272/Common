using System;
using System.Collections;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The container class for the tables adapters of the organization and client dataset.
    /// </summary>
    public sealed class ClientTableAdapters : TableAdaptersHolder, ICloneable
    {
        #region Members

        private static ClientTableAdapters s_Current;

        private string m_ConnectionString;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the current instance of the class.
        /// </summary>
        public static ClientTableAdapters Current
        {
            get
            {
                if (s_Current == null)
                    s_Current = new ClientTableAdapters();
                return s_Current;
            }
            set
            {
                s_Current = value;
                Micajah.Common.Application.WebApplication.RefreshOrganizationDataSetTableAdaptersList();
                Micajah.Common.Application.WebApplication.RefreshOrganizationDataSets();
            }
        }

        public ITableAdapter InstanceTableAdapter { get { return this.TableAdapters[TableName.Instance]; } }

        public ITableAdapter GroupsInstancesActionsTableAdapter { get { return this.TableAdapters[TableName.GroupsInstancesActions]; } }

        public ITableAdapter GroupsInstancesRolesTableAdapter { get { return this.TableAdapters[TableName.GroupsInstancesRoles]; } }

        public ITableAdapter GroupTableAdapter { get { return this.TableAdapters[TableName.Group]; } }

        public ITableAdapter EntityFieldTableAdapter { get { return this.TableAdapters[TableName.EntityField]; } }

        public ITableAdapter EntityFieldListsValuesTableAdapter { get { return this.TableAdapters[TableName.EntityFieldListsValues]; } }

        public ITableAdapter EntityFieldsValuesTableAdapter { get { return this.TableAdapters[TableName.EntityFieldsValues]; } }

        public ITableAdapter EntityNodeTableAdapter { get { return this.TableAdapters[TableName.EntityNode]; } }

        public ITableAdapter EntityNodesRelatedEntityNodesTableAdapter { get { return (EntityNodesRelatedEntityNodesTableAdapter)this.TableAdapters[TableName.EntityNodesRelatedEntityNodes]; } }

        public ITableAdapter EntityNodeTypeTableAdapter { get { return this.TableAdapters[TableName.EntityNodeType]; } }

        public ITableAdapter MessageTableAdapter { get { return this.TableAdapters[TableName.Message]; } }

        public ITableAdapter OrganizationsUsersTableAdapter { get { return this.TableAdapters[TableName.OrganizationsUsers]; } }

        public ITableAdapter RecurringScheduleTableAdapter { get { return (RecurringScheduleTableAdapter)this.TableAdapters[TableName.RecurringSchedule]; } }

        public RuleTableAdapter RuleTableAdapter { get { return (RuleTableAdapter)this.TableAdapters[TableName.Rule]; } }

        public ITableAdapter RuleParametersTableAdapter { get { return (RuleParametersTableAdapter)this.TableAdapters[TableName.RuleParameters]; } }

        public ITableAdapter SettingsValuesTableAdapter { get { return this.TableAdapters[TableName.SettingsValues]; } }

        public ITableAdapter UsersGroupsTableAdapter { get { return this.TableAdapters[TableName.UsersGroups]; } }

        public ITableAdapter UsersInstancesTableAdapter { get { return this.TableAdapters[TableName.UsersInstances]; } }

        public ITableAdapter UserTableAdapter { get { return this.TableAdapters[TableName.User]; } }

        public string ConnectionString
        {
            get { return m_ConnectionString; }
            set { this.SetConnectionString(value); }
        }

        #endregion

        #region Constructors

        public ClientTableAdapters()
        {
            this.Initialize();
        }

        public ClientTableAdapters(ICollection adapters)
            : base(adapters)
        {
            this.Initialize();
        }

        #endregion

        #region Private Methods

        private void SetConnectionString(string value)
        {
            m_ConnectionString = value;
            foreach (TableName tableName in this.TableAdapters.Keys)
            {
                if (this.TableAdapters.ContainsKey(tableName))
                {
                    ITableAdapter adapter = this.TableAdapters[tableName];
                    if (adapter != null) adapter.ConnectionString = value;
                }
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Fills the tables of specified OrganizationDataSet.
        /// </summary>
        /// <param name="dataSet">The OrganizationDataSet to fill.</param>
        /// <param name="organizationId">The organization's identifier to get data to fill data set.</param>
        internal void Fill(OrganizationDataSet dataSet, Guid organizationId)
        {
            InstanceTableAdapter.Fill(dataSet.Instance, 0, organizationId);
            GroupsInstancesActionsTableAdapter.Fill(dataSet.GroupsInstancesActions, 0, organizationId);
            GroupsInstancesRolesTableAdapter.Fill(dataSet.GroupsInstancesRoles, 0, organizationId);
            GroupTableAdapter.Fill(dataSet.Group, 0, organizationId);
            SettingsValuesTableAdapter.Fill(dataSet.SettingsValues, 0, organizationId);
        }

        internal static void RefreshCurrent()
        {
            s_Current = null;
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            this.AddAdapter(TableName.Instance, typeof(InstanceTableAdapter));
            this.AddAdapter(TableName.GroupsInstancesActions, typeof(GroupsInstancesActionsTableAdapter));
            this.AddAdapter(TableName.GroupsInstancesRoles, typeof(GroupsInstancesRolesTableAdapter));
            this.AddAdapter(TableName.Group, typeof(GroupTableAdapter));
            this.AddAdapter(TableName.EntityField, typeof(EntityFieldTableAdapter));
            this.AddAdapter(TableName.EntityFieldListsValues, typeof(EntityFieldListsValuesTableAdapter));
            this.AddAdapter(TableName.EntityFieldsValues, typeof(EntityFieldsValuesTableAdapter));
            this.AddAdapter(TableName.EntityNode, typeof(EntityNodeTableAdapter));
            this.AddAdapter(TableName.EntityNodesRelatedEntityNodes, typeof(EntityNodesRelatedEntityNodesTableAdapter));
            this.AddAdapter(TableName.EntityNodeType, typeof(EntityNodeTypeTableAdapter));
            this.AddAdapter(TableName.Message, typeof(MessageTableAdapter));
            this.AddAdapter(TableName.OrganizationsUsers, typeof(OrganizationsUsersTableAdapter));
            this.AddAdapter(TableName.SettingsValues, typeof(SettingsValuesTableAdapter));
            this.AddAdapter(TableName.UsersGroups, typeof(UsersGroupsTableAdapter));
            this.AddAdapter(TableName.UsersInstances, typeof(UsersInstancesTableAdapter));
            this.AddAdapter(TableName.User, typeof(UserTableAdapter));
            this.AddAdapter(TableName.RecurringSchedule, typeof(RecurringScheduleTableAdapter));
            this.AddAdapter(TableName.Rule, typeof(RuleTableAdapter));
            this.AddAdapter(TableName.RuleParameters, typeof(RuleParametersTableAdapter));
        }

        #endregion

        #region Public Methods

        public ClientTableAdapters Clone()
        {
            ArrayList list = new ArrayList();
            foreach (TableName tableName in this.TableAdapters.Keys)
            {
                if (this.TableAdapters.ContainsKey(tableName))
                {
                    ITableAdapter adapter = this.TableAdapters[tableName];
                    if (adapter != null) list.Add(adapter.Clone());
                }
            }
            ClientTableAdapters adapters = new ClientTableAdapters(list);
            adapters.ConnectionString = this.ConnectionString;
            return adapters;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion
    }
}
