IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UnitsOfMeasureConversion_INSERT]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
DROP TRIGGER [dbo].[Mc_UnitsOfMeasureConversion_INSERT]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Mc_UnitsOfMeasureConversion_DELETE]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
DROP TRIGGER [dbo].[Mc_UnitsOfMeasureConversion_DELETE]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Mc_UnitsOfMeasureConversion_INSERT]
   ON  [dbo].[Mc_UnitsOfMeasureConversion]
   AFTER INSERT, UPDATE
AS 
BEGIN
	set nocount on;
	
	declare @sourceid uniqueidentifier, @targetid uniqueidentifier, @orgid uniqueidentifier;
	declare @factor float;
	
	select	@sourceid = [UnitOfMeasureFrom], 
			@targetid = [UnitOfMeasureTo],
			@orgid = [OrganizationId],
			@factor = [Factor]
	from inserted;
	
	if @factor < = 0.0 or @sourceid is null or @targetid is null return;
	
	if exists(select 'true'
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
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create TRIGGER [dbo].[Mc_UnitsOfMeasureConversion_DELETE]
   ON  [dbo].[Mc_UnitsOfMeasureConversion]
   AFTER DELETE
AS 
BEGIN
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
END

GO
