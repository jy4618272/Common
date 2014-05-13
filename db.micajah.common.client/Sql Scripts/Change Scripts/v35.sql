BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_EntityNodesRelatedEntityNodes] (
	   [EntityNodesRelatedEntityNodesId] [uniqueidentifier] NOT NULL,
	   [EntityNodeId] [uniqueidentifier] NOT NULL,
	   [RelatedEntityNodeId] [uniqueidentifier] NOT NULL,
	   [RelationType] [int] NOT NULL
	)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityNodesRelatedEntityNodes] ADD CONSTRAINT [PK_Mc_EntityNodesRelatedEntityNodes] PRIMARY KEY CLUSTERED ([EntityNodesRelatedEntityNodesId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP TABLE [dbo].[Mc_EntityNodesUsers]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_DeleteEntityNodesRelatedEntityNodes]
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
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_GetAllEntityNodesRelatedEntityNodes]
	@EntityNodeId uniqueidentifier,
	@OrganizationId uniqueidentifier
AS
BEGIN

	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		[EntityNodesRelatedEntityNodesId],
		en.[EntityNodeId],
		[RelatedEntityNodeId],
		[RelationType]
	FROM [Mc_EntityNodesRelatedEntityNodes] as enren
	LEFT JOIN Mc_EntityNode as en ON en.EntityNodeId = enren.RelatedEntityNodeId
	WHERE enren.EntityNodeId = @EntityNodeId AND en.OrganizationId = @OrganizationId

	SET @Err = @@Error

	RETURN @Err
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_GetEntityNodesRelatedEntityNodes]
	@EntityNodesRelatedEntityNodesId uniqueidentifier	
AS
BEGIN

	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		[EntityNodesRelatedEntityNodesId],
		[EntityNodeId],
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
exec('CREATE PROCEDURE [dbo].[Mc_InsertEntityNodesRelatedEntityNodes]
(
	@EntityNodesRelatedEntityNodesId uniqueidentifier,
	@EntityNodeId uniqueidentifier,
	@RelatedEntityNodeId uniqueidentifier,
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
		[RelationType]
	)
	VALUES
	(
		@EntityNodesRelatedEntityNodesId,
		@EntityNodeId,
		@RelatedEntityNodeId,
		@RelationType
	)

	SET @Err = @@Error


	RETURN @Err
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_UpdateEntityNodesRelatedEntityNodes]
(
	@EntityNodesRelatedEntityNodesId uniqueidentifier,
	@EntityNodeId uniqueidentifier,
	@RelatedEntityNodeId uniqueidentifier,
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
		[RelationType] = @RelationType
	WHERE
		[EntityNodesRelatedEntityNodesId] = @EntityNodesRelatedEntityNodesId


	SET @Err = @@Error


	RETURN @Err
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNodesRelatedEntityNodes_Mc_EntityNode1')
      ALTER TABLE [dbo].[Mc_EntityNodesRelatedEntityNodes] DROP CONSTRAINT [FK_Mc_EntityNodesRelatedEntityNodes_Mc_EntityNode1]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNodesRelatedEntityNodes_Mc_EntityNode1')
      ALTER TABLE [dbo].[Mc_EntityNodesRelatedEntityNodes] ADD CONSTRAINT [FK_Mc_EntityNodesRelatedEntityNodes_Mc_EntityNode1] FOREIGN KEY ([RelatedEntityNodeId]) REFERENCES [dbo].[Mc_EntityNode] ([EntityNodeId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteEntityNodesUsers]') AND type in (N'P', N'PC'))
		DROP PROCEDURE [dbo].[Mc_DeleteEntityNodesUsers]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetAllEntityNodesUsers]') AND type in (N'P', N'PC'))
		DROP PROCEDURE [dbo].[Mc_GetAllEntityNodesUsers]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetEntityNodesUsers]') AND type in (N'P', N'PC'))
		DROP PROCEDURE [dbo].[Mc_GetEntityNodesUsers]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertEntityNodesUsers]') AND type in (N'P', N'PC'))
		DROP PROCEDURE [dbo].[Mc_InsertEntityNodesUsers]	
	
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1	
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateEntityNodesUsers]') AND type in (N'P', N'PC'))
		DROP PROCEDURE [dbo].[Mc_UpdateEntityNodesUsers]	

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
