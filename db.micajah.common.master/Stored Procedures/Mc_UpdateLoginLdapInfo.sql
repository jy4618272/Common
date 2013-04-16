CREATE PROCEDURE [dbo].[Mc_UpdateLoginLdapInfo]    
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
END