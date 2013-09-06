using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_Email table2.
    /// </summary>
    public class EmailTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the EmailTableAdapter class.
        /// </summary>
        public EmailTableAdapter()
        {
            #region TableMapping

            TableName = TableName.Email;
            TableMapping.ColumnMappings.Add("Email", "Email");
            TableMapping.ColumnMappings.Add("LoginId", "LoginId");            

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertEmail";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Email", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@LoginId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "LoginId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion           

            #region DeleteCommand

            DeleteCommand.CommandText = "dbo.Mc_DeleteEmail";
            DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@LoginId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "LoginId", DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Email", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommands

            SelectCommand.CommandText = "dbo.Mc_GetEmail";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Email", DataRowVersion.Current, false, null, "", "", ""));

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetEmails"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@LoginId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "LoginId", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }


            #endregion
        }
        #endregion
    }
}
