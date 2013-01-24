CREATE PROCEDURE [dbo].[Mc_UpdateEntityFieldValue]
(
	@EntityFieldValueId uniqueidentifier,
	@EntityFieldId uniqueidentifier,
	@LocalEntityId nvarchar(255),
	@Value nvarchar(MAX)
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	UPDATE [dbo].[Mc_EntityFieldsValues]
	SET [EntityFieldId] = @EntityFieldId, [LocalEntityId] = @LocalEntityId, [Value] = @Value
	WHERE (EntityFieldValueId = @EntityFieldValueId);

	SELECT [EntityFieldValueId], [EntityFieldId], [LocalEntityId], [Value]
	FROM [dbo].[Mc_EntityFieldsValues]
	WHERE (EntityFieldValueId = @EntityFieldValueId);
END