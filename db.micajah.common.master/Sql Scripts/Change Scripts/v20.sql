BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Setting] SET [DefaultValue] = N'Gray' WHERE SettingId = '00000000-0000-0000-0000-000000000065'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('C2D9FFFB-9588-494F-8B8B-4A84D61EACE1', '00000000-0000-0000-0000-000000000012', 1, N'Invite Users', N'Invite Users', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/InviteUsers.aspx', 0, N'', 0, 0, 0, 0, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE TABLE [dbo].[Mc_InvitedLogin] (
	[InvitedLoginId] [uniqueidentifier] NOT NULL,
	[LoginName] [nvarchar](255) NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[GroupId] [varchar](2056) NOT NULL,
	[InvitedBy] [uniqueidentifier] NOT NULL,
	[CreatedTime] [datetime] NOT NULL
)')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_InvitedLogin] ADD CONSTRAINT [PK_Mc_InvitedLogin] PRIMARY KEY CLUSTERED ([LoginName], [OrganizationId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
   
IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM dbo.sysobjects WHERE name = N'FK_Mc_InvitedLogin_Mc_Login')
      ALTER TABLE [dbo].[Mc_InvitedLogin] DROP CONSTRAINT [FK_Mc_InvitedLogin_Mc_Login]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM dbo.sysobjects WHERE name = N'FK_Mc_InvitedLogin_Mc_Login')
      ALTER TABLE [dbo].[Mc_InvitedLogin] ADD CONSTRAINT [FK_Mc_InvitedLogin_Mc_Login] FOREIGN KEY ([InvitedBy]) REFERENCES [dbo].[Mc_Login] ([LoginId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM dbo.sysobjects WHERE name = N'FK_Mc_InvitedLogin_Mc_Organization')
      ALTER TABLE [dbo].[Mc_InvitedLogin] DROP CONSTRAINT [FK_Mc_InvitedLogin_Mc_Organization]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM dbo.sysobjects WHERE name = N'FK_Mc_InvitedLogin_Mc_Organization')
      ALTER TABLE [dbo].[Mc_InvitedLogin] ADD CONSTRAINT [FK_Mc_InvitedLogin_Mc_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_InvitedLogin] ADD  CONSTRAINT [DF_Mc_InvitedLogin_InvitedLoginId]  DEFAULT (newid()) FOR [InvitedLoginId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_InvitedLogin] ADD  CONSTRAINT [DF_Mc_InvitedLogin_CreatedTime]  DEFAULT (getdate()) FOR [CreatedTime]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteInvitedLogin]
(
	@InvitedLoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_InvitedLogin
	WHERE 
		(InvitedLoginId = @InvitedLoginId)
		OR (DATEDIFF(DAY, CreatedTime, GETDATE()) >= 30);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetInvitedLogin]
(
	@InvitedLoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT InvitedLoginId, LoginName, OrganizationId, GroupId, InvitedBy, CreatedTime
	FROM dbo.Mc_InvitedLogin
	WHERE (InvitedLoginId = @InvitedLoginId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetInvitedLoginsByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT InvitedLoginId, LoginName, OrganizationId, GroupId, InvitedBy, CreatedTime
	FROM dbo.Mc_InvitedLogin
	WHERE (OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertInvitedLogin]
(
	@InvitedLoginId uniqueidentifier,
	@LoginName nvarchar(255),
	@OrganizationId uniqueidentifier,
	@GroupId varchar(2056),
	@InvitedBy uniqueidentifier,
	@CreatedTime datetime
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	INSERT INTO dbo.Mc_InvitedLogin	(InvitedLoginId, LoginName, OrganizationId, GroupId, InvitedBy, CreatedTime)
	VALUES (@InvitedLoginId, @LoginName, @OrganizationId, @GroupId, @InvitedBy, @CreatedTime)
	
	SELECT InvitedLoginId, LoginName, OrganizationId, GroupId, InvitedBy, CreatedTime
	FROM dbo.Mc_InvitedLogin
	WHERE (InvitedLoginId = @InvitedLoginId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION