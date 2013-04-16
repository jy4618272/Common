
CREATE PROCEDURE [dbo].[Mc_DeleteGroupInstanceAction]
(
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@ActionId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_GroupsInstancesActions 
	WHERE (GroupId = @GroupId) AND (InstanceId = @InstanceId) AND (ActionId = @ActionId);
END

