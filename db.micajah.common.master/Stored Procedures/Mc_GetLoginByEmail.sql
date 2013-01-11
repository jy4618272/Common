
CREATE PROCEDURE [dbo].[Mc_GetLoginByEmail]      
(      
 @EmailAddress nvarchar(255)      
)      
AS      
BEGIN      
 SET NOCOUNT ON;      
       
 SELECT DISTINCT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.Password, ol.OrganizationId, ol.Active,   
  ol.LdapDomain as 'UserLdapDomain', ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId,   
  ol.LdapUserId, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword   
 FROM Mc_Login l
  left join Mc_Email e ON l.LoginId = e.LoginId
  inner join Mc_OrganizationsLogins ol ON l.LoginId = ol.LoginId   
  inner join Mc_Organization o ON ol.OrganizationId = o.OrganizationId   
 WHERE l.Deleted = 0 AND o.Deleted = 0 AND o.Active = 1 AND   
  (l.LoginName = @EmailAddress OR ol.LdapUPN = @EmailAddress OR e.Email = @EmailAddress)  
 ORDER BY o.LdapServerAddress DESC;  
END
