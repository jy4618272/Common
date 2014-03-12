BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

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
IF @LoginId IS NULL 
 SELECT DISTINCT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.ProfileUpdated, l.SessionId, l.Deleted      
  , CASE WHEN o.OrganizationId IS NULL THEN 0 ELSE 1 END AS OrganizationExists      
 FROM dbo.Mc_Login AS l
 LEFT JOIN dbo.Mc_Email AS e ON l.LoginId = e.LoginId
 LEFT JOIN dbo.Mc_OrganizationsLogins ol ON ol.LoginId=l.LoginId 
 INNER JOIN dbo.Mc_Organization o ON o.OrganizationId = ol.OrganizationId
 WHERE (l.LoginName=@LoginName OR e.Email=@LoginName) AND (@Password IS NULL OR l.[Password]=@Password) AND o.Deleted=0
ELSE
 SELECT DISTINCT l.LoginId, l.FirstName, l.LastName, l.LoginName, l.[Password], l.ProfileUpdated, l.SessionId, l.Deleted      
  , CASE WHEN o.OrganizationId IS NULL THEN 0 ELSE 1 END AS OrganizationExists      
 FROM dbo.Mc_Login AS l
 LEFT JOIN dbo.Mc_OrganizationsLogins ol ON ol.LoginId=l.LoginId 
 INNER JOIN dbo.Mc_Organization o ON o.OrganizationId = ol.OrganizationId
 WHERE l.LoginId=@LoginId AND (@Password IS NULL OR l.[Password]=@Password) AND o.Deleted=0
END')

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION