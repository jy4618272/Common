using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_InvitedLogin table2.
    /// </summary>
    internal class InvitedLoginTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the InvitedLoginTableAdapter class.
        /// </summary>
        public InvitedLoginTableAdapter()
        {
            #region TableMapping

            TableName = TableName.InvitedLogin;
            TableMapping.ColumnMappings.Add("InvitedLoginId", "InvitedLoginId");
            TableMapping.ColumnMappings.Add("LoginName", "LoginName");
            TableMapping.ColumnMappings.Add("OrganizationId", "OrganizationId");
            TableMapping.ColumnMappings.Add("GroupId", "GroupId");
            TableMapping.ColumnMappings.Add("InvitedBy", "InvitedBy");
            TableMapping.ColumnMappings.Add("CreatedTime", "CreatedTime");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertInvitedLogin";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@InvitedLoginId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InvitedLoginId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@LoginName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LoginName", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.VarChar, 2056, ParameterDirection.Input, 0, 0, "GroupId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@InvitedBy", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InvitedBy", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@CreatedTime", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "CreatedTime", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommands

            SelectCommand.CommandText = "dbo.Mc_GetInvitedLogin";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@InvitedLoginId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InvitedLoginId", DataRowVersion.Current, false, null, "", "", ""));

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetInvitedLoginsByOrganizationId"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            #endregion

            #region DeleteCommand

            DeleteCommand.CommandText = "dbo.Mc_DeleteInvitedLogin";
            DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@InvitedLoginId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InvitedLoginId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion
        }

        #endregion
    }
}
