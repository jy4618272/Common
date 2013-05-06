CREATE PROCEDURE [dbo].[Mc_InsertNonce]
(
	@Context nvarchar(100),
	@Code nvarchar(50),
	@CreatedTime datetime
)
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO dbo.Mc_Nonce(Context, Code, CreatedTime)
	VALUES (@Context, @Code, @CreatedTime);

	SELECT Context, Code, CreatedTime
	FROM dbo.Mc_Nonce
	WHERE (Context = @Context) AND (Code = @Code) AND (CreatedTime = @CreatedTime);
END