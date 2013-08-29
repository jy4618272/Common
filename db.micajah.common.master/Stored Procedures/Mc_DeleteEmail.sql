
CREATE PROCEDURE [dbo].[Mc_DeleteEmail]  
(  
 @LoginId uniqueidentifier,
 @Email nvarchar(255)
)  
AS  
BEGIN  
	SET NOCOUNT OFF;  

	IF (@LoginId <> '00000000-0000-0000-0000-000000000000')
	BEGIN 
		DELETE FROM dbo.Mc_Email  
		WHERE LoginId = @LoginId
	END
	ELSE
	BEGIN
		DELETE FROM dbo.Mc_Email  
		WHERE Email = @Email
	END
END
