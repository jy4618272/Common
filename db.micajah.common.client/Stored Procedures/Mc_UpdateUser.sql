CREATE PROCEDURE [dbo].[Mc_UpdateUser]
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
