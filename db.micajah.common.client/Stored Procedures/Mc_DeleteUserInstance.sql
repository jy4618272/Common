CREATE PROCEDURE [dbo].[Mc_DeleteUserInstance]
(
	@UserId uniqueidentifier,
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_UsersInstances 
	WHERE (UserId = @UserId) AND (InstanceId = @InstanceId);
END
