CREATE PROCEDURE [dbo].[Mc_GetDatabase]
(
	@DatabaseId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT DatabaseId, [Name], [Description], UserName, [Password], DatabaseServerId, [Private], Deleted 
	FROM dbo.Mc_Database
	WHERE (DatabaseId = @DatabaseId);
END
