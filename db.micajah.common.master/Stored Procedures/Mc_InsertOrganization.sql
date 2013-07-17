CREATE PROCEDURE [dbo].[Mc_InsertOrganization]
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
	END
	