CREATE PROCEDURE [dbo].[Mc_GetEmailSuffixesByOrganizationId]  
(  
 @OrganizationId uniqueidentifier  
)  
AS  
BEGIN  
 SET NOCOUNT OFF;  
  
 SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName   
 FROM dbo.Mc_EmailSuffix  
 WHERE ((OrganizationId = @OrganizationId) AND (InstanceId IS NULL))
 UNION
 SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName   
 FROM dbo.Mc_EmailSuffix  
 WHERE ((OrganizationId = @OrganizationId) AND (InstanceId IS NOT NULL));  
END
