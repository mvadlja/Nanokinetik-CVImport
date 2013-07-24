-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PP_DOCUMENT_MN_GetEntity]
	@pp_document_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[pp_document_PK], [pp_FK], [doc_FK]
	FROM [dbo].[PP_DOCUMENT_MN]
	WHERE [pp_document_PK] = @pp_document_PK
END
