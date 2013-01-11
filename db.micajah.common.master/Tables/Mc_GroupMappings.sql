CREATE TABLE [dbo].[Mc_GroupMappings] (
    [GroupId]        UNIQUEIDENTIFIER NOT NULL,
    [OrganizationId] UNIQUEIDENTIFIER NOT NULL,
    [GroupName]      NVARCHAR (255)   NOT NULL,
    [LdapDomainId]   UNIQUEIDENTIFIER NOT NULL,
    [LdapDomainName] NVARCHAR (255)   NOT NULL,
    [LdapGroupId]    UNIQUEIDENTIFIER NOT NULL,
    [LdapGroupName]  NVARCHAR (255)   NOT NULL,
    CONSTRAINT [PK_Mc_GroupMappings] PRIMARY KEY CLUSTERED ([GroupId] ASC, [LdapDomainId] ASC, [LdapGroupId] ASC)
);

