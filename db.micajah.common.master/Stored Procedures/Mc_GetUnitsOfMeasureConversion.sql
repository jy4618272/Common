CREATE PROCEDURE [dbo].[Mc_GetUnitsOfMeasureConversion]
(
	@UnitOfMeasureFrom uniqueidentifier=null,
	@UnitOfMeasureTo uniqueidentifier=null
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [UnitOfMeasureFrom]
	  ,[UnitOfMeasureTo]
	  ,[OrganizationId]
	  ,[Factor]
	FROM [dbo].[Mc_UnitsOfMeasureConversion]
	WHERE (@UnitOfMeasureFrom IS NULL OR UnitOfMeasureFrom = @UnitOfMeasureFrom)
	AND (@UnitOfMeasureTo IS NULL OR UnitOfMeasureTo = @UnitOfMeasureTo);
END
