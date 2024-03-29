﻿BEGIN TRANSACTION

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL, XACT_ABORT ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action SET [ClassFullName] = N'Micajah.Common.Bll.Providers.ActionProvider' WHERE ActionId = 'DAB9B65E-0358-408E-A2F1-2D616FCA33EC'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action SET [ClassFullName] = N'Micajah.Common.Bll.Providers.ActionProvider' WHERE ActionId = 'EF0F170E-CF70-4C8B-ACE2-E7E2E8096821'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Action SET [ClassFullName] = N'Micajah.Common.Bll.Providers.ActionProvider' WHERE ActionId = 'D63CAC58-9B6E-46A4-B845-68332F759DFB'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
