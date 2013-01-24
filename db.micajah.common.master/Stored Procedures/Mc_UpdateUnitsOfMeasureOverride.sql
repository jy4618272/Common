CREATE PROCEDURE [dbo].[Mc_UpdateUnitsOfMeasureOverride]
(
	@UnitsOfMeasureId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	declare @emptyId uniqueidentifier;
	select @emptyId = '00000000-0000-0000-0000-000000000000';

	if not exists(select 'true' 
		from [dbo].[Mc_UnitsOfMeasure]
		where [UnitsOfMeasureId] = @UnitsOfMeasureId
		and OrganizationId = @emptyId)
	return;
	
	if not exists(select 'true' 
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
