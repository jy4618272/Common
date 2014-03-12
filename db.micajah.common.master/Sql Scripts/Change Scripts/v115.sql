BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetLoginByDomainName]      
		(      
		 @DomainName nvarchar(255),   
		 @UserAlias nvarchar(255),   
		 @FirstName nvarchar(255),   
		 @LastName nvarchar(255)   
		)      
		AS      
		BEGIN      
		 SET NOCOUNT OFF;      
       
		DECLARE @Logins TABLE  (LoginId uniqueidentifier NOT NULL,
								FirstName nvarchar(255) NOT NULL,
								LastName nvarchar(255) NOT NULL,
								LoginName nvarchar(255) NOT NULL,
								Password nvarchar(50) NOT NULL,
								OrganizationId uniqueidentifier NOT NULL,
								Active bit NOT NULL,
								UserLdapDomain nvarchar(255),
								LdapDomainFull nvarchar(255) NOT NULL,
								LdapUserAlias nvarchar(255),
								LdapUPN nvarchar(255),
								LdapSecurityId nvarchar(255),
								LdapUserId uniqueidentifier,
								LdapServerAddress nvarchar(255) NOT NULL,
								LdapServerPort nvarchar(50) NOT NULL,
								LdapDomain nvarchar(255) NOT NULL,
								LdapUserName nvarchar(255) NOT NULL,
								LdapPassword nvarchar(255) NOT NULL)

		IF (@DomainName is null AND @UserAlias is not null) OR @DomainName is not null
			INSERT INTO @Logins
			SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.Password, ol.OrganizationId, ol.Active,   
			  ol.LdapDomain as ''UserLdapDomain'', ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId,   
			  ol.LdapUserId, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword   
			 FROM Mc_OrganizationsLogins ol   
			  inner join Mc_Login l ON l.LoginId = ol.LoginId   
			  inner join Mc_Organization o ON o.OrganizationId = ol.OrganizationId   
			 WHERE   
			  ((@DomainName is null) OR ((ol.LdapDomain = @DomainName) OR (ol.LdapDomainFull = @DomainName))) AND   
			  ((@UserAlias is not null) AND (ol.LdapUserAlias = @UserAlias)) AND l.Deleted = 0 AND o.Deleted = 0 AND o.Active = 1

		IF @FirstName is not null AND @LastName is not null
			INSERT INTO @Logins
			SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.Password, ol.OrganizationId, ol.Active,   
			  ol.LdapDomain as ''UserLdapDomain'', ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId,   
			  ol.LdapUserId, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword   
			 FROM Mc_Login l   
			  inner join Mc_OrganizationsLogins ol ON l.LoginId = ol.LoginId   
			  inner join Mc_Organization o ON ol.OrganizationId = o.OrganizationId   
			 WHERE
			  l.FirstName = @FirstName AND l.LastName = @LastName AND l.Deleted = 0 AND o.Deleted = 0 AND o.Active = 1

		SELECT DISTINCT * FROM @Logins ORDER BY LdapServerAddress DESC;

		END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetLoginByEmail]      
		(      
		 @EmailAddress nvarchar(255)      
		)      
		AS      
		BEGIN      
		 SET NOCOUNT ON;      
       
			DECLARE @Logins TABLE (LoginId uniqueidentifier NOT NULL,
								FirstName nvarchar(255) NOT NULL,
								LastName nvarchar(255) NOT NULL,
								LoginName nvarchar(255) NOT NULL,
								Password nvarchar(50) NOT NULL,
								OrganizationId uniqueidentifier NOT NULL,
								Active bit NOT NULL,
								UserLdapDomain nvarchar(255),
								LdapDomainFull nvarchar(255) NOT NULL,
								LdapUserAlias nvarchar(255),
								LdapUPN nvarchar(255),
								LdapSecurityId nvarchar(255),
								LdapUserId uniqueidentifier,
								LdapServerAddress nvarchar(255) NOT NULL,
								LdapServerPort nvarchar(50) NOT NULL,
								LdapDomain nvarchar(255) NOT NULL,
								LdapUserName nvarchar(255) NOT NULL,
								LdapPassword nvarchar(255) NOT NULL)

		 INSERT INTO @Logins
		 SELECT DISTINCT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.Password, ol.OrganizationId, ol.Active,   
		  ol.LdapDomain as ''UserLdapDomain'', ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId,   
		  ol.LdapUserId, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword   
		 FROM Mc_Email e
		  inner join Mc_Login l ON l.LoginId = e.LoginId
		  inner join Mc_OrganizationsLogins ol ON ol.LoginId=l.LoginId   
		  inner join Mc_Organization o ON o.OrganizationId=ol.OrganizationId   
		 WHERE e.Email = @EmailAddress AND l.Deleted = 0 AND o.Deleted = 0 AND o.Active = 1  

		 INSERT INTO @Logins
		 SELECT DISTINCT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.Password, ol.OrganizationId, ol.Active,   
		  ol.LdapDomain as ''UserLdapDomain'', ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId,   
		  ol.LdapUserId, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword   
		 FROM Mc_Login l
		  inner join Mc_OrganizationsLogins ol ON ol.LoginId=l.LoginId   
		  inner join Mc_Organization o ON o.OrganizationId=ol.OrganizationId   
		 WHERE l.LoginName = @EmailAddress AND l.Deleted = 0 AND o.Deleted = 0 AND o.Active = 1  

		 INSERT INTO @Logins
		 SELECT DISTINCT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.Password, ol.OrganizationId, ol.Active,   
		  ol.LdapDomain as ''UserLdapDomain'', ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId,   
		  ol.LdapUserId, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword   
		 FROM Mc_OrganizationsLogins ol
		  inner join Mc_Login l ON l.LoginId=ol.LoginId   
		  inner join Mc_Organization o ON o.OrganizationId=ol.OrganizationId   
		 WHERE ol.LdapUPN = @EmailAddress AND l.Deleted = 0 AND o.Deleted = 0 AND o.Active = 1  

		 SELECT DISTINCT * FROM @Logins ORDER BY LdapServerAddress DESC;

		END')

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

			DECLARE @Logins TABLE (LoginId uniqueidentifier NOT NULL,
								FirstName nvarchar(255) NOT NULL,
								LastName nvarchar(255) NOT NULL,
								LoginName nvarchar(255) NOT NULL,
								Password nvarchar(50) NOT NULL,
								ProfileUpdated smalldatetime NOT NULL,
								Deleted bit NOT NULL,
								SessionId varchar(50),
								Token varchar(50))

			INSERT INTO @Logins
			SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.ProfileUpdated, l.Deleted, l.SessionId, l.Token
			FROM dbo.Mc_Login AS l
			WHERE l.LoginName = @LoginName

			INSERT INTO @Logins
			SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.ProfileUpdated, l.Deleted, l.SessionId, l.Token
			FROM dbo.Mc_Email AS e
			INNER JOIN dbo.Mc_Login AS l ON l.LoginId = e.LoginId
			WHERE e.Email = @LoginName

			SELECT DISTINCT * FROM @Logins

		END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION