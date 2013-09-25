CREATE PROCEDURE [dbo].[Mc_GetWebsiteByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT w.WebsiteId, w.[Name], w.Url, w.[Description], w.AdminContactInfo, w.Deleted 
	FROM dbo.Mc_Organization AS o
	INNER JOIN dbo.Mc_Database AS db
		ON o.DatabaseId = db.DatabaseId
	INNER JOIN dbo.Mc_DatabaseServer AS ds
		ON db.DatabaseServerId = ds.DatabaseServerId
	INNER JOIN dbo.Mc_Website AS w
		ON ds.WebsiteId = w.WebsiteId
	WHERE o.OrganizationId = @OrganizationId;
END
