CREATE PROCEDURE [dbo].[Mc_InsertCustomUrl]
(
	@CustomUrlId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@FullCustomUrl nvarchar(1024),
	@PartialCustomUrl nvarchar(1024)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_CustomUrl (CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl)
	VALUES (@CustomUrlId, @OrganizationId, @InstanceId, @FullCustomUrl, @PartialCustomUrl);
	
	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE CustomUrlId = @CustomUrlId;
END