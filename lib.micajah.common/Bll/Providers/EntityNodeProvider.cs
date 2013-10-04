using Micajah.Common.Application;
using Micajah.Common.Dal;
using Micajah.Common.Dal.ClientDataSetTableAdapters;
using Micajah.Common.Security;
using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;

namespace Micajah.Common.Bll.Providers
{
    [DataObjectAttribute(true)]
    public static class EntityNodeProvider
    {
        #region Private Methods

        private static void DeleteChildEntityNodes(ClientDataSet.EntityNodeRow parentRow, Guid organizationId)
        {
            parentRow.Deleted = true;

            ClientDataSet.EntityNodeDataTable table = null;
            using (EntityNodeTableAdapter adapter = new EntityNodeTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                adapter.Update(parentRow);

                table = adapter.GetEntityNodesByParentEntityNodeId(parentRow.EntityNodeId);
            }

            foreach (ClientDataSet.EntityNodeRow row in table)
            {
                DeleteChildEntityNodes(row, organizationId);
            }
        }

        private static ClientDataSet.EntityNodeRow GetEntityNode(Guid entityNodeId, Guid organizationId)
        {
            using (EntityNodeTableAdapter adapter = new EntityNodeTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                ClientDataSet.EntityNodeDataTable table = adapter.GetEntityNode(entityNodeId);
                return ((table.Count > 0) ? table[0] : null);
            }
        }

        private static ClientDataSet.EntityNodeTypeRow GetEntityNodeType(Guid entityNodeTypeId, Guid organizationId)
        {
            using (EntityNodeTypeTableAdapter adapter = new EntityNodeTypeTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                ClientDataSet.EntityNodeTypeDataTable table = adapter.GetEntityNodeType(entityNodeTypeId);
                return ((table.Count > 0) ? table[0] : null);
            }
        }

        #endregion

        #region Public Methods

        public static void CopyEntityNode(Guid organizationId, Guid? instanceId, Guid sourceId, Guid targetId, EntityLevel level)
        {
            ClientDataSet.EntityNodeRow source = GetEntityNode(sourceId, organizationId);
            if (source != null)
                InsertEntityNode(organizationId, instanceId, source.Name, source.EntityNodeTypeId, source.EntityId, targetId, level);
        }

        public static void UpdateEntityNodePath(Guid entityNodeId, string fullPath)
        {
            Guid organizationId = UserContext.Current.OrganizationId;
            ClientDataSet.EntityNodeRow row = GetEntityNode(entityNodeId, organizationId);
            if (row != null)
            {
                row.FullPath = fullPath;

                using (EntityNodeTableAdapter adapter = new EntityNodeTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                {
                    adapter.Update(row);
                }
            }
        }

        public static void MergeEntityNode(Guid sourceId, Guid targetId)
        {
            Guid organizationId = UserContext.Current.OrganizationId;

            ClientDataSet.EntityNodeRow sourceRow = GetEntityNode(sourceId, organizationId);
            ClientDataSet.EntityNodeRow destRow = GetEntityNode(targetId, organizationId);

            if (sourceRow != null && destRow != null)
            {
                destRow.Name = sourceRow.Name;

                sourceRow.Deleted = true;

                using (EntityNodeTableAdapter adapter = new EntityNodeTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                {
                    adapter.Update(destRow);
                    adapter.Update(sourceRow);
                }
            }
        }

        /// <summary>
        /// Changes the parent of the specified entity node.
        /// </summary>
        /// <param name="entityNodeId">The identifier of the entity node.</param>
        /// <param name="parentEntityNodeId">The identifier of new parent of the entity node.</param>
        public static void ChangeParentEntityNode(Guid entityNodeId, Guid? parentEntityNodeId)
        {
            Guid organizationId = UserContext.Current.OrganizationId;

            ClientDataSet.EntityNodeRow row = GetEntityNode(entityNodeId, organizationId);
            if (row != null)
            {
                if (parentEntityNodeId.HasValue && (parentEntityNodeId.Value != Guid.Empty))
                    row.ParentEntityNodeId = parentEntityNodeId.Value;
                else
                    row.SetParentEntityNodeIdNull();

                using (EntityNodeTableAdapter adapter = new EntityNodeTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                {
                    adapter.Update(row);
                }
            }
        }

        public static void ChangeParentEntityNodeType(Guid entityId, Guid sourceId, Guid destinationId)
        {
            Guid organizationId = UserContext.Current.OrganizationId;

            EntityNodeType source = EntityFieldProvider.Entities[entityId.ToString("N")].CustomNodeTypes[sourceId.ToString("N")];
            EntityNodeType dest = EntityFieldProvider.Entities[entityId.ToString("N")].CustomNodeTypes[destinationId.ToString("N")];

            int indexDest = EntityFieldProvider.Entities[entityId.ToString("N")].CustomNodeTypes.IndexOf(dest);
            EntityFieldProvider.Entities[entityId.ToString("N")].CustomNodeTypes.Remove(source);
            EntityFieldProvider.Entities[entityId.ToString("N")].CustomNodeTypes.Insert(indexDest, source);

            using (EntityNodeTypeTableAdapter adapter = new EntityNodeTypeTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                int order = 1;
                foreach (EntityNodeType ent in EntityFieldProvider.Entities[entityId.ToString("N")].CustomNodeTypes)
                {
                    ClientDataSet.EntityNodeTypeRow row = GetEntityNodeType(ent.Id, organizationId);
                    row.OrderNumber = order;

                    adapter.Update(row);

                    order++;
                }
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.EntityNodeRow GetEntityNode(Guid entityNodeId)
        {
            return GetEntityNode(entityNodeId, UserContext.Current.OrganizationId);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static Entity GetEntityNodeType(Guid entityNodeTypeId)
        {
            foreach (Entity ent in EntityFieldProvider.Entities)
            {
                if (ent.Id.Equals(entityNodeTypeId)) return ent;
            }
            return null;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.EntityNodeTypeRow GetCustomEntityNodeType(Guid entityNodeTypeId)
        {
            return GetEntityNodeType(entityNodeTypeId, UserContext.Current.OrganizationId);
        }

        public static string GetEntityValueAndName(Guid entityNodeTypeId, string fieldName, object value)
        {
            foreach (Entity ent in EntityFieldProvider.Entities)
            {
                if (ent.Id.Equals(entityNodeTypeId))
                {
                    EntityField field = ent.Fields[fieldName];
                    if (field != null)
                    {
                        if (field.ListValues != null && field.ListValues.Count > 0)
                        {
                            System.Collections.Generic.Dictionary<string, object[]>.KeyCollection.Enumerator keys = field.ListValues.Keys.GetEnumerator();
                            foreach (object[] obj in field.ListValues.Values)
                            {
                                if (!keys.MoveNext()) break;
                                if (obj[0].Equals(value))
                                    return keys.Current;
                            }
                        }
                        else if (field.DataType.Equals(typeof(Entity)))
                        {
                            if (value is Guid)
                            {
                                ClientDataSet.EntityNodeRow row = EntityNodeProvider.GetEntityNode((Guid)value);
                                if (row != null)
                                    return row.Name;
                            }
                        }
                    }
                }
            }
            return Convert.ToString(value, CultureInfo.CurrentCulture);
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateEntityType(Guid entityNodeId, Guid entityNodeTypeId)
        {
            Guid organizationId = UserContext.Current.OrganizationId;

            ClientDataSet.EntityNodeRow row = GetEntityNode(entityNodeId, organizationId);
            if (row != null)
            {
                row.EntityNodeTypeId = entityNodeTypeId;

                using (EntityNodeTableAdapter adapter = new EntityNodeTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                {
                    adapter.Update(row);
                }
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateEntityName(Guid entityNodeId, string name)
        {
            Guid organizationId = UserContext.Current.OrganizationId;

            ClientDataSet.EntityNodeRow row = GetEntityNode(entityNodeId, organizationId);
            if (row != null)
            {
                row.Name = name;

                using (EntityNodeTableAdapter adapter = new EntityNodeTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                {
                    adapter.Update(row);
                }
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateEntityNodeTypeName(Guid entityId, Guid entityNodeTypeId, string name)
        {
            Guid organizationId = UserContext.Current.OrganizationId;

            ClientDataSet.EntityNodeTypeRow row = GetEntityNodeType(entityNodeTypeId, organizationId);
            if (row != null)
            {
                row.Name = name;

                using (EntityNodeTypeTableAdapter adapter = new EntityNodeTypeTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                {
                    adapter.Update(row);
                }
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateEntityNodeType(Guid organizationId, Guid entityNodeTypeId, string name, int orderNumber)
        {
            using (EntityNodeTypeTableAdapter adapter = new EntityNodeTypeTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                ClientDataSet.EntityNodeTypeDataTable table = adapter.GetEntityNodeType(entityNodeTypeId);
                if (table.Count > 0)
                {
                    ClientDataSet.EntityNodeTypeRow row = table[0];
                    row.Name = name;
                    row.OrderNumber = orderNumber;

                    adapter.Update(row);
                }
            }
        }

        public static Guid InsertEntityNode(Guid organizationId, Guid? instanceId, string name, Guid entityNodeTypeId, Guid entityId, Guid parentEntityNodeId, EntityLevel level)
        {
            ClientDataSet.EntityNodeDataTable table = new ClientDataSet.EntityNodeDataTable();
            ClientDataSet.EntityNodeRow row = table.NewEntityNodeRow();

            row.EntityNodeId = Guid.NewGuid();
            row.Name = name;
            if (level == EntityLevel.Instance)
            {
                if (instanceId.HasValue)
                    row.InstanceId = instanceId.Value;
            }
            row.OrganizationId = organizationId;
            row.EntityNodeTypeId = entityNodeTypeId;
            row.EntityId = entityId;
            row.FullPath = string.Empty;
            if (parentEntityNodeId != Guid.Empty)
                row.ParentEntityNodeId = parentEntityNodeId;
            row.OrderNumber = 0;
            table.AddEntityNodeRow(row);

            using (EntityNodeTableAdapter adapter = new EntityNodeTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                adapter.Update(row);
            }

            return row.EntityNodeId;
        }

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertEntityNodeType(Guid organizationId, Guid? instanceId, string name, Guid entityId, int orderNumber)
        {
            ClientDataSet.EntityNodeTypeDataTable table = new ClientDataSet.EntityNodeTypeDataTable();
            ClientDataSet.EntityNodeTypeRow row = table.NewEntityNodeTypeRow();

            Entity entity = EntityFieldProvider.Entities[entityId.ToString("N")];

            row.EntityNodeTypeId = Guid.NewGuid();
            row.Name = name;
            row.EntityId = entityId;
            if (entity.HierarchyStartLevel == EntityLevel.Instance)
            {
                if (instanceId.HasValue)
                    row.InstanceId = instanceId.Value;
            }
            row.OrganizationId = organizationId;
            if (orderNumber == 0)
                row.OrderNumber = entity.GetCustomNodeTypes(organizationId, instanceId).Count + 1;
            else
                row.OrderNumber = orderNumber;
            table.AddEntityNodeTypeRow(row);

            using (EntityNodeTypeTableAdapter adapter = new EntityNodeTypeTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                adapter.Update(row);
            }

            return row.EntityNodeTypeId;
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEntityNodeType(Guid entityNodeTypeId)
        {
            Guid organizationId = UserContext.Current.OrganizationId;

            ClientDataSet.EntityNodeTypeRow row = GetEntityNodeType(entityNodeTypeId, organizationId);
            if (row != null)
            {
                row.Deleted = true;

                using (EntityNodeTypeTableAdapter adapter = new EntityNodeTypeTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                {
                    adapter.Update(row);
                }
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEntityNode(Guid entityNodeId)
        {
            Guid organizationId = UserContext.Current.OrganizationId;

            ClientDataSet.EntityNodeRow row = GetEntityNode(entityNodeId, organizationId);
            if (row != null)
                DeleteChildEntityNodes(row, organizationId);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static EntityCollection GetEntityTypes()
        {
            EntityCollection col = new EntityCollection();
            foreach (Entity ent in EntityFieldProvider.Entities)
            {
                if (ent.Fields.Count > 0)
                    col.Add(ent);
            }
            return col;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static EntityFieldCollection GetEntityFields(Guid entityTypeId)
        {
            Entity ent = EntityFieldProvider.Entities[entityTypeId.ToString()];
            if (ent != null)
                return ent.Fields;
            return null;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.EntityNodeDataTable GetEntityNodes(Guid organizationId, Guid nodeTypeId)
        {
            using (EntityNodeTableAdapter adapter = new EntityNodeTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetEntityNodesByType(nodeTypeId);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.EntityNodeTypeDataTable GetCustomEntityNodeTypesByEntityId(Guid organizationId, Guid? instanceId, Guid entityId)
        {
            using (EntityNodeTypeTableAdapter adapter = new EntityNodeTypeTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetEntityNodeTypesByEntityId(entityId, organizationId, instanceId);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetEntityNodeTypesByEntityId(Guid organizationId, Guid? instanceId, Guid entityId)
        {
            ClientDataSet.EntityNodeTypeDataTable table = GetCustomEntityNodeTypesByEntityId(organizationId, instanceId, entityId);

            ClientDataSet.EntityNodeTypeRow row = null;
            foreach (EntityNodeType ent in EntityFieldProvider.Entities[entityId.ToString("N")].NodeTypes)
            {
                row = table.NewEntityNodeTypeRow();
                row.EntityNodeTypeId = ent.Id;
                row.OrganizationId = organizationId;
                row.EntityId = entityId;
                row.Name = ent.Name;
                row.OrderNumber = ent.OrderNumber;
                table.AddEntityNodeTypeRow(row);
            }
            table.DefaultView.Sort = "OrderNumber ASC";

            return table.DefaultView;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.EntityNodeDataTable GetEntityNodesTree(Guid organizationId, Guid? instanceId, Guid entityId, string entityName)
        {
            ClientDataSet.EntityNodeDataTable table = null;
            using (EntityNodeTableAdapter adapter = new EntityNodeTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                table = adapter.GetEntityNodesByEntityId(entityId, organizationId, instanceId);
            }

            string customRootNodeText = EntityFieldProvider.Entities[entityId.ToString()].CustomRootNodeText;

            ClientDataSet.EntityNodeRow rootRow = table.NewEntityNodeRow();
            rootRow.EntityNodeId = Guid.Empty;
            if (!string.IsNullOrEmpty(customRootNodeText))
                rootRow.Name = customRootNodeText.Replace("#organizationName#", UserContext.Current.Organization.Name);
            else
                rootRow.Name = entityName;
            rootRow.EntityId = entityId;
            rootRow.OrganizationId = organizationId;
            rootRow.SetParentEntityNodeIdNull();
            table.AddEntityNodeRow(rootRow);

            foreach (ClientDataSet.EntityNodeRow row in table)
            {
                if (row.EntityNodeId != Guid.Empty)
                {
                    if (row.IsParentEntityNodeIdNull())
                        row.ParentEntityNodeId = Guid.Empty;
                }
            }

            table.AcceptChanges();

            return table;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.EntityNodesRelatedEntityNodesDataTable GetAllEntityNodesRelatedEntityNodes(Guid organizationId, Guid entityNodeId, Guid entityId)
        {
            using (EntityNodesRelatedEntityNodesTableAdapter adapter = new EntityNodesRelatedEntityNodesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetEntityNodesRelatedEntityNodesByEntityNodeIdEntityId(entityNodeId, entityId, organizationId);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void InsertEntityNodesRelatedEntityNodes(Guid entityNodeId, Guid relatedEntityNodeId, Guid entityId, RelationType relationType, Guid organizationId)
        {
            UpdateEntityNodesRelatedEntityNodes(Guid.NewGuid(), entityNodeId, relatedEntityNodeId, entityId, relationType, organizationId);
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteAllEntityNodesRelatedEntityNodes(Guid organizationId, Guid entityNodeId, Guid entityId)
        {
            ClientDataSet.EntityNodesRelatedEntityNodesDataTable table = GetAllEntityNodesRelatedEntityNodes(organizationId, entityNodeId, entityId);
            foreach (ClientDataSet.EntityNodesRelatedEntityNodesRow row in table)
            {
                row.Delete();
            }

            using (EntityNodesRelatedEntityNodesTableAdapter adapter = new EntityNodesRelatedEntityNodesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                adapter.Update(table);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateEntityNodesRelatedEntityNodes(Guid entityNodesRelatedEntityNodesId, Guid entityNodeId, Guid relatedEntityNodeId, Guid entityId, RelationType relationType, Guid organizationId)
        {
            using (EntityNodesRelatedEntityNodesTableAdapter adapter = new EntityNodesRelatedEntityNodesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                ClientDataSet.EntityNodesRelatedEntityNodesDataTable table = adapter.GetEntityNodesRelatedEntityNodes(entityNodesRelatedEntityNodesId);
                ClientDataSet.EntityNodesRelatedEntityNodesRow row = null;
                if (table.Count > 0) row = table[0];

                if (row == null)
                {
                    row = table.NewEntityNodesRelatedEntityNodesRow();
                    row.EntityNodesRelatedEntityNodesId = entityNodesRelatedEntityNodesId;
                }
                row.EntityNodeId = entityNodeId;
                row.RelatedEntityNodeId = relatedEntityNodeId;
                row.EntityId = entityId;
                row.RelationType = (int)relationType;

                if (row.RowState == DataRowState.Detached)
                    table.AddEntityNodesRelatedEntityNodesRow(row);

                adapter.Update(row);
            }
        }

        #endregion
    }
}
