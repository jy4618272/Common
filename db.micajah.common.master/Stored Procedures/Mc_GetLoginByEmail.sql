CREATE PROCEDURE [dbo].[Mc_GetLoginByEmail]      
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
END
