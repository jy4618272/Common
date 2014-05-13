BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectSession]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectSession]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_ValidateSession]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_ValidateSession]
		
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('/****** Object:  StoredProcedure [dbo].[Mc_ValidateSession] ******/
CREATE PROCEDURE [dbo].[Mc_ValidateSession]
	@SessionCookie varchar(50),
	@LoginId int,
	@SessionIsValid bit OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

	SET @SessionIsValid = 0;

	IF EXISTS(SELECT 0 FROM [dbo].[Mc_Session] WHERE LoginId = @LoginId AND SessionCookie = @SessionCookie)
		SET @SessionIsValid = 1;
	ELSE IF NOT EXISTS(SELECT 0 FROM [dbo].[Mc_Session] WHERE LoginId = @LoginId)
		SET @SessionIsValid = 1;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION