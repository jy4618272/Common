BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Organization_HowYouHearAboutUs')
      ALTER TABLE [dbo].[Mc_Organization] DROP CONSTRAINT [DF_Mc_Organization_HowYouHearAboutUs]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization]
		ADD [HowYouHearAboutUs] NVARCHAR (255) CONSTRAINT [DF_Mc_Organization_HowYouHearAboutUs] DEFAULT ('') NOT NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizations]  
	AS  
	BEGIN  
	 SET NOCOUNT OFF;  
	  
	 SELECT OrganizationId, PseudoId, [Name], [Description], WebsiteUrl, DatabaseId  
	  , FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains  
	  , ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime
	  , Street, Street2, City, [State], PostalCode, Country, Currency, HowYouHearAboutUs
	 FROM dbo.Mc_Organization   
	END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizationsByLdapDomain]
	(
		@LdapDomain nvarchar(255)
	)
	AS
	BEGIN
		SET NOCOUNT OFF;

		SELECT o.OrganizationId, o.PseudoId, o.[Name], o.[Description], o.WebsiteUrl, o.DatabaseId
			, o.FiscalYearStartMonth, o.FiscalYearStartDay, o.WeekStartsDay, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword, o.LdapDomains
			, o.ExpirationTime, o.GraceDays, o.ExternalId, o.Active, o.CanceledTime, o.Trial, o.Beta, o.Deleted, o.CreatedTime
			, o.Street, o.Street2, o.City, o.[State], o.PostalCode, o.Country, o.Currency, o.HowYouHearAboutUs
		FROM dbo.Mc_Organization AS o
		WHERE (o.Deleted = 0) AND (o.Active = 1) AND ('','' + o.LdapDomains + '','' like ''%,'' + @LdapDomain + '',%'')
		ORDER BY o.LdapServerAddress DESC;
	END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizationsByLoginId]
	(  
		@LoginId uniqueidentifier
	)
	AS
	BEGIN
		SET NOCOUNT OFF;

		SELECT o.OrganizationId, o.PseudoId, o.[Name], o.[Description], o.WebsiteUrl, o.DatabaseId
			, o.FiscalYearStartMonth, o.FiscalYearStartDay, o.WeekStartsDay, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword, o.LdapDomains
			, o.ExpirationTime, o.GraceDays, o.ExternalId, o.Active, o.CanceledTime, o.Trial, o.Beta, o.Deleted, o.CreatedTime
			, o.Street, o.Street2, o.City, o.[State], o.PostalCode, o.Country, o.Currency, o.HowYouHearAboutUs
		FROM dbo.Mc_OrganizationsLogins AS ol
		INNER JOIN dbo.Mc_Organization AS o
			ON (ol.OrganizationId = o.OrganizationId) AND (ol.LoginId = @LoginId) AND (o.Deleted = 0) AND (o.Active = 1) AND (ol.Active = 1);
	END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertOrganization]
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

		INSERT INTO dbo.Mc_Organization (OrganizationId, PseudoId, [Name], [Description], WebsiteUrl, DatabaseId
			, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, ExpirationTime, GraceDays, Active, CanceledTime, Trial, Deleted, CreatedTime
			, Street, Street2, City, [State], PostalCode, Country, Currency, HowYouHearAboutUs) 
		VALUES (@OrganizationId, @PseudoId, @Name, @Description, @WebsiteUrl, @DatabaseId
			, @FiscalYearStartMonth, @FiscalYearStartDay, @WeekStartsDay, @ExpirationTime, @GraceDays, @Active, @CanceledTime, @Trial, @Deleted, GETUTCDATE()
			, @Street, @Street2, @City, @State, @PostalCode, @Country, @Currency, @HowYouHearAboutUs);
		
		SELECT OrganizationId, PseudoId, [Name], [Description], WebsiteUrl, DatabaseId
			, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword
			, ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Deleted, CreatedTime 
			, Street, Street2, City, [State], PostalCode, Country, Currency, HowYouHearAboutUs
		FROM dbo.Mc_Organization 
		WHERE (OrganizationId = @OrganizationId);
	END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateOrganization]
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
		SET PseudoId  = @PseudoId, [Name] = @Name, [Description] = @Description, WebsiteUrl = @WebsiteUrl, DatabaseId = @DatabaseId
			, FiscalYearStartMonth = @FiscalYearStartMonth, FiscalYearStartDay = @FiscalYearStartDay, WeekStartsDay = @WeekStartsDay
			, LdapServerAddress = @LdapServerAddress, LdapServerPort = @LdapServerPort, LdapDomain = @LdapDomain, LdapUserName = @LdapUserName, LdapPassword = @LdapPassword, LdapDomains = @LdapDomains
			, ExpirationTime = @ExpirationTime, GraceDays = @GraceDays, Active = @Active, CanceledTime = @CanceledTime, Trial = @Trial, Beta = @Beta, Deleted = @Deleted
			, Street = @Street, Street2 = @Street2, City = @City, [State] = @State, PostalCode = @PostalCode, Country = @Country, Currency = @Currency, HowYouHearAboutUs = @HowYouHearAboutUs
		WHERE (OrganizationId = @OrganizationId);

		SELECT OrganizationId, PseudoId, [Name], [Description], WebsiteUrl, DatabaseId
			, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains
			, ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime
			, Street, Street2, City, [State], PostalCode, Country, Currency, HowYouHearAboutUs
		FROM dbo.Mc_Organization
		WHERE (OrganizationId = @OrganizationId);
	END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION