
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
