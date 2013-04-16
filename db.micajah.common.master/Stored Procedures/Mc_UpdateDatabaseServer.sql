
CREATE PROCEDURE [dbo].[Mc_UpdateDatabaseServer]
(
	@DatabaseServerId uniqueidentifier,
	@Name nvarchar(255),
	@InstanceName nvarchar(255),
	@Port int,
	@Description nvarchar(1024),
	@WebsiteId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_DatabaseServer 
	SET [Name] = @Name, InstanceName = @InstanceName, Port = @Port, [Description] = @Description, WebsiteId = @WebsiteId, Deleted = @Deleted 
	WHERE (DatabaseServerId = @DatabaseServerId);
	
	SELECT DatabaseServerId, [Name], InstanceName, Port, [Description], WebsiteId, Deleted 
	FROM dbo.Mc_DatabaseServer 
	WHERE (DatabaseServerId = @DatabaseServerId);
END

