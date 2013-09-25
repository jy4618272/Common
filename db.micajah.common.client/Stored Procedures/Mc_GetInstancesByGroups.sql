CREATE PROCEDURE [dbo].[Mc_GetInstancesByGroups]
(
	@OrganizationId uniqueidentifier,
	@Groups varchar(1024)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT DISTINCT
		d.InstanceId, d.PseudoId, d.OrganizationId, d.[Name], d.[Description], d.EnableSignUpUser, 
		d.ExternalId, d.WorkingDays, d.Active, d.CanceledTime, d.Trial, d.Beta, d.Deleted, d.CreatedTime, d.TimeZoneId, d.TimeFormat, d.[DateFormat], d.BillingPlan, d.CreditCardStatus
	FROM dbo.Mc_GroupsInstancesRoles AS gdr
	INNER JOIN dbo.Mc_Instance AS d
		ON	(gdr.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.OrganizationId = @OrganizationId)
	WHERE (',' + @Groups + ',' LIKE '%,' + CAST(gdr.GroupId as varchar(50)) + ',%');
END
	