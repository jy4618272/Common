CREATE PROCEDURE [dbo].[Mc_GetGroupsInstancesRolesByGroupsInstanceId]
(
	@OrganizationId uniqueidentifier,
	@Groups varchar(1024),
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT gdr.GroupId, gdr.InstanceId, gdr.RoleId
	FROM dbo.Mc_GroupsInstancesRoles AS gdr
	INNER JOIN dbo.Mc_Group AS g
		ON	(gdr.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId)
	INNER JOIN dbo.Mc_Instance AS d
		ON	(gdr.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.OrganizationId = @OrganizationId) AND (d.InstanceId = @InstanceId)
	WHERE (',' + @Groups + ',' LIKE '%,' + CAST(gdr.GroupId as varchar(50)) + ',%');
END
