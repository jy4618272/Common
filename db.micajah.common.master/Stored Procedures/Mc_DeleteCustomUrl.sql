CREATE PROCEDURE [dbo].[Mc_DeleteCustomUrl]
(
	@CustomUrlId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_CustomUrl
	WHERE CustomUrlId = @CustomUrlId;
END