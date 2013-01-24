CREATE TABLE [dbo].[Mc_UnitsOfMeasureConversion] (
    [UnitOfMeasureFrom] UNIQUEIDENTIFIER NOT NULL,
    [UnitOfMeasureTo]   UNIQUEIDENTIFIER NOT NULL,
    [Factor]            FLOAT (53)       CONSTRAINT [DF_Mc_UnitsOfMeasureConversion_Factor] DEFAULT ((1.0)) NOT NULL,
    [OrganizationId]    UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Mc_UnitsOfMeasureConversion] PRIMARY KEY CLUSTERED ([UnitOfMeasureFrom] ASC, [UnitOfMeasureTo] ASC, [OrganizationId] ASC)
);


GO
create trigger [dbo].[Mc_UnitsOfMeasureConversion_INSERT]
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
end
GO
create trigger [dbo].[Mc_UnitsOfMeasureConversion_DELETE]
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
end