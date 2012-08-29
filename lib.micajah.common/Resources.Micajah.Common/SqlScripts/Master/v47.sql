BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Organization_Mc_Resource')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [FK_Mc_Organization_Mc_Resource]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Resource_ResourceId')
      ALTER TABLE [dbo].[Mc_Resource] DROP CONSTRAINT [DF_Mc_Resource_ResourceId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Resource_Temporary')
      ALTER TABLE [dbo].[Mc_Resource] DROP CONSTRAINT [DF_Mc_Resource_Temporary]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE TABLE [dbo].[tmp_Mc_Resource] (
   [ResourceId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Mc_Resource_ResourceId] DEFAULT (newid()),
   [ParentResourceId] [uniqueidentifier] NULL,
   [LocalObjectType] [nvarchar] (50) NOT NULL,
   [LocalObjectId] [nvarchar] (255) NOT NULL,
   [Content] [varbinary] (max) NOT NULL,
   [ContentType] [varchar] (255) NULL,
   [Name] [nvarchar] (255) NULL,
   [Width] [int] NULL,
   [Height] [int] NULL,
   [Align] [int] NULL,
   [Temporary] [bit] NOT NULL CONSTRAINT [DF_Mc_Resource_Temporary] DEFAULT ((0)),
   [CreatedTime] [datetime] NOT NULL
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Resource] ([ResourceId], [ParentResourceId], [LocalObjectType], [LocalObjectId], [Content], [ContentType], [Name], [Width], [Height], [Align], [Temporary], [CreatedTime])
	SELECT r.[ResourceId], r.[ParentResourceId], 'OrganizationLogo', LOWER(REPLACE(CAST(OrganizationId AS nvarchar(255)), '-', '')), r.[Content], r.[ContentType], r.[Name], r.[Width], r.[Height], r.[Align], r.[Temporary], r.[CreatedDate]
	FROM [dbo].[Mc_Resource] AS r
	LEFT JOIN dbo.Mc_Organization AS o
		ON	r.ResourceId = o.LogoImageResourceId
	WHERE OrganizationId IS NOT NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP TABLE [dbo].[Mc_Resource]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC sp_rename N'[dbo].[tmp_Mc_Resource]', N'Mc_Resource'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Resource] ADD CONSTRAINT [PK_Mc_Resource] PRIMARY KEY CLUSTERED ([ResourceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Organization]
      DROP COLUMN [LogoImageResourceId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_DeleteResource]
(
	@ResourceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	DECLARE @Date datetime;
	SET @Date = DATEADD(day, -1, GETDATE());

	DELETE FROM dbo.Mc_Resource
	WHERE (ResourceId = @ResourceId) 
		OR (ParentResourceId = @ResourceId)
		OR ((Temporary = 1) AND (CreatedTime <= @Date));
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizations]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT OrganizationId, [Name], [Description], WebsiteUrl, DatabaseId, Deleted 
	FROM dbo.Mc_Organization 
	WHERE (Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizationsByLoginId]
(
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT o.OrganizationId, o.[Name], o.[Description], o.WebsiteUrl, o.DatabaseId, o.Deleted
	FROM dbo.Mc_OrganizationsLogins AS ol
	INNER JOIN dbo.Mc_Organization AS o
		ON	(ol.OrganizationId = o.OrganizationId) AND (ol.LoginId = @LoginId) AND (o.Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetResource]
(
	@ResourceId uniqueidentifier,
	@Height int,
	@Width int,
	@Align int
)
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @ResId uniqueidentifier;
	
	IF (@Height IS NOT NULL) OR (@Width IS NOT NULL)
	BEGIN
		IF EXISTS(SELECT 0 FROM dbo.Mc_Resource WHERE (ResourceId = @ResourceId) AND (UPPER(LEFT(ContentType, 6)) = ''IMAGE/''))
		BEGIN
			SELECT @ResId = ResourceId
			FROM dbo.Mc_Resource
			WHERE (ParentResourceId = @ResourceId)
				AND (((@Height IS NULL) AND (Height IS NULL)) OR Height = @Height)
				AND (((@Width IS NULL) AND (Width IS NULL)) OR Width = @Width)
				AND (((@Align IS NULL) AND (Align IS NULL)) OR Align = @Align);
		END
	END
	
	IF (@ResId IS NULL)
		SET @ResId = @ResourceId;

	SELECT ResourceId, ParentResourceId, LocalObjectType, LocalObjectId, Content, ContentType, [Name], Width, Height, Align, Temporary, CreatedTime
	FROM dbo.Mc_Resource
	WHERE (ResourceId = @ResId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetResources]
(
	@LocalObjectType nvarchar(50),
	@LocalObjectId nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ResourceId, ParentResourceId, LocalObjectType, LocalObjectId, Content, ContentType, [Name], Width, Height, Align, Temporary, CreatedTime
	FROM dbo.Mc_Resource
	WHERE (LocalObjectType = @LocalObjectType) AND (LocalObjectId = @LocalObjectId)
	ORDER BY CreatedTime, Name;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertOrganization]
(
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(255),
	@WebsiteUrl nvarchar(2048),
	@DatabaseId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Organization (OrganizationId, [Name], [Description], WebsiteUrl, DatabaseId, Deleted) 
	VALUES (@OrganizationId, @Name, @Description, @WebsiteUrl, @DatabaseId, @Deleted);
	
	SELECT OrganizationId, [Name], [Description], WebsiteUrl, DatabaseId, Deleted 
	FROM dbo.Mc_Organization 
	WHERE (OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertResource]
(
	@ResourceId uniqueidentifier,
	@ParentResourceId uniqueidentifier,
	@LocalObjectType nvarchar(50),
	@LocalObjectId nvarchar(50),
	@Content varbinary(max),
	@ContentType varchar(255),
	@Name nvarchar(255),
	@Width int,
	@Height int,
	@Align int,
	@Temporary bit,
	@CreatedTime datetime
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Resource (ResourceId, ParentResourceId, LocalObjectType, LocalObjectId, Content, ContentType, [Name], Width, Height, Align, Temporary, CreatedTime) 
	VALUES (@ResourceId, @ParentResourceId, @LocalObjectType, @LocalObjectId, @Content, @ContentType, @Name, @Width, @Height, @Align, @Temporary, @CreatedTime);
	
	SELECT ResourceId, ParentResourceId, LocalObjectType, LocalObjectId, Content, ContentType, [Name], Width, Height, Align, Temporary, CreatedTime
	FROM dbo.Mc_Resource 
	WHERE ResourceId = @ResourceId;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateOrganization]
(
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(255),
	@WebsiteUrl nvarchar(2048),
	@DatabaseId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Organization 
	SET [Name] = @Name, [Description] = @Description, WebsiteUrl = @WebsiteUrl, DatabaseId = @DatabaseId, Deleted = @Deleted 
	WHERE (OrganizationId = @OrganizationId);
	
	SELECT OrganizationId, [Name], [Description], WebsiteUrl, DatabaseId, Deleted 
	FROM dbo.Mc_Organization 
	WHERE (OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateResource]
(
	@ResourceId uniqueidentifier,
	@LocalObjectType nvarchar(50),
	@LocalObjectId nvarchar(50),
	@Temporary bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Resource 
	SET 
		LocalObjectType = @LocalObjectType,
		LocalObjectId = @LocalObjectId,
		Temporary = @Temporary
	WHERE (ResourceId = @ResourceId) OR (ParentResourceId = @ResourceId);
	
	SELECT ResourceId, ParentResourceId, LocalObjectType, LocalObjectId, Content, ContentType, [Name], Width, Height, Align, Temporary, CreatedTime
	FROM dbo.Mc_Resource 
	WHERE ResourceId = @ResourceId;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Resource_Mc_Resource')
      ALTER TABLE [dbo].[Mc_Resource] ADD CONSTRAINT [FK_Mc_Resource_Mc_Resource] FOREIGN KEY ([ParentResourceId]) REFERENCES [dbo].[Mc_Resource] ([ResourceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
