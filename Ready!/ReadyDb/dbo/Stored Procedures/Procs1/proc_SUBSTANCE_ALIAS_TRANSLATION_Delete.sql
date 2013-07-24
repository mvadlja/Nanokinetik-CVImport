-- Delete
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_ALIAS_TRANSLATION_Delete]
	@substance_alias_translation_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SUBSTANCE_ALIAS_TRANSLATION] WHERE [substance_alias_translation_PK] = @substance_alias_translation_PK
END
