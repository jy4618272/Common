CREATE PROCEDURE [dbo].[Mc_InsertEntityNodesRelatedEntityNodes]
(
	@EntityNodesRelatedEntityNodesId uniqueidentifier,
	@EntityNodeId uniqueidentifier,	
	@RelatedEntityNodeId uniqueidentifier,
	@EntityId uniqueidentifier,
	@RelationType int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	IF @EntityNodesRelatedEntityNodesId IS NULL
		 SET @EntityNodesRelatedEntityNodesId = NEWID()

	SET @Err = @@Error

	IF (@Err <> 0)
	    RETURN @Err


	INSERT
	INTO [Mc_EntityNodesRelatedEntityNodes]
	(
		[EntityNodesRelatedEntityNodesId],
		[EntityNodeId],
		[RelatedEntityNodeId],
		[EntityId],
		[RelationType]
	)
	VALUES
	(
		@EntityNodesRelatedEntityNodesId,
		@EntityNodeId,
		@RelatedEntityNodeId,
		@EntityId,
		@RelationType
	)

	SET @Err = @@Error


	RETURN @Err
END