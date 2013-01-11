CREATE TABLE [dbo].[Mc_DatabaseServer] (
    [DatabaseServerId] UNIQUEIDENTIFIER CONSTRAINT [DF_Mc_DatabaseServer_DatabaseServerId] DEFAULT (newid()) NOT NULL,
    [Name]             NVARCHAR (255)   NOT NULL,
    [InstanceName]     NVARCHAR (255)   CONSTRAINT [DF_Mc_SqlServer_InstanceName] DEFAULT (N'') NOT NULL,
    [Port]             INT              CONSTRAINT [DF_Mc_SqlServer_Port] DEFAULT ((0)) NOT NULL,
    [Description]      NVARCHAR (1024)  CONSTRAINT [DF_Mc_SqlServer_Description] DEFAULT (N'') NOT NULL,
    [WebsiteId]        UNIQUEIDENTIFIER NOT NULL,
    [Deleted]          BIT              CONSTRAINT [DF_Mc_SqlServer_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Mc_DatabaseServer] PRIMARY KEY CLUSTERED ([DatabaseServerId] ASC),
    CONSTRAINT [FK_Mc_DatabaseServer_Mc_Website] FOREIGN KEY ([WebsiteId]) REFERENCES [dbo].[Mc_Website] ([WebsiteId])
);

