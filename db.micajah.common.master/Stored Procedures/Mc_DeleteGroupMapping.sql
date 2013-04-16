CREATE PROCEDURE [dbo].[Mc_DeleteGroupMapping]
(
	@GroupId uniqueidentifier,
	@LdapDomainId uniqueidentifier,
	@LdapGroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_GroupMappings
	WHERE GroupId = @GroupId AND LdapDomainId = @LdapDomainId AND LdapGroupId = @LdapGroupId;
END