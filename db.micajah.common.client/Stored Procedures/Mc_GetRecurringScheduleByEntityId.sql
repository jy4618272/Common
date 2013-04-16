CREATE procedure [dbo].[Mc_GetRecurringScheduleByEntityId]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier=null,
	@LocalEntityType nvarchar(50),
	@LocalEntityId  nvarchar(255)
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
	where RS.OrganizationId = @OrganizationId
	and RS.Deleted = 0
	and (	@InstanceId is null 
			or RS.InstanceId is null 
			or RS.InstanceId = @InstanceId)
	and RS.LocalEntityType = @LocalEntityType
	and RS.LocalEntityId = @LocalEntityId;
	
END
