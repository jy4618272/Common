CREATE PROCEDURE [dbo].[Mc_DeleteUnitsOfMeasureConversion]
(
	@UnitOfMeasureFrom uniqueidentifier,
	@UnitOfMeasureTo uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.[Mc_UnitsOfMeasureConversion]
	WHERE UnitOfMeasureFrom = @UnitOfMeasureFrom
	AND UnitOfMeasureTo = @UnitOfMeasureTo
	AND OrganizationId = @OrganizationId;
END
