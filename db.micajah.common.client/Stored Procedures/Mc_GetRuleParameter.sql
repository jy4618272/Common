CREATE PROCEDURE [dbo].[Mc_GetRuleParameter]
(
	@RuleParameterId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF

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
