BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
CREATE TABLE [dbo].[Mc_OrganizationsLdapGroups](
	[Id] [uniqueidentifier] NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[DomainId] [uniqueidentifier] NOT NULL,
	[Domain] [nvarchar](255) NOT NULL,
	[ObjectGUID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[DistinguishedName] [nvarchar](2048) NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Mc_OrganizationsLdapGroups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('
CREATE PROCEDURE [dbo].[Mc_DeleteOrganizationLdapGroup]
(
	@OrganizationId uniqueidentifier,
	@Domain nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_OrganizationsLdapGroups 
	WHERE (OrganizationId = @OrganizationId) AND (Domain = @Domain)
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('
CREATE PROCEDURE [dbo].[Mc_InsertOrganizationLdapGroup]
(
	@Id uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@DomainId uniqueidentifier,	
	@Domain nvarchar(255),
	@ObjectGUID uniqueidentifier,
	@Name nvarchar(255),
	@DistinguishedName nvarchar(2048),
	@CreatedTime datetime
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Mc_OrganizationsLdapGroups]([Id],[OrganizationId],[DomainId], [Domain],[ObjectGUID],[Name],[DistinguishedName],[CreatedTime])
    VALUES(@Id,@OrganizationId, @DomainId, @Domain, @ObjectGUID, @Name, @DistinguishedName, @CreatedTime)
END
')


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('
CREATE PROCEDURE [dbo].[Mc_GetOrganizationsLdapGroupsAll]
(
	@OrganizationId uniqueidentifier,
	@Domain nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT [Id]
      ,[OrganizationId]
	  ,[DomainId]
      ,[Domain]
      ,[ObjectGUID]
      ,[Name]
      ,[DistinguishedName]
      ,[CreatedTime]
	FROM [dbo].[Mc_OrganizationsLdapGroups]
	WHERE (OrganizationId = @OrganizationId) AND (Domain = @Domain)
	ORDER BY [Name]
END
')


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('
CREATE PROCEDURE [dbo].[Mc_GetOrganizationsLdapGroups]
(
	@OrganizationId uniqueidentifier,
	@Domain nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT TOP 25 [Id]
      ,[OrganizationId]
	  ,[DomainId]
      ,[Domain]
      ,[ObjectGUID]
      ,[Name]
      ,[DistinguishedName]
      ,[CreatedTime]
	FROM [dbo].[Mc_OrganizationsLdapGroups]
	WHERE (OrganizationId = @OrganizationId) AND (Domain = @Domain)
	ORDER BY [Name]
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('
CREATE PROCEDURE [dbo].[Mc_GetOrganizationsLdapGroupsDomains]
(
	@OrganizationId uniqueidentifier)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT 
		Domain
		,[DomainId]
		,CASE [Id]
			WHEN ''00000000-0000-0000-0000-000000000000'' THEN NEWID()
		END as [Id]
		,[OrganizationId]
		,[ObjectGUID]
		,[Name]
		,[DistinguishedName]
		,[CreatedTime]
	FROM
	(
		SELECT 
			DISTINCT Domain
			, [DomainId] as [DomainId]
			, ''00000000-0000-0000-0000-000000000000'' as [Id]
			, @OrganizationId as [OrganizationId]
			, ''00000000-0000-0000-0000-000000000000'' as [ObjectGUID]
			, '''' as [Name]
			, '''' as [DistinguishedName]
			, GETDATE() as [CreatedTime]
		FROM [dbo].[Mc_OrganizationsLdapGroups]
		WHERE (OrganizationId = @OrganizationId)
	) A	
	ORDER BY Domain
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('
CREATE PROCEDURE [dbo].[Mc_GetOrganizationsLdapGroupsByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT [Id]
      ,[OrganizationId]
	  ,[DomainId]
      ,[Domain]
      ,[ObjectGUID]
      ,[Name]
      ,[DistinguishedName]
      ,[CreatedTime]
	FROM [dbo].[Mc_OrganizationsLdapGroups]
	WHERE (OrganizationId = @OrganizationId)
	ORDER BY [Name]
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION


