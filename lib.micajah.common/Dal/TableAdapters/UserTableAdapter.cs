using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_User table.
    /// </summary>
    internal class UserTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the UserTableAdapter class.
        /// </summary>
        public UserTableAdapter()
        {
            #region TableMapping

            TableName = TableName.User;
            TableMapping.ColumnMappings.Add("UserId", "UserId");
            TableMapping.ColumnMappings.Add("Email", "Email");
            TableMapping.ColumnMappings.Add("FirstName", "FirstName");
            TableMapping.ColumnMappings.Add("LastName", "LastName");
            TableMapping.ColumnMappings.Add("MiddleName", "MiddleName");
            TableMapping.ColumnMappings.Add("Phone", "Phone");
            TableMapping.ColumnMappings.Add("MobilePhone", "MobilePhone");
            TableMapping.ColumnMappings.Add("Fax", "Fax");
            TableMapping.ColumnMappings.Add("Title", "Title");
            TableMapping.ColumnMappings.Add("Department", "Department");
            TableMapping.ColumnMappings.Add("Street", "Street");
            TableMapping.ColumnMappings.Add("Street2", "Street2");
            TableMapping.ColumnMappings.Add("City", "City");
            TableMapping.ColumnMappings.Add("State", "State");
            TableMapping.ColumnMappings.Add("PostalCode", "PostalCode");
            TableMapping.ColumnMappings.Add("Country", "Country");
            TableMapping.ColumnMappings.Add("LastLoginDate", "LastLoginDate");
            TableMapping.ColumnMappings.Add("Deleted", "Deleted");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertUser";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UserId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Email", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "FirstName", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LastName", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@MiddleName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "MiddleName", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NVarChar, 20, ParameterDirection.Input, 0, 0, "Phone", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@MobilePhone", SqlDbType.NVarChar, 20, ParameterDirection.Input, 0, 0, "MobilePhone", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Fax", SqlDbType.NVarChar, 20, ParameterDirection.Input, 0, 0, "Fax", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 30, ParameterDirection.Input, 0, 0, "Title", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Department", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Department", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Street", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Street", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Street2", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Street2", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@City", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "City", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@State", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "State", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@PostalCode", SqlDbType.NVarChar, 20, ParameterDirection.Input, 0, 0, "PostalCode", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Country", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Country", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@LastLoginDate", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "LastLoginDate", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Deleted", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Deleted", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateUser";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UserId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Email", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "FirstName", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LastName", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@MiddleName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "MiddleName", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NVarChar, 20, ParameterDirection.Input, 0, 0, "Phone", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@MobilePhone", SqlDbType.NVarChar, 20, ParameterDirection.Input, 0, 0, "MobilePhone", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Fax", SqlDbType.NVarChar, 20, ParameterDirection.Input, 0, 0, "Fax", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 30, ParameterDirection.Input, 0, 0, "Title", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Department", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Department", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Street", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Street", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Street2", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Street2", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@City", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "City", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@State", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "State", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@PostalCode", SqlDbType.NVarChar, 20, ParameterDirection.Input, 0, 0, "PostalCode", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Country", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Country", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@LastLoginDate", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "LastLoginDate", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Deleted", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Deleted", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommand

            SelectCommand.CommandText = "dbo.Mc_GetUsers";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetUser"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UserId", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetUserByEmail"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Email", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetAnotherAdministrator"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UserId", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetUsersByRoles"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationAdministrator", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "OrganizationAdministrator", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Active", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@InRoles", SqlDbType.VarChar, 1024, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@NotInRoles", SqlDbType.VarChar, 1024, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            #endregion
        }

        #endregion
    }
}