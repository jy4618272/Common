CREATE TABLE [dbo].[Mc_GroupsInstancesActions] (
    [GroupId]    UNIQUEIDENTIFIER NOT NULL,
    [InstanceId] UNIQUEIDENTIFIER NOT NULL,
    [ActionId]   UNIQUEIDENTIFIER NOT NULL,
    [Enabled]    BIT              CONSTRAINT [DF_Mc_GroupsInstancesActions_Enabled] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Mc_GroupsInstancesActions] PRIMARY KEY CLUSTERED ([GroupId] ASC, [InstanceId] ASC, [ActionId] ASC),
    CONSTRAINT [FK_Mc_GroupsInstancesActions_Mc_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Mc_Group] ([GroupId]),
    CONSTRAINT [FK_Mc_GroupsInstancesActions_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])
);

