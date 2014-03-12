BEGIN TRANSACTION

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_EntityNodeType_Mc_Instance]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_EntityNodeType]'))
	ALTER TABLE [dbo].[Mc_EntityNodeType] DROP CONSTRAINT [FK_Mc_EntityNodeType_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
   
IF @@TRANCOUNT = 1   
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_EntityNodeType_Deleted]') AND type = 'D')
	BEGIN
		ALTER TABLE [dbo].[Mc_EntityNodeType] DROP CONSTRAINT [DF_Mc_EntityNodeType_Deleted]
	END

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
   
IF @@TRANCOUNT = 1   
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_EntityNodeType]') AND type in (N'U'))
		DROP TABLE [dbo].[Mc_EntityNodeType]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
   
IF @@TRANCOUNT = 1   
CREATE TABLE [dbo].[Mc_EntityNodeType](
	[EntityNodeTypeId] [uniqueidentifier] NOT NULL,
	[EntityId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[OrderNumber] [int] NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[InstanceId] [uniqueidentifier] NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Mc_EntityNodeType] PRIMARY KEY CLUSTERED 
(
	[EntityNodeTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

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
   COMMIT TRANSACTION