CREATE procedure [dbo].[Mc_GetRecurringScheduleEntityTypes]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier=null
)
as
begin
	set NOCOUNT OFF;

	select distinct [LocalEntityType]
	from [Mc_RecurringSchedule] as RS
	where RS.OrganizationId = @OrganizationId
	and RS.Deleted = 0
	and (	@InstanceId is null 
			or RS.InstanceId is null 
			or RS.InstanceId = @InstanceId)
	order by [LocalEntityType] asc;
	
END
