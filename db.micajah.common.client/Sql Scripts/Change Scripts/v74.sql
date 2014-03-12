BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetAllRules]
	@RuleEngineId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier = NULL
AS
BEGIN

	SET NOCOUNT ON

	SELECT [RuleId], [RuleEngineId], [OrganizationId], [InstanceId], [Name], [DisplayName], [UsedQty], [LastUsedUser], [LastUsedDate], [CreatedBy], [CreatedDate], [OrderNumber], [Active]
	FROM [dbo].[Mc_Rule]
	WHERE [RuleEngineId] = @RuleEngineId
		AND [OrganizationId] = @OrganizationId
		AND (	(@InstanceId IS NULL AND [InstanceId] IS NULL)
				OR ([InstanceId] = @InstanceId))
	ORDER BY [OrderNumber] ASC;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEntityFields]
(
	@EntityId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataTypeId], [DefaultValue], [AllowDBNull], [Unique], [MaxLength], [MinValue], [MaxValue], [DecimalDigits], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active]
	FROM dbo.Mc_EntityField
	WHERE ([EntityId] = @EntityId) 
		AND (OrganizationId = @OrganizationId) 
		AND ((@InstanceId IS NULL AND InstanceId IS NULL) OR (InstanceId = @InstanceId))
		AND ((@Active IS NULL) OR (Active = @Active));
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
