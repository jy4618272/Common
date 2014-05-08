BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

DECLARE @ProductVersion nvarchar(128), @NVarCharMax nvarchar(128)

SET @ProductVersion = CAST(SERVERPROPERTY('productversion') AS nvarchar(128))
SET @ProductVersion = LEFT(@ProductVersion, CHARINDEX('.', @ProductVersion) - 1)

IF (CAST(@ProductVersion AS int) > 8)
	SET @NVarCharMax = 'max'
ELSE
	SET @NVarCharMax = '4000'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_EntityField](
		[EntityFieldId] [uniqueidentifier] NOT NULL,
		[EntityFieldTypeId] [int] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Description] [nvarchar](255) NOT NULL,
		[DataType] [varchar](50) NOT NULL,
		[DefaultValue] [nvarchar](512) NULL,
		[AllowDBNull] [bit] NOT NULL,
		[ReadOnly] [bit] NOT NULL,
		[Unique] [bit] NOT NULL,
		[MaxLength] [int] NOT NULL,
		[MinValue] [nvarchar](512) NULL,
		[MaxValue] [nvarchar](512) NULL,
		[OrderNumber] [int] NOT NULL,
		[EntityId] [uniqueidentifier] NOT NULL,
		[OrganizationId] [uniqueidentifier] NOT NULL,
		[InstanceId] [uniqueidentifier] NULL,
		[Active] [bit] NOT NULL,
	 CONSTRAINT [PK_Mc_EntityField] PRIMARY KEY CLUSTERED 
	(
		[EntityFieldId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_EntityFieldListsValues](
		[EntityFieldListValueId] [uniqueidentifier] NOT NULL,
		[EntityFieldId] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Value] [nvarchar](512) NOT NULL,
		[Active] [bit] NOT NULL,
	 CONSTRAINT [PK_Mc_EntityFieldListsValues] PRIMARY KEY CLUSTERED 
	(
		[EntityFieldListValueId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE TABLE [dbo].[Mc_EntityFieldsValues](
	[EntityFieldValueId] [uniqueidentifier] NOT NULL,
	[EntityFieldId] [uniqueidentifier] NOT NULL,
	[LocalEntityId] [nvarchar](255) NOT NULL,
	[Value] [nvarchar](' + @NVarCharMax + ') NULL,
 CONSTRAINT [PK_Mc_EntityFieldsValues] PRIMARY KEY CLUSTERED 
(
	[EntityFieldValueId] ASC
) ON [PRIMARY]
) ON [PRIMARY]')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityField]  WITH CHECK ADD  CONSTRAINT [FK_Mc_EntityField_Mc_Instance] FOREIGN KEY([InstanceId])
	REFERENCES [dbo].[Mc_Instance] ([InstanceId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityField] CHECK CONSTRAINT [FK_Mc_EntityField_Mc_Instance]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityField] ADD  CONSTRAINT [DF_Mc_EntityField_AllowDBNull]  DEFAULT ((1)) FOR [AllowDBNull]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityField] ADD  CONSTRAINT [DF_Mc_EntityField_ReadOnly]  DEFAULT ((0)) FOR [ReadOnly]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityField] ADD  CONSTRAINT [DF_Mc_EntityField_Unique]  DEFAULT ((0)) FOR [Unique]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityField] ADD  CONSTRAINT [DF_Mc_EntityField_MaxLength]  DEFAULT ((0)) FOR [MaxLength]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityField] ADD  CONSTRAINT [DF_Mc_EntityField_OrderNumber]  DEFAULT ((0)) FOR [OrderNumber]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityField] ADD  CONSTRAINT [DF_Mc_EntityField_Active]  DEFAULT ((1)) FOR [Active]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityFieldListsValues]  WITH CHECK ADD  CONSTRAINT [FK_Mc_EntityFieldListsValues_Mc_EntityField] FOREIGN KEY([EntityFieldId])
	REFERENCES [dbo].[Mc_EntityField] ([EntityFieldId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityFieldListsValues] CHECK CONSTRAINT [FK_Mc_EntityFieldListsValues_Mc_EntityField]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityFieldListsValues] ADD  CONSTRAINT [DF_Mc_EntityFieldListsValues_Active]  DEFAULT ((1)) FOR [Active]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityFieldsValues]  WITH CHECK ADD  CONSTRAINT [FK_Mc_EntityFieldsValues_Mc_EntityField] FOREIGN KEY([EntityFieldId])
	REFERENCES [dbo].[Mc_EntityField] ([EntityFieldId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityFieldsValues] CHECK CONSTRAINT [FK_Mc_EntityFieldsValues_Mc_EntityField]

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_EntityFieldsValues] ADD  CONSTRAINT [DF_Mc_EntityFieldsValues_EntityFieldValueId]  DEFAULT (newid()) FOR [EntityFieldValueId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteEntityField]
(
	@EntityFieldId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM [dbo].[Mc_EntityField]
	WHERE (EntityFieldId = @EntityFieldId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteEntityFieldListValue]
(
	@EntityFieldListValueId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM [dbo].[Mc_EntityFieldListsValues]
	WHERE (EntityFieldListValueId = @EntityFieldListValueId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteEntityFieldValue]
(
	@EntityFieldValueId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM [dbo].[Mc_EntityFieldsValues]
	WHERE (EntityFieldValueId = @EntityFieldValueId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetEntityField]
(
	@EntityFieldId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataType], [DefaultValue], [AllowDBNull], [ReadOnly], [Unique], [MaxLength], [MinValue], [MaxValue], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active]
	FROM dbo.Mc_EntityField
	WHERE (EntityFieldId = @EntityFieldId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetEntityFieldListValue]
(
	@EntityFieldListValueId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [EntityFieldListValueId], [EntityFieldId], [Name], [Value], [Active]
	FROM [dbo].[Mc_EntityFieldListsValues]
	WHERE (EntityFieldListValueId = @EntityFieldListValueId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetEntityFieldListValues]
(
	@EntityFieldId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [EntityFieldListValueId], [EntityFieldId], [Name], [Value], [Active]
	FROM [dbo].[Mc_EntityFieldListsValues]
	WHERE (EntityFieldId = @EntityFieldId) AND ((@Active IS NULL) OR (Active = @Active));
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetEntityFields]
(
	@EntityId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataType], [DefaultValue], [AllowDBNull], [ReadOnly], [Unique], [MaxLength], [MinValue], [MaxValue], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active]
	FROM dbo.Mc_EntityField
	WHERE ([EntityId] = @EntityId) 
		AND (OrganizationId = @OrganizationId) 
		AND (((@InstanceId IS NULL) AND (InstanceId IS NULL)) OR (InstanceId = @InstanceId))
		AND ((@Active IS NULL) OR (Active = @Active));
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetEntityFieldsValues]
(
	@EntityId uniqueidentifier,
	@LocalEntityId nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT v.[EntityFieldValueId], v.[EntityFieldId], v.[LocalEntityId], v.[Value]
	FROM [dbo].[Mc_EntityFieldsValues] AS v
	INNER JOIN dbo.Mc_EntityField AS f
		ON	v.EntityFieldId = f.EntityFieldId
	WHERE (f.EntityId = @EntityId) AND (v.LocalEntityId = @LocalEntityId) AND (f.Active = 1);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertEntityField]
(
	@EntityFieldId uniqueidentifier,
	@EntityFieldTypeId int,
	@Name nvarchar(255),
	@Description nvarchar(255),
	@DataType varchar(50),
	@DefaultValue nvarchar(512),
	@AllowDBNull bit,
	@ReadOnly bit,
	@Unique bit,
	@MaxLength int,
	@MinValue nvarchar(512),
	@MaxValue nvarchar(512),
	@OrderNumber int,
	@EntityId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_EntityField] ([EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataType], [DefaultValue], [AllowDBNull], [ReadOnly], [Unique], [MaxLength], [MinValue], [MaxValue], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active])
	VALUES (@EntityFieldId, @EntityFieldTypeId, @Name, @Description, @DataType, @DefaultValue, @AllowDBNull, @ReadOnly, @Unique, @MaxLength, @MinValue, @MaxValue, @OrderNumber, @EntityId, @OrganizationId, @InstanceId, @Active);
	
	SELECT [EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataType], [DefaultValue], [AllowDBNull], [ReadOnly], [Unique], [MaxLength], [MinValue], [MaxValue], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active]
	FROM dbo.Mc_EntityField
	WHERE (EntityFieldId = @EntityFieldId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertEntityFieldListValue]
(
	@EntityFieldListValueId uniqueidentifier,
	@EntityFieldId uniqueidentifier,
	@Name nvarchar(255),
	@Value nvarchar(512),
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_EntityFieldListsValues] (EntityFieldListValueId, [EntityFieldId], [Name], [Value], [Active])
	VALUES (@EntityFieldListValueId, @EntityFieldId, @Name, @Value, @Active);

	SELECT EntityFieldListValueId, [EntityFieldId], [Name], [Value], [Active]
	FROM [dbo].[Mc_EntityFieldListsValues]
	WHERE (EntityFieldListValueId = @EntityFieldListValueId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertEntityFieldValue]
(
	@EntityFieldValueId uniqueidentifier,
	@EntityFieldId uniqueidentifier,
	@LocalEntityId nvarchar(255),
	@Value nvarchar(MAX)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_EntityFieldsValues] ([EntityFieldValueId], [EntityFieldId], [LocalEntityId], [Value])
	VALUES (@EntityFieldValueId, @EntityFieldId, @LocalEntityId, @Value);

	SELECT [EntityFieldValueId], [EntityFieldId], [LocalEntityId], [Value]
	FROM [dbo].[Mc_EntityFieldsValues]
	WHERE (EntityFieldValueId = @EntityFieldValueId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateEntityField]
(
	@EntityFieldId uniqueidentifier,
	@EntityFieldTypeId int,
	@Name nvarchar(255),
	@Description nvarchar(255),
	@DataType varchar(50),
	@DefaultValue nvarchar(512),
	@AllowDBNull bit,
	@ReadOnly bit,
	@Unique bit,
	@MaxLength int,
	@MinValue nvarchar(512),
	@MaxValue nvarchar(512),
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
	SET [EntityFieldTypeId] = @EntityFieldTypeId, [Name] = @Name, [Description] = @Description, [DataType] = @DataType, [DefaultValue] = @DefaultValue
		, [AllowDBNull] = @AllowDBNull, [ReadOnly] = @ReadOnly, [Unique] = @Unique, [MaxLength] = @MaxLength, [MinValue] = @MinValue, [MaxValue] = @MaxValue
		, [OrderNumber] = @OrderNumber, [EntityId] = @EntityId, [OrganizationId] = @OrganizationId, [InstanceId] = @InstanceId, [Active] = @Active
	WHERE (EntityFieldId = @EntityFieldId);
	
	SELECT [EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataType], [DefaultValue], [AllowDBNull], [ReadOnly], [Unique], [MaxLength], [MinValue], [MaxValue], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active]
	FROM dbo.Mc_EntityField
	WHERE (EntityFieldId = @EntityFieldId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateEntityFieldListValue]
(
	@EntityFieldListValueId uniqueidentifier,
	@EntityFieldId uniqueidentifier,
	@Name nvarchar(255),
	@Value nvarchar(512),
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [dbo].[Mc_EntityFieldListsValues]
	SET [EntityFieldId] = @EntityFieldId, [Name] = @Name, [Value] = @Value, [Active] = @Active
	WHERE (EntityFieldListValueId = @EntityFieldListValueId);

	SELECT EntityFieldListValueId, [EntityFieldId], [Name], [Value], [Active]
	FROM [dbo].[Mc_EntityFieldListsValues]
	WHERE (EntityFieldListValueId = @EntityFieldListValueId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateEntityFieldValue]
(
	@EntityFieldValueId uniqueidentifier,
	@EntityFieldId uniqueidentifier,
	@LocalEntityId nvarchar(255),
	@Value nvarchar(MAX)
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	UPDATE [dbo].[Mc_EntityFieldsValues]
	SET [EntityFieldId] = @EntityFieldId, [LocalEntityId] = @LocalEntityId, [Value] = @Value
	WHERE (EntityFieldValueId = @EntityFieldValueId);

	SELECT [EntityFieldValueId], [EntityFieldId], [LocalEntityId], [Value]
	FROM [dbo].[Mc_EntityFieldsValues]
	WHERE (EntityFieldValueId = @EntityFieldValueId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
	
IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
