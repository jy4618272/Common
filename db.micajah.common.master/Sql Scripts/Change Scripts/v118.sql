BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP PROCEDURE [dbo].[Mc_GetEmailSuffix];

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP PROCEDURE [dbo].[Mc_GetLdapServerDetails];

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEmails]
(
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Email, LoginId
	FROM dbo.Mc_Email
	WHERE (LoginId = @LoginId)
	ORDER BY Email;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEmailSuffixes]  
(  
	@OrganizationId uniqueidentifier,  
	@InstanceId uniqueidentifier,  
	@EmailSuffixName nvarchar(255)  
)  
AS  
BEGIN  
	SET NOCOUNT OFF;
  
	SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName   
	FROM dbo.Mc_EmailSuffix  
	WHERE  
		((@OrganizationId IS NULL) OR (OrganizationId = @OrganizationId)) AND  
		((@InstanceId IS NULL) OR (InstanceId = @InstanceId)) AND 
		((@EmailSuffixName IS NOT NULL) AND ('','' + EmailSuffixName + '','' LIKE ''%,'' + @EmailSuffixName + '',%''))
	ORDER BY EmailSuffixName;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetGroupMappings]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT GroupId, OrganizationId, [GroupName], LdapDomainId, [LdapDomainName], LdapGroupId, [LdapGroupName] 
	FROM dbo.Mc_GroupMappings 
	WHERE (OrganizationId = @OrganizationId)
	ORDER BY GroupName;
END')

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
	SET NOCOUNT ON;
       
	DECLARE @Logins TABLE (
		OrganizationId uniqueidentifier NOT NULL,
		LoginId uniqueidentifier NOT NULL,
		OrganizationAdministrator bit NOT NULL,
		Active bit NOT NULL,
		LdapDomain nvarchar(255),
		LdapDomainFull nvarchar(255) NOT NULL,
		LdapUserAlias nvarchar(255),
		LdapUPN nvarchar(255) NOT NULL,
		LdapSecurityId nvarchar(255),
		LdapUserId uniqueidentifier,
		LdapOUPath nvarchar(255),
		LdapServerAddress nvarchar(255) NOT NULL,
		LdapServerPort nvarchar(50) NOT NULL,
		OrganizationLdapDomain nvarchar(255) NOT NULL,
		LdapUserName nvarchar(255) NOT NULL,
		LdapPassword nvarchar(255) NOT NULL,
		LdapDomains nvarchar(2048) NOT NULL,
		FirstName nvarchar(255) NOT NULL,
		LastName nvarchar(255) NOT NULL,
		LoginName nvarchar(255) NOT NULL,
		[Password] nvarchar(50) NOT NULL)

	IF (@DomainName IS NULL AND @UserAlias IS NOT NULL) OR @DomainName IS NOT NULL
		INSERT INTO @Logins
		SELECT
			ol.OrganizationId, ol.LoginId, ol.OrganizationAdministrator, ol.Active, ol.LdapDomain, ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, ol.LdapUserId, ol.LdapOUPath
			, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain AS OrganizationLdapDomain, o.LdapUserName, o.LdapPassword, o.LdapDomains
			, l.FirstName, l.LastName, l.LoginName, l.[Password]
		FROM Mc_OrganizationsLogins ol   
		INNER JOIN Mc_Login l 
			ON l.LoginId = ol.LoginId   
		INNER JOIN Mc_Organization o 
			ON o.OrganizationId = ol.OrganizationId   
		WHERE ((@DomainName IS NULL) OR ((ol.LdapDomain = @DomainName) OR (ol.LdapDomainFull = @DomainName))) AND   
			((@UserAlias IS NOT NULL) AND (ol.LdapUserAlias = @UserAlias)) AND l.Deleted = 0 AND o.Deleted = 0 AND o.Active = 1

	IF @FirstName IS NOT NULL AND @LastName IS NOT NULL
		INSERT INTO @Logins
		SELECT 
			ol.OrganizationId, ol.LoginId, ol.OrganizationAdministrator, ol.Active, ol.LdapDomain, ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, ol.LdapUserId, ol.LdapOUPath
			, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain AS OrganizationLdapDomain, o.LdapUserName, o.LdapPassword, o.LdapDomains
			, l.FirstName, l.LastName, l.LoginName, l.[Password]
		FROM Mc_Login l   
		INNER JOIN Mc_OrganizationsLogins ol 
			ON l.LoginId = ol.LoginId   
		INNER JOIN Mc_Organization o 
			ON ol.OrganizationId = o.OrganizationId   
		WHERE l.FirstName = @FirstName AND l.LastName = @LastName AND l.Deleted = 0 AND o.Deleted = 0 AND o.Active = 1

	SET NOCOUNT OFF;

	SELECT DISTINCT * 
	FROM @Logins 
	ORDER BY LdapServerAddress DESC;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetLoginByEmail]      
(      
	@Email nvarchar(255)      
)      
AS      
BEGIN      
	SET NOCOUNT ON;
       
	DECLARE @Logins TABLE (
		OrganizationId uniqueidentifier NOT NULL,
		LoginId uniqueidentifier NOT NULL,
		OrganizationAdministrator bit NOT NULL,
		Active bit NOT NULL,
		LdapDomain nvarchar(255),
		LdapDomainFull nvarchar(255) NOT NULL,
		LdapUserAlias nvarchar(255),
		LdapUPN nvarchar(255) NOT NULL,
		LdapSecurityId nvarchar(255),
		LdapUserId uniqueidentifier,
		LdapOUPath nvarchar(255),
		LdapServerAddress nvarchar(255) NOT NULL,
		LdapServerPort nvarchar(50) NOT NULL,
		OrganizationLdapDomain nvarchar(255) NOT NULL,
		LdapUserName nvarchar(255) NOT NULL,
		LdapPassword nvarchar(255) NOT NULL,
		LdapDomains nvarchar(2048) NOT NULL,
		FirstName nvarchar(255) NOT NULL,
		LastName nvarchar(255) NOT NULL,
		LoginName nvarchar(255) NOT NULL,
		[Password] nvarchar(50) NOT NULL)

	INSERT INTO @Logins
	SELECT DISTINCT 
		ol.OrganizationId, ol.LoginId, ol.OrganizationAdministrator, ol.Active, ol.LdapDomain, ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, ol.LdapUserId, ol.LdapOUPath
		, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain AS OrganizationLdapDomain, o.LdapUserName, o.LdapPassword, o.LdapDomains
		, l.FirstName, l.LastName, l.LoginName, l.[Password]
	FROM Mc_Email e
	INNER JOIN Mc_Login l 
		ON l.LoginId = e.LoginId
	INNER JOIN Mc_OrganizationsLogins ol 
		ON ol.LoginId = l.LoginId   
	INNER JOIN Mc_Organization o 
		ON o.OrganizationId = ol.OrganizationId
	WHERE e.Email = @Email AND l.Deleted = 0 AND o.Deleted = 0 AND o.Active = 1  

	INSERT INTO @Logins
	SELECT DISTINCT 
		ol.OrganizationId, ol.LoginId, ol.OrganizationAdministrator, ol.Active, ol.LdapDomain, ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, ol.LdapUserId, ol.LdapOUPath
		, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain AS OrganizationLdapDomain, o.LdapUserName, o.LdapPassword, o.LdapDomains
		, l.FirstName, l.LastName, l.LoginName, l.[Password]
	FROM Mc_Login l
	INNER JOIN Mc_OrganizationsLogins ol 
		ON ol.LoginId = l.LoginId   
	INNER JOIN Mc_Organization o 
		ON o.OrganizationId = ol.OrganizationId
	WHERE l.LoginName = @Email AND l.Deleted = 0 AND o.Deleted = 0 AND o.Active = 1  

	INSERT INTO @Logins
	SELECT DISTINCT
		ol.OrganizationId, ol.LoginId, ol.OrganizationAdministrator, ol.Active, ol.LdapDomain, ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, ol.LdapUserId, ol.LdapOUPath
		, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain AS OrganizationLdapDomain, o.LdapUserName, o.LdapPassword, o.LdapDomains
		, l.FirstName, l.LastName, l.LoginName, l.[Password]
	FROM Mc_OrganizationsLogins ol
	INNER JOIN Mc_Login l 
		ON l.LoginId = ol.LoginId   
	INNER JOIN Mc_Organization o 
		ON o.OrganizationId = ol.OrganizationId   
	WHERE ol.LdapUPN = @Email AND l.Deleted = 0 AND o.Deleted = 0 AND o.Active = 1

	SET NOCOUNT OFF;

	SELECT DISTINCT *
	FROM @Logins
	ORDER BY LdapServerAddress DESC;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEmails]
(
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Email, LoginId
	FROM dbo.Mc_Email
	WHERE (LoginId = @LoginId)
	ORDER BY Email;
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
	SET NOCOUNT OFF;      

	SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.ProfileUpdated, l.Deleted, l.SessionId, l.Token
		, ol.OrganizationId, ol.OrganizationAdministrator, ol.Active, ol.LdapDomain, ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, ol.LdapUserId, ol.LdapOUPath
	FROM dbo.Mc_OrganizationsLogins AS ol      
	INNER JOIN dbo.Mc_Login AS l      
		ON (ol.LoginId = l.LoginId) AND (ol.OrganizationId = @OrganizationId) AND (l.Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizations]
(
	@Deleted bit
)
AS  
BEGIN  
	SET NOCOUNT OFF;  
	  
	SELECT OrganizationId, PseudoId, ParentOrganizationId, [Name], [Description], WebsiteUrl, DatabaseId  
		, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains  
		, ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime
		, Street, Street2, City, [State], PostalCode, Country, Currency, HowYouHearAboutUs
	FROM dbo.Mc_Organization
	WHERE (@Deleted IS NULL) OR (Deleted = @Deleted)
	ORDER BY [Name];
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizationsByLoginId]
(  
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT o.OrganizationId, o.PseudoId, o.ParentOrganizationId, o.[Name], o.[Description], o.WebsiteUrl, o.DatabaseId
		, o.FiscalYearStartMonth, o.FiscalYearStartDay, o.WeekStartsDay, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword, o.LdapDomains
		, o.ExpirationTime, o.GraceDays, o.ExternalId, o.Active, o.CanceledTime, o.Trial, o.Beta, o.Deleted, o.CreatedTime
		, o.Street, o.Street2, o.City, o.[State], o.PostalCode, o.Country, o.Currency, o.HowYouHearAboutUs
	FROM dbo.Mc_OrganizationsLogins AS ol
	INNER JOIN dbo.Mc_Organization AS o
		ON (ol.OrganizationId = o.OrganizationId) AND (ol.LoginId = @LoginId) AND (o.Deleted = 0) AND (o.Active = 1) AND (ol.Active = 1)
	ORDER BY [Name];
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetDatabase]
(
	@DatabaseId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT DatabaseId, [Name], [Description], UserName, [Password], DatabaseServerId, [Private], Deleted 
	FROM dbo.Mc_Database
	WHERE (DatabaseId = @DatabaseId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetDatabaseByDatabaseId]
(
	@DatabaseId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT d.DatabaseId, d.[Name], d.[Description], d.UserName, d.[Password], d.DatabaseServerId, d.[Private], d.Deleted
		, s.Name AS DatabaseServerName, s.InstanceName, s.Port
	FROM dbo.Mc_Database AS d
	INNER JOIN dbo.Mc_DatabaseServer AS s
		ON d.DatabaseServerId = s.DatabaseServerId AND s.Deleted = 0 AND d.DatabaseId = @DatabaseId AND d.Deleted = 0
	INNER JOIN dbo.Mc_Website AS w
		ON s.WebsiteId = w.WebsiteId AND w.Deleted = 0;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetDatabaseByMinimalNumberOfOrganizations]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT TOP 1 DatabaseId, [Name], [Description], UserName, [Password], DatabaseServerId, [Private], Deleted
		, (SELECT COUNT(1) FROM dbo.Mc_Organization AS o WHERE o.DatabaseId = db.DatabaseId) AS OrganizationsCount
	FROM dbo.Mc_Database AS db
	ORDER BY OrganizationsCount;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetDatabaseByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT d.DatabaseId, d.[Name], d.[Description], d.UserName, d.[Password], d.DatabaseServerId, d.[Private], d.Deleted
		, s.Name AS DatabaseServerName, s.InstanceName, s.Port
	FROM dbo.Mc_Organization AS o
	INNER JOIN dbo.Mc_Database AS d
		ON o.DatabaseId = d.DatabaseId AND o.OrganizationId = @OrganizationId AND d.Deleted = 0
	INNER JOIN dbo.Mc_DatabaseServer AS s
		ON d.DatabaseServerId = s.DatabaseServerId AND s.Deleted = 0
	INNER JOIN dbo.Mc_Website AS w
		ON s.WebsiteId = w.WebsiteId AND w.Deleted = 0;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetDatabaseServer]
(
	@DatabaseServerId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT DatabaseServerId, [Name], InstanceName, Port, [Description], WebsiteId, Deleted 
	FROM dbo.Mc_DatabaseServer 
	WHERE (DatabaseServerId = @DatabaseServerId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetDatabaseServerByDatabaseId]
(
	@DatabaseId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT ds.DatabaseServerId, ds.[Name], ds.InstanceName, ds.Port, ds.[Description], ds.WebsiteId, ds.Deleted 
	FROM dbo.Mc_Database AS db
	INNER JOIN dbo.Mc_DatabaseServer AS ds
		ON db.DatabaseServerId = ds.DatabaseServerId
	WHERE (db.DatabaseId = @DatabaseId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetOrganization]
(
	@OrganizationId uniqueidentifier
)
AS  
BEGIN  
	SET NOCOUNT OFF;
	  
	SELECT OrganizationId, PseudoId, ParentOrganizationId, [Name], [Description], WebsiteUrl, DatabaseId
		, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains
		, ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime
		, Street, Street2, City, [State], PostalCode, Country, Currency, HowYouHearAboutUs
	FROM dbo.Mc_Organization
	WHERE (OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetOrganizationByName]
(
	@Name nvarchar (255)
)
AS  
BEGIN  
	SET NOCOUNT OFF;
	  
	SELECT OrganizationId, PseudoId, ParentOrganizationId, [Name], [Description], WebsiteUrl, DatabaseId
		, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains
		, ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime
		, Street, Street2, City, [State], PostalCode, Country, Currency, HowYouHearAboutUs
	FROM dbo.Mc_Organization
	WHERE ([Name] = @Name);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetOrganizationByPseudoId]
(
	@PseudoId varchar(6)
)
AS  
BEGIN  
	SET NOCOUNT OFF;
	  
	SELECT OrganizationId, PseudoId, ParentOrganizationId, [Name], [Description], WebsiteUrl, DatabaseId
		, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains
		, ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime
		, Street, Street2, City, [State], PostalCode, Country, Currency, HowYouHearAboutUs
	FROM dbo.Mc_Organization
	WHERE (PseudoId = @PseudoId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetOrganizationsByParentOrganizationId]
(
	@ParentOrganizationId uniqueidentifier
)
AS  
BEGIN  
	SET NOCOUNT OFF;  
	  
	SELECT OrganizationId, PseudoId, ParentOrganizationId, [Name], [Description], WebsiteUrl, DatabaseId  
		, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains  
		, ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime
		, Street, Street2, City, [State], PostalCode, Country, Currency, HowYouHearAboutUs
	FROM dbo.Mc_Organization
	WHERE (Deleted = 0) AND (((@ParentOrganizationId IS NULL) AND (ParentOrganizationId IS NULL)) OR (ParentOrganizationId = @ParentOrganizationId))
	ORDER BY [Name];
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetWebsite]
(
	@WebsiteId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT WebsiteId, [Name], Url, [Description], AdminContactInfo, Deleted 
	FROM dbo.Mc_Website 
	WHERE (WebsiteId = @WebsiteId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetWebsiteByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT w.WebsiteId, w.[Name], w.Url, w.[Description], w.AdminContactInfo, w.Deleted 
	FROM dbo.Mc_Organization AS o
	INNER JOIN dbo.Mc_Database AS db
		ON o.DatabaseId = db.DatabaseId
	INNER JOIN dbo.Mc_DatabaseServer AS ds
		ON db.DatabaseServerId = ds.DatabaseServerId
	INNER JOIN dbo.Mc_Website AS w
		ON ds.WebsiteId = w.WebsiteId
	WHERE o.OrganizationId = @OrganizationId;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION