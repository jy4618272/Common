CREATE PROCEDURE [dbo].[Mc_InsertEntityFieldValue]
(
	@EntityFieldValueId uniqueidentifier,
	@EntityFieldId uniqueidentifier,
	@LocalEntityId nvarchar(255),
	@Value nvarchar(MAX)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_EntityFieldsValues] ([EntityFieldValueId], [EntityFieldId], [LocalEntityId], [Value])
	VALUES (@EntityFieldValueId, @EntityFieldId, @LocalEntityId, @Value);

	SELECT [EntityFieldValueId], [EntityFieldId], [LocalEntityId], [Value]
	FROM [dbo].[Mc_EntityFieldsValues]
	WHERE (EntityFieldValueId = @EntityFieldValueId);
END