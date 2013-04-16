CREATE TABLE [dbo].[Mc_OrganizationsLdapGroups] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [OrganizationId]    UNIQUEIDENTIFIER NOT NULL,
    [DomainId]          UNIQUEIDENTIFIER NOT NULL,
    [Domain]            NVARCHAR (255)   NOT NULL,
    [ObjectGUID]        UNIQUEIDENTIFIER NOT NULL,
    [Name]              NVARCHAR (255)   NOT NULL,
    [DistinguishedName] NVARCHAR (2048)  NOT NULL,
    [CreatedTime]       DATETIME         NOT NULL,
    CONSTRAINT [PK_Mc_OrganizationsLdapGroups] PRIMARY KEY CLUSTERED ([Id] ASC)
);

