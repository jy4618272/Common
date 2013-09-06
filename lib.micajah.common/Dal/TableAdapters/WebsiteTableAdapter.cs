using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_Website table2.
    /// </summary>
    internal sealed class WebsiteTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WebsiteTableAdapter()
        {
            #region TableMapping

            TableName = TableName.Website;
            TableMapping.ColumnMappings.Add("WebsiteId", "WebsiteId");
            TableMapping.ColumnMappings.Add("Name", "Name");
            TableMapping.ColumnMappings.Add("Url", "Url");
            TableMapping.ColumnMappings.Add("Description", "Description");
            TableMapping.ColumnMappings.Add("AdminContactInfo", "AdminContactInfo");
            TableMapping.ColumnMappings.Add("Deleted", "Deleted");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertWebsite";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@WebsiteId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "WebsiteId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Url", SqlDbType.NVarChar, 2048, ParameterDirection.Input, 0, 0, "Url", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 1024, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@AdminContactInfo", SqlDbType.NVarChar, 2048, ParameterDirection.Input, 0, 0, "AdminContactInfo", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Deleted", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Deleted", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateWebsite";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@WebsiteId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "WebsiteId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Url", SqlDbType.NVarChar, 2048, ParameterDirection.Input, 0, 0, "Url", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 1024, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@AdminContactInfo", SqlDbType.NVarChar, 2048, ParameterDirection.Input, 0, 0, "AdminContactInfo", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Deleted", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Deleted", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommand

            SelectCommand.CommandText = "dbo.Mc_GetWebsites";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));

            #endregion
        }

        #endregion
    }
}

