using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_CustomUrl table.
    /// </summary>
    internal class CustomUrlTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the CustomUrlTableAdapter class.
        /// </summary>
        public CustomUrlTableAdapter()
        {
            #region TableMapping

            TableName = TableName.CustomUrl;
            TableMapping.ColumnMappings.Add("CustomUrlId", "CustomUrlId");
            TableMapping.ColumnMappings.Add("OrganizationId", "OrganizationId");
            TableMapping.ColumnMappings.Add("InstanceId", "InstanceId");
            TableMapping.ColumnMappings.Add("FullCustomUrl", "FullCustomUrl");
            TableMapping.ColumnMappings.Add("PartialCustomUrl", "PartialCustomUrl");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertCustomUrl";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@CustomUrlId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "CustomUrlId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@FullCustomUrl", SqlDbType.NVarChar, 1024, ParameterDirection.Input, 0, 0, "FullCustomUrl", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@PartialCustomUrl", SqlDbType.NVarChar, 1024, ParameterDirection.Input, 0, 0, "PartialCustomUrl", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateCustomUrl";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@CustomUrlId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "CustomUrlId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@FullCustomUrl", SqlDbType.NVarChar, 1024, ParameterDirection.Input, 0, 0, "FullCustomUrl", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@PartialCustomUrl", SqlDbType.NVarChar, 1024, ParameterDirection.Input, 0, 0, "PartialCustomUrl", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region DeleteCommand

            DeleteCommand.CommandText = "dbo.Mc_DeleteCustomUrl";
            DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@CustomUrlId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "CustomUrlId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommands

            SelectCommand.CommandText = "dbo.Mc_GetCustomUrlsByOrganizationId";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetCustomUrl"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@CustomUrlId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "CustomUrlId", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetCustomUrls"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@FullCustomUrl", SqlDbType.NVarChar, 1024, ParameterDirection.Input, 0, 0, "FullCustomUrl", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@PartialCustomUrl", SqlDbType.NVarChar, 1024, ParameterDirection.Input, 0, 0, "PartialCustomUrl", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            #endregion
        }

        #endregion
    }
}
