BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityField_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityField] DROP CONSTRAINT [FK_Mc_EntityField_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNode_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityNode] DROP CONSTRAINT [FK_Mc_EntityNode_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNodeType_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityNodeType] DROP CONSTRAINT [FK_Mc_EntityNodeType_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsInstancesActions_Mc_Instance')
      ALTER TABLE [dbo].[Mc_GroupsInstancesActions] DROP CONSTRAINT [FK_Mc_GroupsInstancesActions_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsInstancesRoles_Mc_Instance')
      ALTER TABLE [dbo].[Mc_GroupsInstancesRoles] DROP CONSTRAINT [FK_Mc_GroupsInstancesRoles_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_SettingsValues_Mc_Instance')
      ALTER TABLE [dbo].[Mc_SettingsValues] DROP CONSTRAINT [FK_Mc_SettingsValues_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_UsersInstances_Mc_Instance')
      ALTER TABLE [dbo].[Mc_UsersInstances] DROP CONSTRAINT [FK_Mc_UsersInstances_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_InstanceId')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_InstanceId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_SimpleId')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_SimpleId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_Description')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_Description]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_EnableSignUpUser')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_EnableSignUpUser]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_Deleted')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE TABLE [dbo].[tmp_Mc_Instance] (
	[InstanceId] [uniqueidentifier] NOT NULL,
	[PseudoId] [varchar](6) NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
	[EnableSignUpUser] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   INSERT INTO [dbo].[tmp_Mc_Instance] ([InstanceId], [PseudoId], [OrganizationId], [Name], [Description], [EnableSignUpUser], [Deleted])
   SELECT [InstanceId], '', [OrganizationId], [Name], [Description], [EnableSignUpUser], [Deleted]
   FROM [dbo].[Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP TABLE [dbo].[Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC sp_rename N'[dbo].[tmp_Mc_Instance]', N'Mc_Instance'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Instance] ADD CONSTRAINT [PK_Mc_Instance] PRIMARY KEY CLUSTERED ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetInstances]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, Deleted 
	FROM dbo.Mc_Instance 
	WHERE (OrganizationId = @OrganizationId) AND (Deleted = 0);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertInstance]
(
	@InstanceId uniqueidentifier,
	@PseudoId varchar(6),
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@EnableSignUpUser bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Instance (InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, Deleted) 
	VALUES (@InstanceId, @PseudoId, @OrganizationId, @Name, @Description, @EnableSignUpUser, @Deleted);
	
	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, Deleted 
	FROM dbo.Mc_Instance 
	WHERE (InstanceId = @InstanceId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateInstance]
(
	@InstanceId uniqueidentifier,
	@PseudoId varchar(6),
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@EnableSignUpUser bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Instance 
	SET PseudoId  = @PseudoId, OrganizationId = @OrganizationId, [Name] = @Name, [Description] = @Description, EnableSignUpUser = @EnableSignUpUser, Deleted = @Deleted 
	WHERE (InstanceId = @InstanceId);
	
	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, Deleted 
	FROM dbo.Mc_Instance 
	WHERE (InstanceId = @InstanceId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_InstanceId')
      ALTER TABLE [dbo].[Mc_Instance] ADD  CONSTRAINT [DF_Mc_Instance_InstanceId]  DEFAULT (newid()) FOR [InstanceId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_PseudoId')
      ALTER TABLE [dbo].[Mc_Instance] ADD  CONSTRAINT [DF_Mc_Instance_PseudoId]  DEFAULT ('') FOR [PseudoId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_Description')
      ALTER TABLE [dbo].[Mc_Instance] ADD  CONSTRAINT [DF_Mc_Instance_Description]  DEFAULT (N'') FOR [Description]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_EnableSignUpUser')
      ALTER TABLE [dbo].[Mc_Instance] ADD  CONSTRAINT [DF_Mc_Instance_EnableSignUpUser]  DEFAULT ((0)) FOR [EnableSignUpUser]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_Deleted')
      ALTER TABLE [dbo].[Mc_Instance] ADD  CONSTRAINT [DF_Mc_Instance_Deleted]  DEFAULT ((0)) FOR [Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityField_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityField] ADD CONSTRAINT [FK_Mc_EntityField_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNode_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityNode] ADD CONSTRAINT [FK_Mc_EntityNode_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNodeType_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityNodeType] ADD CONSTRAINT [FK_Mc_EntityNodeType_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsInstancesActions_Mc_Instance')
      ALTER TABLE [dbo].[Mc_GroupsInstancesActions] ADD CONSTRAINT [FK_Mc_GroupsInstancesActions_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsInstancesRoles_Mc_Instance')
      ALTER TABLE [dbo].[Mc_GroupsInstancesRoles] ADD CONSTRAINT [FK_Mc_GroupsInstancesRoles_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_SettingsValues_Mc_Instance')
      ALTER TABLE [dbo].[Mc_SettingsValues] ADD CONSTRAINT [FK_Mc_SettingsValues_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_UsersInstances_Mc_Instance')
      ALTER TABLE [dbo].[Mc_UsersInstances] ADD CONSTRAINT [FK_Mc_UsersInstances_Mc_Instance] FOREIGN KEY([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
