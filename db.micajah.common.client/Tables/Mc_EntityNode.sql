CREATE TABLE [dbo].[Mc_EntityNode] (
    [EntityNodeId]       UNIQUEIDENTIFIER NOT NULL,
    [ParentEntityNodeId] UNIQUEIDENTIFIER NULL,
    [Name]               NVARCHAR (255)   NOT NULL,
    [OrderNumber]        INT              CONSTRAINT [DF_Mc_EntityNode_OrderNumber] DEFAULT ((0)) NOT NULL,
    [OrganizationId]     UNIQUEIDENTIFIER NOT NULL,
    [InstanceId]         UNIQUEIDENTIFIER NULL,
    [EntityId]           UNIQUEIDENTIFIER NOT NULL,
    [EntityNodeTypeId]   UNIQUEIDENTIFIER NULL,
    [SubEntityId]        UNIQUEIDENTIFIER NULL,
    [SubEntityLocalId]   NVARCHAR (255)   NULL,
    [FullPath]           NVARCHAR (1024)  NOT NULL,
    [Deleted]            BIT              CONSTRAINT [DF_Mc_EntityNode_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Mc_EntityNode] PRIMARY KEY CLUSTERED ([EntityNodeId] ASC),
    CONSTRAINT [FK_Mc_EntityNode_Mc_EntityNode] FOREIGN KEY ([ParentEntityNodeId]) REFERENCES [dbo].[Mc_EntityNode] ([EntityNodeId]),
    CONSTRAINT [FK_Mc_EntityNode_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])
);

