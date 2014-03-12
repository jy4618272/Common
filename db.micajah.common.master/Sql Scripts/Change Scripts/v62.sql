BEGIN TRANSACTION

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL, XACT_ABORT ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Description] = N'Settings to integrate your Ldap or Active Directory server',	[NavigateUrl] = N'/mc/Admin/LdapIntegration.aspx' WHERE [ActionId] = '{A3223AB8-D9E7-437A-AE0E-14164C0F90B7}'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('{F53C5189-BD4D-47E5-88EF-58B81178F7EF}', '{A3223AB8-D9E7-437A-AE0E-14164C0F90B7}', 1, N'Server Settings', N'Organization Ldap Settings for authentication', N'', N'', N'/mc/Admin/OrganizationLdapSettings.aspx', 1, N'', 1, 0, 1, 1, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('{72B411D0-FB81-4444-B217-AFC4416A5319}', '{A3223AB8-D9E7-437A-AE0E-14164C0F90B7}', 1, N'Group Mappings', N'Ldap Group Mappings', N'', N'', N'/mc/Admin/LdapGroupMappings.aspx', 2, N'', 1, 0, 1, 1, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Name] = N'Profile' WHERE [ActionId] = '{00000000-0000-0000-0000-000000000024}'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Name] = N'Password' WHERE [ActionId] = '{00000000-0000-0000-0000-000000000025}'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Name] = N'Groups' WHERE [ActionId] = '{00000000-0000-0000-0000-000000000026}'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Name] = N'Ldap Info' WHERE [ActionId] = '{5749FF70-4592-4B91-8579-5E7E203C0410}'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
