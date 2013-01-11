CREATE PROCEDURE [dbo].[Mc_InsertEntityNode]
(
	@EntityNodeId uniqueidentifier,
	@ParentEntityNodeId uniqueidentifier,
	@Name nvarchar(255),
	@OrderNumber int,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@EntityId uniqueidentifier,
	@EntityNodeTypeId uniqueidentifier,
	@SubEntityId uniqueidentifier,
	@SubEntityLocalId nvarchar(255),
	@FullPath nvarchar(1024),
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_EntityNode] (EntityNodeId, ParentEntityNodeId, [Name], OrderNumber, OrganizationId, InstanceId, EntityId, EntityNodeTypeId, SubEntityId, SubEntityLocalId, FullPath, Deleted)
	VALUES (@EntityNodeId, @ParentEntityNodeId, @Name, @OrderNumber, @OrganizationId, @InstanceId, @EntityId, @EntityNodeTypeId, @SubEntityId, @SubEntityLocalId, @FullPath, @Deleted);
	
	SELECT EntityNodeId, ParentEntityNodeId, [Name], OrderNumber, OrganizationId, InstanceId, EntityId, EntityNodeTypeId, SubEntityId, SubEntityLocalId, FullPath, Deleted
	FROM [dbo].[Mc_EntityNode]
	WHERE (EntityNodeId = @EntityNodeId);
END