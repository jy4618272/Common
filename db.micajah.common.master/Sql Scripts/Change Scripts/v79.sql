BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Login]
      ADD [SessionId] [varchar] (50) NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_ViewState_Mc_Session')
      ALTER TABLE [dbo].[Mc_ViewState] DROP CONSTRAINT [FK_Mc_ViewState_Mc_Session]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP TABLE [dbo].[Mc_ViewState]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE TABLE [dbo].[Mc_ViewState] (
   [ViewStateId] [uniqueidentifier] NOT NULL,
   [SessionId] [varchar] (50) NOT NULL,
   [ViewState] [varbinary] (max) NULL,
   [ExpirationTime] [datetime] NOT NULL
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_ViewState] ADD CONSTRAINT [PK_Mc_ViewState] PRIMARY KEY CLUSTERED ([ViewStateId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_ViewState_Mc_Session')
      ALTER TABLE [dbo].[Mc_ViewState] DROP CONSTRAINT [FK_Mc_ViewState_Mc_Session]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP TABLE [dbo].[Mc_Session]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP PROCEDURE [dbo].[Mc_UpdateSessionLogin]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP PROCEDURE [dbo].[Mc_DeleteSession]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteViewState]
(
    @SessionId varchar(50),
    @Now datetime
)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @SessionId IS NULL
		DELETE dbo.Mc_ViewState
		WHERE ExpirationTime < @Now;
    ELSE
		DELETE dbo.Mc_ViewState
		WHERE SessionId = @SessionId;
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetLogin]    
(    
	@LoginId uniqueidentifier,    
	@LoginName nvarchar(255),    
	@Password nvarchar(50)    
)    
AS    
BEGIN    
	SET NOCOUNT ON;    

	SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.ProfileUpdated, l.SessionId, l.Deleted    
		, CASE WHEN EXISTS(    
		SELECT TOP 1 o.OrganizationId    
		FROM dbo.Mc_Organization AS o    
		INNER JOIN dbo.Mc_OrganizationsLogins AS ol    
			ON (o.OrganizationId = ol.OrganizationId) AND (ol.LoginId = l.LoginId) AND (o.Deleted = 0)    
		) THEN 1 ELSE 0 END AS OrganizationExists    
	FROM dbo.Mc_Login AS l    
	WHERE     
		((@LoginId IS NULL) OR (l.LoginId = @LoginId))    
		AND ((@LoginName IS NULL) OR (l.LoginName = @LoginName))    
		AND ((@Password IS NULL) OR (l.[Password] = @Password));    
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
	SET NOCOUNT ON;      

	SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.ProfileUpdated, l.SessionId, l.Deleted
		, ol.OrganizationAdministrator, ol.LdapDomain, ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, ol.LdapUserId, ol.LdapOUPath, ol.Active      
	FROM dbo.Mc_OrganizationsLogins AS ol      
	INNER JOIN dbo.Mc_Login AS l      
		ON (ol.LoginId = l.LoginId) AND (ol.OrganizationId = @OrganizationId) AND (l.Deleted = 0);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateViewState]
(
	@ViewStateId uniqueidentifier,
    @SessionId varchar(50),
    @ViewState varbinary(max),
    @ExpirationTime datetime
)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS(
		SELECT 0
		FROM dbo.Mc_ViewState
		WHERE (ViewStateId = @ViewStateId) AND (SessionId = @SessionId)
    )
		UPDATE dbo.Mc_ViewState
		SET ViewState = @ViewState, ExpirationTime = @ExpirationTime
		WHERE ViewStateId = @ViewStateId;
    ELSE
		INSERT INTO dbo.Mc_ViewState (ViewStateId, SessionId, ViewState, ExpirationTime) 
		VALUES (@ViewStateId, @SessionId, @ViewState, @ExpirationTime);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP PROCEDURE [dbo].[Mc_UpdateSession]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetViewState]
(
    @ViewStateId uniqueidentifier
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ViewState
    FROM dbo.Mc_ViewState 
    WHERE (ViewStateId = @ViewStateId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateLogin]
(
	@LoginId uniqueidentifier,
	@LoginName nvarchar(255),
	@Password nvarchar(50),
	@FirstName nvarchar(255),
	@LastName nvarchar(255),
	@Deleted bit
)  
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Login
	SET FirstName = (CASE WHEN @FirstName IS NULL THEN FirstName ELSE @FirstName END)
		, LastName = (CASE WHEN @LastName IS NULL THEN LastName ELSE @LastName END)
		, LoginName = (CASE WHEN @LoginName IS NULL THEN LoginName ELSE @LoginName END)
		, [Password] = (CASE WHEN @Password IS NULL THEN [Password] ELSE @Password END)
		, Deleted = (CASE WHEN @Deleted IS NULL THEN Deleted ELSE @Deleted END)
	WHERE (LoginId = @LoginId);

	SELECT LoginId, FirstName, LastName, LoginName, [Password], SessionId, Deleted
	FROM dbo.Mc_Login
	WHERE (LoginId = @LoginId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateLoginSession]
(
	@LoginId uniqueidentifier,
	@SessionId varchar(50)
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.Mc_Login
	SET SessionId = @SessionId
	WHERE LoginId = @LoginId;
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP PROCEDURE [dbo].[Mc_ValidateSession]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION