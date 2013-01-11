
CREATE PROCEDURE [dbo].[Mc_InsertCountry]
(
	@CountryId uniqueidentifier,
	@Name nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Country(CountryId, [Name])
	VALUES (@CountryId, @Name)

	SELECT CountryId, [Name]
	FROM dbo.Mc_Country
	WHERE (CountryId = @CountryId);
END

