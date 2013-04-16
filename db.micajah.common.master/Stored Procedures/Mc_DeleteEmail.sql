
CREATE PROCEDURE [dbo].[Mc_DeleteEmail]  
(  
 @LoginId uniqueidentifier
)  
AS  
BEGIN  
	SET NOCOUNT OFF;  
	DELETE FROM dbo.Mc_Email  
	WHERE LoginId = @LoginId
END
