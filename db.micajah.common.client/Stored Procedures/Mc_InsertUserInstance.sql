CREATE PROCEDURE [dbo].[Mc_InsertUserInstance]
(
	@UserId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_UsersInstances (UserId, InstanceId, Active) 
	VALUES (@UserId, @InstanceId, @Active);
	
	SELECT UserId, InstanceId, Active 
	FROM dbo.Mc_UsersInstances 
	WHERE (UserId = @UserId) AND (InstanceId = @InstanceId);
END
