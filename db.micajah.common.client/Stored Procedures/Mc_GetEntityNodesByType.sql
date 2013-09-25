CREATE PROCEDURE [dbo].[Mc_GetEntityNodesByType]
(
	@EntityNodeTypeId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT EntityNodeId, ParentEntityNodeId, [Name], OrderNumber, OrganizationId, InstanceId, EntityId, EntityNodeTypeId, SubEntityId, SubEntityLocalId, FullPath, Deleted
	FROM [dbo].[Mc_EntityNode]
	WHERE (EntityNodeTypeId = @EntityNodeTypeId) AND (Deleted = 0)
	ORDER BY [Name];
END