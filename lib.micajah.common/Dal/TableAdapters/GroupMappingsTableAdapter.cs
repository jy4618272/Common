using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_GroupMappings table2.
    /// </summary>
    internal class GroupMappingsTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the GroupMappingsTableAdapter class.
        /// </summary>
        public GroupMappingsTableAdapter()
        {
            #region TableMapping

            TableName = TableName.GroupMappings;
            TableMapping.ColumnMappings.Add("GroupId", "GroupId");
            TableMapping.ColumnMappings.Add("OrganizationId", "OrganizationId");
			TableMapping.ColumnMappings.Add("GroupName", "GroupName");
			TableMapping.ColumnMappings.Add("LdapDomainId", "LdapDomainId");
			TableMapping.ColumnMappings.Add("LdapDomainName", "LdapDomainName");
			TableMapping.ColumnMappings.Add("LdapGroupId", "LdapGroupId");
			TableMapping.ColumnMappings.Add("LdapGroupName", "LdapGroupName");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertGroupMappings";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "GroupId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
			InsertCommand.Parameters.Add(new SqlParameter("@GroupName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "GroupName", DataRowVersion.Current, false, null, "", "", ""));
			InsertCommand.Parameters.Add(new SqlParameter("@LdapDomainId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "LdapDomainId", DataRowVersion.Current, false, null, "", "", ""));
			InsertCommand.Parameters.Add(new SqlParameter("@LdapDomainName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LdapDomainName", DataRowVersion.Current, false, null, "", "", ""));
			InsertCommand.Parameters.Add(new SqlParameter("@LdapGroupId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "LdapGroupId", DataRowVersion.Current, false, null, "", "", ""));
			InsertCommand.Parameters.Add(new SqlParameter("@LdapGroupName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LdapGroupName", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

			#region DeleteCommand

			DeleteCommand.CommandText = "dbo.Mc_DeleteGroupMapping";
			DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
			DeleteCommand.Parameters.Add(new SqlParameter("@groupId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "GroupId", DataRowVersion.Current, false, null, "", "", ""));
			DeleteCommand.Parameters.Add(new SqlParameter("@ldapDomainId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "LdapDomainId", DataRowVersion.Current, false, null, "", "", ""));
			DeleteCommand.Parameters.Add(new SqlParameter("@ldapGroupId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "LdapGroupId", DataRowVersion.Current, false, null, "", "", ""));

			#endregion

            #region SelectCommand

            SelectCommand.CommandText = "dbo.Mc_GetGroupMappings";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion
        }

        #endregion
    }
}