BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
BEGIN
	SET IDENTITY_INSERT dbo.Mc_Action ON

	INSERT INTO dbo.Mc_Action (ActionId, ParentActionId, ActionTypeId, [Name], [Description], IconUrl, ButtonIconUrl, NavigateUrl, OrderNumber, ClassFullName, DepartmentRequired, Visible, ShowInDetailMenu, ShowChildrenInDetailMenu, GroupInDetailMenu, HighlightInDetailMenu, Deleted) 
	SELECT 300, ParentActionId, ActionTypeId, [Name], [Description], IconUrl, ButtonIconUrl, NavigateUrl, OrderNumber, ClassFullName, DepartmentRequired, Visible, ShowInDetailMenu, ShowChildrenInDetailMenu, GroupInDetailMenu, HighlightInDetailMenu, Deleted 
	FROM dbo.Mc_Action 
	WHERE ActionId = 8
	
	SET IDENTITY_INSERT dbo.Mc_Action OFF
END

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_RolesActions (RoleId, ActionId)
	SELECT RoleId, 300 FROM dbo.Mc_RolesActions WHERE ActionId = 8

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_ActionsParentActions (ActionId, ParentActionId)
	SELECT 300, ParentActionId FROM dbo.Mc_ActionsParentActions WHERE ActionId = 8

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_ActionsParentActions (ActionId, ParentActionId)
	SELECT ActionId, 300 FROM dbo.Mc_ActionsParentActions WHERE ParentActionId = 8

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action SET ParentActionId = 300 WHERE ParentActionId = 8

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action SET NavigateUrl = N'/Resources.Micajah.Common/Pages/DetailMenu.aspx?pageid=15' WHERE ActionId = 3

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action SET NavigateUrl = N'/Resources.Micajah.Common/Pages/DetailMenu.aspx?pageid=21' WHERE ActionId = 5

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Role SET StartActionId = 300 WHERE StartActionId = 8

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DELETE FROM dbo.Mc_ActionsParentActions WHERE (ActionId = 8) OR (ParentActionId = 8)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DELETE FROM dbo.Mc_RolesActions WHERE ActionId = 8

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DELETE FROM dbo.Mc_Action WHERE ActionId = 8

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Setting SET ShortName = N'CopyrightCompanyWebsiteUrl' WHERE SettingId = 7
	
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Setting SET DefaultValue = N'MD5'	WHERE SettingId = 22

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Setting SET ShortName = N'VisibleBreadcrumbs' WHERE SettingId = 47

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Setting SET ShortName = N'BreadcrumbsCenterHtml' WHERE SettingId = 59

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Setting SET ShortName = N'BreadcrumbsRightHtml' WHERE SettingId = 60

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION