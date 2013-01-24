CREATE PROCEDURE [dbo].[Mc_UpdateEmailSuffix]
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
END