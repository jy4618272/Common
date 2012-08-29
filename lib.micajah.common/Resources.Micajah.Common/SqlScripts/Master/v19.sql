BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('042D39D6-A607-4063-808F-0D75BA9CB899', '00000000-0000-0000-0000-000000000027', 1, N'Embedded Icons', N'The icons which are embedded into library', N'', N'', N'/Resources.Micajah.Common/Pages/Setup/EmbeddedIconsMenu.aspx', 3, N'', 0, 0, 1, 1, 0, 0, 0, 1, 0)
	
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('43387D96-C28F-41C3-87A7-185DE4487B65', '042D39D6-A607-4063-808F-0D75BA9CB899', 1, N'Show Embedded Icons', N'Shows the embedded icons', N'', N'', N'/Resources.Micajah.Common/Pages/Setup/EmbeddedIcons.aspx', 1, N'', 0, 0, 1, 1, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION