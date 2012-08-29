﻿BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('00000000-0000-0000-0000-000000000008', '00000000-0000-0000-0000-000000000027', 1, N'Entities', N'', N'', N'', N'/Resources.Micajah.Common/Pages/Setup/Entities.aspx', 3, N'Micajah.Common.Bll.Providers.ActionProvider', 0, 0, 1, 1, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action 
	SET [OrderNumber] = [OrderNumber] + 1 
	WHERE ActionId IN ('00000000-0000-0000-0000-000000000030', '00000000-0000-0000-0000-000000000031', '00000000-0000-0000-0000-000000000034', '00000000-0000-0000-0000-000000000037', '00000000-0000-0000-0000-000000000038', '00000000-0000-0000-0000-000000000039', '00000000-0000-0000-0000-000000000040', '00000000-0000-0000-0000-000000000041')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION