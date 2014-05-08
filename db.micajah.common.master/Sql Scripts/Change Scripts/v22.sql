BEGIN TRANSACTION
 
IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('D63CAC58-9B6E-46A4-B845-68332F759DFB', '00000000-0000-0000-0000-000000000009', 1, N'Rules Engines', N'Rules Engine List ', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/RulesEngine.aspx', 0, N'', 1, 1, 1, 1, 0, 0, 0, 1, 0)  

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
   	
IF @@TRANCOUNT = 1	
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('BB0239EC-6259-4D7A-B392-FA22ED8FEF65', 'D63CAC58-9B6E-46A4-B845-68332F759DFB', 1, N'Rules', N'Rules List in Engine', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/Rules.aspx', 0, N'', 1, 1, 1, 1, 0, 0, 0, 1, 0)
	
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
   	
IF @@TRANCOUNT = 1	
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('CD1EBE2B-C229-421C-847A-6FF560665D1A', 'BB0239EC-6259-4D7A-B392-FA22ED8FEF65', 1, N'Rule Parameters', N'Rule Parameters List', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/RuleParameters.aspx', 0, N'', 1, 1, 1, 0, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
