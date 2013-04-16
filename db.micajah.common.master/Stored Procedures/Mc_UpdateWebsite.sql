
CREATE PROCEDURE [dbo].[Mc_UpdateWebsite]
(
	@WebsiteId uniqueidentifier,
	@Name nvarchar(255),
	@Url nvarchar(2048),
	@Description nvarchar(1024),
	@AdminContactInfo nvarchar(2048),
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Website
	SET [Name] = @Name, Url = @Url, [Description] = @Description, AdminContactInfo = @AdminContactInfo, Deleted = @Deleted 
	WHERE (WebsiteId = @WebsiteId);
	
	SELECT WebsiteId, [Name], Url, [Description], AdminContactInfo, Deleted 
	FROM dbo.Mc_Website 
	WHERE (WebsiteId = @WebsiteId);
END

