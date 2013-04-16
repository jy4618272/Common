CREATE PROCEDURE [dbo].[Mc_GetCustomUrl]
(
	@CustomUrlId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE (CustomUrlId = @CustomUrlId);
END
