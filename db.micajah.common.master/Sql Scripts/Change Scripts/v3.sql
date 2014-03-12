BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DELETE FROM [dbo].[Mc_SettingListValues]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	SET IDENTITY_INSERT [dbo].[Mc_SettingListValues] ON

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (1, 9, N'None', N'None', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (2, 9, N'Tan gray', N'TanGray', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (3, 9, N'Blue', N'Blue', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (4, 9, N'Brown', N'Brown', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (5, 9, N'Gray', N'Gray', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (6, 9, N'Green', N'Green', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (7, 9, N'Yellow', N'Yellow', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (8, 9, N'Red', N'Red', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (9, 9, N'Silver', N'Silver', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (10, 9, N'White', N'White', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (11, 22, N'MD5 hash algorithm', N'Md5', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (12, 22, N'Non encrypted', N'None', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (13, 62, N'Left', N'Left', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (14, 62, N'Top', N'Top', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (15, 63, N'96x96 pixels', N'Big', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (16, 63, N'64x64 pixels', N'Large', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (17, 63, N'48x48 pixels', N'Normal', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (18, 63, N'32x32 pixels', N'Small', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (19, 63, N'40x40 pixels', N'x40', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (20, 63, N'80x80 pixels', N'x80', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (21, 64, N'Gradient', N'Gradient', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (22, 64, N'Reflective', N'Reflective', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (23, 64, N'Standard', N'Standard', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (24, 65, N'Blue', N'Blue', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (25, 65, N'Brown', N'Brown', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (26, 65, N'Gray', N'Gray', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (27, 65, N'Olive', N'Olive', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (28, 65, N'Orange', N'Orange', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (29, 65, N'Red', N'Red', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (30, 65, N'Violet', N'Violet', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (31, 65, N'Yellow', N'Yellow', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (32, 67, N'Reflective', N'Reflective', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (33, 67, N'Standard', N'Standard', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_SettingListValues]([SettingListValuesId], [SettingId], [Name], [Value], [Deleted]) VALUES (34, 64, N'Standard - Tabs', N'StandardTabs', 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	SET IDENTITY_INSERT [dbo].[Mc_SettingListValues] OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_OrganizationsLogins_Mc_Organization')
      ALTER TABLE [dbo].[Mc_OrganizationsLogins] DROP CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Organization]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Organization_Mc_Database')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [FK_Mc_Organization_Mc_Database]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_Description')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Description]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_WebSiteUrl')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_WebSiteUrl]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_IconUrl')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_IconUrl]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_Deleted')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE TABLE [dbo].[tmp_Mc_Organization] (
	   [OrganizationId] [int] IDENTITY (1, 1) NOT NULL,
	   [OrganizationGuid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Mc_Organization_OrganizationGuid] DEFAULT (newid()),
	   [Name] [nvarchar] (255) NOT NULL,
	   [Description] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_Organization_Description] DEFAULT (N''),
	   [WebSiteUrl] [nvarchar] (2048) NOT NULL CONSTRAINT [DF_Mc_Organization_WebSiteUrl] DEFAULT (N''),
	   [IconUrl] [nvarchar] (2048) NOT NULL CONSTRAINT [DF_Mc_Organization_IconUrl] DEFAULT (N''),
	   [DatabaseId] [int] NULL,
	   [Deleted] [bit] NOT NULL CONSTRAINT [DF_Mc_Organization_Deleted] DEFAULT ((0))
	)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   SET IDENTITY_INSERT [dbo].[tmp_Mc_Organization] ON

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   INSERT INTO [dbo].[tmp_Mc_Organization] ([OrganizationId], [Name], [Description], [WebSiteUrl], [IconUrl], [DatabaseId], [Deleted])
   SELECT [OrganizationId], [Name], [Description], [WebSiteUrl], [IconUrl], [DatabaseId], [Deleted]
   FROM [dbo].[Mc_Organization]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   SET IDENTITY_INSERT [dbo].[tmp_Mc_Organization] OFF

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
   ALTER TABLE [dbo].[Mc_Organization] ADD CONSTRAINT [PK_Mc_Organization] PRIMARY KEY CLUSTERED ([OrganizationId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertOrganization]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		EXEC('/****** Object:  StoredProcedure [dbo].[Mc_InsertOrganization] ******/
ALTER PROCEDURE [dbo].[Mc_InsertOrganization]
(
	@Name nvarchar(255),
	@Description nvarchar(255),
	@WebSiteUrl nvarchar(2048),
	@IconUrl nvarchar(2048),
	@DatabaseId int,
	@Deleted bit
)
AS
	SET NOCOUNT OFF;
INSERT INTO [dbo].[Mc_Organization] ([Name], [Description], [WebSiteUrl], [IconUrl], [DatabaseId], [Deleted]) VALUES (@Name, @Description, @WebSiteUrl , @IconUrl, @DatabaseId, @Deleted);
	
SELECT [OrganizationId], [OrganizationGuid], [Name], Description, WebSiteUrl, IconUrl, DatabaseId, Deleted FROM Mc_Organization WHERE (OrganizationId = SCOPE_IDENTITY())')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllOrganization]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		EXEC('/****** Object:  StoredProcedure [dbo].[Mc_SelectAllOrganization] ******/
ALTER PROCEDURE [dbo].[Mc_SelectAllOrganization]
AS
	SET NOCOUNT ON;
SELECT OrganizationId, [OrganizationGuid], [Name], Description, WebSiteUrl, IconUrl, DatabaseId, Deleted FROM dbo.Mc_Organization WHERE (Deleted = 0)')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllOrganizationByLoginId]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		EXEC('/****** Object:  StoredProcedure [dbo].[Mc_SelectAllOrganizationByLoginId] ******/
ALTER PROCEDURE [dbo].[Mc_SelectAllOrganizationByLoginId]
	@LoginId int
AS
	SET NOCOUNT ON;

SELECT 
	o.[OrganizationId]
	,o.[OrganizationGuid]
	,o.[Name]
	,o.Description
	,o.WebSiteUrl
	,o.IconUrl
	,o.DatabaseId
	,o.Deleted
FROM dbo.Mc_OrganizationsLogins ol
INNER JOIN dbo.Mc_Organization o
	ON	ol.OrganizationId = o.OrganizationId
		AND ol.LoginId = @LoginId
		AND o.Deleted = 0;')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateOrganization]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		EXEC('/****** Object:  StoredProcedure [dbo].[Mc_UpdateOrganization] ******/
ALTER PROCEDURE [dbo].[Mc_UpdateOrganization]
(
	@OrganizationId int,
	@Name nvarchar(255),
	@Description nvarchar(255),
	@WebSiteUrl nvarchar(2048),
	@IconUrl nvarchar(2048),
	@DatabaseId int,
	@Deleted bit
)
AS
	SET NOCOUNT OFF;
UPDATE [dbo].[Mc_Organization] SET [Name] = @Name, [Description] = @Description, [WebSiteUrl] = @WebSiteUrl, [IconUrl] = @IconUrl, [DatabaseId] = @DatabaseId, [Deleted] = @Deleted WHERE ([OrganizationId] = @OrganizationId);
	
SELECT [OrganizationId], [OrganizationGuid], [Name], Description, WebSiteUrl, IconUrl, DatabaseId, Deleted FROM Mc_Organization WHERE (OrganizationId = @OrganizationId)')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Organization_Mc_Database')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [FK_Mc_Organization_Mc_Database]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Organization_Mc_Database')
      ALTER TABLE [dbo].[Mc_Organization] ADD CONSTRAINT [FK_Mc_Organization_Mc_Database] FOREIGN KEY ([DatabaseId]) REFERENCES [dbo].[Mc_Database] ([DatabaseId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_OrganizationsLogins]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
		IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_OrganizationsLogins_Mc_Organization')
			ALTER TABLE [dbo].[Mc_OrganizationsLogins] ADD CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION