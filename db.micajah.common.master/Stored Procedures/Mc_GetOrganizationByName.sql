﻿CREATE PROCEDURE [dbo].[Mc_GetOrganizationByName]
(
	@Name nvarchar (255)
)
AS  
BEGIN  
	SET NOCOUNT OFF;
	  
	SELECT OrganizationId, PseudoId, ParentOrganizationId, [Name], [Description], WebsiteUrl, DatabaseId
		, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains
		, ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime
		, Street, Street2, City, [State], PostalCode, Country, Currency, HowYouHearAboutUs, GoogleAdminAuthToken
	FROM dbo.Mc_Organization
	WHERE ([Name] = @Name);
END
	