
CREATE PROCEDURE [dbo].[Mc_UpdateGroupInstanceAction]
(
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@ActionId uniqueidentifier,
	@Enabled bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_GroupsInstancesActions 
	SET [Enabled] = @Enabled 
	WHERE (ActionId = @ActionId) AND (InstanceId = @InstanceId) AND (GroupId = @GroupId);
	
	SELECT GroupId, InstanceId, ActionId, [Enabled] 
	FROM dbo.Mc_GroupsInstancesActions 
	WHERE (ActionId = @ActionId) AND (InstanceId = @InstanceId) AND (GroupId = @GroupId);
END

