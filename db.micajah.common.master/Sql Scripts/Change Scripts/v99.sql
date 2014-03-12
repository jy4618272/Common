BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_InvitedLogin_InvitedLoginId')
      ALTER TABLE [dbo].[Mc_InvitedLogin] DROP CONSTRAINT [DF_Mc_InvitedLogin_InvitedLoginId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_InvitedLogin_CreatedTime')
      ALTER TABLE [dbo].[Mc_InvitedLogin] DROP CONSTRAINT [DF_Mc_InvitedLogin_CreatedTime]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_DeleteInvitedLogin]
(
	@InvitedLoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_InvitedLogin
	WHERE 
		(InvitedLoginId = @InvitedLoginId)
		OR (DATEDIFF(DAY, CreatedTime, GETUTCDATE()) >= 30);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_DeleteResetPasswordRequest]
(
	@ResetPasswordRequestId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_ResetPasswordRequest
	WHERE 
		(ResetPasswordRequestId = @ResetPasswordRequestId)
		OR (DATEDIFF(MINUTE, CreatedTime, GETUTCDATE()) > 60);
END
')

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
	SET @Date = DATEADD(DAY, -1, GETUTCDATE());

	DELETE FROM dbo.Mc_Resource
	WHERE (ResourceId = @ResourceId) 
		OR (ParentResourceId = @ResourceId)
		OR ((Temporary = 1) AND (CreatedTime <= @Date));
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizationsLdapGroupsDomains]
(
	@OrganizationId uniqueidentifier)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT 
		Domain
		,[DomainId]
		,CASE [Id]
			WHEN ''00000000-0000-0000-0000-000000000000'' THEN NEWID()
		END as [Id]
		,[OrganizationId]
		,[ObjectGUID]
		,[Name]
		,[DistinguishedName]
		,[CreatedTime]
	FROM
	(
		SELECT 
			DISTINCT Domain
			, [DomainId] as [DomainId]
			, ''00000000-0000-0000-0000-000000000000'' as [Id]
			, @OrganizationId as [OrganizationId]
			, ''00000000-0000-0000-0000-000000000000'' as [ObjectGUID]
			, '''' as [Name]
			, '''' as [DistinguishedName]
			, GETUTCDATE() as [CreatedTime]
		FROM [dbo].[Mc_OrganizationsLdapGroups]
		WHERE (OrganizationId = @OrganizationId)
	) A	
	ORDER BY Domain
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
		, @FiscalYearStartMonth, @FiscalYearStartDay, @WeekStartsDay, @ExpirationTime, @GraceDays, @Active, @CanceledTime, @Trial, @Deleted, GETUTCDATE(), @BillingPlan
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
   COMMIT TRANSACTION
