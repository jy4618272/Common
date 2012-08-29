BEGIN TRANSACTION

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
   
IF @@TRANCOUNT = 1   
exec('ALTER PROCEDURE [dbo].[Mc_InsertEntityNode]
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
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
