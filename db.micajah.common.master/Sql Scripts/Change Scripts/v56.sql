BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_OrganizationsLogins_Mc_Login')
      ALTER TABLE [dbo].[Mc_OrganizationsLogins] DROP CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Login]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_OrganizationsLogins_Mc_Organization')
      ALTER TABLE [dbo].[Mc_OrganizationsLogins] DROP CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Organization]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_OrganizationsLogins_OrganizationAdministrator')
      ALTER TABLE [dbo].[Mc_OrganizationsLogins] DROP CONSTRAINT [DF_Mc_OrganizationsLogins_OrganizationAdministrator]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE TABLE [dbo].[tmp_Mc_OrganizationsLogins] (
   [OrganizationId] [uniqueidentifier] NOT NULL,
   [LoginId] [uniqueidentifier] NOT NULL,
   [OrganizationAdministrator] [bit] NOT NULL CONSTRAINT [DF_Mc_OrganizationsLogins_OrganizationAdministrator] DEFAULT ((0)),
   [Active] [bit] NOT NULL CONSTRAINT [DF_Mc_OrganizationsLogins_Active] DEFAULT ((1))
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   INSERT INTO [dbo].[tmp_Mc_OrganizationsLogins] ([OrganizationId], [LoginId], [OrganizationAdministrator])
   SELECT [OrganizationId], [LoginId], [OrganizationAdministrator]
   FROM [dbo].[Mc_OrganizationsLogins]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP TABLE [dbo].[Mc_OrganizationsLogins]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC sp_rename N'[dbo].[tmp_Mc_OrganizationsLogins]', N'Mc_OrganizationsLogins'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_OrganizationsLogins] ADD CONSTRAINT [PK_Mc_OrganizationsLogins] PRIMARY KEY CLUSTERED ([OrganizationId], [LoginId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetLoginsByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT l.LoginId, l.LoginName, l.[Password], l.Deleted, ol.OrganizationAdministrator, ol.Active
	FROM dbo.Mc_OrganizationsLogins AS ol
	INNER JOIN dbo.Mc_Login AS l
		ON	(ol.LoginId = l.LoginId) AND (ol.OrganizationId = @OrganizationId) AND (l.Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizationLogin]
(
	@OrganizationId uniqueidentifier,
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT OrganizationId, LoginId, OrganizationAdministrator, Active
	FROM dbo.Mc_OrganizationsLogins
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertOrganizationLogin]
(
	@OrganizationId uniqueidentifier,
	@LoginId uniqueidentifier,
	@OrganizationAdministrator bit,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_OrganizationsLogins (OrganizationId, LoginId, OrganizationAdministrator, Active)
	VALUES (@OrganizationId, @LoginId, @OrganizationAdministrator, @Active);
	
	SELECT OrganizationId, LoginId, OrganizationAdministrator, Active
	FROM dbo.Mc_OrganizationsLogins
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateOrganizationLogin]
(
	@OrganizationId uniqueidentifier,
	@LoginId uniqueidentifier,
	@OrganizationAdministrator bit,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_OrganizationsLogins 
	SET OrganizationAdministrator = (CASE WHEN @OrganizationAdministrator IS NULL THEN OrganizationAdministrator ELSE @OrganizationAdministrator END)
		, Active = (CASE WHEN @Active IS NULL THEN Active ELSE @Active END)
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
	
	SELECT OrganizationId, LoginId, OrganizationAdministrator, Active
	FROM dbo.Mc_OrganizationsLogins
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_OrganizationsLogins_Mc_Login')
      ALTER TABLE [dbo].[Mc_OrganizationsLogins] DROP CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Login]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_OrganizationsLogins_Mc_Login')
      ALTER TABLE [dbo].[Mc_OrganizationsLogins] ADD CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Login] FOREIGN KEY ([LoginId]) REFERENCES [dbo].[Mc_Login] ([LoginId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_OrganizationsLogins_Mc_Organization')
      ALTER TABLE [dbo].[Mc_OrganizationsLogins] DROP CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Organization]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_OrganizationsLogins_Mc_Organization')
      ALTER TABLE [dbo].[Mc_OrganizationsLogins] ADD CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
