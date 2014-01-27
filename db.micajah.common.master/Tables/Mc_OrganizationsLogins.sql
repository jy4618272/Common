CREATE TABLE [dbo].[Mc_OrganizationsLogins] (
    [OrganizationId]            UNIQUEIDENTIFIER NOT NULL,
    [LoginId]                   UNIQUEIDENTIFIER NOT NULL,
    [OrganizationAdministrator] BIT              CONSTRAINT [DF_Mc_OrganizationsLogins_OrganizationAdministrator] DEFAULT ((0)) NOT NULL,
    [Active]                    BIT              CONSTRAINT [DF_Mc_OrganizationsLogins_Active] DEFAULT ((1)) NOT NULL,
    [LdapDomain]                NVARCHAR (255)   NULL,
    [LdapDomainFull]            NVARCHAR (255)   CONSTRAINT [DF_Mc_OrganizationsLogins_LdapDomainFull] DEFAULT (N'') NOT NULL,
    [LdapUserAlias]             NVARCHAR (255)   NULL,
    [LdapUPN]                   NVARCHAR (255)   CONSTRAINT [DF_Mc_OrganizationsLogins_LdapUPN] DEFAULT (N'') NOT NULL,
    [LdapSecurityId]            NVARCHAR (255)   NULL,
    [LdapUserId]                UNIQUEIDENTIFIER NULL,
    [LdapOUPath]                NVARCHAR (255)   NULL,
    CONSTRAINT [PK_Mc_OrganizationsLogins] PRIMARY KEY CLUSTERED ([OrganizationId] ASC, [LoginId] ASC),
    CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Login] FOREIGN KEY ([LoginId]) REFERENCES [dbo].[Mc_Login] ([LoginId]),
    CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])
);




GO
CREATE NONCLUSTERED INDEX [IX_Mc_OrganizationsLogins_LoginId]
    ON [dbo].[Mc_OrganizationsLogins]([LoginId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Mc_OrganizationsLogins_LdapUserAlias]
    ON [dbo].[Mc_OrganizationsLogins]([LdapUserAlias] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Mc_OrganizationsLogins_LdapUPN]
    ON [dbo].[Mc_OrganizationsLogins]([LdapUPN] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Mc_OrganizationsLogins_LdapDomain]
    ON [dbo].[Mc_OrganizationsLogins]([LdapDomain] ASC);

