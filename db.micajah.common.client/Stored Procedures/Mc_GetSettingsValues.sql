CREATE PROCEDURE [dbo].[Mc_GetSettingsValues]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT SettingValueId, SettingId, [Value], OrganizationId, InstanceId, GroupId
	FROM dbo.Mc_SettingsValues 
	WHERE (OrganizationId IS NULL) OR (OrganizationId = @OrganizationId);
END
