BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

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
   ALTER TABLE [dbo].[Mc_Instance]
      ALTER COLUMN [TimeFormat] [int] NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Instance]
      ALTER COLUMN [TimeZoneId] [nvarchar] (100) NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_User]
      ADD [TimeFormat] [int] NULL

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_User]
      ADD [TimeZoneId] [nvarchar] (100) NULL

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
		, u.LastLoginDate, u.Deleted, u.TimeZoneId, u.TimeFormat, uo.OrganizationId, uo.OrganizationAdministrator, uo.Active
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
		, u.LastLoginDate, u.Deleted, u.TimeZoneId, u.TimeFormat, uo.OrganizationId, uo.OrganizationAdministrator, uo.Active
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
		, u.LastLoginDate, u.Deleted, u.TimeZoneId, u.TimeFormat, uo.OrganizationId, uo.OrganizationAdministrator, uo.Active
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
		, u.LastLoginDate, u.Deleted, u.TimeZoneId, u.TimeFormat, uo.OrganizationId, uo.OrganizationAdministrator
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
	@TimeFormat int
)
AS
BEGIN
	SET NOCOUNT OFF;

	IF NOT EXISTS(SELECT 0 FROM dbo.Mc_User WHERE UserId = @UserId)
		INSERT INTO dbo.Mc_User (UserId, Email, FirstName, LastName, MiddleName, Phone, MobilePhone, Fax, Title, Department, Street, Street2, City, [State], PostalCode, Country, LastLoginDate, Deleted, TimeZoneId, TimeFormat) 
		VALUES (@UserId, @Email, @FirstName, @LastName, @MiddleName, @Phone, @MobilePhone, @Fax, @Title, @Department, @Street, @Street2, @City, @State, @PostalCode, @Country, @LastLoginDate, @Deleted, @TimeZoneId, @TimeFormat);
	
	SELECT UserId, Email, FirstName, LastName, MiddleName, Phone, MobilePhone, Fax, Title, Department, Street, Street2, City, [State], PostalCode, Country, LastLoginDate, Deleted, TimeZoneId, TimeFormat
	FROM dbo.Mc_User 
	WHERE (UserId = @UserId);
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
	@TimeFormat int
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
		, TimeZoneId = @TimeZoneId, TimeFormat = @TimeFormat
	WHERE (UserId = @UserId);
	
	SELECT UserId, Email, FirstName, LastName, MiddleName, Phone, MobilePhone, Fax, Title, Department, Street, Street2, City, [State], PostalCode, Country, LastLoginDate, Deleted, TimeZoneId, TimeFormat
	FROM dbo.Mc_User 
	WHERE (UserId = @UserId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Instance] 
	SET TimeZoneId = NULL
	WHERE LEN(ISNULL(TimeZoneId, '')) = 0

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
