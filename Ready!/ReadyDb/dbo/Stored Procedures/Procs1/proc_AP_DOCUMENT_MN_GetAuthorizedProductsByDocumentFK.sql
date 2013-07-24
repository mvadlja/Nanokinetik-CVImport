-- GetAuthorizedProductsByDocumentFK
CREATE PROCEDURE  [dbo].[proc_AP_DOCUMENT_MN_GetAuthorizedProductsByDocumentFK]
	@document_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[AP_DOCUMENT_MN].ap_document_mn_PK, [dbo].[AP_DOCUMENT_MN].document_FK, [dbo].[AP_DOCUMENT_MN].ap_FK
	FROM [dbo].[AP_DOCUMENT_MN]
	WHERE ([dbo].[AP_DOCUMENT_MN].document_FK = @document_FK OR @document_FK IS NULL)

END
