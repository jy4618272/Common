CREATE TABLE [dbo].[Mc_EntityNodeType] (
    [EntityNodeTypeId] UNIQUEIDENTIFIER NOT NULL,
    [EntityId]         UNIQUEIDENTIFIER NOT NULL,
    [Name]             NVARCHAR (255)   NOT NULL,
    [OrderNumber]      INT              NOT NULL,
    [OrganizationId]   UNIQUEIDENTIFIER NOT NULL,
    [InstanceId]       UNIQUEIDENTIFIER NULL,
    [Deleted]          BIT              CONSTRAINT [DF_Mc_EntityNodeType_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Mc_EntityNodeType] PRIMARY KEY CLUSTERED ([EntityNodeTypeId] ASC),
    CONSTRAINT [FK_Mc_EntityNodeType_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])
);

