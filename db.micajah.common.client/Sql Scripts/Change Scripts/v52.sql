-- Converts the old groups for Instance Administrators to new groups.
BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

DECLARE @InstanceAdministratorRoleId uniqueidentifier
SET @InstanceAdministratorRoleId = '00000000-0000-0000-0000-000000000002'

DECLARE @TmpGroup TABLE(
	[GroupId] uniqueidentifier,
	[OrganizationId] uniqueidentifier,
	[Name] nvarchar(255),
	[Description] nvarchar(1024),
	[InstanceId] uniqueidentifier
)

DECLARE @OldGroups TABLE (
	[GroupId] uniqueidentifier
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO @OldGroups (GroupId)
	SELECT g.GroupId
	FROM dbo.Mc_Group AS g
	INNER JOIN	dbo.Mc_GroupsInstancesRoles AS gir
		ON	g.GroupId = gir.GroupId
	WHERE gir.RoleId = @InstanceAdministratorRoleId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	-- Creates the new groups for Instance Administrators: 1 group per 1 instance
	INSERT INTO @TmpGroup ([GroupId], [OrganizationId], [Name], [Description], [InstanceId])
	SELECT 
		NEWID() AS GroupId
		, i.OrganizationId
		, i.[Name] + ' Administrators' AS [Name]
		, 'The built-in Group for the Instance Administrators' AS [Description]
		, i.InstanceId
	FROM dbo.Mc_Instance AS i
	WHERE i.Deleted = 0

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	-- Inserts new groups to real table
	INSERT INTO dbo.Mc_Group ([GroupId], [OrganizationId], [Name], [Description], [BuiltIn], [Deleted])
	SELECT [GroupId], [OrganizationId], [Name], [Description], 1, 0
	FROM @TmpGroup

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	-- Associates the users with new groups
	INSERT Mc_UsersGroups ([UserId], [GroupId])
	SELECT ug.UserId, tg.GroupId
	FROM dbo.Mc_UsersGroups AS ug
	INNER JOIN dbo.Mc_GroupsInstancesRoles AS gir
		ON	ug.GroupId = gir.GroupId
	INNER JOIN @TmpGroup AS tg
		ON	gir.InstanceId = tg.InstanceId
	WHERE gir.RoleId = @InstanceAdministratorRoleId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	-- Deletes the permissions to the actions of the old groups
	DELETE gia 
	FROM dbo.Mc_GroupsInstancesActions AS gia
	INNER JOIN dbo.Mc_GroupsInstancesRoles AS gir
		ON	gia.GroupId = gir.GroupId
			AND gia.InstanceId = gir.InstanceId
	WHERE gir.RoleId = @InstanceAdministratorRoleId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	-- Delete the associates with old groups
	DELETE ug
	FROM dbo.Mc_UsersGroups AS ug
	INNER JOIN dbo.Mc_GroupsInstancesRoles AS gir
		ON	ug.GroupId = gir.GroupId
	WHERE gir.RoleId = @InstanceAdministratorRoleId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	-- Deletes the roles of the old groups
	DELETE dbo.Mc_GroupsInstancesRoles
	WHERE RoleId = @InstanceAdministratorRoleId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	-- Deletes the old groups
	DELETE FROM dbo.Mc_Group 
	WHERE GroupId IN (
		SELECT og.GroupId
		FROM @OldGroups AS og
		WHERE 
			NOT EXISTS (SELECT 0 FROM dbo.Mc_GroupsInstancesRoles WHERE GroupId = og.GroupId)
			AND NOT EXISTS (SELECT 0 FROM dbo.Mc_GroupsInstancesActions WHERE GroupId = og.GroupId)
		)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	-- Associates the new groups with Instance Administrator role
	INSERT INTO dbo.Mc_GroupsInstancesRoles (GroupId, InstanceId, RoleId)
	SELECT [GroupId], [InstanceId], @InstanceAdministratorRoleId
	FROM @TmpGroup

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
