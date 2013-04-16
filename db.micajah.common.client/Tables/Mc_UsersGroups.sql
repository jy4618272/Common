CREATE TABLE [dbo].[Mc_UsersGroups] (
    [UserId]  UNIQUEIDENTIFIER NOT NULL,
    [GroupId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Mc_UsersGroups] PRIMARY KEY CLUSTERED ([UserId] ASC, [GroupId] ASC),
    CONSTRAINT [FK_Mc_UsersGroups_Mc_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Mc_Group] ([GroupId]),
    CONSTRAINT [FK_Mc_UsersGroups_Mc_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Mc_User] ([UserId])
);

