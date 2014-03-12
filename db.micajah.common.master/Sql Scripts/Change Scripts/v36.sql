BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

declare @emptyId uniqueidentifier;
select @emptyId = '00000000-0000-0000-0000-000000000000';

IF @@TRANCOUNT = 1
	delete from [dbo].[Mc_UnitsOfMeasure];

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
   
IF @@TRANCOUNT = 1
	delete from [dbo].[Mc_UnitsOfMeasureConversion];

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 AND EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[Mc_UnitsOfMeasure]') AND name = N'PK_Mc_UnitsOfMeasure_1')
	ALTER TABLE [dbo].[Mc_UnitsOfMeasure] DROP CONSTRAINT [PK_Mc_UnitsOfMeasure_1];

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 AND EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[Mc_UnitsOfMeasure]') AND name = N'PK_Mc_UnitsOfMeasure')
	ALTER TABLE [dbo].[Mc_UnitsOfMeasure] DROP CONSTRAINT [PK_Mc_UnitsOfMeasure];

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF  @@TRANCOUNT = 1 AND EXISTS(SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UnitsOfMeasureConversion_INSERT]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
	DROP TRIGGER [dbo].[Mc_UnitsOfMeasureConversion_INSERT];
	
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF  @@TRANCOUNT = 1 AND EXISTS(SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UnitsOfMeasureConversion_DELETE]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
	DROP TRIGGER [dbo].[Mc_UnitsOfMeasureConversion_DELETE];

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF  @@TRANCOUNT = 1 AND EXISTS(SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[dbo].[Mc_UnitsOfMeasureConversion]') AND name = N'PK_Mc_UnitsOfMeasureConversion')
	ALTER TABLE [dbo].[Mc_UnitsOfMeasureConversion] DROP CONSTRAINT [PK_Mc_UnitsOfMeasureConversion]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 AND EXISTS(SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_GetUnitsOfMeasureConversion]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Mc_GetUnitsOfMeasureConversion]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 AND EXISTS(SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertUnitsOfMeasure]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Mc_InsertUnitsOfMeasure]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 AND EXISTS(SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_InsertUnitsOfMeasureConversion]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Mc_InsertUnitsOfMeasureConversion]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 AND EXISTS(SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateUnitsOfMeasure]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Mc_UpdateUnitsOfMeasure]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 AND EXISTS(SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateUnitsOfMeasureConversion]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Mc_UpdateUnitsOfMeasureConversion]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 AND EXISTS(SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_DeleteUnitsOfMeasure]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Mc_DeleteUnitsOfMeasure]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 AND EXISTS(SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_DeleteUnitsOfMeasureConversion]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Mc_DeleteUnitsOfMeasureConversion]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 AND EXISTS(SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UpdateUnitsOfMeasureOverride]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Mc_UpdateUnitsOfMeasureOverride]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 
	ALTER TABLE [dbo].[Mc_UnitsOfMeasure] ALTER COLUMN OrganizationId uniqueidentifier NOT NULL;

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 
	ALTER TABLE [dbo].[Mc_UnitsOfMeasure] ADD CONSTRAINT [PK_Mc_UnitsOfMeasure_OrganizationId] PRIMARY KEY CLUSTERED 
	(
		[UnitsOfMeasureId] ASC,
		[OrganizationId] ASC
	) ON [PRIMARY];

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 
	ALTER TABLE [dbo].[Mc_UnitsOfMeasureConversion] ADD [OrganizationId] uniqueidentifier NOT NULL;

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF  @@TRANCOUNT = 1 
	ALTER TABLE [dbo].[Mc_UnitsOfMeasureConversion] ADD CONSTRAINT [PK_Mc_UnitsOfMeasureConversion] PRIMARY KEY CLUSTERED 
	(
		[UnitOfMeasureFrom] ASC,
		[UnitOfMeasureTo] ASC,
		[OrganizationId] ASC
	) ON [PRIMARY];

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 
	exec('create trigger [dbo].[Mc_UnitsOfMeasureConversion_INSERT]
   on  [dbo].[Mc_UnitsOfMeasureConversion]
   after insert, update
as 
begin
	set nocount on;
	
	declare @sourceid uniqueidentifier, @targetid uniqueidentifier, @orgid uniqueidentifier;
	declare @factor float;
	
	select	@sourceid = [UnitOfMeasureFrom], 
			@targetid = [UnitOfMeasureTo],
			@orgid = [OrganizationId],
			@factor = [Factor]
	from inserted;
	
	if @factor < = 0.0 or @sourceid is null or @targetid is null return;
	
	if exists(select ''true''
		from [dbo].[Mc_UnitsOfMeasureConversion] 
		where [UnitOfMeasureFrom] = @targetid
		and [UnitOfMeasureTo] = @sourceid
		and [OrganizationId] = @orgid)
	begin
		update [dbo].[Mc_UnitsOfMeasureConversion]
		set [Factor] = (1/@factor)
		where [UnitOfMeasureFrom] = @targetid
		and [UnitOfMeasureTo] = @sourceid
		and [OrganizationId] = @orgid;
	end
	else 
	begin 
		insert into [dbo].[Mc_UnitsOfMeasureConversion]
           ([UnitOfMeasureFrom]
           ,[UnitOfMeasureTo]
           ,[OrganizationId]
           ,[Factor])
		values
           (@targetid
           ,@sourceid
           ,@orgid
           ,(1/@factor));
	end
end');

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 
	exec('create trigger [dbo].[Mc_UnitsOfMeasureConversion_DELETE]
   on  [dbo].[Mc_UnitsOfMeasureConversion]
   after delete
as 
begin
	set nocount on;
	
	declare @sourceid uniqueidentifier, @targetid uniqueidentifier, @orgid uniqueidentifier;
	
	select	@sourceid = [UnitOfMeasureFrom], 
			@targetid = [UnitOfMeasureTo],
			@orgid = [OrganizationId]
	from deleted;
	
	delete from [dbo].[Mc_UnitsOfMeasureConversion]
	where [UnitOfMeasureFrom] = @targetid
	and [UnitOfMeasureTo] = @sourceid
	and [OrganizationId] = @orgid;
end');

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 
	exec('CREATE PROCEDURE [dbo].[Mc_InsertUnitsOfMeasure]
(
	@UnitsOfMeasureId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@SingularName nvarchar(255),
	@SingularAbbrv nvarchar(50),
	@PluralName nvarchar(255),
	@PluralAbbrv nvarchar(50),
	@GroupName nvarchar(50),
	@LocalName nvarchar(50)
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
           ,[GroupName]
           ,[LocalName])
     VALUES(
			@UnitsOfMeasureId
           ,@OrganizationId
           ,@SingularName
           ,@SingularAbbrv
           ,@PluralName
           ,@PluralAbbrv
           ,@GroupName
           ,@LocalName);
	
	SELECT [UnitsOfMeasureId]
      ,[OrganizationId]
      ,[SingularName]
      ,[SingularAbbrv]
      ,[PluralName]
      ,[PluralAbbrv]
      ,[GroupName]
      ,[LocalName]
	FROM [dbo].[Mc_UnitsOfMeasure]
	WHERE [UnitsOfMeasureId] = @UnitsOfMeasureId
	AND [OrganizationId] = @OrganizationId;
END');

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 
	exec('CREATE PROCEDURE [dbo].[Mc_GetUnitsOfMeasureConversion]
(
	@UnitOfMeasureFrom uniqueidentifier=null,
	@UnitOfMeasureTo uniqueidentifier=null
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [UnitOfMeasureFrom]
	  ,[UnitOfMeasureTo]
	  ,[OrganizationId]
	  ,[Factor]
	FROM [dbo].[Mc_UnitsOfMeasureConversion]
	WHERE (@UnitOfMeasureFrom IS NULL OR UnitOfMeasureFrom = @UnitOfMeasureFrom)
	AND (@UnitOfMeasureTo IS NULL OR UnitOfMeasureTo = @UnitOfMeasureTo);
END');

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 
	exec('CREATE PROCEDURE [dbo].[Mc_InsertUnitsOfMeasureConversion]
(
	@UnitOfMeasureFrom uniqueidentifier,
	@UnitOfMeasureTo uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@Factor float
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]
           ([UnitOfMeasureFrom]
           ,[UnitOfMeasureTo]
           ,[OrganizationId]
           ,[Factor])
    VALUES
           (@UnitOfMeasureFrom
           ,@UnitOfMeasureTo
           ,@OrganizationId
           ,@Factor);
	
	SELECT [UnitOfMeasureFrom]
      ,[UnitOfMeasureTo]
      ,[OrganizationId]
      ,[Factor]
	FROM [dbo].[Mc_UnitsOfMeasureConversion]
	WHERE UnitOfMeasureFrom = @UnitOfMeasureFrom
	AND UnitOfMeasureTo = @UnitOfMeasureTo
	AND OrganizationId = @OrganizationId;
END');

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 
	exec('CREATE PROCEDURE [dbo].[Mc_UpdateUnitsOfMeasure]
(
	@UnitsOfMeasureId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@SingularName nvarchar(255),
	@SingularAbbrv nvarchar(50),
	@PluralName nvarchar(255),
	@PluralAbbrv nvarchar(50),
	@GroupName nvarchar(50),
	@LocalName nvarchar(50)
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
      ,[LocalName] = @LocalName
	WHERE UnitsOfMeasureId = @UnitsOfMeasureId
	AND OrganizationId = @OrganizationId;

	SELECT [UnitsOfMeasureId]
      ,[OrganizationId]
      ,[SingularName]
      ,[SingularAbbrv]
      ,[PluralName]
      ,[PluralAbbrv]
      ,[GroupName]
      ,[LocalName]
	FROM [dbo].[Mc_UnitsOfMeasure]
	WHERE [UnitsOfMeasureId] = @UnitsOfMeasureId
	AND [OrganizationId] = @OrganizationId;
END
');

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 
	exec('CREATE PROCEDURE [dbo].[Mc_UpdateUnitsOfMeasureConversion]
(
	@UnitOfMeasureFrom uniqueidentifier,
	@UnitOfMeasureTo uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@Factor float
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [dbo].[Mc_UnitsOfMeasureConversion]
	SET [Factor] = @Factor
	WHERE UnitOfMeasureFrom = @UnitOfMeasureFrom
	AND UnitOfMeasureTo = @UnitOfMeasureTo
	AND OrganizationId = @OrganizationId;

	SELECT [UnitOfMeasureFrom]
      ,[UnitOfMeasureTo]
      ,[Factor]
	FROM [dbo].[Mc_UnitsOfMeasureConversion]
	WHERE UnitOfMeasureFrom = @UnitOfMeasureFrom
	AND UnitOfMeasureTo = @UnitOfMeasureTo
	AND OrganizationId = @OrganizationId;
END
');

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 
	exec('CREATE PROCEDURE [dbo].[Mc_DeleteUnitsOfMeasure]
(
	@UnitsOfMeasureId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM [dbo].[Mc_UnitsOfMeasureConversion]
    WHERE OrganizationId = @OrganizationId
    AND (UnitOfMeasureFrom = @UnitsOfMeasureId OR UnitOfMeasureTo = @UnitsOfMeasureId);

	DELETE FROM [dbo].[Mc_UnitsOfMeasure] 
	WHERE OrganizationId = @OrganizationId
	AND UnitsOfMeasureId = @UnitsOfMeasureId;
END
');

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 
	exec('CREATE PROCEDURE [dbo].[Mc_DeleteUnitsOfMeasureConversion]
(
	@UnitOfMeasureFrom uniqueidentifier,
	@UnitOfMeasureTo uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.[Mc_UnitsOfMeasureConversion]
	WHERE UnitOfMeasureFrom = @UnitOfMeasureFrom
	AND UnitOfMeasureTo = @UnitOfMeasureTo
	AND OrganizationId = @OrganizationId;
END
');

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 
	exec('CREATE PROCEDURE [dbo].[Mc_UpdateUnitsOfMeasureOverride]
(
	@UnitsOfMeasureId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	declare @emptyId uniqueidentifier;
	select @emptyId = ''00000000-0000-0000-0000-000000000000'';

	if not exists(select ''true'' 
		from [dbo].[Mc_UnitsOfMeasure]
		where [UnitsOfMeasureId] = @UnitsOfMeasureId
		and OrganizationId = @emptyId)
	return;
	
	if not exists(select ''true'' 
		from [dbo].[Mc_UnitsOfMeasure]
		where [UnitsOfMeasureId] = @UnitsOfMeasureId
		and OrganizationId = @OrganizationId)
	begin 
		insert into [dbo].[Mc_UnitsOfMeasure]
			   ([UnitsOfMeasureId]
			   ,[OrganizationId]
			   ,[SingularName]
			   ,[SingularAbbrv]
			   ,[PluralName]
			   ,[PluralAbbrv]
			   ,[GroupName]
			   ,[LocalName])
		 select UnitsOfMeasureId
			   ,@OrganizationId
			   ,SingularName
			   ,SingularAbbrv
			   ,PluralName
			   ,PluralAbbrv
			   ,GroupName
			   ,LocalName
		from [dbo].[Mc_UnitsOfMeasure]
		where UnitsOfMeasureId = @UnitsOfMeasureId
		and OrganizationId = @emptyId;
		
		insert into [dbo].[Mc_UnitsOfMeasureConversion]
           ([UnitOfMeasureFrom]
           ,[UnitOfMeasureTo]
           ,[Factor]
           ,[OrganizationId])
		select [UnitOfMeasureFrom]
		  ,[UnitOfMeasureTo]
		  ,[Factor]
		  ,@OrganizationId
		from [dbo].[Mc_UnitsOfMeasureConversion]
		where UnitOfMeasureFrom = @UnitsOfMeasureId
		and OrganizationId = @emptyid
		and UnitOfMeasureTo in (
			select UnitOfMeasureTo from (
				select UnitOfMeasureTo
				from [dbo].[Mc_UnitsOfMeasureConversion]
				where (UnitOfMeasureFrom = @UnitsOfMeasureId)
				and OrganizationId = @emptyid) as s
			inner join [dbo].[Mc_UnitsOfMeasure] as s1
			on s1.UnitsOfMeasureId = s.UnitOfMeasureTo
			and s1.OrganizationId = @OrganizationId
			)

		select [UnitsOfMeasureId]
		  ,[OrganizationId]
		  ,[SingularName]
		  ,[SingularAbbrv]
		  ,[PluralName]
		  ,[PluralAbbrv]
		  ,[GroupName]
		  ,[LocalName]
		from [dbo].[Mc_UnitsOfMeasure]
		where [UnitsOfMeasureId] = @UnitsOfMeasureId
		and [OrganizationId] = @OrganizationId;
	end
END
');

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
