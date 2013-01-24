CREATE PROCEDURE [dbo].[Mc_UpdateDatabase]
(
	@DatabaseId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@UserName nvarchar(255),
	@Password nvarchar(255),
	@DatabaseServerId uniqueidentifier,
	@Private bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Database 
	SET [Name] = @Name, [Description] = @Description, UserName = @UserName, [Password] = @Password, DatabaseServerId = @DatabaseServerId, [Private] = @Private, Deleted = @Deleted 
	WHERE ([DatabaseId] = @DatabaseId);
	
	SELECT DatabaseId, [Name], [Description], UserName, [Password], DatabaseServerId, [Private], Deleted 
	FROM dbo.Mc_Database 
	WHERE (DatabaseId = @DatabaseId);
END
