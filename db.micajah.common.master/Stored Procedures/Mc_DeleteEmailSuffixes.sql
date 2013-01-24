CREATE PROCEDURE [dbo].[Mc_DeleteEmailSuffixes]  
(  
 @OrganizationId uniqueidentifier,
 @InstanceId uniqueidentifier
)  
AS  
BEGIN  
 SET NOCOUNT OFF;  

 IF @InstanceId IS NOT NULL
 BEGIN
   DELETE FROM dbo.Mc_EmailSuffix  
   WHERE InstanceId = @InstanceId;  
 END
 ELSE  
   DELETE FROM dbo.Mc_EmailSuffix  
   WHERE OrganizationId = @OrganizationId AND InstanceId IS NULL;  
END