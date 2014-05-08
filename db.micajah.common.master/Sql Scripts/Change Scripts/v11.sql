BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

DECLARE @ProductVersion nvarchar(128), @NVarCharMax nvarchar(128), @EmptyGuid varchar(36), @ObjectName sysname, @ObjectType char(2), @OwnerTableName sysname, @NewObjectName nvarchar(700), @Output nvarchar(4000)

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
			AND (',Mc_Action,Mc_ActionsParentActions,Mc_Database,Mc_Login,Mc_Organization,Mc_OrganizationsLogins,Mc_Role,Mc_RolesActions,Mc_RolesSettings,Mc_Session,Mc_Setting,Mc_SettingListValues,Mc_SettingType,Mc_SqlServer,Mc_ViewState,Mc_WebSite,' LIKE '%,' + UPPER(so1.name) + ',%')
		) 
		OR ((so.type = 'U') 
			AND (',Mc_Action,Mc_ActionsParentActions,Mc_Database,Mc_Login,Mc_Organization,Mc_OrganizationsLogins,Mc_Role,Mc_RolesActions,Mc_RolesSettings,Mc_Session,Mc_Setting,Mc_SettingListValues,Mc_SettingType,Mc_SqlServer,Mc_ViewState,Mc_WebSite,' LIKE '%,' + UPPER(so.name) + ',%')
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
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_DeleteActionsParentActions]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_DeleteActionsParentActions]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_DeleteOrganizationsLogins]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_DeleteOrganizationsLogins]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_DeleteRolesActions]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_DeleteRolesActions]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_DeleteRolesSettings]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_DeleteRolesSettings]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_DeleteSession]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_DeleteSession]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertAction]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertAction]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertActionsParentActions]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertActionsParentActions]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertDatabase]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertDatabase]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertLogin]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertLogin]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertOrganization]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertOrganization]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertOrganizationsLogins]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertOrganizationsLogins]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertRole]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertRole]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertRolesActions]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertRolesActions]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertRolesSettings]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertRolesSettings]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertSetting]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertSetting]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertSettingListValues]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertSettingListValues]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertSqlServer]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertSqlServer]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertWebSite]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_InsertWebSite]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllAction]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllAction]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllActionsParentActions]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllActionsParentActions]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllDatabase]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllDatabase]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllLoginByOrganizationId]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllLoginByOrganizationId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllOrganization]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllOrganization]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllOrganizationByLoginId]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllOrganizationByLoginId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllRole]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllRole]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllRolesActions]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllRolesActions]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllRolesSettings]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllRolesSettings]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllSetting]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllSetting]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllSettingListValues]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllSettingListValues]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllSettingType]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllSettingType]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllSqlServer]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllSqlServer]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectAllWebSite]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectAllWebSite]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectLogin]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Mc_SelectLogin]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectLoginByDetails]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectLoginByDetails]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectOrganizationsLogins]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectOrganizationsLogins]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectVersion]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectVersion]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_SelectViewState]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_SelectViewState]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateAction]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateAction]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateDatabase]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateDatabase]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateLogin]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateLogin]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateOrganization]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateOrganization]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateOrganizationsLogins]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateOrganizationsLogins]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateRole]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateRole]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateRolesSettings]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Mc_UpdateRolesSettings]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateSession]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateSession]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateSessionLogin]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateSessionLogin]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateSetting]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateSetting]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateSettingListValues]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateSettingListValues]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateSqlServer]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateSqlServer]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateViewState]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateViewState]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateWebSite]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_UpdateWebSite]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_ValidateSession]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
		DROP PROCEDURE [dbo].[Mc_ValidateSession]

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_Action](
		[ActionId] [uniqueidentifier] NOT NULL,
		[ParentActionId] [uniqueidentifier] NULL,
		[ActionTypeId] [int] NOT NULL,
		[Name] [nvarchar](1024) NOT NULL,
		[Description] [nvarchar](1024) NOT NULL,
		[IconUrl] [nvarchar](2048) NOT NULL,
		[ButtonIconUrl] [nvarchar](2048) NOT NULL,
		[NavigateUrl] [nvarchar](2048) NULL,
		[OrderNumber] [int] NOT NULL,
		[ClassFullName] [nvarchar](1024) NOT NULL,
		[AuthenticationRequired] [bit] NOT NULL,
		[DepartmentRequired] [bit] NOT NULL,
		[Visible] [bit] NOT NULL,
		[ShowInDetailMenu] [bit] NOT NULL,
		[ShowChildrenInDetailMenu] [bit] NOT NULL,
		[GroupInDetailMenu] [bit] NOT NULL,
		[HighlightInDetailMenu] [bit] NOT NULL,
		[BuiltIn] [bit] NOT NULL,
		[Deleted] [bit] NOT NULL,
	 CONSTRAINT [PK_Mc_Action] PRIMARY KEY CLUSTERED 
	(
		[ActionId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_ActionsParentActions](
		[ActionId] [uniqueidentifier] NOT NULL,
		[ParentActionId] [uniqueidentifier] NOT NULL,
	 CONSTRAINT [PK_Mc_ActionsParentActions] PRIMARY KEY CLUSTERED 
	(
		[ActionId] ASC,
		[ParentActionId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_Database](
		[DatabaseId] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Description] [nvarchar](1024) NOT NULL,
		[UserName] [nvarchar](255) NOT NULL,
		[Password] [nvarchar](255) NOT NULL,
		[DatabaseServerId] [uniqueidentifier] NOT NULL,
		[Deleted] [bit] NOT NULL,
	 CONSTRAINT [PK_Mc_Database] PRIMARY KEY CLUSTERED 
	(
		[DatabaseId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_DatabaseServer](
		[DatabaseServerId] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[InstanceName] [nvarchar](255) NOT NULL,
		[Port] [int] NOT NULL,
		[Description] [nvarchar](1024) NOT NULL,
		[WebsiteId] [uniqueidentifier] NOT NULL,
		[Deleted] [bit] NOT NULL,
	 CONSTRAINT [PK_Mc_DatabaseServer] PRIMARY KEY CLUSTERED 
	(
		[DatabaseServerId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_Login](
		[LoginId] [uniqueidentifier] NOT NULL,
		[LoginName] [nvarchar](255) NOT NULL,
		[Password] [nvarchar](50) NOT NULL,
		[Deleted] [bit] NOT NULL,
	 CONSTRAINT [PK_Mc_Login] PRIMARY KEY CLUSTERED 
	(
		[LoginId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_Organization](
		[OrganizationId] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Description] [nvarchar](255) NOT NULL,
		[WebsiteUrl] [nvarchar](2048) NOT NULL,
		[IconUrl] [nvarchar](2048) NOT NULL,
		[DatabaseId] [uniqueidentifier] NULL,
		[Deleted] [bit] NOT NULL,
	 CONSTRAINT [PK_Mc_Organization] PRIMARY KEY CLUSTERED 
	(
		[OrganizationId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_OrganizationsLogins](
		[OrganizationId] [uniqueidentifier] NOT NULL,
		[LoginId] [uniqueidentifier] NOT NULL,
		[OrganizationAdministrator] [bit] NOT NULL,
	 CONSTRAINT [PK_Mc_OrganizationsLogins] PRIMARY KEY CLUSTERED 
	(
		[OrganizationId] ASC,
		[LoginId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_Role](
		[RoleId] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Description] [nvarchar](1024) NOT NULL,
		[ShortName] [nvarchar](50) NOT NULL,
		[Rank] [int] NOT NULL,
		[StartActionId] [uniqueidentifier] NOT NULL,
		[BuiltIn] [bit] NOT NULL,
		[Deleted] [bit] NOT NULL,
	 CONSTRAINT [PK_Mc_Role] PRIMARY KEY CLUSTERED 
	(
		[RoleId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_RolesActions](
		[RoleId] [uniqueidentifier] NOT NULL,
		[ActionId] [uniqueidentifier] NOT NULL,
	 CONSTRAINT [PK_Mc_RolesActions] PRIMARY KEY CLUSTERED 
	(
		[RoleId] ASC,
		[ActionId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_RolesSettings](
		[RoleId] [uniqueidentifier] NOT NULL,
		[SettingId] [uniqueidentifier] NOT NULL,
		[Value] [nvarchar](512) NOT NULL,
	 CONSTRAINT [PK_Mc_RolesSettings] PRIMARY KEY CLUSTERED 
	(
		[RoleId] ASC,
		[SettingId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_Session](
		[SessionId] [int] IDENTITY(1,1) NOT NULL,
		[SessionCookie] [varchar](50) NOT NULL,
		[LoginId] [uniqueidentifier] NULL,
		[CreatedTime] [datetime] NOT NULL,
		[LastAccessTime] [datetime] NOT NULL,
	 CONSTRAINT [PK_Mc_Session] PRIMARY KEY CLUSTERED 
	(
		[SessionId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE TABLE [dbo].[Mc_Setting](
	[SettingId] [uniqueidentifier] NOT NULL,
	[ParentSettingId] [uniqueidentifier] NULL,
	[SettingTypeId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
	[ShortName] [nvarchar](50) NOT NULL,
	[DefaultValue] [nvarchar](512) NOT NULL,
	[Value] [nvarchar](' + @NVarCharMax + ') NULL,
	[OrderNumber] [int] NOT NULL,
	[EnableOrganization] [bit] NOT NULL,
	[EnableDepartment] [bit] NOT NULL,
	[EnableRole] [bit] NOT NULL,
	[EnableGroup] [bit] NOT NULL,
	[BuiltIn] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Mc_Setting] PRIMARY KEY CLUSTERED 
(
	[SettingId] ASC
) ON [PRIMARY]
) ON [PRIMARY]')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_SettingListsValues](
		[SettingListValueId] [uniqueidentifier] NOT NULL,
		[SettingId] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Value] [nvarchar](512) NOT NULL,
		[Deleted] [bit] NOT NULL,
	 CONSTRAINT [PK_Mc_SettingListValues] PRIMARY KEY CLUSTERED 
	(
		[SettingListValueId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_ViewState](
		[ViewStateId] [int] IDENTITY(1,1) NOT NULL,
		[SessionId] [int] NOT NULL,
		[ViewState] [image] NULL,
	 CONSTRAINT [PK_Mc_ViewState] PRIMARY KEY CLUSTERED 
	(
		[ViewStateId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_Website](
		[WebsiteId] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Url] [nvarchar](2048) NOT NULL,
		[Description] [nvarchar](1024) NOT NULL,
		[AdminContactInfo] [nvarchar](2048) NOT NULL,
		[Deleted] [bit] NOT NULL,
	 CONSTRAINT [PK_Mc_Website] PRIMARY KEY CLUSTERED 
	(
		[WebsiteId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_Action (ActionId, ParentActionId, ActionTypeId, [Name], [Description], IconUrl, ButtonIconUrl, NavigateUrl, OrderNumber, ClassFullName, AuthenticationRequired, DepartmentRequired, Visible, ShowInDetailMenu, ShowChildrenInDetailMenu, GroupInDetailMenu, HighlightInDetailMenu, BuiltIn, Deleted)
	SELECT 
		LEFT(@EmptyGuid, (36 - LEN(CAST(ActionId AS varchar(10))))) + CAST(ActionId AS varchar(10))
		, CASE WHEN ParentActionId IS NULL THEN NULL ELSE LEFT(@EmptyGuid, (36 - LEN(CAST(ParentActionId AS varchar(10))))) + CAST(ParentActionId AS varchar(10)) END
		, ActionTypeId, [Name], [Description], IconUrl, ButtonIconUrl
		, CASE 
			WHEN ActionId = 3 THEN N'/Resources.Micajah.Common/Pages/DetailMenu.aspx?pageid=00000000000000000000000000000015'
			WHEN ActionId = 5 THEN N'/Resources.Micajah.Common/Pages/DetailMenu.aspx?pageid=00000000000000000000000000000021'
			WHEN ActionId = 38 THEN  N'/Resources.Micajah.Common/Pages/Setup/DatabaseServers.aspx'
			ELSE NavigateUrl END
		, OrderNumber, ClassFullName, AuthenticationRequired, DepartmentRequired, Visible, ShowInDetailMenu, ShowChildrenInDetailMenu, GroupInDetailMenu, HighlightInDetailMenu
		, CASE WHEN ActionId < 100 THEN 1 ELSE 0 END
		, Deleted
	FROM dbo.Mc_Action_Old

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_ActionsParentActions (ActionId, ParentActionId)
	SELECT 
		LEFT(@EmptyGuid, (36 - LEN(CAST(ActionId AS varchar(10))))) + CAST(ActionId AS varchar(10))
		, LEFT(@EmptyGuid, (36 - LEN(CAST(ParentActionId AS varchar(10))))) + CAST(ParentActionId AS varchar(10))
	FROM dbo.Mc_ActionsParentActions_Old

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_Database (DatabaseId, [Name], [Description], UserName, [Password], DatabaseServerId, Deleted)
	SELECT 
		LEFT(@EmptyGuid, (36 - LEN(CAST(DatabaseId AS varchar(10))))) + CAST(DatabaseId AS varchar(10))
		, [Name], [Description], UserName, [Password]
		, LEFT(@EmptyGuid, (36 - LEN(CAST(SqlServerId AS varchar(10))))) + CAST(SqlServerId AS varchar(10))
		, Deleted
	FROM dbo.Mc_Database_Old

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_DatabaseServer (DatabaseServerId, [Name], InstanceName, Port, [Description], [WebsiteId], Deleted)
	SELECT
		LEFT(@EmptyGuid, (36 - LEN(CAST(SqlServerId AS varchar(10))))) + CAST(SqlServerId AS varchar(10))
		, [Name], InstanceName, Port, [Description]
		, LEFT(@EmptyGuid, (36 - LEN(CAST(WebSiteId AS varchar(10))))) + CAST(WebSiteId AS varchar(10))
		, Deleted
	FROM dbo.Mc_SqlServer_Old

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_Login (LoginId, LoginName, [Password], Deleted)
	SELECT 
		LEFT(@EmptyGuid, (36 - LEN(CAST(LoginId AS varchar(10))))) + CAST(LoginId AS varchar(10))
		, LoginName, [Password], Deleted
	FROM dbo.Mc_Login_Old

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE dbo.Mc_Login_Temp(
		LoginId int NOT NULL,
		LoginGuid uniqueidentifier NOT NULL
	)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_Login_Temp (LoginId, LoginGuid)
	SELECT LoginId, LEFT(@EmptyGuid, (36 - LEN(CAST(LoginId AS varchar(10))))) + CAST(LoginId AS varchar(10))
	FROM dbo.Mc_Login_Old

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_Organization (OrganizationId, [Name], [Description], WebsiteUrl, IconUrl, DatabaseId, Deleted)
	SELECT 
		OrganizationGuid, [Name], [Description], WebSiteUrl, IconUrl
		, LEFT(@EmptyGuid, (36 - LEN(CAST(DatabaseId AS varchar(10))))) + CAST(DatabaseId AS varchar(10))
		, Deleted
	FROM dbo.Mc_Organization_Old

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_Organization_Temp](
		[OrganizationId] [int] NOT NULL,
		[OrganizationGuid] [uniqueidentifier] NOT NULL
	)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_Organization_Temp (OrganizationId, OrganizationGuid)
	SELECT OrganizationId, OrganizationGuid
	FROM dbo.Mc_Organization_Old

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_OrganizationsLogins (OrganizationId, LoginId, OrganizationAdministrator)
	SELECT 
		o.OrganizationGuid
		, LEFT(@EmptyGuid, (36 - LEN(CAST(ol.LoginId AS varchar(10))))) + CAST(ol.LoginId AS varchar(10))
		, ol.IsOrganizationAdministrator
	FROM dbo.Mc_OrganizationsLogins_Old AS ol
	INNER JOIN dbo.Mc_Organization_Old AS o
		ON	ol.OrganizationId = o.OrganizationId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_Role (RoleId, [Name], [Description], ShortName, [Rank], StartActionId, BuiltIn, Deleted)
	SELECT 
		LEFT(@EmptyGuid, (36 - LEN(CAST(RoleId AS varchar(10))))) + CAST(RoleId AS varchar(10))
		, [Name], [Description], ShortName, [Rank]
		, LEFT(@EmptyGuid, (36 - LEN(CAST(StartActionId AS varchar(10))))) + CAST(StartActionId AS varchar(10))
		, CASE WHEN RoleId < 10 THEN 1 ELSE 0 END
		, Deleted
	 FROM dbo.Mc_Role_Old

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_RolesActions (RoleId, ActionId)
	SELECT 
		LEFT(@EmptyGuid, (36 - LEN(CAST(RoleId AS varchar(10))))) + CAST(RoleId AS varchar(10))
		, LEFT(@EmptyGuid, (36 - LEN(CAST(ActionId AS varchar(10))))) + CAST(ActionId AS varchar(10))
	FROM dbo.Mc_RolesActions_Old

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_RolesSettings (RoleId, SettingId, Value)
	SELECT 
		LEFT(@EmptyGuid, (36 - LEN(CAST(RoleId AS varchar(10))))) + CAST(RoleId AS varchar(10))
		, LEFT(@EmptyGuid, (36 - LEN(CAST(SettingId AS varchar(10))))) + CAST(SettingId AS varchar(10))
		, Value
	FROM dbo.Mc_RolesSettings_Old

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_Setting (SettingId, ParentSettingId, SettingTypeId, [Name], [Description], ShortName, DefaultValue, Value, OrderNumber, EnableOrganization, EnableDepartment, EnableRole, EnableGroup, BuiltIn, Deleted)
	SELECT 
		LEFT(@EmptyGuid, (36 - LEN(CAST(SettingId AS varchar(10))))) + CAST(SettingId AS varchar(10))
		, CASE WHEN ParentSettingId IS NULL THEN NULL ELSE LEFT(@EmptyGuid, (36 - LEN(CAST(ParentSettingId AS varchar(10))))) + CAST(ParentSettingId AS varchar(10)) END
		, SettingTypeId, [Name], [Description], ShortName, DefaultValue, Value, OrderNumber, EnableOrganization, EnableDepartment, EnableRole, EnableGroup
		, CASE WHEN SettingId < 150 THEN 1 ELSE 0 END
		, Deleted
	FROM dbo.Mc_Setting_Old

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_SettingListsValues (SettingListValueId, SettingId, [Name], Value, Deleted)
	SELECT 
		LEFT(@EmptyGuid, (36 - LEN(CAST(SettingListValuesId AS varchar(10))))) + CAST(SettingListValuesId AS varchar(10))
		, LEFT(@EmptyGuid, (36 - LEN(CAST(SettingId AS varchar(10))))) + CAST(SettingId AS varchar(10))
		, [Name], Value, Deleted
	FROM dbo.Mc_SettingListValues_Old

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO dbo.Mc_Website (WebsiteId, [Name], Url, [Description], AdminContactInfo, Deleted)
	SELECT 
		LEFT(@EmptyGuid, (36 - LEN(CAST(WebSiteId AS varchar(10))))) + CAST(WebSiteId AS varchar(10))
		, [Name], Url, [Description], AdminContactInfo, Deleted
	FROM dbo.Mc_WebSite_Old

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Action]  WITH CHECK ADD  CONSTRAINT [FK_Mc_Action_Mc_Action] FOREIGN KEY([ParentActionId])
	REFERENCES [dbo].[Mc_Action] ([ActionId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Action] CHECK CONSTRAINT [FK_Mc_Action_Mc_Action]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Action] ADD  CONSTRAINT [DF_Mc_Action_ActionId]  DEFAULT (newid()) FOR [ActionId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Action] ADD  CONSTRAINT [DF_Mc_Action_Description]  DEFAULT (N'') FOR [Description]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Action] ADD  CONSTRAINT [DF_Mc_Action_IconUrl]  DEFAULT (N'') FOR [IconUrl]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Action] ADD  CONSTRAINT [DF_Mc_Action_ButtonIconUrl]  DEFAULT (N'') FOR [ButtonIconUrl]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Action] ADD  CONSTRAINT [DF_Mc_Action_OrderNumber]  DEFAULT ((0)) FOR [OrderNumber]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Action] ADD  CONSTRAINT [DF_Mc_Action_ClassFullName]  DEFAULT (N'') FOR [ClassFullName]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Action] ADD  CONSTRAINT [DF_Mc_Action_AuthenticationRequired]  DEFAULT ((0)) FOR [AuthenticationRequired]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Action] ADD  CONSTRAINT [DF_Mc_Action_DepartmentRequired]  DEFAULT ((1)) FOR [DepartmentRequired]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Action] ADD  CONSTRAINT [DF_Mc_Action_Visible]  DEFAULT ((1)) FOR [Visible]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Action] ADD  CONSTRAINT [DF_Mc_Action_ShowInDetailMenu]  DEFAULT ((1)) FOR [ShowInDetailMenu]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Action] ADD  CONSTRAINT [DF_Mc_Action_ShowChildrenInDetailMenu]  DEFAULT ((0)) FOR [ShowChildrenInDetailMenu]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Action] ADD  CONSTRAINT [DF_Mc_Action_GroupInDetailMenu]  DEFAULT ((0)) FOR [GroupInDetailMenu]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Action] ADD  CONSTRAINT [DF_Mc_Action_HighlightInDetailMenu]  DEFAULT ((0)) FOR [HighlightInDetailMenu]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Action] ADD  CONSTRAINT [DF_Mc_Action_BuiltIn]  DEFAULT ((0)) FOR [BuiltIn]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Action] ADD  CONSTRAINT [DF_Mc_Action_Deleted]  DEFAULT ((0)) FOR [Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_ActionsParentActions]  WITH CHECK ADD  CONSTRAINT [FK_Mc_ActionsParentActions_Mc_Action_1] FOREIGN KEY([ParentActionId])
	REFERENCES [dbo].[Mc_Action] ([ActionId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_ActionsParentActions] CHECK CONSTRAINT [FK_Mc_ActionsParentActions_Mc_Action_1]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_ActionsParentActions]  WITH CHECK ADD  CONSTRAINT [FK_Mc_ActionsParentActions_Mc_Action_2] FOREIGN KEY([ActionId])
	REFERENCES [dbo].[Mc_Action] ([ActionId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_ActionsParentActions] CHECK CONSTRAINT [FK_Mc_ActionsParentActions_Mc_Action_2]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Database]  WITH CHECK ADD  CONSTRAINT [FK_Mc_Database_Mc_DatabaseServer] FOREIGN KEY([DatabaseServerId])
	REFERENCES [dbo].[Mc_DatabaseServer] ([DatabaseServerId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Database] CHECK CONSTRAINT [FK_Mc_Database_Mc_DatabaseServer]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Database] ADD  CONSTRAINT [DF_Mc_Database_DatabaseId]  DEFAULT (newid()) FOR [DatabaseId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Database] ADD  CONSTRAINT [DF_Mc_Database_Description]  DEFAULT (N'') FOR [Description]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Database] ADD  CONSTRAINT [DF_Mc_Database_Deleted]  DEFAULT ((0)) FOR [Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_DatabaseServer]  WITH CHECK ADD  CONSTRAINT [FK_Mc_DatabaseServer_Mc_Website] FOREIGN KEY([WebsiteId])
	REFERENCES [dbo].[Mc_Website] ([WebsiteId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_DatabaseServer] CHECK CONSTRAINT [FK_Mc_DatabaseServer_Mc_Website]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_DatabaseServer] ADD  CONSTRAINT [DF_Mc_DatabaseServer_DatabaseServerId]  DEFAULT (newid()) FOR [DatabaseServerId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_DatabaseServer] ADD  CONSTRAINT [DF_Mc_SqlServer_InstanceName]  DEFAULT (N'') FOR [InstanceName]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_DatabaseServer] ADD  CONSTRAINT [DF_Mc_SqlServer_Port]  DEFAULT ((0)) FOR [Port]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_DatabaseServer] ADD  CONSTRAINT [DF_Mc_SqlServer_Description]  DEFAULT (N'') FOR [Description]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_DatabaseServer] ADD  CONSTRAINT [DF_Mc_SqlServer_Deleted]  DEFAULT ((0)) FOR [Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Login] ADD  CONSTRAINT [DF_Mc_Login_LoginId]  DEFAULT (newid()) FOR [LoginId]


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Login] ADD  CONSTRAINT [DF_Mc_Login_Deleted]  DEFAULT ((0)) FOR [Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization]  WITH CHECK ADD  CONSTRAINT [FK_Mc_Organization_Mc_Database] FOREIGN KEY([DatabaseId])
	REFERENCES [dbo].[Mc_Database] ([DatabaseId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization] CHECK CONSTRAINT [FK_Mc_Organization_Mc_Database]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_OrganizationId]  DEFAULT (newid()) FOR [OrganizationId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_Description]  DEFAULT (N'') FOR [Description]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_WebsiteUrl]  DEFAULT (N'') FOR [WebsiteUrl]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_IconUrl]  DEFAULT (N'') FOR [IconUrl]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization] ADD  CONSTRAINT [DF_Mc_Organization_Deleted]  DEFAULT ((0)) FOR [Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_OrganizationsLogins]  WITH CHECK ADD  CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Login] FOREIGN KEY([LoginId])
	REFERENCES [dbo].[Mc_Login] ([LoginId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_OrganizationsLogins] CHECK CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Login]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_OrganizationsLogins]  WITH CHECK ADD  CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Organization] FOREIGN KEY([OrganizationId])
	REFERENCES [dbo].[Mc_Organization] ([OrganizationId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_OrganizationsLogins] CHECK CONSTRAINT [FK_Mc_OrganizationsLogins_Mc_Organization]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_OrganizationsLogins] ADD  CONSTRAINT [DF_Mc_OrganizationsLogins_OrganizationAdministrator]  DEFAULT ((0)) FOR [OrganizationAdministrator]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Role]  WITH CHECK ADD  CONSTRAINT [FK_Mc_Role_Mc_Action] FOREIGN KEY([StartActionId])
	REFERENCES [dbo].[Mc_Action] ([ActionId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Role] CHECK CONSTRAINT [FK_Mc_Role_Mc_Action]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Role] ADD  CONSTRAINT [DF_Mc_Role_RoleId]  DEFAULT (newid()) FOR [RoleId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Role] ADD  CONSTRAINT [DF_Mc_Role_Description]  DEFAULT (N'') FOR [Description]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Role] ADD  CONSTRAINT [DF_Mc_Role_BuiltIn]  DEFAULT ((0)) FOR [BuiltIn]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Role] ADD  CONSTRAINT [DF_Mc_Role_Deleted]  DEFAULT ((0)) FOR [Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_RolesActions]  WITH CHECK ADD  CONSTRAINT [FK_Mc_RolesActions_Mc_Action] FOREIGN KEY([ActionId])
	REFERENCES [dbo].[Mc_Action] ([ActionId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_RolesActions] CHECK CONSTRAINT [FK_Mc_RolesActions_Mc_Action]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_RolesActions]  WITH CHECK ADD  CONSTRAINT [FK_Mc_RolesActions_Mc_Role] FOREIGN KEY([RoleId])
	REFERENCES [dbo].[Mc_Role] ([RoleId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_RolesActions] CHECK CONSTRAINT [FK_Mc_RolesActions_Mc_Role]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_RolesSettings]  WITH CHECK ADD  CONSTRAINT [FK_Mc_RolesSettings_Mc_Role] FOREIGN KEY([RoleId])
	REFERENCES [dbo].[Mc_Role] ([RoleId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_RolesSettings] CHECK CONSTRAINT [FK_Mc_RolesSettings_Mc_Role]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_RolesSettings]  WITH CHECK ADD  CONSTRAINT [FK_Mc_RolesSettings_Mc_Setting] FOREIGN KEY([SettingId])
	REFERENCES [dbo].[Mc_Setting] ([SettingId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_RolesSettings] CHECK CONSTRAINT [FK_Mc_RolesSettings_Mc_Setting]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Session]  WITH CHECK ADD  CONSTRAINT [FK_Mc_Session_Mc_Login] FOREIGN KEY([LoginId])
	REFERENCES [dbo].[Mc_Login] ([LoginId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Session] CHECK CONSTRAINT [FK_Mc_Session_Mc_Login]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Setting]  WITH CHECK ADD  CONSTRAINT [FK_Mc_Setting_Mc_Setting] FOREIGN KEY([ParentSettingId])
	REFERENCES [dbo].[Mc_Setting] ([SettingId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Setting] CHECK CONSTRAINT [FK_Mc_Setting_Mc_Setting]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Setting] ADD  CONSTRAINT [DF_Mc_Setting_SettingId]  DEFAULT (newid()) FOR [SettingId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Setting] ADD  CONSTRAINT [DF_Mc_Setting_Description]  DEFAULT (N'') FOR [Description]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Setting] ADD  CONSTRAINT [DF_Mc_Setting_DefaultValue]  DEFAULT (N'') FOR [DefaultValue]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Setting] ADD  CONSTRAINT [DF_Mc_Setting_OrderNumber]  DEFAULT ((0)) FOR [OrderNumber]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Setting] ADD  CONSTRAINT [DF_Mc_Setting_EnableOrganization]  DEFAULT ((0)) FOR [EnableOrganization]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Setting] ADD  CONSTRAINT [DF_Mc_Setting_EnableDepartment]  DEFAULT ((0)) FOR [EnableDepartment]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Setting] ADD  CONSTRAINT [DF_Mc_Setting_EnableRole]  DEFAULT ((0)) FOR [EnableRole]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Setting] ADD  CONSTRAINT [DF_Mc_Setting_EnableGroup]  DEFAULT ((0)) FOR [EnableGroup]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Setting] ADD  CONSTRAINT [DF_Mc_Setting_BuiltIn]  DEFAULT ((0)) FOR [BuiltIn]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Setting] ADD  CONSTRAINT [DF_Mc_Setting_Deleted]  DEFAULT ((0)) FOR [Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_SettingListsValues]  WITH CHECK ADD  CONSTRAINT [FK_Mc_SettingListsValues_Mc_Setting] FOREIGN KEY([SettingId])
	REFERENCES [dbo].[Mc_Setting] ([SettingId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_SettingListsValues] CHECK CONSTRAINT [FK_Mc_SettingListsValues_Mc_Setting]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_SettingListsValues] ADD  CONSTRAINT [DF_Mc_SettingListValues_SettingListValueId]  DEFAULT (newid()) FOR [SettingListValueId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_SettingListsValues] ADD  CONSTRAINT [DF_Mc_SettingListValues_Deleted]  DEFAULT ((0)) FOR [Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_ViewState]  WITH CHECK ADD  CONSTRAINT [FK_Mc_ViewState_Mc_Session] FOREIGN KEY([SessionId])
	REFERENCES [dbo].[Mc_Session] ([SessionId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_ViewState] CHECK CONSTRAINT [FK_Mc_ViewState_Mc_Session]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Website] ADD  CONSTRAINT [DF_Mc_Website_WebsiteId]  DEFAULT (newid()) FOR [WebsiteId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Website] ADD  CONSTRAINT [DF_Mc_Website_Description]  DEFAULT ('') FOR [Description]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Website] ADD  CONSTRAINT [DF_Mc_Website_AdminContactInfo]  DEFAULT (N'') FOR [AdminContactInfo]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Website] ADD  CONSTRAINT [DF_Mc_Website_Deleted]  DEFAULT ((0)) FOR [Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
BEGIN
	DECLARE @OrganizationId int, @OrganizationGuid uniqueidentifier

	SET @Output = 'CREATE TABLE dbo.Mc_Organization_Old (OrganizationId int NOT NULL, OrganizationGuid uniqueidentifier NOT NULL)'

	DECLARE Cursor1 CURSOR FOR
	SELECT OrganizationId, OrganizationGuid 
	FROM dbo.Mc_Organization_Old

	OPEN Cursor1

	FETCH NEXT FROM Cursor1
	INTO @OrganizationId, @OrganizationGuid

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @Output = @Output + CHAR(13) + CHAR(10) + 'INSERT INTO dbo.Mc_Organization_Old (OrganizationId, OrganizationGuid) VALUES (' + CAST(@OrganizationId AS varchar(10)) +  ', ''''' + CAST(@OrganizationGuid AS varchar(50)) + ''''')'

		IF @@ERROR <> 0
		BEGIN
			CLOSE Cursor1
			DEALLOCATE Cursor1
			
			IF @@TRANCOUNT = 1 
				ROLLBACK TRANSACTION
			
			RETURN
		END

		FETCH NEXT FROM Cursor1
		INTO @OrganizationId, @OrganizationGuid
	END

	CLOSE Cursor1
	DEALLOCATE Cursor1
END

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
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteActionParentAction]
(
	@ActionId uniqueidentifier,
	@ParentActionId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_ActionsParentActions
	WHERE (ActionId = @ActionId) AND (ParentActionId = @ParentActionId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteOrganizationLogin]
(
	@OrganizationId uniqueidentifier,
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_OrganizationsLogins 
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteRoleAction]
(
	@RoleId uniqueidentifier,
	@ActionId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_RolesActions 
	WHERE (RoleId = @RoleId) AND (ActionId = @ActionId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteRoleSetting]
(
	@RoleId uniqueidentifier,
	@SettingId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_RolesSettings 
	WHERE (RoleId = @RoleId) AND (SettingId = @SettingId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteSession]
(
    @SessionCookie varchar(50),
	@Timeout int
)
AS
BEGIN
    SET NOCOUNT ON;

	DELETE dbo.Mc_ViewState
	WHERE	SessionId IN (
				SELECT SessionId 
				FROM dbo.Mc_Session 
				WHERE	((@Timeout IS NOT NULL) AND (DATEDIFF(MINUTE, LastAccessTime, GETDATE()) >= @Timeout))
						OR (SessionCookie = @SessionCookie)
			);

    DELETE FROM dbo.Mc_Session
    WHERE	((@Timeout IS NOT NULL) AND (DATEDIFF(MINUTE, LastAccessTime, GETDATE()) >= @Timeout))
			OR (SessionCookie = @SessionCookie);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetActions]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ActionId, ParentActionId, ActionTypeId, [Name], [Description], IconUrl, ButtonIconUrl, NavigateUrl, OrderNumber, ClassFullName, AuthenticationRequired, DepartmentRequired, Visible, ShowInDetailMenu, ShowChildrenInDetailMenu, GroupInDetailMenu, HighlightInDetailMenu, BuiltIn, Deleted 
	FROM dbo.Mc_Action 
	WHERE (Deleted = 0) 
	ORDER BY ParentActionId, OrderNumber, [Name];
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetActionsParentActions]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.ActionId, p.ParentActionId
	FROM dbo.Mc_ActionsParentActions AS p
	INNER JOIN dbo.Mc_Action AS a
		ON	(p.ActionId = a.ActionId) AND (a.Deleted = 0)
	INNER JOIN dbo.Mc_Action AS a1
		ON	(p.ActionId = a1.ActionId) AND (a1.Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetDatabases]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DatabaseId, [Name], [Description], UserName, [Password], DatabaseServerId, Deleted 
	FROM dbo.Mc_Database
	WHERE (Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetDatabaseServers]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DatabaseServerId, [Name], InstanceName, Port, [Description], WebsiteId, Deleted 
	FROM dbo.Mc_DatabaseServer 
	WHERE (Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetLogin]
(
	@LoginId uniqueidentifier,
	@LoginName nvarchar(255),
	@Password nvarchar(50)
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT l.LoginId, l.LoginName, l.[Password], l.Deleted
		,	CASE WHEN EXISTS(
				SELECT TOP 1 o.OrganizationId
				FROM dbo.Mc_Organization AS o
				INNER JOIN dbo.Mc_OrganizationsLogins AS ol
					ON	(o.OrganizationId = ol.OrganizationId) AND (ol.LoginId = l.LoginId) AND (o.Deleted = 0)
			) THEN 1 ELSE 0 END AS OrganizationExists
	FROM dbo.Mc_Login AS l
	WHERE 
		((@LoginId IS NULL) OR (l.LoginId = @LoginId))
		AND ((@LoginName IS NULL) OR (l.LoginName = @LoginName))
		AND ((@Password IS NULL) OR (l.[Password] = @Password));
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetLoginsByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT l.LoginId, l.LoginName, l.[Password], l.Deleted, ol.OrganizationAdministrator
	FROM dbo.Mc_OrganizationsLogins AS ol
	INNER JOIN dbo.Mc_Login AS l
		ON	(ol.LoginId = l.LoginId) AND (ol.OrganizationId = @OrganizationId) AND (l.Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetOrganizationLogin]
(
	@OrganizationId uniqueidentifier,
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT OrganizationId, LoginId, OrganizationAdministrator
	FROM dbo.Mc_OrganizationsLogins
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetOrganizations]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT OrganizationId, [Name], [Description], WebsiteUrl, IconUrl, DatabaseId, Deleted 
	FROM dbo.Mc_Organization 
	WHERE (Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetOrganizationsByLoginId]
(
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT o.OrganizationId, o.[Name], o.[Description], o.WebsiteUrl, o.IconUrl, o.DatabaseId, o.Deleted
	FROM dbo.Mc_OrganizationsLogins AS ol
	INNER JOIN dbo.Mc_Organization AS o
		ON	(ol.OrganizationId = o.OrganizationId) AND (ol.LoginId = @LoginId) AND (o.Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetRoles]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT RoleId, [Name], [Description], ShortName, [Rank], StartActionId, BuiltIn, Deleted 
	FROM dbo.Mc_Role 
	WHERE (Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetRolesActions]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ra.RoleId, ra.ActionId 
	FROM dbo.Mc_RolesActions AS ra
	INNER JOIN dbo.Mc_Role AS r
		ON	(ra.RoleId = r.RoleId) AND (r.Deleted = 0)
	INNER JOIN dbo.Mc_Action AS a
		ON	(ra.ActionId = a.ActionId) AND (a.Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetRolesSettings]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT rs.RoleId, rs.SettingId, rs.Value
	FROM dbo.Mc_RolesSettings AS rs
	INNER JOIN dbo.Mc_Role AS r
		ON	(rs.RoleId = r.RoleId) AND (r.Deleted = 0)
	INNER JOIN dbo.Mc_Setting AS s
		ON	(rs.SettingId = s.SettingId) AND (s.Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetSettingListsValues]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT SettingListValueId, SettingId, [Name], Value, Deleted 
	FROM dbo.Mc_SettingListsValues 
	WHERE (Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetSettings]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT SettingId, ParentSettingId, SettingTypeId, [Name], [Description], ShortName, DefaultValue, Value, OrderNumber, EnableOrganization, EnableDepartment, EnableRole, EnableGroup, BuiltIn, Deleted 
	FROM dbo.Mc_Setting 
	WHERE (Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetWebsites]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT WebsiteId, [Name], Url, [Description], AdminContactInfo, Deleted 
	FROM dbo.Mc_Website 
	WHERE (Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertAction]
(
	@ActionId uniqueidentifier,
	@ParentActionId uniqueidentifier,
	@ActionTypeId int,
	@Name nvarchar(1024),
	@Description nvarchar(1024),
	@IconUrl nvarchar(2048),
	@ButtonIconUrl nvarchar(2048),
	@NavigateUrl nvarchar(2048),
	@OrderNumber int,
	@ClassFullName nvarchar(1024),
	@AuthenticationRequired bit,
	@DepartmentRequired bit,
	@Visible bit,
	@ShowInDetailMenu bit,
	@ShowChildrenInDetailMenu bit,
	@GroupInDetailMenu bit,
	@HighlightInDetailMenu bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Action (ActionId, ParentActionId, ActionTypeId, [Name], [Description], IconUrl, ButtonIconUrl, NavigateUrl, OrderNumber, ClassFullName, AuthenticationRequired, DepartmentRequired, Visible, ShowInDetailMenu, ShowChildrenInDetailMenu, GroupInDetailMenu, HighlightInDetailMenu, BuiltIn, Deleted) 
	VALUES (@ActionId, @ParentActionId, @ActionTypeId, @Name, @Description, @IconUrl, @ButtonIconUrl, @NavigateUrl, @OrderNumber, @ClassFullName, @AuthenticationRequired, @DepartmentRequired, @Visible, @ShowInDetailMenu, @ShowChildrenInDetailMenu, @GroupInDetailMenu, @HighlightInDetailMenu, 0, @Deleted);
	
	SELECT ActionId, ParentActionId, ActionTypeId, [Name], [Description], IconUrl, ButtonIconUrl, NavigateUrl, OrderNumber, ClassFullName, AuthenticationRequired, DepartmentRequired, Visible, ShowInDetailMenu, ShowChildrenInDetailMenu, GroupInDetailMenu, HighlightInDetailMenu, BuiltIn, Deleted 
	FROM dbo.Mc_Action 
	WHERE (ActionId = @ActionId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertActionParentAction]
(
	@ActionId uniqueidentifier,
	@ParentActionId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_ActionsParentActions (ActionId, ParentActionId) 
	VALUES (@ActionId, @ParentActionId);
	
	SELECT ActionId, ParentActionId 
	FROM dbo.Mc_ActionsParentActions 
	WHERE (ActionId = @ActionId) AND (ParentActionId = @ParentActionId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertDatabase]
(
	@DatabaseId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@UserName nvarchar(255),
	@Password nvarchar(255),
	@DatabaseServerId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Database (DatabaseId, [Name], [Description], UserName, [Password], DatabaseServerId, Deleted) 
	VALUES (@DatabaseId, @Name, @Description, @UserName, @Password, @DatabaseServerId, @Deleted);
	
	SELECT DatabaseId, [Name], [Description], UserName, [Password], DatabaseServerId, Deleted 
	FROM dbo.Mc_Database 
	WHERE (DatabaseId = @DatabaseId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertDatabaseServer]
(
	@DatabaseServerId uniqueidentifier,
	@Name nvarchar(255),
	@InstanceName nvarchar(255),
	@Port int,
	@Description nvarchar(1024),
	@WebsiteId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_DatabaseServer (DatabaseServerId, [Name], InstanceName, Port, [Description], WebsiteId, Deleted) 
	VALUES (@DatabaseServerId, @Name, @InstanceName, @Port, @Description, @WebSiteId, @Deleted);
	
	SELECT @DatabaseServerId, [Name], InstanceName, Port, [Description], WebsiteId, Deleted 
	FROM dbo.Mc_DatabaseServer 
	WHERE (DatabaseServerId = @DatabaseServerId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertLogin]
(
	@LoginId uniqueidentifier,
	@LoginName nvarchar(255),
	@Password nvarchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Login (LoginId, LoginName, [Password], Deleted) 
	VALUES (@LoginId, @LoginName, @Password, 0);
	
	SELECT LoginId, LoginName, [Password], Deleted 
	FROM dbo.Mc_Login 
	WHERE (LoginId = @LoginId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertOrganization]
(
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(255),
	@WebsiteUrl nvarchar(2048),
	@IconUrl nvarchar(2048),
	@DatabaseId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Organization (OrganizationId, [Name], [Description], WebsiteUrl, IconUrl, DatabaseId, Deleted) 
	VALUES (@OrganizationId, @Name, @Description, @WebsiteUrl, @IconUrl, @DatabaseId, @Deleted);
	
	SELECT OrganizationId, [Name], [Description], WebsiteUrl, IconUrl, DatabaseId, Deleted 
	FROM dbo.Mc_Organization 
	WHERE (OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertOrganizationLogin]
(
	@OrganizationId uniqueidentifier,
	@LoginId uniqueidentifier,
	@OrganizationAdministrator bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_OrganizationsLogins (OrganizationId, LoginId, OrganizationAdministrator)
	VALUES (@OrganizationId, @LoginId, @OrganizationAdministrator);
	
	SELECT OrganizationId, LoginId, OrganizationAdministrator
	FROM dbo.Mc_OrganizationsLogins
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertRole]
(
	@RoleId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@ShortName nvarchar(50),
	@Rank int,
	@StartActionId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Role (RoleId, [Name], [Description], ShortName, [Rank], StartActionId, BuiltIn, Deleted) 
	VALUES (@RoleId, @Name, @Description, @ShortName, @Rank, @StartActionId, 0, @Deleted);
	
	SELECT RoleId, [Name], [Description], ShortName, [Rank], StartActionId, BuiltIn, Deleted 
	FROM dbo.Mc_Role 
	WHERE (RoleId = @RoleId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertRoleAction]
(
	@RoleId uniqueidentifier,
	@ActionId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_RolesActions (RoleId, ActionId)
	VALUES (@RoleId, @ActionId);
	
	SELECT RoleId, ActionId 
	FROM dbo.Mc_RolesActions 
	WHERE (ActionId = @ActionId) AND (RoleId = @RoleId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertRoleSetting]
(
	@RoleId uniqueidentifier,
	@SettingId uniqueidentifier,
	@Value nvarchar(512)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_RolesSettings (RoleId, SettingId, Value) 
	VALUES (@RoleId, @SettingId, @Value);
	
	SELECT RoleId, SettingId, Value 
	FROM dbo.Mc_RolesSettings 
	WHERE (RoleId = @RoleId) AND (SettingId = @SettingId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertSetting]
(
	@SettingId uniqueidentifier,
	@ParentSettingId uniqueidentifier,
	@SettingTypeId int,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@ShortName nvarchar(50),
	@DefaultValue nvarchar(512),
	@Value nvarchar(' + @NVarCharMax + '),
	@OrderNumber int,
	@EnableOrganization bit,
	@EnableDepartment bit,
	@EnableRole bit,
	@EnableGroup bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Setting (SettingId, ParentSettingId, SettingTypeId, [Name], [Description], ShortName, DefaultValue, Value, OrderNumber, EnableOrganization, EnableDepartment, EnableRole, EnableGroup, BuiltIn, Deleted) 
	VALUES (@SettingId, @ParentSettingId, @SettingTypeId, @Name, @Description, @ShortName, @DefaultValue, @Value, @OrderNumber, @EnableOrganization, @EnableDepartment, @EnableRole, @EnableGroup, 0, @Deleted);
	
	SELECT SettingId, ParentSettingId, SettingTypeId, [Name], [Description], ShortName, DefaultValue, Value, OrderNumber, EnableOrganization, EnableDepartment, EnableRole, EnableGroup, BuiltIn, Deleted 
	FROM dbo.Mc_Setting 
	WHERE (SettingId = @SettingId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertSettingListValue]
(
	@SettingListValueId uniqueidentifier,
	@SettingId uniqueidentifier,
	@Name nvarchar(255),
	@Value nvarchar(512),
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_SettingListsValues (SettingListValueId, SettingId, [Name], Value, Deleted)
	VALUES (@SettingListValueId, @SettingId, @Name, @Value, @Deleted);
	
	SELECT SettingListValueId, SettingId, [Name], Value, Deleted 
	FROM dbo.Mc_SettingListsValues 
	WHERE (SettingListValueId = @SettingListValueId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertWebsite]
(
	@WebsiteId uniqueidentifier,
	@Name nvarchar(255),
	@Url nvarchar(2048),
	@Description nvarchar(1024),
	@AdminContactInfo nvarchar(2048),
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Website (WebsiteId, [Name], Url, [Description], [AdminContactInfo], [Deleted]) 
	VALUES (@WebsiteId, @Name, @Url, @Description, @AdminContactInfo, @Deleted);
	
	SELECT WebsiteId, [Name], Url, [Description], AdminContactInfo, Deleted 
	FROM dbo.Mc_Website 
	WHERE (WebsiteId = @WebsiteId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateAction]
(
	@ActionId uniqueidentifier,
	@ParentActionId uniqueidentifier,
	@ActionTypeId int,
	@Name nvarchar(1024),
	@Description nvarchar(1024),
	@IconUrl nvarchar(2048),
	@ButtonIconUrl nvarchar(2048),
	@NavigateUrl nvarchar(2048),
	@OrderNumber int,
	@ClassFullName nvarchar(1024),
	@AuthenticationRequired bit,
	@DepartmentRequired bit,
	@Visible bit,
	@ShowInDetailMenu bit,
	@ShowChildrenInDetailMenu bit,
	@GroupInDetailMenu bit,
	@HighlightInDetailMenu bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Action 
	SET ParentActionId = @ParentActionId, ActionTypeId = @ActionTypeId, [Name] = @Name, [Description] = @Description, IconUrl = @IconUrl, ButtonIconUrl = @ButtonIconUrl, NavigateUrl = @NavigateUrl, OrderNumber = @OrderNumber, ClassFullName = @ClassFullName, AuthenticationRequired = @AuthenticationRequired, DepartmentRequired = @DepartmentRequired, Visible = @Visible, ShowInDetailMenu = @ShowInDetailMenu, ShowChildrenInDetailMenu = @ShowChildrenInDetailMenu, GroupInDetailMenu = @GroupInDetailMenu, HighlightInDetailMenu = @HighlightInDetailMenu, Deleted = @Deleted 
	WHERE (ActionId = @ActionId);
	
	SELECT ActionId, ParentActionId, ActionTypeId, [Name], [Description], IconUrl, ButtonIconUrl, NavigateUrl, OrderNumber, ClassFullName, AuthenticationRequired, DepartmentRequired, Visible, ShowInDetailMenu, ShowChildrenInDetailMenu, GroupInDetailMenu, HighlightInDetailMenu, BuiltIn, Deleted 
	FROM dbo.Mc_Action 
	WHERE (ActionId = @ActionId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateDatabase]
(
	@DatabaseId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@UserName nvarchar(255),
	@Password nvarchar(255),
	@DatabaseServerId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Database 
	SET [Name] = @Name, [Description] = @Description, UserName = @UserName, [Password] = @Password, DatabaseServerId = @DatabaseServerId, Deleted = @Deleted 
	WHERE ([DatabaseId] = @DatabaseId);
	
	SELECT DatabaseId, [Name], [Description], UserName, [Password], DatabaseServerId, Deleted 
	FROM dbo.Mc_Database 
	WHERE (DatabaseId = @DatabaseId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateDatabaseServer]
(
	@DatabaseServerId uniqueidentifier,
	@Name nvarchar(255),
	@InstanceName nvarchar(255),
	@Port int,
	@Description nvarchar(1024),
	@WebsiteId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_DatabaseServer 
	SET [Name] = @Name, InstanceName = @InstanceName, Port = @Port, [Description] = @Description, WebsiteId = @WebsiteId, Deleted = @Deleted 
	WHERE (DatabaseServerId = @DatabaseServerId);
	
	SELECT DatabaseServerId, [Name], InstanceName, Port, [Description], WebsiteId, Deleted 
	FROM dbo.Mc_DatabaseServer 
	WHERE (DatabaseServerId = @DatabaseServerId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateLogin]
(
	@LoginId uniqueidentifier,
	@LoginName nvarchar(255),
	@Password nvarchar(50),
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Login 
	SET LoginName = (CASE WHEN @LoginName IS NULL THEN LoginName ELSE @LoginName END)
		, [Password] = (CASE WHEN @Password IS NULL THEN [Password] ELSE @Password END)
		, Deleted = (CASE WHEN @Deleted IS NULL THEN Deleted ELSE @Deleted END)
	WHERE (LoginId = @LoginId);
	
	
	SELECT LoginId, LoginName, [Password], Deleted
	FROM dbo.Mc_Login
	WHERE (LoginId = @LoginId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateOrganization]
(
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(255),
	@WebsiteUrl nvarchar(2048),
	@IconUrl nvarchar(2048),
	@DatabaseId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Organization 
	SET [Name] = @Name, [Description] = @Description, WebsiteUrl = @WebsiteUrl, IconUrl = @IconUrl, DatabaseId = @DatabaseId, Deleted = @Deleted 
	WHERE (OrganizationId = @OrganizationId);
	
	SELECT OrganizationId, [Name], [Description], WebsiteUrl, IconUrl, DatabaseId, Deleted 
	FROM dbo.Mc_Organization 
	WHERE (OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateOrganizationLogin]
(
	@OrganizationId uniqueidentifier,
	@LoginId uniqueidentifier,
	@OrganizationAdministrator bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_OrganizationsLogins 
	SET OrganizationAdministrator = @OrganizationAdministrator 
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
	
	SELECT OrganizationId, LoginId, OrganizationAdministrator
	FROM dbo.Mc_OrganizationsLogins
	WHERE (OrganizationId = @OrganizationId) AND (LoginId = @LoginId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateRole]
(
	@RoleId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@ShortName nvarchar(50),
	@Rank int,
	@StartActionId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Role 
	SET [Name] = @Name, [Description] = @Description, ShortName = @ShortName, [Rank] = @Rank, StartActionId = @StartActionId, Deleted = @Deleted 
	WHERE (RoleId = @RoleId);
	
	SELECT RoleId, [Name], [Description], ShortName, [Rank], StartActionId, BuiltIn, Deleted 
	FROM dbo.Mc_Role 
	WHERE (RoleId = @RoleId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateRoleSetting]
(
	@RoleId uniqueidentifier,
	@SettingId uniqueidentifier,
	@Value nvarchar(512)
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_RolesSettings 
	SET Value = @Value 
	WHERE (RoleId = @RoleId) AND (SettingId = @SettingId);
	
	SELECT RoleId, SettingId, [Value] 
	FROM dbo.Mc_RolesSettings 
	WHERE (RoleId = @RoleId) AND (SettingId = @SettingId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateSession]
(
	@SessionCookie varchar(50),
    @SessionId int OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

	SELECT @SessionId = SessionId 
    FROM dbo.Mc_Session 
    WHERE SessionCookie = @SessionCookie;

    IF @SessionId IS NULL
    BEGIN
        INSERT INTO dbo.Mc_Session (SessionCookie, CreatedTime, LastAccessTime) 
        VALUES (@SessionCookie, GETDATE(), GETDATE());
        
        SET @SessionId = SCOPE_IDENTITY();
    END
    ELSE
        UPDATE dbo.Mc_Session
        SET LastAccessTime = GETDATE() 
        WHERE SessionId = @SessionId;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetViewState]
(
    @ViewStateId int,
    @SessionCookie varchar(50)
)
AS
BEGIN
    SET NOCOUNT ON;
    
    EXEC [Mc_UpdateSession] @SessionCookie, NULL;

    SELECT ViewState
    FROM dbo.Mc_ViewState 
    WHERE (ViewStateId = @ViewStateId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateSessionLogin]
(
	@SessionCookie varchar(50),
	@LoginId uniqueidentifier
)
AS
BEGIN
    SET NOCOUNT ON;

	IF (@LoginId IS NOT NULL)
	BEGIN
		DECLARE @FirstSessionCookie varchar(50);
		
		SELECT @FirstSessionCookie = SessionCookie
		FROM dbo.Mc_Session
		WHERE (LoginId = @LoginId) AND (SessionCookie <> @SessionCookie);
	    
		IF (@FirstSessionCookie IS NOT NULL)
			EXEC dbo.Mc_DeleteSession @FirstSessionCookie, NULL;
	END
	
	EXEC dbo.Mc_UpdateSession @SessionCookie, NULL;

	UPDATE dbo.Mc_Session
	SET LoginId = @LoginId
	WHERE SessionCookie = @SessionCookie;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateSetting]
(
	@SettingId uniqueidentifier,
	@ParentSettingId uniqueidentifier,
	@SettingTypeId int,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@ShortName nvarchar(50),
	@DefaultValue nvarchar(512),
	@Value nvarchar(' + @NVarCharMax + '),
	@OrderNumber int,
	@EnableOrganization bit,
	@EnableDepartment bit,
	@EnableRole bit,
	@EnableGroup bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Setting 
	SET ParentSettingId = @ParentSettingId, SettingTypeId = @SettingTypeId, [Name] = @Name, [Description] = @Description, ShortName = @ShortName, DefaultValue = @DefaultValue, Value = @Value, OrderNumber = @OrderNumber, EnableOrganization = @EnableOrganization, EnableDepartment = @EnableDepartment, EnableRole = @EnableRole, EnableGroup = @EnableGroup , Deleted = @Deleted 
	WHERE (SettingId = @SettingId);
	
	SELECT SettingId, ParentSettingId, SettingTypeId, [Name], [Description], ShortName, DefaultValue, Value, OrderNumber, EnableOrganization, EnableDepartment, EnableRole, EnableGroup, BuiltIn, Deleted 
	FROM dbo.Mc_Setting 
	WHERE (SettingId = @SettingId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateSettingListValue]
(
	@SettingListValueId uniqueidentifier,
	@SettingId uniqueidentifier,
	@Name nvarchar(255),
	@Value nvarchar(512),
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_SettingListsValues 
	SET SettingId = @SettingId, [Name] = @Name, Value = @Value, Deleted = @Deleted 
	WHERE (SettingListValueId = @SettingListValueId);
	
	SELECT SettingListValueId, SettingId, [Name], Value, Deleted 
	FROM Mc_SettingListsValues 
	WHERE (SettingListValueId = @SettingListValueId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateViewState]
(
	@ViewStateId int OUTPUT,
    @SessionCookie varchar(50),
    @ViewState image
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SessionId INT;
  
    EXEC dbo.Mc_UpdateSession @SessionCookie, @SessionId OUTPUT;
    
    IF EXISTS(
		SELECT 0
		FROM dbo.Mc_ViewState
		WHERE (ViewStateId = @ViewStateId) AND (SessionId = @SessionId)
    )
		UPDATE dbo.Mc_ViewState
		SET ViewState = @ViewState
		WHERE ViewStateId = @ViewStateId;
    ELSE
    BEGIN
		INSERT INTO dbo.Mc_ViewState (SessionId, ViewState) 
		VALUES (@SessionId, @ViewState);
    
		SET @ViewStateId = SCOPE_IDENTITY();
    END
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateWebsite]
(
	@WebsiteId uniqueidentifier,
	@Name nvarchar(255),
	@Url nvarchar(2048),
	@Description nvarchar(1024),
	@AdminContactInfo nvarchar(2048),
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_WebSite
	SET [Name] = @Name, Url = @Url, [Description] = @Description, AdminContactInfo = @AdminContactInfo, Deleted = @Deleted 
	WHERE (WebsiteId = @WebsiteId);
	
	SELECT WebsiteId, [Name], Url, [Description], AdminContactInfo, Deleted 
	FROM dbo.Mc_WebSite 
	WHERE (WebsiteId = @WebsiteId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_ValidateSession]
(
	@SessionCookie varchar(50),
	@LoginId uniqueidentifier,
	@SessionIsValid bit OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

	SET @SessionIsValid = 0;

	IF EXISTS(
		SELECT 0 
		FROM dbo.Mc_Session 
		WHERE (LoginId = @LoginId) AND (SessionCookie = @SessionCookie)
	)
		SET @SessionIsValid = 1;
	ELSE IF NOT EXISTS(
		SELECT 0 
		FROM dbo.Mc_Session 
		WHERE (LoginId = @LoginId)
	)
		SET @SessionIsValid = 1;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION

SELECT @Output
