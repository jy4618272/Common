BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_EntityNode]
      ALTER COLUMN [OrganizationId] [uniqueidentifier] NOT NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_EntityNodeType_Deleted')
      ALTER TABLE [dbo].[Mc_EntityNodeType] DROP CONSTRAINT [DF_Mc_EntityNodeType_Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
CREATE TABLE [dbo].[tmp_Mc_EntityNodeType] (
[EntityNodeTypeId] [uniqueidentifier] NOT NULL,
[EntityId] [uniqueidentifier] NOT NULL,
[Name] [nvarchar] (255) COLLATE Cyrillic_General_CI_AS NOT NULL,
[OrderNumber] [int] NOT NULL,
[OrganizationId] [uniqueidentifier] NOT NULL,
[InstanceId] [uniqueidentifier] NULL,
[Deleted] [bit] NOT NULL CONSTRAINT [DF_Mc_EntityNodeType_Deleted] DEFAULT ((0))
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   INSERT INTO [dbo].[tmp_Mc_EntityNodeType] ([EntityNodeTypeId], [EntityId], [Name], [OrderNumber], [OrganizationId], [InstanceId], [Deleted])
   SELECT [EntityNodeTypeId], newid(), [Name], [OrderNumber], [OrganizationId], [InstanceId], [Deleted]
   FROM [dbo].[Mc_EntityNodeType]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP TABLE [dbo].[Mc_EntityNodeType]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec sp_rename N'[dbo].[tmp_Mc_EntityNodeType]', N'Mc_EntityNodeType'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_EntityNodeType] ADD CONSTRAINT [PK_Mc_EntityNodeType] PRIMARY KEY CLUSTERED ([EntityNodeTypeId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('ALTER PROCEDURE [dbo].[Mc_GetEntityNodeTypes]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT EntityNodeTypeId, EntityId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted
	FROM [dbo].[Mc_EntityNodeType]
	WHERE (Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_GetEntityNodeTypesByEntityId]
	@EntityId uniqueidentifier,
	@OrganizationId uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	SELECT EntityNodeTypeId, EntityId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted
	FROM [dbo].[Mc_EntityNodeType]
	WHERE Deleted = 0 AND EntityId = @EntityId AND OrganizationId = @OrganizationId
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('ALTER PROCEDURE [dbo].[Mc_InsertEntityNodeType]
(
	@EntityNodeTypeId uniqueidentifier,
	@EntityId uniqueidentifier,
	@Name nvarchar(255),
	@OrderNumber int,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_EntityNodeType] (EntityNodeTypeId, EntityId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted)
	VALUES (@EntityNodeTypeId, @EntityId, @Name, @OrderNumber, @OrganizationId, @InstanceId, @Deleted);
	
	SELECT EntityNodeTypeId, EntityId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted
	FROM [dbo].[Mc_EntityNodeType]
	WHERE (EntityNodeTypeId = @EntityNodeTypeId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('ALTER PROCEDURE [dbo].[Mc_UpdateEntityNodeType]
(
	@EntityNodeTypeId uniqueidentifier,
	@EntityId uniqueidentifier,
	@Name nvarchar(255),
	@OrderNumber int,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [dbo].[Mc_EntityNodeType]
	SET [Name] = @Name,  EntityId = @EntityId, OrderNumber = @OrderNumber, OrganizationId = @OrganizationId, InstanceId = @InstanceId, Deleted = @Deleted
	WHERE (EntityNodeTypeId = @EntityNodeTypeId);

	SELECT EntityNodeTypeId, EntityId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted
	FROM [dbo].[Mc_EntityNodeType]
	WHERE (EntityNodeTypeId = @EntityNodeTypeId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNode_Mc_EntityNode')
      ALTER TABLE [dbo].[Mc_EntityNode] DROP CONSTRAINT [FK_Mc_EntityNode_Mc_EntityNode]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNode_Mc_EntityNode')
      ALTER TABLE [dbo].[Mc_EntityNode] ADD CONSTRAINT [FK_Mc_EntityNode_Mc_EntityNode] FOREIGN KEY ([ParentEntityNodeId]) REFERENCES [dbo].[Mc_EntityNode] ([EntityNodeId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNode_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityNode] DROP CONSTRAINT [FK_Mc_EntityNode_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNode_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityNode] ADD CONSTRAINT [FK_Mc_EntityNode_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNodeType_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityNodeType] DROP CONSTRAINT [FK_Mc_EntityNodeType_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNodeType_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityNodeType] ADD CONSTRAINT [FK_Mc_EntityNodeType_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
