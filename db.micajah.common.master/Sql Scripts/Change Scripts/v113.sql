﻿BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('
ALTER PROCEDURE [dbo].[Mc_DeleteEmail]  
(  
 @LoginId uniqueidentifier,
 @Email nvarchar(255)
)  
AS  
BEGIN  
	SET NOCOUNT OFF;  

	IF (@LoginId <> ''00000000-0000-0000-0000-000000000000'')
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION


IF @@TRANCOUNT = 1
   COMMIT TRANSACTION