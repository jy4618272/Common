CREATE procedure [dbo].[Mc_UpdateRuleOrder]
(
	@RuleId uniqueidentifier,
	@OrderNumber int
)
as
begin

	set NOCOUNT OFF

	UPDATE [Mc_Rule]
	SET [OrderNumber] = @OrderNumber
	WHERE [RuleId] = @RuleId;
	
end
