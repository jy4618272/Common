BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNode_Mc_EntityNode')
      ALTER TABLE [dbo].[Mc_EntityNode] DROP CONSTRAINT [FK_Mc_EntityNode_Mc_EntityNode]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNode_Mc_EntityNode')
      ALTER TABLE [dbo].[Mc_EntityNode] DROP CONSTRAINT [FK_Mc_EntityNode_Mc_EntityNode]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNode_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityNode] DROP CONSTRAINT [FK_Mc_EntityNode_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_EntityNode_OrderNumber')
      ALTER TABLE [dbo].[Mc_EntityNode] DROP CONSTRAINT [DF_Mc_EntityNode_OrderNumber]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_EntityNode_Deleted')
      ALTER TABLE [dbo].[Mc_EntityNode] DROP CONSTRAINT [DF_Mc_EntityNode_Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE TABLE [dbo].[tmp_Mc_EntityNode] (
   [EntityNodeId] [uniqueidentifier] NOT NULL,
   [ParentEntityNodeId] [uniqueidentifier] NULL,
   [Name] [nvarchar] (255) COLLATE Cyrillic_General_CI_AS NOT NULL,
   [OrderNumber] [int] NOT NULL CONSTRAINT [DF_Mc_EntityNode_OrderNumber] DEFAULT ((0)),
   [OrganizationId] [uniqueidentifier] NOT NULL,
   [InstanceId] [uniqueidentifier] NULL,
   [EntityId] [uniqueidentifier] NOT NULL,
   [EntityNodeTypeId] [uniqueidentifier] NULL,
   [SubEntityId] [uniqueidentifier] NULL,
   [SubEntityLocalId] [nvarchar] (255) COLLATE Cyrillic_General_CI_AS NULL,
   [FullPath] [nvarchar] (1024) COLLATE Cyrillic_General_CI_AS NOT NULL,
   [Deleted] [bit] NOT NULL CONSTRAINT [DF_Mc_EntityNode_Deleted] DEFAULT ((0)))


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   INSERT INTO [dbo].[tmp_Mc_EntityNode] ([EntityNodeId], [ParentEntityNodeId], [Name], [OrderNumber], [OrganizationId], [InstanceId], [EntityId], [EntityNodeTypeId], [SubEntityId], [SubEntityLocalId], [FullPath], [Deleted])
   SELECT [EntityNodeId], [ParentEntityNodeId], [Name], [OrderNumber], [OrganizationId], [InstanceId], [EntityId], [EntityNodeTypeId], [SubEntityId], [SubEntityLocalId], '', [Deleted]
   FROM [dbo].[Mc_EntityNode]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP TABLE [dbo].[Mc_EntityNode]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec sp_rename N'[dbo].[tmp_Mc_EntityNode]', N'Mc_EntityNode'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_EntityNode] ADD CONSTRAINT [PK_Mc_EntityNode] PRIMARY KEY CLUSTERED ([EntityNodeId])

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
			AND enren.EntityId = @EntityId AND (en.Deleted = 0 OR RelatedEntityNodeId = ''00000000-0000-0000-0000-000000000000'')

		SET @Err = @@Error

		RETURN @Err
	END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec('ALTER PROCEDURE [dbo].[Mc_GetEntityNodes]
	AS
	BEGIN
		SET NOCOUNT ON;
	
		SELECT EntityNodeId, ParentEntityNodeId, [Name], OrderNumber, OrganizationId, InstanceId, EntityId, EntityNodeTypeId, SubEntityId, SubEntityLocalId, FullPath, Deleted
		FROM [dbo].[Mc_EntityNode]
		WHERE (Deleted = 0);
	END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec('ALTER PROCEDURE [dbo].[Mc_UpdateEntityNode]
	(
		@EntityNodeId uniqueidentifier,
		@ParentEntityNodeId uniqueidentifier,
		@Name nvarchar(255),
		@OrderNumber int,
		@OrganizationId uniqueidentifier,
		@InstanceId uniqueidentifier,
		@EntityId uniqueidentifier,
		@EntityNodeTypeId uniqueidentifier,
		@SubEntityId uniqueidentifier,
		@SubEntityLocalId nvarchar(255),
		@FullPath nvarchar(1024),
		@Deleted bit
	)
	AS
	BEGIN
		SET NOCOUNT OFF;

		UPDATE [dbo].[Mc_EntityNode]
		SET ParentEntityNodeId = @ParentEntityNodeId, [Name] = @Name, OrderNumber = @OrderNumber, OrganizationId = @OrganizationId, InstanceId = @InstanceId, EntityId = @EntityId, EntityNodeTypeId = @EntityNodeTypeId, SubEntityId = @SubEntityId, FullPath = @FullPath, Deleted = @Deleted
		WHERE (EntityNodeId = @EntityNodeId);

		
		SELECT EntityNodeId, ParentEntityNodeId, [Name], OrderNumber, OrganizationId, InstanceId, EntityId, EntityNodeTypeId, SubEntityId, SubEntityLocalId, FullPath, Deleted
		FROM [dbo].[Mc_EntityNode]
		WHERE (EntityNodeId = @EntityNodeId);
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
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNode_Mc_EntityNode')
      ALTER TABLE [dbo].[Mc_EntityNode] ADD CONSTRAINT [FK_Mc_EntityNode_Mc_EntityNode] FOREIGN KEY ([ParentEntityNodeId]) REFERENCES [dbo].[Mc_EntityNode] ([EntityNodeId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION