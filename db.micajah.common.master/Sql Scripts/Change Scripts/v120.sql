BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE NONCLUSTERED INDEX [IX_Mc_Resource_LocalObjectType_LocalObjectId]
		ON [dbo].[Mc_Resource]([LocalObjectType] ASC, [LocalObjectId] ASC);

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetResources]
		(
			@LocalObjectType nvarchar(50),
			@LocalObjectId nvarchar(255)
		)
		AS
		BEGIN
			SET NOCOUNT OFF;

			SELECT ResourceId, ParentResourceId, LocalObjectType, LocalObjectId, convert(varbinary, '''') AS Content, ContentType, [Name], Width, Height, Align, Temporary, CreatedTime
			FROM dbo.Mc_Resource
			WHERE (LocalObjectType = @LocalObjectType) AND (LocalObjectId = @LocalObjectId)
			ORDER BY CreatedTime, Name;
		END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION