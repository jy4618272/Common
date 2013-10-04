CREATE PROCEDURE [dbo].[Mc_GetRecurringScheduleByEntityType]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier = null,
	@LocalEntityType nvarchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [RecurringScheduleId]
		  ,[OrganizationId]
		  ,[InstanceId]
		  ,[LocalEntityType]
		  ,[LocalEntityId]
		  ,[Name]
		  ,[StartDate]
		  ,[EndDate]
		  ,[RecurrenceRule]
		  ,[UpdatedTime]
		  ,[UpdatedBy]
		  ,[Deleted]
	FROM [Mc_RecurringSchedule] AS RS
	WHERE RS.OrganizationId = @OrganizationId AND RS.Deleted = 0
		AND (@InstanceId IS NULL OR RS.InstanceId IS NULL OR RS.InstanceId = @InstanceId)
		AND RS.LocalEntityType = @LocalEntityType;
END
