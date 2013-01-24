CREATE TABLE [dbo].[Mc_RecurringSchedule] (
    [RecurringScheduleId] UNIQUEIDENTIFIER CONSTRAINT [DF_MC_RecurringSchedule_RecurringScheduleId] DEFAULT (newid()) NOT NULL,
    [OrganizationId]      UNIQUEIDENTIFIER NOT NULL,
    [InstanceId]          UNIQUEIDENTIFIER NULL,
    [LocalEntityType]     NVARCHAR (50)    CONSTRAINT [DF_Table_1_EntityType] DEFAULT (N'') NOT NULL,
    [LocalEntityId]       NVARCHAR (255)   NOT NULL,
    [Name]                NVARCHAR (255)   NOT NULL,
    [StartDate]           DATETIME         NOT NULL,
    [EndDate]             DATETIME         NOT NULL,
    [RecurrenceRule]      NVARCHAR (1024)  NOT NULL,
    [UpdatedTime]         DATETIME         NOT NULL,
    [UpdatedBy]           UNIQUEIDENTIFIER NOT NULL,
    [Deleted]             BIT              CONSTRAINT [DF_MC_RecurringSchedule_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MC_RecurringSchedule] PRIMARY KEY CLUSTERED ([RecurringScheduleId] ASC)
);

