BEGIN TRANSACTION

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL, XACT_ABORT ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Name] = N'My Profile', [Description] = N'Edit my name, email, phone and address information' WHERE [ActionId] = '00000000-0000-0000-0000-000000000022'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Description] = N'Edit my password' WHERE [ActionId] = '00000000-0000-0000-0000-000000000023'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Name] = N'User Profile', [Description] = N'Edit a user''s name, email, phone and address information' WHERE [ActionId] = '00000000-0000-0000-0000-000000000024'

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Description] = N'Edit user password' WHERE [ActionId] = '00000000-0000-0000-0000-000000000025'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	UPDATE [dbo].[Mc_Action] SET [Description] = N'Edit user''s groups' WHERE [ActionId] = '00000000-0000-0000-0000-000000000026'

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE TABLE [dbo].[Mc_Country](
	[CountryId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL
)')

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Country] ADD CONSTRAINT [PK_Mc_Country] PRIMARY KEY CLUSTERED ([CountryId])

IF @@ERROR <> 0
	IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	ALTER TABLE [dbo].[Mc_Country] ADD  CONSTRAINT [DF_Mc_Country_CountryId]  DEFAULT (newid()) FOR [CountryId]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetCountries]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT CountryId, [Name]
	FROM dbo.Mc_Country
	ORDER BY [Name];
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertCountry]
(
	@CountryId uniqueidentifier,
	@Name nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Country(CountryId, [Name])
	VALUES (@CountryId, @Name)

	SELECT CountryId, [Name]
	FROM dbo.Mc_Country
	WHERE (CountryId = @CountryId);
END')

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION