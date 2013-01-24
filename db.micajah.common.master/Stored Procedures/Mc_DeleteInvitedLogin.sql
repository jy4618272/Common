CREATE PROCEDURE [dbo].[Mc_DeleteInvitedLogin]
(
	@InvitedLoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_InvitedLogin
	WHERE 
		(InvitedLoginId = @InvitedLoginId)
		OR (DATEDIFF(DAY, CreatedTime, GETUTCDATE()) >= 30);
END
