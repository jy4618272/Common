BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetUnitOfMeasure]
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
	WHERE (UnitsOfMeasureId = @UnitsOfMeasureId) AND (OrganizationId = @OrganizationId OR OrganizationId = ''00000000-0000-0000-0000-000000000000'')
	ORDER BY [OrganizationId] DESC;
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetUnitOfMeasureConversionByOrganizationId]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetUnitOfMeasureConversionFromByOrganizationId]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetUnitsOfMeasureByOrganizationId]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION