
-- Delete
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_CODE_Delete]
	@substance_code_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SUBSTANCE_CODE] WHERE [substance_code_PK] = @substance_code_PK
END
