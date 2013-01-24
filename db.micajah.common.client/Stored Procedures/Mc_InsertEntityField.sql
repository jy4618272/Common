CREATE PROCEDURE [dbo].[Mc_InsertEntityField]
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

	INSERT INTO [dbo].[Mc_EntityField] ([EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataTypeId], [DefaultValue], [AllowDBNull], [Unique], [MaxLength], [MinValue], [MaxValue], [DecimalDigits], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active])
	VALUES (@EntityFieldId, @EntityFieldTypeId, @Name, @Description, @DataTypeId, @DefaultValue, @AllowDBNull, @Unique, @MaxLength, @MinValue, @MaxValue, @DecimalDigits, @OrderNumber, @EntityId, @OrganizationId, @InstanceId, @Active);
	
	SELECT [EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataTypeId], [DefaultValue], [AllowDBNull], [Unique], [MaxLength], [MinValue], [MaxValue], [DecimalDigits], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active]
	FROM dbo.Mc_EntityField
	WHERE (EntityFieldId = @EntityFieldId);
END