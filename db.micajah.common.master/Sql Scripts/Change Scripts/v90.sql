BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_Email](
	[Email] [nvarchar](255) NOT NULL,
	[LoginId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Mc_Email] PRIMARY KEY CLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('
CREATE PROCEDURE [dbo].[Mc_DeleteEmail]  
(  
 @LoginId uniqueidentifier
)  
AS  
BEGIN  
	SET NOCOUNT OFF;  
	DELETE FROM dbo.Mc_Email  
	WHERE LoginId = @LoginId
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('
CREATE PROCEDURE [dbo].[Mc_InsertEmail]
(
	@Email [nvarchar](255),
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Email (Email, LoginId)
	VALUES (@Email, @LoginId)	
	
	SELECT Email, LoginId
	FROM dbo.Mc_Email
	WHERE (Email = @Email)
END
')


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('
CREATE PROCEDURE [dbo].[Mc_GetEmail]
(
	@Email [nvarchar](255)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Email, LoginId
	FROM dbo.Mc_Email
	WHERE (Email = @Email)
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('
CREATE PROCEDURE [dbo].[Mc_GetEmails]
(
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Email, LoginId
	FROM dbo.Mc_Email
	WHERE (LoginId = @LoginId)
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('
ALTER PROCEDURE [dbo].[Mc_GetLoginByEmail]      
(      
 @EmailAddress nvarchar(255)      
)      
AS      
BEGIN      
 SET NOCOUNT ON;      
       
 SELECT DISTINCT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.Password, ol.OrganizationId, ol.Active,   
  ol.LdapDomain as ''UserLdapDomain'', ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId,   
  ol.LdapUserId, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword   
 FROM Mc_Login l
  left join Mc_Email e ON l.LoginId = e.LoginId
  inner join Mc_OrganizationsLogins ol ON l.LoginId = ol.LoginId   
  inner join Mc_Organization o ON ol.OrganizationId = o.OrganizationId   
 WHERE l.Deleted = 0 AND o.Deleted = 0 AND o.Active = 1 AND   
  (l.LoginName = @EmailAddress OR ol.LdapUPN = @EmailAddress OR e.Email = @EmailAddress)  
 ORDER BY o.LdapServerAddress DESC;  
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION