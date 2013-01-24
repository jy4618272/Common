CREATE PROCEDURE [dbo].[Mc_InsertResetPasswordRequest]
(
	@ResetPasswordRequestId uniqueidentifier,
	@LoginId uniqueidentifier,
	@CreatedTime datetime
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	INSERT INTO dbo.Mc_ResetPasswordRequest	(ResetPasswordRequestId, LoginId, CreatedTime)
	VALUES (@ResetPasswordRequestId, @LoginId, @CreatedTime)
	
	SELECT ResetPasswordRequestId, LoginId, CreatedTime
	FROM dbo.Mc_ResetPasswordRequest
	WHERE (ResetPasswordRequestId = @ResetPasswordRequestId);
END