BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsDepartmentsActions_Mc_Department')
      ALTER TABLE [dbo].[Mc_GroupsDepartmentsActions] DROP CONSTRAINT [FK_Mc_GroupsDepartmentsActions_Mc_Department]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsDepartmentsRoles_Mc_Department')
      ALTER TABLE [dbo].[Mc_GroupsDepartmentsRoles] DROP CONSTRAINT [FK_Mc_GroupsDepartmentsRoles_Mc_Department]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_SettingValue_Mc_Department')
      ALTER TABLE [dbo].[Mc_SettingValue] DROP CONSTRAINT [FK_Mc_SettingValue_Mc_Department]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Department_Description')
      ALTER TABLE [dbo].[Mc_Department] DROP CONSTRAINT [DF_Mc_Department_Description]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Department_EnableSignUpUser')
      ALTER TABLE [dbo].[Mc_Department] DROP CONSTRAINT [DF_Mc_Department_EnableSignUpUser]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Department_Deleted')
      ALTER TABLE [dbo].[Mc_Department] DROP CONSTRAINT [DF_Mc_Department_Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE TABLE [dbo].[tmp_Mc_Department] (
	   [DepartmentId] [int] IDENTITY (1, 1) NOT NULL,
	   [DepartmentGuid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Mc_Department_DepartmentGuid] DEFAULT (newid()),
	   [OrganizationId] [int] NOT NULL,
	   [Name] [nvarchar] (255) NOT NULL,
	   [Description] [nvarchar] (1024) NOT NULL CONSTRAINT [DF_Mc_Department_Description] DEFAULT (N''),
	   [EnableSignUpUser] [bit] NOT NULL CONSTRAINT [DF_Mc_Department_EnableSignUpUser] DEFAULT ((0)),
	   [Deleted] [bit] NOT NULL CONSTRAINT [DF_Mc_Department_Deleted] DEFAULT ((0))
	)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   SET IDENTITY_INSERT [dbo].[tmp_Mc_Department] ON

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   INSERT INTO [dbo].[tmp_Mc_Department] ([DepartmentId], [OrganizationId], [Name], [Description], [EnableSignUpUser], [Deleted])
   SELECT [DepartmentId], [OrganizationId], [Name], [Description], [EnableSignUpUser], [Deleted]
   FROM [dbo].[Mc_Department]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   SET IDENTITY_INSERT [dbo].[tmp_Mc_Department] OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP TABLE [dbo].[Mc_Department]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC sp_rename N'[dbo].[tmp_Mc_Department]', N'Mc_Department'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Department] ADD CONSTRAINT [PK_Mc_Department] PRIMARY KEY CLUSTERED ([DepartmentId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertDepartment]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		EXEC('/****** Object:  StoredProcedure [dbo].[Mc_InsertDepartment] ******/
ALTER PROCEDURE [dbo].[Mc_InsertDepartment]
(
	@OrganizationId int,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@EnableSignUpUser bit,
	@Deleted bit
)
AS
	SET NOCOUNT OFF;
INSERT INTO [dbo].[Mc_Department] ([OrganizationId], [Name], [Description], [EnableSignUpUser], [Deleted]) VALUES (@OrganizationId, @Name, @Description, @EnableSignUpUser, @Deleted);
	
SELECT DepartmentId, [DepartmentGuid], OrganizationId, [Name], Description, EnableSignUpUser, Deleted FROM Mc_Department WHERE (DepartmentId = SCOPE_IDENTITY())')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllDepartment]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		EXEC('/****** Object:  StoredProcedure [dbo].[Mc_SelectAllDepartment] ******/
ALTER PROCEDURE [dbo].[Mc_SelectAllDepartment]
	@OrganizationId int
AS
	SET NOCOUNT ON;
SELECT DepartmentId, [DepartmentGuid], OrganizationId, [Name], Description, EnableSignUpUser, Deleted FROM dbo.Mc_Department WHERE (Deleted = 0) AND (OrganizationId = @OrganizationId)')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateDepartment]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		EXEC('/****** Object:  StoredProcedure [dbo].[Mc_UpdateDepartment] ******/
ALTER PROCEDURE [dbo].[Mc_UpdateDepartment]
(
	@DepartmentId int,
	@OrganizationId int,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@EnableSignUpUser bit,
	@Deleted bit
)
AS
	SET NOCOUNT OFF;
UPDATE [dbo].[Mc_Department] SET [OrganizationId] = @OrganizationId, [Name] = @Name, [Description] = @Description, [EnableSignUpUser] = @EnableSignUpUser, [Deleted] = @Deleted WHERE ([DepartmentId] = @DepartmentId);
	
SELECT DepartmentId, [DepartmentGuid], OrganizationId, [Name], Description, EnableSignUpUser, Deleted FROM Mc_Department WHERE (DepartmentId = @DepartmentId)')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsDepartmentsActions_Mc_Department')
      ALTER TABLE [dbo].[Mc_GroupsDepartmentsActions] ADD CONSTRAINT [FK_Mc_GroupsDepartmentsActions_Mc_Department] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[Mc_Department] ([DepartmentId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_GroupsDepartmentsRoles_Mc_Department')
      ALTER TABLE [dbo].[Mc_GroupsDepartmentsRoles] ADD CONSTRAINT [FK_Mc_GroupsDepartmentsRoles_Mc_Department] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[Mc_Department] ([DepartmentId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_SettingValue_Mc_Department')
      ALTER TABLE [dbo].[Mc_SettingValue] ADD CONSTRAINT [FK_Mc_SettingValue_Mc_Department] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[Mc_Department] ([DepartmentId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION