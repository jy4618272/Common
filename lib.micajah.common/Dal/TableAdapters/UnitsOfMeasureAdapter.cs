using System;
using System.Data;
using System.Data.SqlClient;
using Micajah.Common.Bll;

namespace Micajah.Common.Dal.TableAdapters
{
    internal class UnitsOfMeasureAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the UnitsOfMeasureAdapter class.
        /// </summary>
        public UnitsOfMeasureAdapter()
        {
            #region TableMapping

            TableName = TableName.UnitsOfMeasure;
            TableMapping.ColumnMappings.Add("UnitsOfMeasureId", "UnitsOfMeasureId");
            TableMapping.ColumnMappings.Add("OrganizationId", "OrganizationId");
            TableMapping.ColumnMappings.Add("SingularName", "SingularName");
            TableMapping.ColumnMappings.Add("SingularAbbrv", "SingularAbbrv");
            TableMapping.ColumnMappings.Add("PluralName", "PluralName");
            TableMapping.ColumnMappings.Add("PluralAbbrv", "PluralAbbrv");
            TableMapping.ColumnMappings.Add("GroupName", "GroupName");
            TableMapping.ColumnMappings.Add("LocalName", "LocalName");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertUnitsOfMeasure";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@UnitsOfMeasureId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UnitsOfMeasureId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@SingularName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "SingularName", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@SingularAbbrv", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "SingularAbbrv", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@PluralName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "PluralName", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@PluralAbbrv", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "PluralAbbrv", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@GroupName", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "GroupName", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@LocalName", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "LocalName", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateUnitsOfMeasure";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@UnitsOfMeasureId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UnitsOfMeasureId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@SingularName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "SingularName", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@SingularAbbrv", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "SingularAbbrv", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@PluralName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "PluralName", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@PluralAbbrv", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "PluralAbbrv", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@GroupName", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "GroupName", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@LocalName", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "LocalName", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region DeleteCommand

            DeleteCommand.CommandText = "dbo.Mc_DeleteUnitsOfMeasure";
            DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@UnitsOfMeasureId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UnitsOfMeasureId", DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommand

            SelectCommand.CommandText = "dbo.Mc_GetUnitsOfMeasure";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));

            #endregion
        }

        #endregion

        #region Public Methods

        public int OverrideUnit(Guid unitsOfMeasureId, Guid organizationId)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            try
            {
                connection = new SqlConnection(this.ConnectionString);

                command = new SqlCommand("dbo.Mc_UpdateUnitsOfMeasureOverride", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@UnitsOfMeasureId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RuleId", DataRowVersion.Current, false, unitsOfMeasureId, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, organizationId, "", "", ""));

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
