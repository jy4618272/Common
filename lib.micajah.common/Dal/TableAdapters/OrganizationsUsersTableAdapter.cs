using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_OrganizationsUsers table2.
    /// </summary>
    internal class OrganizationsUsersTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the OrganizationsUsersTableAdapter class.
        /// </summary>
        public OrganizationsUsersTableAdapter()
        {
            #region TableMapping

            TableName = TableName.OrganizationsUsers;
            TableMapping.DataSetTable = "OrganizationsUsers";
            TableMapping.ColumnMappings.Add("OrganizationId", "OrganizationId");
            TableMapping.ColumnMappings.Add("UserId", "UserId");
            TableMapping.ColumnMappings.Add("OrganizationAdministrator", "OrganizationAdministrator");
            TableMapping.ColumnMappings.Add("Active", "Active");

            #endregion

            #region DeleteCommand

            DeleteCommand.CommandText = "dbo.Mc_DeleteOrganizationUser";
            DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Original, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UserId", DataRowVersion.Original, false, null, "", "", ""));

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertOrganizationUser";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Original, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UserId", DataRowVersion.Original, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@OrganizationAdministrator", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "OrganizationAdministrator", DataRowVersion.Original, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Active", DataRowVersion.Original, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateOrganizationUser";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Original, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UserId", DataRowVersion.Original, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@OrganizationAdministrator", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "OrganizationAdministrator", DataRowVersion.Original, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Active", DataRowVersion.Original, false, null, "", "", ""));

            #endregion
        }

        #endregion
    }
}