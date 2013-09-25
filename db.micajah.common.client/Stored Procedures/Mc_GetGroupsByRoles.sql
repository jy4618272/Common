CREATE PROCEDURE [dbo].[Mc_GetGroupsByRoles]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Roles varchar(1024)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT DISTINCT g.GroupId, g.OrganizationId, g.[Name], g.[Description], g.BuiltIn, g.Deleted 
	FROM dbo.Mc_Group AS g
	INNER JOIN dbo.Mc_GroupsInstancesRoles AS gir
		ON g.GroupId = gir.GroupId
	WHERE (g.OrganizationId = @OrganizationId) AND (g.Deleted = 0)
		AND (@InstanceId IS NULL OR gir.InstanceId = @InstanceId)
		AND (',' + @Roles + ',' LIKE '%,' + CAST(gir.RoleId as varchar(50)) + ',%');
END
