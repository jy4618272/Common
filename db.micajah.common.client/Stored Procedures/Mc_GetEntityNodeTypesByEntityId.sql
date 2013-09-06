CREATE PROCEDURE [dbo].[Mc_GetEntityNodeTypesByEntityId]
(
	@EntityId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT EntityNodeTypeId, EntityId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted
	FROM [dbo].[Mc_EntityNodeType]
	WHERE (EntityId = @EntityId) AND (OrganizationId = @OrganizationId) AND ((@InstanceId IS NULL AND InstanceId IS NULL) OR (InstanceId = @InstanceId)) AND (Deleted = 0);
END
