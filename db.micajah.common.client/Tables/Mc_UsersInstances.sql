CREATE TABLE [dbo].[Mc_UsersInstances] (
    [UserId]     UNIQUEIDENTIFIER NOT NULL,
    [InstanceId] UNIQUEIDENTIFIER NOT NULL,
    [Active]     BIT              CONSTRAINT [DF_Mc_UsersInstances_Active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Mc_UsersInstances] PRIMARY KEY CLUSTERED ([UserId] ASC, [InstanceId] ASC),
    CONSTRAINT [FK_Mc_UsersInstances_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId]),
    CONSTRAINT [FK_Mc_UsersInstances_Mc_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Mc_User] ([UserId])
);

