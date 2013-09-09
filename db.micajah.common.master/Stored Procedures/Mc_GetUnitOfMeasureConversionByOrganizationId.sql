CREATE PROCEDURE [dbo].[Mc_GetUnitOfMeasureConversionByOrganizationId]
(
	@UnitOfMeasureFrom uniqueidentifier,
	@UnitOfMeasureTo uniqueidentifier,
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
	WHERE (UnitOfMeasureFrom = @UnitOfMeasureFrom) AND (UnitOfMeasureTo = @UnitOfMeasureTo) AND (OrganizationId = @OrganizationId);
END
