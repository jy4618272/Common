CREATE PROCEDURE [dbo].[Mc_GetViewState]
(
    @ViewStateId uniqueidentifier
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ViewState
    FROM dbo.Mc_ViewState WITH (READUNCOMMITTED)
    WHERE (ViewStateId = @ViewStateId);
END
