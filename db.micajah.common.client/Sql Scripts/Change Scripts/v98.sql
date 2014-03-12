BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec('ALTER PROCEDURE [dbo].[Mc_GetUserByEmail]
(
	@Email nvarchar(255),
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, uo.OrganizationId, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	LEFT JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId = uo.UserId)
	WHERE (u.Email = @Email) AND (@OrganizationId IS NULL OR uo.OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec('ALTER PROCEDURE [dbo].[Mc_GetUser]
(
	@UserId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, uo.OrganizationId, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	LEFT JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId = uo.UserId)
	WHERE (u.UserId = @UserId) AND (@OrganizationId IS NULL OR uo.OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
