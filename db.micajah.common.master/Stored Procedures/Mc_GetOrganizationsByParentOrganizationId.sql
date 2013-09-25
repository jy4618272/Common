CREATE PROCEDURE [dbo].[Mc_GetOrganizationsByParentOrganizationId]
(
	@ParentOrganizationId uniqueidentifier
)
AS  
BEGIN  
	SET NOCOUNT OFF;  
	  
	SELECT OrganizationId, PseudoId, ParentOrganizationId, [Name], [Description], WebsiteUrl, DatabaseId  
		, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains  
		, ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime
		, Street, Street2, City, [State], PostalCode, Country, Currency, HowYouHearAboutUs
	FROM dbo.Mc_Organization
	WHERE (Deleted = 0) AND (((@ParentOrganizationId IS NULL) AND (ParentOrganizationId IS NULL)) OR (ParentOrganizationId = @ParentOrganizationId))
	ORDER BY [Name];
END