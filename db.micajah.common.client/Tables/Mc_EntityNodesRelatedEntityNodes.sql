CREATE TABLE [dbo].[Mc_EntityNodesRelatedEntityNodes] (
    [EntityNodesRelatedEntityNodesId] UNIQUEIDENTIFIER NOT NULL,
    [EntityNodeId]                    UNIQUEIDENTIFIER NOT NULL,
    [RelatedEntityNodeId]             UNIQUEIDENTIFIER NOT NULL,
    [EntityId]                        UNIQUEIDENTIFIER NOT NULL,
    [RelationType]                    INT              NOT NULL,
    CONSTRAINT [PK_Mc_EntityNodesRelatedEntityNodes] PRIMARY KEY CLUSTERED ([EntityNodesRelatedEntityNodesId] ASC)
);

