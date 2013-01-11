CREATE PROCEDURE [dbo].[Mc_UpdateOrganizationLogin]
(
	@OrganizationId uniqueidentifier,
	@LoginId uniqueidentifier,
	@OrganizationAdministrator bit,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_OrganizationsLogins 
	SET OrganizationAdministrator = (CASE WHEN @OrganizationAdministrator IS NULL THEN OrganizationAdministrator ELSE @OrganizationAdministrator END)
		, Active = (CASE WHEN @Active IS NULL THEN Active ELSE @Active END)
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
	
	SELECT OrganizationId, LoginId, OrganizationAdministrator, LdapDomain, LdapUserAlias, LdapSecurityId, LdapUserId, Active
	FROM dbo.Mc_OrganizationsLogins
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
END