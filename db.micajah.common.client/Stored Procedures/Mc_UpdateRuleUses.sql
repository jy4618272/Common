CREATE PROCEDURE [dbo].[Mc_UpdateRuleUses]
(
	@RuleId uniqueidentifier,
	@LastUsedUser uniqueidentifier,
	@LastUsedDate datetime
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [Mc_Rule]
	SET
		[UsedQty] = [UsedQty] + 1,
		[LastUsedUser] = @LastUsedUser,
		[LastUsedDate] = @LastUsedDate
	WHERE
		[RuleId] = @RuleId;

	SELECT
		[RuleId]
	  ,[OrganizationId]
	  ,[InstanceId]
	  ,[Name]
	  ,[DisplayName]
	  ,[UsedQty]
	  ,[LastUsedUser]
	  ,[LastUsedDate]
	  ,[CreatedDate]
	  ,[CreatedBy]
	  ,[Active]
	FROM [Mc_Rule]
	WHERE
		([RuleId] = @RuleId);
END
