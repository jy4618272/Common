BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('{A3223AB8-D9E7-437A-AE0E-14164C0F90B7}', '{00000000-0000-0000-0000-000000000009}', 1, N'Ldap Integration', N'Settings to integrate your Ldap or Active Directory server for authentication', N'', N'', N'/mc/Admin/OrganizationLdapSettings.aspx', 0, N'', 1, 0, 1, 1, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization] ADD	[LdapServerAddress] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_Organization_LdapServerAddress] DEFAULT (N'')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization] ADD	[LdapServerPort] [nvarchar] (50) NOT NULL CONSTRAINT [DF_Mc_Organization_LdapServerPort] DEFAULT (N'')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization] ADD	[LdapDomain] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_Organization_LdapDomain] DEFAULT (N'')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization] ADD	[LdapUserName] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_Organization_LdapUserName] DEFAULT (N'')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Organization] ADD	[LdapPassword] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_Organization_LdapPassword] DEFAULT (N'')
	
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateOrganization]  
(  
	@OrganizationId uniqueidentifier,  
	@Name nvarchar(255),  
	@Description nvarchar(255),  
	@WebsiteUrl nvarchar(2048),  
	@DatabaseId uniqueidentifier,  
	@FiscalYearStartMonth int,  
	@FiscalYearStartDay int,  
	@WeekStartsDay int,  
	@LdapServerAddress nvarchar(255),  
	@LdapServerPort nvarchar(50),  
	@LdapDomain nvarchar(255),  
	@LdapUserName nvarchar(255),  
	@LdapPassword nvarchar(255),  
	@Deleted bit  
)  
AS  
BEGIN  
	SET NOCOUNT OFF;  

	UPDATE dbo.Mc_Organization   
	SET [Name] = @Name, [Description] = @Description, WebsiteUrl = @WebsiteUrl, DatabaseId = @DatabaseId, FiscalYearStartMonth = @FiscalYearStartMonth, FiscalYearStartDay = @FiscalYearStartDay, WeekStartsDay = @WeekStartsDay, [LdapServerAddress] = @LdapServerAddress, [LdapServerPort] = @LdapServerPort, [LdapDomain] = @LdapDomain, [LdapUserName] = @LdapUserName, [LdapPassword] = @LdapPassword, Deleted = @Deleted   
	WHERE (OrganizationId = @OrganizationId);  

	SELECT OrganizationId, [Name], [Description], WebsiteUrl, DatabaseId, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, Deleted  
	FROM dbo.Mc_Organization   
	WHERE (OrganizationId = @OrganizationId);  
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizations]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT OrganizationId, [Name], [Description], WebsiteUrl, DatabaseId, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, Deleted 
	FROM dbo.Mc_Organization 
	WHERE (Deleted = 0);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('{14C1A008-D65D-40BB-BFF8-D077BA1DE995}', '{00000000-0000-0000-0000-000000000011}', 1, N'Ldap Mappings', N'Ldap Mappings', N'', N'', N'/mc/Admin/LdapGroupMappings.aspx', 0, N'', 0, 0, 0, 0, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_GroupMappings] (
		[GroupId] [uniqueidentifier] NOT NULL,
		[OrganizationId] [uniqueidentifier] NOT NULL,
		[GroupName] [nvarchar] (255) NOT NULL,
		[LdapDomainId] [uniqueidentifier] NOT NULL,
		[LdapDomainName] [nvarchar] (255) NOT NULL,
		[LdapGroupId] [uniqueidentifier] NOT NULL,
		[LdapGroupName] [nvarchar] (255) NOT NULL
)
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_GroupMappings] ADD CONSTRAINT [PK_Mc_GroupMappings] PRIMARY KEY CLUSTERED ([GroupId], [LdapDomainId], [LdapGroupId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteGroupMapping]
(
	@GroupId uniqueidentifier,
	@LdapDomainId uniqueidentifier,
	@LdapGroupId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_GroupMappings
	WHERE GroupId = @GroupId AND LdapDomainId = @LdapDomainId AND LdapGroupId = @LdapGroupId;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetGroupMappings]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT GroupId, OrganizationId, [GroupName], LdapDomainId, [LdapDomainName], LdapGroupId, [LdapGroupName] 
	FROM dbo.Mc_GroupMappings 
	WHERE (OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertGroupMappings]
(
	@GroupId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@GroupName nvarchar(255),
	@LdapDomainId uniqueidentifier,
	@LdapDomainName nvarchar(255),
	@LdapGroupId uniqueidentifier,
	@LdapGroupName nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_GroupMappings (GroupId, OrganizationId, [GroupName], LdapDomainId, [LdapDomainName], LdapGroupId, [LdapGroupName]) 
	VALUES (@GroupId, @OrganizationId, @GroupName, @LdapDomainId, @LdapDomainName, @LdapGroupId, @LdapGroupName);
	
	SELECT GroupId, OrganizationId, [GroupName], LdapDomainId, [LdapDomainName], LdapGroupId, [LdapGroupName] 
	FROM dbo.Mc_GroupMappings 
	WHERE (GroupId = @GroupId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION

