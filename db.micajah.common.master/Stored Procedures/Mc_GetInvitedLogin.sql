CREATE PROCEDURE [dbo].[Mc_GetInvitedLogin]
(
	@InvitedLoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT InvitedLoginId, LoginName, OrganizationId, GroupId, InvitedBy, CreatedTime
	FROM dbo.Mc_InvitedLogin
	WHERE (InvitedLoginId = @InvitedLoginId);
END
