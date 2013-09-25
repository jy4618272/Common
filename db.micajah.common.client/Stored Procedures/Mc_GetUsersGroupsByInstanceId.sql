CREATE PROCEDURE [dbo].[Mc_GetUsersGroupsByInstanceId]
(
	@UserId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT ug.UserId, ug.GroupId, gir.RoleId
	FROM dbo.Mc_UsersGroups AS ug
	INNER JOIN dbo.Mc_Group AS g
		ON	(ug.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId)
	INNER JOIN dbo.Mc_User AS u
		ON	(ug.UserId = u.UserId) AND (u.UserId = @UserId)
	INNER JOIN dbo.Mc_GroupsInstancesRoles AS gir
		ON (g.GroupId = gir.GroupId) AND (gir.InstanceId = @InstanceId);
END
