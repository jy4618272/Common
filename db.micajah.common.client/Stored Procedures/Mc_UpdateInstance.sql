CREATE PROCEDURE [dbo].[Mc_UpdateInstance]
	(
		@InstanceId uniqueidentifier,
		@PseudoId varchar(6),
		@OrganizationId uniqueidentifier,
		@Name nvarchar(255),
		@Description nvarchar(1024),
		@EnableSignUpUser bit,
		@ExternalId nvarchar(255),
		@WorkingDays char(7),
		@Active bit,
		@CanceledTime datetime,
		@Trial bit,
		@Beta bit,
		@Deleted bit,
		@TimeZoneId nvarchar(255),
		@TimeFormat int,
		@DateFormat int,
		@BillingPlan tinyint,
		@CreditCardStatus tinyint
	)
	AS
	BEGIN
		SET NOCOUNT OFF;

		UPDATE dbo.Mc_Instance 
		SET PseudoId  = @PseudoId, OrganizationId = @OrganizationId, [Name] = @Name, [Description] = @Description, EnableSignUpUser = @EnableSignUpUser, ExternalId = @ExternalId, WorkingDays = @WorkingDays, Active = @Active, CanceledTime = @CanceledTime, Trial = @Trial, Beta = @Beta, Deleted = @Deleted, TimeZoneId = @TimeZoneId, TimeFormat = @TimeFormat, [DateFormat] = @DateFormat, BillingPlan = @BillingPlan, CreditCardStatus=@CreditCardStatus
		WHERE (InstanceId = @InstanceId);
		
		SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat, [DateFormat], BillingPlan, CreditCardStatus
		FROM dbo.Mc_Instance 
		WHERE (InstanceId = @InstanceId);
	END
	