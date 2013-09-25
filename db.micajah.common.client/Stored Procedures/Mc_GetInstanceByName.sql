CREATE PROCEDURE [dbo].[Mc_GetInstanceByName]
(
	@Name nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, 
		ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat, [DateFormat], BillingPlan, CreditCardStatus
	FROM dbo.Mc_Instance 
	WHERE ([Name] = @Name) AND (Deleted = 0);
END
	