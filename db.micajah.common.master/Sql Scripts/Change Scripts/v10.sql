BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Action_Mc_Action')
      ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [FK_Mc_Action_Mc_Action]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_ActionsParentActions_ActionId_Mc_Action')
      ALTER TABLE [dbo].[Mc_ActionsParentActions] DROP CONSTRAINT [FK_Mc_ActionsParentActions_ActionId_Mc_Action]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_ActionsParentActions_ParentActionId_Mc_Action')
      ALTER TABLE [dbo].[Mc_ActionsParentActions] DROP CONSTRAINT [FK_Mc_ActionsParentActions_ParentActionId_Mc_Action]

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
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Action_Deleted')
      ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [DF_Mc_Action_Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE TABLE [dbo].[tmp_Mc_Action] (
	   [ActionId] [int] IDENTITY (100, 1) NOT NULL,
	   [ParentActionId] [int] NULL,
	   [ActionTypeId] [int] NOT NULL,
	   [Name] [nvarchar] (1024) NOT NULL,
	   [Description] [nvarchar] (1024) NOT NULL CONSTRAINT [DF_Mc_Action_Description] DEFAULT (N''),
	   [IconUrl] [nvarchar] (2048) NOT NULL CONSTRAINT [DF_Mc_Action_IconUrl] DEFAULT (N''),
	   [ButtonIconUrl] [nvarchar] (2048) NOT NULL CONSTRAINT [DF_Mc_Action_ButtonIconUrl] DEFAULT (N''),
	   [NavigateUrl] [nvarchar] (2048) NULL,
	   [OrderNumber] [int] NOT NULL CONSTRAINT [DF_Mc_Action_OrderNumber] DEFAULT ((0)),
	   [ClassFullName] [nvarchar] (1024) NOT NULL CONSTRAINT [DF_Mc_Action_ClassFullName] DEFAULT (N''),
	   [AuthenticationRequired] [bit] NOT NULL CONSTRAINT [DF_Mc_Action_AuthenticationRequired] DEFAULT ((0)),
	   [DepartmentRequired] [bit] NOT NULL CONSTRAINT [DF_Mc_Action_DepartmentRequired] DEFAULT ((1)),
	   [Visible] [bit] NOT NULL CONSTRAINT [DF_Mc_Action_Visible] DEFAULT ((1)),
	   [ShowInDetailMenu] [bit] NOT NULL CONSTRAINT [DF_Mc_Action_ShowInDetailMenu] DEFAULT ((1)),
	   [ShowChildrenInDetailMenu] [bit] NOT NULL CONSTRAINT [DF_Mc_Action_ShowChildrenInDetailMenu] DEFAULT ((0)),
	   [GroupInDetailMenu] [bit] NOT NULL CONSTRAINT [DF_Mc_Action_GroupInDetailMenu] DEFAULT ((0)),
	   [HighlightInDetailMenu] [bit] NOT NULL CONSTRAINT [DF_Mc_Action_HighlightInDetailMenu] DEFAULT ((0)),
	   [Deleted] [bit] NOT NULL CONSTRAINT [DF_Mc_Action_Deleted] DEFAULT ((0))
	)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   SET IDENTITY_INSERT [dbo].[tmp_Mc_Action] ON

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   INSERT INTO [dbo].[tmp_Mc_Action] ([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [Deleted])
   SELECT [ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName]
   , CASE WHEN [ActionId] IN (1, 7) THEN 0 ELSE 1 END
   , [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [Deleted]
   FROM [dbo].[Mc_Action]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [Deleted]) VALUES (27, 7, 1, N'Setup', N'', N'', N'', N'', 0, N'', 0, 0, 0, 0, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [Deleted]) VALUES (28, 27, 1, N'Framework', N'', N'', N'', N'/Resources.Micajah.Common/Pages/Setup/Framework.aspx', 1, N'', 0, 0, 1, 1, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [Deleted]) VALUES (29, 27, 1, N'Global Settings', N'', N'', N'', N'/Resources.Micajah.Common/Pages/Setup/GlobalSettings.aspx', 2, N'', 0, 0, 1, 1, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [Deleted]) VALUES (30, 27, 1, N'Actions', N'', N'', N'', N'/Resources.Micajah.Common/Pages/Setup/Actions.aspx', 3, N'', 0, 0, 1, 1, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [Deleted]) VALUES (31, 27, 1, N'Roles', N'', N'', N'', N'/Resources.Micajah.Common/Pages/Setup/Roles.aspx', 4, N'', 0, 0, 1, 1, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [Deleted]) VALUES (32, 31, 1, N'Edit Role', N'', N'', N'', N'/Resources.Micajah.Common/Pages/Setup/RoleEdit.aspx', 0, N'', 0, 0, 1, 0, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [Deleted]) VALUES (33, 31, 1, N'Edit Role Settings', N'', N'', N'', N'/Resources.Micajah.Common/Pages/Setup/RoleSettings.aspx', 0, N'', 0, 0, 1, 0, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [Deleted]) VALUES (34, 27, 1, N'Settings', N'', N'', N'', N'/Resources.Micajah.Common/Pages/Setup/Settings.aspx', 5, N'', 0, 0, 1, 1, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [Deleted]) VALUES (35, 34, 1, N'Edit Setting', N'', N'', N'', N'/Resources.Micajah.Common/Pages/Setup/SettingEdit.aspx', 0, N'', 0, 0, 1, 0, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [Deleted]) VALUES (36, 34, 1, N'Edit Setting Values', N'', N'', N'', N'/Resources.Micajah.Common/Pages/Setup/SettingListValues.aspx', 0, N'', 0, 0, 1, 0, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [Deleted]) VALUES (37, 27, 1, N'Websites', N'', N'', N'', N'/Resources.Micajah.Common/Pages/Setup/Websites.aspx', 6, N'', 0, 0, 1, 1, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [Deleted]) VALUES (38, 27, 1, N'Database Servers', N'', N'', N'', N'/Resources.Micajah.Common/Pages/Setup/SqlServers.aspx', 7, N'', 0, 0, 1, 1, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [Deleted]) VALUES (39, 27, 1, N'Databases', N'', N'', N'', N'/Resources.Micajah.Common/Pages/Setup/Databases.aspx', 8, N'', 0, 0, 1, 1, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [Deleted]) VALUES (40, 27, 1, N'Organizations', N'', N'', N'', N'/Resources.Micajah.Common/Pages/Setup/Organizations.aspx', 9, N'', 0, 0, 1, 1, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [Deleted]) VALUES (41, 27, 1, N'Login as User', N'', N'', N'', N'/Resources.Micajah.Common/Pages/Security/LoginAsUser.aspx', 10, N'', 0, 0, 1, 1, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [Deleted]) VALUES (42, 1, 3, N'Setup', N'', N'', N'', N'/Resources.Micajah.Common/Pages/Setup/Framework.aspx', 10001, N'', 0, 0, 1, 0, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [DepartmentRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [Deleted]) VALUES (43, 1, 3, N'Login', N'', N'', N'', N'/Resources.Micajah.Common/Pages/Security/Login.aspx', 10002, N'', 0, 0, 1, 0, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   SET IDENTITY_INSERT [dbo].[tmp_Mc_Action] OFF

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
	EXEC('/****** Object:  StoredProcedure [dbo].[Mc_InsertAction] ******/
ALTER PROCEDURE [dbo].[Mc_InsertAction]
(
	@ParentActionId int,
	@ActionTypeId int,
	@Name nvarchar(1024),
	@Description nvarchar(1024),
	@IconUrl nvarchar(2048),
	@ButtonIconUrl nvarchar(2048),
	@NavigateUrl nvarchar(2048),
	@OrderNumber int,
	@ClassFullName nvarchar(1024),
	@AuthenticationRequired bit,
	@DepartmentRequired bit,
	@Visible bit,
	@ShowInDetailMenu bit,
	@ShowChildrenInDetailMenu bit,
	@GroupInDetailMenu bit,
	@HighlightInDetailMenu bit,
	@Deleted bit
)
AS
	SET NOCOUNT OFF;
INSERT INTO [dbo].[Mc_Action] (ParentActionId, ActionTypeId, [Name], [Description], IconUrl, ButtonIconUrl, NavigateUrl, OrderNumber, ClassFullName, AuthenticationRequired, DepartmentRequired, Visible, ShowInDetailMenu, ShowChildrenInDetailMenu, GroupInDetailMenu, HighlightInDetailMenu , Deleted) VALUES (@ParentActionId, @ActionTypeId, @Name, @Description, @IconUrl, @ButtonIconUrl, @NavigateUrl, @OrderNumber, @ClassFullName, @AuthenticationRequired, @DepartmentRequired, @Visible, @ShowInDetailMenu, @ShowChildrenInDetailMenu, @GroupInDetailMenu, @HighlightInDetailMenu, @Deleted);
	
SELECT ActionId, ParentActionId, ActionTypeId, [Name], [Description], IconUrl, ButtonIconUrl, NavigateUrl, OrderNumber, ClassFullName, AuthenticationRequired, DepartmentRequired, Visible, ShowInDetailMenu, ShowChildrenInDetailMenu, GroupInDetailMenu, HighlightInDetailMenu, Deleted FROM dbo.Mc_Action WHERE (ActionId = SCOPE_IDENTITY())')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('/****** Object:  StoredProcedure [dbo].[Mc_SelectAllAction] ******/
ALTER PROCEDURE [dbo].[Mc_SelectAllAction]
AS
	SET NOCOUNT ON;
SELECT ActionId, ParentActionId, ActionTypeId, [Name], [Description], IconUrl, ButtonIconUrl, NavigateUrl, OrderNumber, ClassFullName, AuthenticationRequired, DepartmentRequired, Visible, ShowInDetailMenu, ShowChildrenInDetailMenu, GroupInDetailMenu, HighlightInDetailMenu, Deleted FROM dbo.Mc_Action WHERE (Deleted = 0) ORDER BY ParentActionId, OrderNumber, [Name]')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('/****** Object:  StoredProcedure [dbo].[Mc_UpdateAction] ******/
ALTER PROCEDURE [dbo].[Mc_UpdateAction]
(
	@ActionId int,
	@ParentActionId int,
	@ActionTypeId int,
	@Name nvarchar(1024),
	@Description nvarchar(1024),
	@IconUrl nvarchar(2048),
	@ButtonIconUrl nvarchar(2048),
	@NavigateUrl nvarchar(2048),
	@OrderNumber int,
	@ClassFullName nvarchar(1024),
	@AuthenticationRequired bit,
	@DepartmentRequired bit,
	@Visible bit,
	@ShowInDetailMenu bit,
	@ShowChildrenInDetailMenu bit,
	@GroupInDetailMenu bit,
	@HighlightInDetailMenu bit,
	@Deleted bit
)
AS
	SET NOCOUNT OFF;
UPDATE [dbo].[Mc_Action] SET ParentActionId = @ParentActionId, ActionTypeId = @ActionTypeId, [Name] = @Name, [Description] = @Description, IconUrl = @IconUrl, ButtonIconUrl = @ButtonIconUrl, NavigateUrl = @NavigateUrl, OrderNumber = @OrderNumber, ClassFullName = @ClassFullName, AuthenticationRequired = @AuthenticationRequired, DepartmentRequired = @DepartmentRequired, Visible = @Visible, ShowInDetailMenu = @ShowInDetailMenu, ShowChildrenInDetailMenu = @ShowChildrenInDetailMenu, GroupInDetailMenu = @GroupInDetailMenu, HighlightInDetailMenu = @HighlightInDetailMenu, Deleted = @Deleted WHERE ([ActionId] = @ActionId);
	
SELECT ActionId, ParentActionId, ActionTypeId, [Name], [Description], IconUrl, ButtonIconUrl, NavigateUrl, OrderNumber, ClassFullName, AuthenticationRequired, DepartmentRequired, Visible, ShowInDetailMenu, ShowChildrenInDetailMenu, GroupInDetailMenu, HighlightInDetailMenu, Deleted FROM dbo.Mc_Action WHERE (ActionId = @ActionId)')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Action_Mc_Action')
      ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [FK_Mc_Action_Mc_Action]

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
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_ActionsParentActions_ActionId_Mc_Action')
      ALTER TABLE [dbo].[Mc_ActionsParentActions] ADD CONSTRAINT [FK_Mc_ActionsParentActions_ActionId_Mc_Action] FOREIGN KEY ([ActionId]) REFERENCES [dbo].[Mc_Action] ([ActionId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_ActionsParentActions_ParentActionId_Mc_Action')
      ALTER TABLE [dbo].[Mc_ActionsParentActions] ADD CONSTRAINT [FK_Mc_ActionsParentActions_ParentActionId_Mc_Action] FOREIGN KEY ([ParentActionId]) REFERENCES [dbo].[Mc_Action] ([ActionId])

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
   COMMIT TRANSACTION