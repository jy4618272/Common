CREATE PROCEDURE [dbo].[Mc_InsertResource]
(
	@ResourceId uniqueidentifier,
	@ParentResourceId uniqueidentifier,
	@LocalObjectType nvarchar(50),
	@LocalObjectId nvarchar(50),
	@Content varbinary(max),
	@ContentType varchar(255),
	@Name nvarchar(255),
	@Width int,
	@Height int,
	@Align int,
	@Temporary bit,
	@CreatedTime datetime
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Resource (ResourceId, ParentResourceId, LocalObjectType, LocalObjectId, Content, ContentType, [Name], Width, Height, Align, Temporary, CreatedTime) 
	VALUES (@ResourceId, @ParentResourceId, @LocalObjectType, @LocalObjectId, @Content, @ContentType, @Name, @Width, @Height, @Align, @Temporary, @CreatedTime);
	
	SELECT ResourceId, ParentResourceId, LocalObjectType, LocalObjectId, Content, ContentType, [Name], Width, Height, Align, Temporary, CreatedTime
	FROM dbo.Mc_Resource 
	WHERE ResourceId = @ResourceId;
END