using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Micajah.Common.Bll
{
    [Serializable]
    public class EntityNodeType
    {
        #region Members

        private Guid m_Id;
        private string m_Name;
        private int m_OrderNumber;
        private int m_MaxRestrict;
        private Entity m_Entity;
        private EntityEventCollection m_Events;

        #endregion

        #region Private Properties

        private Entity Entity
        {
            get
            {
                if (m_Entity == null) m_Entity = EntityFieldProvider.Entities[this.Id.ToString()];
                return m_Entity;
            }
        }

        #endregion

        #region Public Properties

        public Guid Id
        {
            get { return m_Id; }
            internal set { m_Id = value; }
        }

        public string Name
        {
            get
            {
                if (m_Name == null)
                {
                    if (this.Entity != null)
                        m_Name = m_Entity.Name;
                }
                return m_Name;
            }
            internal set { m_Name = value; }
        }

        public int OrderNumber
        {
            get { return m_OrderNumber; }
            internal set { m_OrderNumber = value; }
        }

        public int MaxRestrict
        {
            get { return m_MaxRestrict; }
            internal set { m_MaxRestrict = value; }
        }

        public EntityEventCollection Events
        {
            get
            {
                if (m_Events == null) m_Events = new EntityEventCollection(true, false, true, true);
                return m_Events;
            }
        }

        #endregion

        #region Internal Methods

        internal static EntityNodeType Create(EntityNodeTypeElement value)
        {
            EntityNodeType nodeType = new EntityNodeType();
            nodeType.Id = value.Id;
            nodeType.Name = value.Name;
            nodeType.OrderNumber = value.OrderNumber;
            nodeType.MaxRestrict = value.MaxRestrict;
            return nodeType;
        }

        #endregion
    }

    [Serializable]
    public class EntityNodeTypeCollection : Collection<EntityNodeType>
    {
        #region Private Properties

        private List<EntityNodeType> ItemList
        {
            get { return base.Items as List<EntityNodeType>; }
        }

        #endregion

        #region Public Properties

        public EntityNodeType this[string name]
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
                delegate(EntityNodeType entityNodeType)
                {
                    return (string.Compare(entityNodeType.Name, value, StringComparison.Ordinal) == 0);
                });

            if (index == -1)
            {
                object obj = Support.ConvertStringToType(value, typeof(Guid));
                if (obj != null)
                {
                    Guid id = (Guid)obj;

                    index = this.ItemList.FindIndex(
                        delegate(EntityNodeType entityNodeType)
                        {
                            return (entityNodeType.Id == id);
                        });
                }
            }

            return index;
        }

        #endregion

        #region Private Methods

        private static int CompareByOrderNumber(EntityNodeType x, EntityNodeType y)
        {
            if (x == null)
            {
                return ((y == null) ? 0 : -1);
            }
            else
            {
                if (y == null)
                    return 1;
                else
                    return x.OrderNumber.CompareTo(y.OrderNumber);
            }
        }

        #endregion

        #region Public Methods

        public void Sort()
        {
            this.ItemList.Sort(CompareByOrderNumber);
        }

        #endregion
    }
}
