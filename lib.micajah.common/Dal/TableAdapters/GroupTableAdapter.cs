using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_Group table2.
    /// </summary>
    internal class GroupTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the GroupTableAdapter class.
        /// </summary>
        public GroupTableAdapter()
        {
            #region TableMapping

            TableName = TableName.Group;
            TableMapping.ColumnMappings.Add("GroupId", "GroupId");
            TableMapping.ColumnMappings.Add("OrganizationId", "OrganizationId");
            TableMapping.ColumnMappings.Add("Name", "Name");
            TableMapping.ColumnMappings.Add("Description", "Description");
            TableMapping.ColumnMappings.Add("BuiltIn", "BuiltIn");
            TableMapping.ColumnMappings.Add("Deleted", "Deleted");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertGroup";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "GroupId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 1024, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@BuiltIn", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "BuiltIn", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Deleted", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Deleted", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateGroup";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "GroupId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 1024, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@BuiltIn", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "BuiltIn", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Deleted", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Deleted", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommand

            SelectCommand.CommandText = "dbo.Mc_GetGroups";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion
        }

        #endregion
    }
}