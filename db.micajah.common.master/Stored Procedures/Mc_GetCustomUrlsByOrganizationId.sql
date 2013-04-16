CREATE PROCEDURE [dbo].[Mc_GetCustomUrlsByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE (OrganizationId = @OrganizationId);
END
