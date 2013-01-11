CREATE PROCEDURE [dbo].[Mc_GetEmailSuffixesByInstanceId]  
(  
 @InstanceId uniqueidentifier  
)  
AS  
BEGIN  
 SET NOCOUNT OFF;  
  
 SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName   
 FROM dbo.Mc_EmailSuffix  
 WHERE (InstanceId = @InstanceId);  
END
