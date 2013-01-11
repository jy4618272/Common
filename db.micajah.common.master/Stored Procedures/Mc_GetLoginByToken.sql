CREATE PROCEDURE [dbo].[Mc_GetLoginByToken]
(
	@Token varchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT LoginId, FirstName, LastName, LoginName, [Password], ProfileUpdated, Deleted, SessionId, Token
	FROM dbo.Mc_Login
	WHERE Token = @Token;
END
