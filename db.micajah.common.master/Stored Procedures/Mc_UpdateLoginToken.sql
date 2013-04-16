CREATE PROCEDURE [dbo].[Mc_UpdateLoginToken]
(
	@LoginId uniqueidentifier,
	@Token varchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Login
	SET Token = @Token
	WHERE LoginId = @LoginId;
END
