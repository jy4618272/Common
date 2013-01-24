CREATE PROCEDURE [dbo].[Mc_GetLoginByLoginName]
(
	@LoginName nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.ProfileUpdated, l.Deleted, l.SessionId, l.Token
	FROM dbo.Mc_Login AS l
	LEFT JOIN dbo.Mc_Email AS e 
		ON l.LoginId = e.LoginId AND e.Email = @LoginName
	WHERE (l.LoginName = @LoginName OR e.Email IS NOT NULL);
END
