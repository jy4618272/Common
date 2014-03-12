BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE TABLE [dbo].[Mc_Nonce] (
   [Context] [nvarchar] (100) NOT NULL,
   [Code] [nvarchar] (50) NOT NULL,
   [CreatedTime] [datetime] NOT NULL
)
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Nonce] ADD CONSTRAINT [PK_Mc_Nonce] PRIMARY KEY CLUSTERED ([Context], [Code], [CreatedTime]) 

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC ('CREATE TABLE [dbo].[Mc_OAuthConsumer] (
   [ConsumerId] [uniqueidentifier] NOT NULL,
   [ConsumerKey] [nvarchar] (50) NOT NULL,
   [ConsumerSecret] [nvarchar] (50) NOT NULL,
   [Callback] [nvarchar] (2048) NOT NULL,
   [VerificationCodeFormat] [int] NOT NULL,
   [VerificationCodeLength] [int] NOT NULL
)
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_OAuthConsumer] ADD CONSTRAINT [DF_Mc_OAuthConsumer_Callback] DEFAULT (N'') FOR [Callback]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_OAuthConsumer] ADD CONSTRAINT [DF_Mc_OAuthConsumer_VerificationCodeFormat] DEFAULT ((1)) FOR [VerificationCodeFormat]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_OAuthConsumer] ADD CONSTRAINT [DF_Mc_OAuthConsumer_VerificationCodeLength] DEFAULT ((10)) FOR [VerificationCodeLength]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_OAuthConsumer] ADD CONSTRAINT [PK_Mc_OAuthConsumer] PRIMARY KEY CLUSTERED ([ConsumerId]) 

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE TABLE [dbo].[Mc_OAuthToken] (
   [TokenId] [uniqueidentifier] NOT NULL,
   [Token] [nvarchar] (50) NOT NULL,
   [TokenSecret] [nvarchar] (50) NOT NULL,
   [TokenTypeId] [int] NOT NULL,
   [ConsumerId] [uniqueidentifier] NOT NULL,
   [ConsumerVersion] [nvarchar] (50) NOT NULL,
   [Scope] [nvarchar] (max) NOT NULL,
   [LoginId] [uniqueidentifier] NULL,
   [RequestTokenVerifier] [nvarchar] (255) NOT NULL,
   [RequestTokenCallback] [nvarchar] (2048) NOT NULL,
   [CreatedTime] [datetime] NOT NULL
)
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_OAuthToken] ADD CONSTRAINT [PK_Mc_OAuthToken] PRIMARY KEY CLUSTERED ([TokenId]) 

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteOAuthTokenByToken]
(
	@Token nvarchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_OAuthToken
	WHERE (Token = @Token);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetOAuthConsumer]
(
	@ConsumerId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT ConsumerId, ConsumerKey, ConsumerSecret, Callback, VerificationCodeFormat, VerificationCodeLength
	FROM dbo.Mc_OAuthConsumer
	WHERE (ConsumerId = @ConsumerId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetOAuthConsumerByConsumerKey]
(
	@ConsumerKey nvarchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT ConsumerId, ConsumerKey, ConsumerSecret, Callback, VerificationCodeFormat, VerificationCodeLength
	FROM dbo.Mc_OAuthConsumer
	WHERE (ConsumerKey = @ConsumerKey);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetOAuthTokenByToken]
(
	@Token nvarchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT TokenId, Token, TokenSecret, TokenTypeId, ConsumerId, ConsumerVersion, Scope, LoginId, RequestTokenVerifier, RequestTokenCallback, CreatedTime
	FROM dbo.Mc_OAuthToken
	WHERE (Token = @Token);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertNonce]
(
	@Context nvarchar(100),
	@Code nvarchar(50),
	@CreatedTime datetime
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Nonce(Context, Code, CreatedTime)
	VALUES (@Context, @Code, @CreatedTime);

	SELECT Context, Code, CreatedTime
	FROM dbo.Mc_Nonce
	WHERE (Context = @Context) AND (Code = @Code) AND (CreatedTime = @CreatedTime);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertOAuthToken]
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

	INSERT INTO dbo.Mc_OAuthToken (TokenId, Token, TokenSecret, TokenTypeId, ConsumerId, ConsumerVersion, Scope, LoginId, RequestTokenVerifier, RequestTokenCallback, CreatedTime)
	VALUES (@TokenId, @Token, @TokenSecret, @TokenTypeId, @ConsumerId, @ConsumerVersion, @Scope, @LoginId, @RequestTokenVerifier, @RequestTokenCallback, @CreatedTime);

	SELECT TokenId, Token, TokenSecret, TokenTypeId, ConsumerId, ConsumerVersion, Scope, LoginId, RequestTokenVerifier, RequestTokenCallback, CreatedTime
	FROM dbo.Mc_OAuthToken
	WHERE (TokenId = @TokenId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateOAuthToken]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_OAuthToken_Mc_Login')
      ALTER TABLE [dbo].[Mc_OAuthToken] DROP CONSTRAINT [FK_Mc_OAuthToken_Mc_Login]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_OAuthToken_Mc_Login')
      ALTER TABLE [dbo].[Mc_OAuthToken] ADD CONSTRAINT [FK_Mc_OAuthToken_Mc_Login] FOREIGN KEY ([LoginId]) REFERENCES [dbo].[Mc_Login] ([LoginId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_OAuthToken_Mc_OAuthConsumer')
      ALTER TABLE [dbo].[Mc_OAuthToken] DROP CONSTRAINT [FK_Mc_OAuthToken_Mc_OAuthConsumer]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_OAuthToken_Mc_OAuthConsumer')
      ALTER TABLE [dbo].[Mc_OAuthToken] ADD CONSTRAINT [FK_Mc_OAuthToken_Mc_OAuthConsumer] FOREIGN KEY ([ConsumerId]) REFERENCES [dbo].[Mc_OAuthConsumer] ([ConsumerId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION