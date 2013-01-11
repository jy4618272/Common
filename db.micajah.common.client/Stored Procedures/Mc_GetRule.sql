CREATE PROCEDURE [dbo].[Mc_GetRule]
(
	@RuleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT
		[RuleId]
	  ,[RuleEngineId]
	  ,[OrganizationId]
	  ,[InstanceId]
	  ,[Name]
	  ,[DisplayName]
	  ,[UsedQty]
	  ,[LastUsedUser]
	  ,[LastUsedDate]
	  ,[CreatedBy]
	  ,[CreatedDate]
	  ,[OrderNumber]
	  ,[Active]
	FROM [Mc_Rule]
	WHERE
		([RuleId] = @RuleId)
END
