
CREATE PROCEDURE [dbo].[Mc_InsertDatabaseServer]
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

	INSERT INTO dbo.Mc_DatabaseServer (DatabaseServerId, [Name], InstanceName, Port, [Description], WebsiteId, Deleted) 
	VALUES (@DatabaseServerId, @Name, @InstanceName, @Port, @Description, @WebsiteId, @Deleted);
	
	SELECT @DatabaseServerId, [Name], InstanceName, Port, [Description], WebsiteId, Deleted 
	FROM dbo.Mc_DatabaseServer 
	WHERE (DatabaseServerId = @DatabaseServerId);
END

