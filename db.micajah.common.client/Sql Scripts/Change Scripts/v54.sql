BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetAllEntityNodesRelatedEntityNodes]
	@EntityNodeId uniqueidentifier,
	@EntityId uniqueidentifier,
	@OrganizationId uniqueidentifier
AS
BEGIN

	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		[EntityNodesRelatedEntityNodesId],
		enren.[EntityNodeId],
		[RelatedEntityNodeId],
		enren.[EntityId],
		[RelationType]
	FROM [Mc_EntityNodesRelatedEntityNodes] as enren
	LEFT JOIN Mc_EntityNode as en ON en.EntityNodeId = enren.RelatedEntityNodeId
	WHERE enren.EntityNodeId = @EntityNodeId
		AND enren.EntityId = @EntityId AND (en.Deleted = 0 OR RelatedEntityNodeId = ''00000000-0000-0000-0000-000000000000'')

	SET @Err = @@Error

	RETURN @Err
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEntityNodes]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT EntityNodeId, ParentEntityNodeId, [Name], OrderNumber, OrganizationId, InstanceId, EntityId, EntityNodeTypeId, SubEntityId, SubEntityLocalId, FullPath, Deleted
	FROM [dbo].[Mc_EntityNode]
	WHERE (Deleted = 0)
	ORDER BY [Name]
END')
	
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
