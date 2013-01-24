CREATE PROCEDURE [dbo].[Mc_InsertGroup]
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

	INSERT INTO dbo.Mc_Group (GroupId, OrganizationId, [Name], [Description], BuiltIn, Deleted) 
	VALUES (@GroupId, @OrganizationId, @Name, @Description, @BuiltIn, @Deleted);
	
	SELECT GroupId, OrganizationId, [Name], [Description], BuiltIn, Deleted 
	FROM dbo.Mc_Group 
	WHERE (GroupId = @GroupId);
END