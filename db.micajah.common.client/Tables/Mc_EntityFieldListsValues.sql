CREATE TABLE [dbo].[Mc_EntityFieldListsValues] (
    [EntityFieldListValueId] UNIQUEIDENTIFIER NOT NULL,
    [EntityFieldId]          UNIQUEIDENTIFIER NOT NULL,
    [Name]                   NVARCHAR (255)   NOT NULL,
    [Value]                  NVARCHAR (512)   NOT NULL,
    [Default]                BIT              CONSTRAINT [DF_Mc_EntityFieldListsValues_Default] DEFAULT ((0)) NOT NULL,
    [Active]                 BIT              CONSTRAINT [DF_Mc_EntityFieldListsValues_Active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Mc_EntityFieldListsValues] PRIMARY KEY CLUSTERED ([EntityFieldListValueId] ASC),
    CONSTRAINT [FK_Mc_EntityFieldListsValues_Mc_EntityField] FOREIGN KEY ([EntityFieldId]) REFERENCES [dbo].[Mc_EntityField] ([EntityFieldId])
);

