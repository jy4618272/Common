BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

declare @emptyId uniqueidentifier;
select @emptyId = '00000000-0000-0000-0000-000000000000';

	-- acre foot
IF @@TRANCOUNT = 1 
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'F1A2A563-BC75-45D8-8581-1CDFE4807B39', @emptyId, 'acre-foot', 'acr-ft', 'acre-feet', 'acr-ft', 'Volume', 'English'
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'F1A2A563-BC75-45D8-8581-1CDFE4807B39', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 1233.48185532, @emptyId;

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1 
BEGIN
	-- acre foot [US survey]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '251A68AE-FE92-40BB-9A28-84C90C1F3360', @emptyId, 'acre-foot', 'acr-ft', 'acre-feet', 'acr-ft', 'Volume', 'US survey'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '251A68AE-FE92-40BB-9A28-84C90C1F3360', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 1233.489, @emptyId;

	-- acre inch
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '38FF4C33-14BE-4F2B-BC57-7906911A0F0C', @emptyId, 'acre-inch', 'acr-in', 'acre-inches', 'acr-ins', 'Volume', 'English'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '38FF4C33-14BE-4F2B-BC57-7906911A0F0C', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 102.79015461, @emptyId;

	-- barrel [UK, wine]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '6968A935-AC1C-4EBF-85C4-B5C806173428', @emptyId, 'barrel', 'barrel', 'barrels', 'barrels', 'Volume', 'UK, wine'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '6968A935-AC1C-4EBF-85C4-B5C806173428', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.143201835, @emptyId;

	-- barrel [UK]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '545A3A8E-F797-4572-8606-A5E84FB88D66', @emptyId, 'barrel', 'barrel', 'barrels', 'barrels', 'Volume', 'UK'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '545A3A8E-F797-4572-8606-A5E84FB88D66', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.16365924, @emptyId;

	-- barrel [US, dry]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'D21EEA3E-0773-49CB-8FFF-1AA7A00BA710', @emptyId, 'barrel', 'barrel', 'barrels', 'barrels', 'Volume', 'US, dry'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'D21EEA3E-0773-49CB-8FFF-1AA7A00BA710', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.11562712407272727272727272727273, @emptyId;

	-- barrel [US, federal]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'D21EEA3E-0773-49CB-8FFF-1AA7A00BA711', @emptyId, 'barrel', 'barrel', 'barrels', 'barrels', 'Volume', 'US, federal'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'D21EEA3E-0773-49CB-8FFF-1AA7A00BA711', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.1173477658, @emptyId;

	-- barrel [US, liquid]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'D21EEA3E-0773-49CB-8FFF-1AA7A00BA712', @emptyId, 'barrel', 'barrel', 'barrels', 'barrels', 'Volume', 'US, liquid'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'D21EEA3E-0773-49CB-8FFF-1AA7A00BA712', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.1192404717, @emptyId;

	-- barrel [US, petroleum]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'D21EEA3E-0773-49CB-8FFF-1AA7A00BA713', @emptyId, 'barrel', 'barrel', 'barrels', 'barrels', 'Volume', 'US, petroleum'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'D21EEA3E-0773-49CB-8FFF-1AA7A00BA713', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.1589872956, @emptyId;

	-- board foot
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'B135DCF4-4F91-4364-8C36-28A2DA39BCDC', @emptyId, 'board foot', 'board foot', 'board feet', 'board feet', 'Volume', 'English'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'B135DCF4-4F91-4364-8C36-28A2DA39BCDC', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.002359737225974025974025974025974, @emptyId;

	-- bucket [UK]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '83079BB0-AA10-43B0-AB34-231D649A5AA4', @emptyId, 'bucket', 'bucket', 'buckets', 'buckets', 'Volume', 'UK'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '83079BB0-AA10-43B0-AB34-231D649A5AA4', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.01818436, @emptyId;

	-- bucket [US]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '83079BB0-AA10-43B0-AB34-231D649A5AA5', @emptyId, 'bucket', 'bucket', 'buckets', 'buckets', 'Volume', 'US'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '83079BB0-AA10-43B0-AB34-231D649A5AA5', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.018927059, @emptyId;

	-- bushel [UK]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '5E72D30C-34CF-48C5-AC32-01CD90BCD9B9', @emptyId, 'bushel', 'bushel', 'bushels', 'bushels', 'Volume', 'UK'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '5E72D30C-34CF-48C5-AC32-01CD90BCD9B9', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.03636872, @emptyId;

	-- bushel [US, dry]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '5E72D30C-34CF-48C5-AC32-01CD90BCD9B8', @emptyId, 'bushel', 'bushel', 'bushels', 'bushels', 'Volume', 'US, dry'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '5E72D30C-34CF-48C5-AC32-01CD90BCD9B8', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.0352390704, @emptyId;

	-- centiliter
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'C73F4862-AC5C-4A04-8CAE-CFC9048B1A6E', @emptyId, 'centiliter', 'centiliter', 'centiliters', 'centiliters', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'C73F4862-AC5C-4A04-8CAE-CFC9048B1A6E', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.00001, @emptyId;

	-- cord foot [timber]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '4B757669-9D5E-45EF-AFE4-E01F33274BCE', @emptyId, 'cord foot', 'cord ft', 'cord feet', 'cord ft', 'Volume', 'Firewood'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '4B757669-9D5E-45EF-AFE4-E01F33274BCE', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.453069552, @emptyId;

	-- cord [firewood]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '90F7277D-F031-4127-AD90-937D40B6CF7A', @emptyId, 'cord', 'cord', 'cords', 'cords', 'Volume', 'Firewood'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '90F7277D-F031-4127-AD90-937D40B6CF7A', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 3.624556416, @emptyId;

	-- cubic centimeter
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '4000CDA8-3E10-4FF3-BDDF-53961C1F8E6A', @emptyId, 'cubic centimeter', 'cubic centimeter', 'cubic centimeters', 'cubic centimeters', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '4000CDA8-3E10-4FF3-BDDF-53961C1F8E6A', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.000001, @emptyId;

	-- cubic cubit [ancient egypt]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'ECEE5BFA-0CA7-4119-9104-AB8FE9855F7E', @emptyId, 'cubic cubit', 'cubic cubit', 'cubic cubits', 'cubic cubits', 'Volume', 'Ancient'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'ECEE5BFA-0CA7-4119-9104-AB8FE9855F7E', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.144, @emptyId;

	-- cubic decimeter
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'F36BAEF3-2BE1-467C-9FCB-C296F229422E', @emptyId, 'cubic decimeter', 'cubic decimeter', 'cubic decimeter', 'cubic decimeter', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'F36BAEF3-2BE1-467C-9FCB-C296F229422E', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.001, @emptyId;

	-- cubic dekameter
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '8FA34151-B1CC-4DEA-879F-6667AED92264', @emptyId, 'cubic dekameter', 'cubic dekameter', 'cubic dekameters', 'cubic dekameters', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '8FA34151-B1CC-4DEA-879F-6667AED92264', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 1000, @emptyId;

	-- cubic foot
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'E15F79CF-7298-4442-8DB8-301AF1C163EC', @emptyId, 'cubic foot', 'cubic ft', 'cubic feet', 'cubic ft', 'Volume', 'English'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'E15F79CF-7298-4442-8DB8-301AF1C163EC', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.028316846711688311688311688311688, @emptyId;

	-- cubic inch
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '9E5F2157-35DD-478D-9DF5-F730F51D910D', @emptyId, 'cubic inch', 'cubic inch', 'cubic inches', 'cubic ins', 'Volume', 'English'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '9E5F2157-35DD-478D-9DF5-F730F51D910D', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 1.6387064069264069264069264069264e-5, @emptyId;

	-- cubic kilometer
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '47FC6301-4475-4FFD-BCB6-90F05B678904', @emptyId, 'cubic kilometer', 'cubic kilometer', 'cubic kilometers', 'cubic kilometers', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '47FC6301-4475-4FFD-BCB6-90F05B678904', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 1.0e+9, @emptyId;

	-- cubic meter
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'A26227E7-B775-4069-AF7F-4ECA92F9E957', @emptyId, 'cubic meter', 'cubic meter', 'cubic meters', 'cubic meters', 'Volume', 'Metric'

	-- cubic micrometer
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '020C737D-44BC-4752-93A9-187561877D98', @emptyId, 'cubic micrometer', 'cubic micrometer', 'cubic micrometers', 'cubic micrometers', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '020C737D-44BC-4752-93A9-187561877D98', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 1.0e-18, @emptyId;

	-- cubic mile
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '61C584BC-4CE7-47E8-AD62-5DC7B88A2CE8', @emptyId, 'cubic mile', 'cubic mile', 'cubic miles', 'cubic miles', 'Volume', 'English'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '61C584BC-4CE7-47E8-AD62-5DC7B88A2CE8', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 4168181843.0584539428571428571429, @emptyId;

	-- cubic millimeter
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'CE174062-7797-4D66-B0A1-220628F9D717', @emptyId, 'cubic millimeter', 'cubic millimeter', 'cubic millimeters', 'cubic millimeters', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'CE174062-7797-4D66-B0A1-220628F9D717', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 1.0e-9, @emptyId;

	-- cubic yard
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '94FCE680-ADB4-4257-BB54-94F209C688DD', @emptyId, 'cubic yard', 'cubic yd', 'cubic yards', 'cubic yds', 'Volume', 'English'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '94FCE680-ADB4-4257-BB54-94F209C688DD', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.76455486121558441558441558441558, @emptyId;

	-- cup [Canada]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '3C93FBFA-6FB7-40BD-8179-F023737C3E00', @emptyId, 'cup', 'cup', 'cups', 'cups', 'Volume', 'Canada'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '3C93FBFA-6FB7-40BD-8179-F023737C3E00', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.0002273045, @emptyId;

	-- cup [metric]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '3C93FBFA-6FB7-40BD-8179-F023737C3E01', @emptyId, 'cup', 'cup', 'cups', 'cups', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '3C93FBFA-6FB7-40BD-8179-F023737C3E01', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.00025, @emptyId;

	-- cup [US]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '3C93FBFA-6FB7-40BD-8179-F023737C3E02', @emptyId, 'cup', 'cup', 'cups', 'cups', 'Volume', 'US'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '3C93FBFA-6FB7-40BD-8179-F023737C3E02', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.0002365882375, @emptyId;

	-- deciliter
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '27E7FECC-0CBE-44C8-AD1C-3AA26E82542A', @emptyId, 'deciliter', 'deciliter', 'deciliters', 'deciliters', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '27E7FECC-0CBE-44C8-AD1C-3AA26E82542A', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.0001, @emptyId;

	-- dekaliter
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'C82B5A86-79CB-4D4B-BEE1-4141314AF6D2', @emptyId, 'dekaliter', 'dekaliter', 'dekaliters', 'dekaliters', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'C82B5A86-79CB-4D4B-BEE1-4141314AF6D2', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.01, @emptyId;

	-- dram
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '40D4BF54-E96D-48AA-90F0-EAABF3B32756', @emptyId, 'dram', 'dram', 'drams', 'drams', 'Volume', 'English'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '40D4BF54-E96D-48AA-90F0-EAABF3B32756', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.0000036966912109375, @emptyId;

	-- drum [metric, petroleum]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '17BE7850-9D75-4D07-953E-80838FB46AE0', @emptyId, 'drum', 'drum', 'drums', 'drums', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '17BE7850-9D75-4D07-953E-80838FB46AE0', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.2, @emptyId;

	-- drum [US, petroleum]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '17BE7850-9D75-4D07-953E-80838FB46AE1', @emptyId, 'drum', 'drum', 'drums', 'drums', 'Volume', 'US'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '17BE7850-9D75-4D07-953E-80838FB46AE1', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.208197649, @emptyId;

	-- fifth
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '3447E42D-6AED-4359-B5FB-21B1C9BF9860', @emptyId, 'fifth', 'fifth', 'fifths', 'fifths', 'Volume', 'US'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '3447E42D-6AED-4359-B5FB-21B1C9BF9860', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.00075708236, @emptyId;

	-- gallon [UK]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '5D74D0AC-A723-4B80-B47F-FEB325720CD0', @emptyId, 'gallon', 'gal', 'gallons', 'gals', 'Volume', 'UK'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '5D74D0AC-A723-4B80-B47F-FEB325720CD0', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.00454609, @emptyId;

	-- gallon [US, dry]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '5D74D0AC-A723-4B80-B47F-FEB325720CD1', @emptyId, 'gallon', 'gal', 'gallons', 'gals', 'Volume', 'US, dry'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '5D74D0AC-A723-4B80-B47F-FEB325720CD1', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.0044048838, @emptyId;

	-- gallon [US, liquid]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '27CF5FE9-957C-46D2-A986-9B9A0F9075A0', @emptyId, 'gallon', 'gal', 'gallons', 'gals', 'Volume', 'US, liquid'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '27CF5FE9-957C-46D2-A986-9B9A0F9075A0', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.0037854118, @emptyId;

	-- gill [UK]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '7880AC64-A423-4DC6-B1D8-60099723BEAD', @emptyId, 'gill', 'gill', 'gills', 'gills', 'Volume', 'UK'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '7880AC64-A423-4DC6-B1D8-60099723BEAD', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.0001420653125, @emptyId;

	-- gill [US]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '7880AC64-A423-4DC6-B1D8-60099723BEAE', @emptyId, 'gill', 'gill', 'gills', 'gills', 'Volume', 'US'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '7880AC64-A423-4DC6-B1D8-60099723BEAE', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.00011829411875, @emptyId;

	-- hectare meter
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'ACDA85A4-D479-40F0-97A8-B08E48B69CEE', @emptyId, 'hectare meter', 'hectare meter', 'hectare meter', 'hectare meter', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'ACDA85A4-D479-40F0-97A8-B08E48B69CEE', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 10000, @emptyId;

	-- hectoliter
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '1E0A6D80-C979-4BDE-BA46-039BF1A21FCD', @emptyId, 'hectoliter', 'hectoliter', 'hectoliters', 'hectoliters', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '1E0A6D80-C979-4BDE-BA46-039BF1A21FCD', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.1, @emptyId;

	-- hogshead [UK]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '0570BD1F-CAD2-4C9D-AB0A-D43D4BE8EDB1', @emptyId, 'hogshead', 'hogshead', 'hogsheads', 'hogsheads', 'Volume', 'UK'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '0570BD1F-CAD2-4C9D-AB0A-D43D4BE8EDB1', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.28640367, @emptyId;

	-- hogshead [US]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '0570BD1F-CAD2-4C9D-AB0A-D43D4BE8EDB2', @emptyId, 'hogshead', 'hogshead', 'hogsheads', 'hogsheads', 'Volume', 'US'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '0570BD1F-CAD2-4C9D-AB0A-D43D4BE8EDB2', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.2384809434, @emptyId;

	-- jigger
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '29DA47D5-C20E-4DE8-9D25-FB02D51A26DB', @emptyId, 'jigger', 'jigger', 'jiggers', 'jiggers', 'Volume', 'US'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '29DA47D5-C20E-4DE8-9D25-FB02D51A26DB', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.00004436029453125, @emptyId;

	-- kiloliter
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '930D2CDC-460F-4358-BA43-C5C23EDEECDA', @emptyId, 'kiloliter', 'kiloliter', 'kiloliters', 'kiloliters', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '930D2CDC-460F-4358-BA43-C5C23EDEECDA', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 1, @emptyId;

	-- liter
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '7B16FA6F-0576-4C8E-9F3E-717DCADE3A27', @emptyId, 'liter', 'liter', 'liters', 'liters', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '7B16FA6F-0576-4C8E-9F3E-717DCADE3A27', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.001, @emptyId;

	-- measure [ancient hebrew]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '90585785-2550-4EE5-8F1D-79D4CF063BB7', @emptyId, 'measure', 'measure', 'measures', 'measures', 'Volume', 'Ancient'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '90585785-2550-4EE5-8F1D-79D4CF063BB7', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.0077, @emptyId;

	-- megaliter
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'FFB5BC8F-9847-4534-91B8-F84BFAFA7C12', @emptyId, 'megaliter', 'megaliter', 'megaliters', 'megaliters', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'FFB5BC8F-9847-4534-91B8-F84BFAFA7C12', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 1000, @emptyId;

	-- microliter
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'A6967934-6C35-46FE-8250-F9EF204CDFE6', @emptyId, 'microliter', 'microliter', 'microliters', 'microliters', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'A6967934-6C35-46FE-8250-F9EF204CDFE6', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 1.0e-9, @emptyId;

	-- milliliter
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'B1534940-19E5-4CEC-95AD-19F1FD1346EF', @emptyId, 'milliliter', 'milliliter', 'milliliters', 'milliliters', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'B1534940-19E5-4CEC-95AD-19F1FD1346EF', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.000001, @emptyId;

	-- minim [UK]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'C1F4A088-7DFD-49BD-B2F3-912CB74DA490', @emptyId, 'minim', 'minim', 'minims', 'minims', 'Volume', 'UK'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'C1F4A088-7DFD-49BD-B2F3-912CB74DA490', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 5.9193880208333333333e-8, @emptyId;

	-- minim [US]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'C1F4A088-7DFD-49BD-B2F3-912CB74DA491', @emptyId, 'minim', 'minim', 'minims', 'minims', 'Volume', 'US'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'C1F4A088-7DFD-49BD-B2F3-912CB74DA491', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 6.1611520182291666666666667e-8, @emptyId;

	-- ounce [UK, liquid]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'E2823D37-F050-4F03-B19B-F9E7E65DBBCF', @emptyId, 'ounce', 'ounce', 'ounces', 'ounces', 'Volume', 'UK, liquid'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'E2823D37-F050-4F03-B19B-F9E7E65DBBCF', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.0000284130625, @emptyId;

	-- ounce [US, liquid]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'E2823D37-F050-4F03-B19B-F9E7E65DBBCE', @emptyId, 'ounce', 'ounce', 'ounces', 'ounces', 'Volume', 'US, liquid'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'E2823D37-F050-4F03-B19B-F9E7E65DBBCE', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.0000295735296875, @emptyId;

	-- peck [UK]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '151F4A82-7EEA-41C8-8BD6-892785E30E92', @emptyId, 'peck', 'peck', 'pecks', 'pecks', 'Volume', 'UK'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '151F4A82-7EEA-41C8-8BD6-892785E30E92', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.00909218, @emptyId;

	-- peck [US]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '151F4A82-7EEA-41C8-8BD6-892785E30E93', @emptyId, 'peck', 'peck', 'pecks', 'pecks', 'Volume', 'US'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '151F4A82-7EEA-41C8-8BD6-892785E30E93', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.0088097676, @emptyId;

	-- pint [UK]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'FC04A8BA-2780-464F-8919-BEE977C19DEF', @emptyId, 'pint', 'pints', 'pt', 'pts', 'Volume', 'UK'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'FC04A8BA-2780-464F-8919-BEE977C19DEF', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.00056826125, @emptyId;

	-- pint [US, dry]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'FC04A8BA-2780-464F-8919-BEE977C19DEE', @emptyId, 'pint', 'pints', 'pt', 'pts', 'Volume', 'US, dry'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'FC04A8BA-2780-464F-8919-BEE977C19DEE', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.000550610475, @emptyId;

	-- pint [US, liquid]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'EF22DB7D-3244-464F-B7D0-03CDB527B490', @emptyId, 'pint', 'pints', 'pt', 'pts', 'Volume', 'US, liquid'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'EF22DB7D-3244-464F-B7D0-03CDB527B490', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.000473176475, @emptyId;

	-- pipe [UK]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'A2058AD2-8053-4AAA-A947-1F5B615E38E7', @emptyId, 'pipe', 'pipes', 'pipe', 'pipes', 'Volume', 'UK'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'A2058AD2-8053-4AAA-A947-1F5B615E38E7', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.49097772, @emptyId;

	-- pipe [US]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'A2058AD2-8053-4AAA-A947-1F5B615E38E8', @emptyId, 'pipe', 'pipes', 'pipe', 'pipes', 'Volume', 'US'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'A2058AD2-8053-4AAA-A947-1F5B615E38E8', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.4769618868, @emptyId;

	-- pony
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '620FAD74-C560-4466-8585-CB83FB37B259', @emptyId, 'pony', 'pony', 'ponies', 'ponies', 'Volume', 'English'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '620FAD74-C560-4466-8585-CB83FB37B259', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.0000295735296875, @emptyId;

	-- quart [ancient hebrew]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'F9AEC75D-3CBC-4B7E-A3DA-416CFF644341', @emptyId, 'quart', 'qrt', 'quarts', 'qrts', 'Volume', 'Ancient'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'F9AEC75D-3CBC-4B7E-A3DA-416CFF644341', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.00108, @emptyId;

	-- quart [Germany]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'F9AEC75D-3CBC-4B7E-A3DA-416CFF644342', @emptyId, 'quart', 'qrt', 'quarts', 'qrts', 'Volume', 'Germany'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'F9AEC75D-3CBC-4B7E-A3DA-416CFF644342', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.00114504, @emptyId;

	-- quart [UK]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'F9AEC75D-3CBC-4B7E-A3DA-416CFF644343', @emptyId, 'quart', 'qrt', 'quarts', 'qrts', 'Volume', 'UK'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'F9AEC75D-3CBC-4B7E-A3DA-416CFF644343', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.0011365225, @emptyId;

	-- quart [US, dry]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'F9AEC75D-3CBC-4B7E-A3DA-416CFF644344', @emptyId, 'quart', 'qrt', 'quarts', 'qrts', 'Volume', 'US, dry'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'F9AEC75D-3CBC-4B7E-A3DA-416CFF644344', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.00110122095, @emptyId;

	-- quart [US, liquid]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'B104173D-E539-4022-94C3-3F7ABBCBEB73', @emptyId, 'quart', 'qrt', 'quarts', 'qrts', 'Volume', 'US, liquid'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'B104173D-E539-4022-94C3-3F7ABBCBEB73', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.00094635295, @emptyId;

	-- quarter [UK, liquid]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '17F2B5DB-3439-4483-BD47-DFF378EA094A', @emptyId, 'quarter', 'quarters', 'quarter', 'quarters', 'Volume', 'UK, liquid'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '17F2B5DB-3439-4483-BD47-DFF378EA094A', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.29094976, @emptyId;

	-- shot
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'B737CAD1-2CAD-40A2-B9CF-1CF2E1CF5B8C', @emptyId, 'shot', 'shot', 'shots', 'shots', 'Volume', 'English'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'B737CAD1-2CAD-40A2-B9CF-1CF2E1CF5B8C', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.0000295735296875, @emptyId;

	-- stere
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT 'A5401CE7-80AF-4B56-B04D-B7EB834300D4', @emptyId, 'stere', 'stere', 'steres', 'steres', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'A5401CE7-80AF-4B56-B04D-B7EB834300D4', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 1, @emptyId;

	-- Tablespoon [metric]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '7BE6545A-482D-4A5E-9160-DB62D07BE2F1', @emptyId, 'Tablespoon', 'Tablespoons', 'Tablespoon', 'Tablespoon', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '7BE6545A-482D-4A5E-9160-DB62D07BE2F1', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.000015, @emptyId;

	-- Tablespoon [UK]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '7BE6545A-482D-4A5E-9160-DB62D07BE2F2', @emptyId, 'Tablespoon', 'Tablespoons', 'Tablespoon', 'Tablespoon', 'Volume', 'UK'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '7BE6545A-482D-4A5E-9160-DB62D07BE2F2', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.00001420653125, @emptyId;

	-- Tablespoon [US]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '7BE6545A-482D-4A5E-9160-DB62D07BE2F3', @emptyId, 'Tablespoon', 'Tablespoons', 'Tablespoon', 'Tablespoon', 'Volume', 'US'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '7BE6545A-482D-4A5E-9160-DB62D07BE2F3', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.00001478676484375, @emptyId;

	-- Teaspoon [metric]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '85DB147C-D5E6-49B2-B7AC-A16DFC0A8397', @emptyId, 'Teaspoon', 'Teaspoons', 'Teaspoon', 'Teaspoons', 'Volume', 'Metric'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '85DB147C-D5E6-49B2-B7AC-A16DFC0A8397', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.000005, @emptyId;

	-- Teaspoon [UK]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '85DB147C-D5E6-49B2-B7AC-A16DFC0A8398', @emptyId, 'Teaspoon', 'Teaspoons', 'Teaspoon', 'Teaspoons', 'Volume', 'UK'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '85DB147C-D5E6-49B2-B7AC-A16DFC0A8398', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 0.0000035516328125, @emptyId;

	-- Teaspoon [US]
	INSERT INTO [Mc_UnitsOfMeasure]([UnitsOfMeasureId], [OrganizationId], [SingularName], [SingularAbbrv], [PluralName], [PluralAbbrv], [GroupName], [LocalName])
	SELECT '85DB147C-D5E6-49B2-B7AC-A16DFC0A8399', @emptyId, 'Teaspoon', 'Teaspoons', 'Teaspoon', 'Teaspoons', 'Volume', 'US'
	INSERT INTO [Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '85DB147C-D5E6-49B2-B7AC-A16DFC0A8399', 'A26227E7-B775-4069-AF7F-4ECA92F9E957', 4.9289216145833333333333333333333e-6, @emptyId;

	-- centigram
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT 'EE89AA08-2AD9-44CB-98CF-158A2B3F361D', @emptyId, 'centigram', 'centigram', 'centigrams', 'centigrams', 'Weight', 'Metric';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'EE89AA08-2AD9-44CB-98CF-158A2B3F361D', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 0.00001, @emptyId;

	-- decigram
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT '63DEBA68-4A3D-46B8-BB22-EF4A58EDB3ED', @emptyId, 'decigram', 'decigram', 'decigrams', 'decigrams', 'Weight', 'Metric';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '63DEBA68-4A3D-46B8-BB22-EF4A58EDB3ED', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 0.0001, @emptyId;

	-- dekagram
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT 'CA32A145-197D-4C07-BAA7-BEA0E0119397', @emptyId, 'dekagram', 'dekagram', 'dekagrams', 'dekagrams', 'Weight', 'Metric';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'CA32A145-197D-4C07-BAA7-BEA0E0119397', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 0.01, @emptyId;

	-- dram
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT '51947B1B-43A4-4E50-8289-DAEFF5A42E57', @emptyId, 'dram', 'dr', 'drams', 'dr', 'Weight', 'English';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '51947B1B-43A4-4E50-8289-DAEFF5A42E57', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 0.0017718451953125, @emptyId;

	-- grain
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT '9E262CB8-FD40-4AB0-B41C-7801F0081E4E', @emptyId, 'grain', 'gr', 'grains', 'gr', 'Weight', 'English';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '9E262CB8-FD40-4AB0-B41C-7801F0081E4E', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 0.00006479891, @emptyId;

	-- gram
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT 'E476BEDF-EBBB-4319-AB6E-F4CA818837F9', @emptyId, 'gram', 'g', 'grams', 'g', 'Weight', 'Metric';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'E476BEDF-EBBB-4319-AB6E-F4CA818837F9', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 0.001, @emptyId;

	-- hectogram
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT '4BA29AB5-9C74-4352-B425-C1C567B539CD', @emptyId, 'hectogram', 'hectogram', 'hectograms', 'hectograms', 'Weight', 'Metric';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '4BA29AB5-9C74-4352-B425-C1C567B539CD', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 0.1, @emptyId;

	-- hundredweight [long, UK]
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT 'F052BF9D-40A7-40AD-B82E-60CB8D89E17D', @emptyId, 'hundredweight', 'cwt', 'hundredweights', 'cwt', 'Weight', 'Long';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'F052BF9D-40A7-40AD-B82E-60CB8D89E17D', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 50.80234544, @emptyId;

	-- hundredweight [short, US]
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT 'E280A434-4197-4FC6-B08C-EA5CFACAE7C4', @emptyId, 'hundredweight', 'cwt', 'hundredweights', 'cwt', 'Weight', 'Short';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'E280A434-4197-4FC6-B08C-EA5CFACAE7C4', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 45.359237, @emptyId;

	-- kilogram
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT '91D02BBE-8C25-4F78-9F29-09CA8B27C173', @emptyId, 'kilogram', 'kg', 'kilograms', 'kg', 'Weight', 'Metric';

	-- long ton
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT 'C888E4C6-5DEC-4DD0-9B8C-63B1115F0EB7', @emptyId, 'ton', 'ton', 'tons', 'tons', 'Weight', 'Long';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'C888E4C6-5DEC-4DD0-9B8C-63B1115F0EB7', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 1016.0469088, @emptyId;

	-- megagram
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT 'C9F8A097-342D-41DC-8D60-B9C9482D07DF', @emptyId, 'megagram', 'Mg', 'megagrams', 'Mg', 'Weight', 'Metric';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'C9F8A097-342D-41DC-8D60-B9C9482D07DF', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 1000, @emptyId;

	-- metric ton
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT 'D8B5FF5E-E1BF-4AAC-A441-81B8321CDF26', @emptyId, 'ton', 'ton', 'tons', 'tons', 'Weight', 'Metric';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'D8B5FF5E-E1BF-4AAC-A441-81B8321CDF26', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 1000, @emptyId;

	-- microgram
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT '5D122E6F-A1F1-47F6-AA23-57081C1F4F65', @emptyId, 'microgram', 'microgram', 'micrograms', 'micrograms', 'Weight', 'Metric';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '5D122E6F-A1F1-47F6-AA23-57081C1F4F65', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 1.0e-9, @emptyId;

	-- milligram
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT '27F396A9-56C7-4BEC-B978-0A48F0D4FE1F', @emptyId, 'milligram', 'mg', 'milligram', 'mg', 'Weight', 'Metric';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '27F396A9-56C7-4BEC-B978-0A48F0D4FE1F', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 0.000001, @emptyId;

	-- ounce
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT 'FC3AA808-F1C5-4A61-AFA0-1C374BD2E27F', @emptyId, 'ounce', 'oz', 'ounces', 'oz', 'Weight', 'English';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'FC3AA808-F1C5-4A61-AFA0-1C374BD2E27F', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 0.028349523125, @emptyId;

	-- pound
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT '38BB9678-9F0C-4F50-B258-32C52DCE189D', @emptyId, 'pound', 'lb', 'pounds', 'lb', 'Weight', 'English';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '38BB9678-9F0C-4F50-B258-32C52DCE189D', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 0.45359237, @emptyId;

	-- stone
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT '993B8791-78DA-474E-9EF0-04603F4242B1', @emptyId, 'stone', 'stones', 'stone', 'stones', 'Weight', 'English';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '993B8791-78DA-474E-9EF0-04603F4242B1', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 6.35029318, @emptyId;

	-- ton [short]
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT '4ECEA6A3-26AF-47A4-BC4E-8CF9A871AAEE', @emptyId, 'ton', 'tons', 'ton', 'tons', 'Weight', 'Short';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '4ECEA6A3-26AF-47A4-BC4E-8CF9A871AAEE', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 907.18474, @emptyId;

	-- ton [long]
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT 'B4FF9A04-CC67-411F-809C-BC98716AF4FA', @emptyId, 'ton', 'tons', 'ton', 'tons', 'Weight', 'Long';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'B4FF9A04-CC67-411F-809C-BC98716AF4FA', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 1016.0469088, @emptyId;

	-- ton [metric]
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT '38B4AEF6-850E-43D6-A28D-21CCB59A240B', @emptyId, 'tonne', 't', 'tonnes', 't', 'Weight', 'Metric';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '38B4AEF6-850E-43D6-A28D-21CCB59A240B', '91D02BBE-8C25-4F78-9F29-09CA8B27C173', 1000, @emptyId;

	-- centimeter
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT '702EE05A-ADC9-43EB-9371-21749A0BB373', @emptyId, 'centimeter', 'cm', 'centimeters', 'cms', 'Length', 'Metric';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '702EE05A-ADC9-43EB-9371-21749A0BB373', 'F9B6D4F8-9455-4364-9650-F35F475AF301', 0.01, @emptyId;

	-- feet
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT '311D01C6-2DAA-48C7-AF80-1643EEF930E9', @emptyId, 'foot', 'ft', 'feet', 'ft', 'Length', 'English';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '311D01C6-2DAA-48C7-AF80-1643EEF930E9', 'F9B6D4F8-9455-4364-9650-F35F475AF301', 0.3048, @emptyId;

	-- inch
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT '67AC9A2E-4714-4E53-8392-F923D65A9DD1', @emptyId, 'inch', 'in', 'inches', 'ins', 'Length', 'English';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '67AC9A2E-4714-4E53-8392-F923D65A9DD1', 'F9B6D4F8-9455-4364-9650-F35F475AF301', 0.0254, @emptyId;

	-- kilometer
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT '18A283BD-528E-4693-A702-0C62BAE944B4', @emptyId, 'kilometer', 'km', 'kilometers', 'kms', 'Length', 'Metric';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '18A283BD-528E-4693-A702-0C62BAE944B4', 'F9B6D4F8-9455-4364-9650-F35F475AF301', 1000, @emptyId;

	-- league
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT '2B4A4277-22F6-4942-8BAB-918F00706EED', @emptyId, 'league', 'league', 'leagues', 'leagues', 'Length', 'English';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '2B4A4277-22F6-4942-8BAB-918F00706EED', 'F9B6D4F8-9455-4364-9650-F35F475AF301', 4828.0417, @emptyId;

	-- league [nautical]
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT '38C7DF31-A6C6-473F-9ABE-A5A0DA2FA40C', @emptyId, 'league', 'league', 'leagues', 'leagues', 'Length', 'Nautical';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '38C7DF31-A6C6-473F-9ABE-A5A0DA2FA40C', 'F9B6D4F8-9455-4364-9650-F35F475AF301', 5556, @emptyId;

	-- meter
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT 'F9B6D4F8-9455-4364-9650-F35F475AF301', @emptyId, 'meter', 'm', 'meters', 'm', 'Length', 'Metric';

	-- microinch
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT '60794F1B-DCB4-4BD5-B7E8-DCC3B50C79EC', @emptyId, 'microinch', 'microinch', 'microinches', 'microinch', 'Length', 'English';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '60794F1B-DCB4-4BD5-B7E8-DCC3B50C79EC', 'F9B6D4F8-9455-4364-9650-F35F475AF301', 2.54e-08, @emptyId;

	-- mile
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT 'E7B02CB9-3EE5-40A3-BF44-D1389C185A06', @emptyId, 'mile', 'mi', 'miles', 'mi', 'Length', 'English';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'E7B02CB9-3EE5-40A3-BF44-D1389C185A06', 'F9B6D4F8-9455-4364-9650-F35F475AF301', 1609.344, @emptyId;

	-- millimeter
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT '55AA2781-062B-4427-BEF4-0F6176BFB093', @emptyId, 'millimeter', 'mm', 'millimeters', 'mm', 'Length', 'Metric';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT '55AA2781-062B-4427-BEF4-0F6176BFB093', 'F9B6D4F8-9455-4364-9650-F35F475AF301', 0.001, @emptyId;

	-- yard
	INSERT INTO [dbo].[Mc_UnitsOfMeasure]([UnitsOfMeasureId],[OrganizationId],[SingularName],[SingularAbbrv],[PluralName],[PluralAbbrv],[GroupName],[LocalName])
	SELECT 'EAE5B378-A006-4000-A475-731E87B06666', @emptyId, 'yard', 'yd', 'yard', 'yd', 'Length', 'English';
	INSERT INTO [dbo].[Mc_UnitsOfMeasureConversion]([UnitOfMeasureFrom], [UnitOfMeasureTo], [Factor], [OrganizationId])
	SELECT 'EAE5B378-A006-4000-A475-731E87B06666', 'F9B6D4F8-9455-4364-9650-F35F475AF301', 0.9144, @emptyId;
END

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
