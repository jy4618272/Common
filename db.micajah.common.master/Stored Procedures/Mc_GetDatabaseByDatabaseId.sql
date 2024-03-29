﻿CREATE PROCEDURE [dbo].[Mc_GetDatabaseByDatabaseId]
(
	@DatabaseId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT d.DatabaseId, d.[Name], d.[Description], d.UserName, d.[Password], d.DatabaseServerId, d.[Private], d.Deleted
		, s.Name AS DatabaseServerName, s.InstanceName, s.Port
	FROM dbo.Mc_Database AS d
	INNER JOIN dbo.Mc_DatabaseServer AS s
		ON d.DatabaseServerId = s.DatabaseServerId AND s.Deleted = 0 AND d.DatabaseId = @DatabaseId AND d.Deleted = 0
	INNER JOIN dbo.Mc_Website AS w
		ON s.WebsiteId = w.WebsiteId AND w.Deleted = 0;
END
