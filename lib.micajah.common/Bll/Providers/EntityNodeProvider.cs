using Micajah.Common.Application;
using Micajah.Common.Dal;
using Micajah.Common.Dal.TableAdapters;
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

        private static void DeleteChildEntityNodes(ClientDataSet.EntityNodeRow parentRow, ClientTableAdapters adapters)
        {
            parentRow.Deleted = true;

            adapters.EntityNodeTableAdapter.Update(parentRow);

            ClientDataSet.EntityNodeDataTable table = new ClientDataSet.EntityNodeDataTable();
            adapters.EntityNodeTableAdapter.Fill(table, 4, parentRow.EntityNodeId);

            foreach (ClientDataSet.EntityNodeRow row in table)
            {
                DeleteChildEntityNodes(row, adapters);
            }
        }

        private static ClientDataSet.EntityNodeRow GetEntityNode(Guid entityNodeId, ClientTableAdapters adapters)
        {
            ClientDataSet.EntityNodeDataTable table = new ClientDataSet.EntityNodeDataTable();
            adapters.EntityNodeTableAdapter.Fill(table, 3, entityNodeId);
            return ((table.Count > 0) ? table[0] : null);
        }

        private static ClientDataSet.EntityNodeTypeRow GetEntityNodeType(Guid entityNodeTypeId, ClientTableAdapters adapters)
        {
            ClientDataSet.EntityNodeTypeDataTable table = new ClientDataSet.EntityNodeTypeDataTable();
            adapters.EntityNodeTypeTableAdapter.Fill(table, 2, entityNodeTypeId);
            return ((table.Count > 0) ? table[0] : null);
        }

        #endregion

        #region Public Methods

        public static void CopyEntityNode(Guid organizationId, Guid? instanceId, Guid sourceId, Guid targetId, EntityLevel level)
        {
            ClientDataSet.EntityNodeRow source = GetEntityNode(sourceId, WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId));
            if (source != null)
                InsertEntityNode(organizationId, instanceId, source.Name, source.EntityNodeTypeId, source.EntityId, targetId, level);
        }

        public static void UpdateEntityNodePath(Guid entityNodeId, string fullPath)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(UserContext.Current.SelectedOrganizationId);

            ClientDataSet.EntityNodeRow row = GetEntityNode(entityNodeId, adapters);
            if (row != null)
            {
                row.FullPath = fullPath;

                adapters.EntityNodeTableAdapter.Update(row);
            }
        }

        public static void MergeEntityNode(Guid sourceId, Guid targetId)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(UserContext.Current.SelectedOrganizationId);

            ClientDataSet.EntityNodeRow sourceRow = GetEntityNode(sourceId, adapters);
            ClientDataSet.EntityNodeRow destRow = GetEntityNode(targetId, adapters);

            if (sourceRow != null && destRow != null)
            {
                destRow.Name = sourceRow.Name;

                sourceRow.Deleted = true;

                adapters.EntityNodeTableAdapter.Update(destRow);
                adapters.EntityNodeTableAdapter.Update(sourceRow);
            }
        }

        /// <summary>
        /// Changes the parent of the specified entity node.
        /// </summary>
        /// <param name="entityNodeId">The identifier of the entity node.</param>
        /// <param name="parentEntityNodeId">The identifier of new parent of the entity node.</param>
        public static void ChangeParentEntityNode(Guid entityNodeId, Guid? parentEntityNodeId)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(UserContext.Current.SelectedOrganizationId);

            ClientDataSet.EntityNodeRow row = GetEntityNode(entityNodeId, adapters);
            if (row != null)
            {
                if (parentEntityNodeId.HasValue && (parentEntityNodeId.Value != Guid.Empty))
                    row.ParentEntityNodeId = parentEntityNodeId.Value;
                else
                    row.SetParentEntityNodeIdNull();

                adapters.EntityNodeTableAdapter.Update(row);
            }
        }

        public static void ChangeParentEntityNodeType(Guid entityId, Guid sourceId, Guid destinationId)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(UserContext.Current.SelectedOrganizationId);

            EntityNodeType source = WebApplication.Entities[entityId.ToString("N")].CustomNodeTypes[sourceId.ToString("N")];
            EntityNodeType dest = WebApplication.Entities[entityId.ToString("N")].CustomNodeTypes[destinationId.ToString("N")];

            int indexDest = WebApplication.Entities[entityId.ToString("N")].CustomNodeTypes.IndexOf(dest);
            WebApplication.Entities[entityId.ToString("N")].CustomNodeTypes.Remove(source);
            WebApplication.Entities[entityId.ToString("N")].CustomNodeTypes.Insert(indexDest, source);

            int order = 1;
            foreach (EntityNodeType ent in WebApplication.Entities[entityId.ToString("N")].CustomNodeTypes)
            {
                ClientDataSet.EntityNodeTypeRow row = GetEntityNodeType(ent.Id, adapters);
                row.OrderNumber = order;

                adapters.EntityNodeTypeTableAdapter.Update(row);

                order++;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.EntityNodeRow GetEntityNode(Guid entityNodeId)
        {
            return GetEntityNode(entityNodeId, WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(UserContext.Current.SelectedOrganizationId));
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static Entity GetEntityNodeType(Guid entityNodeTypeId)
        {
            foreach (Entity ent in WebApplication.Entities)
            {
                if (ent.Id.Equals(entityNodeTypeId)) return ent;
            }
            return null;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.EntityNodeTypeRow GetCustomEntityNodeType(Guid entityNodeTypeId)
        {
            return GetEntityNodeType(entityNodeTypeId, WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(UserContext.Current.SelectedOrganizationId));
        }

        public static string GetEntityValueAndName(Guid entityNodeTypeId, string fieldName, object value)
        {
            foreach (Entity ent in WebApplication.Entities)
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
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(UserContext.Current.SelectedOrganizationId);

            ClientDataSet.EntityNodeRow row = GetEntityNode(entityNodeId, adapters);
            if (row != null)
            {
                row.EntityNodeTypeId = entityNodeTypeId;

                adapters.EntityNodeTableAdapter.Update(row);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateEntityName(Guid entityNodeId, string name)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(UserContext.Current.SelectedOrganizationId);

            ClientDataSet.EntityNodeRow row = GetEntityNode(entityNodeId, adapters);
            if (row != null)
            {
                row.Name = name;

                adapters.EntityNodeTableAdapter.Update(row);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateEntityNodeTypeName(Guid entityId, Guid entityNodeTypeId, string name)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(UserContext.Current.SelectedOrganizationId);

            ClientDataSet.EntityNodeTypeRow row = GetEntityNodeType(entityNodeTypeId, adapters);
            if (row != null)
            {
                row.Name = name;

                adapters.EntityNodeTypeTableAdapter.Update(row);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateEntityNodeType(Guid organizationId, Guid entityNodeTypeId, string name, int orderNumber)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.EntityNodeTypeDataTable table = new ClientDataSet.EntityNodeTypeDataTable();
            adapters.EntityNodeTypeTableAdapter.Fill(table, 2, entityNodeTypeId);

            if (table.Count > 0)
            {
                ClientDataSet.EntityNodeTypeRow row = table[0];
                row.Name = name;
                row.OrderNumber = orderNumber;

                adapters.EntityNodeTypeTableAdapter.Update(row);
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

            WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId).EntityNodeTableAdapter.Update(row);

            return row.EntityNodeId;
        }

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertEntityNodeType(Guid organizationId, Guid? instanceId, string name, Guid entityId, int orderNumber)
        {
            ClientDataSet.EntityNodeTypeDataTable table = new ClientDataSet.EntityNodeTypeDataTable();
            ClientDataSet.EntityNodeTypeRow row = table.NewEntityNodeTypeRow();

            Entity entity = WebApplication.Entities[entityId.ToString("N")];

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

            WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId).EntityNodeTypeTableAdapter.Update(row);

            return row.EntityNodeTypeId;
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEntityNodeType(Guid entityNodeTypeId)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(UserContext.Current.SelectedOrganizationId);

            ClientDataSet.EntityNodeTypeRow row = GetEntityNodeType(entityNodeTypeId, adapters);
            if (row != null)
            {
                row.Deleted = true;

                adapters.EntityNodeTypeTableAdapter.Update(row);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEntityNode(Guid entityNodeId)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(UserContext.Current.SelectedOrganizationId);

            ClientDataSet.EntityNodeRow row = GetEntityNode(entityNodeId, adapters);
            if (row != null)
                DeleteChildEntityNodes(row, adapters);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static EntityCollection GetEntityTypes()
        {
            EntityCollection col = new EntityCollection();
            foreach (Entity ent in WebApplication.Entities)
            {
                if (ent.Fields.Count > 0)
                    col.Add(ent);
            }
            return col;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static EntityFieldCollection GetEntityFields(Guid entityTypeId)
        {
            Entity ent = WebApplication.Entities[entityTypeId.ToString()];
            if (ent != null)
                return ent.Fields;
            return null;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.EntityNodeDataTable GetEntityNodes(Guid organizationId, Guid nodeTypeId)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.EntityNodeDataTable table = new ClientDataSet.EntityNodeDataTable();
            adapters.EntityNodeTableAdapter.Fill(table, 1, nodeTypeId);
            return table;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.EntityNodeTypeDataTable GetCustomEntityNodeTypesByEntityId(Guid organizationId, Guid? instanceId, Guid entityId)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.EntityNodeTypeDataTable table = new ClientDataSet.EntityNodeTypeDataTable();
            adapters.EntityNodeTypeTableAdapter.Fill(table, 1, entityId, organizationId, instanceId);
            return table;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetEntityNodeTypesByEntityId(Guid organizationId, Guid? instanceId, Guid entityId)
        {
            ClientDataSet.EntityNodeTypeDataTable table = GetCustomEntityNodeTypesByEntityId(organizationId, instanceId, entityId);

            ClientDataSet.EntityNodeTypeRow row = null;
            foreach (EntityNodeType ent in WebApplication.Entities[entityId.ToString("N")].NodeTypes)
            {
                row = table.NewEntityNodeTypeRow();
                row.EntityNodeTypeId = ent.Id;
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
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.EntityNodeDataTable table = new ClientDataSet.EntityNodeDataTable();
            adapters.EntityNodeTableAdapter.Fill(table, 2, entityId, organizationId, instanceId);

            string customRootNodeText = WebApplication.Entities[entityId.ToString()].CustomRootNodeText;

            ClientDataSet.EntityNodeRow rootRow = table.NewEntityNodeRow();
            rootRow.EntityNodeId = Guid.Empty;
            if (!string.IsNullOrEmpty(customRootNodeText))
                rootRow.Name = customRootNodeText.Replace("#organizationName#", UserContext.Current.SelectedOrganization.Name);
            else
                rootRow.Name = entityName;
            rootRow.EntityId = entityId;
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
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.EntityNodesRelatedEntityNodesDataTable table = new ClientDataSet.EntityNodesRelatedEntityNodesDataTable();
            adapters.EntityNodesRelatedEntityNodesTableAdapter.Fill(table, 0, entityNodeId, entityId, organizationId);
            return table;
        }

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void InsertEntityNodesRelatedEntityNodes(Guid entityNodeId, Guid relatedEntityNodeId, Guid entityId, RelationType relationType, Guid organizationId)
        {
            UpdateEntityNodesRelatedEntityNodes(Guid.NewGuid(), entityNodeId, relatedEntityNodeId, entityId, relationType, organizationId);
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteAllEntityNodesRelatedEntityNodes(Guid organizationId, Guid entityNodeId, Guid entityId)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.EntityNodesRelatedEntityNodesDataTable dt = GetAllEntityNodesRelatedEntityNodes(organizationId, entityNodeId, entityId);
            while (dt.Rows.Count > 0)
            {
                dt.Rows[0].AcceptChanges();
                dt.Rows[0].SetModified();
                dt.Rows[0].Delete();
                adapters.EntityNodesRelatedEntityNodesTableAdapter.Update(dt.Rows[0]);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateEntityNodesRelatedEntityNodes(Guid entityNodesRelatedEntityNodesId, Guid entityNodeId, Guid relatedEntityNodeId, Guid entityId, RelationType relationType, Guid organizationId)
        {
            ClientTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            ClientDataSet.EntityNodesRelatedEntityNodesDataTable table = new ClientDataSet.EntityNodesRelatedEntityNodesDataTable();
            adapters.EntityNodesRelatedEntityNodesTableAdapter.Fill(table, 1, entityNodesRelatedEntityNodesId);

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

            adapters.EntityNodesRelatedEntityNodesTableAdapter.Update(row);
        }

        #endregion
    }
}
