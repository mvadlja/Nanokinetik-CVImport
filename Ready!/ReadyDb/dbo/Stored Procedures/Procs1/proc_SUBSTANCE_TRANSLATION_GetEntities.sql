-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_TRANSLATION_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[substance_translations_PK], [languagecode], [term]
	FROM [dbo].[SUBSTANCE_TRANSLATION]
END
