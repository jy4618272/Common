CREATE PROCEDURE [dbo].[Mc_GetAllRules]
	@RuleEngineId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier = NULL
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [RuleId], [RuleEngineId], [OrganizationId], [InstanceId], [Name], [DisplayName], [UsedQty], [LastUsedUser], [LastUsedDate], [CreatedBy], [CreatedDate], [OrderNumber], [Active]
	FROM [dbo].[Mc_Rule]
	WHERE [RuleEngineId] = @RuleEngineId
		AND [OrganizationId] = @OrganizationId
		AND (	(@InstanceId IS NULL AND [InstanceId] IS NULL)
				OR ([InstanceId] = @InstanceId))
	ORDER BY [OrderNumber] ASC;
END
