BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

DECLARE @ProductVersion nvarchar(128), @NVarCharMax nvarchar(128), @EmptyGuid varchar(36), @ObjectName sysname, @ObjectType char(2), @OwnerTableName sysname, @NewObjectName nvarchar(700)

SET @ProductVersion = CAST(SERVERPROPERTY('productversion') AS nvarchar(128))
SET @ProductVersion = LEFT(@ProductVersion, CHARINDEX('.', @ProductVersion) - 1)
SET @EmptyGuid = '00000000-0000-0000-0000-000000000000'

IF (CAST(@ProductVersion AS int) > 8)
	SET @NVarCharMax = 'max'
ELSE
	SET @NVarCharMax = '4000'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
BEGIN
	DECLARE Cursor1 CURSOR FOR
	SELECT so.name, so.type, so1.name
	FROM sysobjects AS so
	LEFT JOIN sysobjects AS so1
		ON	so.parent_obj = so1.id
	WHERE 
		(	so.type IN ('F', 'D', 'K') 
			AND (',Mc_Department,Mc_Group,Mc_GroupsDepartmentsActions,Mc_GroupsDepartmentsRoles,Mc_OrganizationsUsers,Mc_SettingValue,Mc_User,Mc_UsersGroups,' LIKE '%,' + UPPER(so1.name) + ',%')
		) 
		OR ((so.type = 'U') 
			AND (',Mc_Department,Mc_Group,Mc_GroupsDepartmentsActions,Mc_GroupsDepartmentsRoles,Mc_OrganizationsUsers,Mc_SettingValue,Mc_User,Mc_UsersGroups,' LIKE '%,' + UPPER(so.name) + ',%')
		)
	ORDER BY so.type

	OPEN Cursor1

	FETCH NEXT FROM Cursor1
	INTO @ObjectName, @ObjectType, @OwnerTableName

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @ObjectType = 'U'
		BEGIN
			SET @NewObjectName = @ObjectName + '_Old'

			EXEC sp_rename @ObjectName, @NewObjectName
		END
		ELSE IF @ObjectType IN ('F', 'D', 'K')
			EXEC('IF EXISTS (SELECT 0 FROM dbo.sysobjects WHERE id = OBJECT_ID(N''[' + @ObjectName + ']'') AND parent_obj = OBJECT_ID(N''[' + @OwnerTableName + ']'')) ALTER TABLE [' + @OwnerTableName + '] DROP CONSTRAINT [' + @ObjectName + ']')

		IF @@ERROR <> 0
		BEGIN
			CLOSE Cursor1
			DEALLOCATE Cursor1
			
			IF @@TRANCOUNT = 1 
				ROLLBACK TRANSACTION
			
			RETURN
		END

		FETCH NEXT FROM Cursor1
		INTO @ObjectName, @ObjectType, @OwnerTableName
	END

	CLOSE Cursor1
	DEALLOCATE Cursor1
END

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_DeleteGroupsDepartmentsActions]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_DeleteGroupsDepartmentsActions]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_DeleteGroupsDepartmentsRoles]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_DeleteGroupsDepartmentsRoles]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_DeleteOrganizationsUsers]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_DeleteOrganizationsUsers]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_DeleteSettingValue]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_DeleteSettingValue]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_DeleteUsersGroups]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_DeleteUsersGroups]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertDepartment]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertDepartment]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertGroup]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertGroup]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertGroupsDepartmentsActions]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertGroupsDepartmentsActions]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertGroupsDepartmentsRoles]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertGroupsDepartmentsRoles]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertOrganizationsUsers]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertOrganizationsUsers]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertSettingValue]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertSettingValue]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertUser]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertUser]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertUsersGroups]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertUsersGroups]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllDepartment]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllDepartment]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllGroup]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllGroup]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllGroupsDepartmentsActions]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllGroupsDepartmentsActions]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllGroupsDepartmentsRoles]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllGroupsDepartmentsRoles]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllSettingValue]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllSettingValue]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllUser]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllUser]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllUsersGroups]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllUsersGroups]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectVersion]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectVersion]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateDepartment]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateDepartment]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateGroup]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateGroup]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateGroupsDepartmentsActions]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateGroupsDepartmentsActions]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateGroupsDepartmentsRoles]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateGroupsDepartmentsRoles]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateOrganizationsUsers]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateOrganizationsUsers]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateSettingValue]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateSettingValue]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateUser]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateUser]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_Department](
		[DepartmentId] [uniqueidentifier] NOT NULL,
		[OrganizationId] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Description] [nvarchar](1024) NOT NULL,
		[EnableSignUpUser] [bit] NOT NULL,
		[Deleted] [bit] NOT NULL,
	 CONSTRAINT [PK_Mc_Department] PRIMARY KEY CLUSTERED 
	(
		[DepartmentId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_Group](
		[GroupId] [uniqueidentifier] NOT NULL,
		[OrganizationId] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Description] [nvarchar](1024) NOT NULL,
		[Deleted] [bit] NOT NULL,
	 CONSTRAINT [PK_Mc_Group] PRIMARY KEY CLUSTERED 
	(
		[GroupId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_GroupsDepartmentsActions](
		[GroupId] [uniqueidentifier] NOT NULL,
		[DepartmentId] [uniqueidentifier] NOT NULL,
		[ActionId] [uniqueidentifier] NOT NULL,
		[Enabled] [bit] NOT NULL,
	 CONSTRAINT [PK_Mc_GroupsDepartmentsActions] PRIMARY KEY CLUSTERED 
	(
		[GroupId] ASC,
		[DepartmentId] ASC,
		[ActionId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_GroupsDepartmentsRoles](
		[GroupId] [uniqueidentifier] NOT NULL,
		[DepartmentId] [uniqueidentifier] NOT NULL,
		[RoleId] [uniqueidentifier] NOT NULL,
	 CONSTRAINT [PK_Mc_GroupsDepartmentsRoles] PRIMARY KEY CLUSTERED 
	(
		[GroupId] ASC,
		[DepartmentId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_OrganizationsUsers](
		[OrganizationId] [uniqueidentifier] NOT NULL,
		[UserId] [uniqueidentifier] NOT NULL,
		[OrganizationAdministrator] [bit] NOT NULL,
	 CONSTRAINT [PK_Mc_OrganizationsUsers] PRIMARY KEY CLUSTERED 
	(
		[OrganizationId] ASC,
		[UserId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE TABLE [dbo].[Mc_SettingsValues](
	[SettingValueId] [uniqueidentifier] NOT NULL,
	[SettingId] [uniqueidentifier] NOT NULL,
	[Value] [nvarchar](' + @NVarCharMax + ') NOT NULL,
	[OrganizationId] [uniqueidentifier] NULL,
	[DepartmentId] [uniqueidentifier] NULL,
	[GroupId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Mc_SettingsValues] PRIMARY KEY CLUSTERED 
(
	[SettingValueId] ASC
) ON [PRIMARY]
) ON [PRIMARY]')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_User](
		[UserId] [uniqueidentifier] NOT NULL,
		[Email] [nvarchar](255) NOT NULL,
		[FirstName] [nvarchar](255) NOT NULL,
		[LastName] [nvarchar](255) NOT NULL,
		[MiddleName] [nvarchar](255) NOT NULL,
		[LastLoginDate] [datetime] NULL,
		[Deleted] [bit] NOT NULL,
	 CONSTRAINT [PK_Mc_User] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_UsersGroups](
		[UserId] [uniqueidentifier] NOT NULL,
		[GroupId] [uniqueidentifier] NOT NULL,
	 CONSTRAINT [PK_Mc_UsersGroups] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[GroupId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('$Input$')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_Department (DepartmentId, OrganizationId, [Name], [Description], EnableSignUpUser, Deleted)
	SELECT d.DepartmentGuid, o.OrganizationGuid, d.[Name], d.[Description], d.EnableSignUpUser, d.Deleted
	FROM dbo.Mc_Department_Old AS d
	INNER JOIN dbo.Mc_Organization_Old AS o
		ON	d.OrganizationId = o.OrganizationId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_Department_Temp](
		[DepartmentId] [int] NOT NULL,
		[DepartmentGuid] [uniqueidentifier] NOT NULL
	)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_Department_Temp (DepartmentId, DepartmentGuid)
	SELECT DepartmentId, DepartmentGuid
	FROM dbo.Mc_Department_Old

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_Group (GroupId, OrganizationId, [Name], [Description], Deleted)
	SELECT 
		LEFT(@EmptyGuid, (36 - LEN(CAST(g.GroupId AS varchar(10))))) + CAST(g.GroupId AS varchar(10))
		, o.OrganizationGuid, g.[Name], g.[Description], g.Deleted
	FROM dbo.Mc_Group_Old AS g
	INNER JOIN dbo.Mc_Organization_Old AS o
		ON	g.OrganizationId = o.OrganizationId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_GroupsDepartmentsActions (GroupId, DepartmentId, ActionId, [Enabled])
	SELECT 
		LEFT(@EmptyGuid, (36 - LEN(CAST(a.GroupId AS varchar(10))))) + CAST(a.GroupId AS varchar(10))
		, d.DepartmentGuid
		, LEFT(@EmptyGuid, (36 - LEN(CAST(a.ActionId AS varchar(10))))) + CAST(a.ActionId AS varchar(10))
		, a.[Enabled]
	FROM dbo.Mc_GroupsDepartmentsActions_Old AS a
	INNER JOIN dbo.Mc_Department_Old AS d
		ON a.DepartmentId = d.DepartmentId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_GroupsDepartmentsRoles (GroupId, DepartmentId, RoleId)
	SELECT 
		LEFT(@EmptyGuid, (36 - LEN(CAST(r.GroupId AS varchar(10))))) + CAST(r.GroupId AS varchar(10))
		, d.DepartmentGuid
		, LEFT(@EmptyGuid, (36 - LEN(CAST(r.RoleId AS varchar(10))))) + CAST(r.RoleId AS varchar(10))
	FROM dbo.Mc_GroupsDepartmentsRoles_Old AS r
	INNER JOIN dbo.Mc_Department_Old AS d
		ON r.DepartmentId = d.DepartmentId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_OrganizationsUsers (OrganizationId, UserId, OrganizationAdministrator)
	SELECT 
		o.OrganizationGuid
		, LEFT(@EmptyGuid, (36 - LEN(CAST(u.UserId AS varchar(10))))) + CAST(u.UserId AS varchar(10))
		, IsOrganizationAdministrator
	FROM dbo.Mc_OrganizationsUsers_Old AS u
	INNER JOIN dbo.Mc_Organization_Old AS o
		ON	u.OrganizationId = o.OrganizationId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_SettingsValues (SettingValueId, SettingId, Value, OrganizationId, DepartmentId, GroupId)
	SELECT 
		LEFT(@EmptyGuid, (36 - LEN(CAST(v.SettingValueId AS varchar(10))))) + CAST(v.SettingValueId AS varchar(10))
		, LEFT(@EmptyGuid, (36 - LEN(CAST(v.SettingId AS varchar(10))))) + CAST(v.SettingId AS varchar(10))
		, Value
		, o.OrganizationGuid
		, d.DepartmentGuid
		, LEFT(@EmptyGuid, (36 - LEN(CAST(v.GroupId AS varchar(10))))) + CAST(v.GroupId AS varchar(10))
	FROM dbo.Mc_SettingValue_Old AS v
	INNER JOIN dbo.Mc_Organization_Old AS o
		ON	v.OrganizationId = o.OrganizationId
	INNER JOIN dbo.Mc_Department_Old AS d
		ON v.DepartmentId = d.DepartmentId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_User (UserId, Email, FirstName, LastName, MiddleName, LastLoginDate, Deleted)
	SELECT 
		LEFT(@EmptyGuid, (36 - LEN(CAST(UserId AS varchar(10))))) + CAST(UserId AS varchar(10))
		, Email, FirstName, LastName, MiddleName, LastLoginDate, Deleted
	FROM dbo.Mc_User_Old

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE dbo.Mc_User_Temp(
		UserId int NOT NULL,
		UserGuid uniqueidentifier NOT NULL
	)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_User_Temp (UserId, UserGuid)
	SELECT UserId, LEFT(@EmptyGuid, (36 - LEN(CAST(UserId AS varchar(10))))) + CAST(UserId AS varchar(10))
	FROM dbo.Mc_User_Old

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_UsersGroups (UserId, GroupId)
	SELECT 
		LEFT(@EmptyGuid, (36 - LEN(CAST(UserId AS varchar(10))))) + CAST(UserId AS varchar(10))
		, LEFT(@EmptyGuid, (36 - LEN(CAST(GroupId AS varchar(10))))) + CAST(GroupId AS varchar(10))
	FROM dbo.Mc_UsersGroups_Old

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Department] ADD  CONSTRAINT [DF_Mc_Department_DepartmentGuid]  DEFAULT (newid()) FOR [DepartmentId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Department] ADD  CONSTRAINT [DF_Mc_Department_Description]  DEFAULT (N'') FOR [Description]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Department] ADD  CONSTRAINT [DF_Mc_Department_EnableSignUpUser]  DEFAULT ((0)) FOR [EnableSignUpUser]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Department] ADD  CONSTRAINT [DF_Mc_Department_Deleted]  DEFAULT ((0)) FOR [Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Group] ADD  CONSTRAINT [DF_Mc_Group_Description]  DEFAULT (N'') FOR [Description]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Group] ADD  CONSTRAINT [DF_Mc_Group_Deleted]  DEFAULT ((0)) FOR [Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_GroupsDepartmentsActions]  WITH CHECK ADD  CONSTRAINT [FK_Mc_GroupsDepartmentsActions_Mc_Department] FOREIGN KEY([DepartmentId])
	REFERENCES [dbo].[Mc_Department] ([DepartmentId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_GroupsDepartmentsActions] CHECK CONSTRAINT [FK_Mc_GroupsDepartmentsActions_Mc_Department]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_GroupsDepartmentsActions]  WITH CHECK ADD  CONSTRAINT [FK_Mc_GroupsDepartmentsActions_Mc_Group] FOREIGN KEY([GroupId])
	REFERENCES [dbo].[Mc_Group] ([GroupId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_GroupsDepartmentsActions] CHECK CONSTRAINT [FK_Mc_GroupsDepartmentsActions_Mc_Group]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_GroupsDepartmentsActions] ADD  CONSTRAINT [DF_Mc_GroupsDepartmentsActions_Enabled]  DEFAULT ((1)) FOR [Enabled]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_GroupsDepartmentsRoles]  WITH CHECK ADD  CONSTRAINT [FK_Mc_GroupsDepartmentsRoles_Mc_Department] FOREIGN KEY([DepartmentId])
	REFERENCES [dbo].[Mc_Department] ([DepartmentId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_GroupsDepartmentsRoles] CHECK CONSTRAINT [FK_Mc_GroupsDepartmentsRoles_Mc_Department]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_GroupsDepartmentsRoles]  WITH CHECK ADD  CONSTRAINT [FK_Mc_GroupsDepartmentsRoles_Mc_Group] FOREIGN KEY([GroupId])
	REFERENCES [dbo].[Mc_Group] ([GroupId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_GroupsDepartmentsRoles] CHECK CONSTRAINT [FK_Mc_GroupsDepartmentsRoles_Mc_Group]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_OrganizationsUsers]  WITH CHECK ADD  CONSTRAINT [FK_Mc_OrganizationsUsers_Mc_User] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Mc_User] ([UserId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_OrganizationsUsers] CHECK CONSTRAINT [FK_Mc_OrganizationsUsers_Mc_User]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_OrganizationsUsers] ADD  CONSTRAINT [DF_Mc_OrganizationsUsers_OrganizationAdministrator]  DEFAULT ((0)) FOR [OrganizationAdministrator]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_SettingsValues]  WITH CHECK ADD  CONSTRAINT [FK_Mc_SettingsValues_Mc_Department] FOREIGN KEY([DepartmentId])
	REFERENCES [dbo].[Mc_Department] ([DepartmentId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_SettingsValues] CHECK CONSTRAINT [FK_Mc_SettingsValues_Mc_Department]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_SettingsValues]  WITH CHECK ADD  CONSTRAINT [FK_Mc_SettingsValues_Mc_Group] FOREIGN KEY([GroupId])
	REFERENCES [dbo].[Mc_Group] ([GroupId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_SettingsValues] CHECK CONSTRAINT [FK_Mc_SettingsValues_Mc_Group]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_User] ADD  CONSTRAINT [DF_Mc_User_FirstName]  DEFAULT (N'') FOR [FirstName]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_User] ADD  CONSTRAINT [DF_Mc_User_LastName]  DEFAULT (N'') FOR [LastName]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_User] ADD  CONSTRAINT [DF_Mc_User_MiddleName]  DEFAULT (N'') FOR [MiddleName]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_User] ADD  CONSTRAINT [DF_Mc_User_Deleted]  DEFAULT ((0)) FOR [Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_UsersGroups]  WITH CHECK ADD  CONSTRAINT [FK_Mc_UsersGroups_Mc_Group] FOREIGN KEY([GroupId])
	REFERENCES [dbo].[Mc_Group] ([GroupId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_UsersGroups] CHECK CONSTRAINT [FK_Mc_UsersGroups_Mc_Group]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_UsersGroups]  WITH CHECK ADD  CONSTRAINT [FK_Mc_UsersGroups_Mc_User] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Mc_User] ([UserId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_UsersGroups] CHECK CONSTRAINT [FK_Mc_UsersGroups_Mc_User]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
BEGIN
	DECLARE Cursor1 CURSOR FOR
	SELECT name
	FROM sysobjects
	WHERE type = 'U' AND UPPER(name) LIKE 'MC_%_Old'

	OPEN Cursor1

	FETCH NEXT FROM Cursor1
	INTO @ObjectName

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC('DROP TABLE [' + @ObjectName + ']')

		IF @@ERROR <> 0
		BEGIN
			CLOSE Cursor1
			DEALLOCATE Cursor1
			
			IF @@TRANCOUNT = 1 
				ROLLBACK TRANSACTION
			
			RETURN
		END

		FETCH NEXT FROM Cursor1
		INTO @ObjectName
	END

	CLOSE Cursor1
	DEALLOCATE Cursor1
END

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteGroupDepartmentAction]
(
	@GroupId uniqueidentifier,
	@DepartmentId uniqueidentifier,
	@ActionId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_GroupsDepartmentsActions 
	WHERE (GroupId = @GroupId) AND (DepartmentId = @DepartmentId) AND (ActionId = @ActionId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteGroupDepartmentRoles]
(
	@GroupId uniqueidentifier,
	@DepartmentId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_GroupsDepartmentsRoles 
	WHERE (GroupId = @GroupId) AND (DepartmentId = @DepartmentId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteOrganizationUser]
(
	@OrganizationId uniqueidentifier,
	@UserId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_OrganizationsUsers 
	WHERE (OrganizationId = @OrganizationId) AND (UserId = @UserId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteSettingValue]
(
	@SettingValueId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_SettingsValues 
	WHERE (SettingValueId = @SettingValueId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteUserGroup]
(
	@UserId uniqueidentifier,
	@GroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_UsersGroups 
	WHERE (UserId = @UserId) AND (GroupId = @GroupId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetDepartments]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DepartmentId, OrganizationId, [Name], [Description], EnableSignUpUser, Deleted 
	FROM dbo.Mc_Department 
	WHERE (Deleted = 0) AND (OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetGroups]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT GroupId, OrganizationId, [Name], [Description], Deleted 
	FROM dbo.Mc_Group 
	WHERE (Deleted = 0) AND (OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetGroupsDepartmentsActions]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT gda.GroupId, gda.DepartmentId, gda.ActionId, gda.[Enabled]
	FROM dbo.Mc_GroupsDepartmentsActions AS gda
	INNER JOIN dbo.Mc_Group AS g
		ON	(gda.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId)
	INNER JOIN dbo.Mc_Department AS d
		ON	(gda.DepartmentId = d.DepartmentId) AND (d.Deleted = 0) AND (d.OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetGroupsDepartmentsRoles]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT gdr.GroupId, gdr.DepartmentId, gdr.RoleId
	FROM dbo.Mc_GroupsDepartmentsRoles AS gdr
	INNER JOIN dbo.Mc_Group AS g
		ON	(gdr.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId)
	INNER JOIN dbo.Mc_Department AS d
		ON	(gdr.DepartmentId = d.DepartmentId) AND (d.Deleted = 0) AND (d.OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetSettingsValues]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT SettingValueId, SettingId, [Value], OrganizationId, DepartmentId, GroupId
	FROM dbo.Mc_SettingsValues 
	WHERE (OrganizationId IS NULL) OR (OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetUsers]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName, u.LastLoginDate, u.Deleted, uo.OrganizationAdministrator
	FROM dbo.Mc_User AS u
	INNER JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId = uo.UserId) AND (u.Deleted = 0) AND (uo.OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetUsersGroups]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ug.UserId, ug.GroupId 
	FROM dbo.Mc_UsersGroups AS ug
	INNER JOIN dbo.Mc_Group AS g
		ON	(ug.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId)
	INNER JOIN dbo.Mc_User AS u
		ON	(ug.UserId = u.UserId) AND (u.Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertDepartment]
(
	@DepartmentId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@EnableSignUpUser bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Department (DepartmentId, OrganizationId, [Name], [Description], EnableSignUpUser, Deleted) 
	VALUES (@DepartmentId, @OrganizationId, @Name, @Description, @EnableSignUpUser, @Deleted);
	
	SELECT DepartmentId, OrganizationId, [Name], [Description], EnableSignUpUser, Deleted 
	FROM dbo.Mc_Department 
	WHERE (DepartmentId = @DepartmentId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertGroup]
(
	@GroupId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Group (GroupId, OrganizationId, [Name], [Description], Deleted) 
	VALUES (@GroupId, @OrganizationId, @Name, @Description, @Deleted);
	
	SELECT GroupId, OrganizationId, [Name], [Description], Deleted 
	FROM dbo.Mc_Group 
	WHERE (GroupId = @GroupId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertGroupDepartmentAction]
(
	@GroupId uniqueidentifier,
	@DepartmentId uniqueidentifier,
	@ActionId uniqueidentifier,
	@Enabled bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_GroupsDepartmentsActions (GroupId, DepartmentId, ActionId, [Enabled]) 
	VALUES (@GroupId, @DepartmentId, @ActionId, @Enabled);
	
	SELECT GroupId, DepartmentId, ActionId, [Enabled] 
	FROM dbo.Mc_GroupsDepartmentsActions 
	WHERE (ActionId = @ActionId) AND (DepartmentId = @DepartmentId) AND (GroupId = @GroupId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertGroupDepartmentRole]
(
	@GroupId uniqueidentifier,
	@DepartmentId uniqueidentifier,
	@RoleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_GroupsDepartmentsRoles (GroupId, DepartmentId, RoleId) 
	VALUES (@GroupId, @DepartmentId, @RoleId);
	
	SELECT GroupId, DepartmentId, RoleId 
	FROM dbo.Mc_GroupsDepartmentsRoles 
	WHERE (DepartmentId = @DepartmentId) AND (GroupId = @GroupId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertOrganizationUser]
(
	@OrganizationId uniqueidentifier,
	@UserId uniqueidentifier,
	@OrganizationAdministrator bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_OrganizationsUsers (OrganizationId, UserId, OrganizationAdministrator) 
	VALUES (@OrganizationId, @UserId, @OrganizationAdministrator);
	
	SELECT OrganizationId, UserId, OrganizationAdministrator
	FROM dbo.Mc_OrganizationsUsers
	WHERE (OrganizationId = @OrganizationId) AND (UserId = @UserId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertSettingValue]
(
	@SettingValueId uniqueidentifier,
	@SettingId uniqueidentifier,
	@Value nvarchar(' + @NVarCharMax + '),
	@OrganizationId uniqueidentifier,
	@DepartmentId uniqueidentifier,
	@GroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_SettingsValues (SettingValueId, SettingId, Value, OrganizationId, DepartmentId, GroupId) 
	VALUES (@SettingValueId, @SettingId, @Value, @OrganizationId, @DepartmentId, @GroupId);
	
	SELECT SettingValueId, SettingId, Value, OrganizationId, DepartmentId, GroupId 
	FROM dbo.Mc_SettingsValues
	WHERE (SettingValueId = @SettingValueId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertUser]
(
	@UserId uniqueidentifier,
	@Email nvarchar(255),
	@FirstName nvarchar(255),
	@LastName nvarchar(255),
	@MiddleName nvarchar(255),
	@LastLoginDate datetime,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	IF NOT EXISTS(
		SELECT 0 
		FROM dbo.Mc_User 
		WHERE (UserId = @UserId)
	)
		INSERT INTO dbo.Mc_User (UserId, Email, FirstName, LastName, MiddleName, LastLoginDate, Deleted) 
		VALUES (@UserId, @Email, @FirstName, @LastName, @MiddleName, @LastLoginDate, @Deleted);
	
	SELECT UserId, Email, FirstName, LastName, MiddleName, LastLoginDate, Deleted 
	FROM dbo.Mc_User 
	WHERE (UserId = @UserId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertUserGroup]
(
	@UserId uniqueidentifier,
	@GroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_UsersGroups (UserId, GroupId) 
	VALUES (@UserId, @GroupId);
	
	SELECT UserId, GroupId 
	FROM dbo.Mc_UsersGroups 
	WHERE (GroupId = @GroupId) AND (UserId = @UserId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateDepartment]
(
	@DepartmentId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@EnableSignUpUser bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Department 
	SET OrganizationId = @OrganizationId, [Name] = @Name, [Description] = @Description, EnableSignUpUser = @EnableSignUpUser, Deleted = @Deleted 
	WHERE (DepartmentId = @DepartmentId);
	
	SELECT DepartmentId, OrganizationId, [Name], [Description], EnableSignUpUser, Deleted 
	FROM dbo.Mc_Department 
	WHERE (DepartmentId = @DepartmentId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateGroup]
(
	@GroupId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Group 
	SET OrganizationId = @OrganizationId, [Name] = @Name, [Description] = @Description, Deleted = @Deleted 
	WHERE (GroupId = @GroupId);
	
	SELECT GroupId, OrganizationId, [Name], [Description], Deleted 
	FROM dbo.Mc_Group 
	WHERE (GroupId = @GroupId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateGroupDepartmentAction]
(
	@GroupId uniqueidentifier,
	@DepartmentId uniqueidentifier,
	@ActionId uniqueidentifier,
	@Enabled bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_GroupsDepartmentsActions 
	SET [Enabled] = @Enabled 
	WHERE (ActionId = @ActionId) AND (DepartmentId = @DepartmentId) AND (GroupId = @GroupId);
	
	SELECT GroupId, DepartmentId, ActionId, [Enabled] 
	FROM dbo.Mc_GroupsDepartmentsActions 
	WHERE (ActionId = @ActionId) AND (DepartmentId = @DepartmentId) AND (GroupId = @GroupId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateGroupDepartmentRole]
(
	@GroupId uniqueidentifier,
	@DepartmentId uniqueidentifier,
	@RoleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_GroupsDepartmentsRoles 
	SET GroupId = @GroupId, DepartmentId = @DepartmentId, RoleId = @RoleId 
	WHERE (GroupId = @GroupId) AND (DepartmentId = @DepartmentId);
	
	SELECT GroupId, DepartmentId, RoleId 
	FROM dbo.Mc_GroupsDepartmentsRoles 
	WHERE (DepartmentId = @DepartmentId) AND (GroupId = @GroupId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateOrganizationUser]
(
	@OrganizationId uniqueidentifier,
	@UserId uniqueidentifier,
	@OrganizationAdministrator bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_OrganizationsUsers 
	SET OrganizationAdministrator = @OrganizationAdministrator 
	WHERE (OrganizationId = @OrganizationId) AND (UserId = @UserId);

	SELECT OrganizationId, UserId, OrganizationAdministrator
	FROM dbo.Mc_OrganizationsUsers
	WHERE (OrganizationId = @OrganizationId) AND (UserId = @UserId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateSettingValue]
(
	@SettingValueId uniqueidentifier,
	@SettingId uniqueidentifier,
	@Value nvarchar(' + @NVarCharMax + '),
	@OrganizationId uniqueidentifier,
	@DepartmentId uniqueidentifier,
	@GroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_SettingsValues 
	SET SettingId = @SettingId, Value = @Value, OrganizationId = @OrganizationId, DepartmentId = @DepartmentId, GroupId = @GroupId 
	WHERE (SettingValueId = @SettingValueId);
	
	SELECT SettingValueId, SettingId, Value, OrganizationId, DepartmentId, GroupId 
	FROM dbo.Mc_SettingsValues 
	WHERE (SettingValueId = @SettingValueId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateUser]
(
	@UserId uniqueidentifier,
	@Email nvarchar(255),
	@FirstName nvarchar(255),
	@LastName nvarchar(255),
	@MiddleName nvarchar(255),
	@LastLoginDate datetime,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_User 
	SET UserId = @UserId, Email = @Email, FirstName = @FirstName, LastName = @LastName, MiddleName = @MiddleName, LastLoginDate = @LastLoginDate, Deleted = @Deleted 
	WHERE (UserId = @UserId);
	
	SELECT UserId, Email, FirstName, LastName, MiddleName, LastLoginDate, Deleted 
	FROM dbo.Mc_User 
	WHERE (UserId = @UserId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
