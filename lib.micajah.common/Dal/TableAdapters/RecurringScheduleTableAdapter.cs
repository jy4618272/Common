using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    public class RecurringScheduleTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the UserTableAdapter class.
        /// </summary>
        public RecurringScheduleTableAdapter()
        {
            #region TableMapping

            TableName = TableName.RecurringSchedule;
            TableMapping.ColumnMappings.Add("RecurringScheduleId", "RecurringScheduleId");
            TableMapping.ColumnMappings.Add("OrganizationId", "OrganizationId");
            TableMapping.ColumnMappings.Add("InstanceId", "InstanceId");
            TableMapping.ColumnMappings.Add("LocalEntityType", "LocalEntityType");
            TableMapping.ColumnMappings.Add("LocalEntityId", "LocalEntityId");
            TableMapping.ColumnMappings.Add("Name", "Name");
            TableMapping.ColumnMappings.Add("StartDate", "StartDate");
            TableMapping.ColumnMappings.Add("EndDate", "EndDate");
            TableMapping.ColumnMappings.Add("RecurrenceRule", "RecurrenceRule");
            TableMapping.ColumnMappings.Add("UpdatedTime", "UpdatedTime");
            TableMapping.ColumnMappings.Add("UpdatedBy", "UpdatedBy");
            TableMapping.ColumnMappings.Add("Deleted", "Deleted");

            #endregion

            #region DeleteCommand

            DeleteCommand.CommandText = "dbo.Mc_DeleteRecurringSchedule";
            DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@RecurringScheduleId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RecurringScheduleId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_UpdateRecurringSchedule";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@RecurringScheduleId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RecurringScheduleId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@LocalEntityType", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "LocalEntityType", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@LocalEntityId", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LocalEntityId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "StartDate", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "EndDate", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@RecurrenceRule", SqlDbType.NVarChar, 1024, ParameterDirection.Input, 0, 0, "RecurrenceRule", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@UpdatedTime", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "UpdatedTime", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@UpdatedBy", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UpdatedBy", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Deleted", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Deleted", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateRecurringSchedule";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@RecurringScheduleId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RecurringScheduleId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@LocalEntityType", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "LocalEntityType", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@LocalEntityId", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LocalEntityId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "StartDate", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "EndDate", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@RecurrenceRule", SqlDbType.NVarChar, 1024, ParameterDirection.Input, 0, 0, "RecurrenceRule", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@UpdatedTime", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "UpdatedTime", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@UpdatedBy", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "UpdatedBy", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Deleted", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Deleted", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommand

            SelectCommand.CommandText = "dbo.Mc_GetRecurringSchedules";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, true, null, "", "", ""));

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetRecurringScheduleById"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@RecurringScheduleId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RecurringScheduleId", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetRecurringScheduleByEntityType"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, true, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@LocalEntityType", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "LocalEntityType", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetRecurringScheduleByEntityId"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, true, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@LocalEntityType", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "LocalEntityType", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@LocalEntityId", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LocalEntityId", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetRecurringScheduleByName"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, true, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetRecurringScheduleByRecurrenceRule"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, true, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@RecurrenceRule", SqlDbType.NVarChar, 1024, ParameterDirection.Input, 0, 0, "RecurrenceRule", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetRecurringScheduleEntityTypes"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, true, null, "", "", ""));
                SelectCommands.Add(command);
            }

            #endregion
        }

        #endregion
    }
}
