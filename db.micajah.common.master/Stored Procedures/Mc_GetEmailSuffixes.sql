CREATE PROCEDURE [dbo].[Mc_GetEmailSuffixes]  
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
  ((@EmailSuffixName IS NOT NULL) 
   AND (',' + EmailSuffixName + ',' like '%,' + @EmailSuffixName + ',%') 
  );  
END
