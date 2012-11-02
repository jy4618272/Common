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
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_UtcOffset')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_UtcOffset]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Instance_DateFormat')
      ALTER TABLE [dbo].[Mc_Instance] DROP CONSTRAINT [DF_Mc_Instance_DateFormat]

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
   [TimeZoneId] [nvarchar] (100) NULL,
   [TimeFormat] [int] NULL,
   [DateFormat] [int] NULL
) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   INSERT INTO [dbo].[tmp_Mc_Instance] ([InstanceId], [PseudoId], [OrganizationId], [Name], [Description], [EnableSignUpUser], [ExternalId], [WorkingDays], [Active], [CanceledTime], [Trial], [Beta], [Deleted], [CreatedTime], [TimeZoneId], [TimeFormat], [DateFormat])
   SELECT [InstanceId], [PseudoId], [OrganizationId], [Name], [Description], [EnableSignUpUser], [ExternalId], [WorkingDays], [Active], [CanceledTime], [Trial], [Beta], [Deleted], [CreatedTime], CAST(([UTCOffset] - 5) AS nvarchar(100)), CASE WHEN [DateFormat] IN (0, 2) THEN 0 ELSE 1 END, CASE WHEN [DateFormat] IN (0, 1) THEN 0 ELSE 1 END
   FROM [dbo].[Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP TABLE [dbo].[Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[tmp_Mc_Instance] 
		SET TimeZoneId = CASE CAST(TimeZoneId as decimal(5,2)) 
			WHEN 0 THEN 'UTC'
			WHEN 1 THEN 'Central Europe Standard Time'
			WHEN 2 THEN 'E. Europe Standard Time'
			WHEN 3 THEN 'Arabic Standard Time'
			WHEN 3.5 THEN 'Iran Standard Time'
			WHEN 4 THEN 'Russian Standard Time'
			WHEN 4.5 THEN 'Afghanistan Standard Time'
			WHEN 5 THEN 'West Asia Standard Time'
			WHEN 5.5 THEN 'India Standard Time'
			WHEN 5.75 THEN 'Nepal Standard Time'
			WHEN 6 THEN 'Central Asia Standard Time'
			WHEN 6.5 THEN 'Myanmar Standard Time'
			WHEN 7 THEN 'SE Asia Standard Time'
			WHEN 8 THEN 'North Asia Standard Time'
			WHEN 9 THEN 'North Asia East Standard Time'
			WHEN 9.5 THEN 'Cen. Australia Standard Time'
			WHEN -1 THEN 'Azores Standard Time'
			WHEN -2 THEN 'UTC-02'
			WHEN -3 THEN 'E. South America Standard Time'
			WHEN -3.5 THEN 'Newfoundland Standard Time'
			WHEN -4 THEN 'Atlantic Standard Time'
			WHEN -4.5 THEN 'Venezuela Standard Time'
			WHEN -5 THEN 'Eastern Standard Time'
			WHEN -6 THEN 'Central Standard Time'
			WHEN -7 THEN 'Mountain Standard Time'
			WHEN -8 THEN 'Pacific Standard Time'
			WHEN -9 THEN 'Alaskan Standard Time'
			WHEN -10 THEN 'Hawaiian Standard Time'
			WHEN -11 THEN 'UTC-11'
			WHEN -12 THEN 'Dateline Standard Time'
			ELSE NULL END
	WHERE ISNUMERIC(TimeZoneId) = 1

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC sp_rename N'[dbo].[tmp_Mc_Instance]', N'Mc_Instance'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Instance] ADD CONSTRAINT [PK_Mc_Instance] PRIMARY KEY CLUSTERED ([InstanceId]) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_Mc_Instance_OrganizationId] ON [dbo].[Mc_Instance] ([OrganizationId]) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_Mc_Instance_PseudoId] ON [dbo].[Mc_Instance] ([PseudoId]) ON [PRIMARY]

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
   ALTER TABLE [dbo].[Mc_User]
      ADD [TimeZoneId] [nvarchar] (100) NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_User]
      ADD [TimeFormat] [int] NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_User]
      ADD [DateFormat] [int] NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetAnotherAdministrator]
(
	@UserId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT TOP 1 u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, u.TimeZoneId, u.TimeFormat, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	INNER JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId <> uo.UserId) AND (u.Deleted = 0) AND (uo.OrganizationId = @OrganizationId) AND (uo.OrganizationAdministrator = 1);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetInstances]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, 
		ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat, [DateFormat]
	FROM dbo.Mc_Instance 
	WHERE (OrganizationId = @OrganizationId) AND (Deleted = 0);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetUser]
(
	@UserId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT TOP 1 u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, u.TimeZoneId, u.TimeFormat, u.[DateFormat], uo.OrganizationId, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	LEFT JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId = uo.UserId)
	WHERE (u.UserId = @UserId) AND (@OrganizationId IS NULL OR uo.OrganizationId = @OrganizationId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetUserByEmail]
(
	@Email nvarchar(255),
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, u.TimeZoneId, u.TimeFormat, u.[DateFormat], uo.OrganizationId, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	LEFT JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId = uo.UserId)
	WHERE (u.Email = @Email) AND (@OrganizationId IS NULL OR uo.OrganizationId = @OrganizationId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetUsers]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, u.TimeZoneId, u.TimeFormat, u.[DateFormat], uo.OrganizationId, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	INNER JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId = uo.UserId) AND (u.Deleted = 0) AND (uo.OrganizationId = @OrganizationId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetUsersByRoles]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@OrganizationAdministrator bit,
	@Active bit,
	@InRoles varchar(1024),
	@NotInRoles varchar(1024)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT DISTINCT 
		u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, u.TimeZoneId, u.TimeFormat, u.[DateFormat], uo.OrganizationId, uo.OrganizationAdministrator
		, CASE WHEN ((@InstanceId IS NOT NULL) AND (uo.Active = 1) AND (ISNULL(ui.Active, 1) = 1))
			OR ((@InstanceId IS NULL) AND (uo.Active = 1)) THEN 1 ELSE 0 END AS Active
	FROM dbo.Mc_User AS u
	INNER JOIN dbo.Mc_OrganizationsUsers AS uo
		ON	(u.UserId = uo.UserId) 
			AND (u.Deleted = 0) 
			AND (uo.OrganizationId = @OrganizationId) 
			AND ((@OrganizationAdministrator IS NULL) OR (uo.OrganizationAdministrator = @OrganizationAdministrator))
	LEFT JOIN dbo.Mc_UsersInstances AS ui
		ON	(u.UserId = ui.UserId) 
			AND ((@InstanceId IS NULL) OR (ui.InstanceId = @InstanceId))
	WHERE
		((@Active IS NULL) 
			OR (CASE WHEN ((@InstanceId IS NOT NULL) AND (uo.Active = 1) AND (ISNULL(ui.Active, 1) = 1))
				OR ((@InstanceId IS NULL) AND (uo.Active = 1)) THEN 1 ELSE 0 END) = @Active
		)
		AND ((uo.OrganizationAdministrator = 1) OR ((uo.OrganizationAdministrator = 0) AND EXISTS (SELECT 0 FROM dbo.Mc_UsersGroups WHERE UserId = u.UserId)))
		AND ((@InRoles IS NULL) OR EXISTS (
			SELECT 0 
			FROM dbo.Mc_UsersGroups AS ug1
			INNER JOIN dbo.Mc_Group AS g1
				ON	(ug1.GroupId = g1.GroupId) 
					AND (g1.Deleted = 0) 
					AND (g1.OrganizationId = @OrganizationId)
					AND (ug1.UserId = u.UserId) 
			INNER JOIN dbo.Mc_GroupsInstancesRoles AS gir1
				ON	(g1.GroupId = gir1.GroupId) 
					AND ((@InstanceId IS NULL) OR (gir1.InstanceId = @InstanceId))
			WHERE ('','' + @InRoles + '','' LIKE ''%,'' + CAST(gir1.RoleId as varchar(50)) + '',%''))
		)
		AND ((@NotInRoles IS NULL) OR NOT EXISTS (
			SELECT 0 
			FROM dbo.Mc_UsersGroups AS ug2
			INNER JOIN dbo.Mc_Group AS g2
				ON	(ug2.GroupId = g2.GroupId) 
					AND (g2.Deleted = 0) 
					AND (g2.OrganizationId = @OrganizationId)
					AND (ug2.UserId = u.UserId) 
			INNER JOIN dbo.Mc_GroupsInstancesRoles AS gir2
				ON	(g2.GroupId = gir2.GroupId) 
					AND ((@InstanceId IS NULL) OR (gir2.InstanceId = @InstanceId))
			WHERE ('','' + @NotInRoles + '','' LIKE ''%,'' + CAST(gir2.RoleId as varchar(50)) + '',%''))
		);
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
	@WorkingDays char(7),
	@Active bit,
	@CanceledTime datetime,
	@Trial bit,
	@Beta bit,
	@Deleted bit,
	@TimeZoneId nvarchar(255),
	@TimeFormat int,
	@DateFormat int
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Instance (InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat, [DateFormat]) 
	VALUES (@InstanceId, @PseudoId, @OrganizationId, @Name, @Description, @EnableSignUpUser, @ExternalId, @WorkingDays, @Active, @CanceledTime, @Trial, @Beta, @Deleted, GETUTCDATE(), @TimeZoneId, @TimeFormat, @DateFormat);
	
	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat, [DateFormat]
	FROM dbo.Mc_Instance 
	WHERE (InstanceId = @InstanceId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertUser]
(
	@UserId uniqueidentifier,
	@Email nvarchar(255),
	@FirstName nvarchar(255),
	@LastName nvarchar(255),
	@MiddleName nvarchar(255),
    @Phone nvarchar(20),
    @MobilePhone nvarchar(20),
    @Fax nvarchar(20),
	@Title nvarchar(30),
	@Department nvarchar(255),
	@Street nvarchar(255),
	@Street2 nvarchar(255),
	@City nvarchar(255),
	@State nvarchar(255),
	@PostalCode nvarchar(20),
	@Country nvarchar(255),
	@LastLoginDate datetime,
	@Deleted bit,
	@TimeZoneId nvarchar(100),
	@TimeFormat int,
	@DateFormat int
)
AS
BEGIN
	SET NOCOUNT OFF;

	IF NOT EXISTS(SELECT 0 FROM dbo.Mc_User WHERE UserId = @UserId)
		INSERT INTO dbo.Mc_User (UserId, Email, FirstName, LastName, MiddleName, Phone, MobilePhone, Fax, Title, Department, Street, Street2, City, [State], PostalCode, Country, LastLoginDate, Deleted, TimeZoneId, TimeFormat, [DateFormat]) 
		VALUES (@UserId, @Email, @FirstName, @LastName, @MiddleName, @Phone, @MobilePhone, @Fax, @Title, @Department, @Street, @Street2, @City, @State, @PostalCode, @Country, @LastLoginDate, @Deleted, @TimeZoneId, @TimeFormat, @DateFormat);
	
	SELECT UserId, Email, FirstName, LastName, MiddleName, Phone, MobilePhone, Fax, Title, Department, Street, Street2, City, [State], PostalCode, Country, LastLoginDate, Deleted, TimeZoneId, TimeFormat, [DateFormat]
	FROM dbo.Mc_User 
	WHERE (UserId = @UserId);
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
	@WorkingDays char(7),
	@Active bit,
	@CanceledTime datetime,
	@Trial bit,
	@Beta bit,
	@Deleted bit,
	@TimeZoneId nvarchar(255),
	@TimeFormat int,
	@DateFormat int
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Instance 
	SET PseudoId  = @PseudoId, OrganizationId = @OrganizationId, [Name] = @Name, [Description] = @Description, EnableSignUpUser = @EnableSignUpUser, ExternalId = @ExternalId, WorkingDays = @WorkingDays, Active = @Active, CanceledTime = @CanceledTime, Trial = @Trial, Beta = @Beta, Deleted = @Deleted, TimeZoneId = @TimeZoneId, TimeFormat = @TimeFormat, [DateFormat] = @DateFormat
	WHERE (InstanceId = @InstanceId);
	
	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat, [DateFormat]
	FROM dbo.Mc_Instance 
	WHERE (InstanceId = @InstanceId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateUser]
(
	@UserId uniqueidentifier,
	@Email nvarchar(255),
	@FirstName nvarchar(255),
	@LastName nvarchar(255),
	@MiddleName nvarchar(255),
	@Phone nvarchar(20),
    @MobilePhone nvarchar(20),
    @Fax nvarchar(20),
	@Title nvarchar(30),
	@Department nvarchar(255),
	@Street nvarchar(255),
	@Street2 nvarchar(255),
	@City nvarchar(255),
	@State nvarchar(255),
	@PostalCode nvarchar(20),
	@Country nvarchar(255),
	@LastLoginDate datetime,
	@Deleted bit,
	@TimeZoneId nvarchar(100),
	@TimeFormat int,
	@DateFormat int
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_User 
	SET UserId = @UserId, Email = ISNULL(@Email, Email), FirstName = ISNULL(@FirstName, FirstName), LastName = ISNULL(@LastName, LastName), MiddleName = ISNULL(@MiddleName, MiddleName)
		, Phone = ISNULL(@Phone, Phone), MobilePhone = ISNULL(@MobilePhone, MobilePhone), Fax = ISNULL(@Fax, Fax), Title = ISNULL(@Title, Title), Department = ISNULL(@Department, Department)
		, Street = ISNULL(@Street, Street), Street2 = ISNULL(@Street2, Street2), City = ISNULL(@City, City), [State] = ISNULL(@State, [State])
		, PostalCode = ISNULL(@PostalCode, PostalCode), Country = ISNULL(@Country, Country)
		, LastLoginDate = ISNULL(@LastLoginDate, LastLoginDate), Deleted = ISNULL(@Deleted, Deleted)
		, TimeZoneId = @TimeZoneId, TimeFormat = @TimeFormat, [DateFormat] = @DateFormat
	WHERE (UserId = @UserId);
	
	SELECT UserId, Email, FirstName, LastName, MiddleName, Phone, MobilePhone, Fax, Title, Department, Street, Street2, City, [State], PostalCode, Country, LastLoginDate, Deleted, TimeZoneId, TimeFormat, [DateFormat]
	FROM dbo.Mc_User 
	WHERE (UserId = @UserId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
