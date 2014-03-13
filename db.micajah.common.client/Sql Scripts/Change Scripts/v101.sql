﻿BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Instance] SET [DateFormat] = 0 WHERE [DateFormat] IS NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Instance] SET [TimeFormat] = 0 WHERE [TimeFormat] IS NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Instance] SET [TimeZoneId] = 'Eastern Standard Time' WHERE LEN(ISNULL([TimeZoneId], '')) = 0

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Instance]
      ALTER COLUMN [DateFormat] [int] NOT NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Instance]
      ALTER COLUMN [TimeFormat] [int] NOT NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Instance]
      ALTER COLUMN [TimeZoneId] [nvarchar] (100) NOT NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_DateFormat')
      ALTER TABLE [dbo].[Mc_Instance] ADD CONSTRAINT [DF_Mc_Instance_DateFormat] DEFAULT ((0)) FOR [DateFormat]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_TimeFormat')
      ALTER TABLE [dbo].[Mc_Instance] ADD CONSTRAINT [DF_Mc_Instance_TimeFormat] DEFAULT ((0)) FOR [TimeFormat]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_TimeZoneId')
      ALTER TABLE [dbo].[Mc_Instance] ADD CONSTRAINT [DF_Mc_Instance_TimeZoneId] DEFAULT ('') FOR [TimeZoneId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION