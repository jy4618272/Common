CREATE PROCEDURE [dbo].[Mc_GetOrganizations]
(
	@Deleted bit
)
AS  
BEGIN  
	SET NOCOUNT OFF;  
	  
	SELECT OrganizationId, PseudoId, ParentOrganizationId, [Name], [Description], WebsiteUrl, DatabaseId  
		, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains  
		, ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime
		, Street, Street2, City, [State], PostalCode, Country, Currency, HowYouHearAboutUs, GoogleAdminAuthToken
	FROM dbo.Mc_Organization
	WHERE (@Deleted IS NULL) OR (Deleted = @Deleted)
	ORDER BY [Name];
END