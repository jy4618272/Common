CREATE PROCEDURE [dbo].[Mc_GetInstancesByUserIdRoleId]
(
	@OrganizationId uniqueidentifier,
	@UserId uniqueidentifier,
	@RoleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Instances TABLE (InstanceId uniqueidentifier);

	INSERT INTO @Instances
	SELECT DISTINCT d.InstanceId
	FROM dbo.Mc_GroupsInstancesRoles AS gdr
	INNER JOIN dbo.Mc_UsersGroups AS ug
		ON gdr.GroupId = ug.GroupId AND ug.UserId = @UserId
	INNER JOIN dbo.Mc_Instance AS d
		ON	(gdr.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.Active = 1) AND (d.OrganizationId = @OrganizationId)
	LEFT JOIN dbo.Mc_UsersInstances AS ui
		ON d.InstanceId = ui.InstanceId AND ui.UserId = @UserId 
	WHERE ISNULL(ui.Active, 1) = 1;

	IF NOT EXISTS(SELECT TOP 1 InstanceId FROM @Instances)
		INSERT INTO @Instances
		SELECT DISTINCT d.InstanceId
		FROM dbo.Mc_GroupsInstancesRoles AS gdr
		INNER JOIN dbo.Mc_Instance AS d
			ON	(gdr.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.Active = 1) AND (d.OrganizationId = @OrganizationId) AND (gdr.RoleId = @RoleId)
		LEFT JOIN dbo.Mc_UsersInstances AS ui
			ON d.InstanceId = ui.InstanceId AND ui.UserId = @UserId 
		WHERE ISNULL(ui.Active, 1) = 1;

	SET NOCOUNT OFF;

	SELECT d.InstanceId, d.PseudoId, d.OrganizationId, d.[Name], d.[Description], d.EnableSignUpUser, 
		d.ExternalId, d.WorkingDays, d.Active, d.CanceledTime, d.Trial, d.Beta, d.Deleted, d.CreatedTime, d.TimeZoneId, d.TimeFormat, d.[DateFormat], d.BillingPlan, d.CreditCardStatus
	FROM dbo.Mc_Instance AS d
	INNER JOIN @Instances AS i
		ON d.InstanceId = i.InstanceId
	ORDER BY d.[Name];
END
	