SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON

SET NUMERIC_ROUNDABORT OFF

ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Street]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Street2]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_City]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_State]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_PostalCode]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Country]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Currency]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_OrganizationId]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_PseudoId]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Description]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_WebsiteUrl]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_LdapServerAddress]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_LdapServerPort]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_LdapDomain]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_LdapUserName]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_LdapPassword]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_LdapDomains]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_GraceDays]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_ExternalId]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Active]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Trial]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Beta]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_Deleted]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_HowYouHearAboutUs]
ALTER TABLE [dbo].[Mc_CustomUrl] DROP CONSTRAINT [FK_Mc_CustomUrl_Mc_Organization]
ALTER TABLE [dbo].[Mc_EmailSuffix] DROP CONSTRAINT [FK_Mc_EmailSuffix_Mc_Organization]
ALTER TABLE [dbo].[Mc_InvitedLogin] DROP CONSTRAINT [FK_Mc_InvitedLogin_Mc_Organization]
ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [FK_Mc_Organization_Mc_Database]
ALTER TABLE [dbo].[Mc_OrganizationsLogins] DROP CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Organization]

BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE

SET XACT_ABORT ON

CREATE TABLE [dbo].[tmp_ms_xx_Mc_Organization] (
	[OrganizationId]       UNIQUEIDENTIFIER CONSTRAINT [DF_Mc_Organization_OrganizationId] DEFAULT (newid()) NOT NULL,
	[PseudoId]             VARCHAR (6)      CONSTRAINT [DF_Mc_Organization_PseudoId] DEFAULT ('') NOT NULL,
	[ParentOrganizationId] UNIQUEIDENTIFIER NULL,
	[Name]                 NVARCHAR (255)   NOT NULL,
	[Description]          NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_Description] DEFAULT (N'') NOT NULL,
	[WebsiteUrl]           NVARCHAR (2048)  CONSTRAINT [DF_Mc_Organization_WebsiteUrl] DEFAULT (N'') NOT NULL,
	[DatabaseId]           UNIQUEIDENTIFIER NULL,
	[FiscalYearStartDay]   INT              NULL,
	[FiscalYearStartMonth] INT              NULL,
	[WeekStartsDay]        INT              NULL,
	[LdapServerAddress]    NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_LdapServerAddress] DEFAULT (N'') NOT NULL,
	[LdapServerPort]       NVARCHAR (50)    CONSTRAINT [DF_Mc_Organization_LdapServerPort] DEFAULT (N'') NOT NULL,
	[LdapDomain]           NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_LdapDomain] DEFAULT (N'') NOT NULL,
	[LdapUserName]         NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_LdapUserName] DEFAULT (N'') NOT NULL,
	[LdapPassword]         NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_LdapPassword] DEFAULT (N'') NOT NULL,
	[LdapDomains]          NVARCHAR (2048)  CONSTRAINT [DF_Mc_Organization_LdapDomains] DEFAULT (N'') NOT NULL,
	[ExpirationTime]       DATETIME         NULL,
	[GraceDays]            INT              CONSTRAINT [DF_Mc_Organization_GraceDays] DEFAULT ((0)) NOT NULL,
	[ExternalId]           NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_ExternalId] DEFAULT (N'') NOT NULL,
	[Active]               BIT              CONSTRAINT [DF_Mc_Organization_Active] DEFAULT ((1)) NOT NULL,
	[CanceledTime]         DATETIME         NULL,
	[Trial]                BIT              CONSTRAINT [DF_Mc_Organization_Trial] DEFAULT ((0)) NOT NULL,
	[Beta]                 BIT              CONSTRAINT [DF_Mc_Organization_Beta] DEFAULT ((0)) NOT NULL,
	[Deleted]              BIT              CONSTRAINT [DF_Mc_Organization_Deleted] DEFAULT ((0)) NOT NULL,
	[CreatedTime]          DATETIME         NULL,
	[Street]               NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_Street] DEFAULT (N'') NOT NULL,
	[Street2]              NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_Street2] DEFAULT (N'') NOT NULL,
	[City]                 NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_City] DEFAULT (N'') NOT NULL,
	[State]                NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_State] DEFAULT (N'') NOT NULL,
	[PostalCode]           NVARCHAR (20)    CONSTRAINT [DF_Mc_Organization_PostalCode] DEFAULT (N'') NOT NULL,
	[Country]              NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_Country] DEFAULT (N'') NOT NULL,
	[Currency]             CHAR (3)         CONSTRAINT [DF_Mc_Organization_Currency] DEFAULT ('') NOT NULL,
	[HowYouHearAboutUs]    NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_HowYouHearAboutUs] DEFAULT ('') NOT NULL,
	CONSTRAINT [tmp_ms_xx_constraint_PK_Mc_Organization] PRIMARY KEY CLUSTERED ([OrganizationId] ASC)
)

IF EXISTS (SELECT TOP 1 1 
		   FROM   [dbo].[Mc_Organization])
	BEGIN
		INSERT INTO [dbo].[tmp_ms_xx_Mc_Organization] ([OrganizationId], [PseudoId], [Name], [Description], [WebsiteUrl], [DatabaseId], [FiscalYearStartDay], [FiscalYearStartMonth], [WeekStartsDay], [LdapServerAddress], [LdapServerPort], [LdapDomain], [LdapUserName], [LdapPassword], [LdapDomains], [ExpirationTime], [GraceDays], [ExternalId], [Active], [CanceledTime], [Trial], [Beta], [Deleted], [CreatedTime], [Street], [Street2], [City], [State], [PostalCode], [Country], [Currency], [HowYouHearAboutUs])
		SELECT   [OrganizationId],
				 [PseudoId],
				 [Name],
				 [Description],
				 [WebsiteUrl],
				 [DatabaseId],
				 [FiscalYearStartDay],
				 [FiscalYearStartMonth],
				 [WeekStartsDay],
				 [LdapServerAddress],
				 [LdapServerPort],
				 [LdapDomain],
				 [LdapUserName],
				 [LdapPassword],
				 [LdapDomains],
				 [ExpirationTime],
				 [GraceDays],
				 [ExternalId],
				 [Active],
				 [CanceledTime],
				 [Trial],
				 [Beta],
				 [Deleted],
				 [CreatedTime],
				 [Street],
				 [Street2],
				 [City],
				 [State],
				 [PostalCode],
				 [Country],
				 [Currency],
				 [HowYouHearAboutUs]
		FROM     [dbo].[Mc_Organization]
		ORDER BY [OrganizationId] ASC
	END

DROP TABLE [dbo].[Mc_Organization]

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Mc_Organization]', N'Mc_Organization'

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Mc_Organization]', N'PK_Mc_Organization', N'OBJECT'

COMMIT TRANSACTION

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

CREATE NONCLUSTERED INDEX [IX_Mc_Organization_PseudoId]
	ON [dbo].[Mc_Organization]([PseudoId] ASC)
CREATE NONCLUSTERED INDEX [IX_Mc_Organization_ParentOrganizationId]
	ON [dbo].[Mc_Organization]([ParentOrganizationId] ASC)
ALTER TABLE [dbo].[Mc_CustomUrl] WITH NOCHECK
	ADD CONSTRAINT [FK_Mc_CustomUrl_Mc_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])
ALTER TABLE [dbo].[Mc_EmailSuffix] WITH NOCHECK
	ADD CONSTRAINT [FK_Mc_EmailSuffix_Mc_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])
ALTER TABLE [dbo].[Mc_InvitedLogin] WITH NOCHECK
	ADD CONSTRAINT [FK_Mc_InvitedLogin_Mc_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])
ALTER TABLE [dbo].[Mc_Organization] WITH NOCHECK
	ADD CONSTRAINT [FK_Mc_Organization_Mc_Database] FOREIGN KEY ([DatabaseId]) REFERENCES [dbo].[Mc_Database] ([DatabaseId])
ALTER TABLE [dbo].[Mc_OrganizationsLogins] WITH NOCHECK
	ADD CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])
ALTER TABLE [dbo].[Mc_Organization] WITH NOCHECK
	ADD CONSTRAINT [FK_Mc_Organization_Mc_Organization] FOREIGN KEY ([ParentOrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])

EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizations]  
	AS  
	BEGIN  
	 SET NOCOUNT OFF;  
	  
	 SELECT OrganizationId, PseudoId, ParentOrganizationId, [Name], [Description], WebsiteUrl, DatabaseId  
	  , FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains  
	  , ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime
	  , Street, Street2, City, [State], PostalCode, Country, Currency, HowYouHearAboutUs
	 FROM dbo.Mc_Organization   
	END')

EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizationsByLdapDomain]
	(
		@LdapDomain nvarchar(255)
	)
	AS
	BEGIN
		SET NOCOUNT OFF;

		SELECT o.OrganizationId, o.PseudoId, o.ParentOrganizationId, o.[Name], o.[Description], o.WebsiteUrl, o.DatabaseId
			, o.FiscalYearStartMonth, o.FiscalYearStartDay, o.WeekStartsDay, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword, o.LdapDomains
			, o.ExpirationTime, o.GraceDays, o.ExternalId, o.Active, o.CanceledTime, o.Trial, o.Beta, o.Deleted, o.CreatedTime
			, o.Street, o.Street2, o.City, o.[State], o.PostalCode, o.Country, o.Currency, o.HowYouHearAboutUs
		FROM dbo.Mc_Organization AS o
		WHERE (o.Deleted = 0) AND (o.Active = 1) AND (',' + o.LdapDomains + ',' like '%,' + @LdapDomain + ',%')
		ORDER BY o.LdapServerAddress DESC;
	END')

EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizationsByLoginId]
	(  
		@LoginId uniqueidentifier
	)
	AS
	BEGIN
		SET NOCOUNT OFF;

		SELECT o.OrganizationId, o.PseudoId, o.ParentOrganizationId, o.[Name], o.[Description], o.WebsiteUrl, o.DatabaseId
			, o.FiscalYearStartMonth, o.FiscalYearStartDay, o.WeekStartsDay, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword, o.LdapDomains
			, o.ExpirationTime, o.GraceDays, o.ExternalId, o.Active, o.CanceledTime, o.Trial, o.Beta, o.Deleted, o.CreatedTime
			, o.Street, o.Street2, o.City, o.[State], o.PostalCode, o.Country, o.Currency, o.HowYouHearAboutUs
		FROM dbo.Mc_OrganizationsLogins AS ol
		INNER JOIN dbo.Mc_Organization AS o
			ON (ol.OrganizationId = o.OrganizationId) AND (ol.LoginId = @LoginId) AND (o.Deleted = 0) AND (o.Active = 1) AND (ol.Active = 1);
	END')

EXEC('ALTER PROCEDURE [dbo].[Mc_InsertOrganization]
	(
		@OrganizationId uniqueidentifier,
		@PseudoId varchar(6),
		@ParentOrganizationId uniqueidentifier,
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
		@Street nvarchar(255), 
		@Street2 nvarchar(255), 
		@City nvarchar(255), 
		@State nvarchar(255), 
		@PostalCode nvarchar(20), 
		@Country nvarchar(255), 
		@Currency char(3),
		@HowYouHearAboutUs nvarchar(255)
	)
	AS
	BEGIN
		SET NOCOUNT OFF;

		INSERT INTO dbo.Mc_Organization (OrganizationId, PseudoId, ParentOrganizationId, [Name], [Description], WebsiteUrl, DatabaseId
			, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, ExpirationTime, GraceDays, Active, CanceledTime, Trial, Deleted, CreatedTime
			, Street, Street2, City, [State], PostalCode, Country, Currency, HowYouHearAboutUs) 
		VALUES (@OrganizationId, @PseudoId, @ParentOrganizationId, @Name, @Description, @WebsiteUrl, @DatabaseId
			, @FiscalYearStartMonth, @FiscalYearStartDay, @WeekStartsDay, @ExpirationTime, @GraceDays, @Active, @CanceledTime, @Trial, @Deleted, GETUTCDATE()
			, @Street, @Street2, @City, @State, @PostalCode, @Country, @Currency, @HowYouHearAboutUs);
		
		SELECT OrganizationId, PseudoId, ParentOrganizationId, [Name], [Description], WebsiteUrl, DatabaseId
			, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword
			, ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Deleted, CreatedTime 
			, Street, Street2, City, [State], PostalCode, Country, Currency, HowYouHearAboutUs
		FROM dbo.Mc_Organization 
		WHERE (OrganizationId = @OrganizationId);
	END')

EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateOrganization]
	(
		@OrganizationId uniqueidentifier,
		@PseudoId varchar(6),
		@ParentOrganizationId uniqueidentifier,
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
		@Street nvarchar(255), 
		@Street2 nvarchar(255), 
		@City nvarchar(255), 
		@State nvarchar(255), 
		@PostalCode nvarchar(20), 
		@Country nvarchar(255), 
		@Currency char(3),
		@HowYouHearAboutUs nvarchar(255)
	)
	AS
	BEGIN
		SET NOCOUNT OFF;

		UPDATE dbo.Mc_Organization
		SET PseudoId  = @PseudoId, ParentOrganizationId=@ParentOrganizationId, [Name] = @Name, [Description] = @Description, WebsiteUrl = @WebsiteUrl, DatabaseId = @DatabaseId
			, FiscalYearStartMonth = @FiscalYearStartMonth, FiscalYearStartDay = @FiscalYearStartDay, WeekStartsDay = @WeekStartsDay
			, LdapServerAddress = @LdapServerAddress, LdapServerPort = @LdapServerPort, LdapDomain = @LdapDomain, LdapUserName = @LdapUserName, LdapPassword = @LdapPassword, LdapDomains = @LdapDomains
			, ExpirationTime = @ExpirationTime, GraceDays = @GraceDays, Active = @Active, CanceledTime = @CanceledTime, Trial = @Trial, Beta = @Beta, Deleted = @Deleted
			, Street = @Street, Street2 = @Street2, City = @City, [State] = @State, PostalCode = @PostalCode, Country = @Country, Currency = @Currency, HowYouHearAboutUs = @HowYouHearAboutUs
		WHERE (OrganizationId = @OrganizationId);

		SELECT OrganizationId, PseudoId, ParentOrganizationId, [Name], [Description], WebsiteUrl, DatabaseId
			, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains
			, ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime
			, Street, Street2, City, [State], PostalCode, Country, Currency, HowYouHearAboutUs
		FROM dbo.Mc_Organization
		WHERE (OrganizationId = @OrganizationId);
	END')
