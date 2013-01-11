CREATE procedure [dbo].[Mc_GetRecurringScheduleById]
(
	@RecurringScheduleId uniqueidentifier
)
as
begin
	set NOCOUNT OFF;

	select [RecurringScheduleId]
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
	from [Mc_RecurringSchedule] as RS
	where RS.RecurringScheduleId = @RecurringScheduleId
	and RS.Deleted = 0;
	
END
