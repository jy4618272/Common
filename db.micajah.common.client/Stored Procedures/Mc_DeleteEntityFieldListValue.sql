CREATE PROCEDURE [dbo].[Mc_DeleteEntityFieldListValue]
(
	@EntityFieldListValueId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM [dbo].[Mc_EntityFieldListsValues]
	WHERE (EntityFieldListValueId = @EntityFieldListValueId);
END