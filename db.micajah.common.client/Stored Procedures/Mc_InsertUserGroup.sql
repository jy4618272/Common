
CREATE PROCEDURE [dbo].[Mc_InsertUserGroup]
(
	@UserId uniqueidentifier,
	@GroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_UsersGroups (UserId, GroupId) 
	VALUES (@UserId, @GroupId);
	
	SELECT UserId, GroupId 
	FROM dbo.Mc_UsersGroups 
	WHERE (GroupId = @GroupId) AND (UserId = @UserId);
END

