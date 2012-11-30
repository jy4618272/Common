IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertEmail]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertEmailSuffix]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertEmailSuffix]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertDatabaseServer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertDatabaseServer]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertCustomUrl]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertCustomUrl]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertDatabase]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertDatabase]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertOrganization]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertOrganizationLdapGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertOrganizationLdapGroup]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertLogin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertLogin]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertGroupMappings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertGroupMappings]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertInvitedLogin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertInvitedLogin]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetResource]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetResource]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetResources]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetResources]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetResetPasswordRequestsByLoginId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetResetPasswordRequestsByLoginId]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetOrganizationsLdapGroupsDomains]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetOrganizationsLdapGroupsDomains]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetResetPasswordRequest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetResetPasswordRequest]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetWebsites]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetWebsites]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertCountry]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertCountry]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetViewState]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetViewState]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetUnitsOfMeasure]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetUnitsOfMeasure]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetUnitsOfMeasureConversion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetUnitsOfMeasureConversion]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertOrganizationLogin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertOrganizationLogin]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateLoginToken]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateLoginToken]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateOrganization]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateLoginSession]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateLoginSession]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateLogin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateLogin]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateLoginLdapInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateLoginLdapInfo]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateUnitsOfMeasureConversion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateUnitsOfMeasureConversion]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateUnitsOfMeasureOverride]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateUnitsOfMeasureOverride]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateUnitsOfMeasure]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateUnitsOfMeasure]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateOrganizationLogin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateOrganizationLogin]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateResource]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateResource]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertUnitsOfMeasureConversion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertUnitsOfMeasureConversion]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertViewState]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertViewState]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertUnitsOfMeasure]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertUnitsOfMeasure]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertResetPasswordRequest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertResetPasswordRequest]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertResource]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertResource]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateDatabaseServer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateDatabaseServer]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateEmailSuffix]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateEmailSuffix]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateDatabase]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateDatabase]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertWebsite]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertWebsite]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateCustomUrl]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateCustomUrl]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateWebsite]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateWebsite]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetCountries]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetCountries]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetCustomUrl]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetCustomUrl]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteViewState]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteViewState]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteUnitsOfMeasure]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteUnitsOfMeasure]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteUnitsOfMeasureConversion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteUnitsOfMeasureConversion]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetDatabaseServers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetDatabaseServers]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetEmail]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetDatabases]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetDatabases]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetCustomUrls]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetCustomUrls]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetCustomUrlsByOrganizationId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetCustomUrlsByOrganizationId]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteEmailSuffixes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteEmailSuffixes]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteGroupMapping]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteGroupMapping]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteEmail]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetOrganizationsLdapGroupsByOrganizationId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetOrganizationsLdapGroupsByOrganizationId]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteCustomUrl]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteCustomUrl]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteResetPasswordRequest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteResetPasswordRequest]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteResource]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteResource]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteOrganizationLogin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteOrganizationLogin]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteInvitedLogin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteInvitedLogin]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteOrganizationLdapGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteOrganizationLdapGroup]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetEmails]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetEmails]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetLoginsByOrganizationId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetLoginsByOrganizationId]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetOrganizationLogin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetOrganizationLogin]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetLoginByToken]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetLoginByToken]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetLoginByEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetLoginByEmail]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetLoginByLoginName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetLoginByLoginName]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetOrganizationsLdapGroups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetOrganizationsLdapGroups]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetOrganizationsLdapGroupsAll]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetOrganizationsLdapGroupsAll]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetOrganizationsByLoginId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetOrganizationsByLoginId]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetOrganizations]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetOrganizations]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetOrganizationsByLdapDomain]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetOrganizationsByLdapDomain]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetEmailSuffixesByOrganizationId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetEmailSuffixesByOrganizationId]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetGroupMappings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetGroupMappings]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetEmailSuffixesByInstanceId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetEmailSuffixesByInstanceId]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetEmailSuffix]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetEmailSuffix]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetEmailSuffixes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetEmailSuffixes]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetLogin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetLogin]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetLoginByDomainName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetLoginByDomainName]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetLdapServerDetails]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetLdapServerDetails]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetInvitedLogin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetInvitedLogin]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetInvitedLoginsByOrganizationId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetInvitedLoginsByOrganizationId]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_InsertEmail]
(
	@Email [nvarchar](255),
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Email (Email, LoginId)
	VALUES (@Email, @LoginId)	
	
	SELECT Email, LoginId
	FROM dbo.Mc_Email
	WHERE (Email = @Email)
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertEmailSuffix]
(
	@EmailSuffixId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@EmailSuffixName nvarchar(1024)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_EmailSuffix (EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName)
	VALUES (@EmailSuffixId, @OrganizationId, @InstanceId, @EmailSuffixName);
	
	SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName 
	FROM dbo.Mc_EmailSuffix
	WHERE EmailSuffixId = @EmailSuffixId;
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_InsertDatabaseServer]
(
	@DatabaseServerId uniqueidentifier,
	@Name nvarchar(255),
	@InstanceName nvarchar(255),
	@Port int,
	@Description nvarchar(1024),
	@WebsiteId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_DatabaseServer (DatabaseServerId, [Name], InstanceName, Port, [Description], WebsiteId, Deleted) 
	VALUES (@DatabaseServerId, @Name, @InstanceName, @Port, @Description, @WebSiteId, @Deleted);
	
	SELECT @DatabaseServerId, [Name], InstanceName, Port, [Description], WebsiteId, Deleted 
	FROM dbo.Mc_DatabaseServer 
	WHERE (DatabaseServerId = @DatabaseServerId);
END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertCustomUrl]
(
	@CustomUrlId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@FullCustomUrl nvarchar(1024),
	@PartialCustomUrl nvarchar(1024)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_CustomUrl (CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl)
	VALUES (@CustomUrlId, @OrganizationId, @InstanceId, @FullCustomUrl, @PartialCustomUrl);
	
	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE CustomUrlId = @CustomUrlId;
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertDatabase]
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

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertOrganization]
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

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_InsertOrganizationLdapGroup]
(
	@Id uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@DomainId uniqueidentifier,	
	@Domain nvarchar(255),
	@ObjectGUID uniqueidentifier,
	@Name nvarchar(255),
	@DistinguishedName nvarchar(2048),
	@CreatedTime datetime
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_OrganizationsLdapGroups]([Id],[OrganizationId],[DomainId], [Domain],[ObjectGUID],[Name],[DistinguishedName],[CreatedTime])
    VALUES(@Id,@OrganizationId, @DomainId, @Domain, @ObjectGUID, @Name, @DistinguishedName, @CreatedTime)
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertLogin]
(
	@LoginId uniqueidentifier,
	@LoginName nvarchar(255),
	@Password nvarchar(50),
	@Token varchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Login (LoginId, LoginName, [Password], Deleted, Token) 
	VALUES (@LoginId, @LoginName, @Password, 0, @Token);
	
	SELECT LoginId, FirstName, LastName, LoginName, [Password], ProfileUpdated, Deleted, SessionId, Token
	FROM dbo.Mc_Login 
	WHERE (LoginId = @LoginId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertGroupMappings]
(
	@GroupId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@GroupName nvarchar(255),
	@LdapDomainId uniqueidentifier,
	@LdapDomainName nvarchar(255),
	@LdapGroupId uniqueidentifier,
	@LdapGroupName nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_GroupMappings (GroupId, OrganizationId, [GroupName], LdapDomainId, [LdapDomainName], LdapGroupId, [LdapGroupName]) 
	VALUES (@GroupId, @OrganizationId, @GroupName, @LdapDomainId, @LdapDomainName, @LdapGroupId, @LdapGroupName);
	
	SELECT GroupId, OrganizationId, [GroupName], LdapDomainId, [LdapDomainName], LdapGroupId, [LdapGroupName] 
	FROM dbo.Mc_GroupMappings 
	WHERE (GroupId = @GroupId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertInvitedLogin]
(
	@InvitedLoginId uniqueidentifier,
	@LoginName nvarchar(255),
	@OrganizationId uniqueidentifier,
	@GroupId varchar(2056),
	@InvitedBy uniqueidentifier,
	@CreatedTime datetime
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	INSERT INTO dbo.Mc_InvitedLogin	(InvitedLoginId, LoginName, OrganizationId, GroupId, InvitedBy, CreatedTime)
	VALUES (@InvitedLoginId, @LoginName, @OrganizationId, @GroupId, @InvitedBy, @CreatedTime)
	
	SELECT InvitedLoginId, LoginName, OrganizationId, GroupId, InvitedBy, CreatedTime
	FROM dbo.Mc_InvitedLogin
	WHERE (InvitedLoginId = @InvitedLoginId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetResource]
(
	@ResourceId uniqueidentifier,
	@Height int,
	@Width int,
	@Align int
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	DECLARE @ResId uniqueidentifier;
	
	IF (@Height IS NOT NULL) OR (@Width IS NOT NULL)
	BEGIN
		IF EXISTS(SELECT 0 FROM dbo.Mc_Resource WHERE (ResourceId = @ResourceId) AND (UPPER(LEFT(ContentType, 6)) = 'IMAGE/'))
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
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetResources]
(
	@LocalObjectType nvarchar(50),
	@LocalObjectId nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT ResourceId, ParentResourceId, LocalObjectType, LocalObjectId, Content, ContentType, [Name], Width, Height, Align, Temporary, CreatedTime
	FROM dbo.Mc_Resource
	WHERE (LocalObjectType = @LocalObjectType) AND (LocalObjectId = @LocalObjectId)
	ORDER BY CreatedTime, Name;
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetResetPasswordRequestsByLoginId]
(
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT ResetPasswordRequestId, LoginId, CreatedTime
	FROM dbo.Mc_ResetPasswordRequest
	WHERE (LoginId = @LoginId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetOrganizationsLdapGroupsDomains]
(
	@OrganizationId uniqueidentifier)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT 
		Domain
		,[DomainId]
		,CASE [Id]
			WHEN '00000000-0000-0000-0000-000000000000' THEN NEWID()
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
			, '00000000-0000-0000-0000-000000000000' as [Id]
			, @OrganizationId as [OrganizationId]
			, '00000000-0000-0000-0000-000000000000' as [ObjectGUID]
			, '' as [Name]
			, '' as [DistinguishedName]
			, GETUTCDATE() as [CreatedTime]
		FROM [dbo].[Mc_OrganizationsLdapGroups]
		WHERE (OrganizationId = @OrganizationId)
	) A	
	ORDER BY Domain
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetResetPasswordRequest]
(
	@ResetPasswordRequestId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT ResetPasswordRequestId, LoginId, CreatedTime
	FROM dbo.Mc_ResetPasswordRequest
	WHERE (ResetPasswordRequestId = @ResetPasswordRequestId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetWebsites]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT WebsiteId, [Name], Url, [Description], AdminContactInfo, Deleted 
	FROM dbo.Mc_Website 
	WHERE (Deleted = 0);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_InsertCountry]
(
	@CountryId uniqueidentifier,
	@Name nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Country(CountryId, [Name])
	VALUES (@CountryId, @Name)

	SELECT CountryId, [Name]
	FROM dbo.Mc_Country
	WHERE (CountryId = @CountryId);
END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetViewState]
(
    @ViewStateId uniqueidentifier
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ViewState
    FROM dbo.Mc_ViewState WITH (READUNCOMMITTED)
    WHERE (ViewStateId = @ViewStateId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetUnitsOfMeasure]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [UnitsOfMeasureId]
      ,[OrganizationId]
      ,[SingularName]
      ,[SingularAbbrv]
      ,[PluralName]
      ,[PluralAbbrv]
      ,[GroupName]
      ,[LocalName]
	FROM [dbo].[Mc_UnitsOfMeasure]
	ORDER BY [OrganizationId] asc, [GroupName] asc, [SingularName] asc;
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetUnitsOfMeasureConversion]
(
	@UnitOfMeasureFrom uniqueidentifier=null,
	@UnitOfMeasureTo uniqueidentifier=null
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [UnitOfMeasureFrom]
	  ,[UnitOfMeasureTo]
	  ,[OrganizationId]
	  ,[Factor]
	FROM [dbo].[Mc_UnitsOfMeasureConversion]
	WHERE (@UnitOfMeasureFrom IS NULL OR UnitOfMeasureFrom = @UnitOfMeasureFrom)
	AND (@UnitOfMeasureTo IS NULL OR UnitOfMeasureTo = @UnitOfMeasureTo);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertOrganizationLogin]
(
	@OrganizationId uniqueidentifier,
	@LoginId uniqueidentifier,
	@OrganizationAdministrator bit,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_OrganizationsLogins (OrganizationId, LoginId, OrganizationAdministrator, Active)
	VALUES (@OrganizationId, @LoginId, @OrganizationAdministrator, @Active);
	
	SELECT OrganizationId, LoginId, OrganizationAdministrator, Active
	FROM dbo.Mc_OrganizationsLogins
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateLoginToken]
(
	@LoginId uniqueidentifier,
	@Token varchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Login
	SET Token = @Token
	WHERE LoginId = @LoginId;
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateLoginSession]
(
	@LoginId uniqueidentifier,
	@SessionId varchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Login
	SET SessionId = @SessionId
	WHERE LoginId = @LoginId;
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateLogin]
(
	@LoginId uniqueidentifier,
	@LoginName nvarchar(255),
	@Password nvarchar(50),
	@FirstName nvarchar(255),
	@LastName nvarchar(255),
	@Deleted bit
)  
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Login
	SET FirstName = (CASE WHEN @FirstName IS NULL THEN FirstName ELSE @FirstName END)
		, LastName = (CASE WHEN @LastName IS NULL THEN LastName ELSE @LastName END)
		, LoginName = (CASE WHEN @LoginName IS NULL THEN LoginName ELSE @LoginName END)
		, [Password] = (CASE WHEN @Password IS NULL THEN [Password] ELSE @Password END)
		, Deleted = (CASE WHEN @Deleted IS NULL THEN Deleted ELSE @Deleted END)
	WHERE (LoginId = @LoginId);

	SELECT LoginId, FirstName, LastName, LoginName, [Password], SessionId, Deleted
	FROM dbo.Mc_Login
	WHERE (LoginId = @LoginId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateLoginLdapInfo]    
(    
 @OrganizationId uniqueidentifier,    
 @LoginId uniqueidentifier,    
 @LdapDomain nvarchar(255),    
 @LdapDomainFull nvarchar(255),    
 @LdapUserAlias nvarchar(255),    
 @LdapUPN nvarchar(255),    
 @LdapSecurityId nvarchar(255),    
 @LdapUserId uniqueidentifier,   
 @LdapOUPath nvarchar(255)  
)    
AS    
BEGIN    
 SET NOCOUNT OFF;    
    
 UPDATE dbo.Mc_OrganizationsLogins     
 SET LdapDomain = @LdapDomain,    
  LdapDomainFull = @LdapDomainFull,    
  LdapUserAlias = @LdapUserAlias,    
  LdapUPN = @LdapUPN,    
  LdapSecurityId = @LdapSecurityId,    
  LdapUserId = @LdapUserId,   
  LdapOUPath = @LdapOUPath   
 WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);    
     
 SELECT OrganizationId, LoginId, OrganizationAdministrator, LdapDomain, LdapUserAlias, LdapSecurityId, LdapUserId, LdapOUPath, Active    
 FROM dbo.Mc_OrganizationsLogins    
 WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);    
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateUnitsOfMeasureConversion]
(
	@UnitOfMeasureFrom uniqueidentifier,
	@UnitOfMeasureTo uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@Factor float
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [dbo].[Mc_UnitsOfMeasureConversion]
	SET [Factor] = @Factor
	WHERE UnitOfMeasureFrom = @UnitOfMeasureFrom
	AND UnitOfMeasureTo = @UnitOfMeasureTo
	AND OrganizationId = @OrganizationId;

	SELECT [UnitOfMeasureFrom]
      ,[UnitOfMeasureTo]
      ,[Factor]
	FROM [dbo].[Mc_UnitsOfMeasureConversion]
	WHERE UnitOfMeasureFrom = @UnitOfMeasureFrom
	AND UnitOfMeasureTo = @UnitOfMeasureTo
	AND OrganizationId = @OrganizationId;
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateUnitsOfMeasureOverride]
(
	@UnitsOfMeasureId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	declare @emptyId uniqueidentifier;
	select @emptyId = '00000000-0000-0000-0000-000000000000';

	if not exists(select 'true' 
		from [dbo].[Mc_UnitsOfMeasure]
		where [UnitsOfMeasureId] = @UnitsOfMeasureId
		and OrganizationId = @emptyId)
	return;
	
	if not exists(select 'true' 
		from [dbo].[Mc_UnitsOfMeasure]
		where [UnitsOfMeasureId] = @UnitsOfMeasureId
		and OrganizationId = @OrganizationId)
	begin 
		insert into [dbo].[Mc_UnitsOfMeasure]
			   ([UnitsOfMeasureId]
			   ,[OrganizationId]
			   ,[SingularName]
			   ,[SingularAbbrv]
			   ,[PluralName]
			   ,[PluralAbbrv]
			   ,[GroupName]
			   ,[LocalName])
		 select UnitsOfMeasureId
			   ,@OrganizationId
			   ,SingularName
			   ,SingularAbbrv
			   ,PluralName
			   ,PluralAbbrv
			   ,GroupName
			   ,LocalName
		from [dbo].[Mc_UnitsOfMeasure]
		where UnitsOfMeasureId = @UnitsOfMeasureId
		and OrganizationId = @emptyId;
		
		insert into [dbo].[Mc_UnitsOfMeasureConversion]
           ([UnitOfMeasureFrom]
           ,[UnitOfMeasureTo]
           ,[Factor]
           ,[OrganizationId])
		select [UnitOfMeasureFrom]
		  ,[UnitOfMeasureTo]
		  ,[Factor]
		  ,@OrganizationId
		from [dbo].[Mc_UnitsOfMeasureConversion]
		where UnitOfMeasureFrom = @UnitsOfMeasureId
		and OrganizationId = @emptyid
		and UnitOfMeasureTo in (
			select UnitOfMeasureTo from (
				select UnitOfMeasureTo
				from [dbo].[Mc_UnitsOfMeasureConversion]
				where (UnitOfMeasureFrom = @UnitsOfMeasureId)
				and OrganizationId = @emptyid) as s
			inner join [dbo].[Mc_UnitsOfMeasure] as s1
			on s1.UnitsOfMeasureId = s.UnitOfMeasureTo
			and s1.OrganizationId = @OrganizationId
			)

		select [UnitsOfMeasureId]
		  ,[OrganizationId]
		  ,[SingularName]
		  ,[SingularAbbrv]
		  ,[PluralName]
		  ,[PluralAbbrv]
		  ,[GroupName]
		  ,[LocalName]
		from [dbo].[Mc_UnitsOfMeasure]
		where [UnitsOfMeasureId] = @UnitsOfMeasureId
		and [OrganizationId] = @OrganizationId;
	end
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateUnitsOfMeasure]
(
	@UnitsOfMeasureId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@SingularName nvarchar(255),
	@SingularAbbrv nvarchar(50),
	@PluralName nvarchar(255),
	@PluralAbbrv nvarchar(50),
	@GroupName nvarchar(50),
	@LocalName nvarchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [dbo].[Mc_UnitsOfMeasure]
	SET [SingularName] = @SingularName
      ,[SingularAbbrv] = @SingularAbbrv
      ,[PluralName] = @PluralName
      ,[PluralAbbrv] = @PluralAbbrv
      ,[GroupName] = @GroupName
      ,[LocalName] = @LocalName
	WHERE UnitsOfMeasureId = @UnitsOfMeasureId
	AND OrganizationId = @OrganizationId;

	SELECT [UnitsOfMeasureId]
      ,[OrganizationId]
      ,[SingularName]
      ,[SingularAbbrv]
      ,[PluralName]
      ,[PluralAbbrv]
      ,[GroupName]
      ,[LocalName]
	FROM [dbo].[Mc_UnitsOfMeasure]
	WHERE [UnitsOfMeasureId] = @UnitsOfMeasureId
	AND [OrganizationId] = @OrganizationId;
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateOrganizationLogin]
(
	@OrganizationId uniqueidentifier,
	@LoginId uniqueidentifier,
	@OrganizationAdministrator bit,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_OrganizationsLogins 
	SET OrganizationAdministrator = (CASE WHEN @OrganizationAdministrator IS NULL THEN OrganizationAdministrator ELSE @OrganizationAdministrator END)
		, Active = (CASE WHEN @Active IS NULL THEN Active ELSE @Active END)
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
	
	SELECT OrganizationId, LoginId, OrganizationAdministrator, LdapDomain, LdapUserAlias, LdapSecurityId, LdapUserId, Active
	FROM dbo.Mc_OrganizationsLogins
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateResource]
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
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertUnitsOfMeasureConversion]
(
	@UnitOfMeasureFrom uniqueidentifier,
	@UnitOfMeasureTo uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@Factor float
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]
           ([UnitOfMeasureFrom]
           ,[UnitOfMeasureTo]
           ,[OrganizationId]
           ,[Factor])
    VALUES
           (@UnitOfMeasureFrom
           ,@UnitOfMeasureTo
           ,@OrganizationId
           ,@Factor);
	
	SELECT [UnitOfMeasureFrom]
      ,[UnitOfMeasureTo]
      ,[OrganizationId]
      ,[Factor]
	FROM [dbo].[Mc_UnitsOfMeasureConversion]
	WHERE UnitOfMeasureFrom = @UnitOfMeasureFrom
	AND UnitOfMeasureTo = @UnitOfMeasureTo
	AND OrganizationId = @OrganizationId;
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertViewState]
(
	@ViewStateId uniqueidentifier,
    @ViewState varbinary(max),
    @ExpirationTime datetime
)
AS
BEGIN
    SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_ViewState (ViewStateId, ViewState, ExpirationTime) 
	VALUES (@ViewStateId, @ViewState, @ExpirationTime);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertUnitsOfMeasure]
(
	@UnitsOfMeasureId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@SingularName nvarchar(255),
	@SingularAbbrv nvarchar(50),
	@PluralName nvarchar(255),
	@PluralAbbrv nvarchar(50),
	@GroupName nvarchar(50),
	@LocalName nvarchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_UnitsOfMeasure]
           ([UnitsOfMeasureId]
           ,[OrganizationId]
           ,[SingularName]
           ,[SingularAbbrv]
           ,[PluralName]
           ,[PluralAbbrv]
           ,[GroupName]
           ,[LocalName])
     VALUES(
			@UnitsOfMeasureId
           ,@OrganizationId
           ,@SingularName
           ,@SingularAbbrv
           ,@PluralName
           ,@PluralAbbrv
           ,@GroupName
           ,@LocalName);
	
	SELECT [UnitsOfMeasureId]
      ,[OrganizationId]
      ,[SingularName]
      ,[SingularAbbrv]
      ,[PluralName]
      ,[PluralAbbrv]
      ,[GroupName]
      ,[LocalName]
	FROM [dbo].[Mc_UnitsOfMeasure]
	WHERE [UnitsOfMeasureId] = @UnitsOfMeasureId
	AND [OrganizationId] = @OrganizationId;
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertResetPasswordRequest]
(
	@ResetPasswordRequestId uniqueidentifier,
	@LoginId uniqueidentifier,
	@CreatedTime datetime
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	INSERT INTO dbo.Mc_ResetPasswordRequest	(ResetPasswordRequestId, LoginId, CreatedTime)
	VALUES (@ResetPasswordRequestId, @LoginId, @CreatedTime)
	
	SELECT ResetPasswordRequestId, LoginId, CreatedTime
	FROM dbo.Mc_ResetPasswordRequest
	WHERE (ResetPasswordRequestId = @ResetPasswordRequestId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertResource]
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
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_UpdateDatabaseServer]
(
	@DatabaseServerId uniqueidentifier,
	@Name nvarchar(255),
	@InstanceName nvarchar(255),
	@Port int,
	@Description nvarchar(1024),
	@WebsiteId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_DatabaseServer 
	SET [Name] = @Name, InstanceName = @InstanceName, Port = @Port, [Description] = @Description, WebsiteId = @WebsiteId, Deleted = @Deleted 
	WHERE (DatabaseServerId = @DatabaseServerId);
	
	SELECT DatabaseServerId, [Name], InstanceName, Port, [Description], WebsiteId, Deleted 
	FROM dbo.Mc_DatabaseServer 
	WHERE (DatabaseServerId = @DatabaseServerId);
END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateEmailSuffix]
(
	@EmailSuffixId uniqueidentifier,
	@EmailSuffixName nvarchar(1024)
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_EmailSuffix
	SET EmailSuffixName = @EmailSuffixName 
	WHERE EmailSuffixId = @EmailSuffixId;
	
	SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName 
	FROM dbo.Mc_EmailSuffix
	WHERE EmailSuffixId = @EmailSuffixId;
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateDatabase]
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

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_InsertWebsite]
(
	@WebsiteId uniqueidentifier,
	@Name nvarchar(255),
	@Url nvarchar(2048),
	@Description nvarchar(1024),
	@AdminContactInfo nvarchar(2048),
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Website (WebsiteId, [Name], Url, [Description], [AdminContactInfo], [Deleted]) 
	VALUES (@WebsiteId, @Name, @Url, @Description, @AdminContactInfo, @Deleted);
	
	SELECT WebsiteId, [Name], Url, [Description], AdminContactInfo, Deleted 
	FROM dbo.Mc_Website 
	WHERE (WebsiteId = @WebsiteId);
END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateCustomUrl]
(
	@CustomUrlId uniqueidentifier,
	@FullCustomUrl nvarchar(1024),
	@PartialCustomUrl nvarchar(1024)
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_CustomUrl
	SET FullCustomUrl = @FullCustomUrl, PartialCustomUrl = @PartialCustomUrl
	WHERE CustomUrlId = @CustomUrlId;
	
	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE CustomUrlId = @CustomUrlId;
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_UpdateWebsite]
(
	@WebsiteId uniqueidentifier,
	@Name nvarchar(255),
	@Url nvarchar(2048),
	@Description nvarchar(1024),
	@AdminContactInfo nvarchar(2048),
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_WebSite
	SET [Name] = @Name, Url = @Url, [Description] = @Description, AdminContactInfo = @AdminContactInfo, Deleted = @Deleted 
	WHERE (WebsiteId = @WebsiteId);
	
	SELECT WebsiteId, [Name], Url, [Description], AdminContactInfo, Deleted 
	FROM dbo.Mc_WebSite 
	WHERE (WebsiteId = @WebsiteId);
END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetCountries]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT CountryId, [Name]
	FROM dbo.Mc_Country
	ORDER BY [Name];
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetCustomUrl]
(
	@CustomUrlId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE (CustomUrlId = @CustomUrlId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_DeleteViewState]
(
    @Now datetime
)
AS
BEGIN
    SET NOCOUNT OFF;
    
	DELETE dbo.Mc_ViewState
	WHERE ExpirationTime < @Now;
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_DeleteUnitsOfMeasure]
(
	@UnitsOfMeasureId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM [dbo].[Mc_UnitsOfMeasureConversion]
    WHERE OrganizationId = @OrganizationId
    AND (UnitOfMeasureFrom = @UnitsOfMeasureId OR UnitOfMeasureTo = @UnitsOfMeasureId);

	DELETE FROM [dbo].[Mc_UnitsOfMeasure] 
	WHERE OrganizationId = @OrganizationId
	AND UnitsOfMeasureId = @UnitsOfMeasureId;
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_DeleteUnitsOfMeasureConversion]
(
	@UnitOfMeasureFrom uniqueidentifier,
	@UnitOfMeasureTo uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.[Mc_UnitsOfMeasureConversion]
	WHERE UnitOfMeasureFrom = @UnitOfMeasureFrom
	AND UnitOfMeasureTo = @UnitOfMeasureTo
	AND OrganizationId = @OrganizationId;
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetDatabaseServers]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT DatabaseServerId, [Name], InstanceName, Port, [Description], WebsiteId, Deleted 
	FROM dbo.Mc_DatabaseServer 
	WHERE (Deleted = 0);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_GetEmail]
(
	@Email [nvarchar](255)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Email, LoginId
	FROM dbo.Mc_Email
	WHERE (Email = @Email)
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetDatabases]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT DatabaseId, [Name], [Description], UserName, [Password], DatabaseServerId, [Private], Deleted 
	FROM dbo.Mc_Database
	WHERE (Deleted = 0);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetCustomUrls]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@FullCustomUrl nvarchar(1024),
	@PartialCustomUrl nvarchar(1024)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE 
		(OrganizationId = @OrganizationId)
		AND (	(InstanceId = @InstanceId)
				OR  ((@InstanceId IS NULL) AND (InstanceId IS NULL))
		)
	UNION
	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE
		(	(FullCustomUrl = @FullCustomUrl) 
			AND (@FullCustomUrl IS NOT NULL)
		)
		OR 	(	(PartialCustomUrl = @PartialCustomUrl)
				AND (@PartialCustomUrl IS NOT NULL)
		);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetCustomUrlsByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE (OrganizationId = @OrganizationId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_DeleteEmailSuffixes]  
(  
 @OrganizationId uniqueidentifier,
 @InstanceId uniqueidentifier
)  
AS  
BEGIN  
 SET NOCOUNT OFF;  

 IF @InstanceId IS NOT NULL
 BEGIN
   DELETE FROM dbo.Mc_EmailSuffix  
   WHERE InstanceId = @InstanceId;  
 END
 ELSE  
   DELETE FROM dbo.Mc_EmailSuffix  
   WHERE OrganizationId = @OrganizationId AND InstanceId IS NULL;  
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_DeleteGroupMapping]
(
	@GroupId uniqueidentifier,
	@LdapDomainId uniqueidentifier,
	@LdapGroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_GroupMappings
	WHERE GroupId = @GroupId AND LdapDomainId = @LdapDomainId AND LdapGroupId = @LdapGroupId;
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_DeleteEmail]  
(  
 @LoginId uniqueidentifier
)  
AS  
BEGIN  
	SET NOCOUNT OFF;  
	DELETE FROM dbo.Mc_Email  
	WHERE LoginId = @LoginId
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_GetOrganizationsLdapGroupsByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT [Id]
      ,[OrganizationId]
	  ,[DomainId]
      ,[Domain]
      ,[ObjectGUID]
      ,[Name]
      ,[DistinguishedName]
      ,[CreatedTime]
	FROM [dbo].[Mc_OrganizationsLdapGroups]
	WHERE (OrganizationId = @OrganizationId)
	ORDER BY [Name]
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_DeleteCustomUrl]
(
	@CustomUrlId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_CustomUrl
	WHERE CustomUrlId = @CustomUrlId;
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_DeleteResetPasswordRequest]
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

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_DeleteResource]
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

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_DeleteOrganizationLogin]
(
	@OrganizationId uniqueidentifier,
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_OrganizationsLogins 
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_DeleteInvitedLogin]
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

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_DeleteOrganizationLdapGroup]
(
	@OrganizationId uniqueidentifier,
	@Domain nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_OrganizationsLdapGroups 
	WHERE (OrganizationId = @OrganizationId) AND (Domain = @Domain)
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_GetEmails]
(
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Email, LoginId
	FROM dbo.Mc_Email
	WHERE (LoginId = @LoginId)
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetLoginsByOrganizationId]      
(      
	@OrganizationId uniqueidentifier      
)      
AS      
BEGIN      
	SET NOCOUNT OFF;      

	SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.ProfileUpdated, l.Deleted, l.SessionId, l.Token
		, ol.OrganizationAdministrator, ol.LdapDomain, ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, ol.LdapUserId, ol.LdapOUPath, ol.Active      
	FROM dbo.Mc_OrganizationsLogins AS ol      
	INNER JOIN dbo.Mc_Login AS l      
		ON (ol.LoginId = l.LoginId) AND (ol.OrganizationId = @OrganizationId) AND (l.Deleted = 0);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetOrganizationLogin]    
(    
 @OrganizationId uniqueidentifier,    
 @LoginId uniqueidentifier    
)    
AS    
BEGIN    
 SET NOCOUNT OFF;    
    
 SELECT ol.OrganizationId, ol.LoginId, ol.OrganizationAdministrator, l.FirstName, l.LastName, ol.LdapDomain, ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, ol.LdapUserId, ol.LdapOUPath, ol.Active    
 FROM dbo.Mc_OrganizationsLogins ol 
 INNER JOIN dbo.Mc_Login l 
	ON (ol.LoginId = l.LoginId) 
 WHERE (ol.OrganizationId = @OrganizationId) AND (ol.LoginId = @LoginId);    
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetLoginByToken]
(
	@Token varchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT LoginId, FirstName, LastName, LoginName, [Password], ProfileUpdated, Deleted, SessionId, Token
	FROM dbo.Mc_Login
	WHERE Token = @Token;
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_GetLoginByEmail]      
(      
 @EmailAddress nvarchar(255)      
)      
AS      
BEGIN      
 SET NOCOUNT ON;      
       
 SELECT DISTINCT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.Password, ol.OrganizationId, ol.Active,   
  ol.LdapDomain as 'UserLdapDomain', ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId,   
  ol.LdapUserId, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword   
 FROM Mc_Login l
  left join Mc_Email e ON l.LoginId = e.LoginId
  inner join Mc_OrganizationsLogins ol ON l.LoginId = ol.LoginId   
  inner join Mc_Organization o ON ol.OrganizationId = o.OrganizationId   
 WHERE l.Deleted = 0 AND o.Deleted = 0 AND o.Active = 1 AND   
  (l.LoginName = @EmailAddress OR ol.LdapUPN = @EmailAddress OR e.Email = @EmailAddress)  
 ORDER BY o.LdapServerAddress DESC;  
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetLoginByLoginName]
(
	@LoginName nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.ProfileUpdated, l.Deleted, l.SessionId, l.Token
	FROM dbo.Mc_Login AS l
	LEFT JOIN dbo.Mc_Email AS e 
		ON l.LoginId = e.LoginId AND e.Email = @LoginName
	WHERE (l.LoginName = @LoginName OR e.Email IS NOT NULL);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_GetOrganizationsLdapGroups]
(
	@OrganizationId uniqueidentifier,
	@Domain nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT TOP 25 [Id]
      ,[OrganizationId]
	  ,[DomainId]
      ,[Domain]
      ,[ObjectGUID]
      ,[Name]
      ,[DistinguishedName]
      ,[CreatedTime]
	FROM [dbo].[Mc_OrganizationsLdapGroups]
	WHERE (OrganizationId = @OrganizationId) AND (Domain = @Domain)
	ORDER BY [Name]
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_GetOrganizationsLdapGroupsAll]
(
	@OrganizationId uniqueidentifier,
	@Domain nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT [Id]
      ,[OrganizationId]
	  ,[DomainId]
      ,[Domain]
      ,[ObjectGUID]
      ,[Name]
      ,[DistinguishedName]
      ,[CreatedTime]
	FROM [dbo].[Mc_OrganizationsLdapGroups]
	WHERE (OrganizationId = @OrganizationId) AND (Domain = @Domain)
	ORDER BY [Name]
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetOrganizationsByLoginId]
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

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetOrganizations]  
AS  
BEGIN  
 SET NOCOUNT OFF;  
  
 SELECT OrganizationId, PseudoId, [Name], [Description], WebsiteUrl, DatabaseId  
  , FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains  
  , ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, BillingPlan
  , Street, Street2, City, [State], PostalCode, Country, Currency
 FROM dbo.Mc_Organization   
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetOrganizationsByLdapDomain]
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
	WHERE (o.Deleted = 0) AND (o.Active = 1) AND (',' + o.LdapDomains + ',' like '%,' + @LdapDomain + ',%')
	ORDER BY o.LdapServerAddress DESC;
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetEmailSuffixesByOrganizationId]  
(  
 @OrganizationId uniqueidentifier  
)  
AS  
BEGIN  
 SET NOCOUNT OFF;  
  
 SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName   
 FROM dbo.Mc_EmailSuffix  
 WHERE ((OrganizationId = @OrganizationId) AND (InstanceId IS NULL))
 UNION
 SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName   
 FROM dbo.Mc_EmailSuffix  
 WHERE ((OrganizationId = @OrganizationId) AND (InstanceId IS NOT NULL));  
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetGroupMappings]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT GroupId, OrganizationId, [GroupName], LdapDomainId, [LdapDomainName], LdapGroupId, [LdapGroupName] 
	FROM dbo.Mc_GroupMappings 
	WHERE (OrganizationId = @OrganizationId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetEmailSuffixesByInstanceId]  
(  
 @InstanceId uniqueidentifier  
)  
AS  
BEGIN  
 SET NOCOUNT OFF;  
  
 SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName   
 FROM dbo.Mc_EmailSuffix  
 WHERE (InstanceId = @InstanceId);  
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetEmailSuffix]
(
	@EmailSuffixId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName 
	FROM dbo.Mc_EmailSuffix
	WHERE (EmailSuffixId = @EmailSuffixId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetEmailSuffixes]  
(  
 @OrganizationId uniqueidentifier,  
 @InstanceId uniqueidentifier,  
 @EmailSuffixName nvarchar(255)  
)  
AS  
BEGIN  
 SET NOCOUNT OFF;  
  
 SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName   
 FROM dbo.Mc_EmailSuffix  
 WHERE  
  ((@OrganizationId IS NULL) OR (OrganizationId = @OrganizationId)) AND  
  ((@InstanceId IS NULL) OR (InstanceId = @InstanceId)) AND 
  ((@EmailSuffixName IS NOT NULL) 
   AND (',' + EmailSuffixName + ',' like '%,' + @EmailSuffixName + ',%') 
  );  
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetLogin]
(
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT LoginId, FirstName, LastName, LoginName, [Password], ProfileUpdated, Deleted, SessionId, Token
	FROM dbo.Mc_Login
	WHERE LoginId = @LoginId;
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetLoginByDomainName]      
(      
 @DomainName nvarchar(255),   
 @UserAlias nvarchar(255),   
 @FirstName nvarchar(255),   
 @LastName nvarchar(255)   
)      
AS      
BEGIN      
 SET NOCOUNT OFF;      
       
 SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.Password, ol.OrganizationId, ol.Active,   
  ol.LdapDomain as 'UserLdapDomain', ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId,   
  ol.LdapUserId, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword   
 FROM Mc_Login l   
  inner join Mc_OrganizationsLogins ol ON l.LoginId = ol.LoginId   
  inner join Mc_Organization o ON ol.OrganizationId = o.OrganizationId   
 WHERE l.Deleted = 0 AND o.Deleted = 0 AND o.Active = 1 AND   
  ((@DomainName is null) OR ((ol.LdapDomain = @DomainName) OR (ol.LdapDomainFull = @DomainName))) AND   
  (((@UserAlias is not null) AND (ol.LdapUserAlias = @UserAlias))   
  OR   
  (((@FirstName is not null) AND (@LastName is not null)) AND ((l.FirstName = @FirstName) AND (l.LastName = @LastName)))) 
 ORDER BY o.LdapServerAddress DESC;
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetLdapServerDetails]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword 
	FROM Mc_Organization 
	WHERE Deleted = 0 AND Active = 1 AND 
		OrganizationId = @OrganizationId;
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetInvitedLogin]
(
	@InvitedLoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT InvitedLoginId, LoginName, OrganizationId, GroupId, InvitedBy, CreatedTime
	FROM dbo.Mc_InvitedLogin
	WHERE (InvitedLoginId = @InvitedLoginId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetInvitedLoginsByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT InvitedLoginId, LoginName, OrganizationId, GroupId, InvitedBy, CreatedTime
	FROM dbo.Mc_InvitedLogin
	WHERE (OrganizationId = @OrganizationId);
END

GO
