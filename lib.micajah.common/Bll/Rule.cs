using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Dal;
using Micajah.Common.Security;

namespace Micajah.Common.Bll
{
    [Serializable]
    public class Rule
    {
        #region Members
        private Guid ruleId;
        private Guid rulesEngineId;
        private Guid organizationId;
        private Guid? instanceId;
        private string name;
        private string displayName;
        private int usedQty;
        private Guid? lastUsedUser;
        private DateTime? lastUsedDate;
        private Guid createdBy;
        private DateTime createdDate;
        private int orderNumber;
        private bool active;
        private EntityCollection inputEntities;
        #endregion

        #region Constructors
        private Rule()
        {
            inputEntities = new EntityCollection();
        }
        #endregion

        #region Fabric Methods
        internal static Rule Create(OrganizationDataSet.RuleRow row)
        {
            if (row == null) return null;
            Rule rule = new Rule();
            rule.RuleId = row.RuleId;
            rule.RulesEngineId = row.RuleEngineId;
            rule.OrganizationId = row.OrganizationId;
            rule.InstanceId = row.IsInstanceIdNull() ? new Guid?() : new Guid?(row.InstanceId);
            rule.Name = row.Name;
            rule.DisplayName = row.DisplayName;
            rule.UsedQty = row.UsedQty;
            rule.LastUsedUserId = row.IsLastUsedUserNull() ? new Guid?() : new Guid?(row.LastUsedUser);
            rule.LastUsedDate = row.IsLastUsedDateNull() ? new DateTime?() : new DateTime?(row.LastUsedDate);
            rule.CreatedUserId = row.CreatedBy;
            rule.CreatedDate = row.CreatedDate;
            rule.OrderNumber = row.OrderNumber;
            rule.Active = row.Active;
            return rule;
        }

        public static Rule Create(Guid organizationId, Guid ruleId)
        {
            return Create(RuleEngineProvider.GetRuleRow(ruleId, organizationId));
        }

        public static Rule Create(Guid ruleId)
        {
            return Create(RuleEngineProvider.GetRuleRow(ruleId));
        }

        public static Rule Create(Guid organizationId, string ruleName)
        {
            return Create(RuleEngineProvider.GetRuleRow(ruleName, organizationId));
        }

        public static Rule Create(string ruleName)
        {
            return Create(RuleEngineProvider.GetRuleRow(ruleName));
        }
        #endregion

        #region Properties

        public Guid RuleId
        {
            get { return ruleId; }
            internal set { ruleId = value; }
        }

        public Guid OrganizationId
        {
            get { return organizationId; }
            internal set { organizationId = value; }
        }

        public Guid? InstanceId
        {
            get { return instanceId; }
            internal set { instanceId = value; }
        }

        public Guid RulesEngineId
        {
            get { return rulesEngineId; }
            internal set { rulesEngineId = value; }
        }

        public string Name
        {
            get { return name; }
            internal set { name = value; }
        }

        public string DisplayName
        {
            get { return displayName; }
            internal set { displayName = value; }
        }

        public int UsedQty
        {
            get { return usedQty; }
            internal set { usedQty = value; }
        }

        public Guid? LastUsedUserId
        {
            get { return lastUsedUser; }
            internal set { lastUsedUser = value; }
        }

        public Micajah.Common.Dal.OrganizationDataSet.UserRow LastUsedUser
        {
            get
            {
                if (lastUsedDate.HasValue)
                    return UserProvider.GetUserRow(lastUsedUser.Value, this.OrganizationId);
                return null;
            }
        }

        public DateTime? LastUsedDate
        {
            get { return lastUsedDate; }
            internal set { lastUsedDate = value; }
        }

        public Guid CreatedUserId
        {
            get { return createdBy; }
            internal set { createdBy = value; }
        }

        public Micajah.Common.Dal.OrganizationDataSet.UserRow CreatedBy
        {
            get { return UserProvider.GetUserRow(createdBy, this.OrganizationId); }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
            internal set { createdDate = value; }
        }

        public int OrderNumber
        {
            get { return orderNumber; }
            internal set { orderNumber = value; }
        }

        public bool Active
        {
            get { return active; }
            internal set { active = value; }
        }

        public EntityCollection InputEntities
        {
            get { return inputEntities; }
        }

        #endregion

        #region Public Methods

        public void AddInputEntity(Entity entity)
        {
            inputEntities.Add(entity);
        }

        public void AddInputEntity(EntityCollection entities)
        {
            if (entities != null)
            {
                foreach (Entity entity in entities)
                {
                    inputEntities.Add(entity);
                }
            }
        }
        public void ClearEntities()
        {
            inputEntities.Clear();
        }

        public bool Resolve()
        {
            OrganizationDataSet.RuleParametersDataTable table = RuleEngineProvider.GetRuleParameters(this.RuleId);
            this.LastUsedUserId = UserContext.Current.UserId;
            RuleEngineProvider.UpdateRuleUses(
                UserContext.Current.SelectedOrganization.OrganizationId,
                this.RuleId,
                UserContext.Current.UserId,
                DateTime.UtcNow);
            int count = table.Count;
            if (count == 0) return true;
            int result = 0;
            foreach (OrganizationDataSet.RuleParametersRow row in table)
            {
                foreach (Entity ent in this.InputEntities)
                {
                    if (ent.Id.Equals(row.EntityNodeTypeId))
                    {
                        EntityField field = ent.Fields[row.FieldName];
                        if (field != null)
                        {
                            if (field.DataType.FullName.Equals(row.FullName) && field.Value != null)
                            {
                                #region Comparing
                                if (field.DataType == typeof(short))
                                {
                                    short short1 = (short)row.Value;
                                    short short2 = (short)field.Value;
                                    switch (row.Term)
                                    {
                                        case "==":
                                            if (short2 == short1)
                                                result++;
                                            break;
                                        case "!=":
                                            if (short2 != short1)
                                                result++;
                                            break;
                                        case ">":
                                            if (short2 > short1)
                                                result++;
                                            break;
                                        case ">=":
                                            if (short2 >= short1)
                                                result++;
                                            break;
                                        case "<":
                                            if (short2 < short1)
                                                result++;
                                            break;
                                        case "<=":
                                            if (short2 <= short1)
                                                result++;
                                            break;
                                    }
                                }
                                else if (field.DataType == typeof(int))
                                {
                                    int int1 = (int)row.Value;
                                    int int2 = (int)field.Value;
                                    switch (row.Term)
                                    {
                                        case "==":
                                            if (int2 == int1)
                                                result++;
                                            break;
                                        case "!=":
                                            if (int2 != int1)
                                                result++;
                                            break;
                                        case ">":
                                            if (int2 > int1)
                                                result++;
                                            break;
                                        case ">=":
                                            if (int2 >= int1)
                                                result++;
                                            break;
                                        case "<":
                                            if (int2 < int1)
                                                result++;
                                            break;
                                        case "<=":
                                            if (int2 <= int1)
                                                result++;
                                            break;
                                    }
                                }
                                else if (field.DataType == typeof(long))
                                {
                                    long long1 = (long)row.Value;
                                    long long2 = (long)field.Value;
                                    switch (row.Term)
                                    {
                                        case "==":
                                            if (long2 == long1)
                                                result++;
                                            break;
                                        case "!=":
                                            if (long2 != long1)
                                                result++;
                                            break;
                                        case ">":
                                            if (long2 > long1)
                                                result++;
                                            break;
                                        case ">=":
                                            if (long2 >= long1)
                                                result++;
                                            break;
                                        case "<":
                                            if (long2 < long1)
                                                result++;
                                            break;
                                        case "<=":
                                            if (long2 <= long1)
                                                result++;
                                            break;
                                    }
                                }
                                else if (field.DataType == typeof(decimal))
                                {
                                    decimal decimal1 = (decimal)row.Value;
                                    decimal decimal2 = (decimal)field.Value;
                                    switch (row.Term)
                                    {
                                        case "==":
                                            if (decimal2 == decimal1)
                                                result++;
                                            break;
                                        case "!=":
                                            if (decimal2 != decimal1)
                                                result++;
                                            break;
                                        case ">":
                                            if (decimal2 > decimal1)
                                                result++;
                                            break;
                                        case ">=":
                                            if (decimal2 >= decimal1)
                                                result++;
                                            break;
                                        case "<":
                                            if (decimal2 < decimal1)
                                                result++;
                                            break;
                                        case "<=":
                                            if (decimal2 <= decimal1)
                                                result++;
                                            break;
                                    }
                                }
                                else if (field.DataType == typeof(double))
                                {
                                    double double1 = (double)row.Value;
                                    double double2 = (double)field.Value;
                                    switch (row.Term)
                                    {
                                        case "==":
                                            if (double2 == double1)
                                                result++;
                                            break;
                                        case "!=":
                                            if (double2 != double1)
                                                result++;
                                            break;
                                        case ">":
                                            if (double2 > double1)
                                                result++;
                                            break;
                                        case ">=":
                                            if (double2 >= double1)
                                                result++;
                                            break;
                                        case "<":
                                            if (double2 < double1)
                                                result++;
                                            break;
                                        case "<=":
                                            if (double2 <= double1)
                                                result++;
                                            break;
                                    }
                                }
                                else if (field.DataType == typeof(float))
                                {
                                    float float1 = (float)row.Value;
                                    float float2 = (float)field.Value;
                                    switch (row.Term)
                                    {
                                        case "==":
                                            if (float2 == float1)
                                                result++;
                                            break;
                                        case "!=":
                                            if (float2 != float1)
                                                result++;
                                            break;
                                        case ">":
                                            if (float2 > float1)
                                                result++;
                                            break;
                                        case ">=":
                                            if (float2 >= float1)
                                                result++;
                                            break;
                                        case "<":
                                            if (float2 < float1)
                                                result++;
                                            break;
                                        case "<=":
                                            if (float2 <= float1)
                                                result++;
                                            break;
                                    }
                                }
                                else if (field.DataType == typeof(DateTime))
                                {
                                    DateTime DateTime1 = (DateTime)row.Value;
                                    DateTime DateTime2 = (DateTime)field.Value;
                                    switch (row.Term)
                                    {
                                        case "==":
                                            if (DateTime2 == DateTime1)
                                                result++;
                                            break;
                                        case "!=":
                                            if (DateTime2 != DateTime1)
                                                result++;
                                            break;
                                        case ">":
                                            if (DateTime2 > DateTime1)
                                                result++;
                                            break;
                                        case ">=":
                                            if (DateTime2 >= DateTime1)
                                                result++;
                                            break;
                                        case "<":
                                            if (DateTime2 < DateTime1)
                                                result++;
                                            break;
                                        case "<=":
                                            if (DateTime2 <= DateTime1)
                                                result++;
                                            break;
                                    }
                                }
                                else if (field.DataType == typeof(Entity))
                                {
                                    Guid guid1 = (Guid)row.Value;
                                    Guid guid2 = (Guid)field.Value;
                                    if (row.Term == "==")
                                    {
                                        if (guid1 == guid2)
                                            result++;
                                    }
                                    else if (row.Term == "!=")
                                    {
                                        if (guid1 != guid2)
                                            result++;
                                    }
                                }
                                else
                                {
                                    if (row.Term == "==")
                                    {
                                        if (field.Value.Equals(row.Value))
                                            result++;
                                    }
                                    else
                                    {
                                        if (!field.Value.Equals(row.Value))
                                            result++;
                                    }
                                }
                                #endregion
                                break;
                            }
                        }
                    }
                }
            }
            return (count == result);
        }

        #endregion
    }

    [Serializable]
    public class RuleCollection : Collection<Rule>
    {
        #region Private Properties

        private List<Rule> ItemList
        {
            get { return base.Items as List<Rule>; }
        }

        #endregion

        #region Public Properties

        public Rule this[string name]
        {
            get
            {
                int index = this.FindIndexByIdOrName(name);
                return (((index < 0) || (index >= this.Count)) ? null : base[index]);
            }
            set
            {
                int index = this.FindIndexByIdOrName(name);
                if (index > -1)
                    base[index] = value;
                else
                    base.Add(value);
            }
        }
        #endregion

        #region Private Methods

        private int FindIndexByIdOrName(string value)
        {
            int index = this.ItemList.FindIndex(
                delegate(Rule rule)
                {
                    return (string.Compare(rule.Name, value, StringComparison.Ordinal) == 0);
                });

            if (index == -1)
            {
                object obj = Support.ConvertStringToType(value, typeof(Guid));
                if (obj != null)
                {
                    Guid id = (Guid)obj;

                    index = this.ItemList.FindIndex(
                        delegate(Rule rule)
                        {
                            return (rule.RuleId == id);
                        });
                }
            }

            return index;
        }

        private int SortByOrderNumber(Rule x, Rule y)
        {
            if (x == null)
                if (y == null)
                    return 0;
                else
                    return -1;
            else
                if (y == null)
                    return 1;
                else
                    return x.OrderNumber.CompareTo(y.OrderNumber);
        }

        #endregion

        #region Public Methods

        public static RuleCollection GetRules(Guid ruleEngineId)
        {
            Guid? instanceId = UserContext.Current.SelectedInstanceId;
            if (instanceId.Value == Guid.Empty) instanceId = null;

            RuleCollection coll = new RuleCollection();
            foreach (OrganizationDataSet.RuleRow row in RuleEngineProvider.GetRules(ruleEngineId, UserContext.Current.SelectedOrganizationId, instanceId))
            {
                coll.Add(Rule.Create(row));
            }
            coll.Sort();

            return coll;
        }

        public void Sort()
        {
            this.ItemList.Sort(SortByOrderNumber);
        }

        #endregion
    }
}
