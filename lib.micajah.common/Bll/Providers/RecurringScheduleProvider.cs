using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using Micajah.Common.Application;
using Micajah.Common.Dal;
using Micajah.Common.Dal.TableAdapters;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with Recurring Schedules
    /// </summary>
    [DataObjectAttribute(true)]
    public static class RecurringScheduleProvider
    {
        #region Public Methods

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteRecurringSchedule(Guid recurringScheduleId, Guid organizationId)
        {
            if (recurringScheduleId.Equals(Guid.Empty))
                throw new ArgumentNullException("recurringScheduleId", Properties.Resources.ExceptionMessage_ArgumentsIsEmpty);

            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            OrganizationDataSet.RecurringScheduleDataTable table = new OrganizationDataSet.RecurringScheduleDataTable();
            adapters.RecurringScheduleTableAdapter.Fill(table, 1, recurringScheduleId);
            if (table.Count > 0)
            {
                OrganizationDataSet.RecurringScheduleRow row = table[0];
                row.Deleted = true;
                adapters.RecurringScheduleTableAdapter.Update(row);
            }
        }

        /// <summary>
        /// Updates the Recurring Scheduler of specified Id.
        /// </summary>
        /// <param name="recurringScheduleId">Specifies the recurring scheduler identifier</param>
        /// <param name="organizationId">Specifies the organization identifier</param>
        /// <param name="instanceId">Specifies the instance identifier</param>
        /// <param name="localEntityType">Specifies the entity type as string</param>
        /// <param name="localEntityId">Specifies the entity object identifier as string</param>
        /// <param name="name">Specifies the name</param>
        /// <param name="definition">Specifies the recurring scheduler definition.</param>
        /// <param name="updatedTime">Specifies the update time</param>
        /// <param name="updatedBy">Specifies who updating</param>
        /// <param name="deleted">Specifies the delete mark value</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateRecurringSchedule(
            Guid recurringScheduleId,
            Guid organizationId,
            Guid? instanceId,
            string localEntityType,
            string localEntityId,
            string name,
            DateTime startDate,
            DateTime endDate,
            string recurrenceRule,
            DateTime updatedTime,
            Guid updatedBy,
            bool deleted)
        {
            if (recurringScheduleId.Equals(Guid.Empty)
                || organizationId.Equals(Guid.Empty)
                || string.IsNullOrEmpty(name))
                throw new ArgumentNullException("recurringScheduleId", Properties.Resources.ExceptionMessage_ArgumentsIsEmpty);
            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);

            OrganizationDataSet.RecurringScheduleRow row = ds.RecurringSchedule.FindByRecurringScheduleId(recurringScheduleId);
            if (row == null) row = ds.RecurringSchedule.NewRecurringScheduleRow();
            row.RecurringScheduleId = recurringScheduleId;
            row.OrganizationId = organizationId;
            if (instanceId.HasValue)
                row.InstanceId = instanceId.Value;
            else
                row.SetInstanceIdNull();
            row.LocalEntityType = localEntityType;
            row.LocalEntityId = localEntityId;
            row.Name = name;
            row.StartDate = startDate;
            row.EndDate = endDate;
            row.RecurrenceRule = recurrenceRule;
            row.UpdatedTime = updatedTime;
            row.UpdatedBy = updatedBy;
            row.Deleted = deleted;
            if (row.RowState == System.Data.DataRowState.Detached)
                ds.RecurringSchedule.AddRecurringScheduleRow(row);
            adapters.RecurringScheduleTableAdapter.Update(ds);
        }

        /// <summary>
        /// Updates the Recurring Scheduler of specified Id.
        /// </summary>
        /// <param name="recurringScheduleId">Specifies the recurring scheduler identifier</param>
        /// <param name="organizationId">Specifies the organization identifier</param>
        /// <param name="instanceId">Specifies the instance identifier</param>
        /// <param name="localEntityType">Specifies the entity type as string</param>
        /// <param name="localEntityId">Specifies the entity object identifier as string</param>
        /// <param name="name">Specifies the name</param>
        /// <param name="definition">Specifies the recurring scheduler definition.</param>
        /// <param name="updatedTime">Specifies the update time</param>
        /// <param name="updatedBy">Specifies who updating</param>
        /// <param name="deleted">Specifies the delete mark value</param>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void InsertRecurringSchedule(Guid recurringScheduleId,
            Guid organizationId,
            Guid? instanceId,
            string localEntityType,
            string localEntityId,
            string name,
            DateTime startDate,
            DateTime endDate,
            string recurrenceRule,
            DateTime updatedTime,
            Guid updatedBy,
            bool deleted)
        {
            UpdateRecurringSchedule(recurringScheduleId, organizationId, instanceId,
                localEntityType, localEntityId, name, startDate, endDate, recurrenceRule,
                updatedTime, updatedBy, deleted);
        }

        /// <summary>
        /// Gets all recurring schedules in organization and/or instance
        /// </summary>
        /// <param name="organizationId">Specifies the organization identifier</param>
        /// <param name="instanceId">Specifies the instance identifier</param>
        /// <returns>The RecurringScheduleDataTable object pupulated with information of the recurring schedules.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static OrganizationDataSet.RecurringScheduleDataTable GetRecurringSchedules(Guid organizationId, Guid? instanceId)
        {
            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            adapters.RecurringScheduleTableAdapter.SelectCommand.Parameters["@OrganizationId"].Value = organizationId;
            if (instanceId.HasValue) adapters.RecurringScheduleTableAdapter.SelectCommand.Parameters["@InstanceId"].Value = instanceId.Value;
            else adapters.RecurringScheduleTableAdapter.SelectCommand.Parameters["@InstanceId"].Value = DBNull.Value;
            adapters.RecurringScheduleTableAdapter.Fill(ds.RecurringSchedule);
            return ds.RecurringSchedule;
        }

        /// <summary>
        /// Gets recurring scheduler information for specified identifier
        /// </summary>
        /// <param name="recurringScheduleId">Specifies the recurring scheduler identifier</param>
        /// <returns>The RecurringScheduleRow object pupulated with information of the recurring schedules.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static OrganizationDataSet.RecurringScheduleRow GetRecurringSchedulesRow(Guid recurringScheduleId, Guid organizationId)
        {
            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            OrganizationDataSet.RecurringScheduleDataTable table = new OrganizationDataSet.RecurringScheduleDataTable();
            adapters.RecurringScheduleTableAdapter.Fill(table, 1, recurringScheduleId);
            return ((table.Count > 0) ? table[0] : null);
        }

        /// <summary>
        /// Gets the recurring schedules in organization and/or instance by entity type
        /// </summary>
        /// <param name="organizationId">Specifies the organization identifier</param>
        /// <param name="instanceId">Specifies the instance identifier</param>
        /// <param name="localEntityType">Specifies the Entity Type string</param>
        /// <returns>The RecurringScheduleDataTable object pupulated with information of the recurring schedules.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static OrganizationDataSet.RecurringScheduleDataTable GetRecurringSchedulesByEntityType(Guid organizationId, Guid? instanceId, string localEntityType)
        {
            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            OrganizationDataSet.RecurringScheduleDataTable table = new OrganizationDataSet.RecurringScheduleDataTable();
            adapters.RecurringScheduleTableAdapter.Fill(table, 2, organizationId, instanceId, localEntityType);
            return table;
        }

        /// <summary>
        /// Gets the recurring schedules in organization and/or instance by entity identifier
        /// </summary>
        /// <param name="organizationId">Specifies the organization identifier</param>
        /// <param name="instanceId">Specifies the instance identifier</param>
        /// <param name="localEntityType">Specifies the Entity Type string</param>
        /// <param name="localEntityId">Specifies the Entity object identifier</param>
        /// <returns>The RecurringScheduleDataTable object pupulated with information of the recurring schedules.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static OrganizationDataSet.RecurringScheduleDataTable GetRecurringSchedulesByEntityId(Guid organizationId, Guid? instanceId, string localEntityType, string localEntityId)
        {
            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            OrganizationDataSet.RecurringScheduleDataTable table = new OrganizationDataSet.RecurringScheduleDataTable();
            adapters.RecurringScheduleTableAdapter.Fill(table, 3, organizationId, instanceId, localEntityType, localEntityId);
            return table;
        }

        /// <summary>
        /// Gets the recurring schedules in organization and/or instance by name
        /// </summary>
        /// <param name="organizationId">Specifies the organization identifier</param>
        /// <param name="instanceId">Specifies the instance identifier</param>
        /// <param name="name">Specifies the recurring schedule name</param>
        /// <returns>The RecurringScheduleDataTable object pupulated with information of the recurring schedules.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static OrganizationDataSet.RecurringScheduleDataTable GetRecurringSchedulesByName(Guid organizationId, Guid? instanceId, string name)
        {
            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            OrganizationDataSet.RecurringScheduleDataTable table = new OrganizationDataSet.RecurringScheduleDataTable();
            adapters.RecurringScheduleTableAdapter.Fill(table, 4, organizationId, instanceId, name);
            return table;
        }

        /// <summary>
        /// Gets the recurring schedules in organization and/or instance by definition
        /// </summary>
        /// <param name="organizationId">Specifies the organization identifier</param>
        /// <param name="instanceId">Specifies the instance identifier</param>
        /// <param name="name">Specifies the recurring schedule definition</param>
        /// <returns>The RecurringScheduleDataTable object pupulated with information of the recurring schedules.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static OrganizationDataSet.RecurringScheduleDataTable GetRecurringSchedulesByRecurrenceRule(Guid organizationId, Guid? instanceId, string definition)
        {
            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            OrganizationDataSet.RecurringScheduleDataTable table = new OrganizationDataSet.RecurringScheduleDataTable();
            adapters.RecurringScheduleTableAdapter.Fill(table, 5, organizationId, instanceId, definition);
            return table;
        }

        /// <summary>
        /// Gets the recurring schedules in organization and/or instance by definition
        /// </summary>
        /// <param name="organizationId">Specifies the organization identifier</param>
        /// <param name="instanceId">Specifies the instance identifier</param>
        /// <param name="name">Specifies the recurring schedule definition</param>
        /// <returns>The RecurringScheduleDataTable object pupulated with information of the recurring schedules.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static Collection<string> GetEntityTypes(Guid organizationId, Guid? instanceId)
        {
            Collection<string> result = new Collection<string>();

            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            DataTable table = new DataTable();
            table.Locale = CultureInfo.CurrentCulture;
            adapters.RecurringScheduleTableAdapter.Fill(table, 6, organizationId, instanceId);

            foreach (DataRow row in table.Rows)
            {
                result.Add((string)row[0]);
            }

            return result;
        }

        #endregion
    }
}
