CREATE TABLE [dbo].[Mc_Rule] (
    [RuleId]         UNIQUEIDENTIFIER CONSTRAINT [DF_Mc_RuleEngine_RoleId] DEFAULT (newid()) NOT NULL,
    [RuleEngineId]   UNIQUEIDENTIFIER NOT NULL,
    [OrganizationId] UNIQUEIDENTIFIER NOT NULL,
    [InstanceId]     UNIQUEIDENTIFIER NULL,
    [Name]           NVARCHAR (255)   CONSTRAINT [DF_Mc_RuleEngine_Name] DEFAULT (N'') NOT NULL,
    [DisplayName]    NVARCHAR (255)   CONSTRAINT [DF_Mc_RuleEngine_DisplayName] DEFAULT (N'') NOT NULL,
    [UsedQty]        INT              CONSTRAINT [DF_Mc_Rule_UsedQty] DEFAULT ((0)) NOT NULL,
    [LastUsedUser]   UNIQUEIDENTIFIER NULL,
    [LastUsedDate]   DATETIME         NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate]    DATETIME         NOT NULL,
    [OrderNumber]    INT              NOT NULL,
    [Active]         BIT              CONSTRAINT [DF_Mc_Rule_Active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Mc_RuleEngine] PRIMARY KEY CLUSTERED ([RuleId] ASC)
);

