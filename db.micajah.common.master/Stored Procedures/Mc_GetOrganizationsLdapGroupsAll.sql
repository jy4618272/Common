
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
