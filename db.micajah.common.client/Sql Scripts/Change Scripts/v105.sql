BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityField_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityField] DROP CONSTRAINT [FK_Mc_EntityField_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNode_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityNode] DROP CONSTRAINT [FK_Mc_EntityNode_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNodeType_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityNodeType] DROP CONSTRAINT [FK_Mc_EntityNodeType_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsInstancesActions_Mc_Instance')
      ALTER TABLE [dbo].[Mc_GroupsInstancesActions] DROP CONSTRAINT [FK_Mc_GroupsInstancesActions_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsInstancesRoles_Mc_Instance')
      ALTER TABLE [dbo].[Mc_GroupsInstancesRoles] DROP CONSTRAINT [FK_Mc_GroupsInstancesRoles_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_SettingsValues_Mc_Instance')
      ALTER TABLE [dbo].[Mc_SettingsValues] DROP CONSTRAINT [FK_Mc_SettingsValues_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_UsersInstances_Mc_Instance')
      ALTER TABLE [dbo].[Mc_UsersInstances] DROP CONSTRAINT [FK_Mc_UsersInstances_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_InstanceId')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_InstanceId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_PseudoId')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_PseudoId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_Description')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_Description]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_EnableSignUpUser')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_EnableSignUpUser]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_ExternalId')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_ExternalId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_WorkingDays')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_WorkingDays]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_Active')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_Active]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_Trial')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_Trial]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_Beta')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_Beta]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_Deleted')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_TimeZoneId')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_TimeZoneId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_TimeFormat')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_TimeFormat]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_DateFormat')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_DateFormat]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE TABLE [dbo].[tmp_Mc_Instance] (
   [InstanceId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Mc_Instance_InstanceId] DEFAULT (newid()),
   [PseudoId] [varchar] (6) NOT NULL CONSTRAINT [DF_Mc_Instance_PseudoId] DEFAULT (''),
   [OrganizationId] [uniqueidentifier] NOT NULL,
   [Name] [nvarchar] (255) NOT NULL,
   [Description] [nvarchar] (1024) NOT NULL CONSTRAINT [DF_Mc_Instance_Description] DEFAULT (N''),
   [EnableSignUpUser] [bit] NOT NULL CONSTRAINT [DF_Mc_Instance_EnableSignUpUser] DEFAULT ((0)),
   [ExternalId] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_Instance_ExternalId] DEFAULT (N''),
   [WorkingDays] [char] (7) NOT NULL CONSTRAINT [DF_Mc_Instance_WorkingDays] DEFAULT ((1111100)),
   [Active] [bit] NOT NULL CONSTRAINT [DF_Mc_Instance_Active] DEFAULT ((1)),
   [CanceledTime] [datetime] NULL,
   [Trial] [bit] NOT NULL CONSTRAINT [DF_Mc_Instance_Trial] DEFAULT ((0)),
   [Beta] [bit] NOT NULL CONSTRAINT [DF_Mc_Instance_Beta] DEFAULT ((0)),
   [Deleted] [bit] NOT NULL CONSTRAINT [DF_Mc_Instance_Deleted] DEFAULT ((0)),
   [CreatedTime] [datetime] NULL,
   [TimeZoneId] [nvarchar] (100) NOT NULL CONSTRAINT [DF_Mc_Instance_TimeZoneId] DEFAULT (''),
   [TimeFormat] [int] NOT NULL CONSTRAINT [DF_Mc_Instance_TimeFormat] DEFAULT ((0)),
   [DateFormat] [int] NOT NULL CONSTRAINT [DF_Mc_Instance_DateFormat] DEFAULT ((0)),
   [BillingPlan] [tinyint] NOT NULL CONSTRAINT [DF_Mc_Instance_BillingPlan] DEFAULT ((0)),
   [CreditCardStatus] [tinyint] NOT NULL CONSTRAINT [DF_Mc_Instance_CreditCardStatus] DEFAULT ((0))
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   INSERT INTO [dbo].[tmp_Mc_Instance] ([InstanceId], [PseudoId], [OrganizationId], [Name], [Description], [EnableSignUpUser], [ExternalId], [WorkingDays], [Active], [CanceledTime], [Trial], [Beta], [Deleted], [CreatedTime], [TimeZoneId], [TimeFormat], [DateFormat])
   SELECT [InstanceId], [PseudoId], [OrganizationId], [Name], [Description], [EnableSignUpUser], [ExternalId], [WorkingDays], [Active], [CanceledTime], [Trial], [Beta], [Deleted], [CreatedTime], [TimeZoneId], [TimeFormat], [DateFormat]
   FROM [dbo].[Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP TABLE [dbo].[Mc_Instance]

EXEC sp_rename N'[dbo].[tmp_Mc_Instance]', N'Mc_Instance'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Instance] ADD CONSTRAINT [PK_Mc_Instance] PRIMARY KEY CLUSTERED ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_Mc_Instance_OrganizationId] ON [dbo].[Mc_Instance] ([OrganizationId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_Mc_Instance_PseudoId] ON [dbo].[Mc_Instance] ([PseudoId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec('ALTER PROCEDURE [dbo].[Mc_GetInstances]
	(
		@OrganizationId uniqueidentifier
	)
	AS
	BEGIN
		SET NOCOUNT OFF;

		SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, 
			ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat, [DateFormat], BillingPlan, CreditCardStatus
		FROM dbo.Mc_Instance 
		WHERE (OrganizationId = @OrganizationId) AND (Deleted = 0);
	END
	')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec('ALTER PROCEDURE [dbo].[Mc_InsertInstance]
	(
		@InstanceId uniqueidentifier,
		@PseudoId varchar(6),
		@OrganizationId uniqueidentifier,
		@Name nvarchar(255),
		@Description nvarchar(1024),
		@EnableSignUpUser bit,
		@ExternalId nvarchar(255),
		@WorkingDays char(7),
		@Active bit,
		@CanceledTime datetime,
		@Trial bit,
		@Beta bit,
		@Deleted bit,
		@TimeZoneId nvarchar(255),
		@TimeFormat int,
		@DateFormat int,
		@BillingPlan tinyint,
		@CreditCardStatus tinyint
	)
	AS
	BEGIN
		SET NOCOUNT OFF;

		INSERT INTO dbo.Mc_Instance (InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat, [DateFormat], BillingPlan, CreditCardStatus) 
		VALUES (@InstanceId, @PseudoId, @OrganizationId, @Name, @Description, @EnableSignUpUser, @ExternalId, @WorkingDays, @Active, @CanceledTime, @Trial, @Beta, @Deleted, GETUTCDATE(), @TimeZoneId, @TimeFormat, @DateFormat, @BillingPlan, @CreditCardStatus);
		
		SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat, [DateFormat], BillingPlan, CreditCardStatus
		FROM dbo.Mc_Instance 
		WHERE (InstanceId = @InstanceId);
	END
	')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	exec('ALTER PROCEDURE [dbo].[Mc_UpdateInstance]
	(
		@InstanceId uniqueidentifier,
		@PseudoId varchar(6),
		@OrganizationId uniqueidentifier,
		@Name nvarchar(255),
		@Description nvarchar(1024),
		@EnableSignUpUser bit,
		@ExternalId nvarchar(255),
		@WorkingDays char(7),
		@Active bit,
		@CanceledTime datetime,
		@Trial bit,
		@Beta bit,
		@Deleted bit,
		@TimeZoneId nvarchar(255),
		@TimeFormat int,
		@DateFormat int,
		@BillingPlan tinyint,
		@CreditCardStatus tinyint
	)
	AS
	BEGIN
		SET NOCOUNT OFF;

		UPDATE dbo.Mc_Instance 
		SET PseudoId  = @PseudoId, OrganizationId = @OrganizationId, [Name] = @Name, [Description] = @Description, EnableSignUpUser = @EnableSignUpUser, ExternalId = @ExternalId, WorkingDays = @WorkingDays, Active = @Active, CanceledTime = @CanceledTime, Trial = @Trial, Beta = @Beta, Deleted = @Deleted, TimeZoneId = @TimeZoneId, TimeFormat = @TimeFormat, [DateFormat] = @DateFormat, BillingPlan = @BillingPlan, CreditCardStatus=@CreditCardStatus
		WHERE (InstanceId = @InstanceId);
		
		SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat, [DateFormat], BillingPlan, CreditCardStatus
		FROM dbo.Mc_Instance 
		WHERE (InstanceId = @InstanceId);
	END
	')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityField_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityField] ADD CONSTRAINT [FK_Mc_EntityField_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNode_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityNode] ADD CONSTRAINT [FK_Mc_EntityNode_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNodeType_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityNodeType] ADD CONSTRAINT [FK_Mc_EntityNodeType_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsInstancesActions_Mc_Instance')
      ALTER TABLE [dbo].[Mc_GroupsInstancesActions] ADD CONSTRAINT [FK_Mc_GroupsInstancesActions_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsInstancesRoles_Mc_Instance')
      ALTER TABLE [dbo].[Mc_GroupsInstancesRoles] ADD CONSTRAINT [FK_Mc_GroupsInstancesRoles_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_SettingsValues_Mc_Instance')
      ALTER TABLE [dbo].[Mc_SettingsValues] ADD CONSTRAINT [FK_Mc_SettingsValues_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_UsersInstances_Mc_Instance')
      ALTER TABLE [dbo].[Mc_UsersInstances] ADD CONSTRAINT [FK_Mc_UsersInstances_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
