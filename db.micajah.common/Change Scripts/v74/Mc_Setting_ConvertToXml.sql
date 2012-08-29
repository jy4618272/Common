IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetChildSettings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetChildSettings]
GO

CREATE FUNCTION [dbo].[GetChildSettings]
(
	@SettingId uniqueidentifier
)
RETURNS XML
AS
BEGIN RETURN (
	SELECT 
		[SettingId] AS '@id'
		, CASE
			WHEN [ParentSettingId] IS NULL AND [SettingTypeId] = 1 THEN 'OnOffSwitch'
			WHEN [ParentSettingId] IS NOT NULL AND [SettingTypeId] = 1 THEN 'CheckBox'
			WHEN [SettingTypeId] = 2 THEN 'Value'
			WHEN [SettingTypeId] = 3 THEN 'List'
			ELSE 'NotSet' END AS '@type'
		, [Name] AS '@name'
		, [Description] AS '@description'
		, [ShortName] AS '@shortName'
		, [OrderNumber] AS '@orderNumber'
		,	STUFF(
				(CASE WHEN [EnableOrganization] = 1 THEN ',Organization' ELSE '' END)
				+ (CASE WHEN [EnableInstance] = 1 THEN ',Instance' ELSE '' END)
				+ (CASE WHEN [EnableRole] = 1 OR [EnableGroup] = 1 THEN ',Group' ELSE '' END)
				+ (CASE WHEN [EnableOrganization] = 0 AND [EnableInstance] = 0 AND [EnableRole] = 0 AND [EnableGroup] = 0 THEN ',Global' ELSE '' END)
				, 1, 1, '')
			AS '@levels'
		, CASE 
			WHEN [DefaultValue] = 'True' THEN 'true'
			WHEN [DefaultValue] = 'False' THEN 'false'
			ELSE [DefaultValue] END AS '@defaultValue'
		, CASE WHEN [EnableOrganization] = 0 AND [EnableInstance] = 0 AND [EnableRole] = 0 AND [EnableGroup] = 0
			THEN ISNULL([Value], '') ELSE NULL END AS '@value'
		, (	
			SELECT 
				[Name] AS '@name'
				, [Value] AS '@value'
			FROM [dbo].[Mc_SettingListsValues]
			WHERE [SettingId] = s.[SettingId] AND Deleted = 0
			FOR XML PATH('value'), TYPE, ROOT('values')
		  ) 
		, [dbo].[GetChildSettings]([SettingId])
	FROM [dbo].[Mc_Setting] AS s
	WHERE [ParentSettingId] = @SettingId AND Deleted = 0 AND BuiltIn = 0
	FOR XML PATH('setting'), TYPE, ROOT ('settings')
)
END
GO

SELECT 
	[SettingId] AS '@id'
	, CASE
		WHEN [ParentSettingId] IS NULL AND [SettingTypeId] = 1 THEN 'OnOffSwitch'
		WHEN [ParentSettingId] IS NOT NULL AND [SettingTypeId] = 1 THEN 'CheckBox'
		WHEN [SettingTypeId] = 2 THEN 'Value'
		WHEN [SettingTypeId] = 3 THEN 'List'
		ELSE 'NotSet' END AS '@type'
	, [Name] AS '@name'
	, [Description] AS '@description'
	, [ShortName] AS '@shortName'
	, [OrderNumber] AS '@orderNumber'
	,	STUFF(
			(CASE WHEN [EnableOrganization] = 1 THEN ',Organization' ELSE '' END)
			+ (CASE WHEN [EnableInstance] = 1 THEN ',Instance' ELSE '' END)
			+ (CASE WHEN [EnableRole] = 1 OR [EnableGroup] = 1 THEN ',Group' ELSE '' END)
			+ (CASE WHEN [EnableOrganization] = 0 AND [EnableInstance] = 0 AND [EnableRole] = 0 AND [EnableGroup] = 0 THEN ',Global' ELSE '' END)
			, 1, 1, '')
		AS '@levels'
	, CASE 
		WHEN [DefaultValue] = 'True' THEN 'true'
		WHEN [DefaultValue] = 'False' THEN 'false'
		ELSE [DefaultValue] END AS '@defaultValue'
	, CASE WHEN [EnableOrganization] = 0 AND [EnableInstance] = 0 AND [EnableRole] = 0 AND [EnableGroup] = 0
		THEN ISNULL([Value], '') ELSE NULL END AS '@value'
	, (	
		SELECT 
			[Name] AS '@name'
			, [Value] AS '@value'
		FROM [dbo].[Mc_SettingListsValues]
		WHERE [SettingId] = s.[SettingId] AND Deleted = 0
		FOR XML PATH('value'), TYPE, ROOT('values')
	  ) 
	, [dbo].[GetChildSettings]([SettingId])
FROM [dbo].[Mc_Setting] AS s
WHERE [ParentSettingId] IS NULL AND Deleted = 0 AND BuiltIn = 0
FOR XML PATH('setting'), TYPE, ROOT('settings')
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetChildSettings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetChildSettings]
GO
