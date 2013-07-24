-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_ALIAS_TRANSLATION_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[substance_alias_translation_PK], [languagecode], [term]
	FROM [dbo].[SUBSTANCE_ALIAS_TRANSLATION]
END
