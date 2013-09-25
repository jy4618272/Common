CREATE PROCEDURE [dbo].[Mc_GetGroup]
(
	@GroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT GroupId, OrganizationId, [Name], [Description], BuiltIn, Deleted 
	FROM dbo.Mc_Group 
	WHERE (GroupId = @GroupId) AND (Deleted = 0);
END
