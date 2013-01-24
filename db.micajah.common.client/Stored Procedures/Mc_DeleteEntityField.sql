CREATE PROCEDURE [dbo].[Mc_DeleteEntityField]
(
	@EntityFieldId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM [dbo].[Mc_EntityField]
	WHERE (EntityFieldId = @EntityFieldId);
END