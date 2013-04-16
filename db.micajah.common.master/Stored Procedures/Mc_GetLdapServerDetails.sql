CREATE PROCEDURE [dbo].[Mc_GetLdapServerDetails]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword 
	FROM Mc_Organization 
	WHERE Deleted = 0 AND Active = 1 AND 
		OrganizationId = @OrganizationId;
END
