BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_Message] (
	   [MessageId] [uniqueidentifier] NOT NULL,
	   [ParentMessageId] [uniqueidentifier] NULL,
	   [LocalObjectType] [nvarchar] (50) NOT NULL,
	   [LocalObjectId] [nvarchar] (255) NOT NULL,
	   [FromUserId] [uniqueidentifier] NOT NULL,
	   [ToUserId] [uniqueidentifier] NULL,
	   [Subject] [nvarchar] (255) NOT NULL,
	   [Text] [nvarchar] (max) NOT NULL,
	   [CreatedTime] [datetime] NOT NULL
	)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Message] ADD CONSTRAINT [PK_Mc_Message] PRIMARY KEY CLUSTERED ([MessageId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetMessages]
(
	@LocalObjectType nvarchar(50),
	@LocalObjectId nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT MessageId, ParentMessageId, LocalObjectType, LocalObjectId, FromUserId, ToUserId, [Subject], [Text], CreatedTime
	FROM dbo.Mc_Message
	WHERE (LocalObjectType = @LocalObjectType) AND (LocalObjectId = @LocalObjectId)
	ORDER BY CreatedTime;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertMessage]
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
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Message_Mc_Message')
      ALTER TABLE [dbo].[Mc_Message] DROP CONSTRAINT [FK_Mc_Message_Mc_Message]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Message_Mc_Message')
      ALTER TABLE [dbo].[Mc_Message] ADD CONSTRAINT [FK_Mc_Message_Mc_Message] FOREIGN KEY ([ParentMessageId]) REFERENCES [dbo].[Mc_Message] ([MessageId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Message_Mc_User_1')
      ALTER TABLE [dbo].[Mc_Message] DROP CONSTRAINT [FK_Mc_Message_Mc_User_1]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Message_Mc_User_1')
      ALTER TABLE [dbo].[Mc_Message] ADD CONSTRAINT [FK_Mc_Message_Mc_User_1] FOREIGN KEY ([FromUserId]) REFERENCES [dbo].[Mc_User] ([UserId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Message_Mc_User_2')
      ALTER TABLE [dbo].[Mc_Message] DROP CONSTRAINT [FK_Mc_Message_Mc_User_2]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Message_Mc_User_2')
      ALTER TABLE [dbo].[Mc_Message] ADD CONSTRAINT [FK_Mc_Message_Mc_User_2] FOREIGN KEY ([ToUserId]) REFERENCES [dbo].[Mc_User] ([UserId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
