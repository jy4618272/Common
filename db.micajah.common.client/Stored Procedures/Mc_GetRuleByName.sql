CREATE PROCEDURE [dbo].[Mc_GetRuleByName]
(
	@Name nvarchar(255),
	@OrganizationId uniqueidentifier
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
	WHERE [Name] = @Name
	AND [OrganizationId] = @OrganizationId;
	
END
