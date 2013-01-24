CREATE PROCEDURE [dbo].[Mc_DeleteResource]
(
	@ResourceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	DECLARE @Date datetime;
	SET @Date = DATEADD(DAY, -1, GETUTCDATE());

	DELETE FROM dbo.Mc_Resource
	WHERE (ResourceId = @ResourceId) 
		OR (ParentResourceId = @ResourceId)
		OR ((Temporary = 1) AND (CreatedTime <= @Date));
END
