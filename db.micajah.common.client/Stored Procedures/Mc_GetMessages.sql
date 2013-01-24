CREATE PROCEDURE [dbo].[Mc_GetMessages]
(
	@LocalObjectType nvarchar(50),
	@LocalObjectId nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT MessageId, ParentMessageId, LocalObjectType, LocalObjectId, FromUserId, ToUserId, [Subject], [Text], CreatedTime
	FROM dbo.Mc_Message
	WHERE (LocalObjectType = @LocalObjectType) AND (LocalObjectId = @LocalObjectId)
	ORDER BY CreatedTime;
END
