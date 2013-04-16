CREATE TABLE [dbo].[Mc_EmailSuffix] (
    [EmailSuffixId]   UNIQUEIDENTIFIER CONSTRAINT [DF_Mc_EmailSuffix_EmailSuffixId] DEFAULT (newid()) NOT NULL,
    [OrganizationId]  UNIQUEIDENTIFIER NOT NULL,
    [InstanceId]      UNIQUEIDENTIFIER NULL,
    [EmailSuffixName] NVARCHAR (1024)  CONSTRAINT [DF_Mc_EmailSuffix_EmailSuffixName] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Mc_EmailSuffix] PRIMARY KEY CLUSTERED ([EmailSuffixId] ASC),
    CONSTRAINT [FK_Mc_EmailSuffix_Mc_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])
);

