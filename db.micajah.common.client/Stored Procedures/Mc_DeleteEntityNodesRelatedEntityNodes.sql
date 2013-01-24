CREATE PROCEDURE [dbo].[Mc_DeleteEntityNodesRelatedEntityNodes]
(
	@EntityNodesRelatedEntityNodesId uniqueidentifier
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DELETE
	FROM [Mc_EntityNodesRelatedEntityNodes]
	WHERE
		[EntityNodesRelatedEntityNodesId] = @EntityNodesRelatedEntityNodesId
	SET @Err = @@Error

	RETURN @Err
END