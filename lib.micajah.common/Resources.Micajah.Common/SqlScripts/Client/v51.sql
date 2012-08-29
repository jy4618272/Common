BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsInstancesActions_Mc_Group')
      ALTER TABLE [dbo].[Mc_GroupsInstancesActions] DROP CONSTRAINT [FK_Mc_GroupsInstancesActions_Mc_Group]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsInstancesRoles_Mc_Group')
      ALTER TABLE [dbo].[Mc_GroupsInstancesRoles] DROP CONSTRAINT [FK_Mc_GroupsInstancesRoles_Mc_Group]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_SettingsValues_Mc_Group')
      ALTER TABLE [dbo].[Mc_SettingsValues] DROP CONSTRAINT [FK_Mc_SettingsValues_Mc_Group]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_UsersGroups_Mc_Group')
      ALTER TABLE [dbo].[Mc_UsersGroups] DROP CONSTRAINT [FK_Mc_UsersGroups_Mc_Group]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Group_Description')
      ALTER TABLE [dbo].[Mc_Group] DROP CONSTRAINT [DF_Mc_Group_Description]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Group_Deleted')
      ALTER TABLE [dbo].[Mc_Group] DROP CONSTRAINT [DF_Mc_Group_Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE TABLE [dbo].[tmp_Mc_Group] (
   [GroupId] [uniqueidentifier] NOT NULL,
   [OrganizationId] [uniqueidentifier] NOT NULL,
   [Name] [nvarchar] (255) NOT NULL,
   [Description] [nvarchar] (1024) NOT NULL CONSTRAINT [DF_Mc_Group_Description] DEFAULT (N''),
   [BuiltIn] [bit] NOT NULL CONSTRAINT [DF_Mc_Group_BuiltIn] DEFAULT ((0)),
   [Deleted] [bit] NOT NULL CONSTRAINT [DF_Mc_Group_Deleted] DEFAULT ((0))
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   INSERT INTO [dbo].[tmp_Mc_Group] ([GroupId], [OrganizationId], [Name], [Description], [Deleted])
   SELECT [GroupId], [OrganizationId], [Name], [Description], [Deleted]
   FROM [dbo].[Mc_Group]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP TABLE [dbo].[Mc_Group]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC sp_rename N'[dbo].[tmp_Mc_Group]', N'Mc_Group'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Group] ADD CONSTRAINT [PK_Mc_Group] PRIMARY KEY CLUSTERED ([GroupId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetGroups]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT GroupId, OrganizationId, [Name], [Description], BuiltIn, Deleted 
	FROM dbo.Mc_Group 
	WHERE (Deleted = 0) AND (OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertGroup]
(
	@GroupId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@BuiltIn bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Group (GroupId, OrganizationId, [Name], [Description], BuiltIn, Deleted) 
	VALUES (@GroupId, @OrganizationId, @Name, @Description, @BuiltIn, @Deleted);
	
	SELECT GroupId, OrganizationId, [Name], [Description], BuiltIn, Deleted 
	FROM dbo.Mc_Group 
	WHERE (GroupId = @GroupId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateGroup]
(
	@GroupId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@BuiltIn bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Group 
	SET OrganizationId = @OrganizationId, [Name] = @Name, [Description] = @Description, BuiltIn = @BuiltIn, Deleted = @Deleted 
	WHERE (GroupId = @GroupId);
	
	SELECT GroupId, OrganizationId, [Name], [Description], BuiltIn, Deleted 
	FROM dbo.Mc_Group 
	WHERE (GroupId = @GroupId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsInstancesActions_Mc_Group')
      ALTER TABLE [dbo].[Mc_GroupsInstancesActions] ADD CONSTRAINT [FK_Mc_GroupsInstancesActions_Mc_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Mc_Group] ([GroupId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsInstancesRoles_Mc_Group')
      ALTER TABLE [dbo].[Mc_GroupsInstancesRoles] ADD CONSTRAINT [FK_Mc_GroupsInstancesRoles_Mc_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Mc_Group] ([GroupId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_SettingsValues_Mc_Group')
      ALTER TABLE [dbo].[Mc_SettingsValues] ADD CONSTRAINT [FK_Mc_SettingsValues_Mc_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Mc_Group] ([GroupId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_UsersGroups_Mc_Group')
      ALTER TABLE [dbo].[Mc_UsersGroups] ADD CONSTRAINT [FK_Mc_UsersGroups_Mc_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Mc_Group] ([GroupId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION