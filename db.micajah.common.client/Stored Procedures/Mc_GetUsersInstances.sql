CREATE PROCEDURE [dbo].[Mc_GetUsersInstances]
(
	@UserId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT ui.UserId, ui.InstanceId, ui.Active 
	FROM dbo.Mc_UsersInstances AS ui
	INNER JOIN dbo.Mc_Instance AS i
		ON	(ui.InstanceId = i.InstanceId) AND (i.Deleted = 0) AND (i.OrganizationId = @OrganizationId)
	INNER JOIN dbo.Mc_User AS u
		ON	(ui.UserId = u.UserId) AND (u.UserId = @UserId);
END
