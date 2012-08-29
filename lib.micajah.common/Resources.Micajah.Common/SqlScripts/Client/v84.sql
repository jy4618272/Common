BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_Mc_Group_OrganizationId] ON [dbo].[Mc_Group] ([OrganizationId])

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
   CREATE INDEX [IX_Mc_User_Email] ON [dbo].[Mc_User] ([Email])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetAnotherAdministrator]
(
	@UserId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1 u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	INNER JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId <> uo.UserId) AND (u.Deleted = 0) AND (uo.OrganizationId = @OrganizationId) AND (uo.OrganizationAdministrator = 1);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetUser]
(
	@UserId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, uo.OrganizationId, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	LEFT JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId = uo.UserId) AND (uo.OrganizationId = @OrganizationId)
	WHERE (u.UserId = @UserId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetUserByEmail]
(
	@Email nvarchar(255),
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, uo.OrganizationId, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	LEFT JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId = uo.UserId) AND (uo.OrganizationId = @OrganizationId)
	WHERE (u.Email = @Email);
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
	SET NOCOUNT ON;

	SELECT u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, uo.OrganizationId, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	INNER JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId = uo.UserId) AND (u.Deleted = 0) AND (uo.OrganizationId = @OrganizationId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetUsersByRoles]
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
	SET NOCOUNT ON;

	SELECT DISTINCT 
		u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, uo.OrganizationId, uo.OrganizationAdministrator, uo.Active
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
			OR ((@InstanceId IS NOT NULL) AND (ISNULL(ui.Active, 1) = @Active))
			OR ((@InstanceId IS NULL) AND (uo.Active = @Active))
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
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetUsersGroups]
(
	@UserId uniqueidentifier,
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
		ON	(ug.UserId = u.UserId) AND (u.UserId = @UserId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetUsersInstances]
(
	@UserId uniqueidentifier,
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
		ON	(ui.UserId = u.UserId) AND (u.UserId = @UserId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
