CREATE PROCEDURE [dbo].[Mc_GetSettingsValues]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@GroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT SettingValueId, SettingId, [Value], OrganizationId, InstanceId, GroupId
	FROM dbo.Mc_SettingsValues 
	WHERE (OrganizationId = @OrganizationId)
		AND ((@InstanceId IS NULL AND InstanceId IS NULL AND GroupId IS NULL) OR ((InstanceId = @InstanceId) AND ((@GroupId IS NULL AND GroupId IS NULL) OR (GroupId = @GroupId))));
END