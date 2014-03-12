BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

CREATE TABLE [dbo].[Mc_RecurringSchedule] (
   [RecurringScheduleId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_MC_RecurringSchedule_RecurringScheduleId] DEFAULT (newid()),
   [OrganizationId] [uniqueidentifier] NOT NULL,
   [InstanceId] [uniqueidentifier] NULL,
   [LocalEntityType] [nvarchar] (50) NOT NULL CONSTRAINT [DF_Table_1_EntityType] DEFAULT (N''),
   [LocalEntityId] [nvarchar] (255) NOT NULL,
   [Name] [nvarchar] (255) NOT NULL,
   [StartDate] [datetime] NOT NULL,
   [EndDate] [datetime] NOT NULL,
   [RecurrenceRule] [nvarchar] (1024) NOT NULL,
   [UpdatedTime] [datetime] NOT NULL CONSTRAINT [DF_MC_RecurringSchedule_UpdatedTime] DEFAULT (getdate()),
   [UpdatedBy] [uniqueidentifier] NOT NULL,
   [Deleted] [bit] NOT NULL CONSTRAINT [DF_MC_RecurringSchedule_Deleted] DEFAULT ((0))
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_RecurringSchedule] ADD CONSTRAINT [PK_MC_RecurringSchedule] PRIMARY KEY CLUSTERED ([RecurringScheduleId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 exec('create procedure [dbo].[Mc_DeleteRecurringSchedule]
(
	@RecurringScheduleId uniqueidentifier
)
as
begin
	set nocount off;

	delete from [dbo].[Mc_RecurringSchedule]
	where RecurringScheduleId = @RecurringScheduleId;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 exec('CREATE procedure [dbo].[Mc_GetRecurringScheduleByEntityId]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier=null,
	@LocalEntityType nvarchar(50),
	@LocalEntityId  nvarchar(255)
)
as
begin
	set nocount on;

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
	
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 exec('CREATE procedure [dbo].[Mc_GetRecurringScheduleByEntityType]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier=null,
	@LocalEntityType nvarchar(50)
)
as
begin
	set nocount on;

	select [RecurringScheduleId]
		  ,[OrganizationId]
		  ,[InstanceId]
		  ,[LocalEntityType]
		  ,[LocalEntityId]
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
	and RS.LocalEntityType = @LocalEntityType;
	
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 exec('CREATE procedure [dbo].[Mc_GetRecurringScheduleById]
(
	@RecurringScheduleId uniqueidentifier
)
as
begin
	set nocount on;

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
	
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 exec('CREATE procedure [dbo].[Mc_GetRecurringScheduleByName]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier=null,
	@Name nvarchar(255)
)
as
begin
	set nocount on;

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
	and RS.[Name] = @Name;
	
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 exec('create procedure [dbo].[Mc_GetRecurringScheduleByRecurrenceRule]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier=null,
	@RecurrenceRule nvarchar(1024)
)
as
begin
	set nocount on;

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
	and RS.[RecurrenceRule] = @RecurrenceRule;
	
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 exec('create procedure [dbo].[Mc_GetRecurringScheduleEntityTypes]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier=null
)
as
begin
	set nocount on;

	select distinct [LocalEntityType]
	from [Mc_RecurringSchedule] as RS
	where RS.OrganizationId = @OrganizationId
	and RS.Deleted = 0
	and (	@InstanceId is null 
			or RS.InstanceId is null 
			or RS.InstanceId = @InstanceId)
	order by [LocalEntityType] asc;
	
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 exec('CREATE procedure [dbo].[Mc_GetRecurringSchedules]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier=null
)
as
begin
	set nocount on;

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
			or RS.InstanceId = @InstanceId);
	
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 exec('CREATE procedure [dbo].[Mc_UpdateRecurringSchedule]
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

	if exists(select ''true'' 
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
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
