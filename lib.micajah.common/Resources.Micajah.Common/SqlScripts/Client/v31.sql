BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetEntityFieldValues]
(
	@EntityFieldId uniqueidentifier,
	@LocalEntityId nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT v.[EntityFieldValueId], v.[EntityFieldId], v.[LocalEntityId], v.[Value]
	FROM [dbo].[Mc_EntityFieldsValues] AS v
	INNER JOIN dbo.Mc_EntityField AS f
		ON	v.EntityFieldId = f.EntityFieldId
	WHERE (f.EntityFieldId = @EntityFieldId) AND (v.LocalEntityId = @LocalEntityId) AND (f.Active = 1);
END')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION