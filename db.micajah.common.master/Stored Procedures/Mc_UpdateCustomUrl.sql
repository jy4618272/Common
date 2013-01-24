CREATE PROCEDURE [dbo].[Mc_UpdateCustomUrl]
(
	@CustomUrlId uniqueidentifier,
	@FullCustomUrl nvarchar(1024),
	@PartialCustomUrl nvarchar(1024)
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_CustomUrl
	SET FullCustomUrl = @FullCustomUrl, PartialCustomUrl = @PartialCustomUrl
	WHERE CustomUrlId = @CustomUrlId;
	
	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE CustomUrlId = @CustomUrlId;
END