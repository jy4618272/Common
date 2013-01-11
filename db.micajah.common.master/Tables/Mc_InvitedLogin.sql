CREATE TABLE [dbo].[Mc_InvitedLogin] (
    [InvitedLoginId] UNIQUEIDENTIFIER NOT NULL,
    [LoginName]      NVARCHAR (255)   NOT NULL,
    [OrganizationId] UNIQUEIDENTIFIER NOT NULL,
    [GroupId]        VARCHAR (2056)   NOT NULL,
    [InvitedBy]      UNIQUEIDENTIFIER NOT NULL,
    [CreatedTime]    DATETIME         NOT NULL,
    CONSTRAINT [PK_Mc_InvitedLogin] PRIMARY KEY CLUSTERED ([InvitedLoginId] ASC),
    CONSTRAINT [FK_Mc_InvitedLogin_Mc_Login] FOREIGN KEY ([InvitedBy]) REFERENCES [dbo].[Mc_Login] ([LoginId]),
    CONSTRAINT [FK_Mc_InvitedLogin_Mc_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])
);

