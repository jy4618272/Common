using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_ResetPasswordRequest table.
    /// </summary>
    internal class ResetPasswordRequestTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ResetPasswordRequestTableAdapter class.
        /// </summary>
        public ResetPasswordRequestTableAdapter()
        {
            #region TableMapping

            TableName = TableName.ResetPasswordRequest;
            TableMapping.ColumnMappings.Add("ResetPasswordRequestId", "ResetPasswordRequestId");
            TableMapping.ColumnMappings.Add("LoginId", "LoginId");
            TableMapping.ColumnMappings.Add("CreatedTime", "CreatedTime");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertResetPasswordRequest";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@ResetPasswordRequestId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "ResetPasswordRequestId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@LoginId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "LoginId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@CreatedTime", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "CreatedTime", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommands

            SelectCommand.CommandText = "dbo.Mc_GetResetPasswordRequest";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@ResetPasswordRequestId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "ResetPasswordRequestId", DataRowVersion.Current, false, null, "", "", ""));

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetResetPasswordRequestsByLoginId"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@LoginId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "LoginId", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            #endregion

            #region DeleteCommand

            DeleteCommand.CommandText = "dbo.Mc_DeleteResetPasswordRequest";
            DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@ResetPasswordRequestId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InvitedLoginId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion
        }

        #endregion
    }
}
