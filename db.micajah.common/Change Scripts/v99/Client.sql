UPDATE dbo.Mc_Instance 
SET CanceledTime = dbo.GetUTCDateTime(CanceledTime)
	, CreatedTime = dbo.GetUTCDateTime(CreatedTime)
GO

UPDATE dbo.Mc_Message 
SET CreatedTime = dbo.GetUTCDateTime(CreatedTime)
GO

UPDATE dbo.Mc_RecurringSchedule 
SET StartDate = dbo.GetUTCDateTime(StartDate)
	, EndDate = dbo.GetUTCDateTime(EndDate)
	, UpdatedTime = dbo.GetUTCDateTime(UpdatedTime)
GO

UPDATE dbo.Mc_Rule 
SET LastUsedDate = dbo.GetUTCDateTime(LastUsedDate)
	, CreatedDate = dbo.GetUTCDateTime(CreatedDate)
GO

UPDATE dbo.Mc_User 
SET LastLoginDate = dbo.GetUTCDateTime(LastLoginDate)
GO
