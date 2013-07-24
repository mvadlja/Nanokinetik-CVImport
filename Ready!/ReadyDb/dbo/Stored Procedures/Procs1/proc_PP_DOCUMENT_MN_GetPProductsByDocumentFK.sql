-- GetAuthorizedProductsByDocumentFK
CREATE PROCEDURE  [dbo].[proc_PP_DOCUMENT_MN_GetPProductsByDocumentFK]
	@document_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[pP_DOCUMENT_MN].pp_document_PK, [dbo].[pP_DOCUMENT_MN].doc_FK, [dbo].[pP_DOCUMENT_MN].pp_FK
	FROM [dbo].[pP_DOCUMENT_MN]
	WHERE ([dbo].[pP_DOCUMENT_MN].doc_FK = @document_FK OR @document_FK IS NULL)

END
