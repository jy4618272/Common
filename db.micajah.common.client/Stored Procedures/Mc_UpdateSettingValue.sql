CREATE PROCEDURE [dbo].[Mc_UpdateSettingValue]
(
	@SettingValueId uniqueidentifier,
	@SettingId uniqueidentifier,
	@Value nvarchar(max),
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@GroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_SettingsValues 
	SET SettingId = @SettingId, Value = @Value, OrganizationId = @OrganizationId, InstanceId = @InstanceId, GroupId = @GroupId 
	WHERE (SettingValueId = @SettingValueId);
	
	SELECT SettingValueId, SettingId, Value, OrganizationId, InstanceId, GroupId 
	FROM dbo.Mc_SettingsValues 
	WHERE (SettingValueId = @SettingValueId);
END