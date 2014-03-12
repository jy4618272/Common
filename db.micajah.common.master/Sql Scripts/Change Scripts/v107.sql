BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertLogin]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION