-- GetEntity
CREATE PROCEDURE  [dbo].[proc_DOCUMENT_LANGUAGE_MN_GetEntity]
	@document_language_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[document_language_mn_PK], [document_FK], [language_FK]
	FROM [dbo].[DOCUMENT_LANGUAGE_MN]
	WHERE [document_language_mn_PK] = @document_language_mn_PK
END
