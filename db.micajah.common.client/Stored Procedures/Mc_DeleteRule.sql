CREATE PROCEDURE [dbo].[Mc_DeleteRule]
(
	@RuleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF

	DELETE FROM [Mc_Rule]
	WHERE [RuleId] = @RuleId
END
