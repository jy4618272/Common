CREATE PROCEDURE [dbo].[Mc_UpdateLogin]
(
	@LoginId uniqueidentifier,
	@LoginName nvarchar(255),
	@Password nvarchar(50),
	@FirstName nvarchar(255),
	@LastName nvarchar(255),
	@Deleted bit
)  
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Login
	SET FirstName = (CASE WHEN @FirstName IS NULL THEN FirstName ELSE @FirstName END)
		, LastName = (CASE WHEN @LastName IS NULL THEN LastName ELSE @LastName END)
		, LoginName = (CASE WHEN @LoginName IS NULL THEN LoginName ELSE @LoginName END)
		, [Password] = (CASE WHEN @Password IS NULL THEN [Password] ELSE @Password END)
		, Deleted = (CASE WHEN @Deleted IS NULL THEN Deleted ELSE @Deleted END)
	WHERE (LoginId = @LoginId);

	SELECT LoginId, FirstName, LastName, LoginName, [Password], SessionId, Deleted
	FROM dbo.Mc_Login
	WHERE (LoginId = @LoginId);
END
