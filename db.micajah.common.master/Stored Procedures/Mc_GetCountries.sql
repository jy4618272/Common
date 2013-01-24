CREATE PROCEDURE [dbo].[Mc_GetCountries]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT CountryId, [Name]
	FROM dbo.Mc_Country
	ORDER BY [Name];
END
