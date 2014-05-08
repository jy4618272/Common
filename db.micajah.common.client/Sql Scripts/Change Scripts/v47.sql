BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Instance]
      DROP COLUMN [LogoImageResourceId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetInstances]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT InstanceId, OrganizationId, [Name], [Description], EnableSignUpUser, Deleted 
	FROM dbo.Mc_Instance 
	WHERE (Deleted = 0) AND (OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertInstance]
(
	@InstanceId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@EnableSignUpUser bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Instance (InstanceId, OrganizationId, [Name], [Description], EnableSignUpUser, Deleted) 
	VALUES (@InstanceId, @OrganizationId, @Name, @Description, @EnableSignUpUser, @Deleted);
	
	SELECT InstanceId, OrganizationId, [Name], [Description], EnableSignUpUser, Deleted 
	FROM dbo.Mc_Instance 
	WHERE (InstanceId = @InstanceId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateInstance]
(
	@InstanceId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@EnableSignUpUser bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Instance 
	SET OrganizationId = @OrganizationId, [Name] = @Name, [Description] = @Description, EnableSignUpUser = @EnableSignUpUser, Deleted = @Deleted 
	WHERE (InstanceId = @InstanceId);
	
	SELECT InstanceId, OrganizationId, [Name], [Description], EnableSignUpUser, Deleted 
	FROM dbo.Mc_Instance 
	WHERE (InstanceId = @InstanceId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT
