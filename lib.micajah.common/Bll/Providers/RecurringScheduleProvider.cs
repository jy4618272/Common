using Micajah.Common.Application;
using Micajah.Common.Dal;
using Micajah.Common.Dal.TableAdapters;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;

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

            ClientDataSet.RecurringScheduleRow row = GetRecurringSchedulesRow(recurringScheduleId, organizationId);
            if (row != null)
            {
                row.Deleted = true;

                ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
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
        public static void UpdateRecurringSchedule(Guid recurringScheduleId, Guid organizationId, Guid? instanceId, string localEntityType, string localEntityId
            , string name, DateTime startDate, DateTime endDate, string recurrenceRule, DateTime updatedTime, Guid updatedBy, bool deleted)
        {
            if (recurringScheduleId.Equals(Guid.Empty) || organizationId.Equals(Guid.Empty) || string.IsNullOrEmpty(name))
                throw new ArgumentNullException("recurringScheduleId", Properties.Resources.ExceptionMessage_ArgumentsIsEmpty);

            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.RecurringScheduleDataTable table = new ClientDataSet.RecurringScheduleDataTable();

            ClientDataSet.RecurringScheduleRow row = GetRecurringSchedulesRow(recurringScheduleId, organizationId);
            if (row == null)
                row = table.NewRecurringScheduleRow();

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

            if (row.RowState == DataRowState.Detached)
                table.AddRecurringScheduleRow(row);

            adapters.RecurringScheduleTableAdapter.Update(row);
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
        public static void InsertRecurringSchedule(Guid recurringScheduleId, Guid organizationId, Guid? instanceId, string localEntityType, string localEntityId
            , string name, DateTime startDate, DateTime endDate, string recurrenceRule, DateTime updatedTime, Guid updatedBy, bool deleted)
        {
            UpdateRecurringSchedule(recurringScheduleId, organizationId, instanceId, localEntityType, localEntityId, name, startDate, endDate, recurrenceRule, updatedTime, updatedBy, deleted);
        }

        /// <summary>
        /// Gets all recurring schedules in organization and/or instance
        /// </summary>
        /// <param name="organizationId">Specifies the organization identifier</param>
        /// <param name="instanceId">Specifies the instance identifier</param>
        /// <returns>The RecurringScheduleDataTable object pupulated with information of the recurring schedules.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.RecurringScheduleDataTable GetRecurringSchedules(Guid organizationId, Guid? instanceId)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.RecurringScheduleDataTable table = new ClientDataSet.RecurringScheduleDataTable();
            adapters.RecurringScheduleTableAdapter.Fill(table, 0, organizationId, instanceId);
            return table;
        }

        /// <summary>
        /// Gets recurring scheduler information for specified identifier
        /// </summary>
        /// <param name="recurringScheduleId">Specifies the recurring scheduler identifier</param>
        /// <returns>The RecurringScheduleRow object pupulated with information of the recurring schedules.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.RecurringScheduleRow GetRecurringSchedulesRow(Guid recurringScheduleId, Guid organizationId)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.RecurringScheduleDataTable table = new ClientDataSet.RecurringScheduleDataTable();
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
        public static ClientDataSet.RecurringScheduleDataTable GetRecurringSchedulesByEntityType(Guid organizationId, Guid? instanceId, string localEntityType)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.RecurringScheduleDataTable table = new ClientDataSet.RecurringScheduleDataTable();
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
        public static ClientDataSet.RecurringScheduleDataTable GetRecurringSchedulesByEntityId(Guid organizationId, Guid? instanceId, string localEntityType, string localEntityId)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.RecurringScheduleDataTable table = new ClientDataSet.RecurringScheduleDataTable();
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
        public static ClientDataSet.RecurringScheduleDataTable GetRecurringSchedulesByName(Guid organizationId, Guid? instanceId, string name)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.RecurringScheduleDataTable table = new ClientDataSet.RecurringScheduleDataTable();
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
        public static ClientDataSet.RecurringScheduleDataTable GetRecurringSchedulesByRecurrenceRule(Guid organizationId, Guid? instanceId, string definition)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.RecurringScheduleDataTable table = new ClientDataSet.RecurringScheduleDataTable();
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

            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
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
