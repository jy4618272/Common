CREATE PROCEDURE [dbo].[Mc_UpdateOrganizationGoogleAdminAuthToken]
	(
		@OrganizationId uniqueidentifier,		
		@GoogleAdminAuthToken nvarchar(500)
	)
	AS
	BEGIN
		SET NOCOUNT OFF;

		UPDATE dbo.Mc_Organization
		SET GoogleAdminAuthToken = @GoogleAdminAuthToken
		WHERE (OrganizationId = @OrganizationId);
	END
	