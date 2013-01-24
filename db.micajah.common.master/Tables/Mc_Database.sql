CREATE TABLE [dbo].[Mc_Database] (
    [DatabaseId]       UNIQUEIDENTIFIER CONSTRAINT [DF_Mc_Database_DatabaseId] DEFAULT (newid()) NOT NULL,
    [Name]             NVARCHAR (255)   NOT NULL,
    [Description]      NVARCHAR (1024)  CONSTRAINT [DF_Mc_Database_Description] DEFAULT (N'') NOT NULL,
    [UserName]         NVARCHAR (255)   NOT NULL,
    [Password]         NVARCHAR (255)   NOT NULL,
    [DatabaseServerId] UNIQUEIDENTIFIER NOT NULL,
    [Private]          BIT              CONSTRAINT [DF_Mc_Database_Private] DEFAULT ((0)) NOT NULL,
    [Deleted]          BIT              CONSTRAINT [DF_Mc_Database_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Mc_Database] PRIMARY KEY CLUSTERED ([DatabaseId] ASC),
    CONSTRAINT [FK_Mc_Database_Mc_DatabaseServer] FOREIGN KEY ([DatabaseServerId]) REFERENCES [dbo].[Mc_DatabaseServer] ([DatabaseServerId])
);

