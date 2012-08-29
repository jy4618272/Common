BEGIN TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('{0F6731D3-EE3A-4614-9092-043E088CC3F2}', '{00000000-0000-0000-0000-000000000009}', 1, N'Node Types', N'Manage the Node Types', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/NodeTypes.aspx', 0, N'', 1, 0, 1, 1, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
   	
IF @@TRANCOUNT = 1	
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('{8375FC0A-4A82-46F1-B9D8-E53918E4FB42}', '{0F6731D3-EE3A-4614-9092-043E088CC3F2}', 1, N'Node Types', N'Manage the Tree Node Types', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/NodeType.aspx', 0, N'', 1, 0, 1, 1, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
