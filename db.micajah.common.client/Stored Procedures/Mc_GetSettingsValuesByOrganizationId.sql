CREATE PROCEDURE [dbo].[Mc_GetSettingsValuesByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT SettingValueId, SettingId, [Value], OrganizationId, InstanceId, GroupId
	FROM dbo.Mc_SettingsValues 
	WHERE (OrganizationId = @OrganizationId) AND (InstanceId IS NULL) AND (GroupId IS NULL);
END