CREATE PROCEDURE [dbo].[Mc_GetResources]
(
	@LocalObjectType nvarchar(50),
	@LocalObjectId nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT ResourceId, ParentResourceId, LocalObjectType, LocalObjectId, convert(varbinary, '') AS Content, ContentType, [Name], Width, Height, Align, Temporary, CreatedTime
	FROM dbo.Mc_Resource
	WHERE (LocalObjectType = @LocalObjectType) AND (LocalObjectId = @LocalObjectId)
	ORDER BY CreatedTime, Name;
END
