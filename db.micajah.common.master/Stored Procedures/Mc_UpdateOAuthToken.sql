﻿CREATE PROCEDURE [dbo].[Mc_UpdateOAuthToken]
(
	@TokenId uniqueidentifier,
	@Token nvarchar(50),
	@TokenSecret nvarchar(50),
	@TokenTypeId int,
	@ConsumerId uniqueidentifier,
	@ConsumerVersion nvarchar(50),
	@Scope nvarchar(max),
	@LoginId uniqueidentifier,
	@RequestTokenVerifier nvarchar(255),
	@RequestTokenCallback nvarchar(2048),
	@CreatedTime datetime
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_OAuthToken
	SET TokenId = @TokenId, Token = @Token, TokenSecret = @TokenSecret, TokenTypeId = @TokenTypeId, ConsumerId = @ConsumerId, ConsumerVersion = @ConsumerVersion, Scope = @Scope
		, LoginId = @LoginId, RequestTokenVerifier = @RequestTokenVerifier, RequestTokenCallback = @RequestTokenCallback, CreatedTime = @CreatedTime
	WHERE (TokenId = @TokenId);

	SELECT TokenId, Token, TokenSecret, TokenTypeId, ConsumerId, ConsumerVersion, Scope, LoginId, RequestTokenVerifier, RequestTokenCallback, CreatedTime
	FROM dbo.Mc_OAuthToken
	WHERE (TokenId = @TokenId);
END