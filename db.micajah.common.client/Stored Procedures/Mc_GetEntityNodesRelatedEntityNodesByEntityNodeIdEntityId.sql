CREATE PROCEDURE [dbo].[Mc_GetEntityNodesRelatedEntityNodesByEntityNodeIdEntityId]
(
	@EntityNodeId uniqueidentifier,
	@EntityId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [EntityNodesRelatedEntityNodesId], enren.[EntityNodeId], [RelatedEntityNodeId], enren.[EntityId], [RelationType]
	FROM [Mc_EntityNodesRelatedEntityNodes] AS enren
	LEFT JOIN Mc_EntityNode AS en 
		ON en.EntityNodeId = enren.RelatedEntityNodeId
	WHERE enren.EntityNodeId = @EntityNodeId
		AND enren.EntityId = @EntityId AND (en.Deleted = 0 OR RelatedEntityNodeId = '00000000-0000-0000-0000-000000000000');
END
