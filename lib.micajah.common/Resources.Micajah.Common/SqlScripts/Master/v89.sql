BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_DeleteViewState]
(
    @Now datetime
)
AS
BEGIN
    SET NOCOUNT OFF;
    
	DELETE dbo.Mc_ViewState
	WHERE ExpirationTime < @Now;
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetCountries]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT CountryId, [Name]
	FROM dbo.Mc_Country
	ORDER BY [Name];
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetCustomUrl]
(
	@CustomUrlId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE (CustomUrlId = @CustomUrlId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetCustomUrls]
(
	@OrganizationId uniqueidentifier,
	@InstanceId uniqueidentifier,
	@FullCustomUrl nvarchar(1024),
	@PartialCustomUrl nvarchar(1024)
)
AS
BEGIN
	SET NOCOUNT OFF;

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
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetCustomUrlsByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT CustomUrlId, OrganizationId, InstanceId, FullCustomUrl, PartialCustomUrl
	FROM dbo.Mc_CustomUrl
	WHERE (OrganizationId = @OrganizationId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetDatabases]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT DatabaseId, [Name], [Description], UserName, [Password], DatabaseServerId, Deleted 
	FROM dbo.Mc_Database
	WHERE (Deleted = 0);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetDatabaseServers]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT DatabaseServerId, [Name], InstanceName, Port, [Description], WebsiteId, Deleted 
	FROM dbo.Mc_DatabaseServer 
	WHERE (Deleted = 0);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEmailSuffix]
(
	@EmailSuffixId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName 
	FROM dbo.Mc_EmailSuffix
	WHERE (EmailSuffixId = @EmailSuffixId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEmailSuffixes]  
(  
 @OrganizationId uniqueidentifier,  
 @InstanceId uniqueidentifier,  
 @EmailSuffixName nvarchar(255)  
)  
AS  
BEGIN  
 SET NOCOUNT OFF;  
  
 SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName   
 FROM dbo.Mc_EmailSuffix  
 WHERE  
  ((@OrganizationId IS NULL) OR (OrganizationId = @OrganizationId)) AND  
  ((@InstanceId IS NULL) OR (InstanceId = @InstanceId)) AND 
  ((@EmailSuffixName IS NOT NULL) 
   AND ('','' + EmailSuffixName + '','' like ''%,'' + @EmailSuffixName + '',%'') 
  );  
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEmailSuffixesByInstanceId]  
(  
 @InstanceId uniqueidentifier  
)  
AS  
BEGIN  
 SET NOCOUNT OFF;  
  
 SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName   
 FROM dbo.Mc_EmailSuffix  
 WHERE (InstanceId = @InstanceId);  
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetEmailSuffixesByOrganizationId]  
(  
 @OrganizationId uniqueidentifier  
)  
AS  
BEGIN  
 SET NOCOUNT OFF;  
  
 SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName   
 FROM dbo.Mc_EmailSuffix  
 WHERE ((OrganizationId = @OrganizationId) AND (InstanceId IS NULL))
 UNION
 SELECT EmailSuffixId, OrganizationId, InstanceId, EmailSuffixName   
 FROM dbo.Mc_EmailSuffix  
 WHERE ((OrganizationId = @OrganizationId) AND (InstanceId IS NOT NULL));  
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetGroupMappings]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT GroupId, OrganizationId, [GroupName], LdapDomainId, [LdapDomainName], LdapGroupId, [LdapGroupName] 
	FROM dbo.Mc_GroupMappings 
	WHERE (OrganizationId = @OrganizationId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetInvitedLogin]
(
	@InvitedLoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT InvitedLoginId, LoginName, OrganizationId, GroupId, InvitedBy, CreatedTime
	FROM dbo.Mc_InvitedLogin
	WHERE (InvitedLoginId = @InvitedLoginId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetInvitedLoginsByOrganizationId]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT InvitedLoginId, LoginName, OrganizationId, GroupId, InvitedBy, CreatedTime
	FROM dbo.Mc_InvitedLogin
	WHERE (OrganizationId = @OrganizationId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetLdapServerDetails]
(
	@OrganizationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword 
	FROM Mc_Organization 
	WHERE Deleted = 0 AND Active = 1 AND 
		OrganizationId = @OrganizationId;
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetLogin]    
(    
	@LoginId uniqueidentifier,    
	@LoginName nvarchar(255),    
	@Password nvarchar(50)    
)    
AS    
BEGIN    
	SET NOCOUNT OFF;    

	SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.ProfileUpdated, l.SessionId, l.Deleted    
		, CASE WHEN EXISTS(    
		SELECT TOP 1 o.OrganizationId    
		FROM dbo.Mc_Organization AS o    
		INNER JOIN dbo.Mc_OrganizationsLogins AS ol    
			ON (o.OrganizationId = ol.OrganizationId) AND (ol.LoginId = l.LoginId) AND (o.Deleted = 0)    
		) THEN 1 ELSE 0 END AS OrganizationExists    
	FROM dbo.Mc_Login AS l    
	WHERE     
		((@LoginId IS NULL) OR (l.LoginId = @LoginId))    
		AND ((@LoginName IS NULL) OR (l.LoginName = @LoginName))    
		AND ((@Password IS NULL) OR (l.[Password] = @Password));    
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetLoginByDomainName]      
(      
 @DomainName nvarchar(255),   
 @UserAlias nvarchar(255),   
 @FirstName nvarchar(255),   
 @LastName nvarchar(255)   
)      
AS      
BEGIN      
 SET NOCOUNT OFF;      
       
 SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.Password, ol.OrganizationId, ol.Active,   
  ol.LdapDomain as ''UserLdapDomain'', ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId,   
  ol.LdapUserId, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword   
 FROM Mc_Login l   
  inner join Mc_OrganizationsLogins ol ON l.LoginId = ol.LoginId   
  inner join Mc_Organization o ON ol.OrganizationId = o.OrganizationId   
 WHERE l.Deleted = 0 AND o.Deleted = 0 AND o.Active = 1 AND   
  ((@DomainName is null) OR ((ol.LdapDomain = @DomainName) OR (ol.LdapDomainFull = @DomainName))) AND   
  (((@UserAlias is not null) AND (ol.LdapUserAlias = @UserAlias))   
  OR   
  (((@FirstName is not null) AND (@LastName is not null)) AND ((l.FirstName = @FirstName) AND (l.LastName = @LastName)))) 
 ORDER BY o.LdapServerAddress DESC;
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetLoginByEmail]    
(    
 @EmailAddress nvarchar(255)    
)    
AS    
BEGIN    
 SET NOCOUNT OFF;    
     
 SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.Password, ol.OrganizationId, ol.Active, 
  ol.LdapDomain as ''UserLdapDomain'', ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, 
  ol.LdapUserId, o.LdapServerAddress, o.LdapServerPort, o.LdapDomain, o.LdapUserName, o.LdapPassword 
 FROM Mc_Login l 
  inner join Mc_OrganizationsLogins ol ON l.LoginId = ol.LoginId 
  inner join Mc_Organization o ON ol.OrganizationId = o.OrganizationId 
 WHERE l.Deleted = 0 AND o.Deleted = 0 AND o.Active = 1 AND 
  (l.LoginName = @EmailAddress OR ol.LdapUPN = @EmailAddress)
 ORDER BY o.LdapServerAddress DESC;
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetLoginsByOrganizationId]      
(      
	@OrganizationId uniqueidentifier      
)      
AS      
BEGIN      
	SET NOCOUNT OFF;      

	SELECT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.ProfileUpdated, l.SessionId, l.Deleted
		, ol.OrganizationAdministrator, ol.LdapDomain, ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, ol.LdapUserId, ol.LdapOUPath, ol.Active      
	FROM dbo.Mc_OrganizationsLogins AS ol      
	INNER JOIN dbo.Mc_Login AS l      
		ON (ol.LoginId = l.LoginId) AND (ol.OrganizationId = @OrganizationId) AND (l.Deleted = 0);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizationLogin]    
(    
 @OrganizationId uniqueidentifier,    
 @LoginId uniqueidentifier    
)    
AS    
BEGIN    
 SET NOCOUNT OFF;    
    
 SELECT ol.OrganizationId, ol.LoginId, ol.OrganizationAdministrator, l.FirstName, l.LastName, ol.LdapDomain, ol.LdapDomainFull, ol.LdapUserAlias, ol.LdapUPN, ol.LdapSecurityId, ol.LdapUserId, ol.LdapOUPath, ol.Active    
 FROM dbo.Mc_OrganizationsLogins ol 
 INNER JOIN dbo.Mc_Login l 
	ON (ol.LoginId = l.LoginId) 
 WHERE (ol.OrganizationId = @OrganizationId) AND (ol.LoginId = @LoginId);    
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizations]  
AS  
BEGIN  
 SET NOCOUNT OFF;  
  
 SELECT OrganizationId, PseudoId, [Name], [Description], WebsiteUrl, DatabaseId  
  , FiscalYearStartMonth, FiscalYearStartDay, WeekStartsDay, LdapServerAddress, LdapServerPort, LdapDomain, LdapUserName, LdapPassword, LdapDomains  
  , ExpirationTime, GraceDays, ExternalId, Active, CanceledTime, Trial, Beta, Deleted, CreatedTime   
 FROM dbo.Mc_Organization   
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
	SET NOCOUNT OFF;

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
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetOrganizationsByLoginId]
(  
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;

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
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetResetPasswordRequest]
(
	@ResetPasswordRequestId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT ResetPasswordRequestId, LoginId, CreatedTime
	FROM dbo.Mc_ResetPasswordRequest
	WHERE (ResetPasswordRequestId = @ResetPasswordRequestId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetResetPasswordRequestsByLoginId]
(
	@LoginId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	SELECT ResetPasswordRequestId, LoginId, CreatedTime
	FROM dbo.Mc_ResetPasswordRequest
	WHERE (LoginId = @LoginId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetResource]
(
	@ResourceId uniqueidentifier,
	@Height int,
	@Width int,
	@Align int
)
AS
BEGIN
	SET NOCOUNT OFF;
	
	DECLARE @ResId uniqueidentifier;
	
	IF (@Height IS NOT NULL) OR (@Width IS NOT NULL)
	BEGIN
		IF EXISTS(SELECT 0 FROM dbo.Mc_Resource WHERE (ResourceId = @ResourceId) AND (UPPER(LEFT(ContentType, 6)) = ''IMAGE/''))
		BEGIN
			SELECT @ResId = ResourceId
			FROM dbo.Mc_Resource
			WHERE (ParentResourceId = @ResourceId)
				AND (((@Height IS NULL) AND (Height IS NULL)) OR Height = @Height)
				AND (((@Width IS NULL) AND (Width IS NULL)) OR Width = @Width)
				AND (((@Align IS NULL) AND (Align IS NULL)) OR Align = @Align);
		END
	END
	
	IF (@ResId IS NULL)
		SET @ResId = @ResourceId;

	SELECT ResourceId, ParentResourceId, LocalObjectType, LocalObjectId, Content, ContentType, [Name], Width, Height, Align, Temporary, CreatedTime
	FROM dbo.Mc_Resource
	WHERE (ResourceId = @ResId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetResources]
(
	@LocalObjectType nvarchar(50),
	@LocalObjectId nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT ResourceId, ParentResourceId, LocalObjectType, LocalObjectId, Content, ContentType, [Name], Width, Height, Align, Temporary, CreatedTime
	FROM dbo.Mc_Resource
	WHERE (LocalObjectType = @LocalObjectType) AND (LocalObjectId = @LocalObjectId)
	ORDER BY CreatedTime, Name;
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetUnitsOfMeasure]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [UnitsOfMeasureId]
      ,[OrganizationId]
      ,[SingularName]
      ,[SingularAbbrv]
      ,[PluralName]
      ,[PluralAbbrv]
      ,[GroupName]
      ,[LocalName]
	FROM [dbo].[Mc_UnitsOfMeasure]
	ORDER BY [OrganizationId] asc, [GroupName] asc, [SingularName] asc;
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetUnitsOfMeasureConversion]
(
	@UnitOfMeasureFrom uniqueidentifier=null,
	@UnitOfMeasureTo uniqueidentifier=null
)
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT [UnitOfMeasureFrom]
	  ,[UnitOfMeasureTo]
	  ,[OrganizationId]
	  ,[Factor]
	FROM [dbo].[Mc_UnitsOfMeasureConversion]
	WHERE (@UnitOfMeasureFrom IS NULL OR UnitOfMeasureFrom = @UnitOfMeasureFrom)
	AND (@UnitOfMeasureTo IS NULL OR UnitOfMeasureTo = @UnitOfMeasureTo);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetViewState]
(
    @ViewStateId uniqueidentifier
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ViewState
    FROM dbo.Mc_ViewState WITH (READUNCOMMITTED)
    WHERE (ViewStateId = @ViewStateId);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_GetWebsites]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT WebsiteId, [Name], Url, [Description], AdminContactInfo, Deleted 
	FROM dbo.Mc_Website 
	WHERE (Deleted = 0);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_InsertViewState]
(
	@ViewStateId uniqueidentifier,
    @ViewState varbinary(max),
    @ExpirationTime datetime
)
AS
BEGIN
    SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_ViewState (ViewStateId, ViewState, ExpirationTime) 
	VALUES (@ViewStateId, @ViewState, @ExpirationTime);
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
	EXEC('ALTER PROCEDURE [dbo].[Mc_UpdateLoginSession]
(
	@LoginId uniqueidentifier,
	@SessionId varchar(50)
)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE dbo.Mc_Login
	SET SessionId = @SessionId
	WHERE LoginId = @LoginId;
END
')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
