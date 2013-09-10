using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_Resource table.
    /// </summary>
    internal class ResourceTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ResourceTableAdapter class.
        /// </summary>
        public ResourceTableAdapter()
        {
            #region TableMapping

            TableName = TableName.Resource;
            TableMapping.ColumnMappings.Add("ResourceId", "ResourceId");
            TableMapping.ColumnMappings.Add("ParentResourceId", "ParentResourceId");
            TableMapping.ColumnMappings.Add("LocalObjectType", "LocalObjectType");
            TableMapping.ColumnMappings.Add("LocalObjectId", "LocalObjectId");
            TableMapping.ColumnMappings.Add("Content", "Content");
            TableMapping.ColumnMappings.Add("ContentType", "ContentType");
            TableMapping.ColumnMappings.Add("Name", "Name");
            TableMapping.ColumnMappings.Add("Width", "Width");
            TableMapping.ColumnMappings.Add("Height", "Height");
            TableMapping.ColumnMappings.Add("Align", "Align");
            TableMapping.ColumnMappings.Add("Temporary", "Temporary");
            TableMapping.ColumnMappings.Add("CreatedTime", "CreatedTime");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertResource";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@ResourceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "ResourceId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@ParentResourceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "ParentResourceId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@LocalObjectType", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "LocalObjectType", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@LocalObjectId", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LocalObjectId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Content", SqlDbType.VarBinary, 0, ParameterDirection.Input, 0, 0, "Content", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@ContentType", SqlDbType.VarChar, 255, ParameterDirection.Input, 0, 0, "ContentType", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Width", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "Width", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Height", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "Height", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Align", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "Align", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Temporary", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Temporary", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@CreatedTime", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "CreatedTime", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateResource";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@ResourceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "ResourceId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@LocalObjectType", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "LocalObjectType", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@LocalObjectId", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LocalObjectId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Temporary", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Temporary", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region DeleteCommand

            DeleteCommand.CommandText = "dbo.Mc_DeleteResource";
            DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@ResourceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "ResourceId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommands

            SelectCommand.CommandText = "dbo.Mc_GetResource";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@ResourceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "ResourceId", DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@Width", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "Width", DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@Height", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "Height", DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@Align", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "Align", DataRowVersion.Current, false, null, "", "", ""));

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetResources"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@LocalObjectType", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "LocalObjectType", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@LocalObjectId", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LocalObjectId", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            #endregion
        }

        #endregion
    }
}