CREATE PROCEDURE [dbo].[Mc_GetWebsite]
(
	@WebsiteId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT WebsiteId, [Name], Url, [Description], AdminContactInfo, Deleted 
	FROM dbo.Mc_Website 
	WHERE (WebsiteId = @WebsiteId);
END
