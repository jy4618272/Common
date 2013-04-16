CREATE PROCEDURE [dbo].[Mc_InsertOrganizationUser]
(
	@OrganizationId uniqueidentifier,
	@UserId uniqueidentifier,
	@OrganizationAdministrator bit,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_OrganizationsUsers (OrganizationId, UserId, OrganizationAdministrator, Active) 
	VALUES (@OrganizationId, @UserId, @OrganizationAdministrator, @Active);
	
	SELECT OrganizationId, UserId, OrganizationAdministrator, Active
	FROM dbo.Mc_OrganizationsUsers
	WHERE (OrganizationId = @OrganizationId) AND (UserId = @UserId);
END