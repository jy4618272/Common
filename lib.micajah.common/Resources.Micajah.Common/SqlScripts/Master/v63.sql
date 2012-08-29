BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_ResetPasswordRequest] (
	   [ResetPasswordRequestId] [uniqueidentifier] NOT NULL,
	   [LoginId] [uniqueidentifier] NOT NULL,
	   [CreatedTime] [datetime] NOT NULL
	)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_ResetPasswordRequest] ADD CONSTRAINT [PK_Mc_ResetPasswordRequest] PRIMARY KEY CLUSTERED ([ResetPasswordRequestId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteResetPasswordRequest]
(
	@ResetPasswordRequestId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_ResetPasswordRequest
	WHERE 
		(ResetPasswordRequestId = @ResetPasswordRequestId)
		OR (DATEDIFF(MINUTE, CreatedTime, GETDATE()) > 60);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetResetPasswordRequest]
(
	@ResetPasswordRequestId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT ResetPasswordRequestId, LoginId, CreatedTime
	FROM dbo.Mc_ResetPasswordRequest
	WHERE (ResetPasswordRequestId = @ResetPasswordRequestId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetResetPasswordRequestsByLoginId]
(
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT ResetPasswordRequestId, LoginId, CreatedTime
	FROM dbo.Mc_ResetPasswordRequest
	WHERE (LoginId = @LoginId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertResetPasswordRequest]
(
	@ResetPasswordRequestId uniqueidentifier,
	@LoginId uniqueidentifier,
	@CreatedTime datetime
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	INSERT INTO dbo.Mc_ResetPasswordRequest	(ResetPasswordRequestId, LoginId, CreatedTime)
	VALUES (@ResetPasswordRequestId, @LoginId, @CreatedTime)
	
	SELECT ResetPasswordRequestId, LoginId, CreatedTime
	FROM dbo.Mc_ResetPasswordRequest
	WHERE (ResetPasswordRequestId = @ResetPasswordRequestId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_ResetPasswordRequest_Mc_Login')
      ALTER TABLE [dbo].[Mc_ResetPasswordRequest] DROP CONSTRAINT [FK_Mc_ResetPasswordRequest_Mc_Login]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_ResetPasswordRequest_Mc_Login')
      ALTER TABLE [dbo].[Mc_ResetPasswordRequest] ADD CONSTRAINT [FK_Mc_ResetPasswordRequest_Mc_Login] FOREIGN KEY ([LoginId]) REFERENCES [dbo].[Mc_Login] ([LoginId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
