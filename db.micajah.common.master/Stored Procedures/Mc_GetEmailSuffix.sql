CREATE PROCEDURE [dbo].[Mc_GetEmailSuffix]
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
