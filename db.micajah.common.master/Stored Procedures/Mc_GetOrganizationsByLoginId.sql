CREATE PROCEDURE [dbo].[Mc_GetOrganizationsByLoginId]
	(  
		@LoginId uniqueidentifier
	)
	AS
	BEGIN
		SET NOCOUNT OFF;

		SELECT o.OrganizationId, o.PseudoId, o.[Name], o.[Description], o.WebsiteUrl, o.DatabaseId
			, o.FiscalYearStartMonth, o.FiscalYearStartDay, o.WeekStartsDay, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword, o.LdapDomains
			, o.ExpirationTime, o.GraceDays, o.ExternalId, o.Active, o.CanceledTime, o.Trial, o.Beta, o.Deleted, o.CreatedTime
			, o.Street, o.Street2, o.City, o.[State], o.PostalCode, o.Country, o.Currency
		FROM dbo.Mc_OrganizationsLogins AS ol
		INNER JOIN dbo.Mc_Organization AS o
			ON (ol.OrganizationId = o.OrganizationId) AND (ol.LoginId = @LoginId) AND (o.Deleted = 0) AND (o.Active = 1) AND (ol.Active = 1);
	END
	