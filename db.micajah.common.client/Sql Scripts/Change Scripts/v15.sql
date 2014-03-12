BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_Mc_EntityNode_Mc_EntityNode]') AND type = 'F')
		ALTER TABLE [dbo].[Mc_EntityNode] DROP CONSTRAINT [FK_Mc_EntityNode_Mc_EntityNode]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_Mc_EntityNode_Mc_Instance]') AND type = 'F')
		ALTER TABLE [dbo].[Mc_EntityNode] DROP CONSTRAINT [FK_Mc_EntityNode_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_EntityNode_OrderNumber]') AND type = 'D')
	BEGIN
		ALTER TABLE [dbo].[Mc_EntityNode] DROP CONSTRAINT [DF_Mc_EntityNode_OrderNumber]
	END

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_EntityNode_Deleted]') AND type = 'D')
	BEGIN
		ALTER TABLE [dbo].[Mc_EntityNode] DROP CONSTRAINT [DF_Mc_EntityNode_Deleted]
	END

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_Mc_EntityNodeType_Mc_Instance]') AND type = 'F')
		ALTER TABLE [dbo].[Mc_EntityNodeType] DROP CONSTRAINT [FK_Mc_EntityNodeType_Mc_Instance]

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_EntityNodeType_Deleted]') AND type = 'D')
	BEGIN
		ALTER TABLE [dbo].[Mc_EntityNodeType] DROP CONSTRAINT [DF_Mc_EntityNodeType_Deleted]
	END

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_EntityNode]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [dbo].[Mc_EntityNode]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_EntityNodeType]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
		DROP TABLE [dbo].[Mc_EntityNodeType]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
EXEC('CREATE TABLE [dbo].[Mc_EntityNode](
	[EntityNodeId] [uniqueidentifier] NOT NULL,
	[ParentEntityNodeId] [uniqueidentifier] NULL,
	[Name] [nvarchar](255) NOT NULL,
	[OrderNumber] [int] NOT NULL,
	[OrganizationId] [uniqueidentifier] NULL,
	[InstanceId] [uniqueidentifier] NULL,
	[EntityId] [uniqueidentifier] NOT NULL,
	[EntityNodeTypeId] [uniqueidentifier] NULL,
	[SubEntityId] [uniqueidentifier] NULL,
	[SubEntityLocalId] [nvarchar](255) NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Mc_EntityNode] PRIMARY KEY CLUSTERED 
(
	[EntityNodeId] ASC
) ON [PRIMARY]
) ON [PRIMARY]')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
EXEC('CREATE TABLE [dbo].[Mc_EntityNodeType](
	[EntityNodeTypeId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[OrderNumber] [int] NOT NULL,
	[OrganizationId] [uniqueidentifier] NULL,
	[InstanceId] [uniqueidentifier] NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Mc_EntityNodeType] PRIMARY KEY CLUSTERED 
(
	[EntityNodeTypeId] ASC
) ON [PRIMARY]
) ON [PRIMARY]')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityNode]  WITH CHECK ADD  CONSTRAINT [FK_Mc_EntityNode_Mc_EntityNode] FOREIGN KEY([ParentEntityNodeId])
		REFERENCES [dbo].[Mc_EntityNode] ([EntityNodeId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityNode] CHECK CONSTRAINT [FK_Mc_EntityNode_Mc_EntityNode]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityNode]  WITH CHECK ADD  CONSTRAINT [FK_Mc_EntityNode_Mc_Instance] FOREIGN KEY([InstanceId])
		REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityNode] CHECK CONSTRAINT [FK_Mc_EntityNode_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityNode] ADD  CONSTRAINT [DF_Mc_EntityNode_OrderNumber]  DEFAULT ((0)) FOR [OrderNumber]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityNode] ADD  CONSTRAINT [DF_Mc_EntityNode_Deleted]  DEFAULT ((0)) FOR [Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityNodeType]  WITH CHECK ADD  CONSTRAINT [FK_Mc_EntityNodeType_Mc_Instance] FOREIGN KEY([InstanceId])
		REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityNodeType] CHECK CONSTRAINT [FK_Mc_EntityNodeType_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityNodeType] ADD  CONSTRAINT [DF_Mc_EntityNodeType_Deleted]  DEFAULT ((0)) FOR [Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_GetEntityNodes]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_GetEntityNodes]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_GetEntityNodeTypes]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_GetEntityNodeTypes]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertEntityNode]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertEntityNode]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertEntityNodeType]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertEntityNodeType]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateEntityNode]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateEntityNode]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateEntityNodeType]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateEntityNodeType]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
EXEC('CREATE PROCEDURE [dbo].[Mc_GetEntityNodes]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT EntityNodeId, ParentEntityNodeId, [Name], OrderNumber, OrganizationId, InstanceId, EntityId, EntityNodeTypeId, SubEntityId, SubEntityLocalId, Deleted
	FROM [dbo].[Mc_EntityNode]
	WHERE (Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
EXEC('CREATE PROCEDURE [dbo].[Mc_GetEntityNodeTypes]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT EntityNodeTypeId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted
	FROM [dbo].[Mc_EntityNodeType]
	WHERE (Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
EXEC('CREATE PROCEDURE [dbo].[Mc_InsertEntityNode]
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
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_EntityNode] (EntityNodeId, ParentEntityNodeId, [Name], OrderNumber, OrganizationId, InstanceId, EntityId, EntityNodeTypeId, SubEntityId, SubEntityLocalId, Deleted)
	VALUES (@EntityNodeId, @ParentEntityNodeId, @Name, @OrderNumber, @OrganizationId, @InstanceId, @EntityId, @EntityNodeTypeId, @SubEntityId, @SubEntityLocalId, @Deleted);
	
	SELECT EntityNodeId, ParentEntityNodeId, [Name], OrderNumber, OrganizationId, InstanceId, EntityId, EntityNodeTypeId, SubEntityId, SubEntityLocalId, Deleted
	FROM [dbo].[Mc_EntityNode]
	WHERE (EntityNodeId = @EntityNodeId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
EXEC('CREATE PROCEDURE [dbo].[Mc_InsertEntityNodeType]
(
	@EntityNodeTypeId uniqueidentifier,
	@Name nvarchar(255),
	@OrderNumber int,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_EntityNode] (EntityNodeId, EntityNodeTypeId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted)
	VALUES (@EntityNodeTypeId, @EntityNodeTypeId, @Name, @OrderNumber, @OrganizationId, @InstanceId, @Deleted);
	
	SELECT EntityNodeTypeId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted
	FROM [dbo].[Mc_EntityNode]
	WHERE (EntityNodeTypeId = EntityNodeTypeId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateEntityNode]
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
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [dbo].[Mc_EntityNode]
	SET ParentEntityNodeId = @ParentEntityNodeId, [Name] = @Name, OrderNumber = @OrderNumber, OrganizationId = @OrganizationId, InstanceId = @InstanceId, EntityId = @EntityId, EntityNodeTypeId = @EntityNodeTypeId, SubEntityId = @SubEntityId, Deleted = @Deleted
	WHERE (EntityNodeId = @EntityNodeId);

	
	SELECT EntityNodeId, ParentEntityNodeId, [Name], OrderNumber, OrganizationId, InstanceId, EntityId, EntityNodeTypeId, SubEntityId, SubEntityLocalId, Deleted
	FROM [dbo].[Mc_EntityNode]
	WHERE (EntityNodeId = @EntityNodeId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateEntityNodeType]
(
	@EntityNodeTypeId uniqueidentifier,
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
	SET [Name] = @Name,  OrderNumber = @OrderNumber, OrganizationId = @OrganizationId, InstanceId = @InstanceId, Deleted = @Deleted
	WHERE (EntityNodeTypeId = EntityNodeTypeId);

	SELECT EntityNodeTypeId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted
	FROM [dbo].[Mc_EntityNode]
	WHERE (EntityNodeTypeId = EntityNodeTypeId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION