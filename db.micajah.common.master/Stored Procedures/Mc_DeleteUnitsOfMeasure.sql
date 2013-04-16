CREATE PROCEDURE [dbo].[Mc_DeleteUnitsOfMeasure]
(
	@UnitsOfMeasureId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM [dbo].[Mc_UnitsOfMeasureConversion]
    WHERE OrganizationId = @OrganizationId
    AND (UnitOfMeasureFrom = @UnitsOfMeasureId OR UnitOfMeasureTo = @UnitsOfMeasureId);

	DELETE FROM [dbo].[Mc_UnitsOfMeasure] 
	WHERE OrganizationId = @OrganizationId
	AND UnitsOfMeasureId = @UnitsOfMeasureId;
END
