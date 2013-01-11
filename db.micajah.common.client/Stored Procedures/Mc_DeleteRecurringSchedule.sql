create procedure [dbo].[Mc_DeleteRecurringSchedule]
(
	@RecurringScheduleId uniqueidentifier
)
as
begin
	set nocount off;

	delete from [dbo].[Mc_RecurringSchedule]
	where RecurringScheduleId = @RecurringScheduleId;
END