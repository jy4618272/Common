
CREATE PROCEDURE [dbo].[Mc_DeleteOrganizationLdapGroup]
(
	@OrganizationId uniqueidentifier,
	@Domain nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_OrganizationsLdapGroups 
	WHERE (OrganizationId = @OrganizationId) AND (Domain = @Domain)
END
