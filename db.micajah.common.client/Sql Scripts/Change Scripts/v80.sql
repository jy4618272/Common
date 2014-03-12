BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Instance
		DROP CONSTRAINT DF_Mc_Instance_InstanceId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Instance
		DROP CONSTRAINT DF_Mc_Instance_PseudoId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Instance
		DROP CONSTRAINT DF_Mc_Instance_Description

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Instance
		DROP CONSTRAINT DF_Mc_Instance_EnableSignUpUser

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Instance
		DROP CONSTRAINT DF_Mc_Instance_Deleted

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE dbo.Tmp_Mc_Instance
	(
	InstanceId uniqueidentifier NOT NULL,
	PseudoId varchar(6) NOT NULL,
	OrganizationId uniqueidentifier NOT NULL,
	Name nvarchar(255) NOT NULL,
	Description nvarchar(1024) NOT NULL,
	EnableSignUpUser bit NOT NULL,
	ExternalId nvarchar(255) NOT NULL,
	UTCOffset int NOT NULL,
	DateFormat int NOT NULL,
	WorkingDays char(7) NOT NULL,
	Active bit NOT NULL,
	CanceledTime datetime NULL,
	Trial bit NOT NULL,
	Beta bit NOT NULL,
	Deleted bit NOT NULL,
	CreatedTime datetime NULL
	)  ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Instance ADD CONSTRAINT
		DF_Mc_Instance_InstanceId DEFAULT (newid()) FOR InstanceId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Instance ADD CONSTRAINT
		DF_Mc_Instance_PseudoId DEFAULT ('') FOR PseudoId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Instance ADD CONSTRAINT
		DF_Mc_Instance_Description DEFAULT (N'') FOR Description

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Instance ADD CONSTRAINT
		DF_Mc_Instance_EnableSignUpUser DEFAULT ((0)) FOR EnableSignUpUser

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Instance ADD CONSTRAINT
		DF_Mc_Instance_ExternalId DEFAULT (N'') FOR ExternalId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Instance ADD CONSTRAINT
		DF_Mc_Instance_UTCOffset DEFAULT ((0)) FOR UTCOffset

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Instance ADD CONSTRAINT
		DF_Mc_Instance_DateFormat DEFAULT ((0)) FOR DateFormat

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Instance ADD CONSTRAINT
		DF_Mc_Instance_WorkingDays DEFAULT ((1111100)) FOR WorkingDays

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Instance ADD CONSTRAINT
		DF_Mc_Instance_Active DEFAULT ((1)) FOR Active

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Instance ADD CONSTRAINT
		DF_Mc_Instance_Trial DEFAULT ((0)) FOR Trial

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Instance ADD CONSTRAINT
		DF_Mc_Instance_Beta DEFAULT ((0)) FOR Beta

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Instance ADD CONSTRAINT
		DF_Mc_Instance_Deleted DEFAULT ((0)) FOR Deleted

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS(SELECT * FROM dbo.Mc_Instance)
		 EXEC('INSERT INTO dbo.Tmp_Mc_Instance (InstanceId, PseudoId, OrganizationId, Name, Description, EnableSignUpUser, Deleted)
			SELECT InstanceId, PseudoId, OrganizationId, Name, Description, EnableSignUpUser, Deleted FROM dbo.Mc_Instance WITH (HOLDLOCK TABLOCKX)')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_EntityField
		DROP CONSTRAINT FK_Mc_EntityField_Mc_Instance

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_EntityNode
		DROP CONSTRAINT FK_Mc_EntityNode_Mc_Instance

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_EntityNodeType
		DROP CONSTRAINT FK_Mc_EntityNodeType_Mc_Instance

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_GroupsInstancesActions
		DROP CONSTRAINT FK_Mc_GroupsInstancesActions_Mc_Instance

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_GroupsInstancesRoles
		DROP CONSTRAINT FK_Mc_GroupsInstancesRoles_Mc_Instance

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_SettingsValues
		DROP CONSTRAINT FK_Mc_SettingsValues_Mc_Instance

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_UsersInstances
		DROP CONSTRAINT FK_Mc_UsersInstances_Mc_Instance

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP TABLE dbo.Mc_Instance

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXECUTE sp_rename N'dbo.Tmp_Mc_Instance', N'Mc_Instance', 'OBJECT' 

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Instance ADD CONSTRAINT
	PK_Mc_Instance PRIMARY KEY CLUSTERED 
	(
	InstanceId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_UsersInstances ADD CONSTRAINT
	FK_Mc_UsersInstances_Mc_Instance FOREIGN KEY
	(
	InstanceId
	) REFERENCES dbo.Mc_Instance
	(
	InstanceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_SettingsValues ADD CONSTRAINT
	FK_Mc_SettingsValues_Mc_Instance FOREIGN KEY
	(
	InstanceId
	) REFERENCES dbo.Mc_Instance
	(
	InstanceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_GroupsInstancesRoles ADD CONSTRAINT
	FK_Mc_GroupsInstancesRoles_Mc_Instance FOREIGN KEY
	(
	InstanceId
	) REFERENCES dbo.Mc_Instance
	(
	InstanceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_GroupsInstancesActions ADD CONSTRAINT
	FK_Mc_GroupsInstancesActions_Mc_Instance FOREIGN KEY
	(
	InstanceId
	) REFERENCES dbo.Mc_Instance
	(
	InstanceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_EntityNodeType ADD CONSTRAINT
	FK_Mc_EntityNodeType_Mc_Instance FOREIGN KEY
	(
	InstanceId
	) REFERENCES dbo.Mc_Instance
	(
	InstanceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_EntityNode ADD CONSTRAINT
	FK_Mc_EntityNode_Mc_Instance FOREIGN KEY
	(
	InstanceId
	) REFERENCES dbo.Mc_Instance
	(
	InstanceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_EntityField ADD CONSTRAINT
	FK_Mc_EntityField_Mc_Instance FOREIGN KEY
	(
	InstanceId
	) REFERENCES dbo.Mc_Instance
	(
	InstanceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
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

	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, 
		ExternalId, UTCOffset, [DateFormat], WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime 
	FROM dbo.Mc_Instance 
	WHERE (OrganizationId = @OrganizationId) AND (Deleted = 0);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertInstance]
(
	@InstanceId uniqueidentifier,
	@PseudoId varchar(6),
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@EnableSignUpUser bit,
	@ExternalId nvarchar(255),
	@UTCOffset int,
	@DateFormat int,
	@WorkingDays char(7),
	@Active bit,
	@CanceledTime datetime,
	@Trial bit,
	@Beta bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Instance (InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, ExternalId, UTCOffset, [DateFormat], WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime) 
	VALUES (@InstanceId, @PseudoId, @OrganizationId, @Name, @Description, @EnableSignUpUser, @ExternalId, @UTCOffset, @DateFormat, @WorkingDays, @Active, @CanceledTime, @Trial, @Beta, @Deleted, GETDATE());
	
	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, ExternalId, UTCOffset, [DateFormat], WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime 
	FROM dbo.Mc_Instance 
	WHERE (InstanceId = @InstanceId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateInstance]
(
	@InstanceId uniqueidentifier,
	@PseudoId varchar(6),
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@EnableSignUpUser bit,
	@ExternalId nvarchar(255),
	@UTCOffset int,
	@DateFormat int,
	@WorkingDays char(7),
	@Active bit,
	@CanceledTime datetime,
	@Trial bit,
	@Beta bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Instance 
	SET PseudoId  = @PseudoId, OrganizationId = @OrganizationId, [Name] = @Name, [Description] = @Description, EnableSignUpUser = @EnableSignUpUser, ExternalId = @ExternalId, UTCOffset = @UTCOffset, [DateFormat] = @DateFormat, WorkingDays = @WorkingDays, Active = @Active, CanceledTime = @CanceledTime, Trial = @Trial, Beta = @Beta, Deleted = @Deleted 
	WHERE (InstanceId = @InstanceId);
	
	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, ExternalId, UTCOffset, [DateFormat], WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime 
	FROM dbo.Mc_Instance 
	WHERE (InstanceId = @InstanceId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
