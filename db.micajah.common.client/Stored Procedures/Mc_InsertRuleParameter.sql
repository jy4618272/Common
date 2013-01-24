CREATE PROCEDURE [dbo].[Mc_InsertRuleParameter]
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

	IF @RuleParameterId IS NULL
		 SET @RuleParameterId = NEWID()

	INSERT
	INTO [Mc_RuleParameters]
	(
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
	)
	VALUES
	(
		@RuleParameterId,
		@RuleId,
		@EntityNodeTypeId,
		@IsInputParameter,
		@IsEntity,
		@FieldName,
		@FullName,
		@TypeName,
		@Term,
		@Value
	)

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
