using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_UsersGroups table2.
    /// </summary>
    internal class UsersGroupsTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the UsersGroupsTableAdapter class.
        /// </summary>
        public UsersGroupsTableAdapter()
        {
            #region TableMapping

            TableName = TableName.UsersGroups;
            TableMapping.ColumnMappings.Add("UserId", "UserId");
            TableMapping.ColumnMappings.Add("GroupId", "GroupId");

            #endregion

            #region DeleteCommand

            DeleteCommand.CommandText = "dbo.Mc_DeleteUserGroup";
            DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UserId", DataRowVersion.Original, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "GroupId", DataRowVersion.Original, false, null, "", "", ""));

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertUserGroup";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UserId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "GroupId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommand

            SelectCommand.CommandText = "dbo.Mc_GetUsersGroups";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UserId", DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));

            #endregion
        }

        #endregion
    }
}