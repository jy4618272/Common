BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Login
		DROP CONSTRAINT DF_Mc_Login_LoginId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Login
		DROP CONSTRAINT DF_Mc_Login_Deleted

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE dbo.Tmp_Mc_Login
	(
	LoginId uniqueidentifier NOT NULL CONSTRAINT [DF_Mc_Login_LoginId] DEFAULT (newid()),
	FirstName nvarchar(255) NOT NULL CONSTRAINT [DF_Mc_Login_FirstName] DEFAULT (N''),
	LastName nvarchar(255) NOT NULL CONSTRAINT [DF_Mc_Login_LastName] DEFAULT (N''),
	LoginName nvarchar(255) NOT NULL,
	Password nvarchar(50) NOT NULL,
	Deleted bit NOT NULL CONSTRAINT [DF_Mc_Login_Deleted] DEFAULT ((0))
	)  ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS(SELECT * FROM dbo.Mc_Login)
		 EXEC('INSERT INTO dbo.Tmp_Mc_Login (LoginId, LoginName, Password, Deleted)
			SELECT LoginId, LoginName, Password, Deleted FROM dbo.Mc_Login WITH (HOLDLOCK TABLOCKX)')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_OrganizationsLogins
		DROP CONSTRAINT FK_Mc_OrganizationsLogins_Mc_Login

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_InvitedLogin
		DROP CONSTRAINT FK_Mc_InvitedLogin_Mc_Login

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_ResetPasswordRequest
		DROP CONSTRAINT FK_Mc_ResetPasswordRequest_Mc_Login

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Session
		DROP CONSTRAINT FK_Mc_Session_Mc_Login

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP TABLE dbo.Mc_Login

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXECUTE sp_rename N'dbo.Tmp_Mc_Login', N'Mc_Login', 'OBJECT' 

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Login ADD CONSTRAINT
	PK_Mc_Login PRIMARY KEY CLUSTERED 
	(
		LoginId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Session ADD CONSTRAINT
	FK_Mc_Session_Mc_Login FOREIGN KEY
	(
		LoginId
	) REFERENCES dbo.Mc_Login
	(
		LoginId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_ResetPasswordRequest ADD CONSTRAINT
	FK_Mc_ResetPasswordRequest_Mc_Login FOREIGN KEY
	(
		LoginId
	) REFERENCES dbo.Mc_Login
	(
		LoginId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_InvitedLogin ADD CONSTRAINT
	FK_Mc_InvitedLogin_Mc_Login FOREIGN KEY
	(
		InvitedBy
	) REFERENCES dbo.Mc_Login
	(
		LoginId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_OrganizationsLogins ADD CONSTRAINT
	FK_Mc_OrganizationsLogins_Mc_Login FOREIGN KEY
	(
		LoginId
	) REFERENCES dbo.Mc_Login
	(
		LoginId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_OrganizationsLogins
		DROP CONSTRAINT FK_Mc_OrganizationsLogins_Mc_Login

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_OrganizationsLogins
		DROP CONSTRAINT FK_Mc_OrganizationsLogins_Mc_Organization

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_OrganizationsLogins
		DROP CONSTRAINT DF_Mc_OrganizationsLogins_OrganizationAdministrator

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_OrganizationsLogins
		DROP CONSTRAINT DF_Mc_OrganizationsLogins_Active

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE dbo.Tmp_Mc_OrganizationsLogins
	(
	OrganizationId uniqueidentifier NOT NULL,
	LoginId uniqueidentifier NOT NULL,
	OrganizationAdministrator bit NOT NULL CONSTRAINT [DF_Mc_OrganizationsLogins_OrganizationAdministrator] DEFAULT ((0)),
	Active bit NOT NULL CONSTRAINT [DF_Mc_OrganizationsLogins_Active] DEFAULT ((1)),
	LdapDomain nvarchar(255) NULL,
	LdapDomainFull nvarchar(255) NOT NULL CONSTRAINT [DF_Mc_OrganizationsLogins_LdapDomainFull] DEFAULT (N''),
	LdapUserAlias nvarchar(255) NULL,
	LdapUPN nvarchar(255) NOT NULL CONSTRAINT [DF_Mc_OrganizationsLogins_LdapUPN] DEFAULT (N''),
	LdapSecurityId nvarchar(255) NULL,
	LdapUserId uniqueidentifier NULL,
	LdapOUPath nvarchar(255) NULL
	)  ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS(SELECT * FROM dbo.Mc_OrganizationsLogins)
		 EXEC('INSERT INTO dbo.Tmp_Mc_OrganizationsLogins (OrganizationId, LoginId, OrganizationAdministrator, Active, LdapDomain, LdapUserAlias, LdapSecurityId, LdapUserId, LdapOUPath)
			SELECT OrganizationId, LoginId, OrganizationAdministrator, Active, LdapDomain, LdapUserAlias, LdapSecurityId, LdapUserId, LdapOUPath FROM dbo.Mc_OrganizationsLogins WITH (HOLDLOCK TABLOCKX)')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP TABLE dbo.Mc_OrganizationsLogins

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXECUTE sp_rename N'dbo.Tmp_Mc_OrganizationsLogins', N'Mc_OrganizationsLogins', 'OBJECT' 

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_OrganizationsLogins ADD CONSTRAINT
	PK_Mc_OrganizationsLogins PRIMARY KEY CLUSTERED 
	(
		OrganizationId,
		LoginId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_OrganizationsLogins ADD CONSTRAINT
	FK_Mc_OrganizationsLogins_Mc_Organization FOREIGN KEY
	(
		OrganizationId
	) REFERENCES dbo.Mc_Organization
	(
		OrganizationId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_OrganizationsLogins ADD CONSTRAINT
	FK_Mc_OrganizationsLogins_Mc_Login FOREIGN KEY
	(
		LoginId
	) REFERENCES dbo.Mc_Login
	(
		LoginId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 

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
    
 SELECT ol.OrganizationId, ol.LoginId, ol.OrganizationAdministrator, l.FirstName, l.LastName, ol.LdapDomain, ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, ol.LdapUserId, ol.LdapOUPath, ol.Active    
 FROM dbo.Mc_OrganizationsLogins ol 
 INNER JOIN dbo.Mc_Login l 
	ON (ol.LoginId = l.LoginId) 
 WHERE (ol.OrganizationId = @OrganizationId) AND (ol.LoginId = @LoginId);    
END')

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
   
 SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.Deleted  
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
    
 SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.Deleted, ol.OrganizationAdministrator, ol.LdapDomain, ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, ol.LdapUserId, ol.LdapOUPath, ol.Active    
 FROM dbo.Mc_OrganizationsLogins AS ol    
 INNER JOIN dbo.Mc_Login AS l    
  ON (ol.LoginId = l.LoginId) AND (ol.OrganizationId = @OrganizationId) AND (l.Deleted = 0);    
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateLoginLdapInfo]    
(    
 @OrganizationId uniqueidentifier,    
 @LoginId uniqueidentifier,    
 @LdapDomain nvarchar(255),    
 @LdapDomainFull nvarchar(255),    
 @LdapUserAlias nvarchar(255),    
 @LdapUPN nvarchar(255),    
 @LdapSecurityId nvarchar(255),    
 @LdapUserId uniqueidentifier,   
 @LdapOUPath nvarchar(255)  
)    
AS    
BEGIN    
 SET NOCOUNT OFF;    
    
 UPDATE dbo.Mc_OrganizationsLogins     
 SET LdapDomain = @LdapDomain,    
  LdapDomainFull = @LdapDomainFull,    
  LdapUserAlias = @LdapUserAlias,    
  LdapUPN = @LdapUPN,    
  LdapSecurityId = @LdapSecurityId,    
  LdapUserId = @LdapUserId,   
  LdapOUPath = @LdapOUPath   
 WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);    
     
 SELECT OrganizationId, LoginId, OrganizationAdministrator, LdapDomain, LdapUserAlias, LdapSecurityId, LdapUserId, LdapOUPath, Active    
 FROM dbo.Mc_OrganizationsLogins    
 WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);    
END')

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
   
   
 SELECT LoginId, FirstName, LastName, LoginName, [Password], Deleted  
 FROM dbo.Mc_Login  
 WHERE (LoginId = @LoginId);  
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
