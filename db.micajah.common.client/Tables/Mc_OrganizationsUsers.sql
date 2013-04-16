CREATE TABLE [dbo].[Mc_OrganizationsUsers] (
    [OrganizationId]            UNIQUEIDENTIFIER NOT NULL,
    [UserId]                    UNIQUEIDENTIFIER NOT NULL,
    [OrganizationAdministrator] BIT              CONSTRAINT [DF_Mc_OrganizationsUsers_OrganizationAdministrator] DEFAULT ((0)) NOT NULL,
    [Active]                    BIT              CONSTRAINT [DF_Mc_OrganizationsUsers_Active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Mc_OrganizationsUsers] PRIMARY KEY CLUSTERED ([OrganizationId] ASC, [UserId] ASC),
    CONSTRAINT [FK_Mc_OrganizationsUsers_Mc_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Mc_User] ([UserId])
);

