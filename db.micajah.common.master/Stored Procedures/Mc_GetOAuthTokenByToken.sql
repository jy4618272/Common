CREATE PROCEDURE [dbo].[Mc_GetOAuthTokenByToken]
(
	@Token nvarchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT TokenId, Token, TokenSecret, TokenTypeId, ConsumerId, ConsumerVersion, Scope, LoginId, RequestTokenVerifier, RequestTokenCallback, CreatedTime, PendingUserAuthorizationRequest
	FROM dbo.Mc_OAuthToken
	WHERE (Token = @Token);
END