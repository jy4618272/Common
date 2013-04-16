
CREATE PROCEDURE [dbo].[Mc_InsertGroupInstanceRole]
(
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@RoleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_GroupsInstancesRoles (GroupId, InstanceId, RoleId) 
	VALUES (@GroupId, @InstanceId, @RoleId);
	
	SELECT GroupId, InstanceId, RoleId 
	FROM dbo.Mc_GroupsInstancesRoles 
	WHERE (InstanceId = @InstanceId) AND (GroupId = @GroupId);
END

