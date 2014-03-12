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
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_GetActions]'))
		DROP PROCEDURE [dbo].[Mc_GetActions]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_GetSettings]'))
		DROP PROCEDURE [dbo].[Mc_GetSettings]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertAction]'))
		DROP PROCEDURE [dbo].[Mc_InsertAction]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertSetting]'))
		DROP PROCEDURE [dbo].[Mc_InsertSetting]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateAction]'))
		DROP PROCEDURE [dbo].[Mc_UpdateAction]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateSetting]'))
		DROP PROCEDURE [dbo].[Mc_UpdateSetting]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Action_Mc_Action')
		ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [FK_Mc_Action_Mc_Action]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_ActionsParentActions_Mc_Action_1')
		ALTER TABLE [dbo].[Mc_ActionsParentActions] DROP CONSTRAINT [FK_Mc_ActionsParentActions_Mc_Action_1]

IF @@ERROR <> 0
IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_ActionsParentActions_Mc_Action_2')
		ALTER TABLE [dbo].[Mc_ActionsParentActions] DROP CONSTRAINT [FK_Mc_ActionsParentActions_Mc_Action_2]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Role_Mc_Action')
		ALTER TABLE [dbo].[Mc_Role] DROP CONSTRAINT [FK_Mc_Role_Mc_Action]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_RolesActions_Mc_Action')
		ALTER TABLE [dbo].[Mc_RolesActions] DROP CONSTRAINT [FK_Mc_RolesActions_Mc_Action]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Action_Mc_Action')
		ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [FK_Mc_Action_Mc_Action]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Action_ActionId')
		ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [DF_Mc_Action_ActionId]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Action_Description')
		ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [DF_Mc_Action_Description]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Action_IconUrl')
		ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [DF_Mc_Action_IconUrl]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Action_ButtonIconUrl')
		ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [DF_Mc_Action_ButtonIconUrl]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Action_OrderNumber')
		ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [DF_Mc_Action_OrderNumber]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Action_ClassFullName')
		ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [DF_Mc_Action_ClassFullName]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Action_AuthenticationRequired')
		ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [DF_Mc_Action_AuthenticationRequired]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Action_DepartmentRequired')
		ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [DF_Mc_Action_DepartmentRequired]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Action_Visible')
		ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [DF_Mc_Action_Visible]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Action_ShowInDetailMenu')
		ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [DF_Mc_Action_ShowInDetailMenu]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Action_ShowChildrenInDetailMenu')
		ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [DF_Mc_Action_ShowChildrenInDetailMenu]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Action_GroupInDetailMenu')
		ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [DF_Mc_Action_GroupInDetailMenu]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Action_HighlightInDetailMenu')
		ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [DF_Mc_Action_HighlightInDetailMenu]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Action_BuiltIn')
		ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [DF_Mc_Action_BuiltIn]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Action_Deleted')
		ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [DF_Mc_Action_Deleted]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[tmp_Mc_Action] (
		[ActionId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Mc_Action_ActionId] DEFAULT (newid()),
		[ParentActionId] [uniqueidentifier] NULL,
		[ActionTypeId] [int] NOT NULL,
		[Name] [nvarchar] (1024) NOT NULL,
		[Description] [nvarchar] (1024) NOT NULL CONSTRAINT [DF_Mc_Action_Description] DEFAULT (N''),
		[IconUrl] [nvarchar] (2048) NOT NULL CONSTRAINT [DF_Mc_Action_IconUrl] DEFAULT (N''),
		[ButtonIconUrl] [nvarchar] (2048) NOT NULL CONSTRAINT [DF_Mc_Action_ButtonIconUrl] DEFAULT (N''),
		[NavigateUrl] [nvarchar] (2048) NULL,
		[OrderNumber] [int] NOT NULL CONSTRAINT [DF_Mc_Action_OrderNumber] DEFAULT ((0)),
		[ClassFullName] [nvarchar] (1024) NOT NULL CONSTRAINT [DF_Mc_Action_ClassFullName] DEFAULT (N''),
		[AuthenticationRequired] [bit] NOT NULL CONSTRAINT [DF_Mc_Action_AuthenticationRequired] DEFAULT ((0)),
		[InstanceRequired] [bit] NOT NULL CONSTRAINT [DF_Mc_Action_DepartmentRequired] DEFAULT ((1)),
		[Visible] [bit] NOT NULL CONSTRAINT [DF_Mc_Action_Visible] DEFAULT ((1)),
		[ShowInDetailMenu] [bit] NOT NULL CONSTRAINT [DF_Mc_Action_ShowInDetailMenu] DEFAULT ((1)),
		[ShowChildrenInDetailMenu] [bit] NOT NULL CONSTRAINT [DF_Mc_Action_ShowChildrenInDetailMenu] DEFAULT ((0)),
		[GroupInDetailMenu] [bit] NOT NULL CONSTRAINT [DF_Mc_Action_GroupInDetailMenu] DEFAULT ((0)),
		[HighlightInDetailMenu] [bit] NOT NULL CONSTRAINT [DF_Mc_Action_HighlightInDetailMenu] DEFAULT ((0)),
		[BuiltIn] [bit] NOT NULL CONSTRAINT [DF_Mc_Action_BuiltIn] DEFAULT ((0)),
		[Deleted] [bit] NOT NULL CONSTRAINT [DF_Mc_Action_Deleted] DEFAULT ((0))
	)

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Action] ([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted])
	SELECT [ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]
	FROM [dbo].[Mc_Action]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP TABLE [dbo].[Mc_Action]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC sp_rename N'[dbo].[tmp_Mc_Action]', N'Mc_Action'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Action] ADD CONSTRAINT [PK_Mc_Action] PRIMARY KEY CLUSTERED ([ActionId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_RolesSettings_Mc_Setting')
		ALTER TABLE [dbo].[Mc_RolesSettings] DROP CONSTRAINT [FK_Mc_RolesSettings_Mc_Setting]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Setting_Mc_Setting')
		ALTER TABLE [dbo].[Mc_Setting] DROP CONSTRAINT [FK_Mc_Setting_Mc_Setting]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_SettingListsValues_Mc_Setting')
		ALTER TABLE [dbo].[Mc_SettingListsValues] DROP CONSTRAINT [FK_Mc_SettingListsValues_Mc_Setting]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Setting_Mc_Setting')
		ALTER TABLE [dbo].[Mc_Setting] DROP CONSTRAINT [FK_Mc_Setting_Mc_Setting]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Setting_SettingId')
		ALTER TABLE [dbo].[Mc_Setting] DROP CONSTRAINT [DF_Mc_Setting_SettingId]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Setting_Description')
		ALTER TABLE [dbo].[Mc_Setting] DROP CONSTRAINT [DF_Mc_Setting_Description]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Setting_DefaultValue')
		ALTER TABLE [dbo].[Mc_Setting] DROP CONSTRAINT [DF_Mc_Setting_DefaultValue]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Setting_OrderNumber')
		ALTER TABLE [dbo].[Mc_Setting] DROP CONSTRAINT [DF_Mc_Setting_OrderNumber]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Setting_EnableOrganization')
		ALTER TABLE [dbo].[Mc_Setting] DROP CONSTRAINT [DF_Mc_Setting_EnableOrganization]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Setting_EnableDepartment')
		ALTER TABLE [dbo].[Mc_Setting] DROP CONSTRAINT [DF_Mc_Setting_EnableDepartment]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Setting_EnableRole')
		ALTER TABLE [dbo].[Mc_Setting] DROP CONSTRAINT [DF_Mc_Setting_EnableRole]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Setting_EnableGroup')
		ALTER TABLE [dbo].[Mc_Setting] DROP CONSTRAINT [DF_Mc_Setting_EnableGroup]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Setting_BuiltIn')
		ALTER TABLE [dbo].[Mc_Setting] DROP CONSTRAINT [DF_Mc_Setting_BuiltIn]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Setting_Deleted')
		ALTER TABLE [dbo].[Mc_Setting] DROP CONSTRAINT [DF_Mc_Setting_Deleted]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE TABLE [dbo].[tmp_Mc_Setting] (
	[SettingId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Mc_Setting_SettingId] DEFAULT (newid()),
	[ParentSettingId] [uniqueidentifier] NULL,
	[SettingTypeId] [int] NOT NULL,
	[Name] [nvarchar] (255) NOT NULL,
	[Description] [nvarchar] (1024) NOT NULL CONSTRAINT [DF_Mc_Setting_Description] DEFAULT (N''''),
	[ShortName] [nvarchar] (50) NOT NULL,
	[DefaultValue] [nvarchar] (512) NOT NULL CONSTRAINT [DF_Mc_Setting_DefaultValue] DEFAULT (N''''),
	[Value] [nvarchar] (' + @NVarCharMax + ') NULL,
	[OrderNumber] [int] NOT NULL CONSTRAINT [DF_Mc_Setting_OrderNumber] DEFAULT ((0)),
	[EnableOrganization] [bit] NOT NULL CONSTRAINT [DF_Mc_Setting_EnableOrganization] DEFAULT ((0)),
	[EnableInstance] [bit] NOT NULL CONSTRAINT [DF_Mc_Setting_EnableDepartment] DEFAULT ((0)),
	[EnableRole] [bit] NOT NULL CONSTRAINT [DF_Mc_Setting_EnableRole] DEFAULT ((0)),
	[EnableGroup] [bit] NOT NULL CONSTRAINT [DF_Mc_Setting_EnableGroup] DEFAULT ((0)),
	[BuiltIn] [bit] NOT NULL CONSTRAINT [DF_Mc_Setting_BuiltIn] DEFAULT ((0)),
	[Deleted] [bit] NOT NULL CONSTRAINT [DF_Mc_Setting_Deleted] DEFAULT ((0))
)')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Setting] ([SettingId], [ParentSettingId], [SettingTypeId], [Name], [Description], [ShortName], [DefaultValue], [Value], [OrderNumber], [EnableOrganization], EnableInstance, [EnableRole], [EnableGroup], [BuiltIn], [Deleted])
	SELECT [SettingId], [ParentSettingId], [SettingTypeId], [Name], [Description], [ShortName], [DefaultValue], [Value], [OrderNumber], [EnableOrganization], EnableDepartment, [EnableRole], [EnableGroup], [BuiltIn], [Deleted]
	FROM [dbo].[Mc_Setting]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP TABLE [dbo].[Mc_Setting]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC sp_rename N'[dbo].[tmp_Mc_Setting]', N'Mc_Setting'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Setting] ADD CONSTRAINT [PK_Mc_Setting] PRIMARY KEY CLUSTERED ([SettingId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Action_Mc_Action')
		ALTER TABLE [dbo].[Mc_Action] ADD CONSTRAINT [FK_Mc_Action_Mc_Action] FOREIGN KEY ([ParentActionId]) REFERENCES [dbo].[Mc_Action] ([ActionId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Action_Mc_Action')
		ALTER TABLE [dbo].[Mc_Action] ADD CONSTRAINT [FK_Mc_Action_Mc_Action] FOREIGN KEY ([ParentActionId]) REFERENCES [dbo].[Mc_Action] ([ActionId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_ActionsParentActions_Mc_Action_1')
		ALTER TABLE [dbo].[Mc_ActionsParentActions] ADD CONSTRAINT [FK_Mc_ActionsParentActions_Mc_Action_1] FOREIGN KEY ([ParentActionId]) REFERENCES [dbo].[Mc_Action] ([ActionId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_ActionsParentActions_Mc_Action_2')
		ALTER TABLE [dbo].[Mc_ActionsParentActions] ADD CONSTRAINT [FK_Mc_ActionsParentActions_Mc_Action_2] FOREIGN KEY ([ActionId]) REFERENCES [dbo].[Mc_Action] ([ActionId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Role_Mc_Action')
		ALTER TABLE [dbo].[Mc_Role] ADD CONSTRAINT [FK_Mc_Role_Mc_Action] FOREIGN KEY ([StartActionId]) REFERENCES [dbo].[Mc_Action] ([ActionId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_RolesActions_Mc_Action')
		ALTER TABLE [dbo].[Mc_RolesActions] ADD CONSTRAINT [FK_Mc_RolesActions_Mc_Action] FOREIGN KEY ([ActionId]) REFERENCES [dbo].[Mc_Action] ([ActionId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Setting_Mc_Setting')
		ALTER TABLE [dbo].[Mc_Setting] ADD CONSTRAINT [FK_Mc_Setting_Mc_Setting] FOREIGN KEY ([ParentSettingId]) REFERENCES [dbo].[Mc_Setting] ([SettingId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_RolesSettings_Mc_Setting')
		ALTER TABLE [dbo].[Mc_RolesSettings] ADD CONSTRAINT [FK_Mc_RolesSettings_Mc_Setting] FOREIGN KEY ([SettingId]) REFERENCES [dbo].[Mc_Setting] ([SettingId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Setting_Mc_Setting')
		ALTER TABLE [dbo].[Mc_Setting] ADD CONSTRAINT [FK_Mc_Setting_Mc_Setting] FOREIGN KEY ([ParentSettingId]) REFERENCES [dbo].[Mc_Setting] ([SettingId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_SettingListsValues_Mc_Setting')
		ALTER TABLE [dbo].[Mc_SettingListsValues] ADD CONSTRAINT [FK_Mc_SettingListsValues_Mc_Setting] FOREIGN KEY ([SettingId]) REFERENCES [dbo].[Mc_Setting] ([SettingId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetActions]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ActionId, ParentActionId, ActionTypeId, [Name], [Description], IconUrl, ButtonIconUrl, NavigateUrl, OrderNumber, ClassFullName, AuthenticationRequired, InstanceRequired, Visible, ShowInDetailMenu, ShowChildrenInDetailMenu, GroupInDetailMenu, HighlightInDetailMenu, BuiltIn, Deleted 
	FROM dbo.Mc_Action 
	WHERE (Deleted = 0) 
	ORDER BY ParentActionId, OrderNumber, [Name];
END')

IF @@ERROR <> 0
IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
EXEC('CREATE PROCEDURE [dbo].[Mc_GetSettings]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT SettingId, ParentSettingId, SettingTypeId, [Name], [Description], ShortName, DefaultValue, Value, OrderNumber, EnableOrganization, EnableInstance, EnableRole, EnableGroup, BuiltIn, Deleted 
	FROM dbo.Mc_Setting 
	WHERE (Deleted = 0);
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
EXEC('CREATE PROCEDURE [dbo].[Mc_InsertAction]
(
	@ActionId uniqueidentifier,
	@ParentActionId uniqueidentifier,
	@ActionTypeId int,
	@Name nvarchar(1024),
	@Description nvarchar(1024),
	@IconUrl nvarchar(2048),
	@ButtonIconUrl nvarchar(2048),
	@NavigateUrl nvarchar(2048),
	@OrderNumber int,
	@ClassFullName nvarchar(1024),
	@AuthenticationRequired bit,
	@InstanceRequired bit,
	@Visible bit,
	@ShowInDetailMenu bit,
	@ShowChildrenInDetailMenu bit,
	@GroupInDetailMenu bit,
	@HighlightInDetailMenu bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Action (ActionId, ParentActionId, ActionTypeId, [Name], [Description], IconUrl, ButtonIconUrl, NavigateUrl, OrderNumber, ClassFullName, AuthenticationRequired, InstanceRequired, Visible, ShowInDetailMenu, ShowChildrenInDetailMenu, GroupInDetailMenu, HighlightInDetailMenu, BuiltIn, Deleted) 
	VALUES (@ActionId, @ParentActionId, @ActionTypeId, @Name, @Description, @IconUrl, @ButtonIconUrl, @NavigateUrl, @OrderNumber, @ClassFullName, @AuthenticationRequired, @InstanceRequired, @Visible, @ShowInDetailMenu, @ShowChildrenInDetailMenu, @GroupInDetailMenu, @HighlightInDetailMenu, 0, @Deleted);

	SELECT ActionId, ParentActionId, ActionTypeId, [Name], [Description], IconUrl, ButtonIconUrl, NavigateUrl, OrderNumber, ClassFullName, AuthenticationRequired, InstanceRequired, Visible, ShowInDetailMenu, ShowChildrenInDetailMenu, GroupInDetailMenu, HighlightInDetailMenu, BuiltIn, Deleted 
	FROM dbo.Mc_Action 
	WHERE (ActionId = @ActionId);
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertSetting]
(
	@SettingId uniqueidentifier,
	@ParentSettingId uniqueidentifier,
	@SettingTypeId int,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@ShortName nvarchar(50),
	@DefaultValue nvarchar(512),
	@Value nvarchar(max),
	@OrderNumber int,
	@EnableOrganization bit,
	@EnableInstance bit,
	@EnableRole bit,
	@EnableGroup bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Setting (SettingId, ParentSettingId, SettingTypeId, [Name], [Description], ShortName, DefaultValue, Value, OrderNumber, EnableOrganization, EnableInstance, EnableRole, EnableGroup, BuiltIn, Deleted) 
	VALUES (@SettingId, @ParentSettingId, @SettingTypeId, @Name, @Description, @ShortName, @DefaultValue, @Value, @OrderNumber, @EnableOrganization, @EnableInstance, @EnableRole, @EnableGroup, 0, @Deleted);

	SELECT SettingId, ParentSettingId, SettingTypeId, [Name], [Description], ShortName, DefaultValue, Value, OrderNumber, EnableOrganization, EnableInstance, EnableRole, EnableGroup, BuiltIn, Deleted 
	FROM dbo.Mc_Setting 
	WHERE (SettingId = @SettingId);
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateAction]
(
	@ActionId uniqueidentifier,
	@ParentActionId uniqueidentifier,
	@ActionTypeId int,
	@Name nvarchar(1024),
	@Description nvarchar(1024),
	@IconUrl nvarchar(2048),
	@ButtonIconUrl nvarchar(2048),
	@NavigateUrl nvarchar(2048),
	@OrderNumber int,
	@ClassFullName nvarchar(1024),
	@AuthenticationRequired bit,
	@InstanceRequired bit,
	@Visible bit,
	@ShowInDetailMenu bit,
	@ShowChildrenInDetailMenu bit,
	@GroupInDetailMenu bit,
	@HighlightInDetailMenu bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Action 
	SET ParentActionId = @ParentActionId, ActionTypeId = @ActionTypeId, [Name] = @Name, [Description] = @Description, IconUrl = @IconUrl, ButtonIconUrl = @ButtonIconUrl, NavigateUrl = @NavigateUrl, OrderNumber = @OrderNumber, ClassFullName = @ClassFullName, AuthenticationRequired = @AuthenticationRequired, InstanceRequired = @InstanceRequired, Visible = @Visible, ShowInDetailMenu = @ShowInDetailMenu, ShowChildrenInDetailMenu = @ShowChildrenInDetailMenu, GroupInDetailMenu = @GroupInDetailMenu, HighlightInDetailMenu = @HighlightInDetailMenu, Deleted = @Deleted 
	WHERE (ActionId = @ActionId);

	SELECT ActionId, ParentActionId, ActionTypeId, [Name], [Description], IconUrl, ButtonIconUrl, NavigateUrl, OrderNumber, ClassFullName, AuthenticationRequired, InstanceRequired, Visible, ShowInDetailMenu, ShowChildrenInDetailMenu, GroupInDetailMenu, HighlightInDetailMenu, BuiltIn, Deleted 
	FROM dbo.Mc_Action 
	WHERE (ActionId = @ActionId);
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateSetting]
(
	@SettingId uniqueidentifier,
	@ParentSettingId uniqueidentifier,
	@SettingTypeId int,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@ShortName nvarchar(50),
	@DefaultValue nvarchar(512),
	@Value nvarchar(' + @NVarCharMax + '),
	@OrderNumber int,
	@EnableOrganization bit,
	@EnableInstance bit,
	@EnableRole bit,
	@EnableGroup bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Setting 
	SET ParentSettingId = @ParentSettingId, SettingTypeId = @SettingTypeId, [Name] = @Name, [Description] = @Description, ShortName = @ShortName, DefaultValue = @DefaultValue, Value = @Value, OrderNumber = @OrderNumber, EnableOrganization = @EnableOrganization, EnableInstance = @EnableInstance, EnableRole = @EnableRole, EnableGroup = @EnableGroup , Deleted = @Deleted 
	WHERE (SettingId = @SettingId);

	SELECT SettingId, ParentSettingId, SettingTypeId, [Name], [Description], ShortName, DefaultValue, Value, OrderNumber, EnableOrganization, EnableInstance, EnableRole, EnableGroup, BuiltIn, Deleted 
	FROM dbo.Mc_Setting 
	WHERE (SettingId = @SettingId);
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Name] = N'Instances', [Description] = N'Manage the Instances', [NavigateUrl] = N'/Resources.Micajah.Common/Pages/Admin/Instance.aspx' WHERE [ActionId] = '{00000000-0000-0000-0000-000000000010}'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Description] = N'Manage the Group Roles in Instances', [NavigateUrl] = N'/Resources.Micajah.Common/Pages/Admin/GroupsInstancesRoles.aspx' WHERE [ActionId] = '{00000000-0000-0000-0000-000000000013}'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Name] = N'Instance Options', [Description] = N'The instance specific settings', [NavigateUrl] = N'/Resources.Micajah.Common/Pages/Admin/InstanceSettings.aspx' WHERE [ActionId] = '{00000000-0000-0000-0000-000000000016}'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Name] = N'Group Settings in Instance', [Description] = N'Manage the Settings of the Group in Instance', [NavigateUrl] = N'/Resources.Micajah.Common/Pages/Admin/GroupSettingsInInstance.aspx' WHERE [ActionId] = '{00000000-0000-0000-0000-000000000017}'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Description] = N'Diagnose the Settings of the Users or Groups in Instances' WHERE [ActionId] = '{00000000-0000-0000-0000-000000000018}'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Description] = N'Diagnose the Settings of the Users or Groups in current Instance' WHERE [ActionId] = '{00000000-0000-0000-0000-000000000019}'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Role] SET [Name] = N'Instance Administrator', [Description] = N'Built-in Role for administering the Instance', [ShortName] = N'InstAdmin' WHERE [RoleId] = '{00000000-0000-0000-0000-000000000002}'

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
