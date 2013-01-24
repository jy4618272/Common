CREATE PROCEDURE [dbo].[Mc_InsertInvitedLogin]
(
	@InvitedLoginId uniqueidentifier,
	@LoginName nvarchar(255),
	@OrganizationId uniqueidentifier,
	@GroupId varchar(2056),
	@InvitedBy uniqueidentifier,
	@CreatedTime datetime
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	INSERT INTO dbo.Mc_InvitedLogin	(InvitedLoginId, LoginName, OrganizationId, GroupId, InvitedBy, CreatedTime)
	VALUES (@InvitedLoginId, @LoginName, @OrganizationId, @GroupId, @InvitedBy, @CreatedTime)
	
	SELECT InvitedLoginId, LoginName, OrganizationId, GroupId, InvitedBy, CreatedTime
	FROM dbo.Mc_InvitedLogin
	WHERE (InvitedLoginId = @InvitedLoginId);
END