CREATE PROCEDURE [dbo].[Mc_DeleteResetPasswordRequest]
(
	@ResetPasswordRequestId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_ResetPasswordRequest
	WHERE 
		(ResetPasswordRequestId = @ResetPasswordRequestId)
		OR (DATEDIFF(MINUTE, CreatedTime, GETUTCDATE()) > 60);
END
