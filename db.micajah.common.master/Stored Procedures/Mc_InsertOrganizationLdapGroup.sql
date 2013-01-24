
CREATE PROCEDURE [dbo].[Mc_InsertOrganizationLdapGroup]
(
	@Id uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@DomainId uniqueidentifier,	
	@Domain nvarchar(255),
	@ObjectGUID uniqueidentifier,
	@Name nvarchar(255),
	@DistinguishedName nvarchar(2048),
	@CreatedTime datetime
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_OrganizationsLdapGroups]([Id],[OrganizationId],[DomainId], [Domain],[ObjectGUID],[Name],[DistinguishedName],[CreatedTime])
    VALUES(@Id,@OrganizationId, @DomainId, @Domain, @ObjectGUID, @Name, @DistinguishedName, @CreatedTime)
END
