IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteEntityField]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteEntityField]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteEntityFieldListValue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteEntityFieldListValue]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteEntityFieldValue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteEntityFieldValue]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteEntityNodesRelatedEntityNodes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteEntityNodesRelatedEntityNodes]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteGroupInstanceAction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteGroupInstanceAction]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteGroupInstanceRoles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteGroupInstanceRoles]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteOrganizationUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteOrganizationUser]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteRecurringSchedule]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteRecurringSchedule]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteRule]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteRule]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteRuleParameter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteRuleParameter]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteSettingValue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteSettingValue]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteUserGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteUserGroup]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_DeleteUserInstance]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_DeleteUserInstance]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetAllEntityNodesRelatedEntityNodes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetAllEntityNodesRelatedEntityNodes]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetAllRuleParameters]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetAllRuleParameters]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetAllRules]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetAllRules]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetAnotherAdministrator]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetAnotherAdministrator]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetEntityField]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetEntityField]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetEntityFieldListValue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetEntityFieldListValue]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetEntityFieldListValues]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetEntityFieldListValues]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetEntityFields]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetEntityFields]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetEntityFieldsValues]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetEntityFieldsValues]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetEntityFieldValues]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetEntityFieldValues]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetEntityNodes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetEntityNodes]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetEntityNodesRelatedEntityNodes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetEntityNodesRelatedEntityNodes]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetEntityNodeTypes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetEntityNodeTypes]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetGroups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetGroups]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetGroupsInstancesActions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetGroupsInstancesActions]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetGroupsInstancesRoles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetGroupsInstancesRoles]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetInstances]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetInstances]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetMessages]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetMessages]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetRecurringScheduleByEntityId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetRecurringScheduleByEntityId]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetRecurringScheduleByEntityType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetRecurringScheduleByEntityType]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetRecurringScheduleById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetRecurringScheduleById]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetRecurringScheduleByName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetRecurringScheduleByName]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetRecurringScheduleByRecurrenceRule]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetRecurringScheduleByRecurrenceRule]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetRecurringScheduleEntityTypes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetRecurringScheduleEntityTypes]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetRecurringSchedules]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetRecurringSchedules]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetRule]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetRule]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetRuleByName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetRuleByName]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetRuleParameter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetRuleParameter]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetSettingsValues]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetSettingsValues]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetUser]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetUserByEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetUserByEmail]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetUsers]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetUsersByRoles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetUsersByRoles]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetUsersGroups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetUsersGroups]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetUsersInstances]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_GetUsersInstances]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertEntityField]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertEntityField]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertEntityFieldListValue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertEntityFieldListValue]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertEntityFieldValue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertEntityFieldValue]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertEntityNode]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertEntityNode]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertEntityNodesRelatedEntityNodes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertEntityNodesRelatedEntityNodes]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertEntityNodeType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertEntityNodeType]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertGroup]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertGroupInstanceAction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertGroupInstanceAction]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertGroupInstanceRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertGroupInstanceRole]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertInstance]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertInstance]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertMessage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertMessage]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertOrganizationUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertOrganizationUser]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertRule]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertRule]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertRuleParameter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertRuleParameter]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertSettingValue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertSettingValue]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertUser]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertUserGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertUserGroup]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_InsertUserInstance]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_InsertUserInstance]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateEntityField]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateEntityField]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateEntityFieldListValue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateEntityFieldListValue]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateEntityFieldValue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateEntityFieldValue]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateEntityNode]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateEntityNode]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateEntityNodesRelatedEntityNodes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateEntityNodesRelatedEntityNodes]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateEntityNodeType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateEntityNodeType]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateGroup]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateGroupInstanceAction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateGroupInstanceAction]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateGroupInstanceRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateGroupInstanceRole]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateInstance]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateInstance]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateOrganizationUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateOrganizationUser]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateRecurringSchedule]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateRecurringSchedule]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateRule]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateRule]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateRuleOrder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateRuleOrder]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateRuleParameter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateRuleParameter]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateRuleUses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateRuleUses]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateSettingValue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateSettingValue]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateUser]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_UpdateUserInstance]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Mc_UpdateUserInstance]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_DeleteEntityField]
(
	@EntityFieldId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM [dbo].[Mc_EntityField]
	WHERE (EntityFieldId = @EntityFieldId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_DeleteEntityFieldListValue]
(
	@EntityFieldListValueId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM [dbo].[Mc_EntityFieldListsValues]
	WHERE (EntityFieldListValueId = @EntityFieldListValueId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_DeleteEntityFieldValue]
(
	@EntityFieldValueId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM [dbo].[Mc_EntityFieldsValues]
	WHERE (EntityFieldValueId = @EntityFieldValueId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_DeleteEntityNodesRelatedEntityNodes]
(
	@EntityNodesRelatedEntityNodesId uniqueidentifier
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DELETE
	FROM [Mc_EntityNodesRelatedEntityNodes]
	WHERE
		[EntityNodesRelatedEntityNodesId] = @EntityNodesRelatedEntityNodesId
	SET @Err = @@Error

	RETURN @Err
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_DeleteGroupInstanceAction]
(
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@ActionId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_GroupsInstancesActions 
	WHERE (GroupId = @GroupId) AND (InstanceId = @InstanceId) AND (ActionId = @ActionId);
END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_DeleteGroupInstanceRoles]
(
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_GroupsInstancesRoles 
	WHERE (GroupId = @GroupId) AND (InstanceId = @InstanceId);
END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_DeleteOrganizationUser]
(
	@OrganizationId uniqueidentifier,
	@UserId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_OrganizationsUsers 
	WHERE (OrganizationId = @OrganizationId) AND (UserId = @UserId);
END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[Mc_DeleteRecurringSchedule]
(
	@RecurringScheduleId uniqueidentifier
)
as
begin
	set nocount off;

	delete from [dbo].[Mc_RecurringSchedule]
	where RecurringScheduleId = @RecurringScheduleId;
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_DeleteRule]
(
	@RuleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF

	DELETE FROM [Mc_Rule]
	WHERE [RuleId] = @RuleId
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_DeleteRuleParameter]
(
	@RuleParameterId uniqueidentifier
)
AS
BEGIN

	SET NOCOUNT OFF

	DELETE
	FROM [Mc_RuleParameters]
	WHERE
		[RuleParameterId] = @RuleParameterId
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_DeleteSettingValue]
(
	@SettingValueId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_SettingsValues 
	WHERE (SettingValueId = @SettingValueId);
END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_DeleteUserGroup]
(
	@UserId uniqueidentifier,
	@GroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_UsersGroups 
	WHERE (UserId = @UserId) AND (GroupId = @GroupId);
END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_DeleteUserInstance]
(
	@UserId uniqueidentifier,
	@InstanceId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_UsersInstances 
	WHERE (UserId = @UserId) AND (InstanceId = @InstanceId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetAllEntityNodesRelatedEntityNodes]
	@EntityNodeId uniqueidentifier,
	@EntityId uniqueidentifier,
	@OrganizationId uniqueidentifier
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	SELECT
		[EntityNodesRelatedEntityNodesId],
		enren.[EntityNodeId],
		[RelatedEntityNodeId],
		enren.[EntityId],
		[RelationType]
	FROM [Mc_EntityNodesRelatedEntityNodes] as enren
	LEFT JOIN Mc_EntityNode as en ON en.EntityNodeId = enren.RelatedEntityNodeId
	WHERE enren.EntityNodeId = @EntityNodeId
		AND enren.EntityId = @EntityId AND (en.Deleted = 0 OR RelatedEntityNodeId = '00000000-0000-0000-0000-000000000000')

	SET @Err = @@Error

	RETURN @Err
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetAllRuleParameters]
(
	@RuleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT
		[RuleParameterId],
		[RuleId],
		[EntityNodeTypeId],
		[IsInputParameter],
		[IsEntity],
		[FieldName],
		[FullName],
		[TypeName],
		[Term],
		[Value]
	FROM [Mc_RuleParameters]
	WHERE [RuleId] = @RuleId
	ORDER BY [FieldName] ASC;
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetAllRules]
	@RuleEngineId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier = NULL
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [RuleId], [RuleEngineId], [OrganizationId], [InstanceId], [Name], [DisplayName], [UsedQty], [LastUsedUser], [LastUsedDate], [CreatedBy], [CreatedDate], [OrderNumber], [Active]
	FROM [dbo].[Mc_Rule]
	WHERE [RuleEngineId] = @RuleEngineId
		AND [OrganizationId] = @OrganizationId
		AND (	(@InstanceId IS NULL AND [InstanceId] IS NULL)
				OR ([InstanceId] = @InstanceId))
	ORDER BY [OrderNumber] ASC;
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetAnotherAdministrator]
(
	@UserId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT TOP 1 u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, u.TimeZoneId, u.TimeFormat, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	INNER JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId <> uo.UserId) AND (u.Deleted = 0) AND (uo.OrganizationId = @OrganizationId) AND (uo.OrganizationAdministrator = 1);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetEntityField]
(
	@EntityFieldId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataTypeId], [DefaultValue], [AllowDBNull], [Unique], [MaxLength], [MinValue], [MaxValue], [DecimalDigits], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active]
	FROM dbo.Mc_EntityField
	WHERE (EntityFieldId = @EntityFieldId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetEntityFieldListValue]
(
	@EntityFieldListValueId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [EntityFieldListValueId], [EntityFieldId], [Name], [Value], [Default], [Active]
	FROM [dbo].[Mc_EntityFieldListsValues]
	WHERE (EntityFieldListValueId = @EntityFieldListValueId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetEntityFieldListValues]
(
	@EntityFieldId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [EntityFieldListValueId], [EntityFieldId], [Name], [Value], [Default], [Active]
	FROM [dbo].[Mc_EntityFieldListsValues]
	WHERE (EntityFieldId = @EntityFieldId) AND ((@Active IS NULL) OR (Active = @Active));
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetEntityFields]
(
	@EntityId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataTypeId], [DefaultValue], [AllowDBNull], [Unique], [MaxLength], [MinValue], [MaxValue], [DecimalDigits], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active]
	FROM dbo.Mc_EntityField
	WHERE ([EntityId] = @EntityId) 
		AND (OrganizationId = @OrganizationId) 
		AND ((@InstanceId IS NULL AND InstanceId IS NULL) OR (InstanceId = @InstanceId))
		AND ((@Active IS NULL) OR (Active = @Active));
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetEntityFieldsValues]
(
	@EntityId uniqueidentifier,
	@LocalEntityId nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT v.[EntityFieldValueId], v.[EntityFieldId], v.[LocalEntityId], v.[Value]
	FROM [dbo].[Mc_EntityFieldsValues] AS v
	INNER JOIN dbo.Mc_EntityField AS f
		ON	v.EntityFieldId = f.EntityFieldId
	WHERE (f.EntityId = @EntityId) AND (v.LocalEntityId = @LocalEntityId) AND (f.Active = 1);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetEntityFieldValues]
(
	@EntityFieldId uniqueidentifier,
	@LocalEntityId nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT v.[EntityFieldValueId], v.[EntityFieldId], v.[LocalEntityId], v.[Value]
	FROM [dbo].[Mc_EntityFieldsValues] AS v
	INNER JOIN dbo.Mc_EntityField AS f
		ON	v.EntityFieldId = f.EntityFieldId
	WHERE (f.EntityFieldId = @EntityFieldId) AND (v.LocalEntityId = @LocalEntityId) AND (f.Active = 1);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetEntityNodes]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT EntityNodeId, ParentEntityNodeId, [Name], OrderNumber, OrganizationId, InstanceId, EntityId, EntityNodeTypeId, SubEntityId, SubEntityLocalId, FullPath, Deleted
	FROM [dbo].[Mc_EntityNode]
	WHERE (Deleted = 0)
	ORDER BY [Name]
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetEntityNodesRelatedEntityNodes]
	@EntityNodesRelatedEntityNodesId uniqueidentifier	
AS
BEGIN
	SET NOCOUNT OFF;
	DECLARE @Err int

	SELECT
		[EntityNodesRelatedEntityNodesId],
		[EntityNodeId],
		[EntityId],
		[RelatedEntityNodeId],
		[RelationType]
	FROM [Mc_EntityNodesRelatedEntityNodes]
	WHERE [EntityNodesRelatedEntityNodesId] = @EntityNodesRelatedEntityNodesId

	SET @Err = @@Error

	RETURN @Err
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetEntityNodeTypes]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT EntityNodeTypeId, EntityId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted
	FROM [dbo].[Mc_EntityNodeType]
	WHERE (Deleted = 0);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetGroups]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT GroupId, OrganizationId, [Name], [Description], BuiltIn, Deleted 
	FROM dbo.Mc_Group 
	WHERE (Deleted = 0) AND (OrganizationId = @OrganizationId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetGroupsInstancesActions]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT gda.GroupId, gda.InstanceId, gda.ActionId, gda.[Enabled]
	FROM dbo.Mc_GroupsInstancesActions AS gda
	INNER JOIN dbo.Mc_Group AS g
		ON	(gda.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId)
	INNER JOIN dbo.Mc_Instance AS d
		ON	(gda.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.OrganizationId = @OrganizationId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetGroupsInstancesRoles]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT gdr.GroupId, gdr.InstanceId, gdr.RoleId
	FROM dbo.Mc_GroupsInstancesRoles AS gdr
	INNER JOIN dbo.Mc_Group AS g
		ON	(gdr.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId)
	INNER JOIN dbo.Mc_Instance AS d
		ON	(gdr.InstanceId = d.InstanceId) AND (d.Deleted = 0) AND (d.OrganizationId = @OrganizationId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetInstances]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, 
		ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat
	FROM dbo.Mc_Instance 
	WHERE (OrganizationId = @OrganizationId) AND (Deleted = 0);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetMessages]
(
	@LocalObjectType nvarchar(50),
	@LocalObjectId nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT MessageId, ParentMessageId, LocalObjectType, LocalObjectId, FromUserId, ToUserId, [Subject], [Text], CreatedTime
	FROM dbo.Mc_Message
	WHERE (LocalObjectType = @LocalObjectType) AND (LocalObjectId = @LocalObjectId)
	ORDER BY CreatedTime;
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[Mc_GetRecurringScheduleByEntityId]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier=null,
	@LocalEntityType nvarchar(50),
	@LocalEntityId  nvarchar(255)
)
as
begin
	set NOCOUNT OFF;

	select [RecurringScheduleId]
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
	from [Mc_RecurringSchedule] as RS
	where RS.OrganizationId = @OrganizationId
	and RS.Deleted = 0
	and (	@InstanceId is null 
			or RS.InstanceId is null 
			or RS.InstanceId = @InstanceId)
	and RS.LocalEntityType = @LocalEntityType
	and RS.LocalEntityId = @LocalEntityId;
	
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[Mc_GetRecurringScheduleByEntityType]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier=null,
	@LocalEntityType nvarchar(50)
)
as
begin
	set NOCOUNT OFF;

	select [RecurringScheduleId]
		  ,[OrganizationId]
		  ,[InstanceId]
		  ,[LocalEntityType]
		  ,[LocalEntityId]
		  ,[StartDate]
		  ,[EndDate]
		  ,[RecurrenceRule]
		  ,[UpdatedTime]
		  ,[UpdatedBy]
		  ,[Deleted]
	from [Mc_RecurringSchedule] as RS
	where RS.OrganizationId = @OrganizationId
	and RS.Deleted = 0
	and (	@InstanceId is null 
			or RS.InstanceId is null 
			or RS.InstanceId = @InstanceId)
	and RS.LocalEntityType = @LocalEntityType;
	
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[Mc_GetRecurringScheduleById]
(
	@RecurringScheduleId uniqueidentifier
)
as
begin
	set NOCOUNT OFF;

	select [RecurringScheduleId]
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
	from [Mc_RecurringSchedule] as RS
	where RS.RecurringScheduleId = @RecurringScheduleId
	and RS.Deleted = 0;
	
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[Mc_GetRecurringScheduleByName]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier=null,
	@Name nvarchar(255)
)
as
begin
	set NOCOUNT OFF;

	select [RecurringScheduleId]
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
	from [Mc_RecurringSchedule] as RS
	where RS.OrganizationId = @OrganizationId
	and RS.Deleted = 0
	and (	@InstanceId is null 
			or RS.InstanceId is null 
			or RS.InstanceId = @InstanceId)
	and RS.[Name] = @Name;
	
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[Mc_GetRecurringScheduleByRecurrenceRule]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier=null,
	@RecurrenceRule nvarchar(1024)
)
as
begin
	set NOCOUNT OFF;

	select [RecurringScheduleId]
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
	from [Mc_RecurringSchedule] as RS
	where RS.OrganizationId = @OrganizationId
	and RS.Deleted = 0
	and (	@InstanceId is null 
			or RS.InstanceId is null 
			or RS.InstanceId = @InstanceId)
	and RS.[RecurrenceRule] = @RecurrenceRule;
	
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[Mc_GetRecurringScheduleEntityTypes]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier=null
)
as
begin
	set NOCOUNT OFF;

	select distinct [LocalEntityType]
	from [Mc_RecurringSchedule] as RS
	where RS.OrganizationId = @OrganizationId
	and RS.Deleted = 0
	and (	@InstanceId is null 
			or RS.InstanceId is null 
			or RS.InstanceId = @InstanceId)
	order by [LocalEntityType] asc;
	
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[Mc_GetRecurringSchedules]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier=null
)
as
begin
	set NOCOUNT OFF;

	select [RecurringScheduleId]
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
	from [Mc_RecurringSchedule] as RS
	where RS.OrganizationId = @OrganizationId
	and RS.Deleted = 0
	and (	@InstanceId is null 
			or RS.InstanceId is null 
			or RS.InstanceId = @InstanceId);
	
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetRule]
(
	@RuleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT
		[RuleId]
	  ,[RuleEngineId]
	  ,[OrganizationId]
	  ,[InstanceId]
	  ,[Name]
	  ,[DisplayName]
	  ,[UsedQty]
	  ,[LastUsedUser]
	  ,[LastUsedDate]
	  ,[CreatedBy]
	  ,[CreatedDate]
	  ,[OrderNumber]
	  ,[Active]
	FROM [Mc_Rule]
	WHERE
		([RuleId] = @RuleId)
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetRuleByName]
(
	@Name nvarchar(255),
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT
		[RuleId]
	  ,[RuleEngineId]
	  ,[OrganizationId]
	  ,[InstanceId]
	  ,[Name]
	  ,[DisplayName]
	  ,[UsedQty]
	  ,[LastUsedUser]
	  ,[LastUsedDate]
	  ,[CreatedBy]
	  ,[CreatedDate]
	  ,[OrderNumber]
	  ,[Active]
	FROM [Mc_Rule]
	WHERE [Name] = @Name
	AND [OrganizationId] = @OrganizationId;
	
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetRuleParameter]
(
	@RuleParameterId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF

	SELECT
		[RuleParameterId],
		[RuleId],
		[EntityNodeTypeId],
		[IsInputParameter],
		[IsEntity],
		[FieldName],
		[FullName],
		[TypeName],
		[Term],
		[Value]
	FROM [Mc_RuleParameters]
	WHERE
		([RuleParameterId] = @RuleParameterId)
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetSettingsValues]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT SettingValueId, SettingId, [Value], OrganizationId, InstanceId, GroupId
	FROM dbo.Mc_SettingsValues 
	WHERE (OrganizationId IS NULL) OR (OrganizationId = @OrganizationId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetUser]
(
	@UserId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT TOP 1 u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, u.TimeZoneId, u.TimeFormat, uo.OrganizationId, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	LEFT JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId = uo.UserId)
	WHERE (u.UserId = @UserId) AND (@OrganizationId IS NULL OR uo.OrganizationId = @OrganizationId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetUserByEmail]
(
	@Email nvarchar(255),
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, u.TimeZoneId, u.TimeFormat, uo.OrganizationId, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	LEFT JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId = uo.UserId)
	WHERE (u.Email = @Email) AND (@OrganizationId IS NULL OR uo.OrganizationId = @OrganizationId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetUsers]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, u.TimeZoneId, u.TimeFormat, uo.OrganizationId, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	INNER JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId = uo.UserId) AND (u.Deleted = 0) AND (uo.OrganizationId = @OrganizationId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetUsersByRoles]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@OrganizationAdministrator bit,
	@Active bit,
	@InRoles varchar(1024),
	@NotInRoles varchar(1024)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT DISTINCT 
		u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, u.TimeZoneId, u.TimeFormat, uo.OrganizationId, uo.OrganizationAdministrator
		, CASE WHEN ((@InstanceId IS NOT NULL) AND (uo.Active = 1) AND (ISNULL(ui.Active, 1) = 1))
			OR ((@InstanceId IS NULL) AND (uo.Active = 1)) THEN 1 ELSE 0 END AS Active
	FROM dbo.Mc_User AS u
	INNER JOIN dbo.Mc_OrganizationsUsers AS uo
		ON	(u.UserId = uo.UserId) 
			AND (u.Deleted = 0) 
			AND (uo.OrganizationId = @OrganizationId) 
			AND ((@OrganizationAdministrator IS NULL) OR (uo.OrganizationAdministrator = @OrganizationAdministrator))
	LEFT JOIN dbo.Mc_UsersInstances AS ui
		ON	(u.UserId = ui.UserId) 
			AND ((@InstanceId IS NULL) OR (ui.InstanceId = @InstanceId))
	WHERE
		((@Active IS NULL) 
			OR (CASE WHEN ((@InstanceId IS NOT NULL) AND (uo.Active = 1) AND (ISNULL(ui.Active, 1) = 1))
				OR ((@InstanceId IS NULL) AND (uo.Active = 1)) THEN 1 ELSE 0 END) = @Active
		)
		AND ((uo.OrganizationAdministrator = 1) OR ((uo.OrganizationAdministrator = 0) AND EXISTS (SELECT 0 FROM dbo.Mc_UsersGroups WHERE UserId = u.UserId)))
		AND ((@InRoles IS NULL) OR EXISTS (
			SELECT 0 
			FROM dbo.Mc_UsersGroups AS ug1
			INNER JOIN dbo.Mc_Group AS g1
				ON	(ug1.GroupId = g1.GroupId) 
					AND (g1.Deleted = 0) 
					AND (g1.OrganizationId = @OrganizationId)
					AND (ug1.UserId = u.UserId) 
			INNER JOIN dbo.Mc_GroupsInstancesRoles AS gir1
				ON	(g1.GroupId = gir1.GroupId) 
					AND ((@InstanceId IS NULL) OR (gir1.InstanceId = @InstanceId))
			WHERE (',' + @InRoles + ',' LIKE '%,' + CAST(gir1.RoleId as varchar(50)) + ',%'))
		)
		AND ((@NotInRoles IS NULL) OR NOT EXISTS (
			SELECT 0 
			FROM dbo.Mc_UsersGroups AS ug2
			INNER JOIN dbo.Mc_Group AS g2
				ON	(ug2.GroupId = g2.GroupId) 
					AND (g2.Deleted = 0) 
					AND (g2.OrganizationId = @OrganizationId)
					AND (ug2.UserId = u.UserId) 
			INNER JOIN dbo.Mc_GroupsInstancesRoles AS gir2
				ON	(g2.GroupId = gir2.GroupId) 
					AND ((@InstanceId IS NULL) OR (gir2.InstanceId = @InstanceId))
			WHERE (',' + @NotInRoles + ',' LIKE '%,' + CAST(gir2.RoleId as varchar(50)) + ',%'))
		);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetUsersGroups]
(
	@UserId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT ug.UserId, ug.GroupId 
	FROM dbo.Mc_UsersGroups AS ug
	INNER JOIN dbo.Mc_Group AS g
		ON	(ug.GroupId = g.GroupId) AND (g.Deleted = 0) AND (g.OrganizationId = @OrganizationId)
	INNER JOIN dbo.Mc_User AS u
		ON	(ug.UserId = u.UserId) AND (u.UserId = @UserId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_GetUsersInstances]
(
	@UserId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT ui.UserId, ui.InstanceId, ui.Active 
	FROM dbo.Mc_UsersInstances AS ui
	INNER JOIN dbo.Mc_Instance AS i
		ON	(ui.InstanceId = i.InstanceId) AND (i.Deleted = 0) AND (i.OrganizationId = @OrganizationId)
	INNER JOIN dbo.Mc_User AS u
		ON	(ui.UserId = u.UserId) AND (u.UserId = @UserId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertEntityField]
(
	@EntityFieldId uniqueidentifier,
	@EntityFieldTypeId int,
	@Name nvarchar(255),
	@Description nvarchar(255),
	@DataTypeId int,
	@DefaultValue nvarchar(512),
	@AllowDBNull bit,
	@Unique bit,
	@MaxLength int,
	@MinValue nvarchar(512),
	@MaxValue nvarchar(512),
	@DecimalDigits int,
	@OrderNumber int,
	@EntityId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_EntityField] ([EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataTypeId], [DefaultValue], [AllowDBNull], [Unique], [MaxLength], [MinValue], [MaxValue], [DecimalDigits], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active])
	VALUES (@EntityFieldId, @EntityFieldTypeId, @Name, @Description, @DataTypeId, @DefaultValue, @AllowDBNull, @Unique, @MaxLength, @MinValue, @MaxValue, @DecimalDigits, @OrderNumber, @EntityId, @OrganizationId, @InstanceId, @Active);
	
	SELECT [EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataTypeId], [DefaultValue], [AllowDBNull], [Unique], [MaxLength], [MinValue], [MaxValue], [DecimalDigits], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active]
	FROM dbo.Mc_EntityField
	WHERE (EntityFieldId = @EntityFieldId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertEntityFieldListValue]
(
	@EntityFieldListValueId uniqueidentifier,
	@EntityFieldId uniqueidentifier,
	@Name nvarchar(255),
	@Value nvarchar(512),
	@Default bit,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_EntityFieldListsValues] ([EntityFieldListValueId], [EntityFieldId], [Name], [Value], [Default], [Active])
	VALUES (@EntityFieldListValueId, @EntityFieldId, @Name, @Value, @Default, @Active);

	IF @Default = 1
	BEGIN
		DECLARE @EntityFieldTypeId int;
		
		SELECT @EntityFieldTypeId = [EntityFieldTypeId]
		FROM [dbo].[Mc_EntityField]
		WHERE [EntityFieldId] = @EntityFieldId;

		IF @EntityFieldTypeId = 2
			UPDATE [dbo].[Mc_EntityFieldListsValues]
			SET [Default] = 0
			WHERE ([EntityFieldId] = @EntityFieldId) AND ([EntityFieldListValueId] <> @EntityFieldListValueId);
	END

	SELECT [EntityFieldListValueId], [EntityFieldId], [Name], [Value], [Default], [Active]
	FROM [dbo].[Mc_EntityFieldListsValues]
	WHERE (EntityFieldListValueId = @EntityFieldListValueId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertEntityFieldValue]
(
	@EntityFieldValueId uniqueidentifier,
	@EntityFieldId uniqueidentifier,
	@LocalEntityId nvarchar(255),
	@Value nvarchar(MAX)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_EntityFieldsValues] ([EntityFieldValueId], [EntityFieldId], [LocalEntityId], [Value])
	VALUES (@EntityFieldValueId, @EntityFieldId, @LocalEntityId, @Value);

	SELECT [EntityFieldValueId], [EntityFieldId], [LocalEntityId], [Value]
	FROM [dbo].[Mc_EntityFieldsValues]
	WHERE (EntityFieldValueId = @EntityFieldValueId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertEntityNode]
(
	@EntityNodeId uniqueidentifier,
	@ParentEntityNodeId uniqueidentifier,
	@Name nvarchar(255),
	@OrderNumber int,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@EntityId uniqueidentifier,
	@EntityNodeTypeId uniqueidentifier,
	@SubEntityId uniqueidentifier,
	@SubEntityLocalId nvarchar(255),
	@FullPath nvarchar(1024),
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_EntityNode] (EntityNodeId, ParentEntityNodeId, [Name], OrderNumber, OrganizationId, InstanceId, EntityId, EntityNodeTypeId, SubEntityId, SubEntityLocalId, FullPath, Deleted)
	VALUES (@EntityNodeId, @ParentEntityNodeId, @Name, @OrderNumber, @OrganizationId, @InstanceId, @EntityId, @EntityNodeTypeId, @SubEntityId, @SubEntityLocalId, @FullPath, @Deleted);
	
	SELECT EntityNodeId, ParentEntityNodeId, [Name], OrderNumber, OrganizationId, InstanceId, EntityId, EntityNodeTypeId, SubEntityId, SubEntityLocalId, FullPath, Deleted
	FROM [dbo].[Mc_EntityNode]
	WHERE (EntityNodeId = @EntityNodeId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertEntityNodesRelatedEntityNodes]
(
	@EntityNodesRelatedEntityNodesId uniqueidentifier,
	@EntityNodeId uniqueidentifier,	
	@RelatedEntityNodeId uniqueidentifier,
	@EntityId uniqueidentifier,
	@RelationType int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	IF @EntityNodesRelatedEntityNodesId IS NULL
		 SET @EntityNodesRelatedEntityNodesId = NEWID()

	SET @Err = @@Error

	IF (@Err <> 0)
	    RETURN @Err


	INSERT
	INTO [Mc_EntityNodesRelatedEntityNodes]
	(
		[EntityNodesRelatedEntityNodesId],
		[EntityNodeId],
		[RelatedEntityNodeId],
		[EntityId],
		[RelationType]
	)
	VALUES
	(
		@EntityNodesRelatedEntityNodesId,
		@EntityNodeId,
		@RelatedEntityNodeId,
		@EntityId,
		@RelationType
	)

	SET @Err = @@Error


	RETURN @Err
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertEntityNodeType]
(
	@EntityNodeTypeId uniqueidentifier,
	@EntityId uniqueidentifier,
	@Name nvarchar(255),
	@OrderNumber int,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_EntityNodeType] (EntityNodeTypeId, EntityId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted)
	VALUES (@EntityNodeTypeId, @EntityId, @Name, @OrderNumber, @OrganizationId, @InstanceId, @Deleted);
	
	SELECT EntityNodeTypeId, EntityId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted
	FROM [dbo].[Mc_EntityNodeType]
	WHERE (EntityNodeTypeId = @EntityNodeTypeId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertGroup]
(
	@GroupId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@BuiltIn bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Group (GroupId, OrganizationId, [Name], [Description], BuiltIn, Deleted) 
	VALUES (@GroupId, @OrganizationId, @Name, @Description, @BuiltIn, @Deleted);
	
	SELECT GroupId, OrganizationId, [Name], [Description], BuiltIn, Deleted 
	FROM dbo.Mc_Group 
	WHERE (GroupId = @GroupId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_InsertGroupInstanceAction]
(
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@ActionId uniqueidentifier,
	@Enabled bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_GroupsInstancesActions (GroupId, InstanceId, ActionId, [Enabled]) 
	VALUES (@GroupId, @InstanceId, @ActionId, @Enabled);
	
	SELECT GroupId, InstanceId, ActionId, [Enabled] 
	FROM dbo.Mc_GroupsInstancesActions 
	WHERE (ActionId = @ActionId) AND (InstanceId = @InstanceId) AND (GroupId = @GroupId);
END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_InsertGroupInstanceRole]
(
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@RoleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_GroupsInstancesRoles (GroupId, InstanceId, RoleId) 
	VALUES (@GroupId, @InstanceId, @RoleId);
	
	SELECT GroupId, InstanceId, RoleId 
	FROM dbo.Mc_GroupsInstancesRoles 
	WHERE (InstanceId = @InstanceId) AND (GroupId = @GroupId);
END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertInstance]
(
	@InstanceId uniqueidentifier,
	@PseudoId varchar(6),
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@EnableSignUpUser bit,
	@ExternalId nvarchar(255),
	@WorkingDays char(7),
	@Active bit,
	@CanceledTime datetime,
	@Trial bit,
	@Beta bit,
	@Deleted bit,
	@TimeZoneId nvarchar(255),
	@TimeFormat int
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Instance (InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat) 
	VALUES (@InstanceId, @PseudoId, @OrganizationId, @Name, @Description, @EnableSignUpUser, @ExternalId, @WorkingDays, @Active, @CanceledTime, @Trial, @Beta, @Deleted, GETUTCDATE(), @TimeZoneId, @TimeFormat);
	
	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat
	FROM dbo.Mc_Instance 
	WHERE (InstanceId = @InstanceId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertMessage]
(
	@MessageId uniqueidentifier,
	@ParentMessageId uniqueidentifier,
	@LocalObjectType nvarchar(50),
	@LocalObjectId nvarchar(255),
	@FromUserId uniqueidentifier,
	@ToUserId uniqueidentifier,
	@Subject nvarchar(255),
	@Text nvarchar(max),
	@CreatedTime datetime
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Message (MessageId, ParentMessageId, LocalObjectType, LocalObjectId, FromUserId, ToUserId, [Subject], [Text], CreatedTime) 
	VALUES (@MessageId, @ParentMessageId, @LocalObjectType, @LocalObjectId, @FromUserId, @ToUserId,@Subject, @Text, @CreatedTime);
	
	SELECT MessageId, ParentMessageId, LocalObjectType, LocalObjectId, FromUserId, ToUserId, [Subject], [Text], CreatedTime
	FROM dbo.Mc_Message
	WHERE MessageId = @MessageId;
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertOrganizationUser]
(
	@OrganizationId uniqueidentifier,
	@UserId uniqueidentifier,
	@OrganizationAdministrator bit,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_OrganizationsUsers (OrganizationId, UserId, OrganizationAdministrator, Active) 
	VALUES (@OrganizationId, @UserId, @OrganizationAdministrator, @Active);
	
	SELECT OrganizationId, UserId, OrganizationAdministrator, Active
	FROM dbo.Mc_OrganizationsUsers
	WHERE (OrganizationId = @OrganizationId) AND (UserId = @UserId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertRule]
(
	@RuleId uniqueidentifier,
	@RuleEngineId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier = NULL,
	@Name nvarchar(255),
	@DisplayName nvarchar(255),
	@UsedQty int,
	@LastUsedUser uniqueidentifier,
	@LastUsedDate datetime,
	@CreatedBy uniqueidentifier,
	@CreatedDate datetime,
	@OrderNumber int,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	IF @RuleId IS NULL
		 SET @RuleId = NEWID()

	INSERT INTO [Mc_Rule]
	(
		[RuleId]
	  ,[RuleEngineId]
	  ,[OrganizationId]
	  ,[InstanceId]
	  ,[Name]
	  ,[DisplayName]
	  ,[UsedQty]
	  ,[LastUsedUser]
	  ,[LastUsedDate]
	  ,[CreatedDate]
	  ,[CreatedBy]
	  ,[OrderNumber]
	  ,[Active]
	)
	VALUES
	(
		@RuleId,
		@RuleEngineId,
		@OrganizationId,
		@InstanceId,
		@Name,
		@DisplayName,
		@UsedQty,
		@LastUsedUser,
		@LastUsedDate,
		@CreatedDate,
		@CreatedBy,
		@OrderNumber,
		@Active
	);
	
	SELECT
		[RuleId]
	  ,[RuleEngineId]
	  ,[OrganizationId]
	  ,[InstanceId]
	  ,[Name]
	  ,[DisplayName]
	  ,[UsedQty]
	  ,[LastUsedUser]
	  ,[LastUsedDate]
	  ,[CreatedDate]
	  ,[CreatedBy]
	  ,[OrderNumber]
	  ,[Active]
	FROM [Mc_Rule]
	WHERE
		([RuleId] = @RuleId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertRuleParameter]
(
	@RuleParameterId uniqueidentifier,
	@RuleId uniqueidentifier,
	@EntityNodeTypeId uniqueidentifier = NULL,
	@IsInputParameter bit,
	@IsEntity bit,
	@FieldName nvarchar(255),
	@FullName nvarchar(512),
	@TypeName nvarchar(255) = NULL,
	@Term nvarchar(50),
	@Value sql_variant
)
AS
BEGIN
	SET NOCOUNT OFF;

	IF @RuleParameterId IS NULL
		 SET @RuleParameterId = NEWID()

	INSERT
	INTO [Mc_RuleParameters]
	(
		[RuleParameterId],
		[RuleId],
		[EntityNodeTypeId],
		[IsInputParameter],
		[IsEntity],
		[FieldName],
		[FullName],
		[TypeName],
		[Term],
		[Value]
	)
	VALUES
	(
		@RuleParameterId,
		@RuleId,
		@EntityNodeTypeId,
		@IsInputParameter,
		@IsEntity,
		@FieldName,
		@FullName,
		@TypeName,
		@Term,
		@Value
	)

	SELECT
		[RuleParameterId],
		[RuleId],
		[EntityNodeTypeId],
		[IsInputParameter],
		[IsEntity],
		[FieldName],
		[FullName],
		[TypeName],
		[Term],
		[Value]
	FROM [Mc_RuleParameters]
	WHERE
		([RuleParameterId] = @RuleParameterId)
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertSettingValue]
(
	@SettingValueId uniqueidentifier,
	@SettingId uniqueidentifier,
	@Value nvarchar(max),
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@GroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_SettingsValues (SettingValueId, SettingId, Value, OrganizationId, InstanceId, GroupId) 
	VALUES (@SettingValueId, @SettingId, @Value, @OrganizationId, @InstanceId, @GroupId);
	
	SELECT SettingValueId, SettingId, Value, OrganizationId, InstanceId, GroupId 
	FROM dbo.Mc_SettingsValues
	WHERE (SettingValueId = @SettingValueId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertUser]
(
	@UserId uniqueidentifier,
	@Email nvarchar(255),
	@FirstName nvarchar(255),
	@LastName nvarchar(255),
	@MiddleName nvarchar(255),
    @Phone nvarchar(20),
    @MobilePhone nvarchar(20),
    @Fax nvarchar(20),
	@Title nvarchar(30),
	@Department nvarchar(255),
	@Street nvarchar(255),
	@Street2 nvarchar(255),
	@City nvarchar(255),
	@State nvarchar(255),
	@PostalCode nvarchar(20),
	@Country nvarchar(255),
	@LastLoginDate datetime,
	@Deleted bit,
	@TimeZoneId nvarchar(100),
	@TimeFormat int
)
AS
BEGIN
	SET NOCOUNT OFF;

	IF NOT EXISTS(SELECT 0 FROM dbo.Mc_User WHERE UserId = @UserId)
		INSERT INTO dbo.Mc_User (UserId, Email, FirstName, LastName, MiddleName, Phone, MobilePhone, Fax, Title, Department, Street, Street2, City, [State], PostalCode, Country, LastLoginDate, Deleted, TimeZoneId, TimeFormat) 
		VALUES (@UserId, @Email, @FirstName, @LastName, @MiddleName, @Phone, @MobilePhone, @Fax, @Title, @Department, @Street, @Street2, @City, @State, @PostalCode, @Country, @LastLoginDate, @Deleted, @TimeZoneId, @TimeFormat);
	
	SELECT UserId, Email, FirstName, LastName, MiddleName, Phone, MobilePhone, Fax, Title, Department, Street, Street2, City, [State], PostalCode, Country, LastLoginDate, Deleted, TimeZoneId, TimeFormat
	FROM dbo.Mc_User 
	WHERE (UserId = @UserId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_InsertUserGroup]
(
	@UserId uniqueidentifier,
	@GroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_UsersGroups (UserId, GroupId) 
	VALUES (@UserId, @GroupId);
	
	SELECT UserId, GroupId 
	FROM dbo.Mc_UsersGroups 
	WHERE (GroupId = @GroupId) AND (UserId = @UserId);
END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_InsertUserInstance]
(
	@UserId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_UsersInstances (UserId, InstanceId, Active) 
	VALUES (@UserId, @InstanceId, @Active);
	
	SELECT UserId, InstanceId, Active 
	FROM dbo.Mc_UsersInstances 
	WHERE (UserId = @UserId) AND (InstanceId = @InstanceId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateEntityField]
(
	@EntityFieldId uniqueidentifier,
	@EntityFieldTypeId int,
	@Name nvarchar(255),
	@Description nvarchar(255),
	@DataTypeId int,
	@DefaultValue nvarchar(512),
	@AllowDBNull bit,
	@Unique bit,
	@MaxLength int,
	@MinValue nvarchar(512),
	@MaxValue nvarchar(512),
	@DecimalDigits int,
	@OrderNumber int,
	@EntityId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [dbo].[Mc_EntityField]
	SET [EntityFieldTypeId] = @EntityFieldTypeId, [Name] = @Name, [Description] = @Description, [DataTypeId] = @DataTypeId, [DefaultValue] = @DefaultValue
		, [AllowDBNull] = @AllowDBNull, [Unique] = @Unique, [MaxLength] = @MaxLength, [MinValue] = @MinValue, [MaxValue] = @MaxValue, [DecimalDigits] = @DecimalDigits
		, [OrderNumber] = @OrderNumber, [EntityId] = @EntityId, [OrganizationId] = @OrganizationId, [InstanceId] = @InstanceId, [Active] = @Active
	WHERE (EntityFieldId = @EntityFieldId);
	
	SELECT [EntityFieldId], [EntityFieldTypeId], [Name], [Description], [DataTypeId], [DefaultValue], [AllowDBNull], [Unique], [MaxLength], [MinValue], [MaxValue], [DecimalDigits], [OrderNumber], [EntityId], [OrganizationId], [InstanceId], [Active]
	FROM dbo.Mc_EntityField
	WHERE (EntityFieldId = @EntityFieldId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateEntityFieldListValue]
(
	@EntityFieldListValueId uniqueidentifier,
	@EntityFieldId uniqueidentifier,
	@Name nvarchar(255),
	@Value nvarchar(512),
	@Default bit,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [dbo].[Mc_EntityFieldListsValues]
	SET [EntityFieldId] = @EntityFieldId, [Name] = @Name, [Value] = @Value, [Default] = @Default, [Active] = @Active
	WHERE (EntityFieldListValueId = @EntityFieldListValueId);
	
	IF @Default = 1
	BEGIN
		DECLARE @EntityFieldTypeId int;
		
		SELECT @EntityFieldTypeId = [EntityFieldTypeId]
		FROM [dbo].[Mc_EntityField]
		WHERE [EntityFieldId] = @EntityFieldId;

		IF @EntityFieldTypeId = 2
			UPDATE [dbo].[Mc_EntityFieldListsValues]
			SET [Default] = 0
			WHERE ([EntityFieldId] = @EntityFieldId) AND ([EntityFieldListValueId] <> @EntityFieldListValueId);
	END

	SELECT EntityFieldListValueId, [EntityFieldId], [Name], [Value], [Default], [Active]
	FROM [dbo].[Mc_EntityFieldListsValues]
	WHERE (EntityFieldListValueId = @EntityFieldListValueId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateEntityFieldValue]
(
	@EntityFieldValueId uniqueidentifier,
	@EntityFieldId uniqueidentifier,
	@LocalEntityId nvarchar(255),
	@Value nvarchar(MAX)
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	UPDATE [dbo].[Mc_EntityFieldsValues]
	SET [EntityFieldId] = @EntityFieldId, [LocalEntityId] = @LocalEntityId, [Value] = @Value
	WHERE (EntityFieldValueId = @EntityFieldValueId);

	SELECT [EntityFieldValueId], [EntityFieldId], [LocalEntityId], [Value]
	FROM [dbo].[Mc_EntityFieldsValues]
	WHERE (EntityFieldValueId = @EntityFieldValueId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateEntityNode]
	(
		@EntityNodeId uniqueidentifier,
		@ParentEntityNodeId uniqueidentifier,
		@Name nvarchar(255),
		@OrderNumber int,
		@OrganizationId uniqueidentifier,
		@InstanceId uniqueidentifier,
		@EntityId uniqueidentifier,
		@EntityNodeTypeId uniqueidentifier,
		@SubEntityId uniqueidentifier,
		@SubEntityLocalId nvarchar(255),
		@FullPath nvarchar(1024),
		@Deleted bit
	)
	AS
	BEGIN
		SET NOCOUNT OFF;

		UPDATE [dbo].[Mc_EntityNode]
		SET ParentEntityNodeId = @ParentEntityNodeId, [Name] = @Name, OrderNumber = @OrderNumber, OrganizationId = @OrganizationId, InstanceId = @InstanceId, EntityId = @EntityId, EntityNodeTypeId = @EntityNodeTypeId, SubEntityId = @SubEntityId, FullPath = @FullPath, Deleted = @Deleted
		WHERE (EntityNodeId = @EntityNodeId);

		
		SELECT EntityNodeId, ParentEntityNodeId, [Name], OrderNumber, OrganizationId, InstanceId, EntityId, EntityNodeTypeId, SubEntityId, SubEntityLocalId, FullPath, Deleted
		FROM [dbo].[Mc_EntityNode]
		WHERE (EntityNodeId = @EntityNodeId);
	END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateEntityNodesRelatedEntityNodes]
(
	@EntityNodesRelatedEntityNodesId uniqueidentifier,
	@EntityNodeId uniqueidentifier,
	@RelatedEntityNodeId uniqueidentifier,
	@EntityId uniqueidentifier,
	@RelationType int
)
AS
BEGIN
	SET NOCOUNT OFF;
	DECLARE @Err int

	UPDATE [Mc_EntityNodesRelatedEntityNodes]
	SET
		[EntityNodeId] = @EntityNodeId,
		[RelatedEntityNodeId] = @RelatedEntityNodeId,
		[EntityId] = @EntityId,
		[RelationType] = @RelationType
	WHERE
		[EntityNodesRelatedEntityNodesId] = @EntityNodesRelatedEntityNodesId

	SET @Err = @@Error

	RETURN @Err
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateEntityNodeType]
(
	@EntityNodeTypeId uniqueidentifier,
	@EntityId uniqueidentifier,
	@Name nvarchar(255),
	@OrderNumber int,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [dbo].[Mc_EntityNodeType]
	SET [Name] = @Name,  EntityId = @EntityId, OrderNumber = @OrderNumber, OrganizationId = @OrganizationId, InstanceId = @InstanceId, Deleted = @Deleted
	WHERE (EntityNodeTypeId = @EntityNodeTypeId);

	SELECT EntityNodeTypeId, EntityId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted
	FROM [dbo].[Mc_EntityNodeType]
	WHERE (EntityNodeTypeId = @EntityNodeTypeId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateGroup]
(
	@GroupId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@BuiltIn bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Group 
	SET OrganizationId = @OrganizationId, [Name] = @Name, [Description] = @Description, BuiltIn = @BuiltIn, Deleted = @Deleted 
	WHERE (GroupId = @GroupId);
	
	SELECT GroupId, OrganizationId, [Name], [Description], BuiltIn, Deleted 
	FROM dbo.Mc_Group 
	WHERE (GroupId = @GroupId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_UpdateGroupInstanceAction]
(
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@ActionId uniqueidentifier,
	@Enabled bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_GroupsInstancesActions 
	SET [Enabled] = @Enabled 
	WHERE (ActionId = @ActionId) AND (InstanceId = @InstanceId) AND (GroupId = @GroupId);
	
	SELECT GroupId, InstanceId, ActionId, [Enabled] 
	FROM dbo.Mc_GroupsInstancesActions 
	WHERE (ActionId = @ActionId) AND (InstanceId = @InstanceId) AND (GroupId = @GroupId);
END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Mc_UpdateGroupInstanceRole]
(
	@GroupId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@RoleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_GroupsInstancesRoles 
	SET GroupId = @GroupId, InstanceId = @InstanceId, RoleId = @RoleId 
	WHERE (GroupId = @GroupId) AND (InstanceId = @InstanceId);
	
	SELECT GroupId, InstanceId, RoleId 
	FROM dbo.Mc_GroupsInstancesRoles 
	WHERE (InstanceId = @InstanceId) AND (GroupId = @GroupId);
END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateInstance]
(
	@InstanceId uniqueidentifier,
	@PseudoId varchar(6),
	@OrganizationId uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@EnableSignUpUser bit,
	@ExternalId nvarchar(255),
	@WorkingDays char(7),
	@Active bit,
	@CanceledTime datetime,
	@Trial bit,
	@Beta bit,
	@Deleted bit,
	@TimeZoneId nvarchar(255),
	@TimeFormat int
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Instance 
	SET PseudoId  = @PseudoId, OrganizationId = @OrganizationId, [Name] = @Name, [Description] = @Description, EnableSignUpUser = @EnableSignUpUser, ExternalId = @ExternalId, WorkingDays = @WorkingDays, Active = @Active, CanceledTime = @CanceledTime, Trial = @Trial, Beta = @Beta, Deleted = @Deleted, TimeZoneId = @TimeZoneId, TimeFormat = @TimeFormat
	WHERE (InstanceId = @InstanceId);
	
	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, ExternalId, WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime, TimeZoneId, TimeFormat
	FROM dbo.Mc_Instance 
	WHERE (InstanceId = @InstanceId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateOrganizationUser]
(
	@OrganizationId uniqueidentifier,
	@UserId uniqueidentifier,
	@OrganizationAdministrator bit,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_OrganizationsUsers 
	SET OrganizationAdministrator = (CASE WHEN @OrganizationAdministrator IS NULL THEN OrganizationAdministrator ELSE @OrganizationAdministrator END)
		, Active = (CASE WHEN @Active IS NULL THEN Active ELSE @Active END)
	WHERE (OrganizationId = @OrganizationId) AND (UserId = @UserId);

	SELECT OrganizationId, UserId, OrganizationAdministrator, Active
	FROM dbo.Mc_OrganizationsUsers
	WHERE (OrganizationId = @OrganizationId) AND (UserId = @UserId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[Mc_UpdateRecurringSchedule]
(
	@RecurringScheduleId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier=null,
	@LocalEntityType nvarchar(50),
	@LocalEntityId nvarchar(255),
	@Name nvarchar(255),
	@StartDate datetime,
	@EndDate datetime,
	@RecurrenceRule nvarchar(1024),
	@UpdatedTime datetime,
	@UpdatedBy uniqueidentifier,
	@Deleted bit
)
as
begin
	set nocount off;

	if exists(select 'true' 
		from [dbo].[Mc_RecurringSchedule]
		where RecurringScheduleId = @RecurringScheduleId)
	begin
		update [dbo].[Mc_RecurringSchedule]
		set		[LocalEntityType] = @LocalEntityType
				,[LocalEntityId] = @LocalEntityId
				,[Name] = @Name
				,[StartDate] = @StartDate
				,[EndDate] = @EndDate
				,[RecurrenceRule] = @RecurrenceRule
				,[UpdatedTime] = @UpdatedTime
				,[UpdatedBy] = @UpdatedBy
				,[Deleted] = @Deleted
		where	[RecurringScheduleId] = @RecurringScheduleId;
	end
	else 
	begin
		insert into [dbo].[Mc_RecurringSchedule]
			   ([RecurringScheduleId]
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
			   ,[Deleted])
		 values
			   (@RecurringScheduleId
			   ,@OrganizationId
			   ,@InstanceId
			   ,@LocalEntityType
			   ,@LocalEntityId
			   ,@Name
			   ,@StartDate
			   ,@EndDate
			   ,@RecurrenceRule
			   ,@UpdatedTime
			   ,@UpdatedBy
			   ,@Deleted);
	end

	select [RecurringScheduleId]
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
	from [Mc_RecurringSchedule] as RS
	where RS.RecurringScheduleId = @RecurringScheduleId;
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateRule]
(
	@RuleId uniqueidentifier,
	@RuleEngineId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier = NULL,
	@Name nvarchar(255),
	@DisplayName nvarchar(255),
	@UsedQty int,
	@LastUsedUser uniqueidentifier,
	@LastUsedDate datetime,
	@CreatedBy uniqueidentifier,
	@CreatedDate datetime,
	@OrderNumber int,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [Mc_Rule]
	SET
		[RuleEngineId] = @RuleEngineId,
		[OrganizationId] = @OrganizationId,
		[InstanceId] = @InstanceId,
		[Name] = @Name,
		[DisplayName] = @DisplayName,
		[UsedQty] = @UsedQty,
		[LastUsedUser] = @LastUsedUser,
		[LastUsedDate] = @LastUsedDate,
		[CreatedDate] = @CreatedDate,
		[CreatedBy] = @CreatedBy,
		[OrderNumber] = @OrderNumber,
		[Active] = @Active
	WHERE
		[RuleId] = @RuleId;

	SELECT
		[RuleId]
	  ,[RuleEngineId]
	  ,[OrganizationId]
	  ,[InstanceId]
	  ,[Name]
	  ,[DisplayName]
	  ,[UsedQty]
	  ,[LastUsedUser]
	  ,[LastUsedDate]
	  ,[CreatedDate]
	  ,[CreatedBy]
	  ,[OrderNumber]
	  ,[Active]
	FROM [Mc_Rule]
	WHERE
		([RuleId] = @RuleId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[Mc_UpdateRuleOrder]
(
	@RuleId uniqueidentifier,
	@OrderNumber int
)
as
begin

	set NOCOUNT OFF

	UPDATE [Mc_Rule]
	SET [OrderNumber] = @OrderNumber
	WHERE [RuleId] = @RuleId;
	
end

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateRuleParameter]
(
	@RuleParameterId uniqueidentifier,
	@RuleId uniqueidentifier,
	@EntityNodeTypeId uniqueidentifier = NULL,
	@IsInputParameter bit,
	@IsEntity bit,
	@FieldName nvarchar(255),
	@FullName nvarchar(512),
	@TypeName nvarchar(255) = NULL,
	@Term nvarchar(50),
	@Value sql_variant
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [Mc_RuleParameters]
	SET
		[RuleId] = @RuleId,
		[EntityNodeTypeId] = @EntityNodeTypeId,
		[IsInputParameter] = @IsInputParameter,
		[IsEntity] = @IsEntity,
		[FieldName] = @FieldName,
		[FullName] = @FullName,
		[TypeName] = @TypeName,
		[Term] = @Term,
		[Value] = @Value
	WHERE
		[RuleParameterId] = @RuleParameterId

	SELECT
		[RuleParameterId],
		[RuleId],
		[EntityNodeTypeId],
		[IsInputParameter],
		[IsEntity],
		[FieldName],
		[FullName],
		[TypeName],
		[Term],
		[Value]
	FROM [Mc_RuleParameters]
	WHERE
		([RuleParameterId] = @RuleParameterId)
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateRuleUses]
(
	@RuleId uniqueidentifier,
	@LastUsedUser uniqueidentifier,
	@LastUsedDate datetime
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [Mc_Rule]
	SET
		[UsedQty] = [UsedQty] + 1,
		[LastUsedUser] = @LastUsedUser,
		[LastUsedDate] = @LastUsedDate
	WHERE
		[RuleId] = @RuleId;

	SELECT
		[RuleId]
	  ,[OrganizationId]
	  ,[InstanceId]
	  ,[Name]
	  ,[DisplayName]
	  ,[UsedQty]
	  ,[LastUsedUser]
	  ,[LastUsedDate]
	  ,[CreatedDate]
	  ,[CreatedBy]
	  ,[Active]
	FROM [Mc_Rule]
	WHERE
		([RuleId] = @RuleId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateSettingValue]
(
	@SettingValueId uniqueidentifier,
	@SettingId uniqueidentifier,
	@Value nvarchar(max),
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@GroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_SettingsValues 
	SET SettingId = @SettingId, Value = @Value, OrganizationId = @OrganizationId, InstanceId = @InstanceId, GroupId = @GroupId 
	WHERE (SettingValueId = @SettingValueId);
	
	SELECT SettingValueId, SettingId, Value, OrganizationId, InstanceId, GroupId 
	FROM dbo.Mc_SettingsValues 
	WHERE (SettingValueId = @SettingValueId);
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateUser]
(
	@UserId uniqueidentifier,
	@Email nvarchar(255),
	@FirstName nvarchar(255),
	@LastName nvarchar(255),
	@MiddleName nvarchar(255),
	@Phone nvarchar(20),
    @MobilePhone nvarchar(20),
    @Fax nvarchar(20),
	@Title nvarchar(30),
	@Department nvarchar(255),
	@Street nvarchar(255),
	@Street2 nvarchar(255),
	@City nvarchar(255),
	@State nvarchar(255),
	@PostalCode nvarchar(20),
	@Country nvarchar(255),
	@LastLoginDate datetime,
	@Deleted bit,
	@TimeZoneId nvarchar(100),
	@TimeFormat int
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_User 
	SET UserId = @UserId, Email = ISNULL(@Email, Email), FirstName = ISNULL(@FirstName, FirstName), LastName = ISNULL(@LastName, LastName), MiddleName = ISNULL(@MiddleName, MiddleName)
		, Phone = ISNULL(@Phone, Phone), MobilePhone = ISNULL(@MobilePhone, MobilePhone), Fax = ISNULL(@Fax, Fax), Title = ISNULL(@Title, Title), Department = ISNULL(@Department, Department)
		, Street = ISNULL(@Street, Street), Street2 = ISNULL(@Street2, Street2), City = ISNULL(@City, City), [State] = ISNULL(@State, [State])
		, PostalCode = ISNULL(@PostalCode, PostalCode), Country = ISNULL(@Country, Country)
		, LastLoginDate = ISNULL(@LastLoginDate, LastLoginDate), Deleted = ISNULL(@Deleted, Deleted)
		, TimeZoneId = @TimeZoneId, TimeFormat = @TimeFormat
	WHERE (UserId = @UserId);
	
	SELECT UserId, Email, FirstName, LastName, MiddleName, Phone, MobilePhone, Fax, Title, Department, Street, Street2, City, [State], PostalCode, Country, LastLoginDate, Deleted, TimeZoneId, TimeFormat
	FROM dbo.Mc_User 
	WHERE (UserId = @UserId);
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mc_UpdateUserInstance]
(
	@UserId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@Active bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_UsersInstances 
	SET Active = @Active
	WHERE (UserId = @UserId) AND (InstanceId = @InstanceId);
	
	SELECT UserId, InstanceId, Active 
	FROM dbo.Mc_UsersInstances 
	WHERE (UserId = @UserId) AND (InstanceId = @InstanceId);
END

GO
