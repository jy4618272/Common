using System;
using System.Data;
using System.Data.SqlClient;
using Micajah.Common.Bll;

namespace Micajah.Common.Dal.TableAdapters
{
    public class RuleTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RuleTableAdapter class.
        /// </summary>
        public RuleTableAdapter()
        {
            #region TableMapping

            TableName = TableName.Rule;
            TableMapping.ColumnMappings.Add("RuleId", "RuleId");
            TableMapping.ColumnMappings.Add("RuleEngineId", "RuleEngineId");
            TableMapping.ColumnMappings.Add("OrganizationId", "OrganizationId");
            TableMapping.ColumnMappings.Add("InstanceId", "InstanceId");
            TableMapping.ColumnMappings.Add("Name", "Name");
            TableMapping.ColumnMappings.Add("DisplayName", "DisplayName");
            TableMapping.ColumnMappings.Add("UsedQty", "UsedQty");
            TableMapping.ColumnMappings.Add("LastUsedUser", "LastUsedUser");
            TableMapping.ColumnMappings.Add("LastUsedDate", "LastUsedDate");
            TableMapping.ColumnMappings.Add("CreatedDate", "CreatedDate");
            TableMapping.ColumnMappings.Add("CreatedBy", "CreatedBy");
            TableMapping.ColumnMappings.Add("OrderNumber", "OrderNumber");
            TableMapping.ColumnMappings.Add("Active", "Active");
            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertRule";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@RuleId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RuleId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@RuleEngineId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RuleEngineId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@DisplayName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "DisplayName", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@UsedQty", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "UsedQty", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@LastUsedUser", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "LastUsedUser", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@LastUsedDate", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "LastUsedDate", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "CreatedBy", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "CreatedDate", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@OrderNumber", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "OrderNumber", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Active", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateRule";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@RuleId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RuleId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@RuleEngineId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RuleEngineId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@DisplayName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "DisplayName", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@UsedQty", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "UsedQty", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@LastUsedUser", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "LastUsedUser", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@LastUsedDate", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "LastUsedDate", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "CreatedBy", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "CreatedDate", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@OrderNumber", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "OrderNumber", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Active", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region DeleteCommand

            DeleteCommand.CommandText = "dbo.Mc_DeleteRule";
            DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@RuleId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RuleId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommand

            SelectCommand.CommandText = "dbo.Mc_GetAllRules";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@RuleEngineId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RuleEngineId", DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, false, null, "", "", ""));

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetRule"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@RuleId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RuleId", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetRuleByName"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            #endregion
        }

        #endregion

        #region Public Methods

        public int UpdateRuleUses(Guid ruleId, Guid lastUsedUser, DateTime lastUsedDate)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            try
            {
                connection = new SqlConnection(this.ConnectionString);

                command = new SqlCommand("dbo.Mc_UpdateRuleUses", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@RuleId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RuleId", DataRowVersion.Current, false, ruleId, "", "", ""));
                command.Parameters.Add(new SqlParameter("@LastUsedUser", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "LastUsedUser", DataRowVersion.Current, false, lastUsedUser, "", "", ""));
                command.Parameters.Add(new SqlParameter("@LastUsedDate", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "LastUsedDate", DataRowVersion.Current, false, lastUsedDate, "", "", ""));

                return Support.ExecuteNonQuery(command);
            }
            finally
            {
                if (connection != null) connection.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public int UpdateOrderNumber(Guid ruleId, int orderNumber)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            try
            {
                connection = new SqlConnection(this.ConnectionString);

                command = new SqlCommand("dbo.Mc_UpdateRuleOrder", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@RuleId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RuleId", DataRowVersion.Current, false, ruleId, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrderNumber", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "OrderNumber", DataRowVersion.Current, false, orderNumber, "", "", ""));

                return Support.ExecuteNonQuery(command);
            }
            finally
            {
                if (connection != null) connection.Dispose();
                if (command != null) command.Dispose();
            }
        }

        #endregion
    }
}
