BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action] 
			([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) 
	VALUES	('709A6B39-36A2-43FC-AF18-24C2E3332D7A', '00000000-0000-0000-0000-000000000001', 3, N'Log In as User', N'Log In as another User', N'', N'', N'/Resources.Micajah.Common/Pages/Security/LoginAsUser.aspx', 10004, N'', 1, 0, 1, 0, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action
	SET ParentActionId = '00000000-0000-0000-0000-000000000007'
		, [Description] = N'Log In as another User'
		, AuthenticationRequired = 1
		, OrderNumber = 0
		, Visible = 0
		, ShowInDetailMenu = 0
	WHERE ActionId = '00000000-0000-0000-0000-000000000041'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action
	SET [Name] = N'MC Setup'
		, [Description] = N'Micajah.Common Setup'
		, AuthenticationRequired = 1
	WHERE ActionId = '00000000-0000-0000-0000-000000000027'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action
	SET [Description] = N'The Entities which are registered in the Framework'
		, AuthenticationRequired = 1
	WHERE ActionId = '00000000-0000-0000-0000-000000000008'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action
	SET [Description] = N'Framework information and management'
		, AuthenticationRequired = 0
	WHERE ActionId = '00000000-0000-0000-0000-000000000028'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action
	SET [Description] = N'Manage the Actions'
		, AuthenticationRequired = 1
	WHERE ActionId = '00000000-0000-0000-0000-000000000030'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action
	SET [Description] = N'Manage the Roles'
		, AuthenticationRequired = 1
	WHERE ActionId = '00000000-0000-0000-0000-000000000031'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action
	SET [Description] = N'Manage the Settings'
		, AuthenticationRequired = 1
	WHERE ActionId = '00000000-0000-0000-0000-000000000034'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action
	SET [Description] = N'Manage the Websites'
		, AuthenticationRequired = 1
	WHERE ActionId = '00000000-0000-0000-0000-000000000037'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action
	SET [Description] = N'Manage the Database Servers'
		, AuthenticationRequired = 1
	WHERE ActionId = '00000000-0000-0000-0000-000000000038'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action
	SET [Description] = N'Manage the Databases'
		, AuthenticationRequired = 1
	WHERE ActionId = '00000000-0000-0000-0000-000000000039'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action
	SET [Description] = N'Manage the Organizations'
		, AuthenticationRequired = 1
	WHERE ActionId = '00000000-0000-0000-0000-000000000040'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action
	SET [Description] = N'The Icons which are embedded into Framework'
		, AuthenticationRequired = 1
	WHERE ActionId = '042D39D6-A607-4063-808F-0D75BA9CB899'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action
	SET OrderNumber = 10000
		, [Name] = N'MC Setup'
		, [Description] = N'Micajah.Common Setup'
		, NavigateUrl = '/Resources.Micajah.Common/Pages/DetailMenu.aspx?pageid=00000000000000000000000000000027'
		, AuthenticationRequired = 1
	WHERE ActionId = '00000000-0000-0000-0000-000000000042'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action
	SET [Description] = N'Log In'
		, OrderNumber = 10005
	WHERE ActionId = '00000000-0000-0000-0000-000000000043'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action
	SET AuthenticationRequired = 1
	WHERE ActionId IN ('00000000-0000-0000-0000-000000000032', '00000000-0000-0000-0000-000000000033', '00000000-0000-0000-0000-000000000035', '00000000-0000-0000-0000-000000000036')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION