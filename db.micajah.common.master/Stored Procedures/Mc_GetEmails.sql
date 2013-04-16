
CREATE PROCEDURE [dbo].[Mc_GetEmails]
(
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Email, LoginId
	FROM dbo.Mc_Email
	WHERE (LoginId = @LoginId)
END
