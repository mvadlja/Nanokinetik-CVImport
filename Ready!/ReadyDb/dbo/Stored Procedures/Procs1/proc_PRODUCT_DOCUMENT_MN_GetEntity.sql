-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PRODUCT_DOCUMENT_MN_GetEntity]
	@product_document_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_document_mn_PK], [product_FK], [document_FK]
	FROM [dbo].[PRODUCT_DOCUMENT_MN]
	WHERE [product_document_mn_PK] = @product_document_mn_PK
END
