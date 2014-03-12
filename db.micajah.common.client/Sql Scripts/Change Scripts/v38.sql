BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNodesRelatedEntityNodes_Mc_EntityNode1')
      ALTER TABLE [dbo].[Mc_EntityNodesRelatedEntityNodes] DROP CONSTRAINT [FK_Mc_EntityNodesRelatedEntityNodes_Mc_EntityNode1]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE TABLE [dbo].[tmp_Mc_EntityNodesRelatedEntityNodes] (
   [EntityNodesRelatedEntityNodesId] [uniqueidentifier] NOT NULL,
   [EntityNodeId] [uniqueidentifier] NOT NULL,
   [RelatedEntityNodeId] [uniqueidentifier] NOT NULL,
   [EntityId] [uniqueidentifier] NOT NULL,
   [RelationType] [int] NOT NULL
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP TABLE [dbo].[Mc_EntityNodesRelatedEntityNodes]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   EXEC sp_rename N'[dbo].[tmp_Mc_EntityNodesRelatedEntityNodes]', N'Mc_EntityNodesRelatedEntityNodes'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_EntityNodesRelatedEntityNodes] ADD CONSTRAINT [PK_Mc_EntityNodesRelatedEntityNodes] PRIMARY KEY CLUSTERED ([EntityNodesRelatedEntityNodesId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('ALTER PROCEDURE [dbo].[Mc_GetAllEntityNodesRelatedEntityNodes]
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
	WHERE enren.EntityNodeId = @EntityNodeId AND (en.OrganizationId = @OrganizationId OR en.OrganizationId is null)
		AND enren.EntityId = @EntityId

	SET @Err = @@Error

	RETURN @Err
END')


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('ALTER PROCEDURE [dbo].[Mc_GetEntityNodesRelatedEntityNodes]
	@EntityNodesRelatedEntityNodesId uniqueidentifier	
AS
BEGIN

	SET NOCOUNT ON
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
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('ALTER PROCEDURE [dbo].[Mc_InsertEntityNodesRelatedEntityNodes]
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
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('ALTER PROCEDURE [dbo].[Mc_UpdateEntityNodesRelatedEntityNodes]
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
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION