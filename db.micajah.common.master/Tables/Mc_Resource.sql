CREATE TABLE [dbo].[Mc_Resource] (
    [ResourceId]       UNIQUEIDENTIFIER CONSTRAINT [DF_Mc_Resource_ResourceId] DEFAULT (newid()) NOT NULL,
    [ParentResourceId] UNIQUEIDENTIFIER NULL,
    [LocalObjectType]  NVARCHAR (50)    NOT NULL,
    [LocalObjectId]    NVARCHAR (255)   NOT NULL,
    [Content]          VARBINARY (MAX)  NOT NULL,
    [ContentType]      VARCHAR (255)    NULL,
    [Name]             NVARCHAR (255)   NULL,
    [Width]            INT              NULL,
    [Height]           INT              NULL,
    [Align]            INT              NULL,
    [Temporary]        BIT              CONSTRAINT [DF_Mc_Resource_Temporary] DEFAULT ((0)) NOT NULL,
    [CreatedTime]      DATETIME         NOT NULL,
    CONSTRAINT [PK_Mc_Resource] PRIMARY KEY CLUSTERED ([ResourceId] ASC),
    CONSTRAINT [FK_Mc_Resource_Mc_Resource] FOREIGN KEY ([ParentResourceId]) REFERENCES [dbo].[Mc_Resource] ([ResourceId])
);




GO
CREATE NONCLUSTERED INDEX [IX_Mc_Resource_LocalObjectType_LocalObjectId]
    ON [dbo].[Mc_Resource]([LocalObjectType] ASC, [LocalObjectId] ASC);

