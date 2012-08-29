BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_User
		ALTER COLUMN Title nvarchar(30) NOT NULL

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
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION