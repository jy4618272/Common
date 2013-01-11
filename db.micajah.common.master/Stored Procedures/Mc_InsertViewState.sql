CREATE PROCEDURE [dbo].[Mc_InsertViewState]
(
	@ViewStateId uniqueidentifier,
    @ViewState varbinary(max),
    @ExpirationTime datetime
)
AS
BEGIN
    SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_ViewState (ViewStateId, ViewState, ExpirationTime) 
	VALUES (@ViewStateId, @ViewState, @ExpirationTime);
END
