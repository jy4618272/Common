CREATE PROCEDURE [dbo].[Mc_GetEntityNodeTypes]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT EntityNodeTypeId, EntityId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted
	FROM [dbo].[Mc_EntityNodeType]
	WHERE (Deleted = 0);
END
