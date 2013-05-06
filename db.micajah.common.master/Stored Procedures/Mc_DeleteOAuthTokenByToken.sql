CREATE PROCEDURE [dbo].[Mc_DeleteOAuthTokenByToken]
(
	@Token nvarchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_OAuthToken
	WHERE (Token = @Token);
END