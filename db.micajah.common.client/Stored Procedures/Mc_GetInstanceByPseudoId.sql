CREATE PROCEDURE [dbo].[Mc_GetInstanceByPseudoId]
(
	@PseudoId varchar(6)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, 
		ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat, [DateFormat], BillingPlan, CreditCardStatus
	FROM dbo.Mc_Instance 
	WHERE (PseudoId = @PseudoId) AND (Deleted = 0);
END
	