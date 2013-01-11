CREATE PROCEDURE [dbo].[Mc_UpdateResource]
(
	@ResourceId uniqueidentifier,
	@LocalObjectType nvarchar(50),
	@LocalObjectId nvarchar(50),
	@Temporary bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Resource 
	SET 
		LocalObjectType = @LocalObjectType,
		LocalObjectId = @LocalObjectId,
		Temporary = @Temporary
	WHERE (ResourceId = @ResourceId) OR (ParentResourceId = @ResourceId);
	
	SELECT ResourceId, ParentResourceId, LocalObjectType, LocalObjectId, Content, ContentType, [Name], Width, Height, Align, Temporary, CreatedTime
	FROM dbo.Mc_Resource 
	WHERE ResourceId = @ResourceId;
END