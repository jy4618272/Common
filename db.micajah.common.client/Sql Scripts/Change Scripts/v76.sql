BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_UsersInstances] (
   [UserId] [uniqueidentifier] NOT NULL,
   [InstanceId] [uniqueidentifier] NOT NULL,
   [Active] [bit] NOT NULL CONSTRAINT [DF_Mc_UsersInstances_Active] DEFAULT ((1))
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_UsersInstances] ADD CONSTRAINT [PK_Mc_UsersInstances] PRIMARY KEY CLUSTERED ([UserId], [InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteUserInstance]
(
	@UserId uniqueidentifier,
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_UsersInstances 
	WHERE (UserId = @UserId) AND (InstanceId = @InstanceId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetUsersInstances]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ui.UserId, ui.InstanceId, ui.Active 
	FROM dbo.Mc_UsersInstances AS ui
	INNER JOIN dbo.Mc_Instance AS i
		ON	(ui.InstanceId = i.InstanceId) AND (i.Deleted = 0) AND (i.OrganizationId = @OrganizationId)
	INNER JOIN dbo.Mc_User AS u
		ON	(ui.UserId = u.UserId) AND (u.Deleted = 0);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertUserInstance]
(
	@UserId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_UsersInstances (UserId, InstanceId, Active) 
	VALUES (@UserId, @InstanceId, @Active);
	
	SELECT UserId, InstanceId, Active 
	FROM dbo.Mc_UsersInstances 
	WHERE (UserId = @UserId) AND (InstanceId = @InstanceId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateUserInstance]
(
	@UserId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_UsersInstances 
	SET Active = @Active
	WHERE (UserId = @UserId) AND (InstanceId = @InstanceId);
	
	SELECT UserId, InstanceId, Active 
	FROM dbo.Mc_UsersInstances 
	WHERE (UserId = @UserId) AND (InstanceId = @InstanceId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_UsersInstances_Mc_Instance')
      ALTER TABLE [dbo].[Mc_UsersInstances] DROP CONSTRAINT [FK_Mc_UsersInstances_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_UsersInstances_Mc_Instance')
      ALTER TABLE [dbo].[Mc_UsersInstances] ADD CONSTRAINT [FK_Mc_UsersInstances_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_UsersInstances_Mc_User')
      ALTER TABLE [dbo].[Mc_UsersInstances] DROP CONSTRAINT [FK_Mc_UsersInstances_Mc_User]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_UsersInstances_Mc_User')
      ALTER TABLE [dbo].[Mc_UsersInstances] ADD CONSTRAINT [FK_Mc_UsersInstances_Mc_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Mc_User] ([UserId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
