BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNodesUsers_Mc_User')
      ALTER TABLE [dbo].[Mc_EntityNodesUsers] DROP CONSTRAINT [FK_Mc_EntityNodesUsers_Mc_User]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_OrganizationsUsers_Mc_User')
      ALTER TABLE [dbo].[Mc_OrganizationsUsers] DROP CONSTRAINT [FK_Mc_OrganizationsUsers_Mc_User]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_UsersGroups_Mc_User')
      ALTER TABLE [dbo].[Mc_UsersGroups] DROP CONSTRAINT [FK_Mc_UsersGroups_Mc_User]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_User_FirstName')
      ALTER TABLE [dbo].[Mc_User] DROP CONSTRAINT [DF_Mc_User_FirstName]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_User_LastName')
      ALTER TABLE [dbo].[Mc_User] DROP CONSTRAINT [DF_Mc_User_LastName]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_User_MiddleName')
      ALTER TABLE [dbo].[Mc_User] DROP CONSTRAINT [DF_Mc_User_MiddleName]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_User_Deleted')
      ALTER TABLE [dbo].[Mc_User] DROP CONSTRAINT [DF_Mc_User_Deleted]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE TABLE [dbo].[tmp_Mc_User] (
   [UserId] [uniqueidentifier] NOT NULL,
   [Email] [nvarchar] (255) NOT NULL,
   [FirstName] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_User_FirstName] DEFAULT (N''),
   [LastName] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_User_LastName] DEFAULT (N''),
   [MiddleName] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_User_MiddleName] DEFAULT (N''),
   [Phone] [nvarchar] (20) NOT NULL CONSTRAINT [DF_Mc_User_Phone] DEFAULT (''),
   [MobilePhone] [nvarchar] (20) NOT NULL CONSTRAINT [DF_Mc_User_MobilePhone] DEFAULT (''),
   [Fax] [nvarchar] (20) NOT NULL CONSTRAINT [DF_Mc_User_Fax] DEFAULT (''),
   [Title] [nvarchar] (20) NOT NULL CONSTRAINT [DF_Mc_User_Title] DEFAULT (''),
   [Department] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_User_Department] DEFAULT (''),
   [Street] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_User_Street] DEFAULT (''),
   [Street2] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_User_Street2] DEFAULT (''),
   [City] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_User_City] DEFAULT (''),
   [State] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_User_State] DEFAULT (''),
   [PostalCode] [nvarchar] (20) NOT NULL CONSTRAINT [DF_Mc_User_PostalCode] DEFAULT (''),
   [Country] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_User_Country] DEFAULT (''),
   [LastLoginDate] [datetime] NULL,
   [Deleted] [bit] NOT NULL CONSTRAINT [DF_Mc_User_Deleted] DEFAULT ((0))
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   INSERT INTO [dbo].[tmp_Mc_User] ([UserId], [Email], [FirstName], [LastName], [MiddleName], [LastLoginDate], [Deleted])
   SELECT [UserId], [Email], [FirstName], [LastName], [MiddleName], [LastLoginDate], [Deleted]
   FROM [dbo].[Mc_User]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP TABLE [dbo].[Mc_User]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC sp_rename N'[dbo].[tmp_Mc_User]', N'Mc_User'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_User] ADD CONSTRAINT [PK_Mc_User] PRIMARY KEY CLUSTERED ([UserId])

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

	SELECT DISTINCT u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, uo.OrganizationAdministrator
	FROM dbo.Mc_User AS u
	INNER JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId = uo.UserId) AND (u.Deleted = 0) AND (uo.OrganizationId = @OrganizationId);
END')

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
	@Title nvarchar(20),
	@Department nvarchar(255),
	@Street nvarchar(255),
	@Street2 nvarchar(255),
	@City nvarchar(255),
	@State nvarchar(255),
	@PostalCode nvarchar(20),
	@Country nvarchar(255),
	@LastLoginDate datetime,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	IF NOT EXISTS(SELECT 0 FROM dbo.Mc_User WHERE UserId = @UserId)
		INSERT INTO dbo.Mc_User (UserId, Email, FirstName, LastName, MiddleName, Phone, MobilePhone, Fax, Title, Department, Street, Street2, City, [State], PostalCode, Country, LastLoginDate, Deleted) 
		VALUES (@UserId, @Email, @FirstName, @LastName, @MiddleName, @Phone, @MobilePhone, @Fax, @Title, @Department, @Street, @Street2, @City, @State, @PostalCode, @Country, @LastLoginDate, @Deleted);
	
	SELECT UserId, Email, FirstName, LastName, MiddleName, Phone, MobilePhone, Fax, Title, Department, Street, Street2, City, [State], PostalCode, Country, LastLoginDate, Deleted 
	FROM dbo.Mc_User 
	WHERE (UserId = @UserId);
END')

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
	@Title nvarchar(20),
	@Department nvarchar(255),
	@Street nvarchar(255),
	@Street2 nvarchar(255),
	@City nvarchar(255),
	@State nvarchar(255),
	@PostalCode nvarchar(20),
	@Country nvarchar(255),
	@LastLoginDate datetime,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_User 
	SET UserId = @UserId, Email = @Email, FirstName = @FirstName, LastName = @LastName, MiddleName = @MiddleName
		, Phone = @Phone, MobilePhone = @MobilePhone, Fax = @Fax, Title = @Title, Department = @Department
		, Street = @Street, Street2 = @Street2, City = @City, [State] = @State, PostalCode = @PostalCode, Country = @Country
		, LastLoginDate = @LastLoginDate, Deleted = @Deleted 
	WHERE (UserId = @UserId);
	
	SELECT UserId, Email, FirstName, LastName, MiddleName, Phone, MobilePhone, Fax, Title, Department, Street, Street2, City, [State], PostalCode, Country, LastLoginDate, Deleted 
	FROM dbo.Mc_User 
	WHERE (UserId = @UserId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityNodesUsers_Mc_User')
      ALTER TABLE [dbo].[Mc_EntityNodesUsers] ADD CONSTRAINT [FK_Mc_EntityNodesUsers_Mc_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Mc_User] ([UserId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_OrganizationsUsers_Mc_User')
      ALTER TABLE [dbo].[Mc_OrganizationsUsers] ADD CONSTRAINT [FK_Mc_OrganizationsUsers_Mc_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Mc_User] ([UserId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_UsersGroups_Mc_User')
      ALTER TABLE [dbo].[Mc_UsersGroups] ADD CONSTRAINT [FK_Mc_UsersGroups_Mc_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Mc_User] ([UserId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION