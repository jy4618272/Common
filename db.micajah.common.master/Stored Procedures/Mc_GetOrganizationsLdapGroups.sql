
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
