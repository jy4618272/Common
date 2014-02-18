CREATE TABLE [dbo].[Mc_SettingsValues] (
    [SettingValueId] UNIQUEIDENTIFIER NOT NULL,
    [SettingId]      UNIQUEIDENTIFIER NOT NULL,
    [Value]          NVARCHAR (MAX)   NOT NULL,
    [OrganizationId] UNIQUEIDENTIFIER NULL,
    [InstanceId]     UNIQUEIDENTIFIER NULL,
    [GroupId]        UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Mc_SettingsValues] PRIMARY KEY CLUSTERED ([SettingValueId] ASC),
    CONSTRAINT [FK_Mc_SettingsValues_Mc_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Mc_Group] ([GroupId]),
    CONSTRAINT [FK_Mc_SettingsValues_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])
);






GO
CREATE NONCLUSTERED INDEX [IX_Mc_SettingsValues_InstanceId_SettingId]
    ON [dbo].[Mc_SettingsValues]([InstanceId] ASC, [SettingId] ASC);


