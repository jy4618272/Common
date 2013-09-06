using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_DatabaseServer table2.
    /// </summary>
    internal class DatabaseServerTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DatabaseServerTableAdapter()
        {
            #region TableMapping

            TableName = TableName.DatabaseServer;
            TableMapping.ColumnMappings.Add("DatabaseServerId", "DatabaseServerId");
            TableMapping.ColumnMappings.Add("Name", "Name");
            TableMapping.ColumnMappings.Add("InstanceName", "InstanceName");
            TableMapping.ColumnMappings.Add("Port", "Port");
            TableMapping.ColumnMappings.Add("Description", "Description");
            TableMapping.ColumnMappings.Add("WebsiteId", "WebsiteId");
            TableMapping.ColumnMappings.Add("Deleted", "Deleted");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertDatabaseServer";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@DatabaseServerId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "DatabaseServerId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@InstanceName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "InstanceName", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Port", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "Port", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 1024, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@WebsiteId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "WebsiteId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Deleted", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Deleted", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateDatabaseServer";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@DatabaseServerId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "DatabaseServerId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@InstanceName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "InstanceName", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Port", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "Port", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 1024, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@WebsiteId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "WebsiteId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Deleted", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Deleted", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommand

            SelectCommand.CommandText = "dbo.Mc_GetDatabaseServers";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));

            #endregion
        }

        #endregion
    }
}