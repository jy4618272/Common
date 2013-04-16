CREATE PROCEDURE [dbo].[Mc_GetEntityNodesRelatedEntityNodes]
	@EntityNodesRelatedEntityNodesId uniqueidentifier	
AS
BEGIN
	SET NOCOUNT OFF;
	DECLARE @Err int

	SELECT
		[EntityNodesRelatedEntityNodesId],
		[EntityNodeId],
		[EntityId],
		[RelatedEntityNodeId],
		[RelationType]
	FROM [Mc_EntityNodesRelatedEntityNodes]
	WHERE [EntityNodesRelatedEntityNodesId] = @EntityNodesRelatedEntityNodesId

	SET @Err = @@Error

	RETURN @Err
END
