BEGIN TRANSACTION

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL, XACT_ABORT ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Name] = N'Custom Fields', [Description] = N'Manage the Custom Fields of the Entities' WHERE ActionId = '5A25507B-F8B2-4D5C-AC70-71A0DD5C8729'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Name] = N'Entity Custom Fields', [Description] = N'Manage the Custom Fields of the Entity' WHERE ActionId = '6E03B1C3-7AD0-460B-BF73-E3F9055A71DE'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Name] = N'Custom Fields', [Description] = N'Manage the Custom Fields of the Entities' WHERE ActionId = 'ECAAF36B-7D92-4424-BF7A-CFDF85B689CC'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Name] = N'Entity Custom Fields', [Description] = N'Manage the Custom Fields of the Entity' WHERE ActionId = 'F813370C-B0AA-4D7D-A6AB-B8CE4EA7F771'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Name] = N'Custom Field Values', [Description] = N'Manage the Values of the Custom Field' WHERE ActionId = 'A058B233-203B-472C-8D15-86A353ADD11A'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION