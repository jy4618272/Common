BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_EmailSuffix
		DROP CONSTRAINT FK_Mc_EmailSuffix_Mc_Organization

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_EmailSuffix
		DROP CONSTRAINT DF_Mc_EmailSuffix_EmailSuffixId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_EmailSuffix
		DROP CONSTRAINT DF_Mc_EmailSuffix_EmailSuffixName

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE dbo.Tmp_Mc_EmailSuffix
	(
		EmailSuffixId uniqueidentifier NOT NULL,
		OrganizationId uniqueidentifier NOT NULL,
		InstanceId uniqueidentifier NULL,
		EmailSuffixName nvarchar(1024) NOT NULL
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_EmailSuffix ADD CONSTRAINT
		DF_Mc_EmailSuffix_EmailSuffixId DEFAULT (newid()) FOR EmailSuffixId

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Tmp_Mc_EmailSuffix ADD CONSTRAINT
		DF_Mc_EmailSuffix_EmailSuffixName DEFAULT ('') FOR EmailSuffixName

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	IF EXISTS(SELECT * FROM dbo.Mc_EmailSuffix)
		 EXEC('INSERT INTO dbo.Tmp_Mc_EmailSuffix (EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName)
			SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName FROM dbo.Mc_EmailSuffix WITH (HOLDLOCK TABLOCKX)')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	DROP TABLE dbo.Mc_EmailSuffix

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXECUTE sp_rename N'dbo.Tmp_Mc_EmailSuffix', N'Mc_EmailSuffix', 'OBJECT' 

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE dbo.Mc_EmailSuffix ADD CONSTRAINT
		PK_Mc_EmailSuffix PRIMARY KEY CLUSTERED 
		(
			EmailSuffixId
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

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
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEmailSuffixes]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@EmailSuffixName nvarchar(1024)
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
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertEmailSuffix]
(
	@EmailSuffixId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@EmailSuffixName nvarchar(1024)
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
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateEmailSuffix]
(
	@EmailSuffixId uniqueidentifier,
	@EmailSuffixName nvarchar(1024)
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
	DROP PROCEDURE [dbo].[Mc_DeleteEmailSuffix]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteEmailSuffixes]  
(  
 @OrganizationId uniqueidentifier,
 @InstanceId uniqueidentifier
)  
AS  
BEGIN  
 SET NOCOUNT OFF;  

 IF @InstanceId IS NOT NULL
 BEGIN
   DELETE FROM dbo.Mc_EmailSuffix  
   WHERE InstanceId = @InstanceId;  
 END
 ELSE  
   DELETE FROM dbo.Mc_EmailSuffix  
   WHERE OrganizationId = @OrganizationId AND InstanceId IS NULL;  
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEmailSuffixesByOrganizationId]  
(  
 @OrganizationId uniqueidentifier  
)  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName   
 FROM dbo.Mc_EmailSuffix  
 WHERE ((OrganizationId = @OrganizationId) AND (InstanceId IS NULL))
 UNION
 SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName   
 FROM dbo.Mc_EmailSuffix  
 WHERE ((OrganizationId = @OrganizationId) AND (InstanceId IS NOT NULL));  
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetEmailSuffixesByInstanceId]  
(  
 @InstanceId uniqueidentifier  
)  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName   
 FROM dbo.Mc_EmailSuffix  
 WHERE (InstanceId = @InstanceId);  
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
