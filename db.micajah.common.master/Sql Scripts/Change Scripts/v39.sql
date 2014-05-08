BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_IconUrl')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_IconUrl]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Organization]
      DROP COLUMN [IconUrl]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_Mc_InvitedLogin_Mc_Organization]') AND type = 'F')
		ALTER TABLE [dbo].[Mc_InvitedLogin] DROP CONSTRAINT [FK_Mc_InvitedLogin_Mc_Organization]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_Mc_Organization_Mc_Database]') AND type = 'F')
		ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [FK_Mc_Organization_Mc_Database]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_OrganizationId]') AND type = 'D')
		ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_OrganizationId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_Description]') AND type = 'D')
		ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Description]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_WebsiteUrl]') AND type = 'D')
		ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_WebsiteUrl]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Organization_Deleted]') AND type = 'D')
		ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_Mc_OrganizationsLogins_Mc_Organization]') AND type = 'F')
		ALTER TABLE [dbo].[Mc_OrganizationsLogins] DROP CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Organization]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[Mc_Organization]') AND name = N'PK_Mc_Organization')
		ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [PK_Mc_Organization]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[tmp_Mc_Organization](
		[OrganizationId] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Description] [nvarchar](255) NOT NULL,
		[WebsiteUrl] [nvarchar](2048) NOT NULL,
		[LogoImageResourceId] [uniqueidentifier] NULL,
		[DatabaseId] [uniqueidentifier] NULL,
		[Deleted] [bit] NOT NULL
	)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[tmp_Mc_Organization] ([OrganizationId], [Name], [Description], [WebsiteUrl], [LogoImageResourceId], [DatabaseId], [Deleted])
	SELECT [OrganizationId], [Name], [Description], [WebsiteUrl], NULL, [DatabaseId], [Deleted]
	FROM [dbo].[Mc_Organization]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP TABLE [dbo].[Mc_Organization]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC sp_rename N'[dbo].[tmp_Mc_Organization]', N'Mc_Organization'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_Resource] (
	   [ResourceId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Mc_Resource_ResourceId] DEFAULT (newid()),
	   [ParentResourceId] [uniqueidentifier] NULL,
	   [Content] [varbinary] (max) NOT NULL,
	   [ContentType] [varchar] (255) NULL,
	   [Name] [nvarchar] (255) NULL,
	   [Width] [int] NULL,
	   [Height] [int] NULL,
	   [Align] [int] NULL,
	   [Temporary] [bit] NOT NULL CONSTRAINT [DF_Mc_Resource_Temporary] DEFAULT ((0)),
	   [CreatedDate] [datetime] NOT NULL
	)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Resource] ADD CONSTRAINT [PK_Mc_Resource] PRIMARY KEY CLUSTERED ([ResourceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [PK_Mc_Organization] PRIMARY KEY CLUSTERED ([OrganizationId] ASC) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_InvitedLogin]  WITH CHECK ADD  CONSTRAINT [FK_Mc_InvitedLogin_Mc_Organization] FOREIGN KEY([OrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_InvitedLogin] CHECK CONSTRAINT [FK_Mc_InvitedLogin_Mc_Organization]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization]  WITH CHECK ADD  CONSTRAINT [FK_Mc_Organization_Mc_Database] FOREIGN KEY([DatabaseId]) REFERENCES [dbo].[Mc_Database] ([DatabaseId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization] CHECK CONSTRAINT [FK_Mc_Organization_Mc_Database]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization]  WITH CHECK ADD  CONSTRAINT [FK_Mc_Organization_Mc_Resource] FOREIGN KEY([LogoImageResourceId]) REFERENCES [dbo].[Mc_Resource] ([ResourceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization] CHECK CONSTRAINT [FK_Mc_Organization_Mc_Resource]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_OrganizationId]  DEFAULT (newid()) FOR [OrganizationId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_Description]  DEFAULT (N'') FOR [Description]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_WebsiteUrl]  DEFAULT (N'') FOR [WebsiteUrl]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_Deleted]  DEFAULT ((0)) FOR [Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_OrganizationsLogins]  WITH CHECK ADD  CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Organization] FOREIGN KEY([OrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_OrganizationsLogins] CHECK CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Organization]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Organization_Mc_Resource')
      ALTER TABLE [dbo].[Mc_Organization] ADD CONSTRAINT [FK_Mc_Organization_Mc_Resource] FOREIGN KEY ([LogoImageResourceId]) REFERENCES [dbo].[Mc_Resource] ([ResourceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Resource_Mc_Resource')
      ALTER TABLE [dbo].[Mc_Resource] ADD CONSTRAINT [FK_Mc_Resource_Mc_Resource] FOREIGN KEY ([ParentResourceId]) REFERENCES [dbo].[Mc_Resource] ([ResourceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('{4AB439DB-1E9A-4F2D-9EEA-016810EA3F7C}', '{00000000-0000-0000-0000-000000000015}', 1, N'Instance Profile', N'Edit instance profile', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/InstanceProfile.aspx', 0, N'', 1, 1, 1, 1, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('{D2FC3868-F8C5-4DE4-BE33-D538C276CE7B}', '{00000000-0000-0000-0000-000000000009}', 1, N'Organization Profile', N'Edit organization profile', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/OrganizationProfile.aspx', 0, N'', 1, 0, 1, 1, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteResource]
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
		OR ((Temporary = 1) AND (CreatedDate <= @Date));
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizations]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT OrganizationId, [Name], [Description], WebsiteUrl, LogoImageResourceId, DatabaseId, Deleted 
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

	SELECT o.OrganizationId, o.[Name], o.[Description], o.WebsiteUrl, o.LogoImageResourceId, o.DatabaseId, o.Deleted
	FROM dbo.Mc_OrganizationsLogins AS ol
	INNER JOIN dbo.Mc_Organization AS o
		ON	(ol.OrganizationId = o.OrganizationId) AND (ol.LoginId = @LoginId) AND (o.Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetResource]
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

	SELECT ResourceId, Content, ContentType, [Name], Width, Height, Align, Temporary, CreatedDate
	FROM dbo.Mc_Resource
	WHERE (ResourceId = @ResId);
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
	@LogoImageResourceId uniqueidentifier,
	@DatabaseId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Organization (OrganizationId, [Name], [Description], WebsiteUrl, LogoImageResourceId, DatabaseId, Deleted) 
	VALUES (@OrganizationId, @Name, @Description, @WebsiteUrl, @LogoImageResourceId, @DatabaseId, @Deleted);
	
	SELECT OrganizationId, [Name], [Description], WebsiteUrl, LogoImageResourceId, DatabaseId, Deleted 
	FROM dbo.Mc_Organization 
	WHERE (OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertResource]
(
	@ResourceId uniqueidentifier,
	@ParentResourceId uniqueidentifier,
	@Content varbinary(max),
	@ContentType varchar(255),
	@Name nvarchar(255),
	@Width int,
	@Height int,
	@Align int,
	@Temporary bit,
	@CreatedDate datetime
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Resource (ResourceId, ParentResourceId, Content, ContentType, [Name], Width, Height, Align, Temporary, CreatedDate) 
	VALUES (@ResourceId, @ParentResourceId, @Content, @ContentType, @Name, @Width, @Height, @Align, @Temporary, @CreatedDate);
	
	SELECT ResourceId, ParentResourceId, Content, ContentType, [Name], Width, Height, Align, Temporary, CreatedDate
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
	@LogoImageResourceId uniqueidentifier,
	@DatabaseId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Organization 
	SET [Name] = @Name, [Description] = @Description, WebsiteUrl = @WebsiteUrl, LogoImageResourceId = @LogoImageResourceId, DatabaseId = @DatabaseId, Deleted = @Deleted 
	WHERE (OrganizationId = @OrganizationId);
	
	SELECT OrganizationId, [Name], [Description], WebsiteUrl, LogoImageResourceId, DatabaseId, Deleted 
	FROM dbo.Mc_Organization 
	WHERE (OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateResource]
(
	@ResourceId uniqueidentifier,
	@Temporary bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Resource 
	SET Temporary = @Temporary
	WHERE (ResourceId = @ResourceId)
		OR (ParentResourceId = @ResourceId);
	
	SELECT ResourceId, ParentResourceId, Content, ContentType, [Name], Width, Height, Align, Temporary, CreatedDate
	FROM dbo.Mc_Resource 
	WHERE ResourceId = @ResourceId;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION