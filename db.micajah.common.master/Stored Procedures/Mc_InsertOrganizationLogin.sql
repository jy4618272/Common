CREATE PROCEDURE [dbo].[Mc_InsertOrganizationLogin]
(
	@OrganizationId uniqueidentifier,
	@LoginId uniqueidentifier,
	@OrganizationAdministrator bit,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_OrganizationsLogins (OrganizationId, LoginId, OrganizationAdministrator, Active)
	VALUES (@OrganizationId, @LoginId, @OrganizationAdministrator, @Active);
	
	SELECT OrganizationId, LoginId, OrganizationAdministrator, Active
	FROM dbo.Mc_OrganizationsLogins
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
END