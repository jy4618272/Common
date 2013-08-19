CREATE PROCEDURE [dbo].[Mc_InsertOAuthToken]
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
	@CreatedTime datetime,
	@PendingUserAuthorizationRequest nvarchar(max),
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_OAuthToken (TokenId, Token, TokenSecret, TokenTypeId, ConsumerId, ConsumerVersion, Scope, LoginId, RequestTokenVerifier, RequestTokenCallback, CreatedTime, PendingUserAuthorizationRequest, OrganizationId, InstanceId)
	VALUES (@TokenId, @Token, @TokenSecret, @TokenTypeId, @ConsumerId, @ConsumerVersion, @Scope, @LoginId, @RequestTokenVerifier, @RequestTokenCallback, @CreatedTime, @PendingUserAuthorizationRequest, @OrganizationId, @InstanceId);

	SELECT TokenId, Token, TokenSecret, TokenTypeId, ConsumerId, ConsumerVersion, Scope, LoginId, RequestTokenVerifier, RequestTokenCallback, CreatedTime, @PendingUserAuthorizationRequest, OrganizationId, InstanceId
	FROM dbo.Mc_OAuthToken
	WHERE (TokenId = @TokenId);
END