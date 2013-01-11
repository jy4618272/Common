CREATE PROCEDURE [dbo].[Mc_InsertSettingValue]
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

	INSERT INTO dbo.Mc_SettingsValues (SettingValueId, SettingId, Value, OrganizationId, InstanceId, GroupId) 
	VALUES (@SettingValueId, @SettingId, @Value, @OrganizationId, @InstanceId, @GroupId);
	
	SELECT SettingValueId, SettingId, Value, OrganizationId, InstanceId, GroupId 
	FROM dbo.Mc_SettingsValues
	WHERE (SettingValueId = @SettingValueId);
END