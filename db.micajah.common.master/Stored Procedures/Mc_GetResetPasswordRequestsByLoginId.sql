CREATE PROCEDURE [dbo].[Mc_GetResetPasswordRequestsByLoginId]
(
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT ResetPasswordRequestId, LoginId, CreatedTime
	FROM dbo.Mc_ResetPasswordRequest
	WHERE (LoginId = @LoginId);
END
