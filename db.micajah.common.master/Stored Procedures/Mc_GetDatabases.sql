CREATE PROCEDURE [dbo].[Mc_GetDatabases]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT DatabaseId, [Name], [Description], UserName, [Password], DatabaseServerId, [Private], Deleted 
	FROM dbo.Mc_Database
	WHERE (Deleted = 0);
END
