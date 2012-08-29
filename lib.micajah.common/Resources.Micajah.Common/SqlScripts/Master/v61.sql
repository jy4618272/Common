BEGIN TRANSACTION

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL, XACT_ABORT ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('{5749FF70-4592-4B91-8579-5E7E203C0410}', '{00000000-0000-0000-0000-000000000012}', 1, N'User Ldap Info', N'Edit user''s ldap domain, alias, SID and GUID information', N'', N'', N'/mc/Admin/UserLdapInfo.aspx', 4, N'', 1, 0, 1, 1, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_OrganizationsLogins] ADD [LdapDomain] [nvarchar] (255) NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_OrganizationsLogins] ADD [LdapUserAlias] [nvarchar] (255) NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_OrganizationsLogins] ADD [LdapSecurityId] [uniqueidentifier] NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_OrganizationsLogins] ADD [LdapUserId] [uniqueidentifier] NULL

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizationLogin]
(
	@OrganizationId uniqueidentifier,
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT OrganizationId, LoginId, OrganizationAdministrator, LdapDomain, LdapUserAlias, LdapSecurityId, LdapUserId, Active
	FROM dbo.Mc_OrganizationsLogins
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateLoginLdapInfo]
(
	@OrganizationId uniqueidentifier,
	@LoginId uniqueidentifier,
	@LdapDomain nvarchar(255),
	@LdapUserAlias nvarchar(255),
	@LdapSecurityId uniqueidentifier,
	@LdapUserId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_OrganizationsLogins 
	SET LdapDomain = @LdapDomain,
		LdapUserAlias = @LdapUserAlias,
		LdapSecurityId = @LdapSecurityId,
		LdapUserId = @LdapUserId 
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
	
	SELECT OrganizationId, LoginId, OrganizationAdministrator, LdapDomain, LdapUserAlias, LdapSecurityId, LdapUserId, Active
	FROM dbo.Mc_OrganizationsLogins
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateOrganizationLogin]
(
	@OrganizationId uniqueidentifier,
	@LoginId uniqueidentifier,
	@OrganizationAdministrator bit,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_OrganizationsLogins 
	SET OrganizationAdministrator = (CASE WHEN @OrganizationAdministrator IS NULL THEN OrganizationAdministrator ELSE @OrganizationAdministrator END)
		, Active = (CASE WHEN @Active IS NULL THEN Active ELSE @Active END)
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
	
	SELECT OrganizationId, LoginId, OrganizationAdministrator, LdapDomain, LdapUserAlias, LdapSecurityId, LdapUserId, Active
	FROM dbo.Mc_OrganizationsLogins
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetLoginsByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT l.LoginId, l.LoginName, l.[Password], l.Deleted, ol.OrganizationAdministrator, ol.LdapDomain, ol.LdapUserAlias, ol.LdapSecurityId, ol.LdapUserId, ol.Active
	FROM dbo.Mc_OrganizationsLogins AS ol
	INNER JOIN dbo.Mc_Login AS l
		ON	(ol.LoginId = l.LoginId) AND (ol.OrganizationId = @OrganizationId) AND (l.Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
