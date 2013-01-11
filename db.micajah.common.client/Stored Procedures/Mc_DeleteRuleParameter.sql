CREATE PROCEDURE [dbo].[Mc_DeleteRuleParameter]
(
	@RuleParameterId uniqueidentifier
)
AS
BEGIN

	SET NOCOUNT OFF

	DELETE
	FROM [Mc_RuleParameters]
	WHERE
		[RuleParameterId] = @RuleParameterId
END
