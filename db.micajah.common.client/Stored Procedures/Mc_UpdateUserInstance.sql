CREATE PROCEDURE [dbo].[Mc_UpdateUserInstance]
(
	@UserId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_UsersInstances 
	SET Active = @Active
	WHERE (UserId = @UserId) AND (InstanceId = @InstanceId);
	
	SELECT UserId, InstanceId, Active 
	FROM dbo.Mc_UsersInstances 
	WHERE (UserId = @UserId) AND (InstanceId = @InstanceId);
END
