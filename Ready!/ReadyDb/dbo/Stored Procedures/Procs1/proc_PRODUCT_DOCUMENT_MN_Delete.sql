-- Delete
CREATE PROCEDURE  [dbo].[proc_PRODUCT_DOCUMENT_MN_Delete]
	@product_document_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PRODUCT_DOCUMENT_MN] WHERE [product_document_mn_PK] = @product_document_mn_PK
END
