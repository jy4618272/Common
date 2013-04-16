
CREATE PROCEDURE [dbo].[Mc_InsertGroupInstanceAction]
(
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@ActionId uniqueidentifier,
	@Enabled bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_GroupsInstancesActions (GroupId, InstanceId, ActionId, [Enabled]) 
	VALUES (@GroupId, @InstanceId, @ActionId, @Enabled);
	
	SELECT GroupId, InstanceId, ActionId, [Enabled] 
	FROM dbo.Mc_GroupsInstancesActions 
	WHERE (ActionId = @ActionId) AND (InstanceId = @InstanceId) AND (GroupId = @GroupId);
END

