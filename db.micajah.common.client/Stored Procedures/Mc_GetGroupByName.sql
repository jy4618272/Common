CREATE PROCEDURE [dbo].[Mc_GetGroupByName]
(
	@Name nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT GroupId, OrganizationId, [Name], [Description], BuiltIn, Deleted 
	FROM dbo.Mc_Group 
	WHERE (Name = @Name) AND (Deleted = 0);
END
