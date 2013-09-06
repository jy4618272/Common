using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_EntityNodeType table2.
    /// </summary>
    internal class EntityNodeTypeTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the EntityNodeTypeTableAdapter class.
        /// </summary>
        public EntityNodeTypeTableAdapter()
        {
            #region TableMapping

            TableName = TableName.EntityNodeType;
            TableMapping.ColumnMappings.Add("EntityNodeTypeId", "EntityNodeTypeId");
            TableMapping.ColumnMappings.Add("EntityId", "EntityId");
            TableMapping.ColumnMappings.Add("Name", "Name");
            TableMapping.ColumnMappings.Add("OrderNumber", "OrderNumber");
            TableMapping.ColumnMappings.Add("OrganizationId", "OrganizationId");
            TableMapping.ColumnMappings.Add("InstanceId", "InstanceId");
            TableMapping.ColumnMappings.Add("Deleted", "Deleted");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertEntityNodeType";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@EntityNodeTypeId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityNodeTypeId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@EntityId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@OrderNumber", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "OrderNumber", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Deleted", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Deleted", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateEntityNodeType";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@EntityNodeTypeId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityNodeTypeId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@EntityId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@OrderNumber", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "OrderNumber", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Deleted", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Deleted", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommand

            SelectCommand.CommandText = "dbo.Mc_GetEntityNodeTypes";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetEntityNodeTypesByEntityId"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@EntityId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityId", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetEntityNodeType"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@EntityNodeTypeId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityNodeTypeId", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            #endregion
        }

        #endregion
    }
}