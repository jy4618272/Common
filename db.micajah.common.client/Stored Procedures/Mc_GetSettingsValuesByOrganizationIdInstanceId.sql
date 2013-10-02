CREATE PROCEDURE [dbo].[Mc_GetSettingsValuesByOrganizationIdInstanceId]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT SettingValueId, SettingId, [Value], OrganizationId, InstanceId, GroupId
	FROM dbo.Mc_SettingsValues 
	WHERE (OrganizationId = @OrganizationId) AND (InstanceId IS NULL) AND (GroupId IS NULL)
	UNION ALL
	SELECT SettingValueId, SettingId, [Value], OrganizationId, InstanceId, GroupId
	FROM dbo.Mc_SettingsValues 
	WHERE (OrganizationId = @OrganizationId) AND (InstanceId = @InstanceId) AND (GroupId IS NULL);
END