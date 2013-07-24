-- Delete
CREATE PROCEDURE  [dbo].[proc_LANGUAGE_CODE_Delete]
	@languagecode_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[LANGUAGE_CODE] WHERE [languagecode_PK] = @languagecode_PK
END
