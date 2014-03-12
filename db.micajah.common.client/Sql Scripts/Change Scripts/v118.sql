BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP PROCEDURE [dbo].[Mc_GetAllEntityNodesRelatedEntityNodes];

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP PROCEDURE [dbo].[Mc_GetAllRuleParameters];

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP PROCEDURE [dbo].[Mc_GetAllRules];

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP PROCEDURE [dbo].[Mc_GetSettingsValues];

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER procedure [dbo].[Mc_GetRecurringScheduleByEntityType]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier = null,
	@LocalEntityType nvarchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [RecurringScheduleId]
		  ,[OrganizationId]
		  ,[InstanceId]
		  ,[LocalEntityType]
		  ,[LocalEntityId]
		  ,[Name]
		  ,[StartDate]
		  ,[EndDate]
		  ,[RecurrenceRule]
		  ,[UpdatedTime]
		  ,[UpdatedBy]
		  ,[Deleted]
	FROM [Mc_RecurringSchedule] AS RS
	WHERE RS.OrganizationId = @OrganizationId AND RS.Deleted = 0
		AND (@InstanceId IS NULL OR RS.InstanceId IS NULL OR RS.InstanceId = @InstanceId)
		AND RS.LocalEntityType = @LocalEntityType;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER procedure [dbo].[Mc_GetRecurringScheduleEntityTypes]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier = NULL
)
AS
BEGIN
	SET NOCOUNT OFF;

	DECLARE @Guid uniqueidentifier, @Date datetime;

	SET @Guid = ''00000000-0000-0000-0000-000000000000'';
	SET @Date = GETUTCDATE();

	SELECT DISTINCT
		@Guid AS [RecurringScheduleId], @OrganizationId AS [OrganizationId], @InstanceId AS [InstanceId], [LocalEntityType], N'''' AS [LocalEntityId], N'''' AS [Name]
		, @Date AS [StartDate], @Date AS [EndDate], N'''' AS [RecurrenceRule], @Date AS [UpdatedTime], @Guid AS [UpdatedBy], 0 AS [Deleted]
	FROM [Mc_RecurringSchedule]
	WHERE OrganizationId = @OrganizationId AND Deleted = 0
		AND (@InstanceId IS NULL OR InstanceId IS NULL OR InstanceId = @InstanceId)
	ORDER BY [LocalEntityType];
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetEntityNodesByType]
(
	@EntityNodeTypeId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT EntityNodeId, ParentEntityNodeId, [Name], OrderNumber, OrganizationId, InstanceId, EntityId, EntityNodeTypeId, SubEntityId, SubEntityLocalId, FullPath, Deleted
	FROM [dbo].[Mc_EntityNode]
	WHERE (EntityNodeTypeId = @EntityNodeTypeId) AND (Deleted = 0)
	ORDER BY [Name];
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetEntityNodesRelatedEntityNodesByEntityNodeIdEntityId]
(
	@EntityNodeId uniqueidentifier,
	@EntityId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [EntityNodesRelatedEntityNodesId], enren.[EntityNodeId], [RelatedEntityNodeId], enren.[EntityId], [RelationType]
	FROM [Mc_EntityNodesRelatedEntityNodes] AS enren
	LEFT JOIN Mc_EntityNode AS en 
		ON en.EntityNodeId = enren.RelatedEntityNodeId
	WHERE enren.EntityNodeId = @EntityNodeId
		AND enren.EntityId = @EntityId AND (en.Deleted = 0 OR RelatedEntityNodeId = ''00000000-0000-0000-0000-000000000000'');
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetGroup]
(
	@GroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT GroupId, OrganizationId, [Name], [Description], BuiltIn, Deleted 
	FROM dbo.Mc_Group 
	WHERE (GroupId = @GroupId) AND (Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetGroupByName]
(
	@Name nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT GroupId, OrganizationId, [Name], [Description], BuiltIn, Deleted 
	FROM dbo.Mc_Group 
	WHERE (Name = @Name) AND (Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetGroupsByRoles]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Roles varchar(1024)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT DISTINCT g.GroupId, g.OrganizationId, g.[Name], g.[Description], g.BuiltIn, g.Deleted 
	FROM dbo.Mc_Group AS g
	INNER JOIN dbo.Mc_GroupsInstancesRoles AS gir
		ON g.GroupId = gir.GroupId
	WHERE (g.OrganizationId = @OrganizationId) AND (g.Deleted = 0)
		AND (@InstanceId IS NULL OR gir.InstanceId = @InstanceId)
		AND ('','' + @Roles + '','' LIKE ''%,'' + CAST(gir.RoleId as varchar(50)) + '',%'');
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetGroupsInstancesActionsByGroupIdInstanceId]
(
	@OrganizationId uniqueidentifier,
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT gda.GroupId, gda.InstanceId, gda.ActionId, gda.[Enabled]
	FROM dbo.Mc_GroupsInstancesActions AS gda
	INNER JOIN dbo.Mc_Group AS g
		ON	(gda.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId) AND (g.GroupId = @GroupId)
	INNER JOIN dbo.Mc_Instance AS d
		ON	(gda.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.OrganizationId = @OrganizationId) AND (d.InstanceId = @InstanceId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetGroupsInstancesRolesByGroupId]
(
	@OrganizationId uniqueidentifier,
	@GroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT gdr.GroupId, gdr.InstanceId, gdr.RoleId, d.Name AS InstanceName
	FROM dbo.Mc_GroupsInstancesRoles AS gdr
	INNER JOIN dbo.Mc_Group AS g
		ON	(gdr.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId) AND (g.GroupId = @GroupId)
	INNER JOIN dbo.Mc_Instance AS d
		ON	(gdr.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetGroupsInstancesRolesByGroupIdInstanceId]
(
	@OrganizationId uniqueidentifier,
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT gdr.GroupId, gdr.InstanceId, gdr.RoleId
	FROM dbo.Mc_GroupsInstancesRoles AS gdr
	INNER JOIN dbo.Mc_Group AS g
		ON	(gdr.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId) AND (g.GroupId = @GroupId)
	INNER JOIN dbo.Mc_Instance AS d
		ON	(gdr.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.OrganizationId = @OrganizationId) AND (d.InstanceId = @InstanceId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetGroupsInstancesRolesByGroups]
(
	@OrganizationId uniqueidentifier,
	@Groups varchar(1024)
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT gdr.GroupId, gdr.InstanceId, gdr.RoleId, d.Name AS InstanceName, g.Name
	FROM dbo.Mc_GroupsInstancesRoles AS gdr
	INNER JOIN dbo.Mc_Group AS g
		ON	(gdr.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId)
	INNER JOIN dbo.Mc_Instance AS d
		ON	(gdr.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.OrganizationId = @OrganizationId) AND (d.Active = 1)
	WHERE ('','' + @Groups + '','' LIKE ''%,'' + CAST(gdr.GroupId as varchar(50)) + '',%'');
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetGroupsInstancesRolesByGroupsInstanceId]
(
	@OrganizationId uniqueidentifier,
	@Groups varchar(1024),
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT gdr.GroupId, gdr.InstanceId, gdr.RoleId
	FROM dbo.Mc_GroupsInstancesRoles AS gdr
	INNER JOIN dbo.Mc_Group AS g
		ON	(gdr.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId)
	INNER JOIN dbo.Mc_Instance AS d
		ON	(gdr.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.OrganizationId = @OrganizationId) AND (d.InstanceId = @InstanceId)
	WHERE ('','' + @Groups + '','' LIKE ''%,'' + CAST(gdr.GroupId as varchar(50)) + '',%'');
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetGroupsInstancesRolesByInstanceId]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT gdr.GroupId, gdr.InstanceId, gdr.RoleId
	FROM dbo.Mc_GroupsInstancesRoles AS gdr
	INNER JOIN dbo.Mc_Group AS g
		ON	(gdr.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId)
	INNER JOIN dbo.Mc_Instance AS d
		ON	(gdr.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.OrganizationId = @OrganizationId) AND (d.InstanceId = @InstanceId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetGroupsInstancesRolesByRoleId]
(
	@OrganizationId uniqueidentifier,
	@RoleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT gdr.GroupId, gdr.InstanceId, gdr.RoleId
	FROM dbo.Mc_GroupsInstancesRoles AS gdr
	INNER JOIN dbo.Mc_Group AS g
		ON	(gdr.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId) AND (gdr.RoleId = @RoleId)
	INNER JOIN dbo.Mc_Instance AS d
		ON	(gdr.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetInstance]
(
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, 
		ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat, [DateFormat], BillingPlan, CreditCardStatus
	FROM dbo.Mc_Instance 
	WHERE (InstanceId = @InstanceId) AND (Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetInstanceByName]
(
	@Name nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, 
		ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat, [DateFormat], BillingPlan, CreditCardStatus
	FROM dbo.Mc_Instance 
	WHERE ([Name] = @Name) AND (Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetInstanceByPseudoId]
(
	@PseudoId varchar(6)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, 
		ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat, [DateFormat], BillingPlan, CreditCardStatus
	FROM dbo.Mc_Instance 
	WHERE (PseudoId = @PseudoId) AND (Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetInstancesByGroups]
(
	@OrganizationId uniqueidentifier,
	@Groups varchar(1024)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT DISTINCT
		d.InstanceId, d.PseudoId, d.OrganizationId, d.[Name], d.[Description], d.EnableSignUpUser, 
		d.ExternalId, d.WorkingDays, d.Active, d.CanceledTime, d.Trial, d.Beta, d.Deleted, d.CreatedTime, d.TimeZoneId, d.TimeFormat, d.[DateFormat], d.BillingPlan, d.CreditCardStatus
	FROM dbo.Mc_GroupsInstancesRoles AS gdr
	INNER JOIN dbo.Mc_Instance AS d
		ON	(gdr.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.OrganizationId = @OrganizationId)
	WHERE ('','' + @Groups + '','' LIKE ''%,'' + CAST(gdr.GroupId as varchar(50)) + '',%'');
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetInstancesByUserIdRoleId]
(
	@OrganizationId uniqueidentifier,
	@UserId uniqueidentifier,
	@RoleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Instances TABLE (InstanceId uniqueidentifier);

	INSERT INTO @Instances
	SELECT DISTINCT d.InstanceId
	FROM dbo.Mc_GroupsInstancesRoles AS gdr
	INNER JOIN dbo.Mc_UsersGroups AS ug
		ON gdr.GroupId = ug.GroupId AND ug.UserId = @UserId
	INNER JOIN dbo.Mc_Instance AS d
		ON	(gdr.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.Active = 1) AND (d.OrganizationId = @OrganizationId)
	LEFT JOIN dbo.Mc_UsersInstances AS ui
		ON d.InstanceId = ui.InstanceId AND ui.UserId = @UserId 
	WHERE ISNULL(ui.Active, 1) = 1;

	IF NOT EXISTS(SELECT TOP 1 InstanceId FROM @Instances)
		INSERT INTO @Instances
		SELECT DISTINCT d.InstanceId
		FROM dbo.Mc_GroupsInstancesRoles AS gdr
		INNER JOIN dbo.Mc_Instance AS d
			ON	(gdr.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.Active = 1) AND (d.OrganizationId = @OrganizationId) AND (gdr.RoleId = @RoleId)
		LEFT JOIN dbo.Mc_UsersInstances AS ui
			ON d.InstanceId = ui.InstanceId AND ui.UserId = @UserId 
		WHERE ISNULL(ui.Active, 1) = 1;

	SET NOCOUNT OFF;

	SELECT d.InstanceId, d.PseudoId, d.OrganizationId, d.[Name], d.[Description], d.EnableSignUpUser, 
		d.ExternalId, d.WorkingDays, d.Active, d.CanceledTime, d.Trial, d.Beta, d.Deleted, d.CreatedTime, d.TimeZoneId, d.TimeFormat, d.[DateFormat], d.BillingPlan, d.CreditCardStatus
	FROM dbo.Mc_Instance AS d
	INNER JOIN @Instances AS i
		ON d.InstanceId = i.InstanceId
	ORDER BY d.[Name];
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetRuleParameters]
(
	@RuleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [RuleParameterId], [RuleId], [EntityNodeTypeId], [IsInputParameter], [IsEntity], [FieldName], [FullName], [TypeName], [Term], [Value]
	FROM [Mc_RuleParameters]
	WHERE [RuleId] = @RuleId
	ORDER BY [FieldName] ASC;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetRules]
(
	@RuleEngineId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier = NULL
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [RuleId], [RuleEngineId], [OrganizationId], [InstanceId], [Name], [DisplayName], [UsedQty], [LastUsedUser], [LastUsedDate], [CreatedBy], [CreatedDate], [OrderNumber], [Active]
	FROM [dbo].[Mc_Rule]
	WHERE [RuleEngineId] = @RuleEngineId AND [OrganizationId] = @OrganizationId AND ((@InstanceId IS NULL AND [InstanceId] IS NULL) OR ([InstanceId] = @InstanceId))
	ORDER BY [OrderNumber] ASC;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetSettingsValuesByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT SettingValueId, SettingId, [Value], OrganizationId, InstanceId, GroupId
	FROM dbo.Mc_SettingsValues 
	WHERE (OrganizationId = @OrganizationId) AND (InstanceId IS NULL) AND (GroupId IS NULL);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetSettingsValuesByOrganizationIdInstanceId]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT SettingValueId, SettingId, [Value], OrganizationId, InstanceId, GroupId
	FROM dbo.Mc_SettingsValues 
	WHERE (OrganizationId = @OrganizationId) AND (InstanceId IS NULL) AND (GroupId IS NULL)
	UNION ALL
	SELECT SettingValueId, SettingId, [Value], OrganizationId, InstanceId, GroupId
	FROM dbo.Mc_SettingsValues 
	WHERE (OrganizationId = @OrganizationId) AND (InstanceId = @InstanceId) AND (GroupId IS NULL);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetSettingsValuesByOrganizationIdInstanceIdGroups]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Groups varchar(1024)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT SettingValueId, SettingId, [Value], OrganizationId, InstanceId, GroupId
	FROM dbo.Mc_SettingsValues 
	WHERE (OrganizationId = @OrganizationId) AND (InstanceId IS NULL) AND (GroupId IS NULL)
	UNION ALL
	SELECT SettingValueId, SettingId, [Value], OrganizationId, InstanceId, GroupId
	FROM dbo.Mc_SettingsValues 
	WHERE (OrganizationId = @OrganizationId) AND (InstanceId = @InstanceId) AND (GroupId IS NULL)
	UNION ALL
	SELECT SettingValueId, SettingId, [Value], OrganizationId, InstanceId, GroupId
	FROM dbo.Mc_SettingsValues 
	WHERE (OrganizationId = @OrganizationId) AND (InstanceId = @InstanceId) AND ('','' + @Groups + '','' LIKE ''%,'' + CAST(GroupId as varchar(50)) + '',%'');
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetSettingValue]
(
	@SettingId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@GroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT SettingValueId, SettingId, [Value], OrganizationId, InstanceId, GroupId
	FROM dbo.Mc_SettingsValues 
	WHERE (SettingId = @SettingId)
		AND (OrganizationId = @OrganizationId)
		AND ((@InstanceId IS NULL AND InstanceId IS NULL AND GroupId IS NULL) OR ((InstanceId = @InstanceId) AND ((@GroupId IS NULL AND GroupId IS NULL) OR (GroupId = @GroupId))));
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetUsersGroupsByInstanceId]
(
	@UserId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT ug.UserId, ug.GroupId, gir.RoleId
	FROM dbo.Mc_UsersGroups AS ug
	INNER JOIN dbo.Mc_Group AS g
		ON	(ug.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId)
	INNER JOIN dbo.Mc_User AS u
		ON	(ug.UserId = u.UserId) AND (u.UserId = @UserId)
	INNER JOIN dbo.Mc_GroupsInstancesRoles AS gir
		ON (g.GroupId = gir.GroupId) AND (gir.InstanceId = @InstanceId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION