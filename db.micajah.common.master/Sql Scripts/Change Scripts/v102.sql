BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Login]
      ADD [Token] [varchar] (50) NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetLogin]
(
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT LoginId, FirstName, LastName, LoginName, [Password], ProfileUpdated, Deleted, SessionId, Token
	FROM dbo.Mc_Login
	WHERE LoginId = @LoginId;
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetLoginByLoginName]
(
	@LoginName nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.ProfileUpdated, l.Deleted, l.SessionId, l.Token
	FROM dbo.Mc_Login AS l
	LEFT JOIN dbo.Mc_Email AS e 
		ON l.LoginId = e.LoginId AND e.Email = @LoginName
	WHERE (l.LoginName = @LoginName OR e.Email IS NOT NULL);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetLoginsByOrganizationId]      
(      
	@OrganizationId uniqueidentifier      
)      
AS      
BEGIN      
	SET NOCOUNT OFF;      

	SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.ProfileUpdated, l.Deleted, l.SessionId, l.Token
		, ol.OrganizationAdministrator, ol.LdapDomain, ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, ol.LdapUserId, ol.LdapOUPath, ol.Active      
	FROM dbo.Mc_OrganizationsLogins AS ol      
	INNER JOIN dbo.Mc_Login AS l      
		ON (ol.LoginId = l.LoginId) AND (ol.OrganizationId = @OrganizationId) AND (l.Deleted = 0);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertLogin]
(
	@LoginId uniqueidentifier,
	@LoginName nvarchar(255),
	@Password nvarchar(50),
	@Token varchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Login (LoginId, LoginName, [Password], Deleted, Token) 
	VALUES (@LoginId, @LoginName, @Password, 0, @Token);
	
	SELECT LoginId, FirstName, LastName, LoginName, [Password], ProfileUpdated, Deleted, SessionId, Token
	FROM dbo.Mc_Login 
	WHERE (LoginId = @LoginId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateLoginToken]
(
	@LoginId uniqueidentifier,
	@Token varchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Login
	SET Token = @Token
	WHERE LoginId = @LoginId;
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
