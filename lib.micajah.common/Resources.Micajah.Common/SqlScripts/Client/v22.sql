BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
CREATE TABLE [dbo].[Mc_Rule] (
   [RuleId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Mc_RuleEngine_RoleId] DEFAULT (newid()),
   [RuleEngineId] [uniqueidentifier] NOT NULL,
   [OrganizationId] [uniqueidentifier] NOT NULL,
   [InstanceId] [uniqueidentifier] NULL,
   [Name] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_RuleEngine_Name] DEFAULT (N''),
   [DisplayName] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_RuleEngine_DisplayName] DEFAULT (N''),
   [UsedQty] [int] NOT NULL CONSTRAINT [DF_Mc_Rule_UsedQty] DEFAULT ((0)),
   [LastUsedUser] [uniqueidentifier] NULL,
   [LastUsedDate] [datetime] NULL,
   [CreatedBy] [uniqueidentifier] NOT NULL,
   [CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_Mc_Rule_CreatedDate] DEFAULT (getdate()),
   [OrderNumber] [int] NOT NULL,
   [Active] [bit] NOT NULL CONSTRAINT [DF_Mc_Rule_Active] DEFAULT ((1))
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_Rule] ADD CONSTRAINT [PK_Mc_RuleEngine] PRIMARY KEY CLUSTERED ([RuleId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
CREATE TABLE [dbo].[Mc_RuleParameters] (
   [RuleParameterId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Mc_RuleEngineParameters_RuleParameterId] DEFAULT (newid()),
   [RuleId] [uniqueidentifier] NOT NULL,
   [EntityNodeTypeId] [uniqueidentifier] NULL,
   [IsInputParameter] [bit] NOT NULL,
   [IsEntity] [bit] NOT NULL,
   [FieldName] [nvarchar] (255) NOT NULL,
   [FullName] [nvarchar] (512) NOT NULL,
   [TypeName] [nvarchar] (255) NULL,
   [Term] [nvarchar] (50) NOT NULL CONSTRAINT [DF_Mc_RuleEngineParameters_Term] DEFAULT (N'='),
   [Value] [sql_variant] NOT NULL
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_RuleParameters] ADD CONSTRAINT [PK_Mc_RuleEngineParameters] PRIMARY KEY CLUSTERED ([RuleParameterId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_DeleteRule]
(
	@RuleId uniqueidentifier
)
AS
BEGIN

	SET NOCOUNT ON

	DELETE FROM [Mc_Rule]
	WHERE [RuleId] = @RuleId
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_DeleteRuleParameter]
(
	@RuleParameterId uniqueidentifier
)
AS
BEGIN

	SET NOCOUNT ON

	DELETE
	FROM [Mc_RuleParameters]
	WHERE
		[RuleParameterId] = @RuleParameterId
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_GetAllRuleParameters]
(
	@RuleId uniqueidentifier
)
AS
BEGIN

	SET NOCOUNT ON

	SELECT
		[RuleParameterId],
		[RuleId],
		[EntityNodeTypeId],
		[IsInputParameter],
		[IsEntity],
		[FieldName],
		[FullName],
		[TypeName],
		[Term],
		[Value]
	FROM [Mc_RuleParameters]
	WHERE [RuleId] = @RuleId
	ORDER BY [FieldName] ASC;

END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_GetAllRules]
	@RuleEngineId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier=null
AS
BEGIN

	SET NOCOUNT ON

	SELECT [RuleId]
	  ,[RuleEngineId]
	  ,[OrganizationId]
	  ,[InstanceId]
	  ,[Name]
	  ,[DisplayName]
	  ,[UsedQty]
	  ,[LastUsedUser]
	  ,[LastUsedDate]
	  ,[CreatedBy]
	  ,[CreatedDate]
	  ,[OrderNumber]
	  ,[Active]
	FROM [Mc_Rule]
	WHERE [RuleEngineId] = @RuleEngineId
	AND [OrganizationId] = @OrganizationId
	AND (@InstanceId is null 
			or [InstanceId] is null 
			or [InstanceId] = @InstanceId)
	ORDER BY [OrderNumber] ASC;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_GetRule]
(
	@RuleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[RuleId]
	  ,[RuleEngineId]
	  ,[OrganizationId]
	  ,[InstanceId]
	  ,[Name]
	  ,[DisplayName]
	  ,[UsedQty]
	  ,[LastUsedUser]
	  ,[LastUsedDate]
	  ,[CreatedBy]
	  ,[CreatedDate]
	  ,[OrderNumber]
	  ,[Active]
	FROM [Mc_Rule]
	WHERE
		([RuleId] = @RuleId)
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_GetRuleByName]
(
	@Name nvarchar(255),
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[RuleId]
	  ,[RuleEngineId]
	  ,[OrganizationId]
	  ,[InstanceId]
	  ,[Name]
	  ,[DisplayName]
	  ,[UsedQty]
	  ,[LastUsedUser]
	  ,[LastUsedDate]
	  ,[CreatedBy]
	  ,[CreatedDate]
	  ,[OrderNumber]
	  ,[Active]
	FROM [Mc_Rule]
	WHERE [Name] = @Name
	AND [OrganizationId] = @OrganizationId;
	
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_GetRuleParameter]
(
	@RuleParameterId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[RuleParameterId],
		[RuleId],
		[EntityNodeTypeId],
		[IsInputParameter],
		[IsEntity],
		[FieldName],
		[FullName],
		[TypeName],
		[Term],
		[Value]
	FROM [Mc_RuleParameters]
	WHERE
		([RuleParameterId] = @RuleParameterId)

END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_InsertRule]
(
	@RuleId uniqueidentifier,
	@RuleEngineId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier = NULL,
	@Name nvarchar(255),
	@DisplayName nvarchar(255),
	@UsedQty int,
	@LastUsedUser uniqueidentifier,
	@LastUsedDate datetime,
	@CreatedBy uniqueidentifier,
	@CreatedDate datetime,
	@OrderNumber int,
	@Active bit
)
AS
BEGIN

	SET NOCOUNT ON

	IF @RuleId IS NULL
		 SET @RuleId = NEWID()

	INSERT INTO [Mc_Rule]
	(
		[RuleId]
	  ,[RuleEngineId]
	  ,[OrganizationId]
	  ,[InstanceId]
	  ,[Name]
	  ,[DisplayName]
	  ,[UsedQty]
	  ,[LastUsedUser]
	  ,[LastUsedDate]
	  ,[CreatedDate]
	  ,[CreatedBy]
	  ,[OrderNumber]
	  ,[Active]
	)
	VALUES
	(
		@RuleId,
		@RuleEngineId,
		@OrganizationId,
		@InstanceId,
		@Name,
		@DisplayName,
		@UsedQty,
		@LastUsedUser,
		@LastUsedDate,
		@CreatedDate,
		@CreatedBy,
		@OrderNumber,
		@Active
	);
	
	SELECT
		[RuleId]
	  ,[RuleEngineId]
	  ,[OrganizationId]
	  ,[InstanceId]
	  ,[Name]
	  ,[DisplayName]
	  ,[UsedQty]
	  ,[LastUsedUser]
	  ,[LastUsedDate]
	  ,[CreatedDate]
	  ,[CreatedBy]
	  ,[OrderNumber]
	  ,[Active]
	FROM [Mc_Rule]
	WHERE
		([RuleId] = @RuleId);

END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_InsertRuleParameter]
(
	@RuleParameterId uniqueidentifier,
	@RuleId uniqueidentifier,
	@EntityNodeTypeId uniqueidentifier = NULL,
	@IsInputParameter bit,
	@IsEntity bit,
	@FieldName nvarchar(255),
	@FullName nvarchar(512),
	@TypeName nvarchar(255) = NULL,
	@Term nvarchar(50),
	@Value sql_variant
)
AS
BEGIN

	SET NOCOUNT ON

	IF @RuleParameterId IS NULL
		 SET @RuleParameterId = NEWID()

	INSERT
	INTO [Mc_RuleParameters]
	(
		[RuleParameterId],
		[RuleId],
		[EntityNodeTypeId],
		[IsInputParameter],
		[IsEntity],
		[FieldName],
		[FullName],
		[TypeName],
		[Term],
		[Value]
	)
	VALUES
	(
		@RuleParameterId,
		@RuleId,
		@EntityNodeTypeId,
		@IsInputParameter,
		@IsEntity,
		@FieldName,
		@FullName,
		@TypeName,
		@Term,
		@Value
	)

	SELECT
		[RuleParameterId],
		[RuleId],
		[EntityNodeTypeId],
		[IsInputParameter],
		[IsEntity],
		[FieldName],
		[FullName],
		[TypeName],
		[Term],
		[Value]
	FROM [Mc_RuleParameters]
	WHERE
		([RuleParameterId] = @RuleParameterId)

END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_UpdateRule]
(
	@RuleId uniqueidentifier,
	@RuleEngineId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier = NULL,
	@Name nvarchar(255),
	@DisplayName nvarchar(255),
	@UsedQty int,
	@LastUsedUser uniqueidentifier,
	@LastUsedDate datetime,
	@CreatedBy uniqueidentifier,
	@CreatedDate datetime,
	@OrderNumber int,
	@Active bit
)
AS
BEGIN

	SET NOCOUNT ON

	UPDATE [Mc_Rule]
	SET
		[RuleEngineId] = @RuleEngineId,
		[OrganizationId] = @OrganizationId,
		[InstanceId] = @InstanceId,
		[Name] = @Name,
		[DisplayName] = @DisplayName,
		[UsedQty] = @UsedQty,
		[LastUsedUser] = @LastUsedUser,
		[LastUsedDate] = @LastUsedDate,
		[CreatedDate] = @CreatedDate,
		[CreatedBy] = @CreatedBy,
		[OrderNumber] = @OrderNumber,
		[Active] = @Active
	WHERE
		[RuleId] = @RuleId;

	SELECT
		[RuleId]
	  ,[RuleEngineId]
	  ,[OrganizationId]
	  ,[InstanceId]
	  ,[Name]
	  ,[DisplayName]
	  ,[UsedQty]
	  ,[LastUsedUser]
	  ,[LastUsedDate]
	  ,[CreatedDate]
	  ,[CreatedBy]
	  ,[OrderNumber]
	  ,[Active]
	FROM [Mc_Rule]
	WHERE
		([RuleId] = @RuleId);

END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE procedure [dbo].[Mc_UpdateRuleOrder]
(
	@RuleId uniqueidentifier,
	@OrderNumber int
)
as
begin

	set nocount on

	UPDATE [Mc_Rule]
	SET [OrderNumber] = @OrderNumber
	WHERE [RuleId] = @RuleId;
	
end')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('CREATE PROCEDURE [dbo].[Mc_UpdateRuleParameter]
(
	@RuleParameterId uniqueidentifier,
	@RuleId uniqueidentifier,
	@EntityNodeTypeId uniqueidentifier = NULL,
	@IsInputParameter bit,
	@IsEntity bit,
	@FieldName nvarchar(255),
	@FullName nvarchar(512),
	@TypeName nvarchar(255) = NULL,
	@Term nvarchar(50),
	@Value sql_variant
)
AS
BEGIN

	SET NOCOUNT ON

	UPDATE [Mc_RuleParameters]
	SET
		[RuleId] = @RuleId,
		[EntityNodeTypeId] = @EntityNodeTypeId,
		[IsInputParameter] = @IsInputParameter,
		[IsEntity] = @IsEntity,
		[FieldName] = @FieldName,
		[FullName] = @FullName,
		[TypeName] = @TypeName,
		[Term] = @Term,
		[Value] = @Value
	WHERE
		[RuleParameterId] = @RuleParameterId

	SELECT
		[RuleParameterId],
		[RuleId],
		[EntityNodeTypeId],
		[IsInputParameter],
		[IsEntity],
		[FieldName],
		[FullName],
		[TypeName],
		[Term],
		[Value]
	FROM [Mc_RuleParameters]
	WHERE
		([RuleParameterId] = @RuleParameterId)
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
exec('create PROCEDURE [dbo].[Mc_UpdateRuleUses]
(
	@RuleId uniqueidentifier,
	@LastUsedUser uniqueidentifier,
	@LastUsedDate datetime
)
AS
BEGIN

	SET NOCOUNT ON

	UPDATE [Mc_Rule]
	SET
		[UsedQty] = [UsedQty] + 1,
		[LastUsedUser] = @LastUsedUser,
		[LastUsedDate] = @LastUsedDate
	WHERE
		[RuleId] = @RuleId;

	SELECT
		[RuleId]
	  ,[OrganizationId]
	  ,[InstanceId]
	  ,[Name]
	  ,[DisplayName]
	  ,[UsedQty]
	  ,[LastUsedUser]
	  ,[LastUsedDate]
	  ,[CreatedDate]
	  ,[CreatedBy]
	  ,[Active]
	FROM [Mc_Rule]
	WHERE
		([RuleId] = @RuleId);

END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_RuleEngineParameters_Mc_RuleEngine')
      ALTER TABLE [dbo].[Mc_RuleParameters] DROP CONSTRAINT [FK_Mc_RuleEngineParameters_Mc_RuleEngine]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_RuleEngineParameters_Mc_RuleEngine')
      ALTER TABLE [dbo].[Mc_RuleParameters] ADD CONSTRAINT [FK_Mc_RuleEngineParameters_Mc_RuleEngine] FOREIGN KEY ([RuleId]) REFERENCES [dbo].[Mc_Rule] ([RuleId]) ON DELETE CASCADE

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
