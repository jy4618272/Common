CREATE PROCEDURE [dbo].[Mc_InsertUnitsOfMeasureConversion]
(
	@UnitOfMeasureFrom uniqueidentifier,
	@UnitOfMeasureTo uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@Factor float
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]
           ([UnitOfMeasureFrom]
           ,[UnitOfMeasureTo]
           ,[OrganizationId]
           ,[Factor])
    VALUES
           (@UnitOfMeasureFrom
           ,@UnitOfMeasureTo
           ,@OrganizationId
           ,@Factor);
	
	SELECT [UnitOfMeasureFrom]
      ,[UnitOfMeasureTo]
      ,[OrganizationId]
      ,[Factor]
	FROM [dbo].[Mc_UnitsOfMeasureConversion]
	WHERE UnitOfMeasureFrom = @UnitOfMeasureFrom
	AND UnitOfMeasureTo = @UnitOfMeasureTo
	AND OrganizationId = @OrganizationId;
END