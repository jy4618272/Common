BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Organization_Mc_Database')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [FK_Mc_Organization_Mc_Database]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Database_Mc_DatabaseServer')
      ALTER TABLE [dbo].[Mc_Database] DROP CONSTRAINT [FK_Mc_Database_Mc_DatabaseServer]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Database_DatabaseId')
      ALTER TABLE [dbo].[Mc_Database] DROP CONSTRAINT [DF_Mc_Database_DatabaseId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Database_Description')
      ALTER TABLE [dbo].[Mc_Database] DROP CONSTRAINT [DF_Mc_Database_Description]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Database_Deleted')
      ALTER TABLE [dbo].[Mc_Database] DROP CONSTRAINT [DF_Mc_Database_Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE TABLE [dbo].[tmp_Mc_Database] (
   [DatabaseId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Mc_Database_DatabaseId] DEFAULT (newid()),
   [Name] [nvarchar] (255) NOT NULL,
   [Description] [nvarchar] (1024) NOT NULL CONSTRAINT [DF_Mc_Database_Description] DEFAULT (N''),
   [UserName] [nvarchar] (255) NOT NULL,
   [Password] [nvarchar] (255) NOT NULL,
   [DatabaseServerId] [uniqueidentifier] NOT NULL,
   [Private] [bit] NOT NULL CONSTRAINT [DF_Mc_Database_Private] DEFAULT ((0)),
   [Deleted] [bit] NOT NULL CONSTRAINT [DF_Mc_Database_Deleted] DEFAULT ((0))
) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   INSERT INTO [dbo].[tmp_Mc_Database] ([DatabaseId], [Name], [Description], [UserName], [Password], [DatabaseServerId], [Deleted])
   SELECT [DatabaseId], [Name], [Description], [UserName], [Password], [DatabaseServerId], [Deleted]
   FROM [dbo].[Mc_Database]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP TABLE [dbo].[Mc_Database]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC sp_rename N'[dbo].[tmp_Mc_Database]', N'Mc_Database'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Database] ADD CONSTRAINT [PK_Mc_Database] PRIMARY KEY CLUSTERED ([DatabaseId]) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_CustomUrl_Mc_Organization')
      ALTER TABLE [dbo].[Mc_CustomUrl] DROP CONSTRAINT [FK_Mc_CustomUrl_Mc_Organization]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EmailSuffix_Mc_Organization')
      ALTER TABLE [dbo].[Mc_EmailSuffix] DROP CONSTRAINT [FK_Mc_EmailSuffix_Mc_Organization]

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
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_PseudoId')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_PseudoId]

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
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_LdapDomains')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_LdapDomains]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_GraceDays')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_GraceDays]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_ExternalId')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_ExternalId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_Active')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Active]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_Trial')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Trial]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_Beta')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Beta]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_Deleted')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_BillingPlan')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_BillingPlan]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE TABLE [dbo].[tmp_Mc_Organization] (
   [OrganizationId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Mc_Organization_OrganizationId] DEFAULT (newid()),
   [PseudoId] [varchar] (6) NOT NULL CONSTRAINT [DF_Mc_Organization_PseudoId] DEFAULT (''),
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
   [LdapDomains] [nvarchar] (2048) NOT NULL CONSTRAINT [DF_Mc_Organization_LdapDomains] DEFAULT (N''),
   [ExpirationTime] [datetime] NULL,
   [GraceDays] [int] NOT NULL CONSTRAINT [DF_Mc_Organization_GraceDays] DEFAULT ((0)),
   [ExternalId] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_Organization_ExternalId] DEFAULT (N''),
   [Active] [bit] NOT NULL CONSTRAINT [DF_Mc_Organization_Active] DEFAULT ((1)),
   [CanceledTime] [datetime] NULL,
   [Trial] [bit] NOT NULL CONSTRAINT [DF_Mc_Organization_Trial] DEFAULT ((0)),
   [Beta] [bit] NOT NULL CONSTRAINT [DF_Mc_Organization_Beta] DEFAULT ((0)),
   [Deleted] [bit] NOT NULL CONSTRAINT [DF_Mc_Organization_Deleted] DEFAULT ((0)),
   [CreatedTime] [datetime] NULL,
   [BillingPlan] [tinyint] NOT NULL CONSTRAINT [DF_Mc_Organization_BillingPlan] DEFAULT ((0)),
   [Street] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_Organization_Street] DEFAULT (N''),
   [Street2] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_Organization_Street2] DEFAULT (N''),
   [City] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_Organization_City] DEFAULT (N''),
   [State] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_Organization_State] DEFAULT (N''),
   [PostalCode] [nvarchar] (20) NOT NULL CONSTRAINT [DF_Mc_Organization_PostalCode] DEFAULT (N''),
   [Country] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_Organization_Country] DEFAULT (N''),
   [Currency] [char] (3) NOT NULL CONSTRAINT [DF_Mc_Organization_Currency] DEFAULT ('')
) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   INSERT INTO [dbo].[tmp_Mc_Organization] ([OrganizationId], [PseudoId], [Name], [Description], [WebsiteUrl], [DatabaseId], [FiscalYearStartDay], [FiscalYearStartMonth], [WeekStartsDay], [LdapServerAddress], [LdapServerPort], [LdapDomain], [LdapUserName], [LdapPassword], [LdapDomains], [ExpirationTime], [GraceDays], [ExternalId], [Active], [CanceledTime], [Trial], [Beta], [Deleted], [CreatedTime], [BillingPlan])
   SELECT [OrganizationId], [PseudoId], [Name], [Description], [WebsiteUrl], [DatabaseId], [FiscalYearStartDay], [FiscalYearStartMonth], [WeekStartsDay], [LdapServerAddress], [LdapServerPort], [LdapDomain], [LdapUserName], [LdapPassword], [LdapDomains], [ExpirationTime], [GraceDays], [ExternalId], [Active], [CanceledTime], [Trial], [Beta], [Deleted], [CreatedTime], [BillingPlan]
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
   ALTER TABLE [dbo].[Mc_Organization] ADD CONSTRAINT [PK_Mc_Organization] PRIMARY KEY CLUSTERED ([OrganizationId]) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_Mc_Organization_PseudoId] ON [dbo].[Mc_Organization] ([PseudoId]) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec('ALTER PROCEDURE [dbo].[Mc_GetDatabases]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT DatabaseId, [Name], [Description], UserName, [Password], DatabaseServerId, [Private], Deleted 
	FROM dbo.Mc_Database
	WHERE (Deleted = 0);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec('ALTER PROCEDURE [dbo].[Mc_GetOrganizations]  
AS  
BEGIN  
 SET NOCOUNT OFF;  
  
 SELECT OrganizationId, PseudoId, [Name], [Description], WebsiteUrl, DatabaseId  
  , FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains  
  , ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, BillingPlan
  , Street, Street2, City, [State], PostalCode, Country, Currency
 FROM dbo.Mc_Organization   
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec('ALTER PROCEDURE [dbo].[Mc_GetOrganizationsByLdapDomain]
(
	@LdapDomain nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT o.OrganizationId, o.PseudoId, o.[Name], o.[Description], o.WebsiteUrl, o.DatabaseId
		, o.FiscalYearStartMonth, o.FiscalYearStartDay, o.WeekStartsDay, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword, o.LdapDomains
		, o.ExpirationTime, o.GraceDays, o.ExternalId, o.Active, o.CanceledTime, o.Trial, o.Beta, o.Deleted, o.CreatedTime, o.BillingPlan
		, o.Street, o.Street2, o.City, o.[State], o.PostalCode, o.Country, o.Currency
	FROM dbo.Mc_Organization AS o
	WHERE (o.Deleted = 0) AND (o.Active = 1) AND ('','' + o.LdapDomains + '','' like ''%,'' + @LdapDomain + '',%'')
	ORDER BY o.LdapServerAddress DESC;
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec('ALTER PROCEDURE [dbo].[Mc_GetOrganizationsByLoginId]
(  
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT o.OrganizationId, o.PseudoId, o.[Name], o.[Description], o.WebsiteUrl, o.DatabaseId
		, o.FiscalYearStartMonth, o.FiscalYearStartDay, o.WeekStartsDay, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword, o.LdapDomains
		, o.ExpirationTime, o.GraceDays, o.ExternalId, o.Active, o.CanceledTime, o.Trial, o.Beta, o.Deleted, o.CreatedTime, o.BillingPlan
		, o.Street, o.Street2, o.City, o.[State], o.PostalCode, o.Country, o.Currency
	FROM dbo.Mc_OrganizationsLogins AS ol
	INNER JOIN dbo.Mc_Organization AS o
		ON (ol.OrganizationId = o.OrganizationId) AND (ol.LoginId = @LoginId) AND (o.Deleted = 0) AND (o.Active = 1) AND (ol.Active = 1);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec('ALTER PROCEDURE [dbo].[Mc_InsertDatabase]
(
	@DatabaseId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@UserName nvarchar(255),
	@Password nvarchar(255),
	@DatabaseServerId uniqueidentifier,
	@Private bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Database (DatabaseId, [Name], [Description], UserName, [Password], DatabaseServerId, [Private], Deleted) 
	VALUES (@DatabaseId, @Name, @Description, @UserName, @Password, @DatabaseServerId, @Private, @Deleted);
	
	SELECT DatabaseId, [Name], [Description], UserName, [Password], DatabaseServerId, [Private], Deleted 
	FROM dbo.Mc_Database 
	WHERE (DatabaseId = @DatabaseId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec('ALTER PROCEDURE [dbo].[Mc_InsertOrganization]
(
	@OrganizationId uniqueidentifier,
	@PseudoId varchar(6),
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
	@CanceledTime datetime,
	@Trial bit,
	@Deleted bit,
	@BillingPlan tinyint,
	@Street nvarchar(255), 
	@Street2 nvarchar(255), 
	@City nvarchar(255), 
	@State nvarchar(255), 
	@PostalCode nvarchar(20), 
	@Country nvarchar(255), 
	@Currency  char(3)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Organization (OrganizationId, PseudoId, [Name], [Description], WebsiteUrl, DatabaseId
		, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, ExpirationTime, GraceDays, Active, CanceledTime, Trial, Deleted, CreatedTime, BillingPlan
		, Street, Street2, City, [State], PostalCode, Country, Currency) 
	VALUES (@OrganizationId, @PseudoId, @Name, @Description, @WebsiteUrl, @DatabaseId
		, @FiscalYearStartMonth, @FiscalYearStartDay, @WeekStartsDay, @ExpirationTime, @GraceDays, @Active, @CanceledTime, @Trial, @Deleted, GETDATE(), @BillingPlan
		, @Street, @Street2, @City, @State, @PostalCode, @Country, @Currency);
	
	SELECT OrganizationId, PseudoId, [Name], [Description], WebsiteUrl, DatabaseId
		, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword
		, ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Deleted, CreatedTime, BillingPlan 
		, Street, Street2, City, [State], PostalCode, Country, Currency
	FROM dbo.Mc_Organization 
	WHERE (OrganizationId = @OrganizationId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec('ALTER PROCEDURE [dbo].[Mc_UpdateDatabase]
(
	@DatabaseId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@UserName nvarchar(255),
	@Password nvarchar(255),
	@DatabaseServerId uniqueidentifier,
	@Private bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Database 
	SET [Name] = @Name, [Description] = @Description, UserName = @UserName, [Password] = @Password, DatabaseServerId = @DatabaseServerId, [Private] = @Private, Deleted = @Deleted 
	WHERE ([DatabaseId] = @DatabaseId);
	
	SELECT DatabaseId, [Name], [Description], UserName, [Password], DatabaseServerId, [Private], Deleted 
	FROM dbo.Mc_Database 
	WHERE (DatabaseId = @DatabaseId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec('ALTER PROCEDURE [dbo].[Mc_UpdateOrganization]
(
	@OrganizationId uniqueidentifier,
	@PseudoId varchar(6),
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
	@LdapDomains nvarchar(2048),
	@ExpirationTime datetime,
	@GraceDays int,
	@Active bit,
	@CanceledTime datetime,
	@Trial bit,
	@Beta bit,
	@Deleted bit,
	@BillingPlan tinyint,
	@Street nvarchar(255), 
	@Street2 nvarchar(255), 
	@City nvarchar(255), 
	@State nvarchar(255), 
	@PostalCode nvarchar(20), 
	@Country nvarchar(255), 
	@Currency  char(3)
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Organization
	SET PseudoId  = @PseudoId, [Name] = @Name, [Description] = @Description, WebsiteUrl = @WebsiteUrl, DatabaseId = @DatabaseId
		, FiscalYearStartMonth = @FiscalYearStartMonth, FiscalYearStartDay = @FiscalYearStartDay, WeekStartsDay = @WeekStartsDay
		, LdapServerAddress = @LdapServerAddress, LdapServerPort = @LdapServerPort, LdapDomain = @LdapDomain, LdapUserName = @LdapUserName, LdapPassword = @LdapPassword, LdapDomains = @LdapDomains
		, ExpirationTime = @ExpirationTime, GraceDays = @GraceDays, Active = @Active, CanceledTime = @CanceledTime, Trial = @Trial, Beta = @Beta, Deleted = @Deleted, BillingPlan = @BillingPlan
		, @Street = Street, @Street2 = Street2, @City = City, @State = [State], @PostalCode = PostalCode, @Country = Country, @Currency = Currency
	WHERE (OrganizationId = @OrganizationId);

	SELECT OrganizationId, PseudoId, [Name], [Description], WebsiteUrl, DatabaseId
		, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains
		, ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, BillingPlan
		, Street, Street2, City, [State], PostalCode, Country, Currency
	FROM dbo.Mc_Organization
	WHERE (OrganizationId = @OrganizationId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Database_Mc_DatabaseServer')
      ALTER TABLE [dbo].[Mc_Database] DROP CONSTRAINT [FK_Mc_Database_Mc_DatabaseServer]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Database_Mc_DatabaseServer')
      ALTER TABLE [dbo].[Mc_Database] ADD CONSTRAINT [FK_Mc_Database_Mc_DatabaseServer] FOREIGN KEY ([DatabaseServerId]) REFERENCES [dbo].[Mc_DatabaseServer] ([DatabaseServerId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_Organization_Mc_Database')
      ALTER TABLE [dbo].[Mc_Organization] ADD CONSTRAINT [FK_Mc_Organization_Mc_Database] FOREIGN KEY ([DatabaseId]) REFERENCES [dbo].[Mc_Database] ([DatabaseId])

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
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EmailSuffix_Mc_Organization')
      ALTER TABLE [dbo].[Mc_EmailSuffix] ADD CONSTRAINT [FK_Mc_EmailSuffix_Mc_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])

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
	DELETE FROM [dbo].[Mc_Country] 

IF @@ERROR <> 0
  IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{00A20781-E4F4-4D01-9BD7-0063C9322D84}', N'Barbados')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{00E3B9E4-5DF2-4F2D-A814-77B01920A614}', N'Argentina')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{02E55972-E0C9-481C-9C12-190F4E8D7AC9}', N'Comoros')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{053C3314-CA71-4E35-B60A-9A1F6C1D6E57}', N'Svalbard')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{05733537-52DA-4FE7-ACDD-41F9C1856846}', N'Antarctica')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{05925325-6728-4F36-906D-32E2D697DA12}', N'United Arab Emirates')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{05DA2C16-A37D-4EE9-9A60-176DB0D42A02}', N'Saint Pierre and Miquelon')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{06274FF6-6171-4728-AC31-4E6208E784CF}', N'Lesotho')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{0ABE6123-F8F7-4B66-B207-51E8C8C633C3}', N'Mali')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{0C2CE2F4-253F-4F93-8760-57A8C4932F2F}', N'Laos')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{0C6F052F-0F0F-489B-AAC8-9D2145D9D2D7}', N'Algeria')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{0E30C1CF-8EA7-435F-9CF6-38F1A002FDCD}', N'Nicaragua')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{0EB07BEA-7F47-4440-B281-1DDCAC20B918}', N'Jersey')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{0FE5E4B0-E4B1-48B5-9DE3-51C6072737A0}', N'Azerbaijan')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{10312F7F-1AB9-4248-AB53-75E236421772}', N'Christmas Island')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{1051639B-5722-4772-995F-AFF39511A193}', N'Peru')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{10D6DDB2-9801-4A5E-BE74-4A8DD41A659B}', N'Faroe Islands')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{10E4B882-0809-43BF-B9E0-14A70C0FAF48}', N'El Salvador')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{123B283E-577F-4935-B712-91F87C776653}', N'Western Sahara')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{12AF54AB-33B3-4D2A-AA59-C57E73CC40CE}', N'Palau')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{14428415-8A67-4CB0-9311-A4A9E5708396}', N'Taiwan')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{14653234-DF62-4F58-AF75-651912A72B4E}', N'Kenya')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{1475DEA5-8C93-4355-BF06-BB5BEF0D9DF4}', N'Cook Islands')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{1606F3EC-8E7A-4FAA-9225-C17851249E74}', N'Tanzania')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{172117A0-49E2-42C8-A965-2EA1FF3AEFE9}', N'Malaysia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{179501DA-7CBE-4F15-AFE0-BEDC5FF9A0A6}', N'Cambodia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{18B25BCD-CD15-4947-8A25-9C3E8CF924A7}', N'Kiribati')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{18FBCA0F-5B3B-4F08-822E-63824CA01EAB}', N'Kyrgyzstan')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{1A730667-17C5-4AA0-95E8-3196163B26B5}', N'Antigua and Barbuda')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{1B23BC10-A621-4369-8200-D45B46415930}', N'Libya')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{1BA79C44-A57E-4A65-8FCA-F3462A8866CA}', N'Qatar')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{1CE42435-424B-4430-B560-9278049E7BD8}', N'Montserrat')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{1E54BEEC-390E-4A5D-9619-32D3F5E9F70A}', N'Austria')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{1E6CBF25-4620-4450-9ED0-8C5E1CE620E8}', N'Sierra Leone')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{1F3E7C15-1586-4716-A97F-AAD1B7EE2254}', N'Central African Republic')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{207CDD2E-37BF-460A-AF31-939D00872269}', N'Swaziland')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{212E2C82-263B-4109-847D-EA81B82A4715}', N'American Samoa')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{213FAE5A-ACB3-4D4C-8F13-E99423BF2A3E}', N'Samoa')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{23C70D62-C3E9-4D7E-B5A2-FA3388ABAB48}', N'British Indian Ocean Territory')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{2419D3A3-C350-4D37-A410-F39F844F5F22}', N'Gibraltar')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{26D10874-5DA6-4C54-BBE1-D775E4E8D9B3}', N'Zimbabwe')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{26F71B5E-EBA5-46DF-A6B5-5F1F153A8A12}', N'Angola')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{278E7D91-D960-4469-89F2-98B7DCE9CB1A}', N'Sudan')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{28692FD4-0F45-461C-B502-EEDDB136DC6E}', N'Liberia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{28A52BC2-71E5-4051-9EA3-0CEE3D761221}', N'Romania')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{297ED3A8-7F50-420C-A380-0F23B23BC5CD}', N'Tajikistan')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{2AA01526-FF51-4DFD-8BD5-F9B2F9014D4E}', N'Saint Lucia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{2AC87B5B-9A84-404D-A1FF-5EF00281A0E2}', N'Aland Islands')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{2D9BDC00-6721-4346-97C8-09112D67911F}', N'Egypt')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{2E9B5D6F-6289-4800-A76C-8A6BCD3F35DF}', N'Cape Verde')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{3019238E-8FB5-4B93-9EB3-470E296C433C}', N'Saint Helena')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{31267645-2AB4-4DCE-9419-EF9429912F0F}', N'Albania')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{335C25D1-74C8-43E3-A0B9-80692D6E1F08}', N'Niue')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{33AE6C35-CA62-4180-AA6F-560F37CFC636}', N'Armenia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{33F57D53-928D-4A49-BA2D-B68681FD3BF4}', N'Nigeria')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{36786B39-6D7F-438C-BA26-47AAE452F67F}', N'Mayotte')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{37E49A16-646F-4AE3-B6CB-7E847ECFC3EE}', N'Greenland')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{3946CCE6-F1A2-4854-9CD8-C6ED58BEE320}', N'Singapore')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{397E3DC5-1A56-4900-9930-4938489E0F1D}', N'Japan')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{39DE64E2-7292-4A36-B3C3-B89DCD727CC0}', N'Switzerland')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{3A4CBB0F-A8EB-4F5C-94C2-6460A1D4BC1C}', N'Mexico')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{3B4A6E98-7B56-4BA5-81B4-E93BCDBA9701}', N'Saint Kitts and Nevis')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{3E223922-5114-4658-8A47-700671035531}', N'Greece')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{3E55C055-1F9D-476E-9D62-0F687F8D7B75}', N'Germany')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{3E92B569-64C0-43F0-8D26-4CE50CF18D19}', N'Brazil')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{3FFE94CA-0A95-4E31-BF75-164F07BCC54F}', N'Tuvalu')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{4187AF65-EB5D-4BA4-85F9-4B8926E51B64}', N'New Caledonia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{42E71867-B146-4400-ADBD-E3A6A0527979}', N'Tonga')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{436F68BD-3E6F-48FD-AD26-CECB8AA6A3C3}', N'Belgium')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{445191EC-1B21-44E1-93F9-EC65BEAD1F9D}', N'Hong Kong')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{44E5B1FB-BA23-4BB7-BA58-89E287E4E345}', N'Ivory Coast')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{4503C26D-52B8-4B24-B325-F81DB37211A6}', N'The Gambia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{4600C58E-17F1-4C8B-B688-19089F47D7F9}', N'Maldives')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{46999B08-4235-48FD-AC2D-90E5E716522B}', N'Anguilla')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{4722741A-87C8-49B7-B586-05B613C7DA34}', N'Zambia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{47266E58-E8D5-4D1A-80A0-3BFDE90A4FC3}', N'Netherlands')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{496FEAB0-639B-4E44-BA8D-589880BDF4A0}', N'Cameroon')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{49CC2092-B872-4433-8E97-472256C7BBDD}', N'Namibia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{4A3917C0-4CF5-49B1-8AC1-5097325C29DC}', N'Spain')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{4A57B12F-38E3-4D0B-8476-E82FDA4A6164}', N'Papua New Guinea')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{50AEE2DA-86B3-46F4-BE34-CA73F3557C15}', N'British Virgin Islands')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{522E5354-6C7A-40C8-991A-0517CD5D8DED}', N'Grenada')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{5257828A-E6E1-4598-B120-7639EF14C3F2}', N'Hungary')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{529EA037-8C12-412A-AE14-A2FD311B8CB2}', N'Wallis and Futuna')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{536B12D8-C53E-4554-AB2E-BE4AE28EAEF5}', N'Northern Mariana Islands')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{5427874B-9C57-4B42-BFEB-79C0BA2D5DEC}', N'Pakistan')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{551BE92F-14FE-4BC1-B8A2-2A456D2AE435}', N'Sri Lanka')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{57C56AE3-73DD-431D-9D3D-FBAF59C31F98}', N'Vanuatu')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{583D3249-0638-4033-B2AB-D3AFF650379A}', N'Bahrain')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{5B0E2AFE-70F7-4F1B-A691-BCD5148C9319}', N'Togo')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{5B1F1512-195B-474D-B671-E913184B4D5D}', N'Sweden')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{5C26F507-1B37-43A2-80B9-79DD5BF58AB6}', N'French Guiana')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{5C2ECF0F-63CF-4050-9304-DBAF20BD3F73}', N'Vietnam')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{5CBAD7F8-E612-432A-96E4-C33D0FFB4C75}', N'Eritrea')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{5F9937AD-F30F-4FEE-9CC3-4D2C7BC69561}', N'Burkina Faso')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{61378651-BD83-48D0-BDF3-0F18B796B6A8}', N'Bolivia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{62E36F88-01B7-4CB8-AE50-2EBC54DB5946}', N'Norway')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{640D891A-7630-4C56-B049-FE42CB4E6CB6}', N'Finland')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{653F0313-0C63-4094-8E87-92869D322A32}', N'Trinidad and Tobago')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{65D834A7-A45C-4B69-990E-FB4522C18B0E}', N'Cayman Islands')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{66FA026F-0C4D-4CE1-AE00-AFBCEC995958}', N'Bhutan')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{6835F73B-500D-4A83-8174-EB222BCF6EB8}', N'Niger')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{684C9607-240F-4AAE-B0C6-E475FA2C6F84}', N'South Africa')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{6977252F-FA0A-4D79-9D36-AFC2DD43F8FE}', N'Mongolia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{69BCEE6C-970D-415E-866C-55DD5160A8A2}', N'Denmark')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{6A3E9698-C955-4E71-9935-6ECE207DDE1D}', N'Ethiopia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{6B36B3D3-A97A-4229-AA30-32C16AD96D0F}', N'Uruguay')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{6B7341B9-6640-4847-BEF8-8335CFC2CCEF}', N'Benin')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{6C6E57F4-FE0E-4F3E-80D2-4836E87520F1}', N'Saint Barthelemy')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{6DBD4D86-47D1-4193-8951-259C1D17E11A}', N'French Southern and Antarctic Lands')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{6DCAA5F8-354C-471E-9A6C-42AB2D8D0AB2}', N'French Polynesia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{6E05FD63-7327-4F05-945B-FDCD2388C1E3}', N'Senegal')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{6E236281-FA94-4C0B-A0EE-4B5211D81C1A}', N'Lebanon')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{702907DC-F8A6-4410-9375-D05C402149AA}', N'Australia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{70A5E9D8-5728-40D8-97F9-94FCE2AE6E44}', N'Israel')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{70A75DBA-7383-465B-B4BE-1AA294B766B6}', N'United Kingdom')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{70C261B9-2D71-4106-A18A-C65C843A6783}', N'Guinea-Bissau')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{72F679B2-C3B5-4458-8C7F-70925FA1774B}', N'Saint Vincent and the Grenadines')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{733CE015-F434-4669-9B1C-C3F3FB456B84}', N'Chile')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{745E5B44-6344-47E2-8577-D65B0E962815}', N'Italy')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{74EA94A0-6052-40E1-8780-9196E37CEE73}', N'Indonesia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{76887731-2FA0-4862-84F0-A1CBCAC6A0C3}', N'Kazakhstan')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{76EA5343-1DF6-4CE0-A3F3-D0D3C3EE71E0}', N'Bouvet Island')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{770F77B8-9F6B-4D7F-B969-30CA0ADB497C}', N'Mauritius')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{77CC2759-2A13-4DE7-8E77-A1B7E4413392}', N'Slovakia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{7948A043-D7B4-4FE5-B7F1-795F5BCB082B}', N'Norfolk Island')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{7B354E8D-45F3-43CC-8A22-CB30CA7A3465}', N'Bangladesh')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{7B499346-450F-4AF2-B37E-75C70749293A}', N'Guyana')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{7D442DD9-DD21-4624-B36A-4EAAB8D2B8CF}', N'Jordan')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{7EB94C9D-48C0-4A82-93D6-D34CF94BDA6A}', N'Palesinian Territory')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{7F3890F4-4F20-4D4B-9674-D8700FFD43F9}', N'Ecuador')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{7F837CD9-A584-4056-82DB-DF97E54C8D04}', N'Bosnia and Herzegovina')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{83109EA6-C8AC-4F8D-85DB-5D8360F09EE3}', N'Ghana')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{83290294-5B64-43E5-A339-2335B235F73F}', N'Turks and Caicos Islands')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{834F9EED-1FB0-4B0A-A60E-A592870A628B}', N'Uzbekistan')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{83D494DE-B27F-4082-91DA-B55CB011F513}', N'Netherlands Antilles')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{89FDE730-5B24-469A-992C-8A68083DC96D}', N'Madagascar')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{8A4CEF38-14E3-455A-9750-43AF6E177A26}', N'San Marino')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{8B9C0EFC-6DA8-428B-9B10-80EC90E14476}', N'Democratic Republic of the Congo')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{8CABA4E0-48F2-41E7-B7F6-FFCEB0B534B6}', N'Guernsey')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{8CC7CF4A-BF3E-4467-9F2C-099D50A9F804}', N'Guam')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{8CCA018C-7D07-4E7A-9087-6FC49D1D53CB}', N'Nauru')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{8D90860C-49FC-421E-B404-CE29422FC6C0}', N'Ukraine')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{9013A2ED-C1CD-42E8-9B16-6F3ACEFFBAA4}', N'Dominica')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{951C00F4-5535-465B-8704-0B1101282C51}', N'Croatia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{96FF765A-4740-42DC-81F3-F649C49686D8}', N'Gabon')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{97988A6D-F287-4B59-80D0-4E66520840F5}', N'Turkmenistan')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{97A8AAC9-B63D-4DD7-8693-741C2CC87362}', N'Cyprus')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{97CBAF1E-5CFD-49C5-B45E-4CD41430A0C7}', N'Mauritania')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{97FF08C0-0322-4C2C-B15A-F27FAC8DEC63}', N'Chad')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{982AA2F6-F358-4BA3-98F7-A41498E732CB}', N'Saint Martin')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{98D788ED-DD91-4536-A37B-E504682077BC}', N'Burma')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{9C19E832-C75D-4C42-83AC-4A0290E6A6B6}', N'Sao Tome and Principe')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{9D33784F-C2A2-4906-A0CC-4005F0C5717A}', N'India')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{9F0C3E61-5608-4B70-9AA4-1CB2FCAAD6BC}', N'Monaco')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{9FDEAD8A-7E8E-4CC9-81A7-7D4B703B50FE}', N'Puerto Rico')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{9FE3886E-F407-4545-AC02-74CD4E314DE5}', N'Seychelles')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{A192C516-B923-4C17-8421-E26444580DC0}', N'Iraq')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{A1EE51FC-51EA-41BD-9C37-408BC24D65E0}', N'Botswana')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{A203E78A-16D1-4511-B91D-E2A3A0711397}', N'France')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{A3357B7F-EB2B-4AFE-BD5F-BD8AD681976F}', N'Mozambique')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{A3E6AA89-7C2C-40BD-ADF9-FD0A8693D92E}', N'Morocco')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{A41F93AE-C9BF-4460-A188-CB879FF1E951}', N'Philippines')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{A5349A59-6358-4A37-B611-0B0744EDDA64}', N'Oman')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{A562C766-672F-42F3-B746-FAC5D64F1C34}', N'Dominican Republic')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{A57D08D5-BBCE-4DA5-B836-349C3A7B2C64}', N'Turkey')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{A5A66C0C-1F30-4801-9C1A-127DAECA4DF8}', N'Republic of the Congo')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{A89C082D-35CB-4029-8DAB-3E57E4CFE2A4}', N'Uganda')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{A99897AA-BEB3-49D4-8117-857A68D34863}', N'Equatorial Guinea')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{A9F6E43B-2962-4FD1-9EB8-CC899EE8E568}', N'Iceland')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{AD876C7F-5148-4211-BFE7-047792473EE0}', N'North Korea')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{ADA73BAD-190D-46DD-8C8F-AF8FC59C3DE1}', N'Montenegro')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{ADE3C13D-4531-4ABE-ADD7-433C1C4E1544}', N'Poland')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{AF0402EF-EF87-4FBA-8798-C14768CD92E7}', N'Burundi')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{AF2F67F5-3273-4C8C-B335-B9E5DDC819B3}', N'The Bahamas')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{AF852630-A37D-4BDB-9A85-4989A3F2C14C}', N'Guinea')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{B061D3AF-5D3B-4377-9563-06F4BD3AB0B8}', N'Slovenia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{B076FFF3-9D2A-4153-A8C8-87634C82760B}', N'Russian Federation')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{B07B3E40-BB87-444D-BC72-00FEFBA3D7A2}', N'Czech Republic')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{B1CFEC28-4790-4B37-A168-34C1E42ACAEA}', N'Afghanistan')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{B2A9428C-4AD8-4786-AD65-DD01B8EC6330}', N'Micronesia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{B39B3317-42FD-4453-973F-5D3EAC39FC7E}', N'Colombia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{B49FC740-E996-4271-BE92-55C86124F07B}', N'Macedonia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{B520BB53-9179-4B57-8E68-3211BA8B67DC}', N'Marshall Islands')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{B5756EF5-43A1-4E29-9DD3-B9F348225DBE}', N'Djibouti')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{B6807954-93D6-458C-96D7-18574745F02E}', N'Jamaica')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{B752B0B0-155C-4788-AED6-9BC773421F59}', N'Venezuela')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{BB1D35FF-7FFF-492C-A6DA-25501FBEEF0F}', N'Lithuania')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{BB55A2E0-5588-4D8E-97A3-FDB3505E3A4F}', N'Cuba')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{BF3F25A0-CCCF-45DA-B52F-5B64BC3E3084}', N'Nepal')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{C13164B9-4E37-48F3-A429-9F4BDEA86152}', N'United States Minor Outlying Islands')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{C269CE2E-6B5D-4480-932E-4CFC13E72E8B}', N'Georgia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{C7886148-EA80-4BF0-8AB6-3569F2940256}', N'Ireland')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{C7F1631E-4446-4ED2-B5E0-BEC094569DD1}', N'Suriname')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{C86645C9-09FF-4689-96B7-9BA9E2F775C1}', N'Liechtenstein')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{C87B6D35-2E08-4233-9ED4-3A514A12F813}', N'Guatemala')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{C936DC82-055D-470D-AE30-19E6EB494860}', N'Martinique')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{C9FB07AB-D33B-4855-ADE3-BE74831B67F0}', N'Pitcairn Islands')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{CCAAD038-4EF7-48C1-9135-AED58721ED0B}', N'Panama')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{CCD8524A-89D8-4C00-9A85-66F71AD287B4}', N'Estonia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{CF132F4D-10C8-4050-8E23-21E43A8B6530}', N'Solomon Islands')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{CF4F22E3-4AA0-4853-9C6C-A11C849965C6}', N'Kuwait')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{CF744FA5-3439-40E6-BAEE-16A1C2ACE710}', N'Portugal')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{D08BA29F-14F5-475F-B931-AEF5B5074B2E}', N'Aruba')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{D0CEBFBF-F92B-4A49-93F2-C9CA95B84258}', N'Syria')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{D158E7AD-0B47-4C57-8621-5A6DEF5092FE}', N'Serbia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{D1D12232-19AE-449D-A84D-9AAFFFD37F91}', N'New Zealand')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{D255B0C8-16D2-40D6-9261-95EF106BCF7F}', N'Moldova')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{D452F177-8963-4128-9ABB-DD938ED5D027}', N'Belarus')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{D499FD5C-888E-4D69-9C96-A1AC22D96D79}', N'Brunei')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{D68965C5-ED90-41A9-B83E-C30814E5ECC7}', N'Yemen')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{D797E331-FCBD-4FA1-AF42-534E89F2F23A}', N'Bermuda')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{D7A8F2E0-66CD-4F3E-99F6-F69317AA201B}', N'Falkland Islands')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{D8A7298E-4F02-45BA-9A97-4D8C42233583}', N'Luxembourg')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{DA3410A2-FE77-48B5-91B7-D98A118BE465}', N'Andorra')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{DA9B8DB2-FD7E-4994-941D-04A45EB7CFA6}', N'Paraguay')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{DAEBE51D-B5F1-4E3C-A1F3-9650E1D2D5D3}', N'Saudi Arabia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{DC08348F-6533-4496-B79A-07A7012646BF}', N'South Georgia and South Sandwich Islands')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{DD08498A-3D13-4DF9-A741-0A3E42AEA58C}', N'Cocos (Keeling) Islands')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{DD2082E4-E97D-4C2A-88C0-C9DDF860B1C8}', N'Malta')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{DEB5E206-C996-4B2B-BB9F-12BE9A50C21E}', N'Tunisia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{DEDF85B6-7F8E-450C-A722-99541164A9B5}', N'Rwanda')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{DF9DE1C1-A089-4BA4-B526-ABCF30330BDE}', N'Isle of Man')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{E34904A3-C271-4022-9A09-540E907319A4}', N'Vatican City')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{E3EB6F07-3694-4F9B-993E-951E446FFE26}', N'Guadeloupe')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{E523C6F9-867A-451A-8A32-58542122787D}', N'Haiti')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{E57C6353-77C1-48AC-91E3-03F1011ACBB6}', N'Virgin Islands')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{E7B3D67D-E634-4AEE-9BE5-8FA9CDA3177B}', N'Thailand')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{E7D04959-09F6-4A4E-AF69-66087A7236BC}', N'Heard Island and McDonald Islands')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{EAA61349-4C20-49F1-B955-B38B0A8E4A45}', N'Timor-Leste')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{EABE7034-DD2C-4142-B13C-9DF97397FC0A}', N'Reunion')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{EC671163-AAC6-40CA-A47A-04A3AFF7417F}', N'Belize')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{ED00983A-2219-4CDF-B9DF-49E6E9788CDF}', N'China')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{EE3DEB13-725B-4DFC-916E-4D7697EFE737}', N'Tokelau')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{F0648902-DB51-41ED-9FA8-11E52F6AE5F6}', N'Costa Rica')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{F0A214C3-662A-41ED-9CA8-120E4259F46B}', N'Macau')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{F0DFFEB4-99EA-4F79-9BA2-26DC935862C8}', N'Canada')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{F1F2A7A4-F2F8-4FBD-958E-FEE032037FFE}', N'United States of America')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{F447F2CE-A623-4A10-80C9-CFB989A76224}', N'Honduras')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{F49C84CC-D5B3-4E96-9303-2748F86FFD8C}', N'Bulgaria')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{F64035C8-9E4B-4F9D-81FC-462AD33DD118}', N'Iran')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{F8B7BF01-2707-417E-89A4-DE738AD60C65}', N'Malawi')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{F938E652-6E3F-48CF-96B4-4423158F2ABD}', N'Fiji')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{F9CDF019-7AB2-4C0A-A927-D99570ABD860}', N'Latvia')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{FA2E773E-201F-4F98-904E-D949B71269A4}', N'South Korea')
	INSERT INTO [dbo].[Mc_Country] ([CountryId], [Name]) VALUES ('{FB597CC6-C4A7-4A4C-AB28-7421D7D4DE05}', N'Somalia')

IF @@ERROR <> 0
  IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
