CREATE PROCEDURE [dbo].[Mc_UpdateEntityNodesRelatedEntityNodes]
(
	@EntityNodesRelatedEntityNodesId uniqueidentifier,
	@EntityNodeId uniqueidentifier,
	@RelatedEntityNodeId uniqueidentifier,
	@EntityId uniqueidentifier,
	@RelationType int
)
AS
BEGIN
	SET NOCOUNT OFF;
	DECLARE @Err int

	UPDATE [Mc_EntityNodesRelatedEntityNodes]
	SET
		[EntityNodeId] = @EntityNodeId,
		[RelatedEntityNodeId] = @RelatedEntityNodeId,
		[EntityId] = @EntityId,
		[RelationType] = @RelationType
	WHERE
		[EntityNodesRelatedEntityNodesId] = @EntityNodesRelatedEntityNodesId

	SET @Err = @@Error

	RETURN @Err
END
