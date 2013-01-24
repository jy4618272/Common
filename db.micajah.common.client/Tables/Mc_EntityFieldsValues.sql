CREATE TABLE [dbo].[Mc_EntityFieldsValues] (
    [EntityFieldValueId] UNIQUEIDENTIFIER CONSTRAINT [DF_Mc_EntityFieldsValues_EntityFieldValueId] DEFAULT (newid()) NOT NULL,
    [EntityFieldId]      UNIQUEIDENTIFIER NOT NULL,
    [LocalEntityId]      NVARCHAR (255)   NOT NULL,
    [Value]              NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Mc_EntityFieldsValues] PRIMARY KEY CLUSTERED ([EntityFieldValueId] ASC),
    CONSTRAINT [FK_Mc_EntityFieldsValues_Mc_EntityField] FOREIGN KEY ([EntityFieldId]) REFERENCES [dbo].[Mc_EntityField] ([EntityFieldId])
);

