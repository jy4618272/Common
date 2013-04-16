CREATE TABLE [dbo].[Mc_Group] (
    [GroupId]        UNIQUEIDENTIFIER NOT NULL,
    [OrganizationId] UNIQUEIDENTIFIER NOT NULL,
    [Name]           NVARCHAR (255)   NOT NULL,
    [Description]    NVARCHAR (1024)  CONSTRAINT [DF_Mc_Group_Description] DEFAULT (N'') NOT NULL,
    [BuiltIn]        BIT              CONSTRAINT [DF_Mc_Group_BuiltIn] DEFAULT ((0)) NOT NULL,
    [Deleted]        BIT              CONSTRAINT [DF_Mc_Group_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Mc_Group] PRIMARY KEY CLUSTERED ([GroupId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Mc_Group_OrganizationId]
    ON [dbo].[Mc_Group]([OrganizationId] ASC);

