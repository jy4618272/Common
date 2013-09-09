CREATE PROCEDURE [dbo].[Mc_GetUnitsOfMeasureByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
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
	WHERE OrganizationId = @OrganizationId
	ORDER BY [GroupName] ASC, [SingularName] ASC;
END
