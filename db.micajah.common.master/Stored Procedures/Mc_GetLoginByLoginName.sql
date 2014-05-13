CREATE PROCEDURE [dbo].[Mc_GetLoginByLoginName]
(
	@LoginName nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	DECLARE @Logins TABLE (LoginId uniqueidentifier NOT NULL,
						FirstName nvarchar(255) NOT NULL,
						LastName nvarchar(255) NOT NULL,
						LoginName nvarchar(255) NOT NULL,
						Password nvarchar(50) NOT NULL,
						ProfileUpdated smalldatetime NOT NULL,
						Deleted bit NOT NULL,
						SessionId varchar(50),
						Token varchar(50))

	INSERT INTO @Logins
	SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.ProfileUpdated, l.Deleted, l.SessionId, l.Token
	FROM dbo.Mc_Login AS l
	WHERE l.LoginName = @LoginName

	INSERT INTO @Logins
	SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.ProfileUpdated, l.Deleted, l.SessionId, l.Token
	FROM dbo.Mc_Email AS e
	INNER JOIN dbo.Mc_Login AS l ON l.LoginId = e.LoginId
	WHERE e.Email = @LoginName

	SELECT DISTINCT * FROM @Logins

END
