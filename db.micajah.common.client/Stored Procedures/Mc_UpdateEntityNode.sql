CREATE PROCEDURE [dbo].[Mc_UpdateEntityNode]
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

		UPDATE [dbo].[Mc_EntityNode]
		SET ParentEntityNodeId = @ParentEntityNodeId, [Name] = @Name, OrderNumber = @OrderNumber, OrganizationId = @OrganizationId, InstanceId = @InstanceId, EntityId = @EntityId, EntityNodeTypeId = @EntityNodeTypeId, SubEntityId = @SubEntityId, FullPath = @FullPath, Deleted = @Deleted
		WHERE (EntityNodeId = @EntityNodeId);

		
		SELECT EntityNodeId, ParentEntityNodeId, [Name], OrderNumber, OrganizationId, InstanceId, EntityId, EntityNodeTypeId, SubEntityId, SubEntityLocalId, FullPath, Deleted
		FROM [dbo].[Mc_EntityNode]
		WHERE (EntityNodeId = @EntityNodeId);
	END