CREATE PROCEDURE [dbo].[Mc_DeleteEntityFieldValue]
(
	@EntityFieldValueId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM [dbo].[Mc_EntityFieldsValues]
	WHERE (EntityFieldValueId = @EntityFieldValueId);
END