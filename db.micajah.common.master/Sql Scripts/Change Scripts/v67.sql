BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_OrganizationsLogins ADD
		LdapOUPath nvarchar(255) NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_OrganizationsLogins
		DROP CONSTRAINT FK_Mc_OrganizationsLogins_Mc_Organization
	
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_OrganizationsLogins
		DROP CONSTRAINT FK_Mc_OrganizationsLogins_Mc_Login

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
	OrganizationAdministrator bit NOT NULL,
	Active bit NOT NULL,
	LdapDomain nvarchar(255) NULL,
	LdapUserAlias nvarchar(255) NULL,
	LdapSecurityId nvarchar(255) NULL,
	LdapUserId uniqueidentifier NULL,
	LdapOUPath nvarchar(255) NULL
	)  ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_OrganizationsLogins ADD CONSTRAINT
		DF_Mc_OrganizationsLogins_OrganizationAdministrator DEFAULT ((0)) FOR OrganizationAdministrator

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_OrganizationsLogins ADD CONSTRAINT
		DF_Mc_OrganizationsLogins_Active DEFAULT ((1)) FOR Active

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS(SELECT * FROM dbo.Mc_OrganizationsLogins)
		 EXEC('INSERT INTO dbo.Tmp_Mc_OrganizationsLogins (OrganizationId, LoginId, OrganizationAdministrator, Active, LdapDomain, LdapUserAlias, LdapSecurityId, LdapUserId, LdapOUPath)
			SELECT OrganizationId, LoginId, OrganizationAdministrator, Active, LdapDomain, LdapUserAlias, CONVERT(nvarchar(255), LdapSecurityId), LdapUserId, LdapOUPath FROM dbo.Mc_OrganizationsLogins WITH (HOLDLOCK TABLOCKX)')

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
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizationLogin]  
(  
 @OrganizationId uniqueidentifier,  
 @LoginId uniqueidentifier  
)  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 SELECT OrganizationId, LoginId, OrganizationAdministrator, LdapDomain, LdapUserAlias, LdapSecurityId, LdapUserId, LdapOUPath, Active  
 FROM dbo.Mc_OrganizationsLogins  
 WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);  
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateLoginLdapInfo]  
(  
 @OrganizationId uniqueidentifier,  
 @LoginId uniqueidentifier,  
 @LdapDomain nvarchar(255),  
 @LdapUserAlias nvarchar(255),  
 @LdapSecurityId nvarchar(255),  
 @LdapUserId uniqueidentifier, 
 @LdapOUPath nvarchar(255)
)  
AS  
BEGIN  
 SET NOCOUNT OFF;  
  
 UPDATE dbo.Mc_OrganizationsLogins   
 SET LdapDomain = @LdapDomain,  
  LdapUserAlias = @LdapUserAlias,  
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
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetLoginsByOrganizationId]  
(  
 @OrganizationId uniqueidentifier  
)  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 SELECT l.LoginId, l.LoginName, l.[Password], l.Deleted, ol.OrganizationAdministrator, ol.LdapDomain, ol.LdapUserAlias, ol.LdapSecurityId, ol.LdapUserId, ol.LdapOUPath, ol.Active  
 FROM dbo.Mc_OrganizationsLogins AS ol  
 INNER JOIN dbo.Mc_Login AS l  
  ON (ol.LoginId = l.LoginId) AND (ol.OrganizationId = @OrganizationId) AND (l.Deleted = 0);  
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
