CREATE PROCEDURE [dbo].[Mc_GetOrganizationLogin]    
(    
 @OrganizationId uniqueidentifier,    
 @LoginId uniqueidentifier    
)    
AS    
BEGIN    
 SET NOCOUNT OFF;    
    
 SELECT ol.OrganizationId, ol.LoginId, ol.OrganizationAdministrator, l.FirstName, l.LastName, ol.LdapDomain, ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, ol.LdapUserId, ol.LdapOUPath, ol.Active    
 FROM dbo.Mc_OrganizationsLogins ol 
 INNER JOIN dbo.Mc_Login l 
	ON (ol.LoginId = l.LoginId) 
 WHERE (ol.OrganizationId = @OrganizationId) AND (ol.LoginId = @LoginId);    
END
