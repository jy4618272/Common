CREATE PROCEDURE [dbo].[Mc_InsertGroupMappings]
(
	@GroupId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@GroupName nvarchar(255),
	@LdapDomainId uniqueidentifier,
	@LdapDomainName nvarchar(255),
	@LdapGroupId uniqueidentifier,
	@LdapGroupName nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_GroupMappings (GroupId, OrganizationId, [GroupName], LdapDomainId, [LdapDomainName], LdapGroupId, [LdapGroupName]) 
	VALUES (@GroupId, @OrganizationId, @GroupName, @LdapDomainId, @LdapDomainName, @LdapGroupId, @LdapGroupName);
	
	SELECT GroupId, OrganizationId, [GroupName], LdapDomainId, [LdapDomainName], LdapGroupId, [LdapGroupName] 
	FROM dbo.Mc_GroupMappings 
	WHERE (GroupId = @GroupId);
END