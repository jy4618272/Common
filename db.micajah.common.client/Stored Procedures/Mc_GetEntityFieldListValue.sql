CREATE PROCEDURE [dbo].[Mc_GetEntityFieldListValue]
(
	@EntityFieldListValueId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [EntityFieldListValueId], [EntityFieldId], [Name], [Value], [Default], [Active]
	FROM [dbo].[Mc_EntityFieldListsValues]
	WHERE (EntityFieldListValueId = @EntityFieldListValueId);
END
