BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_MC_RecurringSchedule_UpdatedTime')
      ALTER TABLE [dbo].[Mc_RecurringSchedule] DROP CONSTRAINT [DF_MC_RecurringSchedule_UpdatedTime]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_Rule_CreatedDate')
      ALTER TABLE [dbo].[Mc_Rule] DROP CONSTRAINT [DF_Mc_Rule_CreatedDate]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertInstance]
(
	@InstanceId uniqueidentifier,
	@PseudoId varchar(6),
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@EnableSignUpUser bit,
	@ExternalId nvarchar(255),
	@UtcOffset decimal(4, 2),
	@DateFormat int,
	@WorkingDays char(7),
	@Active bit,
	@CanceledTime datetime,
	@Trial bit,
	@Beta bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Instance (InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, ExternalId, UtcOffset, [DateFormat], WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime) 
	VALUES (@InstanceId, @PseudoId, @OrganizationId, @Name, @Description, @EnableSignUpUser, @ExternalId, @UtcOffset, @DateFormat, @WorkingDays, @Active, @CanceledTime, @Trial, @Beta, @Deleted, GETUTCDATE());
	
	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, ExternalId, UtcOffset, [DateFormat], WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime 
	FROM dbo.Mc_Instance 
	WHERE (InstanceId = @InstanceId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
