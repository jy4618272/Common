using System;
using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    internal class UnitsOfMeasureConversionAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the UnitsOfMeasureAdapter class.
        /// </summary>
        public UnitsOfMeasureConversionAdapter()
        {
            #region TableMapping

            TableName = TableName.UnitsOfMeasureConversion;
            TableMapping.ColumnMappings.Add("UnitOfMeasureFrom", "UnitOfMeasureFrom");
            TableMapping.ColumnMappings.Add("UnitOfMeasureTo", "UnitOfMeasureTo");
            TableMapping.ColumnMappings.Add("OrganizationId", "OrganizationId");
            TableMapping.ColumnMappings.Add("Factor", "Factor");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertUnitsOfMeasureConversion";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@UnitOfMeasureFrom", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UnitOfMeasureFrom", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@UnitOfMeasureTo", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UnitOfMeasureTo", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Factor", SqlDbType.Float, 8, ParameterDirection.Input, 0, 0, "Factor", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateUnitsOfMeasureConversion";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@UnitOfMeasureFrom", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UnitOfMeasureFrom", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@UnitOfMeasureTo", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UnitOfMeasureTo", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Factor", SqlDbType.Float, 8, ParameterDirection.Input, 0, 0, "Factor", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region DeleteCommand

            DeleteCommand.CommandText = "dbo.Mc_DeleteUnitsOfMeasureConversion";
            DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@UnitOfMeasureFrom", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UnitOfMeasureFrom", DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@UnitOfMeasureTo", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UnitOfMeasureTo", DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommands

            SelectCommand.CommandText = "dbo.Mc_GetUnitsOfMeasureConversion";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@UnitOfMeasureFrom", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UnitOfMeasureFrom", DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@UnitOfMeasureTo", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UnitOfMeasureTo", DataRowVersion.Current, false, null, "", "", ""));

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetUnitOfMeasureConversionByOrganizationId"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@UnitOfMeasureFrom", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UnitOfMeasureFrom", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@UnitOfMeasureTo", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UnitOfMeasureTo", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetUnitOfMeasureConversionFromByOrganizationId"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@UnitOfMeasureFrom", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UnitOfMeasureFrom", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            #endregion
        }

        #endregion
    }
}
