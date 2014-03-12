BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_MC_UnitsOfMeasure_UnitsOfMeasureId')
      ALTER TABLE [dbo].[Mc_UnitsOfMeasure] DROP CONSTRAINT [DF_MC_UnitsOfMeasure_UnitsOfMeasureId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_UnitsOfMeasure_GroupName')
      ALTER TABLE [dbo].[Mc_UnitsOfMeasure] DROP CONSTRAINT [DF_Mc_UnitsOfMeasure_GroupName]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE TABLE [dbo].[tmp_Mc_UnitsOfMeasure] (
   [UnitsOfMeasureId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_MC_UnitsOfMeasure_UnitsOfMeasureId] DEFAULT (newid()),
   [OrganizationId] [uniqueidentifier] NULL,
   [SingularName] [nvarchar] (255) NOT NULL,
   [SingularAbbrv] [nvarchar] (50) NOT NULL,
   [PluralName] [nvarchar] (255) NOT NULL,
   [PluralAbbrv] [nvarchar] (50) NOT NULL,
   [GroupName] [nvarchar] (50) NOT NULL CONSTRAINT [DF_Mc_UnitsOfMeasure_GroupName] DEFAULT (N''),
   [LocalName] [nvarchar] (50) NOT NULL CONSTRAINT [DF_Mc_UnitsOfMeasure_LocalName] DEFAULT (N'English')
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   INSERT INTO [dbo].[tmp_Mc_UnitsOfMeasure] ([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName])
   SELECT [UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName]
   FROM [dbo].[Mc_UnitsOfMeasure]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
begin
	DROP TABLE [dbo].[Mc_UnitsOfMeasure];
	EXEC sp_rename N'[dbo].[tmp_Mc_UnitsOfMeasure]', N'Mc_UnitsOfMeasure';
end

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_UnitsOfMeasure] ADD CONSTRAINT [PK_Mc_UnitsOfMeasure] PRIMARY KEY CLUSTERED ([UnitsOfMeasureId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP PROCEDURE [dbo].[Mc_GetUnitsOfMeasure]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec('CREATE PROCEDURE [dbo].[Mc_GetUnitsOfMeasure]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [UnitsOfMeasureId]
      ,[OrganizationId]
      ,[SingularName]
      ,[SingularAbbrv]
      ,[PluralName]
      ,[PluralAbbrv]
      ,[GroupName]
      ,[LocalName]
	FROM [dbo].[Mc_UnitsOfMeasure]
	ORDER BY [OrganizationId] asc, [GroupName] asc, [SingularName] asc;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 DROP PROCEDURE [dbo].[Mc_InsertUnitsOfMeasure]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec('CREATE PROCEDURE [dbo].[Mc_InsertUnitsOfMeasure]
(
	@UnitsOfMeasureId uniqueidentifier,
	@OrganizationId uniqueidentifier = NULL,
	@SingularName nvarchar(255),
	@SingularAbbrv nvarchar(50),
	@PluralName nvarchar(255),
	@PluralAbbrv nvarchar(50),
	@GroupName nvarchar(50),
	@LocalName nvarchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_UnitsOfMeasure]
           ([UnitsOfMeasureId]
           ,[OrganizationId]
           ,[SingularName]
           ,[SingularAbbrv]
           ,[PluralName]
           ,[PluralAbbrv]
           ,[GroupName]
           ,[LocalName])
     VALUES(
			@UnitsOfMeasureId
           ,@OrganizationId
           ,@SingularName
           ,@SingularAbbrv
           ,@PluralName
           ,@PluralAbbrv
           ,@GroupName
           ,@LocalName);
	
	SELECT [UnitsOfMeasureId]
      ,[OrganizationId]
      ,[SingularName]
      ,[SingularAbbrv]
      ,[PluralName]
      ,[PluralAbbrv]
      ,[GroupName]
      ,[LocalName]
	FROM [dbo].[Mc_UnitsOfMeasure]
	WHERE [UnitsOfMeasureId] = @UnitsOfMeasureId;

END')

IF @@TRANCOUNT = 1 
	DROP PROCEDURE [dbo].[Mc_UpdateUnitsOfMeasure]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec('CREATE PROCEDURE [dbo].[Mc_UpdateUnitsOfMeasure]
(
	@UnitsOfMeasureId uniqueidentifier,
	@SingularName nvarchar(255),
	@SingularAbbrv nvarchar(50),
	@PluralName nvarchar(255),
	@PluralAbbrv nvarchar(50),
	@GroupName nvarchar(50),
	@LocalName nvarchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [dbo].[Mc_UnitsOfMeasure]
	SET [SingularName] = @SingularName
      ,[SingularAbbrv] = @SingularAbbrv
      ,[PluralName] = @PluralName
      ,[PluralAbbrv] = @PluralAbbrv
      ,[GroupName] = @GroupName
      ,[LocalName] = @LocalName
	WHERE UnitsOfMeasureId = @UnitsOfMeasureId;

	SELECT [UnitsOfMeasureId]
      ,[OrganizationId]
      ,[SingularName]
      ,[SingularAbbrv]
      ,[PluralName]
      ,[PluralAbbrv]
      ,[GroupName]
      ,[LocalName]
	FROM [dbo].[Mc_UnitsOfMeasure]
	WHERE [UnitsOfMeasureId] = @UnitsOfMeasureId;
	
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION