using Micajah.Common.Application;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Dal.TableAdapters;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;

namespace Micajah.Common.Bll.Providers
{
    [DataObjectAttribute(true)]
    public static class EntityFieldProvider
    {
        #region Members

        private static DataTable s_DataTypes;
        private static DataTable s_EntityFieldTypes;

        #endregion

        #region Private Methods

        private static void LoadEntityFieldValues(EntityField entityField, EntityFieldValueElementCollection values)
        {
            foreach (EntityFieldValueElement element in values)
            {
                object obj = Support.ConvertStringToType(element.Value, entityField.DataType);

                if (obj != null)
                    entityField.ListValues.Add(element.Name, new object[] { obj, false });
            }
            if (entityField.ListValues.Count > 0)
                entityField.EntityFieldType = EntityFieldType.SingleSelectList;
        }

        private static void LoadEntityFieldValues(EntityField entityField)
        {
            ClientDataSet.EntityFieldListsValuesDataTable table = GetEntityFieldListValues(entityField.Id, entityField.OrganizationId, true);
            foreach (ClientDataSet.EntityFieldListsValuesRow row in table)
            {
                object obj = Support.ConvertStringToType(row.Value, entityField.DataType);

                if (!(string.IsNullOrEmpty(row.Name) || (obj == null)))
                    entityField.ListValues.Add(row.Name, new object[] { obj, row.Default });
            }
        }

        private static ClientDataSet.EntityFieldsValuesDataTable GetEntityFieldsValues(Guid organizationId, Guid entityId, string localEntityId)
        {
            ClientDataSet.EntityFieldsValuesDataTable table = null;
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            if (adapters != null)
            {
                table = new ClientDataSet.EntityFieldsValuesDataTable();
                adapters.EntityFieldsValuesTableAdapter.Fill(table, 0, entityId, localEntityId);
            }
            return table;
        }

        private static ClientDataSet.EntityFieldsValuesDataTable GetEntityFieldValues(Guid organizationId, Guid entityFieldId, string localEntityId)
        {
            ClientDataSet.EntityFieldsValuesDataTable table = null;
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            if (adapters != null)
            {
                table = new ClientDataSet.EntityFieldsValuesDataTable();
                adapters.EntityFieldsValuesTableAdapter.Fill(table, 1, entityFieldId, localEntityId);
            }
            return table;
        }

        #endregion

        #region Internal Methods

        internal static EntityField CreateEntityField(EntityFieldElement value)
        {
            EntityField entityField = null;

            Type dataType = Type.GetType(value.DataType, false, true);
            if (dataType != null)
            {
                entityField = new EntityField();
                entityField.Name = value.Name;
                entityField.ColumnName = value.ColumnName;
                entityField.DataType = dataType;
                entityField.DefaultValue = Support.ConvertStringToType(value.DefaultValue, dataType);
                entityField.AllowDBNull = value.AllowDBNull;
                entityField.MinValue = Support.ConvertStringToType(value.MinValue, dataType);
                entityField.MaxValue = Support.ConvertStringToType(value.MaxValue, dataType);
                entityField.MaxLength = value.MaxLength;
                entityField.Unique = value.Unique;
                entityField.Id = value.EntityId;
                // TODO: Do we need this property in XML config? Maybe try to see the associated entity to define the value.
                //if (entityField.Id != Guid.Empty)
                ////    entityField.MappedHierarchyEntity = true;

                LoadEntityFieldValues(entityField, value.Values);
            }

            return entityField;
        }

        internal static EntityField CreateEntityField(ClientDataSet.EntityFieldRow value)
        {
            EntityField entityField = null;

            if (value != null)
            {
                string name = value.Name;
                if (!string.IsNullOrEmpty(name))
                {
                    Type dataType = null;
                    switch ((EntityFieldDataType)value.DataTypeId)
                    {
                        case EntityFieldDataType.Text:
                            dataType = typeof(string);
                            break;
                        case EntityFieldDataType.YesNo:
                            dataType = typeof(bool);
                            break;
                        case EntityFieldDataType.DateTime:
                            dataType = typeof(DateTime);
                            break;
                        case EntityFieldDataType.Numeric:
                            dataType = ((value.DecimalDigits > 0) ? typeof(decimal) : typeof(int));
                            break;
                    }
                    if (dataType != null)
                    {
                        entityField = new EntityField();
                        entityField.Id = value.EntityFieldId;
                        entityField.EntityFieldTypeId = value.EntityFieldTypeId;
                        entityField.Name = name;
                        entityField.MappedHierarchyEntity = (value.EntityFieldTypeId == 4);
                        entityField.Description = value.Description;
                        entityField.EntityFieldDataType = (EntityFieldDataType)value.DataTypeId;
                        entityField.DataType = dataType;
                        if (!value.IsDefaultValueNull()) entityField.DefaultValue = Support.ConvertStringToType(value.DefaultValue, dataType);
                        entityField.AllowDBNull = value.AllowDBNull;
                        entityField.Unique = value.Unique;
                        entityField.MaxLength = value.MaxLength;
                        if (!value.IsMinValueNull()) entityField.MinValue = Support.ConvertStringToType(value.MinValue, dataType);
                        if (!value.IsMaxValueNull()) entityField.MaxValue = Support.ConvertStringToType(value.MaxValue, dataType);
                        entityField.DecimalDigits = value.DecimalDigits;
                        entityField.OrderNumber = value.OrderNumber;
                        entityField.OrganizationId = value.OrganizationId;
                        if (!value.IsInstanceIdNull()) entityField.InstanceId = value.InstanceId;
                        entityField.Active = value.Active;
                        entityField.IsCustom = true;

                        LoadEntityFieldValues(entityField);
                    }
                }
            }

            return entityField;
        }

        internal static void LoadEntityCustomFields(Entity entity, Guid organizationId, Guid? instanceId, string localEntityId)
        {
            ClientDataSet.EntityFieldDataTable table1 = GetEntityFields(entity.Id, organizationId, instanceId, true);
            foreach (ClientDataSet.EntityFieldRow row1 in table1)
            {
                EntityField field = CreateEntityField(row1);
                if (field != null) entity.CustomFields.Add(field);
            }

            entity.CustomFields.Sort();

            LoadEntityCustomFieldsValues(entity, organizationId, localEntityId);
        }

        internal static void LoadEntityCustomFieldsValues(Entity entity, Guid organizationId, string localEntityId)
        {
            if (localEntityId != null)
            {
                ClientDataSet.EntityFieldsValuesDataTable table2 = GetEntityFieldsValues(organizationId, entity.Id, localEntityId);
                foreach (EntityField field in entity.CustomFields)
                {
                    field.ClearSelectedValues();
                    DataRow[] rows = table2.Select(string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", table2.EntityFieldIdColumn.ColumnName, field.Id.ToString()));
                    foreach (ClientDataSet.EntityFieldsValuesRow row2 in rows)
                    {
                        if (row2.IsValueNull())
                        {
                            if (field.AllowDBNull) field.AddSelectedValue(null);
                        }
                        else
                            field.AddSelectedValue(row2.Value);
                    }
                }
            }
        }

        internal static void SaveEntityCustomFields(Entity entity, Guid organizationId, string localEntityId)
        {
            if (localEntityId == null) return;

            ClientDataSet.EntityFieldsValuesDataTable table = GetEntityFieldsValues(organizationId, entity.Id, localEntityId);
            foreach (EntityField field in entity.CustomFields)
            {
                DataRow[] rows = table.Select(string.Format(CultureInfo.InvariantCulture, "{0} = '{1}' AND {2} = '{3}'"
                    , table.EntityFieldIdColumn.ColumnName, field.Id, table.LocalEntityIdColumn.ColumnName, Support.PreserveSingleQuote(localEntityId)));

                List<object> values = new List<object>(field.SelectedValues);
                if (field.AllowDBNull && (values.Count == 0)) values.Add(null);

                foreach (ClientDataSet.EntityFieldsValuesRow row in rows)
                {
                    object obj = null;
                    if (!row.IsValueNull()) obj = Support.ConvertStringToType(row.Value, field.DataType);
                    if (field.SelectedValues.Contains(obj))
                        values.Remove(obj);
                    else
                        row.Delete();
                }

                foreach (object obj in values)
                {
                    ClientDataSet.EntityFieldsValuesRow row = table.NewEntityFieldsValuesRow();
                    row.EntityFieldValueId = Guid.NewGuid();
                    row.EntityFieldId = field.Id;
                    row.LocalEntityId = localEntityId;
                    if (!Support.IsNullOrDBNull(obj))
                        row.Value = Convert.ToString(obj, CultureInfo.CurrentCulture);
                    table.AddEntityFieldsValuesRow(row);
                }
            }

            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            if (adapters != null) adapters.EntityFieldsValuesTableAdapter.Update(table);

            WebApplication.RefreshEntities();
        }

        internal static void SaveEntityCustomField(EntityField field, Guid organizationId, string localEntityId)
        {
            if (localEntityId == null) return;

            ClientDataSet.EntityFieldsValuesDataTable table = GetEntityFieldValues(organizationId, field.Id, localEntityId);

            List<object> values = new List<object>(field.SelectedValues);
            if (field.AllowDBNull && (values.Count == 0)) values.Add(null);

            foreach (ClientDataSet.EntityFieldsValuesRow row in table)
            {
                object obj = null;
                if (!row.IsValueNull()) obj = Support.ConvertStringToType(row.Value, field.DataType);
                if (field.SelectedValues.Contains(obj))
                    values.Remove(obj);
                else
                    row.Delete();
            }

            foreach (object obj in values)
            {
                ClientDataSet.EntityFieldsValuesRow row = table.NewEntityFieldsValuesRow();
                row.EntityFieldValueId = Guid.NewGuid();
                row.EntityFieldId = field.Id;
                row.LocalEntityId = localEntityId;
                if (!Support.IsNullOrDBNull(obj))
                    row.Value = Convert.ToString(obj, CultureInfo.CurrentCulture);
                table.AddEntityFieldsValuesRow(row);
            }

            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            if (adapters != null) adapters.EntityFieldsValuesTableAdapter.Update(table);

            WebApplication.RefreshEntities();
        }

        #endregion

        #region Public Methods

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetDataTypes()
        {
            if (s_DataTypes == null)
            {
                s_DataTypes = new DataTable();
                s_DataTypes.Locale = CultureInfo.CurrentCulture;
                s_DataTypes.Columns.Add("DataTypeId", typeof(int));
                s_DataTypes.Columns.Add("Name", typeof(string));
                s_DataTypes.Rows.Add((int)EntityFieldDataType.Text, Resources.EntityFieldDataType_Text_Name);
                s_DataTypes.Rows.Add((int)EntityFieldDataType.YesNo, Resources.EntityFieldDataType_YesNo_Name);
                s_DataTypes.Rows.Add((int)EntityFieldDataType.DateTime, Resources.EntityFieldDataType_DateTime_Name);
                s_DataTypes.Rows.Add((int)EntityFieldDataType.Numeric, Resources.EntityFieldDataType_Numeric_Name);
                s_DataTypes.AcceptChanges();
            }
            return s_DataTypes;
        }

        public static string GetDataTypeName(int dataTypeId)
        {
            DataRow[] rows = GetDataTypes().Select("DataTypeId = " + dataTypeId.ToString(CultureInfo.InvariantCulture));
            return ((rows.Length > 0) ? (string)rows[0]["Name"] : string.Empty);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetEntityFieldTypes()
        {
            if (s_EntityFieldTypes == null)
            {
                s_EntityFieldTypes = new DataTable();
                s_EntityFieldTypes.Locale = CultureInfo.CurrentCulture;
                s_EntityFieldTypes.Columns.Add("EntityFieldTypeId", typeof(int));
                s_EntityFieldTypes.Columns.Add("Name", typeof(string));
                s_EntityFieldTypes.Rows.Add((int)EntityFieldType.Value, Resources.EntityFieldType_Value_Name);
                s_EntityFieldTypes.Rows.Add((int)EntityFieldType.SingleSelectList, Resources.EntityFieldType_SingleSelectList_Name);
                s_EntityFieldTypes.Rows.Add((int)EntityFieldType.MultipleSelectList, Resources.EntityFieldType_MultipleSelectList_Name);
                s_EntityFieldTypes.AcceptChanges();
            }

            return s_EntityFieldTypes;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.EntityFieldDataTable GetEntityField(Guid entityFieldId)
        {
            return GetEntityField(entityFieldId, UserContext.Current.SelectedOrganizationId);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.EntityFieldDataTable GetEntityField(Guid entityFieldId, Guid organizationId)
        {
            ClientDataSet.EntityFieldDataTable table = null;
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            if (adapters != null)
            {
                table = new ClientDataSet.EntityFieldDataTable();
                adapters.EntityFieldTableAdapter.Fill(table, 1, entityFieldId);
            }
            return table;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.EntityFieldDataTable GetEntityFields(Guid entityId, Guid organizationId, Guid? instanceId, bool? active)
        {
            ClientDataSet.EntityFieldDataTable table = null;
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            if (adapters != null)
            {
                table = new ClientDataSet.EntityFieldDataTable();
                adapters.EntityFieldTableAdapter.Fill(table, 0, entityId, organizationId, instanceId, active);

                if (!FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
                {
                    foreach (ClientDataSet.EntityFieldRow row in table.Select(string.Format(CultureInfo.InvariantCulture, "{0} IS NOT NULL", table.InstanceIdColumn.ColumnName)))
                    {
                        table.RemoveEntityFieldRow(table.FindByEntityFieldId(row.EntityFieldId));
                    }
                    table.AcceptChanges();
                }
            }
            return table;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.EntityFieldListsValuesDataTable GetEntityFieldListValues(Guid entityFieldId, bool? active)
        {
            return GetEntityFieldListValues(entityFieldId, UserContext.Current.SelectedOrganizationId, active);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.EntityFieldListsValuesDataTable GetEntityFieldListValues(Guid entityFieldId, Guid organizationId, bool? active)
        {
            ClientDataSet.EntityFieldListsValuesDataTable table = null;
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            if (adapters != null)
            {
                table = new ClientDataSet.EntityFieldListsValuesDataTable();
                adapters.EntityFieldListsValuesTableAdapter.Fill(table, 0, entityFieldId, active);
            }
            return table;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.EntityFieldListsValuesDataTable GetEntityFieldListValue(Guid entityFieldListValueId)
        {
            ClientDataSet.EntityFieldListsValuesDataTable table = null;
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(UserContext.Current.SelectedOrganizationId);
            if (adapters != null)
            {
                table = new ClientDataSet.EntityFieldListsValuesDataTable();
                adapters.EntityFieldListsValuesTableAdapter.Fill(table, 1, entityFieldListValueId);
            }
            return table;
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEntityField(Guid entityFieldId)
        {
            DeleteEntityField(entityFieldId, UserContext.Current.SelectedOrganizationId);
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEntityField(Guid entityFieldId, Guid organizationId)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            if (adapters != null) adapters.EntityFieldTableAdapter.Delete(entityFieldId);

            WebApplication.RefreshEntities();
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEntityFieldListValue(Guid entityFieldListValueId)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(UserContext.Current.SelectedOrganizationId);
            if (adapters != null) adapters.EntityFieldListsValuesTableAdapter.Delete(entityFieldListValueId);

            WebApplication.RefreshEntities();
        }

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertEntityField(int entityFieldTypeId, string name, string description, int dataTypeId, string defaultValue
            , bool allowDBNull, bool unique, int maxLength, string minValue, string maxValue, int decimalDigits, int orderNumber
            , Guid entityId, Guid organizationId, Guid? instanceId, bool active)
        {
            ClientDataSet.EntityFieldDataTable table = new ClientDataSet.EntityFieldDataTable();
            ClientDataSet.EntityFieldRow row = table.NewEntityFieldRow();

            row.EntityFieldId = Guid.NewGuid();
            row.EntityFieldTypeId = entityFieldTypeId;
            row.Name = name;
            row.Description = description;
            row.DataTypeId = dataTypeId;
            if ((EntityFieldType)entityFieldTypeId == EntityFieldType.Value)
                row.DefaultValue = defaultValue;
            row.AllowDBNull = allowDBNull;
            row.Unique = unique;
            row.MaxLength = maxLength;
            if (minValue != null) row.MinValue = minValue;
            if (maxValue != null) row.MaxValue = maxValue;
            row.DecimalDigits = decimalDigits;
            row.OrderNumber = orderNumber;
            row.EntityId = entityId;
            row.OrganizationId = organizationId;
            if (instanceId.HasValue) row.InstanceId = instanceId.Value;
            row.Active = active;

            table.AddEntityFieldRow(row);

            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            if (adapters != null) adapters.EntityFieldTableAdapter.Update(row);

            WebApplication.RefreshEntities();

            return row.EntityFieldId;
        }

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertEntityFieldListValue(Guid entityFieldId, string name, string value, bool Default, bool active)
        {
            ClientDataSet.EntityFieldListsValuesDataTable table = new ClientDataSet.EntityFieldListsValuesDataTable();
            ClientDataSet.EntityFieldListsValuesRow row = table.NewEntityFieldListsValuesRow();

            row.EntityFieldListValueId = Guid.NewGuid();
            row.EntityFieldId = entityFieldId;
            row.Name = name;
            row.Value = value;
            row.Default = Default;
            row.Active = active;

            table.AddEntityFieldListsValuesRow(row);

            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(UserContext.Current.SelectedOrganizationId);
            if (adapters != null) adapters.EntityFieldListsValuesTableAdapter.Update(row);

            WebApplication.RefreshEntities();

            return row.EntityFieldListValueId;
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateEntityField(Guid entityFieldId, int entityFieldTypeId, string name, string description, int dataTypeId, string defaultValue
            , bool allowDBNull, bool unique, int maxLength, string minValue, string maxValue, int decimalDigits, int orderNumber
            , Guid entityId, Guid organizationId, Guid? instanceId, bool active)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            if (adapters != null)
            {
                adapters.EntityFieldTableAdapter.Update(entityFieldId, entityFieldTypeId, name, description, dataTypeId, (((EntityFieldType)entityFieldTypeId == EntityFieldType.Value) ? defaultValue : null)
                    , allowDBNull, unique, maxLength, minValue, maxValue, decimalDigits, orderNumber, entityId, organizationId, instanceId, active);
            }

            WebApplication.RefreshEntities();
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateEntityFieldListValue(Guid entityFieldListValueId, Guid entityFieldId, string name, string value, bool Default, bool active)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(UserContext.Current.SelectedOrganizationId);
            if (adapters != null) adapters.EntityFieldListsValuesTableAdapter.Update(entityFieldListValueId, entityFieldId, name, value, Default, active);

            WebApplication.RefreshEntities();
        }

        #endregion
    }
}
