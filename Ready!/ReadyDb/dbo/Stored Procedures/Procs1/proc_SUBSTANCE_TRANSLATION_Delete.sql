-- Delete
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_TRANSLATION_Delete]
	@substance_translations_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SUBSTANCE_TRANSLATION] WHERE [substance_translations_PK] = @substance_translations_PK
END
