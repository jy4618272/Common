﻿CREATE PROCEDURE [dbo].[Mc_GetEntityFieldsValues]
(
	@EntityId uniqueidentifier,
	@LocalEntityId nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT v.[EntityFieldValueId], v.[EntityFieldId], v.[LocalEntityId], v.[Value]
	FROM [dbo].[Mc_EntityFieldsValues] AS v
	INNER JOIN dbo.Mc_EntityField AS f
		ON	v.EntityFieldId = f.EntityFieldId
	WHERE (f.EntityId = @EntityId) AND (v.LocalEntityId = @LocalEntityId) AND (f.Active = 1);
END