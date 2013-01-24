CREATE PROCEDURE [dbo].[Mc_GetAnotherAdministrator]
(
	@UserId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT TOP 1 u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, u.TimeZoneId, u.TimeFormat, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	INNER JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId <> uo.UserId) AND (u.Deleted = 0) AND (uo.OrganizationId = @OrganizationId) AND (uo.OrganizationAdministrator = 1);
END
