using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    public class RuleParametersTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RuleParametersTableAdapter class.
        /// </summary>
        public RuleParametersTableAdapter()
        {
            #region TableMapping

            TableName = TableName.RuleParameters;
            TableMapping.ColumnMappings.Add("RuleParameterId", "RuleParameterId");
            TableMapping.ColumnMappings.Add("RuleId", "RuleId");
            TableMapping.ColumnMappings.Add("EntityNodeTypeId", "EntityNodeTypeId");
            TableMapping.ColumnMappings.Add("IsInputParameter", "IsInputParameter");
            TableMapping.ColumnMappings.Add("IsEntity", "IsEntity");
            TableMapping.ColumnMappings.Add("FieldName", "FieldName");
            TableMapping.ColumnMappings.Add("FullName", "FullName");
            TableMapping.ColumnMappings.Add("TypeName", "TypeName");
            TableMapping.ColumnMappings.Add("Term", "Term");
            TableMapping.ColumnMappings.Add("Value", "Value");
            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertRuleParameter";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@RuleParameterId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RuleParameterId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@RuleId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RuleId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@EntityNodeTypeId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityNodeTypeId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@IsInputParameter", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "IsInputParameter", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@IsEntity", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "IsEntity", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@FieldName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "FieldName", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@FullName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "FullName", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@TypeName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "TypeName", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Term", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "Term", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Value", SqlDbType.Variant, 0, ParameterDirection.Input, 0, 0, "Value", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateRuleParameter";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@RuleParameterId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RuleParameterId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@RuleId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RuleId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@EntityNodeTypeId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityNodeTypeId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@IsInputParameter", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "IsInputParameter", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@IsEntity", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "IsEntity", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@FieldName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "FieldName", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@FullName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "FullName", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@TypeName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "TypeName", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Term", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "Term", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Value", SqlDbType.Variant, 0, ParameterDirection.Input, 0, 0, "Value", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region DeleteCommand

            DeleteCommand.CommandText = "dbo.Mc_DeleteRuleParameter";
            DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@RuleParameterId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RuleParameterId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommand

            SelectCommand.CommandText = "dbo.Mc_GetAllRuleParameters";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@RuleId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RuleId", DataRowVersion.Current, false, null, "", "", ""));

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetRuleParameter"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@RuleParameterId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RuleParameterId", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            #endregion
        }

        #endregion
    }
}
