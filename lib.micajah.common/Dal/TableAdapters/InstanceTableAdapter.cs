using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_Instance table2.
    /// </summary>
    internal class InstanceTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the InstanceTableAdapter class.
        /// </summary>
        public InstanceTableAdapter()
        {
            #region TableMapping

            TableName = TableName.Instance;
            TableMapping.ColumnMappings.Add("InstanceId", "InstanceId");
            TableMapping.ColumnMappings.Add("PseudoId", "PseudoId");
            TableMapping.ColumnMappings.Add("OrganizationId", "OrganizationId");
            TableMapping.ColumnMappings.Add("Name", "Name");
            TableMapping.ColumnMappings.Add("Description", "Description");
            TableMapping.ColumnMappings.Add("EnableSignUpUser", "EnableSignUpUser");
            TableMapping.ColumnMappings.Add("ExternalId", "ExternalId");
            TableMapping.ColumnMappings.Add("WorkingDays", "WorkingDays");
            TableMapping.ColumnMappings.Add("Active", "Active");
            TableMapping.ColumnMappings.Add("CanceledTime", "CanceledTime");
            TableMapping.ColumnMappings.Add("Trial", "Trial");
            TableMapping.ColumnMappings.Add("Beta", "Beta");
            TableMapping.ColumnMappings.Add("Deleted", "Deleted");
            TableMapping.ColumnMappings.Add("CreatedTime", "CreatedTime");
            TableMapping.ColumnMappings.Add("TimeZoneId", "TimeZoneId");
            TableMapping.ColumnMappings.Add("TimeFormat", "TimeFormat");
            TableMapping.ColumnMappings.Add("DateFormat", "DateFormat");
            TableMapping.ColumnMappings.Add("BillingPlan", "BillingPlan");
            TableMapping.ColumnMappings.Add("CreditCardStatus", "CreditCardStatus");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertInstance";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@PseudoId", SqlDbType.VarChar, 6, ParameterDirection.Input, 0, 0, "PseudoId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 1024, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@EnableSignUpUser", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "EnableSignUpUser", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@ExternalId", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "ExternalId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@WorkingDays", SqlDbType.Char, 7, ParameterDirection.Input, 0, 0, "WorkingDays", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Active", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@CanceledTime", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "CanceledTime", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Trial", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Trial", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Beta", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Beta", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Deleted", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Deleted", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@TimeZoneId", SqlDbType.NVarChar, 100, ParameterDirection.Input, 0, 0, "TimeZoneId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@TimeFormat", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "TimeFormat", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@DateFormat", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "DateFormat", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@BillingPlan", SqlDbType.TinyInt, 1, ParameterDirection.Input, 3, 0, "BillingPlan", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@CreditCardStatus", SqlDbType.TinyInt, 1, ParameterDirection.Input, 3, 0, "CreditCardStatus", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateInstance";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@PseudoId", SqlDbType.VarChar, 6, ParameterDirection.Input, 0, 0, "PseudoId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 1024, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@EnableSignUpUser", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "EnableSignUpUser", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@ExternalId", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "ExternalId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@WorkingDays", SqlDbType.Char, 7, ParameterDirection.Input, 0, 0, "WorkingDays", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Active", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@CanceledTime", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "CanceledTime", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Trial", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Trial", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Beta", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Beta", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Deleted", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Deleted", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@TimeZoneId", SqlDbType.NVarChar, 100, ParameterDirection.Input, 0, 0, "TimeZoneId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@TimeFormat", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "TimeFormat", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@DateFormat", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "DateFormat", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@BillingPlan", SqlDbType.TinyInt, 1, ParameterDirection.Input, 3, 0, "BillingPlan", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@CreditCardStatus", SqlDbType.TinyInt, 1, ParameterDirection.Input, 3, 0, "CreditCardStatus", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommand

            SelectCommand.CommandText = "dbo.Mc_GetInstances";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion
        }

        #endregion
    }
}

