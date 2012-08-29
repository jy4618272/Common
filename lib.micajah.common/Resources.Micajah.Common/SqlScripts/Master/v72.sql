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
  (((@FirstName is not null) AND (@LastName is not null)) AND ((l.FirstName = @FirstName) AND (l.LastName = @LastName)))) 
 ORDER BY o.LdapServerAddress DESC;
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
     
 SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.Password, ol.OrganizationId, ol.Active, 
  ol.LdapDomain as ''UserLdapDomain'', ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, 
  ol.LdapUserId, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword 
 FROM Mc_Login l 
  inner join Mc_OrganizationsLogins ol ON l.LoginId = ol.LoginId 
  inner join Mc_Organization o ON ol.OrganizationId = o.OrganizationId 
 WHERE l.Deleted = 0 AND o.Deleted = 0 AND o.Active = 1 AND 
  (l.LoginName = @EmailAddress OR ol.LdapUPN = @EmailAddress)
 ORDER BY o.LdapServerAddress DESC;
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
 SET NOCOUNT ON;  
  
 SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName   
 FROM dbo.Mc_EmailSuffix  
 WHERE  
  ((@OrganizationId IS NULL) OR (OrganizationId = @OrganizationId)) AND  
  ((@InstanceId IS NULL) OR (InstanceId = @InstanceId)) AND 
  ((@EmailSuffixName IS NOT NULL) 
   AND ('','' + EmailSuffixName + '','' like ''%,'' + @EmailSuffixName + '',%'') 
  );  
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
