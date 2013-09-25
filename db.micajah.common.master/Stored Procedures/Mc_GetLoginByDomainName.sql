CREATE PROCEDURE [dbo].[Mc_GetLoginByDomainName]      
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
END
