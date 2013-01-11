CREATE PROCEDURE [dbo].[Mc_UpdateOrganizationUser]
(
	@OrganizationId uniqueidentifier,
	@UserId uniqueidentifier,
	@OrganizationAdministrator bit,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_OrganizationsUsers 
	SET OrganizationAdministrator = (CASE WHEN @OrganizationAdministrator IS NULL THEN OrganizationAdministrator ELSE @OrganizationAdministrator END)
		, Active = (CASE WHEN @Active IS NULL THEN Active ELSE @Active END)
	WHERE (OrganizationId = @OrganizationId) AND (UserId = @UserId);

	SELECT OrganizationId, UserId, OrganizationAdministrator, Active
	FROM dbo.Mc_OrganizationsUsers
	WHERE (OrganizationId = @OrganizationId) AND (UserId = @UserId);
END