SELECT 
	[RoleId] AS '@id'
	, [Name] AS '@name'
	, [Description] AS '@description'
	, [ShortName] AS '@shortName'
	, [Rank] AS '@rank'
	, [StartActionId] AS '@startPageId' 
FROM [dbo].[Mc_Role] AS r
WHERE Deleted = 0 AND BuiltIn = 0
FOR XML PATH('role'), TYPE, ROOT('roles')
GO
