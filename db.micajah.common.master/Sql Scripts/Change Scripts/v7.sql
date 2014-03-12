BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	SET IDENTITY_INSERT [dbo].[Mc_Setting] ON

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Setting]([SettingId], [ParentSettingId], [SettingTypeId], [Name], [Description], [ShortName], [DefaultValue], [Value], [OrderNumber], [EnableOrganization], [EnableDepartment], [EnableRole], [EnableGroup], [Deleted]) VALUES (81, NULL, 1, N'The settings for emails', N'The settings for emails.', N'EmailSettings', N'False', NULL, 2, 0, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1	
	INSERT INTO [dbo].[Mc_Setting]([SettingId], [ParentSettingId], [SettingTypeId], [Name], [Description], [ShortName], [DefaultValue], [Value], [OrderNumber], [EnableOrganization], [EnableDepartment], [EnableRole], [EnableGroup], [Deleted]) VALUES (82, 81, 1, N'Allows to send the email notification to a newly created user', N'Whether the email notification with credentials will be sent to newly created user.', N'EnableCreateNewUserEmailNotification', N'True', NULL, 3, 0, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Setting]([SettingId], [ParentSettingId], [SettingTypeId], [Name], [Description], [ShortName], [DefaultValue], [Value], [OrderNumber], [EnableOrganization], [EnableDepartment], [EnableRole], [EnableGroup], [Deleted]) VALUES (83, 81, 1, N'Allows to send the email notification after adding the user to an organization', N'Whether the email notification will be sent after adding the user to an organization.', N'EnableAddUserToOrganizationEmailNotification', N'True', NULL, 4, 0, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Setting]([SettingId], [ParentSettingId], [SettingTypeId], [Name], [Description], [ShortName], [DefaultValue], [Value], [OrderNumber], [EnableOrganization], [EnableDepartment], [EnableRole], [EnableGroup], [Deleted]) VALUES (84, 81, 1, N'Allows to send the email notification after the login name is changed', N'Whether the email notification with new credentials will be sent, if the login name is changed.', N'EnableChangeLoginNameEmailNotification', N'True', NULL, 5, 0, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Setting]([SettingId], [ParentSettingId], [SettingTypeId], [Name], [Description], [ShortName], [DefaultValue], [Value], [OrderNumber], [EnableOrganization], [EnableDepartment], [EnableRole], [EnableGroup], [Deleted]) VALUES (85, 81, 1, N'Allows to send the email notification after the password is changed', N'Whether the email notification with new credentials will be sent, if the password is changed.', N'EnableChangePasswordEmailNotification', N'True', NULL, 6, 0, 0, 0, 0, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	SET IDENTITY_INSERT [dbo].[Mc_Setting] OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Setting SET Value = N'True' WHERE SettingId = 81

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Setting SET ParentSettingId = 81, OrderNumber = 1 WHERE SettingId = 3

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Setting SET ParentSettingId = 81, OrderNumber = 2 WHERE SettingId = 4

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Setting SET OrderNumber = 3 WHERE SettingId = 21

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Setting SET OrderNumber = 4 WHERE SettingId = 31

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Setting SET OrderNumber = 5 WHERE SettingId = 41

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION