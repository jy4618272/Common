BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_EmailSuffix] (
	   [EmailSuffixId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Mc_EmailSuffix_EmailSuffixId] DEFAULT (newid()),
	   [OrganizationId] [uniqueidentifier] NOT NULL,
	   [InstanceId] [uniqueidentifier] NULL,
	   [EmailSuffixName] [nvarchar] (255) NOT NULL CONSTRAINT [DF_Mc_EmailSuffix_EmailSuffixName] DEFAULT (''),
	 CONSTRAINT [PK_Mc_EmailSuffix] PRIMARY KEY CLUSTERED 
	(
		[EmailSuffixId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_EmailSuffix_Mc_Organization')
      ALTER TABLE [dbo].[Mc_EmailSuffix] ADD CONSTRAINT [FK_Mc_EmailSuffix_Mc_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteEmailSuffix]
(
	@EmailSuffixId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_EmailSuffix
	WHERE EmailSuffixId = @EmailSuffixId;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetEmailSuffix]
(
	@EmailSuffixId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName 
	FROM dbo.Mc_EmailSuffix
	WHERE (EmailSuffixId = @EmailSuffixId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetEmailSuffixes]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@EmailSuffixName nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName 
	FROM dbo.Mc_EmailSuffix
	WHERE 
		(OrganizationId = @OrganizationId)
		AND (	(InstanceId = @InstanceId)
				OR  ((@InstanceId IS NULL) AND (InstanceId IS NULL))
		)
	UNION
	SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName 
	FROM dbo.Mc_EmailSuffix
	WHERE
		(	(EmailSuffixName = @EmailSuffixName) 
			AND (@EmailSuffixName IS NOT NULL)
		);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetEmailSuffixesByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName 
	FROM dbo.Mc_EmailSuffix
	WHERE (OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertEmailSuffix]
(
	@EmailSuffixId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@EmailSuffixName nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_EmailSuffix (EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName)
	VALUES (@EmailSuffixId, @OrganizationId, @InstanceId, @EmailSuffixName);
	
	SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName 
	FROM dbo.Mc_EmailSuffix
	WHERE EmailSuffixId = @EmailSuffixId;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateEmailSuffix]
(
	@EmailSuffixId uniqueidentifier,
	@EmailSuffixName nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_EmailSuffix
	SET EmailSuffixName = @EmailSuffixName 
	WHERE EmailSuffixId = @EmailSuffixId;
	
	SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName 
	FROM dbo.Mc_EmailSuffix
	WHERE EmailSuffixId = @EmailSuffixId;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
