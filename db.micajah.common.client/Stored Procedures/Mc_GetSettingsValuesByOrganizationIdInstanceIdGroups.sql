CREATE PROCEDURE [dbo].[Mc_GetSettingsValuesByOrganizationIdInstanceIdGroups]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Groups varchar(1024)
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
	WHERE (OrganizationId = @OrganizationId) AND (InstanceId = @InstanceId) AND (GroupId IS NULL)
	UNION ALL
	SELECT SettingValueId, SettingId, [Value], OrganizationId, InstanceId, GroupId
	FROM dbo.Mc_SettingsValues 
	WHERE (OrganizationId = @OrganizationId) AND (InstanceId = @InstanceId) AND (',' + @Groups + ',' LIKE '%,' + CAST(GroupId as varchar(50)) + ',%');
END