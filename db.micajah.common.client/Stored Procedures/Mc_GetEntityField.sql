CREATE PROCEDURE [dbo].[Mc_GetEntityField]
(
	@EntityFieldId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataTypeId], [DefaultValue], [AllowDBNull], [Unique], [MaxLength], [MinValue], [MaxValue], [DecimalDigits], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active]
	FROM dbo.Mc_EntityField
	WHERE (EntityFieldId = @EntityFieldId);
END
