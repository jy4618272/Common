CREATE PROCEDURE [dbo].[Mc_InsertInstance]
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

		INSERT INTO dbo.Mc_Instance (InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat, [DateFormat], BillingPlan, CreditCardStatus) 
		VALUES (@InstanceId, @PseudoId, @OrganizationId, @Name, @Description, @EnableSignUpUser, @ExternalId, @WorkingDays, @Active, @CanceledTime, @Trial, @Beta, @Deleted, GETUTCDATE(), @TimeZoneId, @TimeFormat, @DateFormat, @BillingPlan, @CreditCardStatus);
		
		SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat, [DateFormat], BillingPlan, CreditCardStatus
		FROM dbo.Mc_Instance 
		WHERE (InstanceId = @InstanceId);
	END
	