CREATE PROCEDURE [dbo].[Mc_GetWebsites]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT WebsiteId, [Name], Url, [Description], AdminContactInfo, Deleted 
	FROM dbo.Mc_Website 
	WHERE (Deleted = 0);
END
