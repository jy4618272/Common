CREATE PROCEDURE [dbo].[Mc_UpdateOrganization]
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
	