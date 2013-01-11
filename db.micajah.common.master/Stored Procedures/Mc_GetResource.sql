CREATE PROCEDURE [dbo].[Mc_GetResource]
(
	@ResourceId uniqueidentifier,
	@Height int,
	@Width int,
	@Align int
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	DECLARE @ResId uniqueidentifier;
	
	IF (@Height IS NOT NULL) OR (@Width IS NOT NULL)
	BEGIN
		IF EXISTS(SELECT 0 FROM dbo.Mc_Resource WHERE (ResourceId = @ResourceId) AND (UPPER(LEFT(ContentType, 6)) = 'IMAGE/'))
		BEGIN
			SELECT @ResId = ResourceId
			FROM dbo.Mc_Resource
			WHERE (ParentResourceId = @ResourceId)
				AND (((@Height IS NULL) AND (Height IS NULL)) OR Height = @Height)
				AND (((@Width IS NULL) AND (Width IS NULL)) OR Width = @Width)
				AND (((@Align IS NULL) AND (Align IS NULL)) OR Align = @Align);
		END
	END
	
	IF (@ResId IS NULL)
		SET @ResId = @ResourceId;

	SELECT ResourceId, ParentResourceId, LocalObjectType, LocalObjectId, Content, ContentType, [Name], Width, Height, Align, Temporary, CreatedTime
	FROM dbo.Mc_Resource
	WHERE (ResourceId = @ResId);
END
