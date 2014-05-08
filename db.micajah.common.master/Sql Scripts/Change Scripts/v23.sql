BEGIN TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Action] DROP CONSTRAINT [FK_Mc_Action_Mc_Action]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
 
IF @@TRANCOUNT = 1
	UPDATE	[dbo].[Mc_Action] 
	SET		[NavigateUrl] = N'/Resources.Micajah.Common/Pages/Admin/OrganizationNodeTypes.aspx' 
	WHERE	[ActionId] = '0F6731D3-EE3A-4614-9092-043E088CC3F2';

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE	[dbo].[Mc_Action] 
	SET		[Description] = N'Node Type', 
			[NavigateUrl] = N'/Resources.Micajah.Common/Pages/Admin/OrganizationNodeType.aspx' 
	WHERE	[ActionId] = '8375FC0A-4A82-46F1-B9D8-E53918E4FB42';

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) 
	VALUES ('CA150872-16A6-4769-8CB9-1195B267A613', 'D934D126-F53A-49F0-833C-1C660EBC7894', 1, N'Node Type', N'Node Type', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/InstanceNodeType.aspx', 0, N'Micajah.Common.Bll.Providers.ActionProvider', 1, 1, 1, 1, 0, 0, 0, 1, 0);

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
   
IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) 
	VALUES ('D934D126-F53A-49F0-833C-1C660EBC7894', '00000000-0000-0000-0000-000000000015', 1, N'Node Types', N'Manage the Node Types', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/InstanceNodeTypes.aspx', 0, N'Micajah.Common.Bll.Providers.ActionProvider', 1, 1, 1, 1, 0, 0, 0, 1, 0);

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE	[dbo].[Mc_Action] 
	SET		[NavigateUrl] = N'/Resources.Micajah.Common/Pages/Admin/OrganizationRulesEngine.aspx',
			[InstanceRequired] = 0
	WHERE	[ActionId] = 'D63CAC58-9B6E-46A4-B845-68332F759DFB';

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE	[dbo].[Mc_Action] 
	SET		[NavigateUrl] = N'/Resources.Micajah.Common/Pages/Admin/OrganizationRules.aspx',
			[InstanceRequired] = 0
	WHERE	[ActionId] = 'BB0239EC-6259-4D7A-B392-FA22ED8FEF65';

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE	[dbo].[Mc_Action] 
	SET		[NavigateUrl] = N'/Resources.Micajah.Common/Pages/Admin/OrganizationRuleParameters.aspx',
			[InstanceRequired] = 0
	WHERE	[ActionId] = 'CD1EBE2B-C229-421C-847A-6FF560665D1A';

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) 
	VALUES ('EF0F170E-CF70-4C8B-ACE2-E7E2E8096821', '00000000-0000-0000-0000-000000000015', 1, N'Rules Engines', N'Rules Engine List ', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/InstanceRulesEngine.aspx', 0, N'', 1, 1, 1, 1, 0, 0, 0, 1, 0)  

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) 
	VALUES ('CC74A2A0-38CE-42A6-8AD7-449747444BCD', 'EF0F170E-CF70-4C8B-ACE2-E7E2E8096821', 1, N'Rules', N'Rules List in Engine', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/InstanceRules.aspx', 0, N'', 1, 1, 1, 1, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) 
	VALUES ('142F6932-2264-4CFD-95BB-3DD336E84A2F', 'CC74A2A0-38CE-42A6-8AD7-449747444BCD', 1, N'Rule Parameters', N'Rule Parameters List', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/InstanceRuleParameters.aspx', 0, N'', 1, 1, 1, 0, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Action] ADD CONSTRAINT [FK_Mc_Action_Mc_Action] FOREIGN KEY ([ParentActionId]) REFERENCES [dbo].[Mc_Action] ([ActionId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
