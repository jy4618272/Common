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
