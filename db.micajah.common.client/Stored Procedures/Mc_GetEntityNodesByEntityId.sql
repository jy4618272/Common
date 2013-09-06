CREATE PROCEDURE [dbo].[Mc_GetEntityNodesByEntityId]
(
	@EntityId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT EntityNodeId, ParentEntityNodeId, [Name], OrderNumber, OrganizationId, InstanceId, EntityId, EntityNodeTypeId, SubEntityId, SubEntityLocalId, FullPath, Deleted
	FROM [dbo].[Mc_EntityNode]
	WHERE (EntityId = @EntityId) AND (OrganizationId = @OrganizationId) AND ((@InstanceId IS NULL AND InstanceId IS NULL) OR (InstanceId = @InstanceId)) AND (Deleted = 0)
	ORDER BY [Name];
END