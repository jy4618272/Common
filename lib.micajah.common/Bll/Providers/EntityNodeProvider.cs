using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using Micajah.Common.Application;
using Micajah.Common.Dal;
using Micajah.Common.Dal.TableAdapters;
using Micajah.Common.Security;

namespace Micajah.Common.Bll.Providers
{
    [DataObjectAttribute(true)]
    public static class EntityNodeProvider
    {
        #region Members

        private static OrganizationDataSetTableAdapters adapter;

        #endregion

        #region Public Properties

        public static OrganizationDataSetTableAdapters Adapter
        {
            get
            {
                if (adapter == null)
                {
                    UserContext user = UserContext.Current;
                    if (user != null)
                        adapter = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(user.SelectedOrganization.OrganizationId);
                }
                return adapter;
            }
        }

        #endregion

        #region Public Methods

        public static void CopyEntityNode(Guid organizationId, Guid? instanceId, Guid sourceId, Guid targetId, EntityLevel level)
        {
            OrganizationDataSet dataSet = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            if (dataSet != null)
            {
                OrganizationDataSet.EntityNodeRow source = dataSet.EntityNode.FindByEntityNodeId(sourceId);
                if (source != null)
                    InsertEntityNode(organizationId, instanceId, source.Name, source.EntityNodeTypeId, source.EntityId, targetId, level);
            }
        }

        public static void UpdateEntityNodePath(Guid entityNodeId, string fullPath)
        {
            UserContext user = UserContext.Current;
            if (user != null)
            {
                OrganizationDataSet.EntityNodeRow entityNode = user.SelectedOrganization.DataSet.EntityNode.FindByEntityNodeId(entityNodeId);
                if (entityNode != null)
                {
                    entityNode.FullPath = fullPath;
                    if (!entityNode.IsParentEntityNodeIdNull() && entityNode.ParentEntityNodeId == Guid.Empty)
                        entityNode.SetParentEntityNodeIdNull();

                    if (Adapter != null) Adapter.EntityNodeTableAdapter.Update(entityNode);
                }
            }
        }

        public static void MergeEntityNode(Guid sourceId, Guid targetId)
        {
            UserContext user = UserContext.Current;
            if (user != null)
            {
                OrganizationDataSet.EntityNodeRow source = user.SelectedOrganization.DataSet.EntityNode.FindByEntityNodeId(sourceId);
                OrganizationDataSet.EntityNodeRow dest = user.SelectedOrganization.DataSet.EntityNode.FindByEntityNodeId(targetId);
                if (source != null && dest != null)
                {
                    dest.Name = source.Name;
                    source.Deleted = true;

                    if (!dest.IsParentEntityNodeIdNull() && dest.ParentEntityNodeId == Guid.Empty)
                        dest.SetParentEntityNodeIdNull();

                    if (!source.IsParentEntityNodeIdNull() && source.ParentEntityNodeId == Guid.Empty)
                        source.SetParentEntityNodeIdNull();

                    if (Adapter != null) Adapter.EntityNodeTableAdapter.Update(dest);
                    if (Adapter != null) Adapter.EntityNodeTableAdapter.Update(source);
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
            UserContext user = UserContext.Current;
            if (user != null)
            {
                OrganizationDataSet.EntityNodeRow row = user.SelectedOrganization.DataSet.EntityNode.FindByEntityNodeId(entityNodeId);
                if (row != null)
                {
                    if (parentEntityNodeId.HasValue && (parentEntityNodeId.Value != Guid.Empty))
                    {
                        Guid pa = parentEntityNodeId.Value;
                        row.ParentEntityNodeId = pa;
                    }
                    else
                        row.SetParentEntityNodeIdNull();

                    if (Adapter != null) Adapter.EntityNodeTableAdapter.Update(row);
                }
            }
        }

        public static void ChangeParentEntityNodeType(Guid entityId, Guid sourceId, Guid destinationId)
        {
            UserContext user = UserContext.Current;
            if (user != null)
            {
                EntityNodeType source = WebApplication.Entities[entityId.ToString("N")].CustomNodeTypes[sourceId.ToString("N")];
                EntityNodeType dest = WebApplication.Entities[entityId.ToString("N")].CustomNodeTypes[destinationId.ToString("N")];
                int indexDest = WebApplication.Entities[entityId.ToString("N")].CustomNodeTypes.IndexOf(dest);
                WebApplication.Entities[entityId.ToString("N")].CustomNodeTypes.Remove(source);
                WebApplication.Entities[entityId.ToString("N")].CustomNodeTypes.Insert(indexDest, source);
                int order = 1;
                OrganizationDataSet.EntityNodeTypeRow entRow;
                foreach (EntityNodeType ent in WebApplication.Entities[entityId.ToString("N")].CustomNodeTypes)
                {
                    entRow = user.SelectedOrganization.DataSet.EntityNodeType.FindByEntityNodeTypeId(ent.Id);
                    entRow.OrderNumber = order;
                    if (Adapter != null) Adapter.EntityNodeTypeTableAdapter.Update(entRow);
                    order++;
                }
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static OrganizationDataSet.EntityNodeRow GetEntityNode(Guid entityNodeId)
        {
            UserContext user = UserContext.Current;
            // TODO: change this function after Adapter update.
            if (user != null)
            {
                if (Adapter != null && user.SelectedOrganization.DataSet.EntityNode.Count == 0)
                    Adapter.EntityNodeTableAdapter.Fill(user.SelectedOrganization.DataSet.EntityNode);
                return user.SelectedOrganization.DataSet.EntityNode.FindByEntityNodeId(entityNodeId);
            }
            return null;
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
        public static OrganizationDataSet.EntityNodeTypeRow GetCustomEntityNodeType(Guid entityNodeTypeId)
        {
            UserContext user = UserContext.Current;
            if (user != null)
            {
                return user.SelectedOrganization.DataSet.EntityNodeType.FindByEntityNodeTypeId(entityNodeTypeId);
            }
            return null;
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
                                OrganizationDataSet.EntityNodeRow row = EntityNodeProvider.GetEntityNode((Guid)value);
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
            UserContext user = UserContext.Current;
            if (user != null)
            {
                OrganizationDataSet.EntityNodeRow row = user.SelectedOrganization.DataSet.EntityNode.FindByEntityNodeId(entityNodeId);
                if (row != null)
                {
                    row.EntityNodeTypeId = entityNodeTypeId;
                    if (row.IsParentEntityNodeIdNull() || row.ParentEntityNodeId == Guid.Empty)
                        row.SetParentEntityNodeIdNull();
                    if (Adapter != null) Adapter.EntityNodeTableAdapter.Update(row);
                }
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateEntityName(Guid entityNodeId, string name)
        {
            UserContext user = UserContext.Current;
            if (user != null)
            {
                OrganizationDataSet.EntityNodeRow row = user.SelectedOrganization.DataSet.EntityNode.FindByEntityNodeId(entityNodeId);
                if (row != null)
                {
                    row.Name = name;
                    if (row.IsParentEntityNodeIdNull() || row.ParentEntityNodeId == Guid.Empty)
                        row.SetParentEntityNodeIdNull();
                    if (Adapter != null) Adapter.EntityNodeTableAdapter.Update(row);
                }
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateEntityNodeTypeName(Guid entityId, Guid entityNodeTypeId, string name)
        {
            UserContext user = UserContext.Current;
            if (user != null)
            {
                OrganizationDataSet.EntityNodeTypeRow row = user.SelectedOrganization.DataSet.EntityNodeType.FindByEntityNodeTypeId(entityNodeTypeId);
                if (row != null)
                {
                    row.Name = name;
                    WebApplication.Entities[entityId.ToString("N")].CustomNodeTypes[entityNodeTypeId.ToString("N")].Name = name;
                    if (Adapter != null) Adapter.EntityNodeTypeTableAdapter.Update(row);
                }
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateEntityNodeType(Guid organizationId, Guid entityNodeTypeId, string name, int orderNumber)
        {
            OrganizationDataSet dataSet = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            if (dataSet != null)
            {
                OrganizationDataSet.EntityNodeTypeRow row = dataSet.EntityNodeType.FindByEntityNodeTypeId(entityNodeTypeId);
                if (row != null)
                {
                    row.Name = name;
                    row.OrderNumber = orderNumber;

                    OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
                    if (adapters != null) adapters.EntityNodeTypeTableAdapter.Update(row);
                }
            }
        }

        public static Guid InsertEntityNode(Guid organizationId, Guid? instanceId, string name, Guid entityNodeTypeId, Guid entityId, Guid parentEntityNodeId, EntityLevel level)
        {
            OrganizationDataSet dataSet = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            if (dataSet != null)
            {
                OrganizationDataSet.EntityNodeDataTable table = dataSet.EntityNode;
                OrganizationDataSet.EntityNodeRow row = table.NewEntityNodeRow();
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

                OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
                if (adapters != null) adapters.EntityNodeTableAdapter.Update(row);

                return row.EntityNodeId;
            }
            return Guid.Empty;
        }

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertEntityNodeType(Guid organizationId, Guid? instanceId, string name, Guid entityId, int orderNumber)
        {
            OrganizationDataSet dataSet = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            if (dataSet != null)
            {
                Entity entity = WebApplication.Entities[entityId.ToString("N")];
                OrganizationDataSet.EntityNodeTypeDataTable table = dataSet.EntityNodeType;
                OrganizationDataSet.EntityNodeTypeRow row = table.NewEntityNodeTypeRow();
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

                OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
                if (adapters != null) adapters.EntityNodeTypeTableAdapter.Update(row);

                return row.EntityNodeTypeId;
            }
            return Guid.Empty;
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEntityNodeType(Guid entityNodeTypeId)
        {
            UserContext user = UserContext.Current;
            if (user != null)
            {
                OrganizationDataSet.EntityNodeTypeRow row = user.SelectedOrganization.DataSet.EntityNodeType.FindByEntityNodeTypeId(entityNodeTypeId);
                if (row != null)
                {
                    row.Deleted = true;
                    if (Adapter != null) Adapter.EntityNodeTypeTableAdapter.Update(row);
                }
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEntityNode(Guid entityNodeId)
        {
            DeleteEntityNode(entityNodeId, null);
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEntityNode(Guid entityNodeId, OrganizationDataSet.EntityNodeDataTable table)
        {
            UserContext user = UserContext.Current;
            if (user != null)
            {
                OrganizationDataSet.EntityNodeRow row = user.SelectedOrganization.DataSet.EntityNode.FindByEntityNodeId(entityNodeId);
                if (row != null)
                {
                    row.Deleted = true;
                    if (row.IsParentEntityNodeIdNull() || row.ParentEntityNodeId == Guid.Empty)
                        row.SetParentEntityNodeIdNull();

                    if (Adapter != null) Adapter.EntityNodeTableAdapter.Update(row);

                    if (table == null)
                    {
                        table = user.SelectedOrganization.DataSet.EntityNode;
                        if (Adapter != null) Adapter.EntityNodeTableAdapter.Fill(table);
                    }
                    foreach (OrganizationDataSet.EntityNodeRow rowEntityNode in table.Select(string.Concat(table.ParentEntityNodeIdColumn.ColumnName, " = '", entityNodeId.ToString(), "' AND ", table.DeletedColumn.ColumnName, " = 'false'")))
                        DeleteEntityNode(rowEntityNode.EntityNodeId, table);
                }
            }
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
            else return null;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static OrganizationDataSet.EntityNodeDataTable GetEntityNodes(Guid organizationId, Guid nodeTypeId)
        {
            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            OrganizationDataSet.EntityNodeDataTable table = new OrganizationDataSet.EntityNodeDataTable();
            adapters.EntityNodeTableAdapter.Fill(table, 1, nodeTypeId);
            return table;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static OrganizationDataSet.EntityNodeTypeDataTable GetCustomEntityNodeTypesByEntityId(Guid organizationId, Guid? instanceId, Guid entityId)
        {
            OrganizationDataSet.EntityNodeTypeDataTable table = null;
            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            OrganizationDataSet.EntityNodeTypeDataTable originalTable = ds.EntityNodeType;
            table = originalTable.Clone() as OrganizationDataSet.EntityNodeTypeDataTable;
            if (table != null)
            {
                if (adapters != null) adapters.EntityNodeTypeTableAdapter.Fill(originalTable);
                foreach (OrganizationDataSet.EntityNodeTypeRow row in originalTable.Select(string.Concat(table.EntityIdColumn.ColumnName, " = '", entityId.ToString(), "' AND ", table.OrganizationIdColumn.ColumnName, " = '", organizationId.ToString(), "' AND ", table.DeletedColumn.ColumnName, " = 'false'", instanceId.HasValue ? " AND " + table.InstanceIdColumn.ColumnName + " = '" + instanceId.Value.ToString() + "'" : String.Empty)))
                    table.ImportRow(row);
                table.AcceptChanges();
            }
            return table;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetEntityNodeTypesByEntityId(Guid organizationId, Guid? instanceId, Guid entityId)
        {
            OrganizationDataSet.EntityNodeTypeDataTable table = null;
            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            OrganizationDataSet.EntityNodeTypeDataTable originalTable = ds.EntityNodeType;
            table = originalTable.Clone() as OrganizationDataSet.EntityNodeTypeDataTable;
            if (table != null)
            {
                if (adapters != null) adapters.EntityNodeTypeTableAdapter.Fill(originalTable);
                foreach (OrganizationDataSet.EntityNodeTypeRow row in originalTable.Select(string.Concat(table.EntityIdColumn.ColumnName, " = '", entityId.ToString(), "' AND ", table.OrganizationIdColumn.ColumnName, " = '", organizationId.ToString(), "' AND ", table.DeletedColumn.ColumnName, " = 'false'", instanceId.HasValue ? " AND " + table.InstanceIdColumn.ColumnName + " = '" + instanceId.Value.ToString() + "'" : String.Empty)))
                    table.ImportRow(row);
                table.AcceptChanges();
            }

            OrganizationDataSet.EntityNodeTypeRow entRow;
            foreach (EntityNodeType ent in WebApplication.Entities[entityId.ToString("N")].NodeTypes)
            {
                entRow = table.NewEntityNodeTypeRow();
                entRow.EntityNodeTypeId = ent.Id;
                entRow.EntityId = entityId;
                entRow.Name = ent.Name;
                entRow.OrderNumber = ent.OrderNumber;
                table.AddEntityNodeTypeRow(entRow);
            }
            table.DefaultView.Sort = "OrderNumber ASC";
            return table.DefaultView;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static OrganizationDataSet.EntityNodeDataTable GetEntityNodesTree(Guid organizationId, Guid? instanceId, Guid entityId, string entityName)
        {
            OrganizationDataSet.EntityNodeDataTable table = null;
            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            string customRootNodeText = WebApplication.Entities[entityId.ToString()].CustomRootNodeText;

            OrganizationDataSet.EntityNodeDataTable originalTable = ds.EntityNode;
            table = originalTable.Clone() as OrganizationDataSet.EntityNodeDataTable;
            if (table != null)
            {
                OrganizationDataSet.EntityNodeRow rootRow = table.NewEntityNodeRow();
                rootRow.EntityNodeId = Guid.Empty;
                if (!string.IsNullOrEmpty(customRootNodeText))
                    rootRow.Name = customRootNodeText.Replace("#organizationName#", UserContext.Current.SelectedOrganization.Name);
                else
                    rootRow.Name = entityName;
                rootRow.EntityId = entityId;
                rootRow.SetParentEntityNodeIdNull();
                table.AddEntityNodeRow(rootRow);

                if (adapters != null) adapters.EntityNodeTableAdapter.Fill(originalTable);
                foreach (OrganizationDataSet.EntityNodeRow row in originalTable.Select(string.Concat(table.EntityIdColumn.ColumnName, " = '", entityId.ToString(), "' AND ", table.OrganizationIdColumn.ColumnName, " = '", organizationId.ToString(), "' AND ", table.OrganizationIdColumn.ColumnName, " = '", organizationId.ToString(), "' AND ", table.DeletedColumn.ColumnName, " = 'false'", instanceId.HasValue ? " AND " + table.InstanceIdColumn.ColumnName + " = '" + instanceId.Value.ToString() + "'" : String.Empty)))
                {
                    if (row.IsParentEntityNodeIdNull())
                        row.ParentEntityNodeId = Guid.Empty;
                    table.ImportRow(row);
                }

                table.AcceptChanges();
            }
            return table;
        }

        #endregion
    }
}
