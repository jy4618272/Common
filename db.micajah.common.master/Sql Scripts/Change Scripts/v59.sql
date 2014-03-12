BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_CustomUrl_Mc_Organization')
      ALTER TABLE [dbo].[Mc_CustomUrl] DROP CONSTRAINT [FK_Mc_CustomUrl_Mc_Organization]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_InvitedLogin_Mc_Organization')
      ALTER TABLE [dbo].[Mc_InvitedLogin] DROP CONSTRAINT [FK_Mc_InvitedLogin_Mc_Organization]

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
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_OrganizationId')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_OrganizationId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_Description')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Description]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_WebsiteUrl')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_WebsiteUrl]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_Deleted')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_LdapServerAddress')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_LdapServerAddress]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_LdapServerPort')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_LdapServerPort]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_LdapDomain')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_LdapDomain]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_LdapUserName')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_LdapUserName]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_LdapPassword')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_LdapPassword]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE TABLE [dbo].[tmp_Mc_Organization] (
   [OrganizationId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Mc_Organization_OrganizationId] DEFAULT (newid()),
   [Name] [nvarchar] (255) NOT NULL,
   [Description] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_Organization_Description] DEFAULT (N''),
   [WebsiteUrl] [nvarchar] (2048) NOT NULL CONSTRAINT [DF_Mc_Organization_WebsiteUrl] DEFAULT (N''),
   [DatabaseId] [uniqueidentifier] NULL,
   [FiscalYearStartDay] [int] NULL,
   [FiscalYearStartMonth] [int] NULL,
   [WeekStartsDay] [int] NULL,
   [LdapServerAddress] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_Organization_LdapServerAddress] DEFAULT (N''),
   [LdapServerPort] [nvarchar] (50) NOT NULL CONSTRAINT [DF_Mc_Organization_LdapServerPort] DEFAULT (N''),
   [LdapDomain] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_Organization_LdapDomain] DEFAULT (N''),
   [LdapUserName] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_Organization_LdapUserName] DEFAULT (N''),
   [LdapPassword] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_Organization_LdapPassword] DEFAULT (N''),
   [ExpirationTime] [datetime] NULL,
   [GraceDays] [int] NOT NULL CONSTRAINT [DF_Mc_Organization_GraceDays] DEFAULT ((0)),
   [Active] [bit] NOT NULL CONSTRAINT [DF_Mc_Organization_Active] DEFAULT ((1)),
   [Deleted] [bit] NOT NULL CONSTRAINT [DF_Mc_Organization_Deleted] DEFAULT ((0))
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   INSERT INTO [dbo].[tmp_Mc_Organization] ([OrganizationId], [Name], [Description], [WebsiteUrl], [DatabaseId], [FiscalYearStartDay], [FiscalYearStartMonth], [WeekStartsDay], [LdapServerAddress], [LdapServerPort], [LdapDomain], [LdapUserName], [LdapPassword], [ExpirationTime], [Active], [Deleted])
   SELECT [OrganizationId], [Name], [Description], [WebsiteUrl], [DatabaseId], [FiscalYearStartDay], [FiscalYearStartMonth], [WeekStartsDay], [LdapServerAddress], [LdapServerPort], [LdapDomain], [LdapUserName], [LdapPassword], NULL, 1, [Deleted]
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
   ALTER TABLE [dbo].[Mc_Organization] ADD CONSTRAINT [PK_Mc_Organization] PRIMARY KEY CLUSTERED ([OrganizationId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizations]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT OrganizationId, [Name], [Description], WebsiteUrl, DatabaseId
		, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword
		, ExpirationTime, GraceDays, Active, Deleted
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

	SELECT o.OrganizationId, o.[Name], o.[Description], o.WebsiteUrl, o.DatabaseId
		, o.FiscalYearStartMonth, o.FiscalYearStartDay, o.WeekStartsDay, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword
		, o.ExpirationTime, o.GraceDays, o.Active, o.Deleted 
	FROM dbo.Mc_OrganizationsLogins AS ol
	INNER JOIN dbo.Mc_Organization AS o
		ON	(ol.OrganizationId = o.OrganizationId) AND (ol.LoginId = @LoginId) AND (o.Deleted = 0) AND (o.Active = 1);
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
	@FiscalYearStartMonth int,
	@FiscalYearStartDay int,
	@WeekStartsDay int,
	@ExpirationTime datetime,
	@GraceDays int,
	@Active bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Organization (OrganizationId, [Name], [Description], WebsiteUrl, DatabaseId, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, ExpirationTime, GraceDays, Active, Deleted) 
	VALUES (@OrganizationId, @Name, @Description, @WebsiteUrl, @DatabaseId, @FiscalYearStartMonth, @FiscalYearStartDay, @WeekStartsDay, @ExpirationTime, @GraceDays, @Active, @Deleted);
	
	SELECT OrganizationId, [Name], [Description], WebsiteUrl, DatabaseId
		, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword
		, ExpirationTime, GraceDays, Active, Deleted 
	FROM dbo.Mc_Organization 
	WHERE (OrganizationId = @OrganizationId);
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
	@FiscalYearStartMonth int,  
	@FiscalYearStartDay int,  
	@WeekStartsDay int,  
	@LdapServerAddress nvarchar(255),  
	@LdapServerPort nvarchar(50),  
	@LdapDomain nvarchar(255),  
	@LdapUserName nvarchar(255),  
	@LdapPassword nvarchar(255),  
	@ExpirationTime datetime,
	@GraceDays int,
	@Active bit,
	@Deleted bit  
)  
AS  
BEGIN  
	SET NOCOUNT OFF;  

	UPDATE dbo.Mc_Organization   
	SET [Name] = @Name, [Description] = @Description, WebsiteUrl = @WebsiteUrl, DatabaseId = @DatabaseId
		, FiscalYearStartMonth = @FiscalYearStartMonth, FiscalYearStartDay = @FiscalYearStartDay, WeekStartsDay = @WeekStartsDay
		, LdapServerAddress = @LdapServerAddress, LdapServerPort = @LdapServerPort, LdapDomain = @LdapDomain, LdapUserName = @LdapUserName, LdapPassword = @LdapPassword
		, ExpirationTime = @ExpirationTime, GraceDays = @GraceDays, Active = @Active, Deleted = @Deleted
	WHERE (OrganizationId = @OrganizationId);

	SELECT OrganizationId, [Name], [Description], WebsiteUrl, DatabaseId
		, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword
		, ExpirationTime, GraceDays, Active, Deleted 
	FROM dbo.Mc_Organization   
	WHERE (OrganizationId = @OrganizationId);  
END')

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
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_CustomUrl_Mc_Organization')
      ALTER TABLE [dbo].[Mc_CustomUrl] ADD CONSTRAINT [FK_Mc_CustomUrl_Mc_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_InvitedLogin_Mc_Organization')
      ALTER TABLE [dbo].[Mc_InvitedLogin] ADD CONSTRAINT [FK_Mc_InvitedLogin_Mc_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_OrganizationsLogins_Mc_Organization')
      ALTER TABLE [dbo].[Mc_OrganizationsLogins] ADD CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
