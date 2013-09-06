CREATE PROCEDURE [dbo].[Mc_GetEntityNodeType]
(
	@EntityNodeTypeId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT EntityNodeTypeId, EntityId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted
	FROM [dbo].[Mc_EntityNodeType]
	WHERE (EntityNodeTypeId = @EntityNodeTypeId);
END
