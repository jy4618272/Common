﻿CREATE PROCEDURE [dbo].[Mc_DeleteViewState]
(
    @Now datetime
)
AS
BEGIN
    SET NOCOUNT ON;
 
	DECLARE @ExpiredViewState TABLE 
	(
		ViewStateId uniqueidentifier NOT NULL
		PRIMARY KEY
	)
 
	INSERT @ExpiredViewState (ViewStateId)
	SELECT ViewStateId
	FROM dbo.Mc_ViewState
	WHERE ExpirationTime < @Now
 
	DECLARE ViewStateCursor CURSOR LOCAL FORWARD_ONLY READ_ONLY
	FOR SELECT ViewStateId FROM @ExpiredViewState ORDER BY CHECKSUM(NEWID())
 
	DECLARE @ViewStateId uniqueidentifier
 
	OPEN ViewStateCursor

	FETCH NEXT FROM ViewStateCursor INTO @ViewStateId
	WHILE @@FETCH_STATUS = 0 BEGIN
		DELETE FROM dbo.Mc_ViewState
		WHERE ViewStateId = @ViewStateId

		FETCH NEXT FROM ViewStateCursor INTO @ViewStateId
	END

	CLOSE ViewStateCursor
	DEALLOCATE ViewStateCursor
END
