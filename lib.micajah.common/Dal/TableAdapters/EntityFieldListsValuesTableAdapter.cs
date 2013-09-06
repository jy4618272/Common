using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_EntityFieldListsValues table2.
    /// </summary>
    internal class EntityFieldListsValuesTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the EntityFieldListsValuesTableAdapter class.
        /// </summary>
        public EntityFieldListsValuesTableAdapter()
        {
            #region TableMapping

            TableName = TableName.EntityFieldListsValues;
            TableMapping.ColumnMappings.Add("EntityFieldListValueId", "EntityFieldListValueId");
            TableMapping.ColumnMappings.Add("EntityFieldId", "EntityFieldId");
            TableMapping.ColumnMappings.Add("Name", "Name");
            TableMapping.ColumnMappings.Add("Value", "Value");
            TableMapping.ColumnMappings.Add("Default", "Default");
            TableMapping.ColumnMappings.Add("Active", "Active");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertEntityFieldListValue";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@EntityFieldListValueId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityFieldListValueId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@EntityFieldId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityFieldId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Value", SqlDbType.NVarChar, 512, ParameterDirection.Input, 0, 0, "Value", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Default", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Default", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Active", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateEntityFieldListValue";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@EntityFieldListValueId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityFieldListValueId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@EntityFieldId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityFieldId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Value", SqlDbType.NVarChar, 512, ParameterDirection.Input, 0, 0, "Value", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Default", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Default", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Active", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommands

            SelectCommand.CommandText = "dbo.Mc_GetEntityFieldListValues";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@EntityFieldId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityFieldId", DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Active", DataRowVersion.Current, false, null, "", "", ""));

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetEntityFieldListValue"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@EntityFieldListValueId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityFieldListValueId", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            #endregion

            #region DeleteCommand

            DeleteCommand.CommandText = "dbo.Mc_DeleteEntityFieldListValue";
            DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@EntityFieldListValueId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityFieldListValueId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion
        }

        #endregion
    }
}