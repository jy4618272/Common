CREATE TABLE [dbo].[Mc_RuleParameters] (
    [RuleParameterId]  UNIQUEIDENTIFIER CONSTRAINT [DF_Mc_RuleEngineParameters_RuleParameterId] DEFAULT (newid()) NOT NULL,
    [RuleId]           UNIQUEIDENTIFIER NOT NULL,
    [EntityNodeTypeId] UNIQUEIDENTIFIER NULL,
    [IsInputParameter] BIT              NOT NULL,
    [IsEntity]         BIT              NOT NULL,
    [FieldName]        NVARCHAR (255)   NOT NULL,
    [FullName]         NVARCHAR (512)   NOT NULL,
    [TypeName]         NVARCHAR (255)   NULL,
    [Term]             NVARCHAR (50)    CONSTRAINT [DF_Mc_RuleEngineParameters_Term] DEFAULT (N'=') NOT NULL,
    [Value]            SQL_VARIANT      NOT NULL,
    CONSTRAINT [PK_Mc_RuleEngineParameters] PRIMARY KEY CLUSTERED ([RuleParameterId] ASC),
    CONSTRAINT [FK_Mc_RuleEngineParameters_Mc_RuleEngine] FOREIGN KEY ([RuleId]) REFERENCES [dbo].[Mc_Rule] ([RuleId]) ON DELETE CASCADE
);

