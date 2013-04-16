CREATE PROCEDURE [dbo].[Mc_InsertLogin]
(
	@LoginId uniqueidentifier,
	@LoginName nvarchar(255),
	@Password nvarchar(50),
	@FirstName nvarchar(255),
	@LastName nvarchar(255),
	@Token varchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Login (LoginId, FirstName, LastName, LoginName, [Password], Deleted, Token) 
	VALUES (@LoginId, @FirstName, @LastName, @LoginName, @Password, 0, @Token);
	
	SELECT LoginId, FirstName, LastName, LoginName, [Password], ProfileUpdated, Deleted, SessionId, Token
	FROM dbo.Mc_Login 
	WHERE (LoginId = @LoginId);
END
