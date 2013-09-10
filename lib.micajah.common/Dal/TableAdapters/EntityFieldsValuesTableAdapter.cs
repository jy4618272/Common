using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_EntityFieldsValues table.
    /// </summary>
    internal class EntityFieldsValuesTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the EntityFieldsValuesTableAdapter class.
        /// </summary>
        public EntityFieldsValuesTableAdapter()
        {
            #region TableMapping

            TableName = TableName.EntityFieldsValues;
            TableMapping.ColumnMappings.Add("EntityFieldValueId", "EntityFieldValueId");
            TableMapping.ColumnMappings.Add("EntityFieldId", "EntityFieldId");
            TableMapping.ColumnMappings.Add("LocalEntityId", "LocalEntityId");
            TableMapping.ColumnMappings.Add("Value", "Value");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertEntityFieldValue";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@EntityFieldValueId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityFieldValueId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@EntityFieldId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityFieldId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@LocalEntityId", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LocalEntityId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Value", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Value", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateEntityFieldValue";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@EntityFieldValueId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityFieldValueId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@EntityFieldId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityFieldId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@LocalEntityId", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LocalEntityId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Value", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Value", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommands

            SelectCommand.CommandText = "dbo.Mc_GetEntityFieldsValues";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@EntityId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityId", DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@LocalEntityId", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LocalEntityId", DataRowVersion.Current, false, null, "", "", ""));

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetEntityFieldValues"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@EntityFieldId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityFieldId", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@LocalEntityId", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LocalEntityId", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            #endregion

            #region DeleteCommand

            DeleteCommand.CommandText = "dbo.Mc_DeleteEntityFieldValue";
            DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@EntityFieldValueId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityFieldValueId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion
        }

        #endregion
    }
}