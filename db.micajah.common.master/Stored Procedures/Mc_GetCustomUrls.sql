CREATE PROCEDURE [dbo].[Mc_GetCustomUrls]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@FullCustomUrl nvarchar(1024),
	@PartialCustomUrl nvarchar(1024)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE 
		(OrganizationId = @OrganizationId)
		AND (	(InstanceId = @InstanceId)
				OR  ((@InstanceId IS NULL) AND (InstanceId IS NULL))
		)
	UNION
	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE
		(	(FullCustomUrl = @FullCustomUrl) 
			AND (@FullCustomUrl IS NOT NULL)
		)
		OR 	(	(PartialCustomUrl = @PartialCustomUrl)
				AND (@PartialCustomUrl IS NOT NULL)
		);
END
