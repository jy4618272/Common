using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;

namespace Micajah.Common.Bll
{
    [Serializable]
    public class EntityEvent
    {
        #region Members

        private string m_Name;
        private string m_ImageUrl;
        private string m_Url;

        #endregion

        #region Public Properties

        public string Name
        {
            get { return m_Name; }
            internal set { m_Name = value; }
        }

        public string ImageUrl
        {
            get { return m_ImageUrl; }
            internal set { m_ImageUrl = value; }
        }

        public string Url
        {
            get { return m_Url; }
            internal set { m_Url = value; }
        }

        #endregion

        #region Internal Methods

        internal static EntityEvent Create(EntityEventElement value)
        {
            EntityEvent entityEvent = new EntityEvent();

            entityEvent = new EntityEvent();
            entityEvent.Name = value.Name;
            entityEvent.ImageUrl = value.IconImageUrl;
            entityEvent.Url = value.EditPageUrl;

            return entityEvent;
        }

        #endregion
    }

    [Serializable]
    public class EntityEventCollection : Collection<EntityEvent>
    {
        #region Constructors

        public EntityEventCollection() { }

        public EntityEventCollection(bool addStandardEvents, bool isRoot, bool isEnableHierarchy, bool enableNodeTypesCustomization)
        {
            if (addStandardEvents)
            {
                EntityEvent entityEvent = new EntityEvent();
                entityEvent.Name = "Create";
                this.Add(entityEvent);

                if (!isRoot) // root
                {
                    entityEvent = new EntityEvent();
                    entityEvent.Name = "Delete";
                    this.Add(entityEvent);

                    entityEvent = new EntityEvent();
                    if (isEnableHierarchy)
                        entityEvent.Name = "Rename";
                    else
                        entityEvent.Name = "Update";
                    this.Add(entityEvent);

                    entityEvent = new EntityEvent();
                    entityEvent.Name = "Clone";
                    this.Add(entityEvent);

                    if (enableNodeTypesCustomization)
                    {
                        entityEvent = new EntityEvent();
                        entityEvent.Name = Resources.EntityControl_EditNodeType;
                        this.Add(entityEvent);
                    }
                }
            }
        }

        #endregion

        #region Private Properties

        private List<EntityEvent> ItemList
        {
            get { return base.Items as List<EntityEvent>; }
        }

        #endregion

        #region Public Properties

        public EntityEvent this[string name]
        {
            get
            {
                return this.ItemList.Find(
                    delegate(EntityEvent entityEvent)
                    {
                        return (string.Compare(entityEvent.Name, name, StringComparison.Ordinal) == 0);
                    });
            }
            set
            {
                int index = this.ItemList.FindIndex(
                    delegate(EntityEvent entityEvent)
                    {
                        return (string.Compare(entityEvent.Name, name, StringComparison.Ordinal) == 0);
                    });

                if (index > -1)
                    base[index] = value;
                else
                    base.Add(value);
            }
        }

        #endregion
    }
}
