CREATE PROCEDURE [dbo].[Mc_GetEntityNodesByParentEntityNodeId]
(
	@ParentEntityNodeId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT EntityNodeId, ParentEntityNodeId, [Name], OrderNumber, OrganizationId, InstanceId, EntityId, EntityNodeTypeId, SubEntityId, SubEntityLocalId, FullPath, Deleted
	FROM [dbo].[Mc_EntityNode]
	WHERE ((@ParentEntityNodeId IS NULL AND ParentEntityNodeId IS NULL) OR (ParentEntityNodeId = @ParentEntityNodeId))
	ORDER BY [Name];
END
