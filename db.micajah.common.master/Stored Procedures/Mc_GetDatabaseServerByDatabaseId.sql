CREATE PROCEDURE [dbo].[Mc_GetDatabaseServerByDatabaseId]
(
	@DatabaseId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT ds.DatabaseServerId, ds.[Name], ds.InstanceName, ds.Port, ds.[Description], ds.WebsiteId, ds.Deleted 
	FROM dbo.Mc_Database AS db
	INNER JOIN dbo.Mc_DatabaseServer AS ds
		ON db.DatabaseServerId = ds.DatabaseServerId
	WHERE (db.DatabaseId = @DatabaseId);
END
