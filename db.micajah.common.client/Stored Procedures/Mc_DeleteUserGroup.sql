
CREATE PROCEDURE [dbo].[Mc_DeleteUserGroup]
(
	@UserId uniqueidentifier,
	@GroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_UsersGroups 
	WHERE (UserId = @UserId) AND (GroupId = @GroupId);
END

