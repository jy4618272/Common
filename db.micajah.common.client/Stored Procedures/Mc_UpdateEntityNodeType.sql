CREATE PROCEDURE [dbo].[Mc_UpdateEntityNodeType]
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

	UPDATE [dbo].[Mc_EntityNodeType]
	SET [Name] = @Name,  EntityId = @EntityId, OrderNumber = @OrderNumber, OrganizationId = @OrganizationId, InstanceId = @InstanceId, Deleted = @Deleted
	WHERE (EntityNodeTypeId = @EntityNodeTypeId);

	SELECT EntityNodeTypeId, EntityId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted
	FROM [dbo].[Mc_EntityNodeType]
	WHERE (EntityNodeTypeId = @EntityNodeTypeId);
END