CREATE PROCEDURE [dbo].[Mc_GetInvitedLoginsByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT InvitedLoginId, LoginName, OrganizationId, GroupId, InvitedBy, CreatedTime
	FROM dbo.Mc_InvitedLogin
	WHERE (OrganizationId = @OrganizationId);
END
