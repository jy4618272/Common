CREATE TABLE [dbo].[Mc_CustomUrl] (
    [CustomUrlId]      UNIQUEIDENTIFIER CONSTRAINT [DF_Mc_CustomUrl_CustomUrlId] DEFAULT (newid()) NOT NULL,
    [OrganizationId]   UNIQUEIDENTIFIER NOT NULL,
    [InstanceId]       UNIQUEIDENTIFIER NULL,
    [FullCustomUrl]    NVARCHAR (1024)  CONSTRAINT [DF_Mc_CustomUrl_FullCustomUrl] DEFAULT ('') NOT NULL,
    [PartialCustomUrl] NVARCHAR (1024)  CONSTRAINT [DF_Mc_CustomUrl_PartialCustomUrl] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Mc_CustomUrl] PRIMARY KEY CLUSTERED ([CustomUrlId] ASC),
    CONSTRAINT [FK_Mc_CustomUrl_Mc_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])
);

