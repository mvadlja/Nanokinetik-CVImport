-- GetRolesByUserID
CREATE PROCEDURE  [dbo].[proc_LANGUAGE_CODE_GetLanguageCodeByDocument]
	@document_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[LANGUAGE_CODE].[languagecode_PK], [dbo].[LANGUAGE_CODE].[name], [dbo].[LANGUAGE_CODE].[code]
	FROM [dbo].[LANGUAGE_CODE]
	LEFT JOIN [dbo].[DOCUMENT_LANGUAGE_MN] ON LANGUAGE_CODE.languagecode_PK = DOCUMENT_LANGUAGE_MN.language_FK 
	WHERE [dbo].[DOCUMENT_LANGUAGE_MN].[document_FK] = @document_PK

END
