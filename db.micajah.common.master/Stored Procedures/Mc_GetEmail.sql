
CREATE PROCEDURE [dbo].[Mc_GetEmail]
(
	@Email [nvarchar](255)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Email, LoginId
	FROM dbo.Mc_Email
	WHERE (Email = @Email)
END
