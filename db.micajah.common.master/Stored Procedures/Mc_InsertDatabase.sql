CREATE PROCEDURE [dbo].[Mc_InsertDatabase]
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

	INSERT INTO dbo.Mc_Database (DatabaseId, [Name], [Description], UserName, [Password], DatabaseServerId, [Private], Deleted) 
	VALUES (@DatabaseId, @Name, @Description, @UserName, @Password, @DatabaseServerId, @Private, @Deleted);
	
	SELECT DatabaseId, [Name], [Description], UserName, [Password], DatabaseServerId, [Private], Deleted 
	FROM dbo.Mc_Database 
	WHERE (DatabaseId = @DatabaseId);
END
