BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Organization
		DROP CONSTRAINT FK_Mc_Organization_Mc_Database

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Organization
		DROP CONSTRAINT DF_Mc_Organization_OrganizationId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Organization
		DROP CONSTRAINT DF_Mc_Organization_PseudoId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Organization
		DROP CONSTRAINT DF_Mc_Organization_Description

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Organization
		DROP CONSTRAINT DF_Mc_Organization_WebsiteUrl

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Organization
		DROP CONSTRAINT DF_Mc_Organization_LdapServerAddress

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Organization
		DROP CONSTRAINT DF_Mc_Organization_LdapServerPort

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Organization
		DROP CONSTRAINT DF_Mc_Organization_LdapDomain

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Organization
		DROP CONSTRAINT DF_Mc_Organization_LdapUserName

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Organization
		DROP CONSTRAINT DF_Mc_Organization_LdapPassword

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Organization
		DROP CONSTRAINT DF_Mc_Organization_LdapDomains

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Organization
		DROP CONSTRAINT DF_Mc_Organization_GraceDays

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Organization
		DROP CONSTRAINT DF_Mc_Organization_Active

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Organization
		DROP CONSTRAINT DF_Mc_Organization_Deleted

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE dbo.Tmp_Mc_Organization
	(
	OrganizationId uniqueidentifier NOT NULL,
	PseudoId varchar(6) NOT NULL,
	Name nvarchar(255) NOT NULL,
	Description nvarchar(255) NOT NULL,
	WebsiteUrl nvarchar(2048) NOT NULL,
	DatabaseId uniqueidentifier NULL,
	FiscalYearStartDay int NULL,
	FiscalYearStartMonth int NULL,
	WeekStartsDay int NULL,
	LdapServerAddress nvarchar(255) NOT NULL,
	LdapServerPort nvarchar(50) NOT NULL,
	LdapDomain nvarchar(255) NOT NULL,
	LdapUserName nvarchar(255) NOT NULL,
	LdapPassword nvarchar(255) NOT NULL,
	LdapDomains nvarchar(2048) NOT NULL,
	ExpirationTime datetime NULL,
	GraceDays int NOT NULL,
	ExternalId nvarchar(255) NOT NULL,
	Active bit NOT NULL,
	CanceledTime datetime NULL,
	Trial bit NOT NULL,
	Beta bit NOT NULL,
	Deleted bit NOT NULL,
	CreatedTime datetime NULL
	)  ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Organization ADD CONSTRAINT
		DF_Mc_Organization_OrganizationId DEFAULT (newid()) FOR OrganizationId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Organization ADD CONSTRAINT
		DF_Mc_Organization_PseudoId DEFAULT ('') FOR PseudoId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Organization ADD CONSTRAINT
		DF_Mc_Organization_Description DEFAULT (N'') FOR Description

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Organization ADD CONSTRAINT
		DF_Mc_Organization_WebsiteUrl DEFAULT (N'') FOR WebsiteUrl

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Organization ADD CONSTRAINT
		DF_Mc_Organization_LdapServerAddress DEFAULT (N'') FOR LdapServerAddress

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Organization ADD CONSTRAINT
		DF_Mc_Organization_LdapServerPort DEFAULT (N'') FOR LdapServerPort

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Organization ADD CONSTRAINT
		DF_Mc_Organization_LdapDomain DEFAULT (N'') FOR LdapDomain

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Organization ADD CONSTRAINT
		DF_Mc_Organization_LdapUserName DEFAULT (N'') FOR LdapUserName

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Organization ADD CONSTRAINT
		DF_Mc_Organization_LdapPassword DEFAULT (N'') FOR LdapPassword

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Organization ADD CONSTRAINT
		DF_Mc_Organization_LdapDomains DEFAULT (N'') FOR LdapDomains

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Organization ADD CONSTRAINT
		DF_Mc_Organization_GraceDays DEFAULT ((0)) FOR GraceDays

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Organization ADD CONSTRAINT
		DF_Mc_Organization_ExternalId DEFAULT (N'') FOR ExternalId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Organization ADD CONSTRAINT
		DF_Mc_Organization_Active DEFAULT ((1)) FOR Active

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Organization ADD CONSTRAINT
		DF_Mc_Organization_Trial DEFAULT ((0)) FOR Trial

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Organization ADD CONSTRAINT
		DF_Mc_Organization_Beta DEFAULT ((0)) FOR Beta

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_Organization ADD CONSTRAINT
		DF_Mc_Organization_Deleted DEFAULT ((0)) FOR Deleted

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS(SELECT * FROM dbo.Mc_Organization)
		 EXEC('INSERT INTO dbo.Tmp_Mc_Organization (OrganizationId, PseudoId, Name, Description, WebsiteUrl, DatabaseId, FiscalYearStartDay, FiscalYearStartMonth, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains, ExpirationTime, GraceDays, Active, Deleted)
			SELECT OrganizationId, PseudoId, Name, Description, WebsiteUrl, DatabaseId, FiscalYearStartDay, FiscalYearStartMonth, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains, ExpirationTime, GraceDays, Active, Deleted FROM dbo.Mc_Organization WITH (HOLDLOCK TABLOCKX)')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_CustomUrl
		DROP CONSTRAINT FK_Mc_CustomUrl_Mc_Organization

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_EmailSuffix
		DROP CONSTRAINT FK_Mc_EmailSuffix_Mc_Organization

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_InvitedLogin
		DROP CONSTRAINT FK_Mc_InvitedLogin_Mc_Organization

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_OrganizationsLogins
		DROP CONSTRAINT FK_Mc_OrganizationsLogins_Mc_Organization

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP TABLE dbo.Mc_Organization

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXECUTE sp_rename N'dbo.Tmp_Mc_Organization', N'Mc_Organization', 'OBJECT' 

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Organization ADD CONSTRAINT
	PK_Mc_Organization PRIMARY KEY CLUSTERED 
	(
	OrganizationId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_Organization ADD CONSTRAINT
	FK_Mc_Organization_Mc_Database FOREIGN KEY
	(
	DatabaseId
	) REFERENCES dbo.Mc_Database
	(
	DatabaseId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_OrganizationsLogins ADD CONSTRAINT
	FK_Mc_OrganizationsLogins_Mc_Organization FOREIGN KEY
	(
	OrganizationId
	) REFERENCES dbo.Mc_Organization
	(
	OrganizationId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_InvitedLogin ADD CONSTRAINT
	FK_Mc_InvitedLogin_Mc_Organization FOREIGN KEY
	(
	OrganizationId
	) REFERENCES dbo.Mc_Organization
	(
	OrganizationId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_EmailSuffix ADD CONSTRAINT
	FK_Mc_EmailSuffix_Mc_Organization FOREIGN KEY
	(
	OrganizationId
	) REFERENCES dbo.Mc_Organization
	(
	OrganizationId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_CustomUrl ADD CONSTRAINT
	FK_Mc_CustomUrl_Mc_Organization FOREIGN KEY
	(
	OrganizationId
	) REFERENCES dbo.Mc_Organization
	(
	OrganizationId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertOrganization]
(
	@OrganizationId uniqueidentifier,
	@PseudoId varchar(6),
	@Name nvarchar(255),
	@Description nvarchar(255),
	@WebsiteUrl nvarchar(2048),
	@DatabaseId uniqueidentifier,
	@FiscalYearStartMonth int,
	@FiscalYearStartDay int,
	@WeekStartsDay int,
	@ExpirationTime datetime,
	@GraceDays int,
	@Active bit,
	@CanceledTime datetime,
	@Trial bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Organization (OrganizationId, PseudoId, [Name], [Description], WebsiteUrl, DatabaseId, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, ExpirationTime, GraceDays, Active, CanceledTime, Trial, Deleted, CreatedTime) 
	VALUES (@OrganizationId, @PseudoId, @Name, @Description, @WebsiteUrl, @DatabaseId, @FiscalYearStartMonth, @FiscalYearStartDay, @WeekStartsDay, @ExpirationTime, @GraceDays, @Active, @CanceledTime, @Trial, @Deleted, GETDATE());
	
	SELECT OrganizationId, PseudoId, [Name], [Description], WebsiteUrl, DatabaseId
		, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword
		, ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Deleted, CreatedTime 
	FROM dbo.Mc_Organization 
	WHERE (OrganizationId = @OrganizationId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizations]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT OrganizationId, PseudoId, [Name], [Description], WebsiteUrl, DatabaseId
		, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains
		, ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime 
	FROM dbo.Mc_Organization
	WHERE (Deleted = 0);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizationsByLoginId]
(  
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT o.OrganizationId, o.PseudoId, o.[Name], o.[Description], o.WebsiteUrl, o.DatabaseId
		, o.FiscalYearStartMonth, o.FiscalYearStartDay, o.WeekStartsDay, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword, o.LdapDomains
		, o.ExpirationTime, o.GraceDays, o.ExternalId, o.Active, o.CanceledTime, o.Trial, o.Beta, o.Deleted, o.CreatedTime 
	FROM dbo.Mc_OrganizationsLogins AS ol
	INNER JOIN dbo.Mc_Organization AS o
		ON (ol.OrganizationId = o.OrganizationId) AND (ol.LoginId = @LoginId) AND (o.Deleted = 0) AND (o.Active = 1) AND (ol.Active = 1);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateOrganization]
(
	@OrganizationId uniqueidentifier,
	@PseudoId varchar(6),
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
	@LdapDomains nvarchar(2048),
	@ExpirationTime datetime,
	@GraceDays int,
	@Active bit,
	@CanceledTime datetime,
	@Trial bit,
	@Beta bit,
	@Deleted bit
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Organization
	SET PseudoId  = @PseudoId, [Name] = @Name, [Description] = @Description, WebsiteUrl = @WebsiteUrl, DatabaseId = @DatabaseId
		, FiscalYearStartMonth = @FiscalYearStartMonth, FiscalYearStartDay = @FiscalYearStartDay, WeekStartsDay = @WeekStartsDay
		, LdapServerAddress = @LdapServerAddress, LdapServerPort = @LdapServerPort, LdapDomain = @LdapDomain, LdapUserName = @LdapUserName, LdapPassword = @LdapPassword, LdapDomains = @LdapDomains
		, ExpirationTime = @ExpirationTime, GraceDays = @GraceDays, Active = @Active, CanceledTime = @CanceledTime, Trial = @Trial, Beta = @Beta, Deleted = @Deleted
	WHERE (OrganizationId = @OrganizationId);

	SELECT OrganizationId, PseudoId, [Name], [Description], WebsiteUrl, DatabaseId
		, FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains
		, ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime 
	FROM dbo.Mc_Organization
	WHERE (OrganizationId = @OrganizationId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizationsByLdapDomain]
(
	@LdapDomain nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT o.OrganizationId, o.PseudoId, o.[Name], o.[Description], o.WebsiteUrl, o.DatabaseId
		, o.FiscalYearStartMonth, o.FiscalYearStartDay, o.WeekStartsDay, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword, o.LdapDomains
		, o.ExpirationTime, o.GraceDays, o.ExternalId, o.Active, o.CanceledTime, o.Trial, o.Beta, o.Deleted, o.CreatedTime 
	FROM dbo.Mc_Organization AS o
	WHERE (o.Deleted = 0) AND (o.Active = 1) AND ('','' + o.LdapDomains + '','' like ''%,'' + @LdapDomain + '',%'')
	ORDER BY o.LdapServerAddress DESC;
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
