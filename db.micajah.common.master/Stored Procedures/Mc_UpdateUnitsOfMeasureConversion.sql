CREATE PROCEDURE [dbo].[Mc_UpdateUnitsOfMeasureConversion]
(
	@UnitOfMeasureFrom uniqueidentifier,
	@UnitOfMeasureTo uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@Factor float
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [dbo].[Mc_UnitsOfMeasureConversion]
	SET [Factor] = @Factor
	WHERE UnitOfMeasureFrom = @UnitOfMeasureFrom
	AND UnitOfMeasureTo = @UnitOfMeasureTo
	AND OrganizationId = @OrganizationId;

	SELECT [UnitOfMeasureFrom]
      ,[UnitOfMeasureTo]
      ,[Factor]
	FROM [dbo].[Mc_UnitsOfMeasureConversion]
	WHERE UnitOfMeasureFrom = @UnitOfMeasureFrom
	AND UnitOfMeasureTo = @UnitOfMeasureTo
	AND OrganizationId = @OrganizationId;
END
