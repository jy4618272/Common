BEGIN TRANSACTION
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	INSERT INTO [dbo].[Mc_Action]([ActionId], [ParentActionId], [ActionTypeId], [Name], [Description], [IconUrl], [ButtonIconUrl], [NavigateUrl], [OrderNumber], [ClassFullName], [AuthenticationRequired], [InstanceRequired], [Visible], [ShowInDetailMenu], [ShowChildrenInDetailMenu], [GroupInDetailMenu], [HighlightInDetailMenu], [BuiltIn], [Deleted]) VALUES ('{291F64C6-EF01-4D22-864D-AE3B4FF92D38}', '{00000000-0000-0000-0000-000000000009}', 1, N'Custom URLs', N'Manage the Custom URLs', N'', N'', N'/Resources.Micajah.Common/Pages/Admin/CustomUrls.aspx', 0, N'Micajah.Common.Bll.Providers.ActionProvider', 1, 0, 1, 1, 0, 0, 0, 1, 0)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	CREATE TABLE [dbo].[Mc_CustomUrl] (
	   [CustomUrlId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Mc_CustomUrl_CustomUrlId] DEFAULT (newid()),
	   [OrganizationId] [uniqueidentifier] NOT NULL,
	   [InstanceId] [uniqueidentifier] NULL,
	   [FullCustomUrl] [nvarchar] (1024) NOT NULL CONSTRAINT [DF_Mc_CustomUrl_FullCustomUrl] DEFAULT (''),
	   [PartialCustomUrl] [nvarchar] (1024) NOT NULL CONSTRAINT [DF_Mc_CustomUrl_PartialCustomUrl] DEFAULT (''),
	 CONSTRAINT [PK_Mc_CustomUrl] PRIMARY KEY CLUSTERED 
	(
		[CustomUrlId] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_Mc_CustomUrl_Mc_Organization')
      ALTER TABLE [dbo].[Mc_CustomUrl] ADD CONSTRAINT [FK_Mc_CustomUrl_Mc_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Mc_Organization] ([OrganizationId])

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_DeleteCustomUrl]
(
	@CustomUrlId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM dbo.Mc_CustomUrl
	WHERE CustomUrlId = @CustomUrlId;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetCustomUrl]
(
	@CustomUrlId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE (CustomUrlId = @CustomUrlId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetCustomUrls]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@FullCustomUrl nvarchar(1024),
	@PartialCustomUrl nvarchar(1024)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE 
		(OrganizationId = @OrganizationId)
		AND (	(InstanceId = @InstanceId)
				OR  ((@InstanceId IS NULL) AND (InstanceId IS NULL))
		)
	UNION
	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE
		(	(FullCustomUrl = @FullCustomUrl) 
			AND (@FullCustomUrl IS NOT NULL)
		)
		OR 	(	(PartialCustomUrl = @PartialCustomUrl)
				AND (@PartialCustomUrl IS NOT NULL)
		);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_GetCustomUrlsByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE (OrganizationId = @OrganizationId);
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_InsertCustomUrl]
(
	@CustomUrlId uniqueidentifier,
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@FullCustomUrl nvarchar(1024),
	@PartialCustomUrl nvarchar(1024)
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_CustomUrl (CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl)
	VALUES (@CustomUrlId, @OrganizationId, @InstanceId, @FullCustomUrl, @PartialCustomUrl);
	
	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE CustomUrlId = @CustomUrlId;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('CREATE PROCEDURE [dbo].[Mc_UpdateCustomUrl]
(
	@CustomUrlId uniqueidentifier,
	@FullCustomUrl nvarchar(1024),
	@PartialCustomUrl nvarchar(1024)
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_CustomUrl
	SET FullCustomUrl = @FullCustomUrl, PartialCustomUrl = @PartialCustomUrl
	WHERE CustomUrlId = @CustomUrlId;
	
	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE CustomUrlId = @CustomUrlId;
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	COMMIT TRANSACTION
