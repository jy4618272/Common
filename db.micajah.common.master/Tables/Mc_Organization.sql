CREATE TABLE [dbo].[Mc_Organization] (
    [OrganizationId]       UNIQUEIDENTIFIER CONSTRAINT [DF_Mc_Organization_OrganizationId] DEFAULT (newid()) NOT NULL,
    [PseudoId]             VARCHAR (6)      CONSTRAINT [DF_Mc_Organization_PseudoId] DEFAULT ('') NOT NULL,
    [Name]                 NVARCHAR (255)   NOT NULL,
    [Description]          NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_Description] DEFAULT (N'') NOT NULL,
    [WebsiteUrl]           NVARCHAR (2048)  CONSTRAINT [DF_Mc_Organization_WebsiteUrl] DEFAULT (N'') NOT NULL,
    [DatabaseId]           UNIQUEIDENTIFIER NULL,
    [FiscalYearStartDay]   INT              NULL,
    [FiscalYearStartMonth] INT              NULL,
    [WeekStartsDay]        INT              NULL,
    [LdapServerAddress]    NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_LdapServerAddress] DEFAULT (N'') NOT NULL,
    [LdapServerPort]       NVARCHAR (50)    CONSTRAINT [DF_Mc_Organization_LdapServerPort] DEFAULT (N'') NOT NULL,
    [LdapDomain]           NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_LdapDomain] DEFAULT (N'') NOT NULL,
    [LdapUserName]         NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_LdapUserName] DEFAULT (N'') NOT NULL,
    [LdapPassword]         NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_LdapPassword] DEFAULT (N'') NOT NULL,
    [LdapDomains]          NVARCHAR (2048)  CONSTRAINT [DF_Mc_Organization_LdapDomains] DEFAULT (N'') NOT NULL,
    [ExpirationTime]       DATETIME         NULL,
    [GraceDays]            INT              CONSTRAINT [DF_Mc_Organization_GraceDays] DEFAULT ((0)) NOT NULL,
    [ExternalId]           NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_ExternalId] DEFAULT (N'') NOT NULL,
    [Active]               BIT              CONSTRAINT [DF_Mc_Organization_Active] DEFAULT ((1)) NOT NULL,
    [CanceledTime]         DATETIME         NULL,
    [Trial]                BIT              CONSTRAINT [DF_Mc_Organization_Trial] DEFAULT ((0)) NOT NULL,
    [Beta]                 BIT              CONSTRAINT [DF_Mc_Organization_Beta] DEFAULT ((0)) NOT NULL,
    [Deleted]              BIT              CONSTRAINT [DF_Mc_Organization_Deleted] DEFAULT ((0)) NOT NULL,
    [CreatedTime]          DATETIME         NULL,
    [Street]               NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_Street] DEFAULT (N'') NOT NULL,
    [Street2]              NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_Street2] DEFAULT (N'') NOT NULL,
    [City]                 NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_City] DEFAULT (N'') NOT NULL,
    [State]                NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_State] DEFAULT (N'') NOT NULL,
    [PostalCode]           NVARCHAR (20)    CONSTRAINT [DF_Mc_Organization_PostalCode] DEFAULT (N'') NOT NULL,
    [Country]              NVARCHAR (255)   CONSTRAINT [DF_Mc_Organization_Country] DEFAULT (N'') NOT NULL,
    [Currency]             CHAR (3)         CONSTRAINT [DF_Mc_Organization_Currency] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Mc_Organization] PRIMARY KEY CLUSTERED ([OrganizationId] ASC),
    CONSTRAINT [FK_Mc_Organization_Mc_Database] FOREIGN KEY ([DatabaseId]) REFERENCES [dbo].[Mc_Database] ([DatabaseId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Mc_Organization_PseudoId]
    ON [dbo].[Mc_Organization]([PseudoId] ASC);

