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
