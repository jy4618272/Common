CREATE PROCEDURE [dbo].[Mc_GetAllRuleParameters]
(
	@RuleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

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
	WHERE [RuleId] = @RuleId
	ORDER BY [FieldName] ASC;
END
