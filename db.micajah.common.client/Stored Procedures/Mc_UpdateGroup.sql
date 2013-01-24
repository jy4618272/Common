CREATE PROCEDURE [dbo].[Mc_UpdateGroup]
(
	@GroupId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@BuiltIn bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Group 
	SET OrganizationId = @OrganizationId, [Name] = @Name, [Description] = @Description, BuiltIn = @BuiltIn, Deleted = @Deleted 
	WHERE (GroupId = @GroupId);
	
	SELECT GroupId, OrganizationId, [Name], [Description], BuiltIn, Deleted 
	FROM dbo.Mc_Group 
	WHERE (GroupId = @GroupId);
END