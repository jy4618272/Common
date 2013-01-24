
CREATE PROCEDURE [dbo].[Mc_DeleteOrganizationLogin]
(
	@OrganizationId uniqueidentifier,
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_OrganizationsLogins 
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
END

