IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NonBuiltInChildActionsCount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[NonBuiltInChildActionsCount]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetActionRoles]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetActionRoles]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetActionControls]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetActionControls]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ActionsView]'))
DROP VIEW [dbo].[ActionsView]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IsOrgSettingsPage]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[IsOrgSettingsPage]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetChildActions]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetChildActions]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetActionParentActions]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetActionParentActions]
GO

CREATE FUNCTION [dbo].[NonBuiltInChildActionsCount]
(
	@ActionId uniqueidentifier
)
RETURNS int
AS
BEGIN RETURN (
	ISNULL((SELECT SUM(ABS(BuiltIn - 1)) AS c1
			FROM [dbo].[Mc_Action]
			WHERE [ParentActionId] = @ActionId AND Deleted = 0 AND ActionTypeId = 1), 0)
	+ ISNULL(
		(	SELECT SUM(sum1)
			FROM (
				SELECT ISNULL([dbo].[NonBuiltInChildActionsCount]([ActionId]), 0) AS sum1
				FROM [dbo].[Mc_Action]
				WHERE [ParentActionId] = @ActionId AND Deleted = 0 AND ActionTypeId = 1
			) AS rs
		)
	, 0)
)
END
GO

CREATE FUNCTION [dbo].[GetActionRoles]
(
	@ActionId uniqueidentifier
)
RETURNS XML
AS
BEGIN RETURN (
	REPLACE(ISNULL((SELECT r.ShortName AS 'data()'
		FROM dbo.Mc_RolesActions AS ra
		INNER JOIN dbo.Mc_Role AS r
			ON	r.RoleId = ra.RoleId
		WHERE ra.ActionId = @ActionId AND r.Deleted = 0
		FOR XML PATH('')), '')
	, ' ', ',')
)
END
GO

CREATE FUNCTION [dbo].[GetActionControls]
(
	@ActionId uniqueidentifier
)
RETURNS XML
AS
BEGIN RETURN (
	SELECT
		[ActionId] AS '@id' 
		, 'Control' AS '@type'
		, [Name] AS '@name'
		, [Description] AS '@description'
		, [ClassFullName]  AS '@classFullName'
		, (SELECT [dbo].[GetActionRoles](c.ActionId) FOR XML PATH('')) AS '@roles'
	FROM [dbo].[Mc_Action] AS c
	WHERE [ParentActionId] = @ActionId AND Deleted = 0 AND ActionTypeId = 2
	FOR XML PATH('action'), TYPE, ROOT('actions')
)
END
GO

CREATE VIEW [dbo].[ActionsView]
AS
	SELECT [ActionId]
		, CASE WHEN [ParentActionId] = '00000000-0000-0000-0000-000000000015' THEN '00000000-0000-0000-0000-000000000009' ELSE [ParentActionId] END AS [ParentActionId]
		, [ActionTypeId]
		, [Name]
		, [Description]
		, [IconUrl]
		, [ButtonIconUrl]
		, [NavigateUrl]
		, [OrderNumber]
		, [ClassFullName]
		, [AuthenticationRequired]
		, [InstanceRequired]
		, [Visible]
		, [ShowInDetailMenu]
		, [ShowChildrenInDetailMenu]
		, [GroupInDetailMenu]
		, [HighlightInDetailMenu]
		, [BuiltIn] 
		, [Deleted]
		, CASE WHEN [ActionId] = '00000000-0000-0000-0000-000000000009' 
			THEN [dbo].[NonBuiltInChildActionsCount]([ActionId]) + [dbo].[NonBuiltInChildActionsCount]('00000000-0000-0000-0000-000000000015')
			ELSE [dbo].[NonBuiltInChildActionsCount]([ActionId]) END AS [NonBuiltInChildActionsCount]
	FROM [dbo].[Mc_Action]
	WHERE [ActionId] <> '00000000-0000-0000-0000-000000000015' 
GO

CREATE FUNCTION [dbo].[IsOrgSettingsPage]
(
	@ActionId uniqueidentifier
)
RETURNS bit
AS
BEGIN RETURN (
	CASE WHEN @ActionId = '00000000-0000-0000-0000-000000000009' 
		THEN 1 
		ELSE ISNULL((SELECT [dbo].[IsOrgSettingsPage] ([ParentActionId]) FROM [dbo].[ActionsView] WHERE [ActionId] = @ActionId), 0) END
)
END
GO

CREATE FUNCTION [dbo].[GetActionParentActions]
(
	@ActionId uniqueidentifier
)
RETURNS XML
AS
BEGIN RETURN (
	REPLACE(ISNULL((SELECT apa.ParentActionId AS 'data()'
		FROM dbo.Mc_ActionsParentActions AS apa
		INNER JOIN dbo.Mc_Action AS a
			ON	apa.ParentActionId = a.ActionId
		WHERE apa.ActionId = @ActionId AND a.Deleted = 0
		FOR XML PATH('')), '')
	, ' ', ',')
)
END
GO

CREATE FUNCTION [dbo].[GetChildActions]
(
	@ActionId uniqueidentifier
)
RETURNS XML
AS
BEGIN RETURN (
	CASE WHEN @ActionId = '00000000-0000-0000-0000-000000000007'
	THEN (
		SELECT 
			[ActionId] AS '@id'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				'Page'
			  END 
			  AS '@type'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				[Name]
			  END 
			  AS '@name'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				[Description] 
			  END 
			  AS '@description'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				ISNULL([NavigateUrl], 'null') 
			  END 
			  AS '@navigateUrl'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				[OrderNumber] 
			  END 
			  AS '@orderNumber'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				[ClassFullName] 
			  END 
			  AS '@classFullName'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				(CASE WHEN [AuthenticationRequired] = 1 THEN 'true' ELSE 'false' END) 
			  END
			  AS '@authenticationRequired'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				(CASE WHEN [InstanceRequired] = 1 THEN 'true' ELSE 'false' END)
			  END 
			  AS '@instanceRequired'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				(CASE WHEN [Visible] = 1 THEN 'true' ELSE 'false' END)
			  END 
			  AS '@visible'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				(	CASE WHEN [dbo].[IsOrgSettingsPage] (a.ActionId) = 1 
						THEN 'OrgAdmin' + (CASE WHEN [InstanceRequired] = 1 THEN ',InstAdmin' ELSE '' END)
						ELSE (SELECT [dbo].[GetActionRoles](a.ActionId) FOR XML PATH(''))
						END
				)
			  END 
			  AS '@roles'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				(SELECT [dbo].[GetActionParentActions](a.ActionId) FOR XML PATH('')) 
			  END 
			  AS '@alternativeParents'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				''
			  END 
			  AS '@learnMoreUrl'
			-- detailMenu
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				(CASE WHEN [ShowInDetailMenu] = 1 THEN 'true' ELSE 'false' END) 
			  END 
			  AS 'detailMenu/@show'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				(CASE WHEN [ShowChildrenInDetailMenu] = 1 THEN 'true' ELSE 'false' END) 
			  END 
			  AS 'detailMenu/@showChildren'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				'true'
			  END 
			  AS 'detailMenu/@showDescription'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				(CASE WHEN [GroupInDetailMenu] = 1 THEN 'true' ELSE 'false' END) 
			  END 
			  AS 'detailMenu/@group'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				(CASE WHEN [HighlightInDetailMenu] = 1 THEN 'true' ELSE 'false' END) 
			  END 
			  AS 'detailMenu/@highlight'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				[IconUrl] 
			  END 
			  AS 'detailMenu/@iconUrl'
			-- submenu
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				CASE WHEN LEN(ISNULL([ButtonIconUrl], '')) > 0 THEN 'ImageButton' ELSE 'Link' END
			  END 
			  AS 'submenu/@itemType'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				[ButtonIconUrl] 
			  END 
			  AS 'submenu/@imageUrl'
			-- controls
			, [dbo].[GetActionControls]([ActionId]) 
			-- child pages
			, [dbo].[GetChildActions]([ActionId])
		FROM [dbo].[ActionsView] AS a
		WHERE [ParentActionId] = @ActionId AND ActionTypeId = 1 AND Deleted = 0
			AND (BuiltIn = 0 OR (BuiltIn = 1 AND [NonBuiltInChildActionsCount] > 0))
		ORDER BY 6, 3
		FOR XML PATH('action'), TYPE
		)
	ELSE (
		SELECT 
			[ActionId] AS '@id'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				'Page'
			  END 
			  AS '@type'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				[Name]
			  END 
			  AS '@name'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				[Description] 
			  END 
			  AS '@description'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				ISNULL([NavigateUrl], 'null') 
			  END 
			  AS '@navigateUrl'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				[OrderNumber] 
			  END 
			  AS '@orderNumber'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				[ClassFullName] 
			  END 
			  AS '@classFullName'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				(CASE WHEN [AuthenticationRequired] = 1 THEN 'true' ELSE 'false' END) 
			  END
			  AS '@authenticationRequired'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				(CASE WHEN [InstanceRequired] = 1 THEN 'true' ELSE 'false' END)
			  END 
			  AS '@instanceRequired'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				(CASE WHEN [Visible] = 1 THEN 'true' ELSE 'false' END)
			  END 
			  AS '@visible'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				(	CASE WHEN [dbo].[IsOrgSettingsPage] (a.ActionId) = 1 
						THEN 'OrgAdmin' + (CASE WHEN [InstanceRequired] = 1 THEN ',InstAdmin' ELSE '' END)
						ELSE (SELECT [dbo].[GetActionRoles](a.ActionId) FOR XML PATH(''))
						END
				)
			  END 
			  AS '@roles'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				(SELECT [dbo].[GetActionParentActions](a.ActionId) FOR XML PATH('')) 
			  END 
			  AS '@alternativeParents'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				''
			  END 
			  AS '@learnMoreUrl'
			-- detailMenu
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				(CASE WHEN [ShowInDetailMenu] = 1 THEN 'true' ELSE 'false' END) 
			  END 
			  AS 'detailMenu/@show'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				(CASE WHEN [ShowChildrenInDetailMenu] = 1 THEN 'true' ELSE 'false' END) 
			  END 
			  AS 'detailMenu/@showChildren'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				'true'
			  END 
			  AS 'detailMenu/@showDescription'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				(CASE WHEN [GroupInDetailMenu] = 1 THEN 'true' ELSE 'false' END) 
			  END 
			  AS 'detailMenu/@group'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				(CASE WHEN [HighlightInDetailMenu] = 1 THEN 'true' ELSE 'false' END) 
			  END 
			  AS 'detailMenu/@highlight'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				[IconUrl] 
			  END 
			  AS 'detailMenu/@iconUrl'
			-- submenu
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				CASE WHEN LEN(ISNULL([ButtonIconUrl], '')) > 0 THEN 'ImageButton' ELSE 'Link' END
			  END 
			  AS 'submenu/@itemType'
			, CASE WHEN [NonBuiltInChildActionsCount] > 0 AND BuiltIn = 1 THEN NULL ELSE 
				[ButtonIconUrl] 
			  END 
			  AS 'submenu/@imageUrl'
			-- controls
			, [dbo].[GetActionControls]([ActionId]) 
			-- child pages
			, [dbo].[GetChildActions]([ActionId])
		FROM [dbo].[ActionsView] AS a
		WHERE [ParentActionId] = @ActionId AND ActionTypeId = 1 AND Deleted = 0
			AND (BuiltIn = 0 OR (BuiltIn = 1 AND [NonBuiltInChildActionsCount] > 0))
		ORDER BY 6, 3
		FOR XML PATH('action'), TYPE, ROOT('actions')
	) END
)
END
GO

DECLARE @Xml XML

SET @Xml = (
	SELECT 
		[ActionId] AS '@id'
		, 'GlobalNavigationLink' AS '@type'
		, [Name] AS '@name'
		, [Description] AS '@description'
		, ISNULL([NavigateUrl], 'null') AS '@navigateUrl'
		, [OrderNumber] AS '@orderNumber'
		, [ClassFullName]  AS '@classFullName'
		, (CASE WHEN [AuthenticationRequired] = 1 THEN 'true' ELSE 'false' END) AS '@authenticationRequired'
		, (CASE WHEN [InstanceRequired] = 1 THEN 'true' ELSE 'false' END) AS '@instanceRequired'
		, (CASE WHEN [Visible] = 1 THEN 'true' ELSE 'false' END) AS '@visible'
		, (SELECT [dbo].[GetActionRoles](a.ActionId) FOR XML PATH('')) AS '@roles'
		, ''
		-- detailMenu
		, '' 
		, '' 
		, '' 
		, '' 
		, '' 
		, ''
		, ''
		-- submenu
		, '' 
		-- controls
		, ''
		-- child pages
		, ''
	FROM [dbo].[ActionsView] AS a
	WHERE ([ParentActionId] = '00000000-0000-0000-0000-000000000001' AND Deleted = 0 AND BuiltIn = 0) OR (ActionTypeId = 5)
	ORDER BY 6, 3
	FOR XML PATH('action'), TYPE--, ROOT('actions')
)

SELECT
	[dbo].[GetChildActions]([ActionId])
FROM [dbo].[ActionsView] AS a
WHERE [ActionId] = '00000000-0000-0000-0000-000000000007' AND Deleted = 0
	AND (BuiltIn = 0 OR (BuiltIn = 1 AND [NonBuiltInChildActionsCount] > 0))
UNION ALL
SELECT
	@Xml
FOR XML PATH(''), TYPE, ROOT('actions')
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetActionParentActions]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetActionParentActions]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetChildActions]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetChildActions]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ActionsView]'))
DROP VIEW [dbo].[ActionsView]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IsOrgSettingsPage]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[IsOrgSettingsPage]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetActionControls]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetActionControls]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetActionRoles]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetActionRoles]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NonBuiltInChildActionsCount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[NonBuiltInChildActionsCount]
GO
