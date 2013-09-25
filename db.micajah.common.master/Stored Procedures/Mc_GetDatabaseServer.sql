CREATE PROCEDURE [dbo].[Mc_GetDatabaseServer]
(
	@DatabaseServerId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT DatabaseServerId, [Name], InstanceName, Port, [Description], WebsiteId, Deleted 
	FROM dbo.Mc_DatabaseServer 
	WHERE (DatabaseServerId = @DatabaseServerId);
END
