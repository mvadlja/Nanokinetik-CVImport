CREATE PROCEDURE  [dbo].[proc_PRODUCT_DOCUMENT_MN_GetProductsByDocumentFK]
	@document_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[PRODUCT_DOCUMENT_MN].product_document_mn_PK, [dbo].[PRODUCT_DOCUMENT_MN].document_FK, [dbo].[PRODUCT_DOCUMENT_MN].product_FK
	FROM [dbo].[PRODUCT_DOCUMENT_MN]
	WHERE ([dbo].[PRODUCT_DOCUMENT_MN].document_FK = @document_FK OR @document_FK IS NULL)

END
