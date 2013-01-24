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
