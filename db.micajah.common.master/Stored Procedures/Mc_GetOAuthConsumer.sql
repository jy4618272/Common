﻿CREATE PROCEDURE [dbo].[Mc_GetOAuthConsumer]
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