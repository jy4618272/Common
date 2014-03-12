BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityFieldListsValues_Mc_EntityField')
      ALTER TABLE [dbo].[Mc_EntityFieldListsValues] DROP CONSTRAINT [FK_Mc_EntityFieldListsValues_Mc_EntityField]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityFieldsValues_Mc_EntityField')
      ALTER TABLE [dbo].[Mc_EntityFieldsValues] DROP CONSTRAINT [FK_Mc_EntityFieldsValues_Mc_EntityField]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityField_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityField] DROP CONSTRAINT [FK_Mc_EntityField_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_EntityField_AllowDBNull')
      ALTER TABLE [dbo].[Mc_EntityField] DROP CONSTRAINT [DF_Mc_EntityField_AllowDBNull]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_EntityField_ReadOnly')
      ALTER TABLE [dbo].[Mc_EntityField] DROP CONSTRAINT [DF_Mc_EntityField_ReadOnly]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_EntityField_Unique')
      ALTER TABLE [dbo].[Mc_EntityField] DROP CONSTRAINT [DF_Mc_EntityField_Unique]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_EntityField_MaxLength')
      ALTER TABLE [dbo].[Mc_EntityField] DROP CONSTRAINT [DF_Mc_EntityField_MaxLength]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_EntityField_OrderNumber')
      ALTER TABLE [dbo].[Mc_EntityField] DROP CONSTRAINT [DF_Mc_EntityField_OrderNumber]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_EntityField_Active')
      ALTER TABLE [dbo].[Mc_EntityField] DROP CONSTRAINT [DF_Mc_EntityField_Active]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE TABLE [dbo].[tmp_Mc_EntityField] (
   [EntityFieldId] [uniqueidentifier] NOT NULL,
   [EntityFieldTypeId] [int] NOT NULL,
   [Name] [nvarchar] (255) NOT NULL,
   [Description] [nvarchar] (255) NOT NULL,
   [DataTypeId] [int] NOT NULL,
   [DefaultValue] [nvarchar] (512) NULL,
   [AllowDBNull] [bit] NOT NULL CONSTRAINT [DF_Mc_EntityField_AllowDBNull] DEFAULT ((1)),
   [Unique] [bit] NOT NULL CONSTRAINT [DF_Mc_EntityField_Unique] DEFAULT ((0)),
   [MaxLength] [int] NOT NULL CONSTRAINT [DF_Mc_EntityField_MaxLength] DEFAULT ((0)),
   [MinValue] [nvarchar] (512) NULL,
   [MaxValue] [nvarchar] (512) NULL,
   [DecimalDigits] [int] NOT NULL CONSTRAINT [DF_Mc_EntityField_DecimalDigits] DEFAULT ((0)),
   [OrderNumber] [int] NOT NULL CONSTRAINT [DF_Mc_EntityField_OrderNumber] DEFAULT ((0)),
   [EntityId] [uniqueidentifier] NOT NULL,
   [OrganizationId] [uniqueidentifier] NOT NULL,
   [InstanceId] [uniqueidentifier] NULL,
   [Active] [bit] NOT NULL CONSTRAINT [DF_Mc_EntityField_Active] DEFAULT ((1))
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   INSERT INTO [dbo].[tmp_Mc_EntityField] ([EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataTypeId], [DefaultValue], [AllowDBNull], [Unique], [MaxLength], [MinValue], [MaxValue], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active])
   SELECT [EntityFieldId], [EntityFieldTypeId], [Name], [Description], 1, [DefaultValue], [AllowDBNull], [Unique], [MaxLength], [MinValue], [MaxValue], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active]
   FROM [dbo].[Mc_EntityField]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP TABLE [dbo].[Mc_EntityField]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC sp_rename N'[dbo].[tmp_Mc_EntityField]', N'Mc_EntityField'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_EntityField] ADD CONSTRAINT [PK_Mc_EntityField] PRIMARY KEY CLUSTERED ([EntityFieldId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityFieldListsValues_Mc_EntityField')
      ALTER TABLE [dbo].[Mc_EntityFieldListsValues] DROP CONSTRAINT [FK_Mc_EntityFieldListsValues_Mc_EntityField]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'DF_Mc_EntityFieldListsValues_Active')
      ALTER TABLE [dbo].[Mc_EntityFieldListsValues] DROP CONSTRAINT [DF_Mc_EntityFieldListsValues_Active]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   CREATE TABLE [dbo].[tmp_Mc_EntityFieldListsValues] (
   [EntityFieldListValueId] [uniqueidentifier] NOT NULL,
   [EntityFieldId] [uniqueidentifier] NOT NULL,
   [Name] [nvarchar] (255) NOT NULL,
   [Value] [nvarchar] (512) NOT NULL,
   [Default] [bit] NOT NULL CONSTRAINT [DF_Mc_EntityFieldListsValues_Default] DEFAULT ((0)),
   [Active] [bit] NOT NULL CONSTRAINT [DF_Mc_EntityFieldListsValues_Active] DEFAULT ((1))
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   INSERT INTO [dbo].[tmp_Mc_EntityFieldListsValues] ([EntityFieldListValueId], [EntityFieldId], [Name], [Value], [Active])
   SELECT [EntityFieldListValueId], [EntityFieldId], [Name], [Value], [Active]
   FROM [dbo].[Mc_EntityFieldListsValues]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   DROP TABLE [dbo].[Mc_EntityFieldListsValues]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC sp_rename N'[dbo].[tmp_Mc_EntityFieldListsValues]', N'Mc_EntityFieldListsValues'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_EntityFieldListsValues] ADD CONSTRAINT [PK_Mc_EntityFieldListsValues] PRIMARY KEY CLUSTERED ([EntityFieldListValueId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEntityField]
(
	@EntityFieldId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataTypeId], [DefaultValue], [AllowDBNull], [Unique], [MaxLength], [MinValue], [MaxValue], [DecimalDigits], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active]
	FROM dbo.Mc_EntityField
	WHERE (EntityFieldId = @EntityFieldId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEntityFieldListValue]
(
	@EntityFieldListValueId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [EntityFieldListValueId], [EntityFieldId], [Name], [Value], [Default], [Active]
	FROM [dbo].[Mc_EntityFieldListsValues]
	WHERE (EntityFieldListValueId = @EntityFieldListValueId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEntityFieldListValues]
(
	@EntityFieldId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [EntityFieldListValueId], [EntityFieldId], [Name], [Value], [Default], [Active]
	FROM [dbo].[Mc_EntityFieldListsValues]
	WHERE (EntityFieldId = @EntityFieldId) AND ((@Active IS NULL) OR (Active = @Active));
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEntityFields]
(
	@EntityId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataTypeId], [DefaultValue], [AllowDBNull], [Unique], [MaxLength], [MinValue], [MaxValue], [DecimalDigits], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active]
	FROM dbo.Mc_EntityField
	WHERE ([EntityId] = @EntityId) 
		AND (OrganizationId = @OrganizationId) 
		AND ((@InstanceId IS NULL) OR (InstanceId = @InstanceId))
		AND ((@Active IS NULL) OR (Active = @Active));
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertEntityField]
(
	@EntityFieldId uniqueidentifier,
	@EntityFieldTypeId int,
	@Name nvarchar(255),
	@Description nvarchar(255),
	@DataTypeId int,
	@DefaultValue nvarchar(512),
	@AllowDBNull bit,
	@Unique bit,
	@MaxLength int,
	@MinValue nvarchar(512),
	@MaxValue nvarchar(512),
	@DecimalDigits int,
	@OrderNumber int,
	@EntityId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_EntityField] ([EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataTypeId], [DefaultValue], [AllowDBNull], [Unique], [MaxLength], [MinValue], [MaxValue], [DecimalDigits], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active])
	VALUES (@EntityFieldId, @EntityFieldTypeId, @Name, @Description, @DataTypeId, @DefaultValue, @AllowDBNull, @Unique, @MaxLength, @MinValue, @MaxValue, @DecimalDigits, @OrderNumber, @EntityId, @OrganizationId, @InstanceId, @Active);
	
	SELECT [EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataTypeId], [DefaultValue], [AllowDBNull], [Unique], [MaxLength], [MinValue], [MaxValue], [DecimalDigits], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active]
	FROM dbo.Mc_EntityField
	WHERE (EntityFieldId = @EntityFieldId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertEntityFieldListValue]
(
	@EntityFieldListValueId uniqueidentifier,
	@EntityFieldId uniqueidentifier,
	@Name nvarchar(255),
	@Value nvarchar(512),
	@Default bit,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_EntityFieldListsValues] ([EntityFieldListValueId], [EntityFieldId], [Name], [Value], [Default], [Active])
	VALUES (@EntityFieldListValueId, @EntityFieldId, @Name, @Value, @Default, @Active);

	IF @Default = 1
	BEGIN
		DECLARE @EntityFieldTypeId int;
		
		SELECT @EntityFieldTypeId = [EntityFieldTypeId]
		FROM [dbo].[Mc_EntityField]
		WHERE [EntityFieldId] = @EntityFieldId;

		IF @EntityFieldTypeId = 2
			UPDATE [dbo].[Mc_EntityFieldListsValues]
			SET [Default] = 0
			WHERE ([EntityFieldId] = @EntityFieldId) AND ([EntityFieldListValueId] <> @EntityFieldListValueId);
	END

	SELECT [EntityFieldListValueId], [EntityFieldId], [Name], [Value], [Default], [Active]
	FROM [dbo].[Mc_EntityFieldListsValues]
	WHERE (EntityFieldListValueId = @EntityFieldListValueId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateEntityField]
(
	@EntityFieldId uniqueidentifier,
	@EntityFieldTypeId int,
	@Name nvarchar(255),
	@Description nvarchar(255),
	@DataTypeId int,
	@DefaultValue nvarchar(512),
	@AllowDBNull bit,
	@Unique bit,
	@MaxLength int,
	@MinValue nvarchar(512),
	@MaxValue nvarchar(512),
	@DecimalDigits int,
	@OrderNumber int,
	@EntityId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [dbo].[Mc_EntityField]
	SET [EntityFieldTypeId] = @EntityFieldTypeId, [Name] = @Name, [Description] = @Description, [DataTypeId] = @DataTypeId, [DefaultValue] = @DefaultValue
		, [AllowDBNull] = @AllowDBNull, [Unique] = @Unique, [MaxLength] = @MaxLength, [MinValue] = @MinValue, [MaxValue] = @MaxValue, [DecimalDigits] = @DecimalDigits
		, [OrderNumber] = @OrderNumber, [EntityId] = @EntityId, [OrganizationId] = @OrganizationId, [InstanceId] = @InstanceId, [Active] = @Active
	WHERE (EntityFieldId = @EntityFieldId);
	
	SELECT [EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataTypeId], [DefaultValue], [AllowDBNull], [Unique], [MaxLength], [MinValue], [MaxValue], [DecimalDigits], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active]
	FROM dbo.Mc_EntityField
	WHERE (EntityFieldId = @EntityFieldId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateEntityFieldListValue]
(
	@EntityFieldListValueId uniqueidentifier,
	@EntityFieldId uniqueidentifier,
	@Name nvarchar(255),
	@Value nvarchar(512),
	@Default bit,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [dbo].[Mc_EntityFieldListsValues]
	SET [EntityFieldId] = @EntityFieldId, [Name] = @Name, [Value] = @Value, [Default] = @Default, [Active] = @Active
	WHERE (EntityFieldListValueId = @EntityFieldListValueId);
	
	IF @Default = 1
	BEGIN
		DECLARE @EntityFieldTypeId int;
		
		SELECT @EntityFieldTypeId = [EntityFieldTypeId]
		FROM [dbo].[Mc_EntityField]
		WHERE [EntityFieldId] = @EntityFieldId;

		IF @EntityFieldTypeId = 2
			UPDATE [dbo].[Mc_EntityFieldListsValues]
			SET [Default] = 0
			WHERE ([EntityFieldId] = @EntityFieldId) AND ([EntityFieldListValueId] <> @EntityFieldListValueId);
	END

	SELECT EntityFieldListValueId, [EntityFieldId], [Name], [Value], [Default], [Active]
	FROM [dbo].[Mc_EntityFieldListsValues]
	WHERE (EntityFieldListValueId = @EntityFieldListValueId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityField_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityField] DROP CONSTRAINT [FK_Mc_EntityField_Mc_Instance]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityField_Mc_Instance')
      ALTER TABLE [dbo].[Mc_EntityField] ADD CONSTRAINT [FK_Mc_EntityField_Mc_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityFieldListsValues_Mc_EntityField')
      ALTER TABLE [dbo].[Mc_EntityFieldListsValues] ADD CONSTRAINT [FK_Mc_EntityFieldListsValues_Mc_EntityField] FOREIGN KEY ([EntityFieldId]) REFERENCES [dbo].[Mc_EntityField] ([EntityFieldId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityFieldsValues_Mc_EntityField')
      ALTER TABLE [dbo].[Mc_EntityFieldsValues] ADD CONSTRAINT [FK_Mc_EntityFieldsValues_Mc_EntityField] FOREIGN KEY ([EntityFieldId]) REFERENCES [dbo].[Mc_EntityField] ([EntityFieldId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityFieldListsValues_Mc_EntityField')
      ALTER TABLE [dbo].[Mc_EntityFieldListsValues] DROP CONSTRAINT [FK_Mc_EntityFieldListsValues_Mc_EntityField]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EntityFieldListsValues_Mc_EntityField')
      ALTER TABLE [dbo].[Mc_EntityFieldListsValues] ADD CONSTRAINT [FK_Mc_EntityFieldListsValues_Mc_EntityField] FOREIGN KEY ([EntityFieldId]) REFERENCES [dbo].[Mc_EntityField] ([EntityFieldId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION