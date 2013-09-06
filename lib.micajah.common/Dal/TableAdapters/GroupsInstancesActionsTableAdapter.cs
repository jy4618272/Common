using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_GroupsInstancesActions table2.
    /// </summary>
    internal class GroupsInstancesActionsTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the GroupsInstancesActionsTableAdapter class.
        /// </summary>
        public GroupsInstancesActionsTableAdapter()
        {
            #region TableMapping

            TableName = TableName.GroupsInstancesActions;
            TableMapping.ColumnMappings.Add("GroupId", "GroupId");
            TableMapping.ColumnMappings.Add("InstanceId", "InstanceId");
            TableMapping.ColumnMappings.Add("ActionId", "ActionId");
            TableMapping.ColumnMappings.Add("Enabled", "Enabled");

            #endregion

            #region DeleteCommand

            DeleteCommand.CommandText = "dbo.Mc_DeleteGroupInstanceAction";
            DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "GroupId", DataRowVersion.Original, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Original, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@ActionId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "ActionId", DataRowVersion.Original, false, null, "", "", ""));

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertGroupInstanceAction";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "GroupId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@ActionId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "ActionId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Enabled", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Enabled", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateGroupInstanceAction";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "GroupId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@ActionId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "ActionId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Enabled", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Enabled", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommand

            SelectCommand.CommandText = "dbo.Mc_GetGroupsInstancesActions";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion
        }

        #endregion
    }
}