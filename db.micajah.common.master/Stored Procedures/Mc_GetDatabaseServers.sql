CREATE PROCEDURE [dbo].[Mc_GetDatabaseServers]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT DatabaseServerId, [Name], InstanceName, Port, [Description], WebsiteId, Deleted 
	FROM dbo.Mc_DatabaseServer 
	WHERE (Deleted = 0);
END
