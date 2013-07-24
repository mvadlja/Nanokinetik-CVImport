-- Delete
CREATE PROCEDURE  [dbo].[proc_COUNTRY_Delete]
	@country_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[COUNTRY] WHERE [country_PK] = @country_PK
END
