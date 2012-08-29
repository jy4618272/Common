BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

DECLARE @ProductVersion nvarchar(128), @NVarCharMax nvarchar(128)

SET @ProductVersion = CAST(SERVERPROPERTY('productversion') AS nvarchar(128))
SET @ProductVersion = LEFT(@ProductVersion, CHARINDEX('.', @ProductVersion) - 1)

IF (CAST(@ProductVersion AS int) > 8)
	SET @NVarCharMax = 'max'
ELSE
	SET @NVarCharMax = '4000'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_DeleteGroupDepartmentAction]'))
		DROP PROCEDURE [dbo].[Mc_DeleteGroupDepartmentAction]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_DeleteGroupDepartmentRoles]'))
		DROP PROCEDURE [dbo].[Mc_DeleteGroupDepartmentRoles]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_GetDepartments]'))
		DROP PROCEDURE [dbo].[Mc_GetDepartments]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_GetGroupsDepartmentsActions]'))
		DROP PROCEDURE [dbo].[Mc_GetGroupsDepartmentsActions]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_GetGroupsDepartmentsRoles]'))
		DROP PROCEDURE [dbo].[Mc_GetGroupsDepartmentsRoles]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_GetSettingsValues]'))
		DROP PROCEDURE [dbo].[Mc_GetSettingsValues]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertDepartment]'))
		DROP PROCEDURE [dbo].[Mc_InsertDepartment]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertGroupDepartmentAction]'))
		DROP PROCEDURE [dbo].[Mc_InsertGroupDepartmentAction]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertGroupDepartmentRole]'))
		DROP PROCEDURE [dbo].[Mc_InsertGroupDepartmentRole]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertSettingValue]'))
		DROP PROCEDURE [dbo].[Mc_InsertSettingValue]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateDepartment]'))
		DROP PROCEDURE [dbo].[Mc_UpdateDepartment]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateGroupDepartmentAction]'))
		DROP PROCEDURE [dbo].[Mc_UpdateGroupDepartmentAction]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateGroupDepartmentRole]'))
		DROP PROCEDURE [dbo].[Mc_UpdateGroupDepartmentRole]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateSettingValue]'))
		DROP PROCEDURE [dbo].[Mc_UpdateSettingValue]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsDepartmentsActions_Mc_Department')
		ALTER TABLE [dbo].[Mc_GroupsDepartmentsActions] DROP CONSTRAINT [FK_Mc_GroupsDepartmentsActions_Mc_Department]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsDepartmentsActions_Mc_Group')
		ALTER TABLE [dbo].[Mc_GroupsDepartmentsActions] DROP CONSTRAINT [FK_Mc_GroupsDepartmentsActions_Mc_Group]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_GroupsDepartmentsActions_Enabled')
		ALTER TABLE [dbo].[Mc_GroupsDepartmentsActions] DROP CONSTRAINT [DF_Mc_GroupsDepartmentsActions_Enabled]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsDepartmentsRoles_Mc_Department')
		ALTER TABLE [dbo].[Mc_GroupsDepartmentsRoles] DROP CONSTRAINT [FK_Mc_GroupsDepartmentsRoles_Mc_Department]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsDepartmentsRoles_Mc_Group')
		ALTER TABLE [dbo].[Mc_GroupsDepartmentsRoles] DROP CONSTRAINT [FK_Mc_GroupsDepartmentsRoles_Mc_Group]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_SettingsValues_Mc_Department')
		ALTER TABLE [dbo].[Mc_SettingsValues] DROP CONSTRAINT [FK_Mc_SettingsValues_Mc_Department]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_SettingsValues_Mc_Group')
		ALTER TABLE [dbo].[Mc_SettingsValues] DROP CONSTRAINT [FK_Mc_SettingsValues_Mc_Group]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Department_DepartmentGuid')
		ALTER TABLE [dbo].[Mc_Department] DROP CONSTRAINT [DF_Mc_Department_DepartmentGuid]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Department_Description')
		ALTER TABLE [dbo].[Mc_Department] DROP CONSTRAINT [DF_Mc_Department_Description]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Department_EnableSignUpUser')
		ALTER TABLE [dbo].[Mc_Department] DROP CONSTRAINT [DF_Mc_Department_EnableSignUpUser]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Department_Deleted')
		ALTER TABLE [dbo].[Mc_Department] DROP CONSTRAINT [DF_Mc_Department_Deleted]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_Instance](
		[InstanceId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Mc_Instance_InstanceId]  DEFAULT (newid()),
		[OrganizationId] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Description] [nvarchar](1024) NOT NULL CONSTRAINT [DF_Mc_Instance_Description]  DEFAULT (N''),
		[EnableSignUpUser] [bit] NOT NULL CONSTRAINT [DF_Mc_Instance_EnableSignUpUser]  DEFAULT ((0)),
		[Deleted] [bit] NOT NULL CONSTRAINT [DF_Mc_Instance_Deleted]  DEFAULT ((0)),
	 CONSTRAINT [PK_Mc_Instance] PRIMARY KEY CLUSTERED 
	(
		[InstanceId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Instance] ([InstanceId], [OrganizationId], [Name], [Description], [EnableSignUpUser], [Deleted])
	SELECT [DepartmentId], [OrganizationId], [Name], [Description], [EnableSignUpUser], [Deleted]
	FROM [dbo].[Mc_Department]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP TABLE [dbo].[Mc_Department]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_GroupsInstancesActions](
		[GroupId] [uniqueidentifier] NOT NULL,
		[InstanceId] [uniqueidentifier] NOT NULL,
		[ActionId] [uniqueidentifier] NOT NULL,
		[Enabled] [bit] NOT NULL CONSTRAINT [DF_Mc_GroupsInstancesActions_Enabled]  DEFAULT ((1)),
	 CONSTRAINT [PK_Mc_GroupsInstancesActions] PRIMARY KEY CLUSTERED 
	(
		[GroupId] ASC,
		[InstanceId] ASC,
		[ActionId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_GroupsInstancesActions] ([GroupId], [InstanceId], [ActionId], [Enabled])
	SELECT [GroupId], [DepartmentId], [ActionId], [Enabled]
	FROM [dbo].[Mc_GroupsDepartmentsActions]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP TABLE [dbo].[Mc_GroupsDepartmentsActions]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_GroupsInstancesRoles](
		[GroupId] [uniqueidentifier] NOT NULL,
		[InstanceId] [uniqueidentifier] NOT NULL,
		[RoleId] [uniqueidentifier] NOT NULL,
	 CONSTRAINT [PK_Mc_GroupsInstancesRoles] PRIMARY KEY CLUSTERED 
	(
		[GroupId] ASC,
		[InstanceId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_GroupsInstancesRoles] ([GroupId], [InstanceId], [RoleId])
	SELECT [GroupId], [DepartmentId], [RoleId]
	FROM [dbo].[Mc_GroupsDepartmentsRoles]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP TABLE [dbo].[Mc_GroupsDepartmentsRoles]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE TABLE [dbo].[tmp_Mc_SettingsValues](
	[SettingValueId] [uniqueidentifier] NOT NULL,
	[SettingId] [uniqueidentifier] NOT NULL,
	[Value] [nvarchar](' + @NVarCharMax + ') NOT NULL,
	[OrganizationId] [uniqueidentifier] NULL,
	[InstanceId] [uniqueidentifier] NULL,
	[GroupId] [uniqueidentifier] NULL
)')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_SettingsValues] ([SettingValueId], [SettingId], [Value], [OrganizationId], [InstanceId], [GroupId])
	SELECT [SettingValueId], [SettingId], [Value], [OrganizationId], [DepartmentId], [GroupId]
	FROM [dbo].[Mc_SettingsValues]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP TABLE [dbo].[Mc_SettingsValues]

IF @@TRANCOUNT = 1
	EXEC sp_rename N'[dbo].[tmp_Mc_SettingsValues]', N'Mc_SettingsValues'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_SettingsValues] ADD CONSTRAINT [PK_Mc_SettingsValues] PRIMARY KEY CLUSTERED ([SettingValueId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsInstancesActions_Mc_Group')
		ALTER TABLE [dbo].[Mc_GroupsInstancesActions] ADD CONSTRAINT [FK_Mc_GroupsInstancesActions_Mc_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Mc_Group] ([GroupId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsInstancesActions_Mc_Instance')
		ALTER TABLE [dbo].[Mc_GroupsInstancesActions] ADD CONSTRAINT [FK_Mc_GroupsInstancesActions_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsInstancesRoles_Mc_Group')
		ALTER TABLE [dbo].[Mc_GroupsInstancesRoles] ADD CONSTRAINT [FK_Mc_GroupsInstancesRoles_Mc_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Mc_Group] ([GroupId])

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
	IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_SettingsValues_Mc_Group')
		ALTER TABLE [dbo].[Mc_SettingsValues] ADD CONSTRAINT [FK_Mc_SettingsValues_Mc_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Mc_Group] ([GroupId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Group] SET [Name] = REPLACE([Name], 'Department', 'Instance') WHERE [Name] LIKE '%Department%'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteGroupInstanceAction]
(
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@ActionId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_GroupsInstancesActions 
	WHERE (GroupId = @GroupId) AND (InstanceId = @InstanceId) AND (ActionId = @ActionId);
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteGroupInstanceRoles]
(
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_GroupsInstancesRoles 
	WHERE (GroupId = @GroupId) AND (InstanceId = @InstanceId);
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetGroupsInstancesActions]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT gda.GroupId, gda.InstanceId, gda.ActionId, gda.[Enabled]
	FROM dbo.Mc_GroupsInstancesActions AS gda
	INNER JOIN dbo.Mc_Group AS g
		ON	(gda.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId)
	INNER JOIN dbo.Mc_Instance AS d
		ON	(gda.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetGroupsInstancesRoles]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT gdr.GroupId, gdr.InstanceId, gdr.RoleId
	FROM dbo.Mc_GroupsInstancesRoles AS gdr
	INNER JOIN dbo.Mc_Group AS g
		ON	(gdr.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId)
	INNER JOIN dbo.Mc_Instance AS d
		ON	(gdr.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetInstances]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT InstanceId, OrganizationId, [Name], [Description], EnableSignUpUser, Deleted 
	FROM dbo.Mc_Instance 
	WHERE (Deleted = 0) AND (OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetSettingsValues]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT SettingValueId, SettingId, [Value], OrganizationId, InstanceId, GroupId
	FROM dbo.Mc_SettingsValues 
	WHERE (OrganizationId IS NULL) OR (OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertGroupInstanceAction]
(
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@ActionId uniqueidentifier,
	@Enabled bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_GroupsInstancesActions (GroupId, InstanceId, ActionId, [Enabled]) 
	VALUES (@GroupId, @InstanceId, @ActionId, @Enabled);
	
	SELECT GroupId, InstanceId, ActionId, [Enabled] 
	FROM dbo.Mc_GroupsInstancesActions 
	WHERE (ActionId = @ActionId) AND (InstanceId = @InstanceId) AND (GroupId = @GroupId);
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertGroupInstanceRole]
(
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@RoleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_GroupsInstancesRoles (GroupId, InstanceId, RoleId) 
	VALUES (@GroupId, @InstanceId, @RoleId);
	
	SELECT GroupId, InstanceId, RoleId 
	FROM dbo.Mc_GroupsInstancesRoles 
	WHERE (InstanceId = @InstanceId) AND (GroupId = @GroupId);
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertInstance]
(
	@InstanceId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@EnableSignUpUser bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Instance (InstanceId, OrganizationId, [Name], [Description], EnableSignUpUser, Deleted) 
	VALUES (@InstanceId, @OrganizationId, @Name, @Description, @EnableSignUpUser, @Deleted);
	
	SELECT InstanceId, OrganizationId, [Name], [Description], EnableSignUpUser, Deleted 
	FROM dbo.Mc_Instance 
	WHERE (InstanceId = @InstanceId);
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertSettingValue]
(
	@SettingValueId uniqueidentifier,
	@SettingId uniqueidentifier,
	@Value nvarchar(' + @NVarCharMax + '),
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@GroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_SettingsValues (SettingValueId, SettingId, Value, OrganizationId, InstanceId, GroupId) 
	VALUES (@SettingValueId, @SettingId, @Value, @OrganizationId, @InstanceId, @GroupId);
	
	SELECT SettingValueId, SettingId, Value, OrganizationId, InstanceId, GroupId 
	FROM dbo.Mc_SettingsValues
	WHERE (SettingValueId = @SettingValueId);
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateGroupInstanceAction]
(
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@ActionId uniqueidentifier,
	@Enabled bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_GroupsInstancesActions 
	SET [Enabled] = @Enabled 
	WHERE (ActionId = @ActionId) AND (InstanceId = @InstanceId) AND (GroupId = @GroupId);
	
	SELECT GroupId, InstanceId, ActionId, [Enabled] 
	FROM dbo.Mc_GroupsInstancesActions 
	WHERE (ActionId = @ActionId) AND (InstanceId = @InstanceId) AND (GroupId = @GroupId);
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateGroupInstanceRole]
(
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@RoleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_GroupsInstancesRoles 
	SET GroupId = @GroupId, InstanceId = @InstanceId, RoleId = @RoleId 
	WHERE (GroupId = @GroupId) AND (InstanceId = @InstanceId);
	
	SELECT GroupId, InstanceId, RoleId 
	FROM dbo.Mc_GroupsInstancesRoles 
	WHERE (InstanceId = @InstanceId) AND (GroupId = @GroupId);
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateInstance]
(
	@InstanceId uniqueidentifier,
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
	SET OrganizationId = @OrganizationId, [Name] = @Name, [Description] = @Description, EnableSignUpUser = @EnableSignUpUser, Deleted = @Deleted 
	WHERE (InstanceId = @InstanceId);
	
	SELECT InstanceId, OrganizationId, [Name], [Description], EnableSignUpUser, Deleted 
	FROM dbo.Mc_Instance 
	WHERE (InstanceId = @InstanceId);
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateSettingValue]
(
	@SettingValueId uniqueidentifier,
	@SettingId uniqueidentifier,
	@Value nvarchar(' + @NVarCharMax + '),
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@GroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_SettingsValues 
	SET SettingId = @SettingId, Value = @Value, OrganizationId = @OrganizationId, InstanceId = @InstanceId, GroupId = @GroupId 
	WHERE (SettingValueId = @SettingValueId);
	
	SELECT SettingValueId, SettingId, Value, OrganizationId, InstanceId, GroupId 
	FROM dbo.Mc_SettingsValues 
	WHERE (SettingValueId = @SettingValueId);
END')


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
