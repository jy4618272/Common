﻿CREATE PROCEDURE [dbo].[Mc_UpdateUnitsOfMeasure]
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

	UPDATE [dbo].[Mc_UnitsOfMeasure]
	SET [SingularName] = @SingularName
      ,[SingularAbbrv] = @SingularAbbrv
      ,[PluralName] = @PluralName
      ,[PluralAbbrv] = @PluralAbbrv
      ,[GroupName] = @GroupName
      ,[LocalName] = @LocalName
	WHERE UnitsOfMeasureId = @UnitsOfMeasureId
	AND OrganizationId = @OrganizationId;

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
