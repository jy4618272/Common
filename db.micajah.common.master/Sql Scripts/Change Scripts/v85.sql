BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysindexes WHERE name = N'IX_Mc_ViewState_SessionId')
      DROP INDEX [IX_Mc_ViewState_SessionId] ON [dbo].[Mc_ViewState]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'PK_Mc_ViewState')
      ALTER TABLE [dbo].[Mc_ViewState] DROP CONSTRAINT [PK_Mc_ViewState]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_ViewState]
      DROP COLUMN [SessionId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_ViewState] ADD CONSTRAINT [PK_Mc_ViewState] PRIMARY KEY NONCLUSTERED ([ViewStateId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE CLUSTERED INDEX [IX_Mc_ViewState_ExpirationTime] ON [dbo].[Mc_ViewState] ([ExpirationTime])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_DeleteViewState]
(
    @Now datetime
)
AS
BEGIN
    SET NOCOUNT ON;
    
	DELETE dbo.Mc_ViewState
	WHERE ExpirationTime < @Now;
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetViewState]
(
    @ViewStateId uniqueidentifier
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ViewState
    FROM dbo.Mc_ViewState WITH (READUNCOMMITTED)
    WHERE (ViewStateId = @ViewStateId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateViewState]
(
	@ViewStateId uniqueidentifier,
    @ViewState varbinary(max),
    @ExpirationTime datetime
)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS(
		SELECT 0
		FROM dbo.Mc_ViewState WITH (READUNCOMMITTED)
		WHERE (ViewStateId = @ViewStateId)
    )
		UPDATE dbo.Mc_ViewState
		SET ViewState = @ViewState, ExpirationTime = @ExpirationTime
		WHERE ViewStateId = @ViewStateId;
    ELSE
		INSERT INTO dbo.Mc_ViewState (ViewStateId, ViewState, ExpirationTime) 
		VALUES (@ViewStateId, @ViewState, @ExpirationTime);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
