
CREATE PROCEDURE [dbo].[Mc_InsertEmail]
(
	@Email [nvarchar](255),
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Email (Email, LoginId)
	VALUES (@Email, @LoginId)	
	
	SELECT Email, LoginId
	FROM dbo.Mc_Email
	WHERE (Email = @Email)
END
