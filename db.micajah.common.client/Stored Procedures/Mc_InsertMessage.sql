CREATE PROCEDURE [dbo].[Mc_InsertMessage]
(
	@MessageId uniqueidentifier,
	@ParentMessageId uniqueidentifier,
	@LocalObjectType nvarchar(50),
	@LocalObjectId nvarchar(255),
	@FromUserId uniqueidentifier,
	@ToUserId uniqueidentifier,
	@Subject nvarchar(255),
	@Text nvarchar(max),
	@CreatedTime datetime
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Message (MessageId, ParentMessageId, LocalObjectType, LocalObjectId, FromUserId, ToUserId, [Subject], [Text], CreatedTime) 
	VALUES (@MessageId, @ParentMessageId, @LocalObjectType, @LocalObjectId, @FromUserId, @ToUserId,@Subject, @Text, @CreatedTime);
	
	SELECT MessageId, ParentMessageId, LocalObjectType, LocalObjectId, FromUserId, ToUserId, [Subject], [Text], CreatedTime
	FROM dbo.Mc_Message
	WHERE MessageId = @MessageId;
END