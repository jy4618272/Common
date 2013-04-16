CREATE PROCEDURE [dbo].[Mc_GetUnitsOfMeasure]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [UnitsOfMeasureId]
      ,[OrganizationId]
      ,[SingularName]
      ,[SingularAbbrv]
      ,[PluralName]
      ,[PluralAbbrv]
      ,[GroupName]
      ,[LocalName]
	FROM [dbo].[Mc_UnitsOfMeasure]
	ORDER BY [OrganizationId] asc, [GroupName] asc, [SingularName] asc;
END
