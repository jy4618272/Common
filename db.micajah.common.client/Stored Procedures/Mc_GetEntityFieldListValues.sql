CREATE PROCEDURE [dbo].[Mc_GetEntityFieldListValues]
(
	@EntityFieldId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [EntityFieldListValueId], [EntityFieldId], [Name], [Value], [Default], [Active]
	FROM [dbo].[Mc_EntityFieldListsValues]
	WHERE (EntityFieldId = @EntityFieldId) AND ((@Active IS NULL) OR (Active = @Active));
END
