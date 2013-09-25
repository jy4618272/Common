CREATE PROCEDURE [dbo].[Mc_GetInstance]
(
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, 
		ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat, [DateFormat], BillingPlan, CreditCardStatus
	FROM dbo.Mc_Instance 
	WHERE (InstanceId = @InstanceId) AND (Deleted = 0);
END
	