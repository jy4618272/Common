CREATE PROCEDURE [dbo].[Mc_UpdateEntityField]
(
	@EntityFieldId uniqueidentifier,
	@EntityFieldTypeId int,
	@Name nvarchar(255),
	@Description nvarchar(255),
	@DataTypeId int,
	@DefaultValue nvarchar(512),
	@AllowDBNull bit,
	@Unique bit,
	@MaxLength int,
	@MinValue nvarchar(512),
	@MaxValue nvarchar(512),
	@DecimalDigits int,
	@OrderNumber int,
	@EntityId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [dbo].[Mc_EntityField]
	SET [EntityFieldTypeId] = @EntityFieldTypeId, [Name] = @Name, [Description] = @Description, [DataTypeId] = @DataTypeId, [DefaultValue] = @DefaultValue
		, [AllowDBNull] = @AllowDBNull, [Unique] = @Unique, [MaxLength] = @MaxLength, [MinValue] = @MinValue, [MaxValue] = @MaxValue, [DecimalDigits] = @DecimalDigits
		, [OrderNumber] = @OrderNumber, [EntityId] = @EntityId, [OrganizationId] = @OrganizationId, [InstanceId] = @InstanceId, [Active] = @Active
	WHERE (EntityFieldId = @EntityFieldId);
	
	SELECT [EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataTypeId], [DefaultValue], [AllowDBNull], [Unique], [MaxLength], [MinValue], [MaxValue], [DecimalDigits], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active]
	FROM dbo.Mc_EntityField
	WHERE (EntityFieldId = @EntityFieldId);
END