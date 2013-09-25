using Micajah.Common.Application;
using Micajah.Common.Dal;
using Micajah.Common.Dal.ClientDataSetTableAdapters;
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

                using (RecurringScheduleTableAdapter adapter = new RecurringScheduleTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                {
                    adapter.Update(row);
                }
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

            using (RecurringScheduleTableAdapter adapter = new RecurringScheduleTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                adapter.Update(row);
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
            using (RecurringScheduleTableAdapter adapter = new RecurringScheduleTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetRecurringSchedules(organizationId, instanceId);
            }
        }

        /// <summary>
        /// Gets recurring scheduler information for specified identifier
        /// </summary>
        /// <param name="recurringScheduleId">Specifies the recurring scheduler identifier</param>
        /// <returns>The RecurringScheduleRow object pupulated with information of the recurring schedules.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.RecurringScheduleRow GetRecurringSchedulesRow(Guid recurringScheduleId, Guid organizationId)
        {
            using (RecurringScheduleTableAdapter adapter = new RecurringScheduleTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                ClientDataSet.RecurringScheduleDataTable table = adapter.GetRecurringScheduleById(recurringScheduleId);
                return ((table.Count > 0) ? table[0] : null);
            }
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
            using (RecurringScheduleTableAdapter adapter = new RecurringScheduleTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetRecurringScheduleByEntityType(organizationId, instanceId, localEntityType);
            }
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
            using (RecurringScheduleTableAdapter adapter = new RecurringScheduleTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetRecurringScheduleByEntityId(organizationId, instanceId, localEntityType, localEntityId);
            }
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
            using (RecurringScheduleTableAdapter adapter = new RecurringScheduleTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetRecurringScheduleByName(organizationId, instanceId, name);
            }
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
            using (RecurringScheduleTableAdapter adapter = new RecurringScheduleTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetRecurringScheduleByRecurrenceRule(organizationId, instanceId, definition);
            }
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

            ClientDataSet.RecurringScheduleDataTable table = null;

            using (RecurringScheduleTableAdapter adapter = new RecurringScheduleTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                table = adapter.GetRecurringScheduleEntityTypes(organizationId, instanceId);
            }

            foreach (ClientDataSet.RecurringScheduleRow row in table)
            {
                result.Add(row.LocalEntityType);
            }

            return result;
        }

        #endregion
    }
}
