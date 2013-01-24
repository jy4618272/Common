CREATE TABLE [dbo].[Mc_GroupsInstancesRoles] (
    [GroupId]    UNIQUEIDENTIFIER NOT NULL,
    [InstanceId] UNIQUEIDENTIFIER NOT NULL,
    [RoleId]     UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Mc_GroupsInstancesRoles] PRIMARY KEY CLUSTERED ([GroupId] ASC, [InstanceId] ASC),
    CONSTRAINT [FK_Mc_GroupsInstancesRoles_Mc_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Mc_Group] ([GroupId]),
    CONSTRAINT [FK_Mc_GroupsInstancesRoles_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])
);

