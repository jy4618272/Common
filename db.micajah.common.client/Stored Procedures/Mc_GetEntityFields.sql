CREATE PROCEDURE [dbo].[Mc_GetEntityFields]
(
	@EntityId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataTypeId], [DefaultValue], [AllowDBNull], [Unique], [MaxLength], [MinValue], [MaxValue], [DecimalDigits], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active]
	FROM dbo.Mc_EntityField
	WHERE ([EntityId] = @EntityId) 
		AND (OrganizationId = @OrganizationId) 
		AND ((@InstanceId IS NULL AND InstanceId IS NULL) OR (InstanceId = @InstanceId))
		AND ((@Active IS NULL) OR (Active = @Active));
END
