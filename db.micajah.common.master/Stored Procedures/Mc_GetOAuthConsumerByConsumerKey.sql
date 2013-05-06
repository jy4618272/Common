CREATE PROCEDURE [dbo].[Mc_GetOAuthConsumerByConsumerKey]
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