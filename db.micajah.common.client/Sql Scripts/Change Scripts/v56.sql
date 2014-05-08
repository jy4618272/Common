BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_OrganizationsUsers_Mc_User')
      ALTER TABLE [dbo].[Mc_OrganizationsUsers] DROP CONSTRAINT [FK_Mc_OrganizationsUsers_Mc_User]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_OrganizationsUsers_OrganizationAdministrator')
      ALTER TABLE [dbo].[Mc_OrganizationsUsers] DROP CONSTRAINT [DF_Mc_OrganizationsUsers_OrganizationAdministrator]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE TABLE [dbo].[tmp_Mc_OrganizationsUsers] (
   [OrganizationId] [uniqueidentifier] NOT NULL,
   [UserId] [uniqueidentifier] NOT NULL,
   [OrganizationAdministrator] [bit] NOT NULL CONSTRAINT [DF_Mc_OrganizationsUsers_OrganizationAdministrator] DEFAULT ((0)),
   [Active] [bit] NOT NULL CONSTRAINT [DF_Mc_OrganizationsUsers_Active] DEFAULT ((1))
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   INSERT INTO [dbo].[tmp_Mc_OrganizationsUsers] ([OrganizationId], [UserId], [OrganizationAdministrator])
   SELECT [OrganizationId], [UserId], [OrganizationAdministrator]
   FROM [dbo].[Mc_OrganizationsUsers]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP TABLE [dbo].[Mc_OrganizationsUsers]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC sp_rename N'[dbo].[tmp_Mc_OrganizationsUsers]', N'Mc_OrganizationsUsers'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_OrganizationsUsers] ADD CONSTRAINT [PK_Mc_OrganizationsUsers] PRIMARY KEY CLUSTERED ([OrganizationId], [UserId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetUsers]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	INNER JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId = uo.UserId) AND (u.Deleted = 0) AND (uo.OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertOrganizationUser]
(
	@OrganizationId uniqueidentifier,
	@UserId uniqueidentifier,
	@OrganizationAdministrator bit,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_OrganizationsUsers (OrganizationId, UserId, OrganizationAdministrator, Active) 
	VALUES (@OrganizationId, @UserId, @OrganizationAdministrator, @Active);
	
	SELECT OrganizationId, UserId, OrganizationAdministrator, Active
	FROM dbo.Mc_OrganizationsUsers
	WHERE (OrganizationId = @OrganizationId) AND (UserId = @UserId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateOrganizationUser]
(
	@OrganizationId uniqueidentifier,
	@UserId uniqueidentifier,
	@OrganizationAdministrator bit,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_OrganizationsUsers 
	SET OrganizationAdministrator = (CASE WHEN @OrganizationAdministrator IS NULL THEN OrganizationAdministrator ELSE @OrganizationAdministrator END)
		, Active = (CASE WHEN @Active IS NULL THEN Active ELSE @Active END)
	WHERE (OrganizationId = @OrganizationId) AND (UserId = @UserId);

	SELECT OrganizationId, UserId, OrganizationAdministrator, Active
	FROM dbo.Mc_OrganizationsUsers
	WHERE (OrganizationId = @OrganizationId) AND (UserId = @UserId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_OrganizationsUsers_Mc_User')
      ALTER TABLE [dbo].[Mc_OrganizationsUsers] DROP CONSTRAINT [FK_Mc_OrganizationsUsers_Mc_User]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_OrganizationsUsers_Mc_User')
      ALTER TABLE [dbo].[Mc_OrganizationsUsers] ADD CONSTRAINT [FK_Mc_OrganizationsUsers_Mc_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Mc_User] ([UserId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
