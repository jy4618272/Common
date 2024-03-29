-- Run this script after updating the database to schema version #99 to convert the value of datetime fields to UTC format.

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUTCDateTime]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[GetUTCDateTime]
GO

CREATE FUNCTION [dbo].[GetUTCDateTime] 
( 
	@dt datetime   
) 
RETURNS DATETIME 
AS 
BEGIN 
	IF @dt IS NULL
		RETURN NULL

	-- Check if only date w\o time inserted like '20120621' so time is '0:00:00.00'
	IF DATEDIFF(ms, DATEDIFF(DAY, 0, @dt), @dt) = 0
		RETURN @dt

	-- Check if only time w\o date inserted '11:02:12' like so date is '1/1/1900'
	IF DATEDIFF(dd, 0, DATEDIFF(dd, 0, @dt)) = 0
		RETURN @dt
 if @dt>'20790605'
	return @dt
    
  DECLARE 
  @offset TINYINT = 5, @month INT = DATEPART(mm, @dt)
  
  IF (@month>4 AND @month<10)
  BEGIN
	SET @offset = @offset - 1
	SELECT @dt = DATEADD(hour, @offset, @dt)
	RETURN @dt
  END
  ELSE IF (@month<4 OR @month>10)
  BEGIN
	SELECT @dt = DATEADD(hour, @offset, @dt)
	RETURN @dt  
  END 

	DECLARE @sdt smalldatetime
		, @edt smalldatetime
		, @i tinyint = 1 

	-- Find first Sunday in April 
	WHILE @i < 7 
	BEGIN 
		SET @sdt = RTRIM(YEAR(@dt)) + '040' + RTRIM(@i) 
		IF DATEPART(WEEKDAY, @sdt) = 1 
			SET @i = 7 
		SET @i = @i + 1 
	END 

	-- Find last Sunday in October 
	SET @i = 31 
	WHILE @i > 24 
	BEGIN 
		SET @edt = RTRIM(YEAR(@dt)) + '10' + RTRIM(@i) 
		IF DATEPART(WEEKDAY, @edt) = 1 
			SET @i = 24 
		SET @i = @i - 1 
	END 

	-- Subtract hour from offset if within DST
	IF (@dt >= @sdt AND @dt < @edt) 
		SET @offset = @offset - 1 

	SELECT @dt = DATEADD(HOUR, @offset, @dt)

	RETURN @dt 
END
GO

UPDATE dbo.Mc_InvitedLogin 
SET CreatedTime = dbo.GetUTCDateTime(CreatedTime)
GO

UPDATE dbo.Mc_Login 
SET ProfileUpdated = dbo.GetUTCDateTime(ProfileUpdated)
GO

UPDATE dbo.Mc_Organization 
SET ExpirationTime = dbo.GetUTCDateTime(ExpirationTime)
	, CanceledTime = dbo.GetUTCDateTime(CanceledTime)
	, CreatedTime = dbo.GetUTCDateTime(CreatedTime)
GO

UPDATE dbo.Mc_OrganizationsLdapGroups 
SET CreatedTime = dbo.GetUTCDateTime(CreatedTime)
GO

UPDATE dbo.Mc_ResetPasswordRequest 
SET CreatedTime = dbo.GetUTCDateTime(CreatedTime)
GO

UPDATE dbo.Mc_Resource 
SET CreatedTime = dbo.GetUTCDateTime(CreatedTime)
GO

UPDATE dbo.Mc_ViewState 
SET ExpirationTime = dbo.GetUTCDateTime(ExpirationTime)
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUTCDateTime]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[GetUTCDateTime]
GO
