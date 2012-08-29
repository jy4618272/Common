using System;
using System.ComponentModel;
using Micajah.Common.Application;
using Micajah.Common.Dal;
using Micajah.Common.Dal.TableAdapters;
using Micajah.Common.Security;

namespace Micajah.Common.Bll.Providers
{
    [DataObjectAttribute(true)]
    public static class EntityNodesRelatedEntityNodesProvider
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

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static OrganizationDataSet.EntityNodesRelatedEntityNodesDataTable GetAllEntityNodesRelatedEntityNodes(Guid organizationId, Guid entityNodeId, Guid entityId)
        {
            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            adapters.EntityNodesRelatedEntityNodesTableAdapter.SelectCommand.Parameters["@EntityNodeId"].Value = entityNodeId;
            adapters.EntityNodesRelatedEntityNodesTableAdapter.SelectCommand.Parameters["@EntityId"].Value = entityId;
            adapters.EntityNodesRelatedEntityNodesTableAdapter.SelectCommand.Parameters["@OrganizationId"].Value = organizationId;
            adapters.EntityNodesRelatedEntityNodesTableAdapter.Fill(ds.EntityNodesRelatedEntityNodes);
            return ds.EntityNodesRelatedEntityNodes;

        }

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void InsertEntityNodesRelatedEntityNodes(Guid entityNodeId, Guid relatedEntityNodeId, Guid entityId, RelationType relationType, Guid organizationId)
        {
            UpdateEntityNodesRelatedEntityNodes(Guid.NewGuid(), entityNodeId, relatedEntityNodeId, entityId, relationType, organizationId);
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteAll(Guid organizationId, Guid entityNodeId, Guid entityId)
        {
            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            OrganizationDataSet.EntityNodesRelatedEntityNodesDataTable dt = GetAllEntityNodesRelatedEntityNodes(organizationId, entityNodeId, entityId);
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
            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            OrganizationDataSetTableAdapters adapters = WebApplication.GetOrganizationDataSetTableAdaptersByOrganizationId(organizationId);
            OrganizationDataSet.EntityNodesRelatedEntityNodesDataTable table = new OrganizationDataSet.EntityNodesRelatedEntityNodesDataTable();
            adapters.EntityNodesRelatedEntityNodesTableAdapter.Fill(table, 1, entityNodesRelatedEntityNodesId);

            OrganizationDataSet.EntityNodesRelatedEntityNodesRow row = null;
            if (table.Count > 0) row = table[0];

            if (row == null)
            {
                row = ds.EntityNodesRelatedEntityNodes.NewEntityNodesRelatedEntityNodesRow();
                row.EntityNodesRelatedEntityNodesId = entityNodesRelatedEntityNodesId;
            }
            row.EntityNodeId = entityNodeId;
            row.RelatedEntityNodeId = relatedEntityNodeId;
            row.EntityId = entityId;
            row.RelationType = (int)relationType;

            if (row.RowState == System.Data.DataRowState.Detached)
            {
                ds.EntityNodesRelatedEntityNodes.AddEntityNodesRelatedEntityNodesRow(row);
                adapters.EntityNodesRelatedEntityNodesTableAdapter.Update(ds);
            }
            else
            {
                row.AcceptChanges();
                row.SetModified();
                adapters.EntityNodesRelatedEntityNodesTableAdapter.Update(row);
            }
        }

        #endregion
    }
}
