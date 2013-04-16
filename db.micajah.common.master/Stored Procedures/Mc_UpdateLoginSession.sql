CREATE PROCEDURE [dbo].[Mc_UpdateLoginSession]
(
	@LoginId uniqueidentifier,
	@SessionId varchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Login
	SET SessionId = @SessionId
	WHERE LoginId = @LoginId;
END
