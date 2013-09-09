CREATE PROCEDURE [dbo].[Mc_GetUnitOfMeasure]
(
	@UnitsOfMeasureId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT TOP 1 [UnitsOfMeasureId]
      ,[OrganizationId]
      ,[SingularName]
      ,[SingularAbbrv]
      ,[PluralName]
      ,[PluralAbbrv]
      ,[GroupName]
      ,[LocalName]
	FROM [dbo].[Mc_UnitsOfMeasure]
	WHERE (UnitsOfMeasureId = @UnitsOfMeasureId) AND (OrganizationId = @OrganizationId OR OrganizationId = '00000000-0000-0000-0000-000000000000')
	ORDER BY [OrganizationId] DESC;
END
