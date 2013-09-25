using Micajah.Common.Bll.Providers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Micajah.Common.Bll
{
    [Serializable]
    public class EntityField : IComparable<EntityField>
    {
        #region Members

        private bool m_Active;
        private bool m_AllowDBNull;
        private string m_Name;
        private string m_ColumnName;
        private Type m_DataType;
        private object m_DefaultValue;
        private string m_Description;
        private Entity m_Entity;
        private int m_EntityFieldTypeId;
        private int m_EntityFieldDataTypeId;
        private Guid m_Id;
        private Guid m_InstanceId;
        private bool m_IsCustom;
        private bool m_MappedHierarchyEntity;
        private object m_MinValue;
        private object m_MaxValue;
        private int m_MaxLength;
        private int m_DecimalDigits;
        private int m_OrderNumber;
        private Guid m_OrganizationId;
        private bool m_Unique;
        private List<object> m_SelectedValues;
        private Dictionary<string, object[]> m_ListValues;

        #endregion

        #region Constructors

        public EntityField()
        {
            m_Active = true;
            m_AllowDBNull = true;
            m_DataType = typeof(string);
            m_EntityFieldTypeId = (int)EntityFieldType.Value;
        }

        #endregion

        #region Public Properties

        public bool Active
        {
            get { return m_Active; }
            internal set { m_Active = value; }
        }

        public bool AllowDBNull
        {
            get { return m_AllowDBNull; }
            internal set { m_AllowDBNull = value; }
        }

        public string Name
        {
            get { return m_Name; }
            internal set { m_Name = value; }
        }

        public string ColumnName
        {
            get { return m_ColumnName; }
            internal set { m_ColumnName = value; }
        }

        public Type DataType
        {
            get { return m_DataType; }
            internal set { m_DataType = value; }
        }

        public object DefaultValue
        {
            get { return m_DefaultValue; }
            internal set { m_DefaultValue = value; }
        }

        public string Description
        {
            get { return m_Description; }
            internal set { m_Description = value; }
        }

        public Entity Entity
        {
            get
            {
                if (!this.IsCustom)
                {
                    if (m_Entity == null)
                        m_Entity = EntityFieldProvider.Entities[this.Id.ToString()];
                }
                return m_Entity;
            }
        }

        public int EntityFieldTypeId
        {
            get { return m_EntityFieldTypeId; }
            internal set { m_EntityFieldTypeId = value; }
        }

        public EntityFieldType EntityFieldType
        {
            get { return (EntityFieldType)m_EntityFieldTypeId; }
            internal set { m_EntityFieldTypeId = (int)value; }
        }

        public EntityFieldDataType EntityFieldDataType
        {
            get { return (EntityFieldDataType)m_EntityFieldDataTypeId; }
            internal set { m_EntityFieldDataTypeId = (int)value; }
        }

        public object MinValue
        {
            get { return m_MinValue; }
            internal set { m_MinValue = value; }
        }

        public object MaxValue
        {
            get { return m_MaxValue; }
            internal set { m_MaxValue = value; }
        }

        public int MaxLength
        {
            get { return m_MaxLength; }
            internal set { m_MaxLength = value; }
        }

        public Guid Id
        {
            get { return m_Id; }
            internal set { m_Id = value; }
        }

        public Guid InstanceId
        {
            get { return m_InstanceId; }
            internal set { m_InstanceId = value; }
        }

        public bool IsCustom
        {
            get { return m_IsCustom; }
            internal set { m_IsCustom = value; }
        }

        public bool MappedHierarchyEntity
        {
            get { return m_MappedHierarchyEntity; }
            set { m_MappedHierarchyEntity = value; }
        }

        public int OrderNumber
        {
            get { return m_OrderNumber; }
            internal set { m_OrderNumber = value; }
        }

        public int DecimalDigits
        {
            get { return m_DecimalDigits; }
            internal set { m_DecimalDigits = value; }
        }

        public Guid OrganizationId
        {
            get { return m_OrganizationId; }
            internal set { m_OrganizationId = value; }
        }

        public bool Unique
        {
            get { return m_Unique; }
            internal set { m_Unique = value; }
        }

        public Dictionary<string, object[]> ListValues
        {
            get
            {
                if (m_ListValues == null) m_ListValues = new Dictionary<string, object[]>();
                return m_ListValues;
            }
        }

        public Dictionary<string, object> ListValuesOld
        {
            get
            {
                Dictionary<string, object> list = new Dictionary<string, object>();
                foreach (string key in ListValues.Keys)
                {
                    object[] obj;
                    if (ListValues.TryGetValue(key, out obj))
                        list.Add(key, obj[0]);
                }
                return list;
            }
        }

        public object Value
        {
            get { return this.SelectedValue; }
            set
            {
                if (value != null)
                {
                    if (value.GetType().Equals(typeof(Entity)))
                    {
                        this.SelectedValue = ((Entity)value).Id.ToString();
                        return;
                    }
                }

                this.SelectedValue = value;
            }
        }

        public object SelectedValue
        {
            get { return ((this.SelectedValues.Count > 0) ? m_SelectedValues[0] : null); }
            set
            {
                if (this.SelectedValues.Count == 0)
                    this.AddSelectedValue(value);
                else
                    this.AddSelectedValue(value, 0);
            }
        }

        public ReadOnlyCollection<object> SelectedValues
        {
            get
            {
                if (m_SelectedValues == null) m_SelectedValues = new List<object>();
                return m_SelectedValues.AsReadOnly();
            }
        }

        #endregion

        #region Operators

        public static bool operator ==(EntityField field1, EntityField field2)
        {
            if (object.ReferenceEquals(field1, field2))
                return true;

            if (((object)field1 == null) || ((object)field2 == null))
                return false;

            return field1.Equals(field2);
        }

        public static bool operator !=(EntityField field1, EntityField field2)
        {
            return (!(field1 == field2));
        }

        public static bool operator <(EntityField field1, EntityField field2)
        {
            return (field1.CompareTo(field2) < 0);
        }

        public static bool operator >(EntityField field1, EntityField field2)
        {
            return (field1.CompareTo(field2) > 0);
        }

        #endregion

        #region Private Methods

        public void AddSelectedValue(object value, int index)
        {
            object obj = null;
            if (value != null)
            {
                if (value.GetType().Equals(typeof(Entity)))
                    obj = ((Entity)value).Id;
                obj = Support.ConvertStringToType(Convert.ToString(value, CultureInfo.CurrentCulture), this.DataType);
            }
            if (this.SelectedValues != null)
            {
                if (index == -1)
                    this.m_SelectedValues.Add(obj);
                else
                    this.m_SelectedValues[index] = obj;
            }
        }

        #endregion

        #region Overriden Methods

        public override bool Equals(object obj)
        {
            EntityField field = obj as EntityField;
            if ((object)field == null)
                return false;
            return (this.CompareTo(field) == 0);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion

        #region Public Methods

        public void AddSelectedValue(object value)
        {
            this.AddSelectedValue(value, -1);
        }

        public void ClearSelectedValues()
        {
            m_SelectedValues = null;
        }

        public int CompareTo(EntityField other)
        {
            int result = 0;
            if ((object)other == null)
                result = 1;
            else
            {
                result = (this.OrderNumber - other.OrderNumber);
                if (result == 0) result += string.Compare(this.Name, other.Name, StringComparison.CurrentCultureIgnoreCase);
            }
            return result;
        }

        #endregion
    }

    [Serializable]
    public class EntityFieldCollection : Collection<EntityField>
    {
        #region Private Properties

        private List<EntityField> ItemList
        {
            get { return base.Items as List<EntityField>; }
        }

        #endregion

        #region Public Properties

        public EntityField this[string name]
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
                delegate(EntityField entityField)
                {
                    return (string.Compare(entityField.Name, value, StringComparison.Ordinal) == 0);
                });

            if (index == -1)
            {
                object obj = Support.ConvertStringToType(value, typeof(Guid));
                if (obj != null)
                {
                    Guid id = (Guid)obj;

                    index = this.ItemList.FindIndex(
                        delegate(EntityField entityField)
                        {
                            return (entityField.Id == id);
                        });
                }
            }

            return index;
        }

        #endregion

        #region Public Methods

        public void Sort()
        {
            this.ItemList.Sort();
        }

        #endregion
    }
}
