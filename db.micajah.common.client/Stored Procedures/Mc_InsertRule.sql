CREATE PROCEDURE [dbo].[Mc_InsertRule]
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

	IF @RuleId IS NULL
		 SET @RuleId = NEWID()

	INSERT INTO [Mc_Rule]
	(
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
	)
	VALUES
	(
		@RuleId,
		@RuleEngineId,
		@OrganizationId,
		@InstanceId,
		@Name,
		@DisplayName,
		@UsedQty,
		@LastUsedUser,
		@LastUsedDate,
		@CreatedDate,
		@CreatedBy,
		@OrderNumber,
		@Active
	);
	
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
