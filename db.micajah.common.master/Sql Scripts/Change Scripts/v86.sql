﻿BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertViewState]
(
	@ViewStateId uniqueidentifier,
    @ViewState varbinary(max),
    @ExpirationTime datetime
)
AS
BEGIN
    SET NOCOUNT ON;

	INSERT INTO dbo.Mc_ViewState (ViewStateId, ViewState, ExpirationTime) 
	VALUES (@ViewStateId, @ViewState, @ExpirationTime);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP PROCEDURE [dbo].[Mc_UpdateViewState]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION