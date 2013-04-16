CREATE PROCEDURE [dbo].[Mc_InsertEntityNodeType]
(
	@EntityNodeTypeId uniqueidentifier,
	@EntityId uniqueidentifier,
	@Name nvarchar(255),
	@OrderNumber int,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_EntityNodeType] (EntityNodeTypeId, EntityId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted)
	VALUES (@EntityNodeTypeId, @EntityId, @Name, @OrderNumber, @OrganizationId, @InstanceId, @Deleted);
	
	SELECT EntityNodeTypeId, EntityId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted
	FROM [dbo].[Mc_EntityNodeType]
	WHERE (EntityNodeTypeId = @EntityNodeTypeId);
END