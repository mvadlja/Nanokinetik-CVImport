-- Delete
CREATE PROCEDURE  [dbo].[proc_INTERNATIONAL_CODE_Delete]
	@international_code_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[INTERNATIONAL_CODE] WHERE [international_code_PK] = @international_code_PK
END
