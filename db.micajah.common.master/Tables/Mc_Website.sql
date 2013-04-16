CREATE TABLE [dbo].[Mc_Website] (
    [WebsiteId]        UNIQUEIDENTIFIER CONSTRAINT [DF_Mc_Website_WebsiteId] DEFAULT (newid()) NOT NULL,
    [Name]             NVARCHAR (255)   NOT NULL,
    [Url]              NVARCHAR (2048)  NOT NULL,
    [Description]      NVARCHAR (1024)  CONSTRAINT [DF_Mc_Website_Description] DEFAULT ('') NOT NULL,
    [AdminContactInfo] NVARCHAR (2048)  CONSTRAINT [DF_Mc_Website_AdminContactInfo] DEFAULT (N'') NOT NULL,
    [Deleted]          BIT              CONSTRAINT [DF_Mc_Website_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Mc_Website] PRIMARY KEY CLUSTERED ([WebsiteId] ASC)
);

