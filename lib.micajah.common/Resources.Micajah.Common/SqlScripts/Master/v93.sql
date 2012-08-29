BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetLogin]
(
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT LoginId, FirstName, LastName, LoginName, [Password], ProfileUpdated, Deleted, SessionId
	FROM dbo.Mc_Login
	WHERE LoginId = @LoginId;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetLoginByLoginName]
(
	@LoginName nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.ProfileUpdated, l.Deleted, l.SessionId
	FROM dbo.Mc_Login AS l
	LEFT JOIN dbo.Mc_Email AS e 
		ON l.LoginId = e.LoginId AND e.Email = @LoginName
	WHERE (l.LoginName = @LoginName OR e.Email IS NOT NULL);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION