IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_EntityField_Mc_Instance]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_EntityField]'))
ALTER TABLE [dbo].[Mc_EntityField] DROP CONSTRAINT [FK_Mc_EntityField_Mc_Instance]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_EntityField_AllowDBNull]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_EntityField] DROP CONSTRAINT [DF_Mc_EntityField_AllowDBNull]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_EntityField_Unique]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_EntityField] DROP CONSTRAINT [DF_Mc_EntityField_Unique]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_EntityField_MaxLength]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_EntityField] DROP CONSTRAINT [DF_Mc_EntityField_MaxLength]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_EntityField_DecimalDigits]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_EntityField] DROP CONSTRAINT [DF_Mc_EntityField_DecimalDigits]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_EntityField_OrderNumber]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_EntityField] DROP CONSTRAINT [DF_Mc_EntityField_OrderNumber]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_EntityField_Active]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_EntityField] DROP CONSTRAINT [DF_Mc_EntityField_Active]
END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_EntityFieldListsValues_Mc_EntityField]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_EntityFieldListsValues]'))
ALTER TABLE [dbo].[Mc_EntityFieldListsValues] DROP CONSTRAINT [FK_Mc_EntityFieldListsValues_Mc_EntityField]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_EntityFieldListsValues_Default]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_EntityFieldListsValues] DROP CONSTRAINT [DF_Mc_EntityFieldListsValues_Default]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_EntityFieldListsValues_Active]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_EntityFieldListsValues] DROP CONSTRAINT [DF_Mc_EntityFieldListsValues_Active]
END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_EntityFieldsValues_Mc_EntityField]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_EntityFieldsValues]'))
ALTER TABLE [dbo].[Mc_EntityFieldsValues] DROP CONSTRAINT [FK_Mc_EntityFieldsValues_Mc_EntityField]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_EntityFieldsValues_EntityFieldValueId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_EntityFieldsValues] DROP CONSTRAINT [DF_Mc_EntityFieldsValues_EntityFieldValueId]
END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_EntityNode_Mc_EntityNode]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_EntityNode]'))
ALTER TABLE [dbo].[Mc_EntityNode] DROP CONSTRAINT [FK_Mc_EntityNode_Mc_EntityNode]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_EntityNode_Mc_Instance]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_EntityNode]'))
ALTER TABLE [dbo].[Mc_EntityNode] DROP CONSTRAINT [FK_Mc_EntityNode_Mc_Instance]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_EntityNode_OrderNumber]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_EntityNode] DROP CONSTRAINT [DF_Mc_EntityNode_OrderNumber]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_EntityNode_Deleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_EntityNode] DROP CONSTRAINT [DF_Mc_EntityNode_Deleted]
END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_EntityNodeType_Mc_Instance]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_EntityNodeType]'))
ALTER TABLE [dbo].[Mc_EntityNodeType] DROP CONSTRAINT [FK_Mc_EntityNodeType_Mc_Instance]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_EntityNodeType_Deleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_EntityNodeType] DROP CONSTRAINT [DF_Mc_EntityNodeType_Deleted]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Group_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Group] DROP CONSTRAINT [DF_Mc_Group_Description]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Group_BuiltIn]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Group] DROP CONSTRAINT [DF_Mc_Group_BuiltIn]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Group_Deleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Group] DROP CONSTRAINT [DF_Mc_Group_Deleted]
END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_GroupsInstancesActions_Mc_Group]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_GroupsInstancesActions]'))
ALTER TABLE [dbo].[Mc_GroupsInstancesActions] DROP CONSTRAINT [FK_Mc_GroupsInstancesActions_Mc_Group]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_GroupsInstancesActions_Mc_Instance]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_GroupsInstancesActions]'))
ALTER TABLE [dbo].[Mc_GroupsInstancesActions] DROP CONSTRAINT [FK_Mc_GroupsInstancesActions_Mc_Instance]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_GroupsInstancesActions_Enabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_GroupsInstancesActions] DROP CONSTRAINT [DF_Mc_GroupsInstancesActions_Enabled]
END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_GroupsInstancesRoles_Mc_Group]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_GroupsInstancesRoles]'))
ALTER TABLE [dbo].[Mc_GroupsInstancesRoles] DROP CONSTRAINT [FK_Mc_GroupsInstancesRoles_Mc_Group]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_GroupsInstancesRoles_Mc_Instance]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_GroupsInstancesRoles]'))
ALTER TABLE [dbo].[Mc_GroupsInstancesRoles] DROP CONSTRAINT [FK_Mc_GroupsInstancesRoles_Mc_Instance]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Instance_InstanceId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_InstanceId]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Instance_PseudoId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_PseudoId]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Instance_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_Description]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Instance_EnableSignUpUser]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_EnableSignUpUser]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Instance_ExternalId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_ExternalId]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Instance_UtcOffset]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_UtcOffset]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Instance_DateFormat]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_DateFormat]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Instance_WorkingDays]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_WorkingDays]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Instance_Active]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_Active]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Instance_Trial]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_Trial]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Instance_Beta]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_Beta]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Instance_Deleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_Deleted]
END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_Message_Mc_Message]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_Message]'))
ALTER TABLE [dbo].[Mc_Message] DROP CONSTRAINT [FK_Mc_Message_Mc_Message]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_Message_Mc_User_1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_Message]'))
ALTER TABLE [dbo].[Mc_Message] DROP CONSTRAINT [FK_Mc_Message_Mc_User_1]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_Message_Mc_User_2]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_Message]'))
ALTER TABLE [dbo].[Mc_Message] DROP CONSTRAINT [FK_Mc_Message_Mc_User_2]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_OrganizationsUsers_Mc_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_OrganizationsUsers]'))
ALTER TABLE [dbo].[Mc_OrganizationsUsers] DROP CONSTRAINT [FK_Mc_OrganizationsUsers_Mc_User]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_OrganizationsUsers_OrganizationAdministrator]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_OrganizationsUsers] DROP CONSTRAINT [DF_Mc_OrganizationsUsers_OrganizationAdministrator]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_OrganizationsUsers_Active]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_OrganizationsUsers] DROP CONSTRAINT [DF_Mc_OrganizationsUsers_Active]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_MC_RecurringSchedule_RecurringScheduleId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_RecurringSchedule] DROP CONSTRAINT [DF_MC_RecurringSchedule_RecurringScheduleId]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Table_1_EntityType]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_RecurringSchedule] DROP CONSTRAINT [DF_Table_1_EntityType]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_MC_RecurringSchedule_UpdatedTime]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_RecurringSchedule] DROP CONSTRAINT [DF_MC_RecurringSchedule_UpdatedTime]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_MC_RecurringSchedule_Deleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_RecurringSchedule] DROP CONSTRAINT [DF_MC_RecurringSchedule_Deleted]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_RuleEngine_RoleId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Rule] DROP CONSTRAINT [DF_Mc_RuleEngine_RoleId]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_RuleEngine_Name]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Rule] DROP CONSTRAINT [DF_Mc_RuleEngine_Name]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_RuleEngine_DisplayName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Rule] DROP CONSTRAINT [DF_Mc_RuleEngine_DisplayName]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Rule_UsedQty]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Rule] DROP CONSTRAINT [DF_Mc_Rule_UsedQty]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Rule_CreatedDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Rule] DROP CONSTRAINT [DF_Mc_Rule_CreatedDate]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_Rule_Active]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_Rule] DROP CONSTRAINT [DF_Mc_Rule_Active]
END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_RuleEngineParameters_Mc_RuleEngine]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_RuleParameters]'))
ALTER TABLE [dbo].[Mc_RuleParameters] DROP CONSTRAINT [FK_Mc_RuleEngineParameters_Mc_RuleEngine]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_RuleEngineParameters_RuleParameterId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_RuleParameters] DROP CONSTRAINT [DF_Mc_RuleEngineParameters_RuleParameterId]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_RuleEngineParameters_Term]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_RuleParameters] DROP CONSTRAINT [DF_Mc_RuleEngineParameters_Term]
END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_SettingsValues_Mc_Group]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_SettingsValues]'))
ALTER TABLE [dbo].[Mc_SettingsValues] DROP CONSTRAINT [FK_Mc_SettingsValues_Mc_Group]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_SettingsValues_Mc_Instance]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_SettingsValues]'))
ALTER TABLE [dbo].[Mc_SettingsValues] DROP CONSTRAINT [FK_Mc_SettingsValues_Mc_Instance]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_User_FirstName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_User] DROP CONSTRAINT [DF_Mc_User_FirstName]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_User_LastName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_User] DROP CONSTRAINT [DF_Mc_User_LastName]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_User_MiddleName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_User] DROP CONSTRAINT [DF_Mc_User_MiddleName]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_User_Phone]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_User] DROP CONSTRAINT [DF_Mc_User_Phone]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_User_MobilePhone]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_User] DROP CONSTRAINT [DF_Mc_User_MobilePhone]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_User_Fax]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_User] DROP CONSTRAINT [DF_Mc_User_Fax]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_User_Title]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_User] DROP CONSTRAINT [DF_Mc_User_Title]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_User_Department]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_User] DROP CONSTRAINT [DF_Mc_User_Department]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_User_Street]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_User] DROP CONSTRAINT [DF_Mc_User_Street]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_User_Street2]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_User] DROP CONSTRAINT [DF_Mc_User_Street2]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_User_City]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_User] DROP CONSTRAINT [DF_Mc_User_City]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_User_State]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_User] DROP CONSTRAINT [DF_Mc_User_State]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_User_PostalCode]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_User] DROP CONSTRAINT [DF_Mc_User_PostalCode]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_User_Country]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_User] DROP CONSTRAINT [DF_Mc_User_Country]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_User_Deleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_User] DROP CONSTRAINT [DF_Mc_User_Deleted]
END

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_UsersGroups_Mc_Group]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_UsersGroups]'))
ALTER TABLE [dbo].[Mc_UsersGroups] DROP CONSTRAINT [FK_Mc_UsersGroups_Mc_Group]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_UsersGroups_Mc_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_UsersGroups]'))
ALTER TABLE [dbo].[Mc_UsersGroups] DROP CONSTRAINT [FK_Mc_UsersGroups_Mc_User]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_UsersInstances_Mc_Instance]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_UsersInstances]'))
ALTER TABLE [dbo].[Mc_UsersInstances] DROP CONSTRAINT [FK_Mc_UsersInstances_Mc_Instance]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Mc_UsersInstances_Mc_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Mc_UsersInstances]'))
ALTER TABLE [dbo].[Mc_UsersInstances] DROP CONSTRAINT [FK_Mc_UsersInstances_Mc_User]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Mc_UsersInstances_Active]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Mc_UsersInstances] DROP CONSTRAINT [DF_Mc_UsersInstances_Active]
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_EntityField]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_EntityField]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_EntityFieldListsValues]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_EntityFieldListsValues]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_EntityFieldsValues]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_EntityFieldsValues]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_EntityNode]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_EntityNode]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_EntityNodesRelatedEntityNodes]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_EntityNodesRelatedEntityNodes]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_EntityNodeType]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_EntityNodeType]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_Group]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_Group]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GroupsInstancesActions]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_GroupsInstancesActions]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GroupsInstancesRoles]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_GroupsInstancesRoles]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_Instance]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_Instance]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_Message]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_Message]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_OrganizationsUsers]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_OrganizationsUsers]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_RecurringSchedule]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_RecurringSchedule]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_Rule]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_Rule]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_RuleParameters]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_RuleParameters]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_SettingsValues]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_SettingsValues]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_User]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_User]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UsersGroups]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_UsersGroups]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UsersInstances]') AND type in (N'U'))
DROP TABLE [dbo].[Mc_UsersInstances]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_EntityField](
	[EntityFieldId] [uniqueidentifier] NOT NULL,
	[EntityFieldTypeId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[DataTypeId] [int] NOT NULL,
	[DefaultValue] [nvarchar](512) NULL,
	[AllowDBNull] [bit] NOT NULL,
	[Unique] [bit] NOT NULL,
	[MaxLength] [int] NOT NULL,
	[MinValue] [nvarchar](512) NULL,
	[MaxValue] [nvarchar](512) NULL,
	[DecimalDigits] [int] NOT NULL,
	[OrderNumber] [int] NOT NULL,
	[EntityId] [uniqueidentifier] NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[InstanceId] [uniqueidentifier] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Mc_EntityField] PRIMARY KEY CLUSTERED 
(
	[EntityFieldId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_EntityFieldListsValues](
	[EntityFieldListValueId] [uniqueidentifier] NOT NULL,
	[EntityFieldId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Value] [nvarchar](512) NOT NULL,
	[Default] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Mc_EntityFieldListsValues] PRIMARY KEY CLUSTERED 
(
	[EntityFieldListValueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_EntityFieldsValues](
	[EntityFieldValueId] [uniqueidentifier] NOT NULL,
	[EntityFieldId] [uniqueidentifier] NOT NULL,
	[LocalEntityId] [nvarchar](255) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_Mc_EntityFieldsValues] PRIMARY KEY CLUSTERED 
(
	[EntityFieldValueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_EntityNode](
	[EntityNodeId] [uniqueidentifier] NOT NULL,
	[ParentEntityNodeId] [uniqueidentifier] NULL,
	[Name] [nvarchar](255) NOT NULL,
	[OrderNumber] [int] NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[InstanceId] [uniqueidentifier] NULL,
	[EntityId] [uniqueidentifier] NOT NULL,
	[EntityNodeTypeId] [uniqueidentifier] NULL,
	[SubEntityId] [uniqueidentifier] NULL,
	[SubEntityLocalId] [nvarchar](255) NULL,
	[FullPath] [nvarchar](1024) NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Mc_EntityNode] PRIMARY KEY CLUSTERED 
(
	[EntityNodeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_EntityNodesRelatedEntityNodes](
	[EntityNodesRelatedEntityNodesId] [uniqueidentifier] NOT NULL,
	[EntityNodeId] [uniqueidentifier] NOT NULL,
	[RelatedEntityNodeId] [uniqueidentifier] NOT NULL,
	[EntityId] [uniqueidentifier] NOT NULL,
	[RelationType] [int] NOT NULL,
 CONSTRAINT [PK_Mc_EntityNodesRelatedEntityNodes] PRIMARY KEY CLUSTERED 
(
	[EntityNodesRelatedEntityNodesId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_EntityNodeType](
	[EntityNodeTypeId] [uniqueidentifier] NOT NULL,
	[EntityId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[OrderNumber] [int] NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[InstanceId] [uniqueidentifier] NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Mc_EntityNodeType] PRIMARY KEY CLUSTERED 
(
	[EntityNodeTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_Group](
	[GroupId] [uniqueidentifier] NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
	[BuiltIn] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Mc_Group] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE NONCLUSTERED INDEX [IX_Mc_Group_OrganizationId] ON [dbo].[Mc_Group] 
(
	[OrganizationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_GroupsInstancesActions](
	[GroupId] [uniqueidentifier] NOT NULL,
	[InstanceId] [uniqueidentifier] NOT NULL,
	[ActionId] [uniqueidentifier] NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_Mc_GroupsInstancesActions] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC,
	[InstanceId] ASC,
	[ActionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_GroupsInstancesRoles](
	[GroupId] [uniqueidentifier] NOT NULL,
	[InstanceId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Mc_GroupsInstancesRoles] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC,
	[InstanceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Mc_Instance](
	[InstanceId] [uniqueidentifier] NOT NULL,
	[PseudoId] [varchar](6) NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
	[EnableSignUpUser] [bit] NOT NULL,
	[ExternalId] [nvarchar](255) NOT NULL,
	[UTCOffset] [decimal](4, 2) NOT NULL,
	[DateFormat] [int] NOT NULL,
	[WorkingDays] [char](7) NOT NULL,
	[Active] [bit] NOT NULL,
	[CanceledTime] [datetime] NULL,
	[Trial] [bit] NOT NULL,
	[Beta] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_Mc_Instance] PRIMARY KEY CLUSTERED 
(
	[InstanceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


CREATE NONCLUSTERED INDEX [IX_Mc_Instance_OrganizationId] ON [dbo].[Mc_Instance] 
(
	[OrganizationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


CREATE NONCLUSTERED INDEX [IX_Mc_Instance_PseudoId] ON [dbo].[Mc_Instance] 
(
	[PseudoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_Message](
	[MessageId] [uniqueidentifier] NOT NULL,
	[ParentMessageId] [uniqueidentifier] NULL,
	[LocalObjectType] [nvarchar](50) NOT NULL,
	[LocalObjectId] [nvarchar](255) NOT NULL,
	[FromUserId] [uniqueidentifier] NOT NULL,
	[ToUserId] [uniqueidentifier] NULL,
	[Subject] [nvarchar](255) NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Mc_Message] PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_OrganizationsUsers](
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[OrganizationAdministrator] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Mc_OrganizationsUsers] PRIMARY KEY CLUSTERED 
(
	[OrganizationId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_RecurringSchedule](
	[RecurringScheduleId] [uniqueidentifier] NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[InstanceId] [uniqueidentifier] NULL,
	[LocalEntityType] [nvarchar](50) NOT NULL,
	[LocalEntityId] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[RecurrenceRule] [nvarchar](1024) NOT NULL,
	[UpdatedTime] [datetime] NOT NULL,
	[UpdatedBy] [uniqueidentifier] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_MC_RecurringSchedule] PRIMARY KEY CLUSTERED 
(
	[RecurringScheduleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_Rule](
	[RuleId] [uniqueidentifier] NOT NULL,
	[RuleEngineId] [uniqueidentifier] NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[InstanceId] [uniqueidentifier] NULL,
	[Name] [nvarchar](255) NOT NULL,
	[DisplayName] [nvarchar](255) NOT NULL,
	[UsedQty] [int] NOT NULL,
	[LastUsedUser] [uniqueidentifier] NULL,
	[LastUsedDate] [datetime] NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[OrderNumber] [int] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Mc_RuleEngine] PRIMARY KEY CLUSTERED 
(
	[RuleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_RuleParameters](
	[RuleParameterId] [uniqueidentifier] NOT NULL,
	[RuleId] [uniqueidentifier] NOT NULL,
	[EntityNodeTypeId] [uniqueidentifier] NULL,
	[IsInputParameter] [bit] NOT NULL,
	[IsEntity] [bit] NOT NULL,
	[FieldName] [nvarchar](255) NOT NULL,
	[FullName] [nvarchar](512) NOT NULL,
	[TypeName] [nvarchar](255) NULL,
	[Term] [nvarchar](50) NOT NULL,
	[Value] [sql_variant] NOT NULL,
 CONSTRAINT [PK_Mc_RuleEngineParameters] PRIMARY KEY CLUSTERED 
(
	[RuleParameterId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_SettingsValues](
	[SettingValueId] [uniqueidentifier] NOT NULL,
	[SettingId] [uniqueidentifier] NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[OrganizationId] [uniqueidentifier] NULL,
	[InstanceId] [uniqueidentifier] NULL,
	[GroupId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Mc_SettingsValues] PRIMARY KEY CLUSTERED 
(
	[SettingValueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_User](
	[UserId] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[FirstName] [nvarchar](255) NOT NULL,
	[LastName] [nvarchar](255) NOT NULL,
	[MiddleName] [nvarchar](255) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[MobilePhone] [nvarchar](20) NOT NULL,
	[Fax] [nvarchar](20) NOT NULL,
	[Title] [nvarchar](30) NOT NULL,
	[Department] [nvarchar](255) NOT NULL,
	[Street] [nvarchar](255) NOT NULL,
	[Street2] [nvarchar](255) NOT NULL,
	[City] [nvarchar](255) NOT NULL,
	[State] [nvarchar](255) NOT NULL,
	[PostalCode] [nvarchar](20) NOT NULL,
	[Country] [nvarchar](255) NOT NULL,
	[LastLoginDate] [datetime] NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Mc_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE NONCLUSTERED INDEX [IX_Mc_User_Email] ON [dbo].[Mc_User] 
(
	[Email] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_UsersGroups](
	[UserId] [uniqueidentifier] NOT NULL,
	[GroupId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Mc_UsersGroups] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[GroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mc_UsersInstances](
	[UserId] [uniqueidentifier] NOT NULL,
	[InstanceId] [uniqueidentifier] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Mc_UsersInstances] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[InstanceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Mc_EntityField]  WITH CHECK ADD  CONSTRAINT [FK_Mc_EntityField_Mc_Instance] FOREIGN KEY([InstanceId])
REFERENCES [dbo].[Mc_Instance] ([InstanceId])
GO

ALTER TABLE [dbo].[Mc_EntityField] CHECK CONSTRAINT [FK_Mc_EntityField_Mc_Instance]
GO

ALTER TABLE [dbo].[Mc_EntityField] ADD  CONSTRAINT [DF_Mc_EntityField_AllowDBNull]  DEFAULT ((1)) FOR [AllowDBNull]
GO

ALTER TABLE [dbo].[Mc_EntityField] ADD  CONSTRAINT [DF_Mc_EntityField_Unique]  DEFAULT ((0)) FOR [Unique]
GO

ALTER TABLE [dbo].[Mc_EntityField] ADD  CONSTRAINT [DF_Mc_EntityField_MaxLength]  DEFAULT ((0)) FOR [MaxLength]
GO

ALTER TABLE [dbo].[Mc_EntityField] ADD  CONSTRAINT [DF_Mc_EntityField_DecimalDigits]  DEFAULT ((0)) FOR [DecimalDigits]
GO

ALTER TABLE [dbo].[Mc_EntityField] ADD  CONSTRAINT [DF_Mc_EntityField_OrderNumber]  DEFAULT ((0)) FOR [OrderNumber]
GO

ALTER TABLE [dbo].[Mc_EntityField] ADD  CONSTRAINT [DF_Mc_EntityField_Active]  DEFAULT ((1)) FOR [Active]
GO

ALTER TABLE [dbo].[Mc_EntityFieldListsValues]  WITH CHECK ADD  CONSTRAINT [FK_Mc_EntityFieldListsValues_Mc_EntityField] FOREIGN KEY([EntityFieldId])
REFERENCES [dbo].[Mc_EntityField] ([EntityFieldId])
GO

ALTER TABLE [dbo].[Mc_EntityFieldListsValues] CHECK CONSTRAINT [FK_Mc_EntityFieldListsValues_Mc_EntityField]
GO

ALTER TABLE [dbo].[Mc_EntityFieldListsValues] ADD  CONSTRAINT [DF_Mc_EntityFieldListsValues_Default]  DEFAULT ((0)) FOR [Default]
GO

ALTER TABLE [dbo].[Mc_EntityFieldListsValues] ADD  CONSTRAINT [DF_Mc_EntityFieldListsValues_Active]  DEFAULT ((1)) FOR [Active]
GO

ALTER TABLE [dbo].[Mc_EntityFieldsValues]  WITH CHECK ADD  CONSTRAINT [FK_Mc_EntityFieldsValues_Mc_EntityField] FOREIGN KEY([EntityFieldId])
REFERENCES [dbo].[Mc_EntityField] ([EntityFieldId])
GO

ALTER TABLE [dbo].[Mc_EntityFieldsValues] CHECK CONSTRAINT [FK_Mc_EntityFieldsValues_Mc_EntityField]
GO

ALTER TABLE [dbo].[Mc_EntityFieldsValues] ADD  CONSTRAINT [DF_Mc_EntityFieldsValues_EntityFieldValueId]  DEFAULT (newid()) FOR [EntityFieldValueId]
GO

ALTER TABLE [dbo].[Mc_EntityNode]  WITH CHECK ADD  CONSTRAINT [FK_Mc_EntityNode_Mc_EntityNode] FOREIGN KEY([ParentEntityNodeId])
REFERENCES [dbo].[Mc_EntityNode] ([EntityNodeId])
GO

ALTER TABLE [dbo].[Mc_EntityNode] CHECK CONSTRAINT [FK_Mc_EntityNode_Mc_EntityNode]
GO

ALTER TABLE [dbo].[Mc_EntityNode]  WITH CHECK ADD  CONSTRAINT [FK_Mc_EntityNode_Mc_Instance] FOREIGN KEY([InstanceId])
REFERENCES [dbo].[Mc_Instance] ([InstanceId])
GO

ALTER TABLE [dbo].[Mc_EntityNode] CHECK CONSTRAINT [FK_Mc_EntityNode_Mc_Instance]
GO

ALTER TABLE [dbo].[Mc_EntityNode] ADD  CONSTRAINT [DF_Mc_EntityNode_OrderNumber]  DEFAULT ((0)) FOR [OrderNumber]
GO

ALTER TABLE [dbo].[Mc_EntityNode] ADD  CONSTRAINT [DF_Mc_EntityNode_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO

ALTER TABLE [dbo].[Mc_EntityNodeType]  WITH CHECK ADD  CONSTRAINT [FK_Mc_EntityNodeType_Mc_Instance] FOREIGN KEY([InstanceId])
REFERENCES [dbo].[Mc_Instance] ([InstanceId])
GO

ALTER TABLE [dbo].[Mc_EntityNodeType] CHECK CONSTRAINT [FK_Mc_EntityNodeType_Mc_Instance]
GO

ALTER TABLE [dbo].[Mc_EntityNodeType] ADD  CONSTRAINT [DF_Mc_EntityNodeType_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO

ALTER TABLE [dbo].[Mc_Group] ADD  CONSTRAINT [DF_Mc_Group_Description]  DEFAULT (N'') FOR [Description]
GO

ALTER TABLE [dbo].[Mc_Group] ADD  CONSTRAINT [DF_Mc_Group_BuiltIn]  DEFAULT ((0)) FOR [BuiltIn]
GO

ALTER TABLE [dbo].[Mc_Group] ADD  CONSTRAINT [DF_Mc_Group_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO

ALTER TABLE [dbo].[Mc_GroupsInstancesActions]  WITH CHECK ADD  CONSTRAINT [FK_Mc_GroupsInstancesActions_Mc_Group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Mc_Group] ([GroupId])
GO

ALTER TABLE [dbo].[Mc_GroupsInstancesActions] CHECK CONSTRAINT [FK_Mc_GroupsInstancesActions_Mc_Group]
GO

ALTER TABLE [dbo].[Mc_GroupsInstancesActions]  WITH CHECK ADD  CONSTRAINT [FK_Mc_GroupsInstancesActions_Mc_Instance] FOREIGN KEY([InstanceId])
REFERENCES [dbo].[Mc_Instance] ([InstanceId])
GO

ALTER TABLE [dbo].[Mc_GroupsInstancesActions] CHECK CONSTRAINT [FK_Mc_GroupsInstancesActions_Mc_Instance]
GO

ALTER TABLE [dbo].[Mc_GroupsInstancesActions] ADD  CONSTRAINT [DF_Mc_GroupsInstancesActions_Enabled]  DEFAULT ((1)) FOR [Enabled]
GO

ALTER TABLE [dbo].[Mc_GroupsInstancesRoles]  WITH CHECK ADD  CONSTRAINT [FK_Mc_GroupsInstancesRoles_Mc_Group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Mc_Group] ([GroupId])
GO

ALTER TABLE [dbo].[Mc_GroupsInstancesRoles] CHECK CONSTRAINT [FK_Mc_GroupsInstancesRoles_Mc_Group]
GO

ALTER TABLE [dbo].[Mc_GroupsInstancesRoles]  WITH CHECK ADD  CONSTRAINT [FK_Mc_GroupsInstancesRoles_Mc_Instance] FOREIGN KEY([InstanceId])
REFERENCES [dbo].[Mc_Instance] ([InstanceId])
GO

ALTER TABLE [dbo].[Mc_GroupsInstancesRoles] CHECK CONSTRAINT [FK_Mc_GroupsInstancesRoles_Mc_Instance]
GO

ALTER TABLE [dbo].[Mc_Instance] ADD  CONSTRAINT [DF_Mc_Instance_InstanceId]  DEFAULT (newid()) FOR [InstanceId]
GO

ALTER TABLE [dbo].[Mc_Instance] ADD  CONSTRAINT [DF_Mc_Instance_PseudoId]  DEFAULT ('') FOR [PseudoId]
GO

ALTER TABLE [dbo].[Mc_Instance] ADD  CONSTRAINT [DF_Mc_Instance_Description]  DEFAULT (N'') FOR [Description]
GO

ALTER TABLE [dbo].[Mc_Instance] ADD  CONSTRAINT [DF_Mc_Instance_EnableSignUpUser]  DEFAULT ((0)) FOR [EnableSignUpUser]
GO

ALTER TABLE [dbo].[Mc_Instance] ADD  CONSTRAINT [DF_Mc_Instance_ExternalId]  DEFAULT (N'') FOR [ExternalId]
GO

ALTER TABLE [dbo].[Mc_Instance] ADD  CONSTRAINT [DF_Mc_Instance_UtcOffset]  DEFAULT ((0)) FOR [UTCOffset]
GO

ALTER TABLE [dbo].[Mc_Instance] ADD  CONSTRAINT [DF_Mc_Instance_DateFormat]  DEFAULT ((0)) FOR [DateFormat]
GO

ALTER TABLE [dbo].[Mc_Instance] ADD  CONSTRAINT [DF_Mc_Instance_WorkingDays]  DEFAULT ((1111100)) FOR [WorkingDays]
GO

ALTER TABLE [dbo].[Mc_Instance] ADD  CONSTRAINT [DF_Mc_Instance_Active]  DEFAULT ((1)) FOR [Active]
GO

ALTER TABLE [dbo].[Mc_Instance] ADD  CONSTRAINT [DF_Mc_Instance_Trial]  DEFAULT ((0)) FOR [Trial]
GO

ALTER TABLE [dbo].[Mc_Instance] ADD  CONSTRAINT [DF_Mc_Instance_Beta]  DEFAULT ((0)) FOR [Beta]
GO

ALTER TABLE [dbo].[Mc_Instance] ADD  CONSTRAINT [DF_Mc_Instance_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO

ALTER TABLE [dbo].[Mc_Message]  WITH CHECK ADD  CONSTRAINT [FK_Mc_Message_Mc_Message] FOREIGN KEY([ParentMessageId])
REFERENCES [dbo].[Mc_Message] ([MessageId])
GO

ALTER TABLE [dbo].[Mc_Message] CHECK CONSTRAINT [FK_Mc_Message_Mc_Message]
GO

ALTER TABLE [dbo].[Mc_Message]  WITH CHECK ADD  CONSTRAINT [FK_Mc_Message_Mc_User_1] FOREIGN KEY([FromUserId])
REFERENCES [dbo].[Mc_User] ([UserId])
GO

ALTER TABLE [dbo].[Mc_Message] CHECK CONSTRAINT [FK_Mc_Message_Mc_User_1]
GO

ALTER TABLE [dbo].[Mc_Message]  WITH CHECK ADD  CONSTRAINT [FK_Mc_Message_Mc_User_2] FOREIGN KEY([ToUserId])
REFERENCES [dbo].[Mc_User] ([UserId])
GO

ALTER TABLE [dbo].[Mc_Message] CHECK CONSTRAINT [FK_Mc_Message_Mc_User_2]
GO

ALTER TABLE [dbo].[Mc_OrganizationsUsers]  WITH CHECK ADD  CONSTRAINT [FK_Mc_OrganizationsUsers_Mc_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[Mc_User] ([UserId])
GO

ALTER TABLE [dbo].[Mc_OrganizationsUsers] CHECK CONSTRAINT [FK_Mc_OrganizationsUsers_Mc_User]
GO

ALTER TABLE [dbo].[Mc_OrganizationsUsers] ADD  CONSTRAINT [DF_Mc_OrganizationsUsers_OrganizationAdministrator]  DEFAULT ((0)) FOR [OrganizationAdministrator]
GO

ALTER TABLE [dbo].[Mc_OrganizationsUsers] ADD  CONSTRAINT [DF_Mc_OrganizationsUsers_Active]  DEFAULT ((1)) FOR [Active]
GO

ALTER TABLE [dbo].[Mc_RecurringSchedule] ADD  CONSTRAINT [DF_MC_RecurringSchedule_RecurringScheduleId]  DEFAULT (newid()) FOR [RecurringScheduleId]
GO

ALTER TABLE [dbo].[Mc_RecurringSchedule] ADD  CONSTRAINT [DF_Table_1_EntityType]  DEFAULT (N'') FOR [LocalEntityType]
GO

ALTER TABLE [dbo].[Mc_RecurringSchedule] ADD  CONSTRAINT [DF_MC_RecurringSchedule_UpdatedTime]  DEFAULT (getdate()) FOR [UpdatedTime]
GO

ALTER TABLE [dbo].[Mc_RecurringSchedule] ADD  CONSTRAINT [DF_MC_RecurringSchedule_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO

ALTER TABLE [dbo].[Mc_Rule] ADD  CONSTRAINT [DF_Mc_RuleEngine_RoleId]  DEFAULT (newid()) FOR [RuleId]
GO

ALTER TABLE [dbo].[Mc_Rule] ADD  CONSTRAINT [DF_Mc_RuleEngine_Name]  DEFAULT (N'') FOR [Name]
GO

ALTER TABLE [dbo].[Mc_Rule] ADD  CONSTRAINT [DF_Mc_RuleEngine_DisplayName]  DEFAULT (N'') FOR [DisplayName]
GO

ALTER TABLE [dbo].[Mc_Rule] ADD  CONSTRAINT [DF_Mc_Rule_UsedQty]  DEFAULT ((0)) FOR [UsedQty]
GO

ALTER TABLE [dbo].[Mc_Rule] ADD  CONSTRAINT [DF_Mc_Rule_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[Mc_Rule] ADD  CONSTRAINT [DF_Mc_Rule_Active]  DEFAULT ((1)) FOR [Active]
GO

ALTER TABLE [dbo].[Mc_RuleParameters]  WITH CHECK ADD  CONSTRAINT [FK_Mc_RuleEngineParameters_Mc_RuleEngine] FOREIGN KEY([RuleId])
REFERENCES [dbo].[Mc_Rule] ([RuleId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Mc_RuleParameters] CHECK CONSTRAINT [FK_Mc_RuleEngineParameters_Mc_RuleEngine]
GO

ALTER TABLE [dbo].[Mc_RuleParameters] ADD  CONSTRAINT [DF_Mc_RuleEngineParameters_RuleParameterId]  DEFAULT (newid()) FOR [RuleParameterId]
GO

ALTER TABLE [dbo].[Mc_RuleParameters] ADD  CONSTRAINT [DF_Mc_RuleEngineParameters_Term]  DEFAULT (N'=') FOR [Term]
GO

ALTER TABLE [dbo].[Mc_SettingsValues]  WITH CHECK ADD  CONSTRAINT [FK_Mc_SettingsValues_Mc_Group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Mc_Group] ([GroupId])
GO

ALTER TABLE [dbo].[Mc_SettingsValues] CHECK CONSTRAINT [FK_Mc_SettingsValues_Mc_Group]
GO

ALTER TABLE [dbo].[Mc_SettingsValues]  WITH CHECK ADD  CONSTRAINT [FK_Mc_SettingsValues_Mc_Instance] FOREIGN KEY([InstanceId])
REFERENCES [dbo].[Mc_Instance] ([InstanceId])
GO

ALTER TABLE [dbo].[Mc_SettingsValues] CHECK CONSTRAINT [FK_Mc_SettingsValues_Mc_Instance]
GO

ALTER TABLE [dbo].[Mc_User] ADD  CONSTRAINT [DF_Mc_User_FirstName]  DEFAULT (N'') FOR [FirstName]
GO

ALTER TABLE [dbo].[Mc_User] ADD  CONSTRAINT [DF_Mc_User_LastName]  DEFAULT (N'') FOR [LastName]
GO

ALTER TABLE [dbo].[Mc_User] ADD  CONSTRAINT [DF_Mc_User_MiddleName]  DEFAULT (N'') FOR [MiddleName]
GO

ALTER TABLE [dbo].[Mc_User] ADD  CONSTRAINT [DF_Mc_User_Phone]  DEFAULT ('') FOR [Phone]
GO

ALTER TABLE [dbo].[Mc_User] ADD  CONSTRAINT [DF_Mc_User_MobilePhone]  DEFAULT ('') FOR [MobilePhone]
GO

ALTER TABLE [dbo].[Mc_User] ADD  CONSTRAINT [DF_Mc_User_Fax]  DEFAULT ('') FOR [Fax]
GO

ALTER TABLE [dbo].[Mc_User] ADD  CONSTRAINT [DF_Mc_User_Title]  DEFAULT ('') FOR [Title]
GO

ALTER TABLE [dbo].[Mc_User] ADD  CONSTRAINT [DF_Mc_User_Department]  DEFAULT ('') FOR [Department]
GO

ALTER TABLE [dbo].[Mc_User] ADD  CONSTRAINT [DF_Mc_User_Street]  DEFAULT ('') FOR [Street]
GO

ALTER TABLE [dbo].[Mc_User] ADD  CONSTRAINT [DF_Mc_User_Street2]  DEFAULT ('') FOR [Street2]
GO

ALTER TABLE [dbo].[Mc_User] ADD  CONSTRAINT [DF_Mc_User_City]  DEFAULT ('') FOR [City]
GO

ALTER TABLE [dbo].[Mc_User] ADD  CONSTRAINT [DF_Mc_User_State]  DEFAULT ('') FOR [State]
GO

ALTER TABLE [dbo].[Mc_User] ADD  CONSTRAINT [DF_Mc_User_PostalCode]  DEFAULT ('') FOR [PostalCode]
GO

ALTER TABLE [dbo].[Mc_User] ADD  CONSTRAINT [DF_Mc_User_Country]  DEFAULT ('') FOR [Country]
GO

ALTER TABLE [dbo].[Mc_User] ADD  CONSTRAINT [DF_Mc_User_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO

ALTER TABLE [dbo].[Mc_UsersGroups]  WITH CHECK ADD  CONSTRAINT [FK_Mc_UsersGroups_Mc_Group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Mc_Group] ([GroupId])
GO

ALTER TABLE [dbo].[Mc_UsersGroups] CHECK CONSTRAINT [FK_Mc_UsersGroups_Mc_Group]
GO

ALTER TABLE [dbo].[Mc_UsersGroups]  WITH CHECK ADD  CONSTRAINT [FK_Mc_UsersGroups_Mc_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[Mc_User] ([UserId])
GO

ALTER TABLE [dbo].[Mc_UsersGroups] CHECK CONSTRAINT [FK_Mc_UsersGroups_Mc_User]
GO

ALTER TABLE [dbo].[Mc_UsersInstances]  WITH CHECK ADD  CONSTRAINT [FK_Mc_UsersInstances_Mc_Instance] FOREIGN KEY([InstanceId])
REFERENCES [dbo].[Mc_Instance] ([InstanceId])
GO

ALTER TABLE [dbo].[Mc_UsersInstances] CHECK CONSTRAINT [FK_Mc_UsersInstances_Mc_Instance]
GO

ALTER TABLE [dbo].[Mc_UsersInstances]  WITH CHECK ADD  CONSTRAINT [FK_Mc_UsersInstances_Mc_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[Mc_User] ([UserId])
GO

ALTER TABLE [dbo].[Mc_UsersInstances] CHECK CONSTRAINT [FK_Mc_UsersInstances_Mc_User]
GO

ALTER TABLE [dbo].[Mc_UsersInstances] ADD  CONSTRAINT [DF_Mc_UsersInstances_Active]  DEFAULT ((1)) FOR [Active]
GO
