BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF

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
END  
')

IF @@TRANCOUNT = 1
	UPDATE dbo.Mc_Organization 
	SET [Name] = [Name] + ' (' + CAST(OrganizationId AS nvarchar(50)) + ')'
	WHERE [Deleted] = 1 AND [Name] IN (SELECT [Name] FROM dbo.Mc_Organization GROUP BY [Name] HAVING COUNT(*) > 1)

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION

IF @@TRANCOUNT = 1
   COMMIT TRANSACTION
