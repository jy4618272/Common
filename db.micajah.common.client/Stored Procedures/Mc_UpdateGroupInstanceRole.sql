
CREATE PROCEDURE [dbo].[Mc_UpdateGroupInstanceRole]
(
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@RoleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_GroupsInstancesRoles 
	SET GroupId = @GroupId, InstanceId = @InstanceId, RoleId = @RoleId 
	WHERE (GroupId = @GroupId) AND (InstanceId = @InstanceId);
	
	SELECT GroupId, InstanceId, RoleId 
	FROM dbo.Mc_GroupsInstancesRoles 
	WHERE (InstanceId = @InstanceId) AND (GroupId = @GroupId);
END

