-- GetEntity
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_ALIAS_TRANSLATION_GetEntity]
	@substance_alias_translation_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[substance_alias_translation_PK], [languagecode], [term]
	FROM [dbo].[SUBSTANCE_ALIAS_TRANSLATION]
	WHERE [substance_alias_translation_PK] = @substance_alias_translation_PK
END
