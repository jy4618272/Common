BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('5A25507B-F8B2-4D5C-AC70-71A0DD5C8729', '00000000-0000-0000-0000-000000000009', 1, N'Entities Fields', N'Manage the Fields of the Entities', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/OrganizationEntitiesFields.aspx', 0, N'Micajah.Common.Bll.Providers.ActionProvider', 1, 0, 1, 1, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('6E03B1C3-7AD0-460B-BF73-E3F9055A71DE', '5A25507B-F8B2-4D5C-AC70-71A0DD5C8729', 1, N'Entity Fields', N'Manage the Fields of the Entity', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/OrganizationEntityFields.aspx', 0, N'', 1, 0, 1, 1, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('ECAAF36B-7D92-4424-BF7A-CFDF85B689CC', '00000000-0000-0000-0000-000000000015', 1, N'Entities Fields', N'Manage the Fields of the Entities', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/InstanceEntitiesFields.aspx', 0, N'Micajah.Common.Bll.Providers.ActionProvider', 1, 1, 1, 1, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('F813370C-B0AA-4D7D-A6AB-B8CE4EA7F771', 'ECAAF36B-7D92-4424-BF7A-CFDF85B689CC', 1, N'Entity Fields', N'Manage the Fields of the Entity', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/InstanceEntityFields.aspx', 0, N'', 1, 1, 1, 1, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('A058B233-203B-472C-8D15-86A353ADD11A', '6E03B1C3-7AD0-460B-BF73-E3F9055A71DE', 1, N'Entity Field Values', N'Manage the Values of the Entity Field', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/EntityFieldListValues.aspx', 0, N'', 1, 0, 1, 1, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_ActionsParentActions] ([ActionId], [ParentActionId]) VALUES ('A058B233-203B-472C-8D15-86A353ADD11A', 'F813370C-B0AA-4D7D-A6AB-B8CE4EA7F771')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
