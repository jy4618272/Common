using Micajah.Common.Application;
using Micajah.Common.Dal;
using Micajah.Common.Dal.TableAdapters;
using Micajah.Common.Security;
using System;
using System.ComponentModel;
using System.Data;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with Rules Engine
    /// </summary>
    [DataObjectAttribute(true)]
    public static class RuleEngineProvider
    {
        #region Rule Methods

        /// <summary>
        /// Deletes the rule .
        /// </summary>
        /// <param name="ruleId">Specifies the rule identifier</param>
        /// <param name="organizationId">Specifies the organization identifier</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteRule(Guid ruleId, Guid organizationId)
        {
            if (ruleId.Equals(Guid.Empty))
                throw new ArgumentNullException("ruleId", Properties.Resources.ExceptionMessage_ArgumentsIsEmpty);

            ClientDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            adapters.RuleTableAdapter.Delete(ruleId);
        }

        /// <summary>
        /// Deletes the rule .
        /// </summary>
        /// <param name="ruleId">Specifies the rule identifier</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteRule(Guid ruleId)
        {
            DeleteRule(ruleId, UserContext.Current.SelectedOrganizationId);
        }

        /// <summary>
        /// Updates the rule
        /// </summary>
        /// <param name="ruleId">Specifies the rule identifier</param>
        /// <param name="organizationId">Specifies the organization identifier</param>
        /// <param name="instanceId">Specifies the instance identifier</param>
        /// <param name="name">Specifies the rule name</param>
        /// <param name="displayName">Specifies the rule display name</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateRule(Guid ruleId, Guid ruleEngineId, Guid organizationId, Guid? instanceId, string name, string displayName, int orderNumber, bool active)
        {
            if (ruleId.Equals(Guid.Empty) || ruleEngineId.Equals(Guid.Empty) || string.IsNullOrEmpty(name))
                throw new ArgumentNullException("ruleId", Properties.Resources.ExceptionMessage_ArgumentsIsEmpty);

            ClientDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.RuleDataTable table = new ClientDataSet.RuleDataTable();
            adapters.RuleTableAdapter.Fill(table, 1, ruleId);

            ClientDataSet.RuleRow row = ((table.Count > 0) ? table[0] : null);

            if (row == null)
            {
                row = table.NewRuleRow();
                row.RuleId = ruleId;
                row.RuleEngineId = ruleEngineId;
                row.OrganizationId = organizationId;
                if (instanceId.HasValue)
                    row.InstanceId = instanceId.Value;
                else
                    row.SetInstanceIdNull();
                row.UsedQty = 0;
                row.SetLastUsedUserNull();
                row.SetLastUsedDateNull();
                row.CreatedBy = UserContext.Current != null ? UserContext.Current.UserId : Guid.Empty;
                row.CreatedDate = DateTime.UtcNow;
            }

            row.Name = name;
            row.DisplayName = displayName;
            row.Active = active;
            row.OrderNumber = orderNumber;

            if (row.RowState == DataRowState.Detached)
                table.AddRuleRow(row);
            adapters.RuleTableAdapter.Update(row);
        }

        /// <summary>
        /// Increment the rule used quantity
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="ruleId"></param>
        /// <param name="lastUsedUser"></param>
        /// <param name="lastUsedDate"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static int UpdateRuleUses(Guid organizationId, Guid ruleId, Guid lastUsedUser, DateTime lastUsedDate)
        {
            if (ruleId.Equals(Guid.Empty)
                || organizationId.Equals(Guid.Empty))
                throw new ArgumentNullException("ruleId", Properties.Resources.ExceptionMessage_ArgumentsIsEmpty);

            ClientDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            return adapters.RuleTableAdapter.UpdateRuleUses(ruleId, lastUsedUser, lastUsedDate);
        }

        /// <summary>
        /// Updates the order number in rule prarmeters list 
        /// </summary>
        /// <param name="organizationId">Specifies the organization identifier</param>
        /// <param name="ruleParameterId">Specifies the rule paramenter identifier</param>
        /// <param name="order">Specifies the order of parameter in rule</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static int UpdateOrderNumber(Guid organizationId, Guid ruleId, int orderNumber)
        {
            if (organizationId.Equals(Guid.Empty)
                || ruleId.Equals(Guid.Empty))
                throw new ArgumentNullException("ruleId", Properties.Resources.ExceptionMessage_ArgumentsIsEmpty);

            ClientDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            return adapters.RuleTableAdapter.UpdateOrderNumber(ruleId, orderNumber);
        }

        /// <summary>
        /// Updates the order number in rule prarmeters list 
        /// </summary>
        /// <param name="ruleParameterId">Specifies the rule paramenter identifier</param>
        /// <param name="order">Specifies the order of parameter in rule</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static int UpdateOrderNumber(Guid ruleId, int orderNumber)
        {
            return UpdateOrderNumber(UserContext.Current.SelectedOrganizationId, ruleId, orderNumber);
        }

        /// <summary>
        /// Inserts the rule.
        /// </summary>
        /// <param name="ruleId">Specifies the rule identifier.</param>
        /// <param name="organizationId">Specifies the organization identifier.</param>
        /// <param name="instanceId">Specifies the instance identifier.</param>
        /// <param name="name">Specifies the rule name.</param>
        /// <param name="displayName">Specifies the rule display name.</param>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void InsertRule(Guid ruleEngineId, Guid organizationId, Guid? instanceId, string name, string displayName, int orderNumber, bool active)
        {
            UpdateRule(Guid.NewGuid(), ruleEngineId, organizationId, instanceId, name, displayName, orderNumber, active);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static RulesEngineCollection GetRulesEngines()
        {
            WebApplication.RefreshRulesEngines();
            return WebApplication.RulesEngines;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static RulesEngine GetRulesEngine(Guid ruleEngineId)
        {
            return WebApplication.RulesEngines[ruleEngineId.ToString()];
        }

        /// <summary>
        /// Gets all rules in organization and/or instance
        /// </summary>
        /// <param name="organizationId">Specifies the organization identifier</param>
        /// <param name="instanceId">Specifies the instance identifier</param>
        /// <returns>The RuleDataTable object pupulated with information of the rules.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.RuleDataTable GetRules(Guid ruleEngineId, Guid organizationId, Guid? instanceId)
        {
            ClientDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.RuleDataTable table = new ClientDataSet.RuleDataTable();
            adapters.RuleTableAdapter.Fill(table, 0, ruleEngineId, organizationId, instanceId);
            return table;
        }

        /// <summary>
        /// Gets rule information for specified identifier
        /// </summary>
        /// <param name="ruleId">Specifies the recurring scheduler identifier</param>
        /// <param name="organizationId">Specifies the organization identifier</param>
        /// <returns>The RuleRow object pupulated with information of the rule.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.RuleRow GetRuleRow(Guid ruleId, Guid organizationId)
        {
            ClientDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.RuleDataTable table = new ClientDataSet.RuleDataTable();
            adapters.RuleTableAdapter.Fill(table, 1, ruleId);
            return ((table.Count > 0) ? table[0] : null);
        }

        /// <summary>
        /// Gets rule information for specified identifier
        /// </summary>
        /// <param name="ruleName">Specifies the rule name</param>
        /// <param name="organizationId">Specifies the organization identifier</param>
        /// <returns>The RuleRow object pupulated with information of the rule.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.RuleRow GetRuleRow(string ruleName, Guid organizationId)
        {
            ClientDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.RuleDataTable table = new ClientDataSet.RuleDataTable();
            adapters.RuleTableAdapter.Fill(table, 2, ruleName);
            return ((table.Count > 0) ? table[0] : null);
        }

        /// <summary>
        /// Gets rule information for specified identifier
        /// </summary>
        /// <param name="ruleName">Specifies the rule name</param>
        /// <returns>The RuleRow object pupulated with information of the rule.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.RuleRow GetRuleRow(string ruleName)
        {
            return GetRuleRow(ruleName, UserContext.Current.SelectedOrganizationId);
        }

        /// <summary>
        /// Gets rule information for specified identifier
        /// </summary>
        /// <param name="ruleId">Specifies the recurring scheduler identifier</param>
        /// <returns>The RuleRow object pupulated with information of the rule.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.RuleRow GetRuleRow(Guid ruleId)
        {
            return GetRuleRow(ruleId, UserContext.Current.SelectedOrganizationId);
        }

        #endregion

        #region Rule Parameters Methods

        /// <summary>
        /// Deletes the rule .
        /// </summary>
        /// <param name="ruleParameterId">Specifies the rule parameter identifier</param>
        /// <param name="organizationId">Specifies the organization identifier</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteRuleParameter(Guid organizationId, Guid ruleParameterId)
        {
            if (ruleParameterId.Equals(Guid.Empty))
                throw new ArgumentNullException("ruleParameterId", Properties.Resources.ExceptionMessage_ArgumentsIsEmpty);

            ClientDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            adapters.RuleParametersTableAdapter.Delete(ruleParameterId);
        }

        /// <summary>
        /// Deletes the rule .
        /// </summary>
        /// <param name="ruleParameterId">Specifies the rule parameter identifier</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteRuleParameter(Guid ruleParameterId)
        {
            DeleteRuleParameter(UserContext.Current.SelectedOrganizationId, ruleParameterId);
        }

        /// <summary>
        /// Updates the rule parameter
        /// </summary>
        /// <param name="organizationId">Specifies the organization identifier</param>
        /// <param name="ruleParameterId">Specifies the rule paramenter identifier</param>
        /// <param name="ruleId">Specifies the rule identifier</param>
        /// <param name="entityNodeId">Specifies the entity object identifier</param>
        /// <param name="isInputParameter">Specifies the input or outpur parameter</param>
        /// <param name="isEntity">Specifies the entity or not value</param>
        /// <param name="fieldName">Specifies the entity field name</param>
        /// <param name="fullName">Specifies the full name for entity and his field or another identifier for typed object</param>
        /// <param name="typeName">Specifies the type of object value</param>
        /// <param name="order">Specifies the order of parameter in rule</param>
        /// <param name="term">Specifies the term of comparing</param>
        /// <param name="value">Specifies the values of comparing</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateRuleParameter(Guid organizationId, Guid ruleParameterId, Guid ruleId,
            Guid entityNodeTypeId, bool isInputParameter, bool isEntity, string fieldName, string fullName
            , string typeName, string term, object value)
        {
            if (organizationId.Equals(Guid.Empty) || ruleParameterId.Equals(Guid.Empty) || value == null)
                throw new ArgumentNullException("ruleParameterId", Properties.Resources.ExceptionMessage_ArgumentsIsEmpty);

            ClientDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.RuleParametersDataTable table = new ClientDataSet.RuleParametersDataTable();
            adapters.RuleParametersTableAdapter.Fill(table, 1, ruleParameterId);

            ClientDataSet.RuleParametersRow row = ((table.Count > 0) ? table[0] : null);

            if (row == null)
                row = table.NewRuleParametersRow();

            row.RuleParameterId = ruleParameterId;
            row.RuleId = ruleId;
            row.EntityNodeTypeId = entityNodeTypeId;
            row.IsInputParameter = isInputParameter;
            row.IsEntity = isEntity;
            row.FieldName = fieldName;
            row.FullName = fullName;
            row.TypeName = typeName;
            row.Term = term;
            row.Value = value;

            if (row.RowState == DataRowState.Detached)
                table.AddRuleParametersRow(row);

            adapters.RuleParametersTableAdapter.Update(row);
        }

        /// <summary>
        /// Updates the rule parameter
        /// </summary>
        /// <param name="ruleParameterId">Specifies the rule paramenter identifier</param>
        /// <param name="ruleId">Specifies the rule identifier</param>
        /// <param name="entityNodeId">Specifies the entity object identifier</param>
        /// <param name="isInputParameter">Specifies the input or outpur parameter</param>
        /// <param name="isEntity">Specifies the entity or not value</param>
        /// <param name="fieldName">Specifies the entity field name</param>
        /// <param name="fullName">Specifies the full name for entity and his field or another identifier for typed object</param>
        /// <param name="typeName">Specifies the type of object value</param>
        /// <param name="order">Specifies the order of parameter in rule</param>
        /// <param name="term">Specifies the term of comparing</param>
        /// <param name="value">Specifies the values of comparing</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateRuleParameter(Guid ruleParameterId, Guid ruleId, Guid entityNodeTypeId,
            bool isInputParameter, bool isEntity, string fieldName, string fullName, string typeName
            , string term, object value)
        {
            UpdateRuleParameter(UserContext.Current.SelectedOrganizationId, ruleParameterId,
                ruleId, entityNodeTypeId, isInputParameter, isEntity, fieldName, fullName, typeName, term, value);
        }

        /// <summary>
        /// Insterts the rule parameter
        /// </summary>
        /// <param name="organizationId">Specifies the organization identifier</param>
        /// <param name="ruleParameterId">Specifies the rule paramenter identifier</param>
        /// <param name="ruleId">Specifies the rule identifier</param>
        /// <param name="entityNodeId">Specifies the entity object identifier</param>
        /// <param name="isInputParameter">Specifies the input or outpur parameter</param>
        /// <param name="isEntity">Specifies the entity or not value</param>
        /// <param name="fieldName">Specifies the entity field name</param>
        /// <param name="fullName">Specifies the full name for entity and his field or another identifier for typed object</param>
        /// <param name="typeName">Specifies the type of object value</param>
        /// <param name="order">Specifies the order of parameter in rule</param>
        /// <param name="term">Specifies the term of comparing</param>
        /// <param name="value">Specifies the values of comparing</param>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void InsertRuleParameter(Guid organizationId, Guid ruleParameterId, Guid ruleId,
            Guid entityNodeTypeId, bool isInputParameter, bool isEntity, string fieldName, string fullName
            , string typeName, string term, object value)
        {
            UpdateRuleParameter(organizationId, ruleParameterId, ruleId, entityNodeTypeId, isInputParameter,
                isEntity, fieldName, fullName, typeName, term, value);
        }

        /// <summary>
        /// Insterts the rule parameter
        /// </summary>
        /// <param name="ruleParameterId">Specifies the rule paramenter identifier</param>
        /// <param name="ruleId">Specifies the rule identifier</param>
        /// <param name="entityNodeId">Specifies the entity object identifier</param>
        /// <param name="isInputParameter">Specifies the input or outpur parameter</param>
        /// <param name="isEntity">Specifies the entity or not value</param>
        /// <param name="fieldName">Specifies the entity field name</param>
        /// <param name="fullName">Specifies the full name for entity and his field or another identifier for typed object</param>
        /// <param name="typeName">Specifies the type of object value</param>
        /// <param name="order">Specifies the order of parameter in rule</param>
        /// <param name="term">Specifies the term of comparing</param>
        /// <param name="value">Specifies the values of comparing</param>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void InsertRuleParameter(Guid ruleParameterId, Guid ruleId, Guid entityNodeTypeId,
            bool isInputParameter, bool isEntity, string fieldName, string fullName, string typeName
            , string term, object value)
        {
            UpdateRuleParameter(ruleParameterId, ruleId, entityNodeTypeId, isInputParameter, isEntity,
                fieldName, fullName, typeName, term, value);
        }

        /// <summary>
        /// Gets all rule parameters by role identifier
        /// </summary>
        /// <param name="organizationId">Specifies the organization identifier</param>
        /// <param name="ruleId">Specifies the rule identifier</param>
        /// <returns>The RuleParametersDataTable object pupulated with information of the rule parameters.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.RuleParametersDataTable GetRuleParameters(Guid organizationId, Guid ruleId)
        {
            ClientDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.RuleParametersDataTable table = new ClientDataSet.RuleParametersDataTable();
            adapters.RuleParametersTableAdapter.Fill(table, 0, ruleId);
            return table;
        }

        /// <summary>
        /// Gets all rule parameters by role identifier
        /// </summary>
        /// <param name="ruleId">Specifies the rule identifier</param>
        /// <returns>The RuleParametersDataTable object pupulated with information of the rule parameters.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.RuleParametersDataTable GetRuleParameters(Guid ruleId)
        {
            return GetRuleParameters(UserContext.Current.SelectedOrganizationId, ruleId);
        }

        /// <summary>
        /// Gets rule parameter information for specified identifier
        /// </summary>
        /// <param name="ruleParameterId">Specifies the rule parameter identifier</param>
        /// <param name="organizationId">Specifies the organization identifier</param>
        /// <returns>The RuleParametersRow object pupulated with information of the rule.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.RuleParametersRow GetRuleParameterRow(Guid ruleParameterId, Guid organizationId)
        {
            ClientDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.RuleParametersDataTable table = new ClientDataSet.RuleParametersDataTable();
            adapters.RuleParametersTableAdapter.Fill(table, 1, ruleParameterId);
            return ((table.Count > 0) ? table[0] : null);
        }

        /// <summary>
        /// Gets rule parameter information for specified identifier
        /// </summary>
        /// <param name="ruleParameterId">Specifies the rule parameter identifier</param>
        /// <returns>The RuleParametersRow object pupulated with information of the rule.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.RuleParametersRow GetRuleParameterRow(Guid ruleParameterId)
        {
            return GetRuleParameterRow(ruleParameterId, UserContext.Current.SelectedOrganizationId);
        }

        #endregion

        #region Support Methods

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static EntityCollection GetEntityTypes(Guid rulesEngineId)
        {
            EntityCollection col = new EntityCollection();
            RulesEngine engine = WebApplication.RulesEngines[rulesEngineId.ToString()];
            if (engine != null)
            {
                foreach (Entity ent in WebApplication.Entities)
                {
                    if (engine.InputParameters.ContainsValue(ent.Id) &&
                        ent.Fields.Count > 0)
                        col.Add(ent);
                }
            }
            return col;
        }

        /// <summary>
        /// Gets display user name
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>First + Last Name</returns>
        public static string GetDisplayUserName(Guid userId, Guid organizationId)
        {
            string result = string.Empty;
            Micajah.Common.Dal.ClientDataSet.UserRow userRow = Micajah.Common.Bll.Providers.UserProvider.GetUserRow(userId, organizationId);
            if (userRow != null) result = userRow.FirstName + " " + userRow.LastName;
            return result;
        }

        #endregion
    }
}
