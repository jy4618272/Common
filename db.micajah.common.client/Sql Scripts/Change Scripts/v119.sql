BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP PROCEDURE [dbo].[Mc_InsertSettingValue];

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateSettingValue]
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

	IF EXISTS(SELECT 1 
		FROM dbo.Mc_SettingsValues 
		WHERE (SettingId = @SettingId) 
			AND (OrganizationId = @OrganizationId) 
			AND ((@InstanceId IS NULL AND InstanceId IS NULL) OR (InstanceId = @InstanceId)) 
			AND ((@GroupId IS NULL AND GroupId IS NULL) OR (GroupId = @GroupId)) 
	)
		UPDATE dbo.Mc_SettingsValues 
		SET SettingId = @SettingId, Value = @Value, OrganizationId = @OrganizationId, InstanceId = @InstanceId, GroupId = @GroupId 
		WHERE (SettingId = @SettingId) 
			AND (OrganizationId = @OrganizationId)
			AND ((@InstanceId IS NULL AND InstanceId IS NULL) OR (InstanceId = @InstanceId)) 
			AND ((@GroupId IS NULL AND GroupId IS NULL) OR (GroupId = @GroupId));
	ELSE
		INSERT INTO dbo.Mc_SettingsValues (SettingValueId, SettingId, Value, OrganizationId, InstanceId, GroupId) 
		VALUES (@SettingValueId, @SettingId, @Value, @OrganizationId, @InstanceId, @GroupId);
	
	SELECT SettingValueId, SettingId, Value, OrganizationId, InstanceId, GroupId 
	FROM dbo.Mc_SettingsValues 
	WHERE (SettingId = @SettingId) 
		AND (OrganizationId = @OrganizationId)
		AND ((@InstanceId IS NULL AND InstanceId IS NULL) OR (InstanceId = @InstanceId)) 
		AND ((@GroupId IS NULL AND GroupId IS NULL) OR (GroupId = @GroupId))
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION