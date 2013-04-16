
CREATE PROCEDURE [dbo].[Mc_DeleteOrganizationUser]
(
	@OrganizationId uniqueidentifier,
	@UserId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_OrganizationsUsers 
	WHERE (OrganizationId = @OrganizationId) AND (UserId = @UserId);
END

