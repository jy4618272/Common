using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_OrganizationsLdapGroups table2.
    /// </summary>
    internal class OrganizationsLdapGroupsTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the OrganizationsLdapGroupsTableAdapter class.
        /// </summary>
        public OrganizationsLdapGroupsTableAdapter()
        {
            #region TableMapping

            TableName = TableName.OrganizationsLdapGroups;
            TableMapping.DataSetTable = "OrganizationsLdapGroups";
            TableMapping.ColumnMappings.Add("Id", "Id");
            TableMapping.ColumnMappings.Add("OrganizationId", "OrganizationId");
            TableMapping.ColumnMappings.Add("DomainId", "DomainId");
            TableMapping.ColumnMappings.Add("Domain", "Domain");
            TableMapping.ColumnMappings.Add("ObjectGUID", "ObjectGUID");
            TableMapping.ColumnMappings.Add("Name", "Name");
            TableMapping.ColumnMappings.Add("DistinguishedName", "DistinguishedName");
            TableMapping.ColumnMappings.Add("CreatedTime", "CreatedTime");

            #endregion

            #region DeleteCommand

            DeleteCommand.CommandText = "dbo.Mc_DeleteOrganizationLdapGroup";
            DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Original, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@Domain", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Domain", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertOrganizationLdapGroup";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "Id", DataRowVersion.Original, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Original, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@DomainId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "DomainId", DataRowVersion.Original, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Domain", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Domain", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@ObjectGUID", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "ObjectGUID", DataRowVersion.Original, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@DistinguishedName", SqlDbType.NVarChar, 2048, ParameterDirection.Input, 0, 0, "DistinguishedName", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@CreatedTime", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "CreatedTime", DataRowVersion.Current, false, null, "", "", ""));

            #endregion          

            #region SelectCommand

            //Returns all rows
            SelectCommand.CommandText = "dbo.Mc_GetOrganizationsLdapGroupsAll";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Original, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@Domain", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Domain", DataRowVersion.Current, false, null, "", "", ""));

            //Returns first 25 rows
            using (SqlCommand command = new SqlCommand("dbo.Mc_GetOrganizationsLdapGroups"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Original, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@Domain", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Domain", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            //Returns distinct domains
            using (SqlCommand command = new SqlCommand("dbo.Mc_GetOrganizationsLdapGroupsDomains"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Original, false, null, "", "", ""));
                SelectCommands.Add(command);
            }


            //Returns all groups
            using (SqlCommand command = new SqlCommand("dbo.Mc_GetOrganizationsLdapGroupsByOrganizationId"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Original, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            #endregion            
        }

        #endregion
    }
}