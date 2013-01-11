CREATE PROCEDURE [dbo].[Mc_GetLogin]
(
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT LoginId, FirstName, LastName, LoginName, [Password], ProfileUpdated, Deleted, SessionId, Token
	FROM dbo.Mc_Login
	WHERE LoginId = @LoginId;
END
