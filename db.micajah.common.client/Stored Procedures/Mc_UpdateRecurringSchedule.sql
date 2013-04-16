CREATE procedure [dbo].[Mc_UpdateRecurringSchedule]
(
	@RecurringScheduleId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier=null,
	@LocalEntityType nvarchar(50),
	@LocalEntityId nvarchar(255),
	@Name nvarchar(255),
	@StartDate datetime,
	@EndDate datetime,
	@RecurrenceRule nvarchar(1024),
	@UpdatedTime datetime,
	@UpdatedBy uniqueidentifier,
	@Deleted bit
)
as
begin
	set nocount off;

	if exists(select 'true' 
		from [dbo].[Mc_RecurringSchedule]
		where RecurringScheduleId = @RecurringScheduleId)
	begin
		update [dbo].[Mc_RecurringSchedule]
		set		[LocalEntityType] = @LocalEntityType
				,[LocalEntityId] = @LocalEntityId
				,[Name] = @Name
				,[StartDate] = @StartDate
				,[EndDate] = @EndDate
				,[RecurrenceRule] = @RecurrenceRule
				,[UpdatedTime] = @UpdatedTime
				,[UpdatedBy] = @UpdatedBy
				,[Deleted] = @Deleted
		where	[RecurringScheduleId] = @RecurringScheduleId;
	end
	else 
	begin
		insert into [dbo].[Mc_RecurringSchedule]
			   ([RecurringScheduleId]
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
			   ,[Deleted])
		 values
			   (@RecurringScheduleId
			   ,@OrganizationId
			   ,@InstanceId
			   ,@LocalEntityType
			   ,@LocalEntityId
			   ,@Name
			   ,@StartDate
			   ,@EndDate
			   ,@RecurrenceRule
			   ,@UpdatedTime
			   ,@UpdatedBy
			   ,@Deleted);
	end

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
	where RS.RecurringScheduleId = @RecurringScheduleId;
END