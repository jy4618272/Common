CREATE TABLE [dbo].[Mc_Instance] (
    [InstanceId]       UNIQUEIDENTIFIER CONSTRAINT [DF_Mc_Instance_InstanceId] DEFAULT (newid()) NOT NULL,
    [PseudoId]         VARCHAR (6)      CONSTRAINT [DF_Mc_Instance_PseudoId] DEFAULT ('') NOT NULL,
    [OrganizationId]   UNIQUEIDENTIFIER NOT NULL,
    [Name]             NVARCHAR (255)   NOT NULL,
    [Description]      NVARCHAR (1024)  CONSTRAINT [DF_Mc_Instance_Description] DEFAULT (N'') NOT NULL,
    [EnableSignUpUser] BIT              CONSTRAINT [DF_Mc_Instance_EnableSignUpUser] DEFAULT ((0)) NOT NULL,
    [ExternalId]       NVARCHAR (255)   CONSTRAINT [DF_Mc_Instance_ExternalId] DEFAULT (N'') NOT NULL,
    [WorkingDays]      CHAR (7)         CONSTRAINT [DF_Mc_Instance_WorkingDays] DEFAULT ((1111100)) NOT NULL,
    [Active]           BIT              CONSTRAINT [DF_Mc_Instance_Active] DEFAULT ((1)) NOT NULL,
    [CanceledTime]     DATETIME         NULL,
    [Trial]            BIT              CONSTRAINT [DF_Mc_Instance_Trial] DEFAULT ((0)) NOT NULL,
    [Beta]             BIT              CONSTRAINT [DF_Mc_Instance_Beta] DEFAULT ((0)) NOT NULL,
    [Deleted]          BIT              CONSTRAINT [DF_Mc_Instance_Deleted] DEFAULT ((0)) NOT NULL,
    [CreatedTime]      DATETIME         NULL,
    [TimeZoneId]       NVARCHAR (100)   CONSTRAINT [DF_Mc_Instance_TimeZoneId] DEFAULT ('') NOT NULL,
    [TimeFormat]       INT              CONSTRAINT [DF_Mc_Instance_TimeFormat] DEFAULT ((0)) NOT NULL,
    [DateFormat]       INT              CONSTRAINT [DF_Mc_Instance_DateFormat] DEFAULT ((0)) NOT NULL,
    [BillingPlan]      TINYINT          CONSTRAINT [DF_Mc_Instance_BillingPlan] DEFAULT ((0)) NOT NULL,
    [CreditCardStatus] TINYINT          CONSTRAINT [DF_Mc_Instance_CreditCardStatus] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Mc_Instance] PRIMARY KEY CLUSTERED ([InstanceId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Mc_Instance_OrganizationId]
    ON [dbo].[Mc_Instance]([OrganizationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Mc_Instance_PseudoId]
    ON [dbo].[Mc_Instance]([PseudoId] ASC);

