-- GetEntity
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_TRANSLATION_GetEntity]
	@substance_translations_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[substance_translations_PK], [languagecode], [term]
	FROM [dbo].[SUBSTANCE_TRANSLATION]
	WHERE [substance_translations_PK] = @substance_translations_PK
END
