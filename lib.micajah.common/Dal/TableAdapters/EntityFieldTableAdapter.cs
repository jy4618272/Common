using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_EntityField table2.
    /// </summary>
    internal class EntityFieldTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the EntityFieldTableAdapter class.
        /// </summary>
        public EntityFieldTableAdapter()
        {
            #region TableMapping

            TableName = TableName.EntityField;
            TableMapping.ColumnMappings.Add("EntityFieldId", "EntityFieldId");
            TableMapping.ColumnMappings.Add("EntityFieldTypeId", "EntityFieldTypeId");
            TableMapping.ColumnMappings.Add("Name", "Name");
            TableMapping.ColumnMappings.Add("Description", "Description");
            TableMapping.ColumnMappings.Add("DataTypeId", "DataTypeId");
            TableMapping.ColumnMappings.Add("DefaultValue", "DefaultValue");
            TableMapping.ColumnMappings.Add("AllowDBNull", "AllowDBNull");
            TableMapping.ColumnMappings.Add("Unique", "Unique");
            TableMapping.ColumnMappings.Add("MaxLength", "MaxLength");
            TableMapping.ColumnMappings.Add("MinValue", "MinValue");
            TableMapping.ColumnMappings.Add("MaxValue", "MaxValue");
            TableMapping.ColumnMappings.Add("DecimalDigits", "DecimalDigits");
            TableMapping.ColumnMappings.Add("OrderNumber", "OrderNumber");
            TableMapping.ColumnMappings.Add("EntityId", "EntityId");
            TableMapping.ColumnMappings.Add("OrganizationId", "OrganizationId");
            TableMapping.ColumnMappings.Add("InstanceId", "InstanceId");
            TableMapping.ColumnMappings.Add("Active", "Active");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertEntityField";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@EntityFieldId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityFieldId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@EntityFieldTypeId", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "EntityFieldTypeId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 1024, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@DataTypeId", SqlDbType.Int, 4, ParameterDirection.Input, 0, 0, "DataTypeId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@DefaultValue", SqlDbType.NVarChar, 512, ParameterDirection.Input, 0, 0, "DefaultValue", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@AllowDBNull", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "AllowDBNull", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Unique", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Unique", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@MaxLength", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "MaxLength", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@MinValue", SqlDbType.NVarChar, 512, ParameterDirection.Input, 0, 0, "MinValue", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@MaxValue", SqlDbType.NVarChar, 512, ParameterDirection.Input, 0, 0, "MaxValue", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@DecimalDigits", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "DecimalDigits", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@OrderNumber", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "OrderNumber", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@EntityId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Active", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateEntityField";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@EntityFieldId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityFieldId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@EntityFieldTypeId", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "EntityFieldTypeId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 1024, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@DataTypeId", SqlDbType.Int, 4, ParameterDirection.Input, 0, 0, "DataTypeId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@DefaultValue", SqlDbType.NVarChar, 512, ParameterDirection.Input, 0, 0, "DefaultValue", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@AllowDBNull", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "AllowDBNull", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Unique", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Unique", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@MaxLength", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "MaxLength", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@MinValue", SqlDbType.NVarChar, 512, ParameterDirection.Input, 0, 0, "MinValue", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@MaxValue", SqlDbType.NVarChar, 512, ParameterDirection.Input, 0, 0, "MaxValue", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@DecimalDigits", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "DecimalDigits", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@OrderNumber", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "OrderNumber", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@EntityId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Active", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommands

            SelectCommand.CommandText = "dbo.Mc_GetEntityFields";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@EntityId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityId", DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Active", DataRowVersion.Current, false, null, "", "", ""));

            SqlCommand command = new SqlCommand("dbo.Mc_GetEntityField");
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            command.Parameters.Add(new SqlParameter("@EntityFieldId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityFieldId", DataRowVersion.Current, false, null, "", "", ""));
            SelectCommands.Add(command);

            #endregion

            #region DeleteCommand

            DeleteCommand.CommandText = "dbo.Mc_DeleteEntityField";
            DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@EntityFieldId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityFieldId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion
        }

        #endregion
    }
}