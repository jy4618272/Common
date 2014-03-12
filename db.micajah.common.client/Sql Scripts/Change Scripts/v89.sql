BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_DeleteRule]
(
	@RuleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF

	DELETE FROM [Mc_Rule]
	WHERE [RuleId] = @RuleId
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_DeleteRuleParameter]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetAllEntityNodesRelatedEntityNodes]
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
		AND enren.EntityId = @EntityId AND (en.Deleted = 0 OR RelatedEntityNodeId = ''00000000-0000-0000-0000-000000000000'')

	SET @Err = @@Error

	RETURN @Err
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetAllRuleParameters]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetAllRules]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetAnotherAdministrator]
(
	@UserId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT TOP 1 u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	INNER JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId <> uo.UserId) AND (u.Deleted = 0) AND (uo.OrganizationId = @OrganizationId) AND (uo.OrganizationAdministrator = 1);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEntityField]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEntityFieldListValue]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEntityFieldListValues]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEntityFields]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEntityFieldsValues]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEntityFieldValues]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEntityNodes]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT EntityNodeId, ParentEntityNodeId, [Name], OrderNumber, OrganizationId, InstanceId, EntityId, EntityNodeTypeId, SubEntityId, SubEntityLocalId, FullPath, Deleted
	FROM [dbo].[Mc_EntityNode]
	WHERE (Deleted = 0)
	ORDER BY [Name]
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEntityNodesRelatedEntityNodes]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEntityNodeTypes]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT EntityNodeTypeId, EntityId, [Name], OrderNumber, OrganizationId, InstanceId, Deleted
	FROM [dbo].[Mc_EntityNodeType]
	WHERE (Deleted = 0);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetGroups]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetGroupsInstancesActions]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetGroupsInstancesRoles]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetInstances]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT InstanceId, PseudoId, OrganizationId, [Name], [Description], EnableSignUpUser, 
		ExternalId, UTCOffset, [DateFormat], WorkingDays, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime 
	FROM dbo.Mc_Instance 
	WHERE (OrganizationId = @OrganizationId) AND (Deleted = 0);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetMessages]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER procedure [dbo].[Mc_GetRecurringScheduleByEntityId]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER procedure [dbo].[Mc_GetRecurringScheduleByEntityType]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER procedure [dbo].[Mc_GetRecurringScheduleById]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER procedure [dbo].[Mc_GetRecurringScheduleByName]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER procedure [dbo].[Mc_GetRecurringScheduleByRecurrenceRule]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER procedure [dbo].[Mc_GetRecurringScheduleEntityTypes]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER procedure [dbo].[Mc_GetRecurringSchedules]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetRule]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetRuleByName]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetRuleParameter]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetSettingsValues]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetUser]
(
	@UserId uniqueidentifier,
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, uo.OrganizationId, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	LEFT JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId = uo.UserId) AND (uo.OrganizationId = @OrganizationId)
	WHERE (u.UserId = @UserId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetUserByEmail]
(
	@Email nvarchar(255),
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, uo.OrganizationId, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	LEFT JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId = uo.UserId) AND (uo.OrganizationId = @OrganizationId)
	WHERE (u.Email = @Email);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetUsers]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT u.UserId, u.Email, u.FirstName, u.LastName, u.MiddleName
		, u.Phone, u.MobilePhone, u.Fax, u.Title, u.Department, u.Street, u.Street2, u.City, u.[State], u.PostalCode, u.Country
		, u.LastLoginDate, u.Deleted, uo.OrganizationId, uo.OrganizationAdministrator, uo.Active
	FROM dbo.Mc_User AS u
	INNER JOIN dbo.Mc_OrganizationsUsers AS uo
		ON (u.UserId = uo.UserId) AND (u.Deleted = 0) AND (uo.OrganizationId = @OrganizationId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetUsersByRoles]
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
		, u.LastLoginDate, u.Deleted, uo.OrganizationId, uo.OrganizationAdministrator, uo.Active
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
			OR ((@InstanceId IS NOT NULL) AND (ISNULL(ui.Active, 1) = @Active))
			OR ((@InstanceId IS NULL) AND (uo.Active = @Active))
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
			WHERE ('','' + @InRoles + '','' LIKE ''%,'' + CAST(gir1.RoleId as varchar(50)) + '',%''))
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
			WHERE ('','' + @NotInRoles + '','' LIKE ''%,'' + CAST(gir2.RoleId as varchar(50)) + '',%''))
		);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetUsersGroups]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetUsersInstances]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertRule]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertRuleParameter]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateEntityNodesRelatedEntityNodes]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateRule]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER procedure [dbo].[Mc_UpdateRuleOrder]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateRuleParameter]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateRuleUses]
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
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
