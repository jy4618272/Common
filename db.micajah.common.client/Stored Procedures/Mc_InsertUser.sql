CREATE PROCEDURE [dbo].[Mc_InsertUser]
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
