CREATE PROCEDURE [dbo].[Mc_GetGroupsInstancesActions]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT gda.GroupId, gda.InstanceId, gda.ActionId, gda.[Enabled]
	FROM dbo.Mc_GroupsInstancesActions AS gda
	INNER JOIN dbo.Mc_Group AS g
		ON	(gda.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId)
	INNER JOIN dbo.Mc_Instance AS d
		ON	(gda.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.OrganizationId = @OrganizationId);
END
