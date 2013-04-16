CREATE PROCEDURE [dbo].[Mc_DeleteViewState]
(
    @Now datetime
)
AS
BEGIN
    SET NOCOUNT OFF;
    
	DELETE dbo.Mc_ViewState
	WHERE ExpirationTime < @Now;
END
