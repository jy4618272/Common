CREATE PROCEDURE [dbo].[Mc_GetOrganizationsByLdapDomain]
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
		WHERE (o.Deleted = 0) AND (o.Active = 1) AND (',' + o.LdapDomains + ',' like '%,' + @LdapDomain + ',%')
		ORDER BY o.LdapServerAddress DESC;
	END
	