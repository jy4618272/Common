CREATE PROCEDURE [dbo].[Mc_UpdateRule]
(
	@RuleId uniqueidentifier,
	@RuleEngineId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier = NULL,
	@Name nvarchar(255),
	@DisplayName nvarchar(255),
	@UsedQty int,
	@LastUsedUser uniqueidentifier,
	@LastUsedDate datetime,
	@CreatedBy uniqueidentifier,
	@CreatedDate datetime,
	@OrderNumber int,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [Mc_Rule]
	SET
		[RuleEngineId] = @RuleEngineId,
		[OrganizationId] = @OrganizationId,
		[InstanceId] = @InstanceId,
		[Name] = @Name,
		[DisplayName] = @DisplayName,
		[UsedQty] = @UsedQty,
		[LastUsedUser] = @LastUsedUser,
		[LastUsedDate] = @LastUsedDate,
		[CreatedDate] = @CreatedDate,
		[CreatedBy] = @CreatedBy,
		[OrderNumber] = @OrderNumber,
		[Active] = @Active
	WHERE
		[RuleId] = @RuleId;

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
	  ,[CreatedDate]
	  ,[CreatedBy]
	  ,[OrderNumber]
	  ,[Active]
	FROM [Mc_Rule]
	WHERE
		([RuleId] = @RuleId);
END
