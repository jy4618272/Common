CREATE PROCEDURE [dbo].[Mc_GetUnitOfMeasureConversionFromByOrganizationId]
(
	@UnitOfMeasureFrom uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [UnitOfMeasureFrom]
	  ,[UnitOfMeasureTo]
	  ,[OrganizationId]
	  ,[Factor]
	FROM [dbo].[Mc_UnitsOfMeasureConversion]
	WHERE (UnitOfMeasureFrom = @UnitOfMeasureFrom) AND (OrganizationId = @OrganizationId);
END
