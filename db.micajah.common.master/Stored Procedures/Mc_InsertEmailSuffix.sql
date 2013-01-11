CREATE PROCEDURE [dbo].[Mc_InsertEmailSuffix]
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
END