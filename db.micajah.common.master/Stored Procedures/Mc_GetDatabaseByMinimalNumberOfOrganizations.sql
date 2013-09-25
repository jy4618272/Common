CREATE PROCEDURE [dbo].[Mc_GetDatabaseByMinimalNumberOfOrganizations]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT TOP 1 DatabaseId, [Name], [Description], UserName, [Password], DatabaseServerId, [Private], Deleted
		, (SELECT COUNT(1) FROM dbo.Mc_Organization AS o WHERE o.DatabaseId = db.DatabaseId) AS OrganizationsCount
	FROM dbo.Mc_Database AS db
	ORDER BY OrganizationsCount;
END
