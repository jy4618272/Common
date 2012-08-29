using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    public class EntityNodesRelatedEntityNodesTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the EntityFieldTableAdapter class.
        /// </summary>
        public EntityNodesRelatedEntityNodesTableAdapter()
        {
            #region TableMapping

            TableName = TableName.EntityNodesRelatedEntityNodes;
            TableMapping.ColumnMappings.Add("EntityNodesRelatedEntityNodesId", "EntityNodesRelatedEntityNodesId");
            TableMapping.ColumnMappings.Add("EntityNodeId", "EntityNodeId");
            TableMapping.ColumnMappings.Add("RelatedEntityNodeId", "RelatedEntityNodeId");
            TableMapping.ColumnMappings.Add("EntityId", "EntityId");
            TableMapping.ColumnMappings.Add("RelationType", "RelationType");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertEntityNodesRelatedEntityNodes";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@EntityNodesRelatedEntityNodesId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityNodesRelatedEntityNodesId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@EntityNodeId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityNodeId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@EntityId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@RelationType", SqlDbType.Int, 16, ParameterDirection.Input, 0, 0, "RelationType", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@RelatedEntityNodeId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RelatedEntityNodeId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateEntityNodesRelatedEntityNodes";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@EntityNodesRelatedEntityNodesId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityNodesRelatedEntityNodesId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@EntityNodeId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityNodeId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@EntityId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@RelationType", SqlDbType.Int, 16, ParameterDirection.Input, 0, 0, "RelationType", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@RelatedEntityNodeId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "RelatedEntityNodeId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommands

            SelectCommand.CommandText = "dbo.Mc_GetAllEntityNodesRelatedEntityNodes";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@EntityNodeId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityNodeId", DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@EntityId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityId", DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetEntityNodesRelatedEntityNodes"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@EntityNodesRelatedEntityNodesId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityNodesRelatedEntityNodesId", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            #endregion

            #region DeleteCommand

            DeleteCommand.CommandText = "dbo.Mc_DeleteEntityNodesRelatedEntityNodes";
            DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@EntityNodesRelatedEntityNodesId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "EntityNodesRelatedEntityNodesId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion
        }

        #endregion
    }
}
