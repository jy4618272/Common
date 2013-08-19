BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_OAuthToken]
    ADD [OrganizationId] UNIQUEIDENTIFIER NULL,
        [InstanceId]     UNIQUEIDENTIFIER NULL;

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOAuthTokenByToken]
(
	@Token nvarchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT TokenId, Token, TokenSecret, TokenTypeId, ConsumerId, ConsumerVersion, Scope, LoginId, RequestTokenVerifier, RequestTokenCallback, CreatedTime, PendingUserAuthorizationRequest, OrganizationId, InstanceId
	FROM dbo.Mc_OAuthToken
	WHERE (Token = @Token);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC ('ALTER PROCEDURE [dbo].[Mc_InsertOAuthToken]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC ('ALTER PROCEDURE [dbo].[Mc_UpdateOAuthToken]
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

	UPDATE dbo.Mc_OAuthToken
	SET TokenId = @TokenId, Token = @Token, TokenSecret = @TokenSecret, TokenTypeId = @TokenTypeId, ConsumerId = @ConsumerId, ConsumerVersion = @ConsumerVersion, Scope = @Scope
		, LoginId = @LoginId, RequestTokenVerifier = @RequestTokenVerifier, RequestTokenCallback = @RequestTokenCallback, CreatedTime = @CreatedTime, PendingUserAuthorizationRequest = @PendingUserAuthorizationRequest
		, OrganizationId = @OrganizationId, InstanceId = @InstanceId
	WHERE (TokenId = @TokenId);

	SELECT TokenId, Token, TokenSecret, TokenTypeId, ConsumerId, ConsumerVersion, Scope, LoginId, RequestTokenVerifier, RequestTokenCallback, CreatedTime, PendingUserAuthorizationRequest, OrganizationId, InstanceId
	FROM dbo.Mc_OAuthToken
	WHERE (TokenId = @TokenId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION