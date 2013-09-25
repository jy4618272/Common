CREATE PROCEDURE [dbo].[Mc_GetGroupsInstancesRolesByGroupId]
(
	@OrganizationId uniqueidentifier,
	@GroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT gdr.GroupId, gdr.InstanceId, gdr.RoleId, d.Name AS InstanceName
	FROM dbo.Mc_GroupsInstancesRoles AS gdr
	INNER JOIN dbo.Mc_Group AS g
		ON	(gdr.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId) AND (g.GroupId = @GroupId)
	INNER JOIN dbo.Mc_Instance AS d
		ON	(gdr.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.OrganizationId = @OrganizationId);
END
