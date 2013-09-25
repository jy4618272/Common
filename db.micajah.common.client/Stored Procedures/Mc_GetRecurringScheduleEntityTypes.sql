CREATE procedure [dbo].[Mc_GetRecurringScheduleEntityTypes]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier = NULL
)
AS
BEGIN
	SET NOCOUNT OFF;

	DECLARE @Guid uniqueidentifier, @Date datetime;

	SET @Guid = '00000000-0000-0000-0000-000000000000';
	SET @Date = GETUTCDATE();

	SELECT DISTINCT
		@Guid AS [RecurringScheduleId], @OrganizationId AS [OrganizationId], @InstanceId AS [InstanceId], [LocalEntityType], N'' AS [LocalEntityId], N'' AS [Name]
		, @Date AS [StartDate], @Date AS [EndDate], N'' AS [RecurrenceRule], @Date AS [UpdatedTime], @Guid AS [UpdatedBy], 0 AS [Deleted]
	FROM [Mc_RecurringSchedule]
	WHERE OrganizationId = @OrganizationId AND Deleted = 0
		AND (@InstanceId IS NULL OR InstanceId IS NULL OR InstanceId = @InstanceId)
	ORDER BY [LocalEntityType];
END
