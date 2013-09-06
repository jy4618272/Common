using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Micajah.Common.Bll
{
    [Serializable]
    public class Entity
    {
        #region Members

        private Guid m_Id;
        private string m_Name;
        private string m_CustomNavigateUrl;
        private string m_CustomRootNodeText;
        private string m_TableName;
        private bool m_EnableHierarchy;
        private bool m_EnableRootNodeSelection;
        private int m_HierarchyMaxDepth;
        private EntityLevel m_HierarchyStartLevel;
        private bool m_EnableNodeTypesCustomization;
        private EntityNodeTypeCollection m_NodeTypes;
        private EntityNodeTypeCollection m_CustomNodeTypes;
        private EntityFieldCollection m_Fields;
        private EntityFieldCollection m_CustomFields;
        private EntityEventCollection m_Events;
        private EntityEventCollection m_CustomEvents;
        private Guid m_OrganizationId = Guid.Empty;
        private Guid? m_InstanceId;
        private string m_LocalEntityId;

        #endregion

        #region Public Properties

        public Guid Id
        {
            get { return m_Id; }
            internal set { m_Id = value; }
        }

        public string Name
        {
            get { return m_Name; }
            internal set { m_Name = value; }
        }

        public string TableName
        {
            get { return m_TableName; }
            internal set { m_TableName = value; }
        }

        public string CustomNavigateUrl
        {
            get { return m_CustomNavigateUrl; }
            internal set { m_CustomNavigateUrl = value; }
        }

        public string CustomRootNodeText
        {
            get { return m_CustomRootNodeText; }
            internal set { m_CustomRootNodeText = value; }
        }

        public bool EnableHierarchy
        {
            get { return m_EnableHierarchy; }
            internal set { m_EnableHierarchy = value; }
        }

        public bool EnableRootNodeSelection
        {
            get { return m_EnableRootNodeSelection; }
            internal set { m_EnableRootNodeSelection = value; }
        }

        public int HierarchyMaxDepth
        {
            get { return m_HierarchyMaxDepth; }
            internal set { m_HierarchyMaxDepth = value; }
        }

        public EntityLevel HierarchyStartLevel
        {
            get { return m_HierarchyStartLevel; }
            internal set { m_HierarchyStartLevel = value; }
        }

        public bool EnableNodeTypesCustomization
        {
            get { return m_EnableNodeTypesCustomization; }
            internal set { m_EnableNodeTypesCustomization = value; }
        }

        public EntityNodeTypeCollection NodeTypes
        {
            get
            {
                if (m_NodeTypes == null) m_NodeTypes = new EntityNodeTypeCollection();
                return m_NodeTypes;
            }
        }

        public void Refresh()
        {
            m_CustomNodeTypes = null;
        }

        // TODO: there is the bug, because the collection filled using the used context, but it's cached in global object (entity).
        public EntityNodeTypeCollection CustomNodeTypes
        {
            get
            {
                UserContext user = UserContext.Current;
                if (m_CustomNodeTypes == null && user != null)
                {
                    Guid? instanceId = null;
                    if (this.HierarchyStartLevel == EntityLevel.Instance)
                        instanceId = user.SelectedInstanceId;
                    if (UserContext.Current != null)
                    {
                        m_CustomNodeTypes = GetCustomNodeTypes(user.SelectedOrganizationId, instanceId);
                    }
                }
                return m_CustomNodeTypes;
            }
        }

        public EntityFieldCollection Fields
        {
            get
            {
                if (m_Fields == null) m_Fields = new EntityFieldCollection();
                return m_Fields;
            }
        }

        public EntityFieldCollection CustomFields
        {
            get
            {
                if (m_CustomFields == null) m_CustomFields = new EntityFieldCollection();
                return m_CustomFields;
            }
        }

        public EntityEventCollection Events
        {
            get
            {
                if (m_Events == null) m_Events = new EntityEventCollection(true, this.EnableHierarchy, false, this.EnableNodeTypesCustomization);
                return m_Events;
            }
        }

        public EntityEventCollection CustomEvents
        {
            get
            {
                if (m_CustomEvents == null) m_CustomEvents = new EntityEventCollection(false, false, false, false);
                return m_CustomEvents;
            }
        }

        #endregion

        #region Constructors

        public Entity()
        {
            m_HierarchyStartLevel = EntityLevel.Organization;
        }

        #endregion

        #region Private Methods

        private static void FillEntityCollections(Entity entity, EntityElement value)
        {
            foreach (EntityFieldElement fieldNode in value.Fields)
            {
                EntityField field = EntityFieldProvider.CreateEntityField(fieldNode);
                if (field != null) entity.Fields.Add(field);
            }

            foreach (EntityEventElement eventNode in value.Events)
            {
                EntityEvent entityEvent = EntityEvent.Create(eventNode);
                if (entityEvent != null) entity.CustomEvents.Add(entityEvent);
            }

            if (entity.EnableHierarchy)
            {
                foreach (EntityNodeTypeElement nodeTypeNode in value.Hierarchy.NodeTypes)
                {
                    EntityNodeType nodeType = EntityNodeType.Create(nodeTypeNode);
                    if (nodeType != null) entity.NodeTypes.Add(nodeType);
                }

                entity.NodeTypes.Sort();
            }
        }

        #endregion

        #region Internal Methods

        internal static Entity Create(EntityElement value)
        {
            Entity entity = new Entity();

            entity.Id = value.Id;
            entity.Name = value.Name;
            entity.TableName = value.TableName;
            entity.CustomNavigateUrl = value.Hierarchy.CustomNavigateUrl;
            entity.CustomRootNodeText = value.Hierarchy.CustomRootNodeText;
            entity.EnableHierarchy = value.Hierarchy.Enabled;
            entity.EnableRootNodeSelection = value.Hierarchy.EnableRootNodeSelection;
            entity.HierarchyMaxDepth = value.Hierarchy.MaxDepth;
            entity.HierarchyStartLevel = value.Hierarchy.StartLevel;
            entity.EnableNodeTypesCustomization = value.Hierarchy.EnableNodeTypesCustomization;

            FillEntityCollections(entity, value);

            return entity;
        }

        #endregion

        #region Public Methods

        public EntityNodeTypeCollection GetCustomNodeTypes(Guid organizationId, Guid? instanceId)
        {
            EntityNodeTypeCollection customNodeTypes = new EntityNodeTypeCollection();
            foreach (ClientDataSet.EntityNodeTypeRow entRow in EntityNodeProvider.GetCustomEntityNodeTypesByEntityId(organizationId, instanceId, this.Id).Rows)
            {
                EntityNodeType ent = new EntityNodeType();
                ent.Id = entRow.EntityNodeTypeId;
                ent.OrderNumber = entRow.OrderNumber;
                ent.Name = entRow.Name;
                customNodeTypes.Add(ent);
            }
            customNodeTypes.Sort();
            return customNodeTypes;
        }

        public static string GetEntityNodePath(Guid entityNodeId, int maxLength, bool highlightLastNode)
        {
            return GetEntityNodePath(entityNodeId, TrimSide.Left, ">", maxLength, highlightLastNode);
        }

        public static string GetEntityNodePath(Guid entityNodeId, TrimSide trimSide, string delimiter, int maxLength, bool highlightLastNode)
        {
            string result = GetEntityNodePath(entityNodeId);
            result = Support.Trim(result.Split('>'), trimSide, delimiter, maxLength, highlightLastNode);

            return result;
        }

        public static string GetEntityNodePath(Guid entityNodeId)
        {
            string result = string.Empty;

            ClientDataSet.EntityNodeRow row = EntityNodeProvider.GetEntityNode(entityNodeId);
            if (row != null)
                result = row.FullPath;

            return result;
        }

        public static ArrayList GetEntityNodePath(string[] entityNodeIds)
        {
            ArrayList result = new ArrayList();

            if (entityNodeIds != null)
            {
                foreach (string guid in entityNodeIds)
                {
                    result.Add(GetEntityNodePath(new Guid(guid)));
                }
            }

            return result;
        }

        public static ArrayList GetEntityNodePath(string[] entityNodeIds, TrimSide trimSide, string delimiter, int maxLength, bool highlightLastNode)
        {
            ArrayList result = new ArrayList();

            if (entityNodeIds != null)
            {
                foreach (string guid in entityNodeIds)
                {
                    result.Add(Support.Trim(GetEntityNodePath(new Guid(guid)).Split('>'), trimSide, delimiter, maxLength, highlightLastNode));
                }
            }

            return result;
        }

        public static ArrayList GetEntityNodePath(string[] entityNodeIds, int maxLength, bool highlightLastNode)
        {
            return GetEntityNodePath(entityNodeIds, TrimSide.Left, ">", maxLength, highlightLastNode);
        }

        public void LoadCustomFields(Guid organizationId)
        {
            this.LoadCustomFields(organizationId, null, null);
        }

        public void LoadCustomFields(Guid organizationId, string localEntityId)
        {
            this.LoadCustomFields(organizationId, null, localEntityId);
        }

        public void LoadCustomFields(Guid organizationId, Guid? instanceId)
        {
            this.LoadCustomFields(organizationId, instanceId, null);
        }

        public void LoadCustomFields(Guid organizationId, Guid? instanceId, string localEntityId)
        {
            bool loadFields = false;
            bool loadFieldsValues = false;
            if (organizationId != m_OrganizationId)
                loadFields = true;
            if (instanceId != m_InstanceId)
                loadFields = true;
            if (string.Compare(localEntityId, m_LocalEntityId, StringComparison.Ordinal) != 0)
                loadFieldsValues = true;

            m_OrganizationId = organizationId;
            m_InstanceId = instanceId;
            m_LocalEntityId = localEntityId;

            if (loadFields || (this.CustomFields.Count == 0))
            {
                m_CustomFields = null;
                EntityFieldProvider.LoadEntityCustomFields(this, organizationId, instanceId, localEntityId);
            }
            else if (loadFieldsValues)
                EntityFieldProvider.LoadEntityCustomFieldsValues(this, organizationId, localEntityId);
        }

        public void SaveCustomFields(Guid organizationId, string localEntityId)
        {
            EntityFieldProvider.SaveEntityCustomFields(this, organizationId, localEntityId);
        }

        #endregion
    }

    [Serializable]
    public class EntityCollection : Collection<Entity>
    {
        #region Private Properties

        private List<Entity> ItemList
        {
            get { return base.Items as List<Entity>; }
        }

        #endregion

        #region Public Properties

        public Entity this[string name]
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
                delegate(Entity entity)
                {
                    return (string.Compare(entity.Name, value, StringComparison.Ordinal) == 0);
                });

            if (index == -1)
            {
                object obj = Support.ConvertStringToType(value, typeof(Guid));
                if (obj != null)
                {
                    Guid id = (Guid)obj;

                    index = this.ItemList.FindIndex(
                        delegate(Entity entity)
                        {
                            return (entity.Id == id);
                        });
                }
            }

            return index;
        }

        #endregion

        #region Internal Methods

        internal static EntityCollection Load()
        {
            EntityCollection entities = new EntityCollection();

            foreach (EntityElement element in FrameworkConfiguration.Current.Entities)
            {
                Entity entity = Entity.Create(element);
                if (entity != null) entities.Add(entity);
            }

            return entities;
        }

        #endregion

        #region Public Methods

        public ReadOnlyCollection<Entity> FindAllByHierarchyStartLevel(EntityLevel hierarchyStartLevel)
        {
            return this.ItemList.FindAll(
                delegate(Entity entity)
                {
                    return entity.HierarchyStartLevel == hierarchyStartLevel;
                }).AsReadOnly();
        }

        public ReadOnlyCollection<Entity> FindAllByEnableHierarchy(bool enableHierarchy)
        {
            return this.ItemList.FindAll(
               delegate(Entity entity)
               {
                   return entity.EnableHierarchy == enableHierarchy;
               }).AsReadOnly();
        }

        #endregion
    }
}
