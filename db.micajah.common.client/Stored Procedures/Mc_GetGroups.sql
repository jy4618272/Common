CREATE PROCEDURE [dbo].[Mc_GetGroups]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT GroupId, OrganizationId, [Name], [Description], BuiltIn, Deleted 
	FROM dbo.Mc_Group 
	WHERE (Deleted = 0) AND (OrganizationId = @OrganizationId);
END
