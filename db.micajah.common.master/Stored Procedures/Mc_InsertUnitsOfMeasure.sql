CREATE PROCEDURE [dbo].[Mc_InsertUnitsOfMeasure]
(
	@UnitsOfMeasureId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@SingularName nvarchar(255),
	@SingularAbbrv nvarchar(50),
	@PluralName nvarchar(255),
	@PluralAbbrv nvarchar(50),
	@GroupName nvarchar(50),
	@LocalName nvarchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_UnitsOfMeasure]
           ([UnitsOfMeasureId]
           ,[OrganizationId]
           ,[SingularName]
           ,[SingularAbbrv]
           ,[PluralName]
           ,[PluralAbbrv]
           ,[GroupName]
           ,[LocalName])
     VALUES(
			@UnitsOfMeasureId
           ,@OrganizationId
           ,@SingularName
           ,@SingularAbbrv
           ,@PluralName
           ,@PluralAbbrv
           ,@GroupName
           ,@LocalName);
	
	SELECT [UnitsOfMeasureId]
      ,[OrganizationId]
      ,[SingularName]
      ,[SingularAbbrv]
      ,[PluralName]
      ,[PluralAbbrv]
      ,[GroupName]
      ,[LocalName]
	FROM [dbo].[Mc_UnitsOfMeasure]
	WHERE [UnitsOfMeasureId] = @UnitsOfMeasureId
	AND [OrganizationId] = @OrganizationId;
END