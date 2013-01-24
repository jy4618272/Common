CREATE PROCEDURE [dbo].[Mc_GetResetPasswordRequest]
(
	@ResetPasswordRequestId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT ResetPasswordRequestId, LoginId, CreatedTime
	FROM dbo.Mc_ResetPasswordRequest
	WHERE (ResetPasswordRequestId = @ResetPasswordRequestId);
END
