CREATE PROCEDURE [dbo].[Mc_InsertEntityFieldListValue]
(
	@EntityFieldListValueId uniqueidentifier,
	@EntityFieldId uniqueidentifier,
	@Name nvarchar(255),
	@Value nvarchar(512),
	@Default bit,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_EntityFieldListsValues] ([EntityFieldListValueId], [EntityFieldId], [Name], [Value], [Default], [Active])
	VALUES (@EntityFieldListValueId, @EntityFieldId, @Name, @Value, @Default, @Active);

	IF @Default = 1
	BEGIN
		DECLARE @EntityFieldTypeId int;
		
		SELECT @EntityFieldTypeId = [EntityFieldTypeId]
		FROM [dbo].[Mc_EntityField]
		WHERE [EntityFieldId] = @EntityFieldId;

		IF @EntityFieldTypeId = 2
			UPDATE [dbo].[Mc_EntityFieldListsValues]
			SET [Default] = 0
			WHERE ([EntityFieldId] = @EntityFieldId) AND ([EntityFieldListValueId] <> @EntityFieldListValueId);
	END

	SELECT [EntityFieldListValueId], [EntityFieldId], [Name], [Value], [Default], [Active]
	FROM [dbo].[Mc_EntityFieldListsValues]
	WHERE (EntityFieldListValueId = @EntityFieldListValueId);
END