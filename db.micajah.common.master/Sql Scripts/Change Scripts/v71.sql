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
		DROP CONSTRAINT DF_Mc_Login_FirstName

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Login
		DROP CONSTRAINT DF_Mc_Login_LastName

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
	LoginId uniqueidentifier NOT NULL,
	FirstName nvarchar(255) NOT NULL,
	LastName nvarchar(255) NOT NULL,
	LoginName nvarchar(255) NOT NULL,
	Password nvarchar(50) NOT NULL,
	ProfileUpdated smalldatetime NOT NULL,
	Deleted bit NOT NULL
	)  ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Login ADD CONSTRAINT
		DF_Mc_Login_LoginId DEFAULT (newid()) FOR LoginId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Login ADD CONSTRAINT
		DF_Mc_Login_FirstName DEFAULT (N'') FOR FirstName

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Login ADD CONSTRAINT
		DF_Mc_Login_LastName DEFAULT (N'') FOR LastName

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Login ADD CONSTRAINT
		DF_Mc_Login_ProfileUpdated DEFAULT CAST('1900-01-01 00:00:00.000' AS smalldatetime) FOR ProfileUpdated

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Login ADD CONSTRAINT
		DF_Mc_Login_Deleted DEFAULT ((0)) FOR Deleted

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS(SELECT * FROM dbo.Mc_Login)
		 EXEC('INSERT INTO dbo.Tmp_Mc_Login (LoginId, FirstName, LastName, LoginName, Password, Deleted)
			SELECT LoginId, FirstName, LastName, LoginName, Password, Deleted FROM dbo.Mc_Login WITH (HOLDLOCK TABLOCKX)')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Session
		DROP CONSTRAINT FK_Mc_Session_Mc_Login

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_ResetPasswordRequest
		DROP CONSTRAINT FK_Mc_ResetPasswordRequest_Mc_Login

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_InvitedLogin
		DROP CONSTRAINT FK_Mc_InvitedLogin_Mc_Login

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_OrganizationsLogins
		DROP CONSTRAINT FK_Mc_OrganizationsLogins_Mc_Login

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
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetLogin]    
(    
 @LoginId uniqueidentifier,    
 @LoginName nvarchar(255),    
 @Password nvarchar(50)    
)    
AS    
BEGIN    
 SET NOCOUNT ON;    
     
 SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.ProfileUpdated, l.Deleted    
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
      
 SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.ProfileUpdated, l.Deleted, ol.OrganizationAdministrator, ol.LdapDomain, ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, ol.LdapUserId, ol.LdapOUPath, ol.Active      
 FROM dbo.Mc_OrganizationsLogins AS ol      
 INNER JOIN dbo.Mc_Login AS l      
  ON (ol.LoginId = l.LoginId) AND (ol.OrganizationId = @OrganizationId) AND (l.Deleted = 0);      
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetLoginByEmail]    
(    
 @EmailAddress nvarchar(255)    
)    
AS    
BEGIN    
 SET NOCOUNT ON;    
     
 SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.Password, ol.OrganizationId, ol.Active, 
  ol.LdapDomain as ''UserLdapDomain'', ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, 
  ol.LdapUserId, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword 
 FROM Mc_Login l 
  inner join Mc_OrganizationsLogins ol ON l.LoginId = ol.LoginId 
  inner join Mc_Organization o ON ol.OrganizationId = o.OrganizationId 
 WHERE l.Deleted = 0 AND o.Deleted = 0 AND o.Active = 1 AND 
  (l.LoginName = @EmailAddress OR ol.LdapUPN = @EmailAddress);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetLoginByDomainName]    
(    
 @DomainName nvarchar(255), 
 @UserAlias nvarchar(255), 
 @FirstName nvarchar(255), 
 @LastName nvarchar(255) 
)    
AS    
BEGIN    
 SET NOCOUNT ON;    
     
 SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.Password, ol.OrganizationId, ol.Active, 
  ol.LdapDomain as ''UserLdapDomain'', ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, 
  ol.LdapUserId, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword 
 FROM Mc_Login l 
  inner join Mc_OrganizationsLogins ol ON l.LoginId = ol.LoginId 
  inner join Mc_Organization o ON ol.OrganizationId = o.OrganizationId 
 WHERE l.Deleted = 0 AND o.Deleted = 0 AND o.Active = 1 AND 
  ((@DomainName is null) OR ((ol.LdapDomain = @DomainName) OR (ol.LdapDomainFull = @DomainName))) AND 
  (((@UserAlias is not null) AND (ol.LdapUserAlias = @UserAlias)) 
  OR 
  (((@FirstName is not null) AND (@LastName is not null)) AND ((l.FirstName = @FirstName) AND (l.LastName = @LastName))));
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
