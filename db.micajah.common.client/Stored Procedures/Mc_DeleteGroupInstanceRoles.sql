
CREATE PROCEDURE [dbo].[Mc_DeleteGroupInstanceRoles]
(
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_GroupsInstancesRoles 
	WHERE (GroupId = @GroupId) AND (InstanceId = @InstanceId);
END

