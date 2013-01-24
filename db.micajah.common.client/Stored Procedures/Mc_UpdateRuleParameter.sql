CREATE PROCEDURE [dbo].[Mc_UpdateRuleParameter]
(
	@RuleParameterId uniqueidentifier,
	@RuleId uniqueidentifier,
	@EntityNodeTypeId uniqueidentifier = NULL,
	@IsInputParameter bit,
	@IsEntity bit,
	@FieldName nvarchar(255),
	@FullName nvarchar(512),
	@TypeName nvarchar(255) = NULL,
	@Term nvarchar(50),
	@Value sql_variant
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [Mc_RuleParameters]
	SET
		[RuleId] = @RuleId,
		[EntityNodeTypeId] = @EntityNodeTypeId,
		[IsInputParameter] = @IsInputParameter,
		[IsEntity] = @IsEntity,
		[FieldName] = @FieldName,
		[FullName] = @FullName,
		[TypeName] = @TypeName,
		[Term] = @Term,
		[Value] = @Value
	WHERE
		[RuleParameterId] = @RuleParameterId

	SELECT
		[RuleParameterId],
		[RuleId],
		[EntityNodeTypeId],
		[IsInputParameter],
		[IsEntity],
		[FieldName],
		[FullName],
		[TypeName],
		[Term],
		[Value]
	FROM [Mc_RuleParameters]
	WHERE
		([RuleParameterId] = @RuleParameterId)
END
