BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
CREATE TABLE [dbo].[Mc_EntityNodesUsers] (
   [EntityNodesUsersId] [uniqueidentifier] NOT NULL,
   [EntityNodeId] [uniqueidentifier] NOT NULL,
   [UserId] [uniqueidentifier] NOT NULL
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_EntityNodesUsers] ADD CONSTRAINT [PK_Mc_EntityNodesUsers] PRIMARY KEY CLUSTERED ([EntityNodesUsersId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_DeleteEntityNodesUsers]
(
	@EntityNodesUsersId uniqueidentifier
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DELETE
	FROM [Mc_EntityNodesUsers]
	WHERE
		[EntityNodesUsersId] = @EntityNodesUsersId
	SET @Err = @@Error

	RETURN @Err
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_GetAllEntityNodesUsers]
	@UserId uniqueidentifier,
	@OrganizationId uniqueidentifier
AS
BEGIN

	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		[EntityNodesUsersId],
		enu.[EntityNodeId],
		[UserId]
	FROM [Mc_EntityNodesUsers] as enu
	LEFT JOIN Mc_EntityNode as en ON en.EntityNodeId = enu.EntityNodeId
	WHERE [UserId] = @UserId AND en.OrganizationId = @OrganizationId

	SET @Err = @@Error

	RETURN @Err
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_GetEntityNodesUsers]
	@EntityNodesUsersId uniqueidentifier	
AS
BEGIN

	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		[EntityNodesUsersId],
		[EntityNodeId],
		[UserId]
	FROM [Mc_EntityNodesUsers]
	WHERE [EntityNodesUsersId] = @EntityNodesUsersId

	SET @Err = @@Error

	RETURN @Err
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_InsertEntityNodesUsers]
(
	@EntityNodesUsersId uniqueidentifier,
	@EntityNodeId uniqueidentifier,
	@UserId uniqueidentifier
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	IF @EntityNodesUsersId IS NULL
		 SET @EntityNodesUsersId = NEWID()

	SET @Err = @@Error

	IF (@Err <> 0)
	    RETURN @Err


	INSERT
	INTO [Mc_EntityNodesUsers]
	(
		[EntityNodesUsersId],
		[EntityNodeId],
		[UserId]
	)
	VALUES
	(
		@EntityNodesUsersId,
		@EntityNodeId,
		@UserId
	)

	SET @Err = @@Error


	RETURN @Err
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_UpdateEntityNodesUsers]
(
	@EntityNodesUsersId uniqueidentifier,
	@EntityNodeId uniqueidentifier,
	@UserId uniqueidentifier
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	UPDATE [Mc_EntityNodesUsers]
	SET
		[EntityNodeId] = @EntityNodeId,
		[UserId] = @UserId
	WHERE
		[EntityNodesUsersId] = @EntityNodesUsersId


	SET @Err = @@Error


	RETURN @Err
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNodesUsers_Mc_EntityNode')
      ALTER TABLE [dbo].[Mc_EntityNodesUsers] DROP CONSTRAINT [FK_Mc_EntityNodesUsers_Mc_EntityNode]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNodesUsers_Mc_EntityNode')
      ALTER TABLE [dbo].[Mc_EntityNodesUsers] ADD CONSTRAINT [FK_Mc_EntityNodesUsers_Mc_EntityNode] FOREIGN KEY ([EntityNodeId]) REFERENCES [dbo].[Mc_EntityNode] ([EntityNodeId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNodesUsers_Mc_User')
      ALTER TABLE [dbo].[Mc_EntityNodesUsers] DROP CONSTRAINT [FK_Mc_EntityNodesUsers_Mc_User]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNodesUsers_Mc_User')
      ALTER TABLE [dbo].[Mc_EntityNodesUsers] ADD CONSTRAINT [FK_Mc_EntityNodesUsers_Mc_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Mc_User] ([UserId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION