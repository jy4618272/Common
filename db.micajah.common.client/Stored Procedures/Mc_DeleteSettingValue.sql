
CREATE PROCEDURE [dbo].[Mc_DeleteSettingValue]
(
	@SettingValueId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_SettingsValues 
	WHERE (SettingValueId = @SettingValueId);
END

