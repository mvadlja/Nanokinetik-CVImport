-- GetUsersInRolesByUserID
CREATE PROCEDURE  [dbo].[proc_DOCUMENT_LANGUAGE_MN_GetLanguagesByDocument]
	@document_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT [dbo].[DOCUMENT_LANGUAGE_MN].[document_language_mn_PK], [dbo].[DOCUMENT_LANGUAGE_MN].[document_FK], [dbo].[DOCUMENT_LANGUAGE_MN].[language_FK]
	FROM [dbo].[DOCUMENT_LANGUAGE_MN]
	WHERE [dbo].[DOCUMENT_LANGUAGE_MN].[document_FK] = @document_PK

END
