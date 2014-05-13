BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

CREATE TABLE [dbo].[Mc_UnitsOfMeasure] (
   [UnitsOfMeasureId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_MC_UnitsOfMeasure_UnitsOfMeasureId] DEFAULT (newid()),
   [OrganizationId] [uniqueidentifier] NULL,
   [SingularName] [nvarchar] (255) NOT NULL,
   [SingularAbbrv] [nvarchar] (50) NOT NULL,
   [PluralName] [nvarchar] (255) NOT NULL,
   [PluralAbbrv] [nvarchar] (50) NOT NULL,
   [GroupName] [nvarchar] (50) NOT NULL CONSTRAINT [DF_Mc_UnitsOfMeasure_GroupName] DEFAULT (N'')
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_UnitsOfMeasure] ADD CONSTRAINT [PK_Mc_UnitsOfMeasure] PRIMARY KEY CLUSTERED ([UnitsOfMeasureId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
CREATE TABLE [dbo].[Mc_UnitsOfMeasureConversion] (
   [UnitOfMeasureFrom] [uniqueidentifier] NOT NULL,
   [UnitOfMeasureTo] [uniqueidentifier] NOT NULL,
   [Factor] [float] NOT NULL CONSTRAINT [DF_Mc_UnitsOfMeasureConversion_Factor] DEFAULT ((1.0))
)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_UnitsOfMeasureConversion] ADD CONSTRAINT [PK_Mc_UnitsOfMeasureConversion] PRIMARY KEY CLUSTERED ([UnitOfMeasureFrom], [UnitOfMeasureTo])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 exec('CREATE PROCEDURE [dbo].[Mc_DeleteUnitsOfMeasure]
(
	@UnitsOfMeasureId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM [dbo].[Mc_UnitsOfMeasureConversion]
    WHERE UnitOfMeasureFrom = @UnitsOfMeasureId
    OR UnitOfMeasureTo = @UnitsOfMeasureId;

	DELETE FROM [dbo].[Mc_UnitsOfMeasure] 
	WHERE UnitsOfMeasureId = @UnitsOfMeasureId;
	
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 exec('CREATE PROCEDURE [dbo].[Mc_DeleteUnitsOfMeasureConversion]
(
	@UnitOfMeasureFrom uniqueidentifier,
	@UnitOfMeasureTo uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.[Mc_UnitsOfMeasureConversion]
	WHERE UnitOfMeasureFrom = @UnitOfMeasureFrom
	AND UnitOfMeasureTo = @UnitOfMeasureTo;
	
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 exec('CREATE PROCEDURE [dbo].[Mc_GetUnitsOfMeasure]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [UnitsOfMeasureId]
      ,[OrganizationId]
      ,[SingularName]
      ,[SingularAbbrv]
      ,[PluralName]
      ,[PluralAbbrv]
      ,[GroupName]
	FROM [dbo].[Mc_UnitsOfMeasure]
	ORDER BY [OrganizationId] asc, [GroupName] asc, [SingularName] asc;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 exec('CREATE PROCEDURE [dbo].[Mc_GetUnitsOfMeasureConversion]
(
	@UnitOfMeasureFrom uniqueidentifier=null,
	@UnitOfMeasureTo uniqueidentifier=null
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [UnitOfMeasureFrom]
	  ,[UnitOfMeasureTo]
	  ,[Factor]
	FROM [dbo].[Mc_UnitsOfMeasureConversion]
	WHERE (@UnitOfMeasureFrom IS NULL OR UnitOfMeasureFrom = @UnitOfMeasureFrom)
	AND (@UnitOfMeasureTo IS NULL OR UnitOfMeasureTo = @UnitOfMeasureTo)
  
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 exec('CREATE PROCEDURE [dbo].[Mc_InsertUnitsOfMeasure]
(
	@UnitsOfMeasureId uniqueidentifier,
	@OrganizationId uniqueidentifier = NULL,
	@SingularName nvarchar(255),
	@SingularAbbrv nvarchar(50),
	@PluralName nvarchar(255),
	@PluralAbbrv nvarchar(50),
	@GroupName nvarchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_UnitsOfMeasure]
           ([UnitsOfMeasureId]
           ,[OrganizationId]
           ,[SingularName]
           ,[SingularAbbrv]
           ,[PluralName]
           ,[PluralAbbrv]
           ,[GroupName])
     VALUES(
			@UnitsOfMeasureId
           ,@OrganizationId
           ,@SingularName
           ,@SingularAbbrv
           ,@PluralName
           ,@PluralAbbrv
           ,@GroupName);
	
	SELECT [UnitsOfMeasureId]
      ,[OrganizationId]
      ,[SingularName]
      ,[SingularAbbrv]
      ,[PluralName]
      ,[PluralAbbrv]
      ,[GroupName]
	FROM [dbo].[Mc_UnitsOfMeasure]
	WHERE [UnitsOfMeasureId] = @UnitsOfMeasureId;

END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 exec('CREATE PROCEDURE [dbo].[Mc_InsertUnitsOfMeasureConversion]
(
	@UnitOfMeasureFrom uniqueidentifier,
	@UnitOfMeasureTo uniqueidentifier,
	@Factor float
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]
           ([UnitOfMeasureFrom]
           ,[UnitOfMeasureTo]
           ,[Factor])
    VALUES
           (@UnitOfMeasureFrom
           ,@UnitOfMeasureTo
           ,@Factor);
	
	SELECT [UnitOfMeasureFrom]
      ,[UnitOfMeasureTo]
      ,[Factor]
	FROM [dbo].[Mc_UnitsOfMeasureConversion]
	WHERE UnitOfMeasureFrom = @UnitOfMeasureFrom
	AND UnitOfMeasureTo = @UnitOfMeasureTo;

END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 exec('CREATE PROCEDURE [dbo].[Mc_UpdateUnitsOfMeasure]
(
	@UnitsOfMeasureId uniqueidentifier,
	@SingularName nvarchar(255),
	@SingularAbbrv nvarchar(50),
	@PluralName nvarchar(255),
	@PluralAbbrv nvarchar(50),
	@GroupName nvarchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [dbo].[Mc_UnitsOfMeasure]
	SET [SingularName] = @SingularName
      ,[SingularAbbrv] = @SingularAbbrv
      ,[PluralName] = @PluralName
      ,[PluralAbbrv] = @PluralAbbrv
      ,[GroupName] = @GroupName
	WHERE UnitsOfMeasureId = @UnitsOfMeasureId;

	SELECT [UnitsOfMeasureId]
      ,[OrganizationId]
      ,[SingularName]
      ,[SingularAbbrv]
      ,[PluralName]
      ,[PluralAbbrv]
      ,[GroupName]
	FROM [dbo].[Mc_UnitsOfMeasure]
	WHERE [UnitsOfMeasureId] = @UnitsOfMeasureId;
	
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 exec('CREATE PROCEDURE [dbo].[Mc_UpdateUnitsOfMeasureConversion]
(
	@UnitOfMeasureFrom uniqueidentifier,
	@UnitOfMeasureTo uniqueidentifier,
	@Factor float
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [dbo].[Mc_UnitsOfMeasureConversion]
	SET [Factor] = @Factor
	WHERE UnitOfMeasureFrom = @UnitOfMeasureFrom
	AND UnitOfMeasureTo = @UnitOfMeasureTo;

	SELECT [UnitOfMeasureFrom]
      ,[UnitOfMeasureTo]
      ,[Factor]
	FROM [dbo].[Mc_UnitsOfMeasureConversion]
	WHERE UnitOfMeasureFrom = @UnitOfMeasureFrom
	AND UnitOfMeasureTo = @UnitOfMeasureTo;
	
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 exec('create TRIGGER [dbo].[Mc_UnitsOfMeasureConversion_DELETE]
   ON  [dbo].[Mc_UnitsOfMeasureConversion]
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;
	
	declare @sourceid uniqueidentifier, @targetid uniqueidentifier;
	
	select	@sourceid = [UnitOfMeasureFrom], 
			@targetid = [UnitOfMeasureTo]
	from deleted;
	
	delete from [dbo].[Mc_UnitsOfMeasureConversion]
	where [UnitOfMeasureFrom] = @targetid
	and [UnitOfMeasureTo] = @sourceid;
	
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 exec('CREATE TRIGGER [dbo].[Mc_UnitsOfMeasureConversion_INSERT]
   ON  [dbo].[Mc_UnitsOfMeasureConversion]
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	declare @sourceid uniqueidentifier, @targetid uniqueidentifier;
	declare @factor float;
	
	select	@sourceid = [UnitOfMeasureFrom], 
			@targetid = [UnitOfMeasureTo],
			@factor = [Factor]
	from inserted;
	
	if @factor < = 0.0 return;
	
	if exists(select ''true'' 
		from [dbo].[Mc_UnitsOfMeasureConversion] 
		where [UnitOfMeasureFrom] = @targetid
		and [UnitOfMeasureTo] = @sourceid)
	begin
		update [dbo].[Mc_UnitsOfMeasureConversion]
		set [Factor] = (1/@factor)
		where [UnitOfMeasureFrom] = @targetid
		and [UnitOfMeasureTo] = @sourceid;
	end
	else 
	begin 
		insert into [dbo].[Mc_UnitsOfMeasureConversion]
           ([UnitOfMeasureFrom]
           ,[UnitOfMeasureTo]
           ,[Factor])
		values
           (@targetid
           ,@sourceid
           ,(1/@factor));
	end

END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('00000000-0000-0000-0000-000000000044', '00000000-0000-0000-0000-000000000009', 1, N'Measure Units', N'Manage the Measure Units & Conversion', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/MeasureUnits.aspx', 0, N'', 1, 0, 1, 1, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_UnitsOfMeasureConversion] DISABLE TRIGGER [Mc_UnitsOfMeasureConversion_DELETE]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_UnitsOfMeasureConversion] DISABLE TRIGGER [Mc_UnitsOfMeasureConversion_INSERT]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName]) VALUES ('{27CF5FE9-957C-46D2-A986-9B9A0F9075A0}', NULL, N'gallon', N'gal', N'gallons', N'gals', N'Volume')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName]) VALUES ('{311D01C6-2DAA-48C7-AF80-1643EEF930E9}', NULL, N'foot', N'ft', N'foots', N'fts', N'Length')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName]) VALUES ('{38BB9678-9F0C-4F50-B258-32C52DCE189D}', NULL, N'pound', N'lb', N'pounds', N'lbs', N'Weight')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName]) VALUES ('{67AC9A2E-4714-4E53-8392-F923D65A9DD1}', NULL, N'inch', N'in', N'inches', N'in', N'Length')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName]) VALUES ('{7B16FA6F-0576-4C8E-9F3E-717DCADE3A27}', NULL, N'liter', N'liter', N'liters', N'liters', N'Volume')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName]) VALUES ('{91D02BBE-8C25-4F78-9F29-09CA8B27C173}', NULL, N'kilogram', N'kg', N'kilograms', N'kgs', N'Weight')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName]) VALUES ('{B104173D-E539-4022-94C3-3F7ABBCBEB73}', NULL, N'quart', N'qrt', N'quarts', N'qrts', N'Volume')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName]) VALUES ('{EAE5B378-A006-4000-A475-731E87B06666}', NULL, N'yard', N'yd', N'yards', N'yds', N'Length')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName]) VALUES ('{EF22DB7D-3244-464F-B7D0-03CDB527B490}', NULL, N'pint', N'pint', N'pints', N'pints', N'Volume')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName]) VALUES ('{F9B6D4F8-9455-4364-9650-F35F475AF301}', NULL, N'metre', N'metre', N'metres', N'metres', N'Length')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{27CF5FE9-957C-46D2-A986-9B9A0F9075A0}', '{7B16FA6F-0576-4C8E-9F3E-717DCADE3A27}', 4.546090000000000e+000)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{311D01C6-2DAA-48C7-AF80-1643EEF930E9}', '{67AC9A2E-4714-4E53-8392-F923D65A9DD1}', 1.200000000000000e+001)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{311D01C6-2DAA-48C7-AF80-1643EEF930E9}', '{EAE5B378-A006-4000-A475-731E87B06666}', 3.333333333333333e-001)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{311D01C6-2DAA-48C7-AF80-1643EEF930E9}', '{F9B6D4F8-9455-4364-9650-F35F475AF301}', 3.048037064130700e-001)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{38BB9678-9F0C-4F50-B258-32C52DCE189D}', '{91D02BBE-8C25-4F78-9F29-09CA8B27C173}', 4.535923700000000e-001)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{67AC9A2E-4714-4E53-8392-F923D65A9DD1}', '{311D01C6-2DAA-48C7-AF80-1643EEF930E9}', 8.333333333333333e-002)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{67AC9A2E-4714-4E53-8392-F923D65A9DD1}', '{EAE5B378-A006-4000-A475-731E87B06666}', 2.777777777777778e-002)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{67AC9A2E-4714-4E53-8392-F923D65A9DD1}', '{F9B6D4F8-9455-4364-9650-F35F475AF301}', 2.540005080010160e-002)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{7B16FA6F-0576-4C8E-9F3E-717DCADE3A27}', '{27CF5FE9-957C-46D2-A986-9B9A0F9075A0}', 2.199692482990878e-001)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{7B16FA6F-0576-4C8E-9F3E-717DCADE3A27}', '{B104173D-E539-4022-94C3-3F7ABBCBEB73}', 8.798769900000000e-001)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{7B16FA6F-0576-4C8E-9F3E-717DCADE3A27}', '{EF22DB7D-3244-464F-B7D0-03CDB527B490}', 1.759753986392702e+000)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{91D02BBE-8C25-4F78-9F29-09CA8B27C173}', '{38BB9678-9F0C-4F50-B258-32C52DCE189D}', 2.204622621848776e+000)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{B104173D-E539-4022-94C3-3F7ABBCBEB73}', '{7B16FA6F-0576-4C8E-9F3E-717DCADE3A27}', 1.136522504128674e+000)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{EAE5B378-A006-4000-A475-731E87B06666}', '{311D01C6-2DAA-48C7-AF80-1643EEF930E9}', 3.000000000000000e+000)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{EAE5B378-A006-4000-A475-731E87B06666}', '{67AC9A2E-4714-4E53-8392-F923D65A9DD1}', 3.600000000000000e+001)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{EAE5B378-A006-4000-A475-731E87B06666}', '{F9B6D4F8-9455-4364-9650-F35F475AF301}', 9.144000000000000e-001)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{EF22DB7D-3244-464F-B7D0-03CDB527B490}', '{7B16FA6F-0576-4C8E-9F3E-717DCADE3A27}', 5.682612500000001e-001)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{F9B6D4F8-9455-4364-9650-F35F475AF301}', '{311D01C6-2DAA-48C7-AF80-1643EEF930E9}', 3.280800000000000e+000)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{F9B6D4F8-9455-4364-9650-F35F475AF301}', '{67AC9A2E-4714-4E53-8392-F923D65A9DD1}', 3.937000000000000e+001)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor]) VALUES ('{F9B6D4F8-9455-4364-9650-F35F475AF301}', '{EAE5B378-A006-4000-A475-731E87B06666}', 1.093613298337708e+000)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_UnitsOfMeasureConversion] ENABLE TRIGGER [Mc_UnitsOfMeasureConversion_DELETE]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Mc_UnitsOfMeasureConversion] ENABLE TRIGGER [Mc_UnitsOfMeasureConversion_INSERT]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION

