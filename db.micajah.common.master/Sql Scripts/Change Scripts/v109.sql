BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_OAuthConsumer] ADD [ConsumerCertificate] NVARCHAR (MAX) NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOAuthConsumer]
(
	@ConsumerId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT ConsumerId, ConsumerKey, ConsumerSecret, Callback, VerificationCodeFormat, VerificationCodeLength, ConsumerCertificate
	FROM dbo.Mc_OAuthConsumer
	WHERE (ConsumerId = @ConsumerId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC ('ALTER PROCEDURE [dbo].[Mc_GetOAuthConsumerByConsumerKey]
(
	@ConsumerKey nvarchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT ConsumerId, ConsumerKey, ConsumerSecret, Callback, VerificationCodeFormat, VerificationCodeLength, ConsumerCertificate
	FROM dbo.Mc_OAuthConsumer
	WHERE (ConsumerKey = @ConsumerKey);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION