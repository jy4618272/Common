
CREATE PROCEDURE [dbo].[Mc_InsertWebsite]
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

	INSERT INTO dbo.Mc_Website (WebsiteId, [Name], Url, [Description], [AdminContactInfo], [Deleted]) 
	VALUES (@WebsiteId, @Name, @Url, @Description, @AdminContactInfo, @Deleted);
	
	SELECT WebsiteId, [Name], Url, [Description], AdminContactInfo, Deleted 
	FROM dbo.Mc_Website 
	WHERE (WebsiteId = @WebsiteId);
END

